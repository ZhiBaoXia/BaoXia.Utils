using BaoXia.Utils.ConcurrentTools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Notification
{
	public class NotificationQueue : ConcurrentQueue<Notification>
	{

		////////////////////////////////////////////////
		// @静态常量
		////////////////////////////////////////////////.

		#region 静态常量

		#endregion



		////////////////////////////////////////////////
		// @自身属性
		////////////////////////////////////////////////

		#region 自身属性

		public string? Name { get; set; }

		public bool IsUnorderedQueue { get; set; }


		////////////////////////////////////////////////

		public Func<double>? ToGetPeekNotificationIntervalSecondsMax { get; set; }

		protected double _peekNotificationIntervalSecondsMax;

		public double PeekNotificationIntervalSecondsMax
		{
			get
			{
				var toGetPeekNotificationIntervalSecondsMax = this.ToGetPeekNotificationIntervalSecondsMax;
				if (toGetPeekNotificationIntervalSecondsMax != null)
				{
					return toGetPeekNotificationIntervalSecondsMax();
				}
				return _peekNotificationIntervalSecondsMax;
			}
			set
			{
				_peekNotificationIntervalSecondsMax = value;
			}
		}

		////////////////////////////////////////////////

		public Func<int>? ToGetSendNotificationConcurrentCountMax { get; set; }

		protected int _sendNotificationConcurrentCountMax;

		public int SendNotificationConcurrentCountMax
		{
			get
			{
				var toGetSendNotificationConcurrentCountMax = this.ToGetSendNotificationConcurrentCountMax;
				if (toGetSendNotificationConcurrentCountMax != null)
				{
					return toGetSendNotificationConcurrentCountMax();
				}
				return _sendNotificationConcurrentCountMax;
			}
			set
			{
				_sendNotificationConcurrentCountMax = value;
			}
		}

		////////////////////////////////////////////////

		public Func<double>? ToGetSendNotificationTimeoutSeconds { get; set; }

		protected double _notificationProcessTimeoutSeconds;

		public double NotificationProcessTimeoutSeconds
		{
			get
			{
				var toGeSendNotificationTimeoutSeconds = this.ToGetSendNotificationTimeoutSeconds;
				if (toGeSendNotificationTimeoutSeconds != null)
				{
					return toGeSendNotificationTimeoutSeconds();
				}
				return _notificationProcessTimeoutSeconds;
			}
			set
			{
				_notificationProcessTimeoutSeconds = value;
			}
		}

		////////////////////////////////////////////////

		public Action<NotificationQueue, Notification, Exception> ToReceiveExceptionThrewBySendNotification { get; set; }

		////////////////////////////////////////////////

		protected readonly LoopTask _taskToPeekNotification;

		protected readonly Tasks _tasksToSendNotification;

		protected readonly List<NotificationListener> _notificationListeners;

		#endregion


		////////////////////////////////////////////////
		// @自身实现
		////////////////////////////////////////////////

		#region 自身实现

		public NotificationQueue(
			// 基础信息：
			string? name,
			bool isUnorderedQueue,
			// 拾取消息的配置：
			Func<double>? toGetPeekNotificationIntervalSecondsMax,
			double peekNotificationIntervalSecondsMax,
			// 发送消息的配置：
			Func<int>? toGetSendNotificationConcurrentCountMax,
			int sendNotificationConcurrentCountMax,
			Func<double>? toGeSendNotificationTimeoutSeconds,
			double notificationProcessTimeoutSeconds,
			Action<NotificationQueue, Notification, Exception> toReceiveExceptionThrewBySendNotification)
		{
			// 基础信息：
			{
				this.Name = name;
				this.IsUnorderedQueue = isUnorderedQueue;
			}

			// 拾取消息的配置：
			{
				this.ToGetPeekNotificationIntervalSecondsMax = toGetPeekNotificationIntervalSecondsMax;
				this.PeekNotificationIntervalSecondsMax = peekNotificationIntervalSecondsMax;
			}

			// 发送消息的配置：
			{
				this.ToGetSendNotificationConcurrentCountMax = toGetSendNotificationConcurrentCountMax;
				if (toGetSendNotificationConcurrentCountMax == null
					&& sendNotificationConcurrentCountMax < 1)
				{
					sendNotificationConcurrentCountMax = 1;
				}
				this.SendNotificationConcurrentCountMax = sendNotificationConcurrentCountMax;


				this.ToGetSendNotificationTimeoutSeconds = toGeSendNotificationTimeoutSeconds;
				this.NotificationProcessTimeoutSeconds = notificationProcessTimeoutSeconds;

				this.ToReceiveExceptionThrewBySendNotification = toReceiveExceptionThrewBySendNotification;
			}


			////////////////////////////////////////////////
			// 内部对象初始化：
			////////////////////////////////////////////////

			_notificationListeners = new();
			_taskToPeekNotification = new(
						(cancellationToken) =>
						{
							// !!!
							var notificationsCountNeedProcessed
							= this.GetNotificationsCountNeedProcessedByTryToDequeueAndSendNotification(
								cancellationToken);
							// !!!
							if (notificationsCountNeedProcessed > 0)
							{
								return true;
							}
							return false;
						},
						() => this.PeekNotificationIntervalSecondsMax,
						false,
						false);
			_tasksToSendNotification = new(() => this.SendNotificationConcurrentCountMax);
		}

		protected int GetNotificationsCountNeedProcessedByTryToDequeueAndSendNotification(
			CancellationToken cancellationToken)
		{
			var notificationsCountNeedProcessed
				 = this.DidTryToPeekAndSendNotificationWithTasks(
					 _tasksToSendNotification,
					 _notificationListeners,
					 cancellationToken);
			{ }
			return notificationsCountNeedProcessed;
		}

		public void StartProcessNotificationQueue()
		{
			// !!!
			_taskToPeekNotification.Start();
			// !!!
		}

		public void CancelProcessNotificationQueue()
		{
			_taskToPeekNotification.Cancel();
			_tasksToSendNotification.Cancel();
		}

		public void CancelProcessNotificationQueueAndWaitAll()
		{
			_taskToPeekNotification.Cancel();
			_tasksToSendNotification.CancelAndWaitAll();
		}

		public async Task CancelProcessNotificationQueueAndWhenAllAsync()
		{
			var tasks = new List<Task>();
			{
				var taskToPeekNotification = _taskToPeekNotification.Cancel();
				if (taskToPeekNotification != null)
				{
					tasks.Add(taskToPeekNotification);
				}

				var taskToSendNotifiation = _tasksToSendNotification.CancelAndWhenAll();
				if (taskToSendNotifiation != null)
				{
					tasks.Add(taskToSendNotifiation);
				}
			}
			;
			if (tasks.Count > 0)
			{
				await Task.WhenAll(tasks);
			}
		}

		public NotificationListener RegisterListenerToListenNotificationName(
			string notificationName,
			//
			IEnumerable<string>? tagNamesWithIntersection,
			IEnumerable<string>? tagNamesWithUnion,
			//
			Func<Notification, CancellationToken, object?>? toDidReceivedNotification,
			Func<Notification, CancellationToken, Task<object?>>? toDidReceivedNotificationAsync)
		{
			var listener = new NotificationListener(
				this,
				//
				notificationName,
				//
				tagNamesWithIntersection,
				tagNamesWithUnion,
				//
				toDidReceivedNotification,
				toDidReceivedNotificationAsync);
			lock (_notificationListeners)
			{
				// !!!
				_notificationListeners.Add(listener);
				// !!!
			}
			return listener;
		}

		public bool UnregisterListener(NotificationListener listener)
		{
			if (listener == null)
			{
				return false;
			}
			var isRemoveSuccess = false;
			lock (_notificationListeners)
			{
				isRemoveSuccess = _notificationListeners.Remove(listener);
			}
			if (isRemoveSuccess)
			{
				listener.NotificationQueue = null;
			}
			return isRemoveSuccess;
		}


		#endregion


		////////////////////////////////////////////////
		// @事件节点
		////////////////////////////////////////////////

		#region 事件节点

		public int DidTryToPeekAndSendNotificationWithTasks(
			Tasks tasks,
			List<NotificationListener> allNotificationListeners,
			CancellationToken cancellationToken)
		{
			List<NotificationListener> currentNotificationListeners = new();
			List<Task> tasksToSendNotification = new();

			List<Notification> notificationsNeedSendDelay = new();

			while (cancellationToken.IsCancellationRequested != true)
			{
				Notification? notification = null;
				var now = DateTime.Now;

				for (var notificationNeedSendDelayIndex = 0;
					notificationNeedSendDelayIndex < notificationsNeedSendDelay.Count;
					notificationNeedSendDelayIndex++)
				{
					var notificationNeedSendDelay
						= notificationsNeedSendDelay[notificationNeedSendDelayIndex];
					if (notificationNeedSendDelay.SendTimeScheduled <= now)
					{
						// !!!
						notification = notificationNeedSendDelay;
						// !!!
					}
				}
				if (notification == null)
				{
					if (this.TryDequeue(out notification)
						&& notification != null)
					{
						if (notification.IsNeedSendBySendTimeScheduled
							&& notification.SendTimeScheduled > now)
						{
							// !!!
							notificationsNeedSendDelay.Add(notification);
							// !!!
							continue;
							// !!!
						}
					}
				}
				if (notification == null)
				{
					return 0;
				}


				// !!!
				notification.SendTimeBegin = DateTime.Now;
				// !!!

				currentNotificationListeners.Clear();
				lock (allNotificationListeners)
				{
					currentNotificationListeners.AddRange(allNotificationListeners);
				}
				if (currentNotificationListeners.Count < 1)
				{
					continue;
				}
				tasksToSendNotification.Clear();

				var objectNotificationListeners = new List<NotificationListener>();
				foreach (var notificationListener in currentNotificationListeners)
				{
					if (notificationListener.IsListenTo(notification))
					{
						// !!!
						objectNotificationListeners.Add(notificationListener);
						// !!!
					}
				}


				var objectNotificationListenersCount = objectNotificationListeners.Count;
				var sendedObjectNotificationListenersCount = objectNotificationListenersCount;
				var notificationSendResults = new List<object>();
				foreach (var objectNotificationListener in objectNotificationListeners)
				{
					var taskToSendNotification = tasks.RunAsync(async () =>
					{
						try
						{
							var notificationProcessTimeoutSeconds = this.NotificationProcessTimeoutSeconds;
							CancellationTokenSource? cancellationTokenSourceWithTimeout = null;
							var cancellationTokenToSendNotification = tasks.CancellationToken;
							if (notificationProcessTimeoutSeconds > 0)
							{
								cancellationTokenSourceWithTimeout
									= CancellationTokenSource.CreateLinkedTokenSource(cancellationTokenToSendNotification);
								{
									int notificationProcessTimeoutMilliseconds
											= (int)(1000.0 * notificationProcessTimeoutSeconds);
									cancellationTokenSourceWithTimeout.CancelAfter(
											notificationProcessTimeoutMilliseconds);
								}
								cancellationTokenToSendNotification = cancellationTokenSourceWithTimeout.Token;
							}
							////////////////////////////////////////////////
							// !!!
							var toSendReceivedNotification = objectNotificationListener.ToDidReceivedNotification;
							var toSendReceivedNotificationAsync = objectNotificationListener.ToDidReceivedNotificationAsync;
							Object? notificationSendResult = null;
							if (toSendReceivedNotification != null)
							{
								notificationSendResult = toSendReceivedNotification(
									notification,
									cancellationTokenToSendNotification);
							}
							else if (toSendReceivedNotificationAsync != null)
							{
								notificationSendResult = await toSendReceivedNotificationAsync(
									notification,
									cancellationTokenToSendNotification);
							}
							if (notificationSendResult != null)
							{
								lock (notificationSendResults)
								{
									notificationSendResults.Add(notificationSendResult);
								}
							}
							// !!!
							////////////////////////////////////////////////
						}
						catch (Exception exception)
						{
							// !!!
							this.DidReceiveExceptionThrewBySendNotification(notification, exception);
							// !!!
						}
						finally
						{
							// !!!
							Interlocked.Decrement(ref sendedObjectNotificationListenersCount);
							// !!!
							if (sendedObjectNotificationListenersCount == 0)
							{
								var toNotificationSended
								= notification.ToNotificationSended;
								var toNotificationSendedAsync
								= notification.ToNotificationSendedAsync;
								if (toNotificationSended != null)
								{
									toNotificationSended(notificationSendResults);
								}
								else if (toNotificationSendedAsync != null)
								{
									await toNotificationSendedAsync(notificationSendResults);
								}
								// !!!
								notification.SendTimeEnd = DateTime.Now;
								// !!!
							}
							else if (sendedObjectNotificationListenersCount < 0)
							{
								throw new ApplicationException(
									"消息发送数超出了预期，"
									+ "预期“" + (objectNotificationListenersCount) + "”个，"
									+ "实际“" + (-1 * sendedObjectNotificationListenersCount) + "”个。");
							}
						}
					});
					// !!!
					tasksToSendNotification.Add(taskToSendNotification);
					// !!!
				}

				// !!!⚠ 有序队列需要处理完当前消息，再处理下一个消息 ⚠!!!
				if (!this.IsUnorderedQueue)
				{
					Task.WaitAll
						(tasksToSendNotification.ToArray(),
						cancellationToken);
				}
			}
			return this.Count;
		}

		protected virtual void DidReceiveExceptionThrewBySendNotification(
			Notification notification,
			Exception exception)
		{
			this.ToReceiveExceptionThrewBySendNotification?.Invoke(
				this,
				notification,
				exception);
		}


		#endregion


		////////////////////////////////////////////////
		// @重载
		////////////////////////////////////////////////

		#region 重载
		public override bool Equals(object? obj)
		{
			if (obj is NotificationQueue notificationQueue)
			{
				if (notificationQueue.Name?.Equals(this.Name) == true)
				{
					return true;
				}
			}
			return false;
		}

		public override int GetHashCode()
		{
			if (this.Name?.Length > 0)
			{
				return this.Name.GetHashCode();
			}
			return 0;
		}

		#endregion
	}
}
