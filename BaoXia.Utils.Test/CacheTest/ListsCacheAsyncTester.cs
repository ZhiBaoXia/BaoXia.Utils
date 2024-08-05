using BaoXia.Utils.Cache;
using BaoXia.Utils.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Test.CacheTest
{
	public class ListsCacheAsyncTester<ListIdType> where ListIdType : IComparable
	{
		////////////////////////////////////////////////
		// @自身属性
		////////////////////////////////////////////////

		#region 自身属性

		protected string _testName;

		protected Func<ListIdType> _toDidCreateListId;

		protected readonly ListsCachAsync<ListIdType, CacheListItem<ListIdType>, object> _listsCache = new(
			async (listId, createListParam) =>
			{
				return await CacheListItem<ListIdType>.CreateItemListAsync(listId);
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

		public ListsCacheAsyncTester(
			string testName,
			Func<ListIdType> toDidCreateListId)
		{
			_testName = testName;
			_toDidCreateListId = toDidCreateListId;
		}

		public void AddUpdateAndQueryTest()
		{
			var listIds = new ConcurrentBag<ListIdType>();
			for (var testListIndex = 1;
				testListIndex <= CacheTestConfig.TestListsCount;
				testListIndex++)
			{
				var testListId = _toDidCreateListId();
				{ }
				listIds.Add(testListId);
			}

			var randomForInsert = new System.Random((int)DateTime.Now.Ticks);
			var testRecorders_Insert = new CacheTestUnitRecorders();
			Func<Task> insertFunc = new(
				async () =>
				{
					var listId = _toDidCreateListId();
					{ }
					listIds.Add(listId);

					var listItemId = CacheListItem<ListIdType>.NextListItemId();
					var testRecorder = new CacheTestUnitRecorder("【异步列表缓存】（" + _testName + "）插入测试");
					testRecorder.BeginTest(listId.ToString()!);
					////////////////////////////////////////////////
					await _listsCache.AddListItemAsync(
					listId,
					null,
					new CacheListItem<ListIdType>()
					{
						Id = listItemId,
						ListId = listId,
						Title = "列表元素（" + listItemId + "）"
					});
					////////////////////////////////////////////////
					testRecorder.EndTest();
					testRecorders_Insert.AddRecorder(testRecorder);
				});


			var randomForQueryList = new System.Random((int)DateTime.Now.Ticks);
			var randomForQueryListItem = new System.Random((int)DateTime.Now.Ticks);
			var testRecorders_Query = new CacheTestUnitRecorders();
			Func<Task> queryFunc = new(
				async () =>
				{
					if (!listIds.TryTake(out ListIdType? listId))
					{
						listId = _toDidCreateListId();
					}

					var testRecorder = new CacheTestUnitRecorder("【异步列表缓存】（" + _testName + "），查询测试");
					testRecorder.BeginTest(listId.ToString()!);
					////////////////////////////////////////////////
					var listItems = await _listsCache.GetAsync(listId, null);
					////////////////////////////////////////////////
					testRecorder.EndTest();
					testRecorders_Query.AddRecorder(testRecorder);
					// !!!
					Assert.IsTrue(listItems != null);
					// !!!
					if (listItems?.Length > 0)
					{
						foreach (var listItem in listItems)
						{
							// !!!
							Assert.IsTrue(listItem.ListId?.Equals(listId) == true);
							// !!!
						}
					}
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
						requestTasks.Add(Task.Run(insertFunc));
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

		#endregion

	}
}
