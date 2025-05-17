using BaoXia.Utils.Extensions;
using BaoXia.Utils.Notification;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Test.NotificationTest;

[TestClass]
public class NotificationCenterTest
{

	////////////////////////////////////////////////
	// @静态常量
	////////////////////////////////////////////////

	#region 静态常量

	public class NotificationQueueNames
	{
		public const string TestQueue_1 = "TestQueue_1";
	}

	public class NotificationNames
	{
		public const string ParamUpdated = "参数更新";

		public const string StopLoop = "停止循环";
	}

	public class NotificationParamObject
	{
		public string? Name { get; set; }

		public bool IsListened { get; set; }
	}

	#endregion

	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	[TestMethod]
	public void PostAndListenNotificationTest()
	{
		NotificationCenter notificationCenter = new();

		var testName = "PostAndListenNotificationTest";
		var testDictionaryParam_Key = "测试关键字。";
		var testDictionaryParam_Value = "测试值：" + StringUtil.StringByFillRandomCharsToLength(8) + "。";
		var testObjectParam = new Object();

		String? receivedDictionaryParam_Value = null;
		Object? receivedObjectParam = null;

		var listenersCount = 10;
		var listenerIsListenings = new bool[listenersCount];

		////////////////////////////////////////////////
		// 1/4，开启监听任务：
		////////////////////////////////////////////////
		var listenerTasks = new List<Task>();
		for (int listenerIndex = 0;
			listenerIndex < listenersCount;
			listenerIndex++)
		{
			var taskId = listenerIndex + 1;
			listenerTasks.Add(Task.Run(() =>
			{
				var isLoopNeedContinue = true;
				notificationCenter.ListenNotification(
					NotificationNames.StopLoop,
					null,
					null,
					(notification, cancellationToken) =>
					{
						System.Diagnostics.Trace.WriteLine(testName + "，任务(" + taskId + ")，收到消息：" + notification.Name + "。");

						isLoopNeedContinue = false;

						if (notification.ParamDictionary?.TryGetValue(
							testDictionaryParam_Key,
							out var objectValueForTestDictionaryParam_Key)
						== true)
						{
							// !!!
							receivedDictionaryParam_Value = objectValueForTestDictionaryParam_Key as string;
							// !!!
						}
						else
						{
							Assert.Fail();
						}

						receivedObjectParam = notification.ParamObject;

						return null;
					});


				////////////////////////////////////////////////
				// !!!
				listenerIsListenings[taskId - 1] = true;
				// !!!
				////////////////////////////////////////////////


				var loopNumber = 0;
				while (isLoopNeedContinue)
				{
					loopNumber++;
					{
						System.Diagnostics.Trace.WriteLine(testName + "，任务(" + taskId + ")，等待消息，第 " + loopNumber + " 次循环。");
					}
					System.Threading.Thread.Sleep(100);
				}
			}));
		}

		////////////////////////////////////////////////
		// 2/4，等所有监听任务开始运行后，再发送测试消息：
		////////////////////////////////////////////////
		while (listenerTasks.IsAllTaskStarted() != true)
		{
			var isAllListenersListening = true;
			foreach (var listenerIsListening in listenerIsListenings)
			{
				if (!listenerIsListening)
				{
					isAllListenersListening = false;
					break;
				}
			}
			if (isAllListenersListening)
			{
				break;
			}
			System.Threading.Thread.Sleep(100);
		}

		////////////////////////////////////////////////
		// 3/4，发送测试消息：
		////////////////////////////////////////////////
		System.Diagnostics.Trace.WriteLine(testName + "，发送消息开始：停止等待。");
		notificationCenter.PostNotification(
			NotificationNames.StopLoop,
			null,
			null,
			new Dictionary<string, object>
			{
				{ testDictionaryParam_Key, testDictionaryParam_Value }
			},
			testObjectParam);
		System.Diagnostics.Trace.WriteLine(testName + "，发送消息结束：停止等待。");

		////////////////////////////////////////////////
		// 4/4，等待监听任务结束，判断测试结果：
		////////////////////////////////////////////////
		// !!!
		Task.WaitAll([.. listenerTasks]);
		// !!!
		Assert.AreEqual(testDictionaryParam_Value, receivedDictionaryParam_Value);
		Assert.AreEqual(testObjectParam, receivedObjectParam);
		// !!!
	}

