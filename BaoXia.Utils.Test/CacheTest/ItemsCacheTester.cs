using BaoXia.Utils.Cache;
using BaoXia.Utils.Extensions;
using BaoXia.Utils.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Test.CacheTest
{
	public class ItemsCacheTester<ItemIdType> where ItemIdType : IComparable
	{
		////////////////////////////////////////////////
		// @自身属性
		////////////////////////////////////////////////

		#region 自身属性

		protected string _testName;

		protected Func<ItemIdType> _toDidCreateItemId;

		protected readonly ItemsCache<ItemIdType, CacheItem<ItemIdType>, object> _itemsCache = new(
			(itemId, createListParam) =>
			{
				var createItemTask = CacheItem<ItemIdType>.CreateItemAsync(itemId);
				// !!!
				createItemTask.Wait();
				// !!!
				var item = createItemTask.Result;
				{
					Assert.IsTrue(item != null);
				}
				return item;
			},
			(listKey, lastList, currentList, itemCacheOperation) =>
			{
				return currentList;
			},
			() => CacheTestConfig.NoneReadSecondsToClearItemCache);

		#endregion
		////////////////////////////////////////////////
		// @自身实现
		////////////////////////////////////////////////

		#region 自身实现

		public ItemsCacheTester(
			string testName,
			Func<ItemIdType> toDidCreateItemId)
		{
			_testName = testName;
			_toDidCreateItemId = toDidCreateItemId;
		}

		public void AddUpdateAndQueryTest()
		{
			var itemIds = new ConcurrentBag<ItemIdType>();
			for (var testItemIndex = 1;
				testItemIndex <= CacheTestConfig.TestItemsCount;
				testItemIndex++)
			{
				var testItemId = _toDidCreateItemId();
				{ }
				itemIds.Add(testItemId);
			}


			var randomForInsert = new System.Random((int)DateTime.Now.Ticks);
			var testRecorders_Insert = new CacheTestUnitRecorders();
			Action insertAction = new(
				() =>
				{
					var itemId = _toDidCreateItemId();
					{ }
					itemIds.Add(itemId);

					var testRecorder = new CacheTestUnitRecorder("【同步元素缓存】（" + _testName + "）插入测试");
					testRecorder.BeginTest(itemId.ToString()!);
					////////////////////////////////////////////////
					_itemsCache.Add(
					 itemId,
					 new CacheItem<ItemIdType>()
					 {
						 Id = itemId,
						 Title = "元素（" + itemId + "）"
					 });
					////////////////////////////////////////////////
					testRecorder.EndTest();
					testRecorders_Insert.AddRecorder(testRecorder);
				});


			var randomForQueryItem = new System.Random((int)DateTime.Now.Ticks);
			var testRecorders_Query = new CacheTestUnitRecorders();
			Action queryFunc = new(
				() =>
				{
					ItemIdType? itemId = default;
					if (!itemIds.TryTake(out itemId))
					{
						itemId = _toDidCreateItemId();
					}

					var testRecorder = new CacheTestUnitRecorder("【同步元素缓存】（" + _testName + "），查询测试");
					testRecorder.BeginTest(itemId.ToString()!);
					////////////////////////////////////////////////
					var item = _itemsCache.Get(itemId, null);
					{
						// !!!
						Assert.IsTrue(item != null);
						Assert.IsTrue(item?.Id?.Equals(itemId) == true);
						// !!!
					}
					////////////////////////////////////////////////
					testRecorder.EndTest();
					testRecorders_Query.AddRecorder(testRecorder);
				});


			////////////////////////////////////////////////
			// 并发插入&读取：
			////////////////////////////////////////////////
			{
				var testTasks = new List<Task>();
				var testsBeginTime = DateTime.Now;
				var testsEndTime = testsBeginTime.AddSeconds(CacheTestConfig.TestDurationSeconds);

				testTasks.Add(Task.Run(() =>
				{
					var requestTasks = new List<Task>();
					while (DateTime.Now < testsEndTime
					&& (CacheTestConfig.InsertTestTasksCount == 0
						|| requestTasks.Count < CacheTestConfig.InsertTestTasksCount))
					{
						requestTasks.Add(Task.Run(insertAction));
						{ }
						// !!!
						Thread.Sleep((int)(1000 * CacheTestConfig.InsertTestIntervalSeconds));
						// !!!
					}
					Task.WaitAll(requestTasks.ToArray());
				}));

				testTasks.Add(Task.Run(() =>
				{
					var requestTasks = new List<Task>();
					while (DateTime.Now < testsEndTime
					&& (CacheTestConfig.QueryTestTasksCount == 0
						|| requestTasks.Count < CacheTestConfig.QueryTestTasksCount))
					{
						requestTasks.Add(Task.Run(queryFunc));
						{ }
						// !!!
						Thread.Sleep((int)(1000 * CacheTestConfig.QueryTestIntervalSeconds));
						// !!!
					}
					Task.WaitAll(requestTasks.ToArray());
				}));

				// !!!
				Task.WaitAll(testTasks.ToArray());
				// !!!
			}


			////////////////////////////////////////////////
			// 打印测试结果：
			////////////////////////////////////////////////
			{
				testRecorders_Insert.LogTestsInfo();
				testRecorders_Query.LogTestsInfo();
			}
		}

		public void GetSameKeyAtSameTimeTest()
		{
			var tasksToGet = new List<Task>();
			var beginTime = DateTime.Now;
			var endTime = beginTime.AddSeconds(CacheTestConfig.GetSameItemTestDurationSeconds);
			var itemId = _toDidCreateItemId();
			var workThreadIds_0 = new List<int>();
			var tasksCount = 0;
			while (DateTime.Now < endTime)
			{
				tasksToGet.Add(Task.Run(() =>
				{
					// !!!
					lock (workThreadIds_0)
					{
						workThreadIds_0.Add(Thread.CurrentThread.ManagedThreadId);
					}
					// !!!
					var item = _itemsCache.Get(itemId, null);
					// !!!
					if (item != null)
					{
						var itemIdGot = item.Id;
						if (itemIdGot?.Equals(itemId) != true)
						{
							Assert.Fail();
						}
					}
				}));

				tasksCount++;
				// System.Diagnostics.Debug.WriteLine("测试线程数：" + tasksCount);
				if (CacheTestConfig.GetSameItemTestTasksCount > 0
					&& tasksCount >= CacheTestConfig.GetSameItemTestTasksCount)
				{
					break;
				}
			}
			// !!!
			Task.WaitAll(tasksToGet.ToArray());
			// !!!

			var now = DateTime.Now;
			var durationMilliseconds = (now - beginTime).TotalMilliseconds;

			System.Diagnostics.Debug.WriteLine(
				"\r\n" + "【同步元素缓存】（" + _testName + "），同时获取同一Key"
				+ "\r\n开始时间：" + beginTime.ToString("HH:mm fff")
				+ "\r\n结束时间：" + now.ToString("HH:mm fff")
				+ "\r\n测试耗时：" + durationMilliseconds + "ms");

			var originalThreadsCount = workThreadIds_0.Count;
			{
				workThreadIds_0.RemoveSameItems();
			}
			var finalThreadsCount = workThreadIds_0.Count;

			System.Diagnostics.Debug.WriteLine(
				"原始线程数：" + originalThreadsCount + "，"
				+ "\r\n实际线程数：" + finalThreadsCount + "。"
				+ "\r\n");
		}

		public void TryGetTest()
		{
			var itemIdsAdded = new ConcurrentBag<ItemIdType>();
			var itemIdsCreatedAsync = new ConcurrentBag<ItemIdType>();


			var randomForInsert = new System.Random((int)DateTime.Now.Ticks);
			var testRecorders_Insert = new CacheTestUnitRecorders();
			Action insertAction = new(
				() =>
				{
					var itemId = _toDidCreateItemId();

					var testRecorder = new CacheTestUnitRecorder("【同步元素缓存】（" + _testName + "）“尝试”插入测试");
					testRecorder.BeginTest(itemId.ToString()!);
					////////////////////////////////////////////////
					_itemsCache.Add(
					 itemId,
					 new CacheItem<ItemIdType>()
					 {
						 Id = itemId,
						 Title = "元素（" + itemId + "）"
					 });
					itemIdsAdded.Add(itemId);
					////////////////////////////////////////////////
					testRecorder.EndTest();
					testRecorders_Insert.AddRecorder(testRecorder);
				});


			var randomForQueryItem = new System.Random((int)DateTime.Now.Ticks);
			var testRecorders_Query = new CacheTestUnitRecorders();
			Action tryGetFunc = new(
				() =>
				{
					ItemIdType? itemId = default;
					var isItemIdExisted = false;
					if ((DateTime.Now.Ticks % 2) == 1
						&& itemIdsAdded.TryTake(out itemId))
					{
						isItemIdExisted = true;
					}
					else
					{
						itemId = _toDidCreateItemId();
						// !!!
						itemIdsCreatedAsync.Add(itemId);
						// !!!
					}

					var testRecorder = new CacheTestUnitRecorder("【同步元素缓存】（" + _testName + "），“尝试”查询测试");
					testRecorder.BeginTest(itemId.ToString()!);
					////////////////////////////////////////////////
					if (_itemsCache.TryGet(
						itemId,
						out var item,
						true))
					{
						// !!!
						//Assert.IsTrue(isItemIdExisted == true);
						Assert.IsTrue(item != null);
						Assert.IsTrue(item?.Id?.Equals(itemId) == true);
						// !!!
					}
					else
					{
						Assert.IsTrue(isItemIdExisted == false);
					}
					////////////////////////////////////////////////
					testRecorder.EndTest();
					testRecorders_Query.AddRecorder(testRecorder);
				});


			////////////////////////////////////////////////
			// 并发插入&读取：
			////////////////////////////////////////////////
			{
				var testTasks = new List<Task>();
				var testsBeginTime = DateTime.Now;
				var testsEndTime = testsBeginTime.AddSeconds(CacheTestConfig.TestDurationSeconds);

				testTasks.Add(Task.Run(() =>
				{
					var requestTasks = new List<Task>();
					while (DateTime.Now < testsEndTime
					&& (CacheTestConfig.InsertTestTasksCount == 0
						|| requestTasks.Count < CacheTestConfig.InsertTestTasksCount))
					{
						requestTasks.Add(Task.Run(insertAction));
						{ }
						// !!!
						Thread.Sleep((int)(1000 * CacheTestConfig.InsertTestIntervalSeconds));
						// !!!
					}
					Task.WaitAll(requestTasks.ToArray());
				}));

				testTasks.Add(Task.Run(() =>
				{
					var requestTasks = new List<Task>();
					while (DateTime.Now < testsEndTime
					&& (CacheTestConfig.QueryTestTasksCount == 0
						|| requestTasks.Count < CacheTestConfig.QueryTestTasksCount))
					{
						requestTasks.Add(Task.Run(tryGetFunc));
						{ }
						// !!!
						Thread.Sleep((int)(1000 * CacheTestConfig.QueryTestIntervalSeconds));
						// !!!
					}
					Task.WaitAll(requestTasks.ToArray());
				}));

				// !!!
				Task.WaitAll(testTasks.ToArray());
				// !!!
			}

			////////////////////////////////////////////////
			// 异步常见缓存测试：
			////////////////////////////////////////////////
			{
				const double waitSecondsMax = 10.0F;
				var beginTime = DateTime.Now;
				while (_itemsCache.ItemContainersCountInCreatingItemAsync > 0)
				{
					Thread.Sleep(100);
					if ((DateTime.Now - beginTime).TotalSeconds >= waitSecondsMax)
					{
						break;
					}
				}
				Assert.IsTrue(_itemsCache.ItemContainersCountInCreatingItemAsync <= 0);
				{
					foreach (var itemId in itemIdsCreatedAsync)
					{
						Assert.IsTrue(_itemsCache.TryGet(itemId, out var item));
						Assert.IsTrue(item?.Id?.Equals(itemId) == true);
					}
				}
			}


			////////////////////////////////////////////////
			// 打印测试结果：
			////////////////////////////////////////////////
			{
				testRecorders_Insert.LogTestsInfo();
				testRecorders_Query.LogTestsInfo();
			}
		}

		#endregion
	}
}
