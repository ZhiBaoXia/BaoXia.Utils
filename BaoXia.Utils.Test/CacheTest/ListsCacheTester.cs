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
	/// <summary>
	/// 列表缓存测试器。
	/// ⚠异步生成列表会导致测试的QPS急剧下降 ⚠
	// 高QPS：
	// List<CacheListItem<ListIdType>> list = new();
	// return list.ToArray();
	//
	// 低QPS：
	// var createListTask = CacheListItem<ListIdType>.CreateItemListAsync(listId);
	// createListTask.Wait();
	// var list = createListTask.Result;
	// {
	//    Assert.IsTrue(list != null);
	// }
	// return list;
	//
	/// </summary>
	/// <typeparam name="ListIdType">列表的Id类型。</typeparam>
	public class ListsCacheTester<ListIdType> where ListIdType : IComparable
	{
		////////////////////////////////////////////////
		// @自身属性
		////////////////////////////////////////////////

		#region 自身属性

		protected string _testName;

		protected Func<ListIdType> _toDidCreateListId;

		protected readonly ListsCache<ListIdType, CacheListItem<ListIdType>, object> _listsCache = new(
			(listId, createListParam) =>
			{
				// !!!⚠异步生成列表会导致测试的QPS急剧下降 ⚠!!!
				//List<CacheListItem<ListIdType>> list = new();
				//return list.ToArray();

				var createListTask = CacheListItem<ListIdType>.CreateItemListAsync(listId);
				// !!!
				createListTask.Wait();
				// !!!
				var list = createListTask.Result;
				{
					Assert.IsTrue(list != null);
				}
				return list;
			},
			(listKey, lastList, currentList, itemCacheOperation) =>
			{
			},
			() => CacheTestConfig.NoneReadSecondsToClearItemCache);

		#endregion


		////////////////////////////////////////////////
		// @自身实现
		////////////////////////////////////////////////

		#region 自身实现

		public ListsCacheTester(
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
			Action insertAction = new(
			    () =>
			    {
				    var listId = _toDidCreateListId();
				    { }
				    listIds.Add(listId);

				    var listItemId = CacheListItem<ListIdType>.NextListItemId();
				    var testRecorder = new CacheTestUnitRecorder("【同步列表缓存】（" + _testName + "）插入测试");
				    testRecorder.BeginTest(listId.ToString()!);
				    ////////////////////////////////////////////////
				    _listsCache.AddListItem(
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
			Action queryAction = new(
			    () =>
			    {
				    ListIdType? listId = default;
				    if (!listIds.TryTake(out listId))
				    {
					    listId = _toDidCreateListId();
				    }

				    var testRecorder = new CacheTestUnitRecorder("【同步列表缓存】（" + _testName + "），查询测试");
				    testRecorder.BeginTest(listId.ToString()!);
				    ////////////////////////////////////////////////
				    var listItems = _listsCache.Get(listId, null);
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
						|| (CacheTestConfig.InsertTestTasksCount == 0
						|| requestTasks.Count < CacheTestConfig.InsertTestTasksCount)))
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
						requestTasks.Add(Task.Run(queryAction));
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