	[TestMethod]
	public void PostAndListenNotificationTestWithHighConcurrentTest()
	{
		NotificationCenter notificationCenter = new();

		var testName = "PostAndListenNotificationTestWithHighConcurrentTest";
		var testDictionaryParam_Key = "测试关键字。";
		var testDictionaryParam_Value = "测试值：" + StringUtil.StringByFillRandomCharsToLength(8) + "。";

		double testDurationSeconds = 1.0;
		int testSendNotificationIntervalMiiliseconds = 50;

		int postersCount = 2;
		int listenersCount = 2;
		var listenerIsListenings = new bool[listenersCount];

		int testTotalNotificationsCountSended = 0;
		int testTotalNotificationsCountReceived = 0;


		////////////////////////////////////////////////
		// 1/4，开启监听任务：
		////////////////////////////////////////////////

		// 【并发接受消息】：
		var listenerTasks = new List<Task>();
		for (var listenerIndex = 0;
		listenerIndex < listenersCount;
		listenerIndex++)
		{
			var taskId = listenerIndex + 1;
			listenerTasks.Add(Task.Run(() =>
			{
				var isLoopNeedContinue = true;
				var isStopped = false;

				notificationCenter.ListenNotification(
					NotificationQueueNames.TestQueue_1,
					//
					NotificationNames.ParamUpdated,
					null,
					null,
					(notification, cancellationToken) =>
					{
						// !!!
						Interlocked.Increment(ref testTotalNotificationsCountReceived);
						// !!!

						// !!!
						Assert.IsTrue(notification.Name.Equals(
							NotificationNames.ParamUpdated));
						Assert.IsFalse(isStopped);
						// !!!

						NotificationParamObject? paramObject
						= notification.ParamObject as NotificationParamObject;
						Assert.IsNotNull(paramObject);
						//Assert.IsTrue(paramObject.IsListened != true);
						{
							paramObject.IsListened = true;
						}

						return null;
					});

				notificationCenter.ListenNotification(
					NotificationQueueNames.TestQueue_1,
					//
					NotificationNames.StopLoop,
					null,
					null,
					(notification, cancellationToken) =>
					{
						System.Diagnostics.Trace.WriteLine(testName + "，任务(" + taskId + ")，收到消息：" + notification.Name + "。");

						// !!!
						Assert.IsTrue(notification.Name.Equals(
							NotificationNames.StopLoop));
						isStopped = true;
						// !!!

						if (notification.ParamDictionary?.TryGetValue(
							testDictionaryParam_Key,
							out var objectValueForTestDictionaryParam_Key)
							== true)
						{
							// !!!
							Assert.IsTrue(objectValueForTestDictionaryParam_Key?.Equals(testDictionaryParam_Value));
							// !!!
						}
						else
						{
							Assert.Fail();
						}

						// !!!
						isLoopNeedContinue = false;
						// !!!

						return null;
					});


				////////////////////////////////////////////////
				// !!!
				listenerIsListenings[taskId - 1] = true;
				// !!!
				////////////////////////////////////////////////


				var loopNumber = 0;
				while (isLoopNeedContinue)
				{
					loopNumber++;
					{
						// System.Diagnostics.Trace.WriteLine(testName + "，任务(" + taskId + ")，等待消息，第 " + loopNumber + " 次循环。");
					}
					System.Threading.Thread.Sleep(100);
				}
			}));
		}


		////////////////////////////////////////////////
		// 2/4，等所有监听任务开始运行后，再发送测试消息：
		////////////////////////////////////////////////
		while (listenerTasks.IsAllTaskStarted() != true)
		{
			var isAllListenersListening = true;
			foreach (var listenerIsListening in listenerIsListenings)
			{
				if (!listenerIsListening)
				{
					isAllListenersListening = false;
					break;
				}
			}
			if (isAllListenersListening)
			{
				break;
			}
			System.Threading.Thread.Sleep(100);
		}


		////////////////////////////////////////////////
		// 3/4，发送测试消息：
		////////////////////////////////////////////////

		// 【并发发送消息】：
		var senderTasks = new List<Task>();
		var testNotificationParamObjects = new List<NotificationParamObject>();
		for (var posterIndex = 0;
			posterIndex < postersCount;
			posterIndex++)
		{
			senderTasks.Add(Task.Run(() =>
			{
				var testEndTime = DateTime.Now.AddSeconds(testDurationSeconds);
				while (DateTime.Now <= testEndTime)
				{
					var testNotificationParamObject = new NotificationParamObject();
					lock (testNotificationParamObjects)
					{
						testNotificationParamObjects.Add(testNotificationParamObject);
					}
					notificationCenter.PostNotification(
						NotificationQueueNames.TestQueue_1,
						//
						NotificationNames.ParamUpdated,
						null,
						null,
						null,
						testNotificationParamObject);
					// !!!
					Interlocked.Increment(ref testTotalNotificationsCountSended);
					// !!!
					System.Threading.Thread.Sleep(testSendNotificationIntervalMiiliseconds);
				}
			}));
		}
		// !!!
		Task.WaitAll([.. senderTasks]);
		// !!!
		{
			System.Diagnostics.Trace.WriteLine(testName + "，发送消息开始：停止等待。");

			notificationCenter.PostNotification(
				NotificationQueueNames.TestQueue_1,
				//
				NotificationNames.StopLoop,
				null,
				null,
				new Dictionary<string, object>
				{
					{ testDictionaryParam_Key, testDictionaryParam_Value }
				},
				null);

			System.Diagnostics.Trace.WriteLine(testName + "，发送消息结束：停止等待。");
		}

		////////////////////////////////////////////////
		// 4/4，等待监听任务结束，判断测试结果：
		////////////////////////////////////////////////
		// !!!
		Task.WaitAll([.. listenerTasks]);
		// !!!

		// 发送的消息，都接收到了。
		Assert.AreEqual(testTotalNotificationsCountReceived, testTotalNotificationsCountSended * listenersCount);
		// 所有发送的消息参数，都被接收者标记了。
		foreach (var testNotificationParamObject in testNotificationParamObjects)
		{
			Assert.IsTrue(testNotificationParamObject.IsListened);
		}
		// !!!
	}

