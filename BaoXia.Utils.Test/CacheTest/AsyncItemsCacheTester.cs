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
	public class AsyncItemsCacheTester<ItemIdType> where ItemIdType : IComparable
	{
		////////////////////////////////////////////////
		// @自身属性
		////////////////////////////////////////////////

		#region 自身属性

		protected string _testName;

		protected Func<ItemIdType> _toDidCreateItemId;

		protected readonly ItemsCacheAsync<ItemIdType, CacheItem<ItemIdType>, object> _itemsCache = new(
			async (itemId, _) =>
			{
				var item = await CacheItem<ItemIdType>.CreateItemAsync(itemId);
				{
					Assert.IsTrue(item != null);
				}
				return item;
			},
			async (listKey, lastList, currentList, listOperation) =>
			{
				return await Task.FromResult(currentList);
			},
			() => CacheTestConfig.NoneReadSecondsToClearItemCache);

		#endregion


		////////////////////////////////////////////////
		// @自身实现
		////////////////////////////////////////////////

		#region 自身实现

		public AsyncItemsCacheTester(
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
			   async () =>
				{
					var itemId = _toDidCreateItemId();
					{ }
					itemIds.Add(itemId);

					var testRecorder = new CacheTestUnitRecorder("【异步元素缓存】（" + _testName + "）插入测试");
					testRecorder.BeginTest(itemId.ToString()!);
					////////////////////////////////////////////////
					await _itemsCache.AddAsync(
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
			Func<Task> queryFunc = new(
				async () =>
				{
					if (!itemIds.TryTake(out var itemId))
					{
						itemId = _toDidCreateItemId();
					}

					var testRecorder = new CacheTestUnitRecorder("【异步元素缓存】（" + _testName + "），查询测试");
					testRecorder.BeginTest(itemId.ToString()!);
					////////////////////////////////////////////////
					var item = await _itemsCache.GetAsync(itemId, null);
					{
						// !!!
						Assert.IsTrue(item != null);
						Assert.IsTrue(item.Id?.Equals(itemId) == true);
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
				tasksToGet.Add(Task.Run(async () =>
				{
					// !!!
					lock (workThreadIds_0)
					{
						workThreadIds_0.Add(System.Environment.CurrentManagedThreadId);
					}
					// !!!
					var item = await _itemsCache.GetAsync(itemId, null);
					// !!!
					if (item != null)
					{
						var itemIdGot = item.Id;
						if (itemIdGot?.Equals(itemId) != true)
						{
							Assert.Fail();
						}
					}
					else
					{
						Assert.Fail();
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
				"\r\n" + "【异步元素缓存】（" + _testName + "），同时获取同一Key"
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


		public async Task TryGetTest()
		{
			var itemIdsAdded = new ConcurrentBag<ItemIdType>();
			var itemIdsCreatedAsync = new ConcurrentBag<ItemIdType>();


			var randomForInsert = new System.Random((int)DateTime.Now.Ticks);
			var testRecorders_Insert = new CacheTestUnitRecorders();
			Action insertAction = new(
				async () =>
				{
					var itemId = _toDidCreateItemId();

					var testRecorder = new CacheTestUnitRecorder("【异步元素缓存】（" + _testName + "）“尝试”插入测试");
					testRecorder.BeginTest(itemId.ToString()!);
					////////////////////////////////////////////////
					await _itemsCache.AddAsync(
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
				async () =>
				{
					var isItemIdExisted = false;
					if ((DateTime.Now.Ticks % 2) == 1
						&& itemIdsAdded.TryTake(out var itemId))
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

					var testRecorder = new CacheTestUnitRecorder("【异步元素缓存】（" + _testName + "），“尝试”查询测试");
					testRecorder.BeginTest(itemId.ToString()!);
					////////////////////////////////////////////////

					var tryGetResult = await _itemsCache.TryGetAsync(
						itemId,
						true);
					if (tryGetResult.IsGotSucess)
					{
						var item = tryGetResult.Item;
						// !!!
						//Assert.IsTrue(isItemIdExisted == true);
						Assert.IsTrue(item != null);
						Assert.IsTrue(item.Id?.Equals(itemId));
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

			// 等待 1 秒后，所有“TryGet”建立的缓存，都应该建立完毕了。
			await Task.Delay(1000);

			////////////////////////////////////////////////
			// 异步常见缓存测试：
			////////////////////////////////////////////////
			Task.Run(async () =>
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
						var tryGetResult = await _itemsCache.TryGetAsync(itemId);
						Assert.IsTrue(tryGetResult.IsGotSucess);
						var item = tryGetResult.Item;
						Assert.IsTrue(item?.Id?.Equals(itemId) == true);
					}
				}
			})
			.Wait();


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