	[TestMethod]
	public void PostAsyncAndListenNotificationAsyncTest()
	{
		NotificationCenter notificationCenter = new();

		var testName = "PostAsyncAndListenNotificationAsyncTest";
		var testDictionaryParam_Key = "测试关键字。";
		var testDictionaryParam_Value = "测试值：" + StringUtil.StringByFillRandomCharsToLength(8) + "。";
		var testObjectParam = new Object();

		String? receivedDictionaryParam_Value = null;
		Object? receivedObjectParam = null;

		var listenersCount = 10;
		var listenerIsListenings = new bool[listenersCount];

		////////////////////////////////////////////////
		// 1/4，开启监听任务：
		////////////////////////////////////////////////
		var listenerTasks = new List<Task>();
		for (int listenerIndex = 0;
			listenerIndex < listenersCount;
			listenerIndex++)
		{
			var taskId = listenerIndex + 1;
			listenerTasks.Add(Task.Run(() =>
			{
				var isLoopNeedContinue = true;
				notificationCenter.ListenNotificationAsync(
					NotificationNames.StopLoop,
					null,
					null,
					async (notification, cancellationToken) =>
					{
						System.Diagnostics.Trace.WriteLine(testName + "，任务(" + taskId + ")，收到消息：" + notification.Name + "。");

						isLoopNeedContinue = false;

						if (notification.ParamDictionary?.TryGetValue(
							testDictionaryParam_Key,
							out var objectValueForTestDictionaryParam_Key)
							== true)
						{
							// !!!
							receivedDictionaryParam_Value = objectValueForTestDictionaryParam_Key as string;
							// !!!
						}
						else
						{
							Assert.Fail();
						}

						receivedObjectParam = notification.ParamObject;

						await Task.Delay(10, cancellationToken);

						return null;
					});


				////////////////////////////////////////////////
				// !!!
				listenerIsListenings[taskId - 1] = true;
				// !!!
				////////////////////////////////////////////////


				var loopNumber = 0;
				while (isLoopNeedContinue)
				{
					loopNumber++;
					{
						System.Diagnostics.Trace.WriteLine(testName + "，任务(" + taskId + ")，等待消息，第 " + loopNumber + " 次循环。");
					}
					System.Threading.Thread.Sleep(100);
				}
			}));
		}

		////////////////////////////////////////////////
		// 2/4，等所有监听任务开始运行后，再发送测试消息：
		////////////////////////////////////////////////
		while (listenerTasks.IsAllTaskStarted() != true)
		{
			var isAllListenersListening = true;
			foreach (var listenerIsListening in listenerIsListenings)
			{
				if (!listenerIsListening)
				{
					isAllListenersListening = false;
					break;
				}
			}
			if (isAllListenersListening)
			{
				break;
			}
			System.Threading.Thread.Sleep(100);
		}


		////////////////////////////////////////////////
		// 3/4，发送测试消息：
		////////////////////////////////////////////////
		System.Diagnostics.Trace.WriteLine(testName + "，发送消息开始：停止等待。");
		notificationCenter.PostNotification(
			NotificationNames.StopLoop,
			null,
			null,
			new Dictionary<string, object>
			{
				{ testDictionaryParam_Key, testDictionaryParam_Value }
			},
			testObjectParam);
		System.Diagnostics.Trace.WriteLine(testName + "，发送消息结束：停止等待。");

		////////////////////////////////////////////////
		// 4/4，等待监听任务结束，判断测试结果：
		////////////////////////////////////////////////
		// !!!
		Task.WaitAll([.. listenerTasks]);
		// !!!
		Assert.AreEqual(testDictionaryParam_Value, receivedDictionaryParam_Value);
		Assert.AreEqual(testObjectParam, receivedObjectParam);
		// !!!
	}

	[TestMethod]
	public void PostAndListenNotificationTestWithHighConcurrentAsyncTest()
	{
		NotificationCenter notificationCenter = new();

		var testName = "PostAndListenNotificationTestWithHighConcurrentAsyncTest";
		var testDictionaryParam_Key = "测试关键字。";
		var testDictionaryParam_Value = "测试值：" + StringUtil.StringByFillRandomCharsToLength(8) + "。";

		double testDurationSeconds = 1.0;
		int testSendNotificationIntervalMiiliseconds = 50;

		int postersCount = 2;
		int listenersCount = 2;
		var listenerIsListenings = new bool[listenersCount];

		int testTotalNotificationsCountSended = 0;
		int testTotalNotificationsCountReceived = 0;


		////////////////////////////////////////////////
		// 1/4，开启监听任务：
		////////////////////////////////////////////////
		// 【并发接受消息】：
		var listenerTasks = new List<Task>();
		for (var listenerIndex = 0;
		listenerIndex < listenersCount;
		listenerIndex++)
		{
			var taskId = listenerIndex + 1;
			listenerTasks.Add(Task.Run(() =>
			{
				var isLoopNeedContinue = true;
				var isStopped = false;

				notificationCenter.ListenNotificationAsync(
					NotificationQueueNames.TestQueue_1,
					//
					NotificationNames.ParamUpdated,
					null,
					null,
					async (notification, cancellationToken) =>
					{
						// !!!
						Interlocked.Increment(ref testTotalNotificationsCountReceived);
						// !!!

						// !!!
						Assert.IsTrue(notification.Name.Equals(
							NotificationNames.ParamUpdated));
						Assert.IsFalse(isStopped);
						// !!!

						var paramObject
						= notification.ParamObject as NotificationParamObject;
						Assert.IsNotNull(paramObject);
						//Assert.IsTrue(paramObject.IsListened != true);
						{
							paramObject.IsListened = true;
						}

						await Task.Delay(1, cancellationToken);

						return null;
					});

				notificationCenter.ListenNotificationAsync(
					NotificationQueueNames.TestQueue_1,
					//
					NotificationNames.StopLoop,
					null,
					null,
					async (notification, cancellationToken) =>
					{
						System.Diagnostics.Trace.WriteLine(testName + "，任务(" + taskId + ")，收到消息：" + notification.Name + "。");

						// !!!
						Assert.IsTrue(notification.Name.Equals(
							NotificationNames.StopLoop));
						isStopped = true;
						// !!!

						if (notification.ParamDictionary?.TryGetValue(
							testDictionaryParam_Key,
							out var objectValueForTestDictionaryParam_Key)
							== true)
						{
							// !!!
							Assert.IsTrue(objectValueForTestDictionaryParam_Key?.Equals(testDictionaryParam_Value));
							// !!!
						}
						else
						{
							// !!!
							Assert.Fail();
							// !!!
						}

						// !!!
						isLoopNeedContinue = false;
						// !!!

						await Task.Delay(1, cancellationToken);

						return null;
					});


				////////////////////////////////////////////////
				// !!!
				listenerIsListenings[taskId - 1] = true;
				// !!!
				////////////////////////////////////////////////


				var loopNumber = 0;
				while (isLoopNeedContinue)
				{
					loopNumber++;
					{
						// System.Diagnostics.Trace.WriteLine(testName + "，" + taskId + "." + loopNumber + "，等待消息。");
					}
					System.Threading.Thread.Sleep(100);
				}
			}));
		}

		////////////////////////////////////////////////
		// 2/4，等所有监听任务开始运行后，再发送测试消息：
		////////////////////////////////////////////////
		while (listenerTasks.IsAllTaskStarted() != true)
		{
			var isAllListenersListening = true;
			foreach (var listenerIsListening in listenerIsListenings)
			{
				if (!listenerIsListening)
				{
					isAllListenersListening = false;
					break;
				}
			}
			if (isAllListenersListening)
			{
				break;
			}
			System.Threading.Thread.Sleep(100);
		}


		////////////////////////////////////////////////
		// 3/4，发送测试消息：
		////////////////////////////////////////////////
		// 【并发发送消息】：
		var senderTasks = new List<Task>();
		var testNotificationParamObjects = new List<NotificationParamObject>();
		for (var posterIndex = 0;
			posterIndex < postersCount;
			posterIndex++)
		{
			senderTasks.Add(Task.Run(() =>
			{
				var testEndTime = DateTime.Now.AddSeconds(testDurationSeconds);
				while (DateTime.Now <= testEndTime)
				{
					var testNotificationParamObject = new NotificationParamObject();
					lock (testNotificationParamObjects)
					{
						testNotificationParamObjects.Add(testNotificationParamObject);
					}
					notificationCenter.PostNotification(
						NotificationQueueNames.TestQueue_1,
						//
						NotificationNames.ParamUpdated,
						null,
						null,
						null,
						testNotificationParamObject);
					// !!!
					Interlocked.Increment(ref testTotalNotificationsCountSended);
					// !!!
					System.Threading.Thread.Sleep(testSendNotificationIntervalMiiliseconds);
				}
			}));
		}
		// !!!
		Task.WaitAll([.. senderTasks]);
		// !!!
		{
			System.Diagnostics.Trace.WriteLine(testName + "，发送消息开始：停止等待。");

			notificationCenter.PostNotification(
				NotificationQueueNames.TestQueue_1,
				//
				NotificationNames.StopLoop,
				null,
				null,
				new Dictionary<string, object>
				{
					{ testDictionaryParam_Key, testDictionaryParam_Value }
				},
				null);

			System.Diagnostics.Trace.WriteLine(testName + "，发送消息结束：停止等待。");
		}


		////////////////////////////////////////////////
		// 4/4，等待监听任务结束，判断测试结果：
		////////////////////////////////////////////////
		// !!!
		Task.WaitAll([.. listenerTasks]);
		// !!!

		// 发送的消息，都接收到了。
		Assert.AreEqual(testTotalNotificationsCountReceived, testTotalNotificationsCountSended * listenersCount);
		// 所有发送的消息参数，都被接收者标记了。
		foreach (var testNotificationParamObject in testNotificationParamObjects)
		{
			Assert.IsTrue(testNotificationParamObject.IsListened);
		}
		// !!!
	}

	[TestMethod]
	public void PostAndListenNotificationWithTagsTest()
	{
		NotificationCenter notificationCenter = new();

		var testName = "PostAndListenNotificationWithTagsTest";
		var testTagName = "测试标签";
		var testDictionaryParam_Key = "测试关键字。";
		var testDictionaryParam_Value = "测试值：" + StringUtil.StringByFillRandomCharsToLength(8) + "。";
		var testObjectParam = new Object();

		String? receivedDictionaryParam_Value = null;
		Object? receivedObjectParam = null;

		var listenersCount = 10;
		var listenerIsListenings = new bool[listenersCount];

		////////////////////////////////////////////////
		// 1/4，开启监听任务：
		////////////////////////////////////////////////
		var listenerTasks = new List<Task>();
		for (int listenerIndex = 0;
			listenerIndex < listenersCount;
			listenerIndex++)
		{
			var taskId = listenerIndex + 1;
			listenerTasks.Add(Task.Run(() =>
			{
				var isLoopNeedContinue = true;
				notificationCenter.ListenNotification(
					NotificationNames.StopLoop,
					[testTagName],
					null,
					(notification, cancellationToken) =>
					{
						System.Diagnostics.Trace.WriteLine(testName + "，任务(" + taskId + ")，收到消息：" + notification.Name + "。");

						isLoopNeedContinue = false;

						if (notification.ParamDictionary?.TryGetValue(
							testDictionaryParam_Key,
							out var objectValueForTestDictionaryParam_Key)
						== true
						&& objectValueForTestDictionaryParam_Key != null)
						{
							// !!!
							receivedDictionaryParam_Value = objectValueForTestDictionaryParam_Key as string;
							// !!!
						}
						receivedObjectParam = notification.ParamObject;

						return null;
					});


				////////////////////////////////////////////////
				// !!!
				listenerIsListenings[taskId - 1] = true;
				// !!!
				////////////////////////////////////////////////


				var loopNumber = 0;
				while (isLoopNeedContinue)
				{
					loopNumber++;
					{
						System.Diagnostics.Trace.WriteLine(testName + "，任务(" + taskId + ")，等待消息，第 " + loopNumber + " 次循环。");
					}
					System.Threading.Thread.Sleep(100);
				}
			}));
		}

		////////////////////////////////////////////////
		// 2/4，等所有监听任务开始运行后，再发送测试消息：
		////////////////////////////////////////////////
		while (listenerTasks.IsAllTaskStarted() != true)
		{
			var isAllListenersListening = true;
			foreach (var listenerIsListening in listenerIsListenings)
			{
				if (!listenerIsListening)
				{
					isAllListenersListening = false;
					break;
				}
			}
			if (isAllListenersListening)
			{
				break;
			}
			System.Threading.Thread.Sleep(100);
		}

		////////////////////////////////////////////////
		// 3/4，发送测试消息：
		////////////////////////////////////////////////
		System.Diagnostics.Trace.WriteLine(testName + "，发送消息开始：停止等待。");
		notificationCenter.PostNotification(
			NotificationNames.StopLoop,
			[testTagName],
			null,
			new Dictionary<string, object>
			{
				{ testDictionaryParam_Key, testDictionaryParam_Value }
			},
			testObjectParam);
		System.Diagnostics.Trace.WriteLine(testName + "，发送消息结束：停止等待。");

		////////////////////////////////////////////////
		// 4/4，等待监听任务结束，判断测试结果：
		////////////////////////////////////////////////
		// !!!
		Task.WaitAll([.. listenerTasks]);
		// !!!
		Assert.AreEqual(testDictionaryParam_Value, receivedDictionaryParam_Value);
		Assert.AreEqual(testObjectParam, receivedObjectParam);
		// !!!
	}

	#endregion
}
