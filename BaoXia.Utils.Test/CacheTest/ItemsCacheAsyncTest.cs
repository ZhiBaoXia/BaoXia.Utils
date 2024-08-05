using BaoXia.Utils.Cache;
using BaoXia.Utils.Cache.Index;
using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Test.CacheTest;

[TestClass]
public class ItemsCacheAsyncTest
{
	////////////////////////////////////////////////
	// @测试方法
	////////////////////////////////////////////////

	[TestMethod]
	public void AddUpdateAndQueryTest_IntKey()
	{
		var itemId = 0;
		var tester = new ItemsCacheAsyncTester<int>(
			"整数Key",
			() =>
			{
				return Interlocked.Increment(ref itemId);
			});
		// !!!
		tester.AddUpdateAndQueryTest();
		// !!!
	}

	[TestMethod]
	public void GetSameKeyAtSameTimeTest_IntKey()
	{
		var itemId = 0;
		var tester = new ItemsCacheAsyncTester<int>(
			"整数Key",
			() =>
			{
				return Interlocked.Increment(ref itemId);
			});
		// !!!
		// tester.GetSameKeyAtSameTimeTest();
		// !!!
	}

	[TestMethod]
	public async Task TryGetTest_IntKey()
	{
		var itemId = 0;
		var tester = new ItemsCacheAsyncTester<int>(
			"整数Key",
			() =>
			{
				return Interlocked.Increment(ref itemId);
			});
		// !!!
		await tester.TryGetTest();
		// !!!
	}

	[TestMethod]
	public void AddUpdateAndQueryTest_StringKey()
	{
		var itemId = 0;
		var tester = new ItemsCacheAsyncTester<string>(
			"字符串Key",
			() =>
			{
				return Interlocked.Increment(ref itemId).ToString();
			});
		// !!!
		tester.AddUpdateAndQueryTest();
		// !!!
	}

	[TestMethod]
	public void GetSameKeyAtSameTimeTest_StringKey()
	{
		var itemId = 0;
		var tester = new ItemsCacheAsyncTester<string>(
			"字符串Key",
			() =>
			{
				return Interlocked.Increment(ref itemId).ToString();
			});
		// !!!
		// tester.GetSameKeyAtSameTimeTest();
		// !!!
	}

	[TestMethod]
	public async Task TryGetTest_StringKey()
	{
		var itemId = 0;
		var tester = new ItemsCacheAsyncTester<string>(
			"字符串Key",
			() =>
			{
				return Interlocked.Increment(ref itemId).ToString();
			});
		// !!!
		await tester.TryGetTest();
		// !!!
	}

	[TestMethod]
	public void InitializeWithNullTest()
	{
		var itemCache = new ItemsCacheAsync<string, object, object>(
			async (key, _) =>
			{
				await Task.Delay(1);

				return null;
			},
			null,
			null,
			null);
		Assert.IsTrue(true);
	}

	class TestItem
	{
		public int Id { get; set; }
		public DateTime CreateTime { get; set; }
	}

	[TestMethod]
	public async Task AutoRemoveNoneReadCacheItems_AutoRemove()
	{
		var itemsCache = new ItemsCacheAsync<int, TestItem, object>(
			async (id, _) =>
			{
				await Task.Delay(0);
				return new TestItem
				{
					Id = id,
					CreateTime = DateTime.Now
				};
			},
			null,
			() => 1.0);
		for (var itemId = 1;
			itemId <= 100;
			itemId++)
		{
			await itemsCache.GetAsync(itemId, null);
		}

		await Task.Delay(3000);

		Assert.IsTrue(itemsCache.Count == 0);
	}

	[TestMethod]
	public async Task AutoRemoveNoneReadCacheItems_AutoUpdateReadTime()
	{
		var itemsCache = new ItemsCacheAsync<int, TestItem, object>(
			async (id, _) =>
			{
				await Task.Delay(0);
				return new TestItem
				{
					Id = id,
					CreateTime = DateTime.Now
				};
			},
			null,
			() => 2.0);

		const int itemsCount = 100;
		for (var itemId = 1;
			itemId <= itemsCount;
			itemId++)
		{
			await itemsCache.GetAsync(itemId, null);
		}

		var retainTaskCancelSource = new CancellationTokenSource();
		_ = Task.Run(async () =>
		{
			while (!retainTaskCancelSource.Token.IsCancellationRequested)
			{
				await Task.Delay(1000);
				for (var itemId = 1;
					itemId <= itemsCount;
					itemId++)
				{
					_ = await itemsCache.GetAsync(itemId, null);
				}
			}
		});

		await Task.Delay(3000);
		retainTaskCancelSource.Cancel();

		Assert.IsTrue(itemsCache.Count == itemsCount);
	}

	class TestItemForIndexTest(
		int id,
		int property0,
		int property1,
		int property2,
		int property3,
		int property4,
		int property5)
	{
		public int Id { get; set; } = id;
		public int Property1 { get; set; } = property0;
		public int Property2 { get; set; } = property1;
		public int Property3 { get; set; } = property2;
		public int Property4 { get; set; } = property3;
		public int Property5 { get; set; } = property4;
		public int Property6 { get; set; } = property5;
	}

	[TestMethod]
	public void ItemCacheIndexByOperation_Get_Test()
	{
		var itemIndexWith1Key = new ItemIndexWith1Key<TestItemForIndexTest, int>(
			(item) => item.Property1);
		var itemIndexWith2Keys = new ItemIndexWith2Keys<TestItemForIndexTest, int, int>(
			(item) => item.Property1,
			(item) => item.Property2);
		var itemIndexWith3Keys = new ItemIndexWith3Keys<TestItemForIndexTest, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3);
		var itemIndexWith4Keys = new ItemIndexWith4Keys<TestItemForIndexTest, int, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3,
			(item) => item.Property4);
		var itemIndexWith5Keys = new ItemIndexWith5Keys<TestItemForIndexTest, int, int, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3,
			(item) => item.Property4,
			(item) => item.Property5);
		var itemIndexWith6Keys = new ItemIndexWith6Keys<TestItemForIndexTest, int, int, int, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3,
			(item) => item.Property4,
			(item) => item.Property5,
			(item) => item.Property6);

		////////////////////////////////////////////////

		var itemsIndexWith1Key = new ItemsIndexWith1Key<TestItemForIndexTest, int>(
			(item) => item.Property1,
			(itemA, itemB) => itemA.Id == itemB.Id,
			(itemA, itemB) => itemB.Id.CompareTo(itemA.Id));
		var itemsIndexWith2Keys = new ItemsIndexWith2Keys<TestItemForIndexTest, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(itemA, itemB) => itemA.Id == itemB.Id,
			(itemA, itemB) => itemB.Id.CompareTo(itemA.Id));
		var itemsIndexWith3Keys = new ItemsIndexWith3Keys<TestItemForIndexTest, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3,
			(itemA, itemB) => itemA.Id == itemB.Id,
			(itemA, itemB) => itemB.Id.CompareTo(itemA.Id));
		var itemsIndexWith4Keys = new ItemsIndexWith4Keys<TestItemForIndexTest, int, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3,
			(item) => item.Property4,
			(itemA, itemB) => itemA.Id == itemB.Id,
			(itemA, itemB) => itemB.Id.CompareTo(itemA.Id));
		var itemsIndexWith5Keys = new ItemsIndexWith5Keys<TestItemForIndexTest, int, int, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3,
			(item) => item.Property4,
			(item) => item.Property5,
			(itemA, itemB) => itemA.Id == itemB.Id,
			(itemA, itemB) => itemB.Id.CompareTo(itemA.Id));
		var itemsIndexWith6Keys = new ItemsIndexWith6Keys<TestItemForIndexTest, int, int, int, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3,
			(item) => item.Property4,
			(item) => item.Property5,
			(item) => item.Property6,
			(itemA, itemB) => itemA.Id == itemB.Id,
			(itemA, itemB) => itemB.Id.CompareTo(itemA.Id));

		////////////////////////////////////////////////

		var itemsCache = new ItemsCacheAsync<int, TestItemForIndexTest, TestItemForIndexTest>(
			async (itemId, itemSpecified) =>
			{
				return await Task.FromResult(itemSpecified);
			},
			null,
			null,
			[
				itemIndexWith1Key,
				itemIndexWith2Keys,
				itemIndexWith3Keys,
				itemIndexWith4Keys,
				itemIndexWith5Keys,
				itemIndexWith6Keys,
				//
				itemsIndexWith1Key,
				itemsIndexWith2Keys,
				itemsIndexWith3Keys,
				itemsIndexWith4Keys,
				itemsIndexWith5Keys,
				itemsIndexWith6Keys
				]);
		var testItems = new TestItemForIndexTest[100];
		var itemsGroupByProperty_1 = new Dictionary<string, List<TestItemForIndexTest>>();
		var itemsGroupByProperty_1_2 = new Dictionary<string, List<TestItemForIndexTest>>();
		var itemsGroupByProperty_1_2_3 = new Dictionary<string, List<TestItemForIndexTest>>();
		var itemsGroupByProperty_1_2_3_4 = new Dictionary<string, List<TestItemForIndexTest>>();
		var itemsGroupByProperty_1_2_3_4_5 = new Dictionary<string, List<TestItemForIndexTest>>();
		var itemsGroupByProperty_1_2_3_4_5_6 = new Dictionary<string, List<TestItemForIndexTest>>();
		for (var testItemIndex = 0;
			testItemIndex < testItems.Length;
			testItemIndex++)
		{
			var testItem = new TestItemForIndexTest(
				(testItemIndex + 1),
				Random.Shared.Next(testItems.Length),
				Random.Shared.Next(testItems.Length),
				Random.Shared.Next(testItems.Length),
				Random.Shared.Next(testItems.Length),
				Random.Shared.Next(testItems.Length),
				Random.Shared.Next(testItems.Length));
			testItems[testItemIndex] = testItem;
			////////////////////////////////////////////////
			// !!!
			var getResult = itemsCache.GetAsync(testItem.Id, testItem);
			getResult.Wait();
			// !!!
			////////////////////////////////////////////////


			var itemKey = StringUtil.StringWithInts([
					testItem.Property1]);
			if (!itemsGroupByProperty_1.TryGetValue(
				itemKey,
				out var items))
			{
				items = [];
				itemsGroupByProperty_1.Add(itemKey, items);
			}
			items.Add(testItem);
			////
			itemKey = StringUtil.StringWithInts([
					testItem.Property1,
					testItem.Property2]);
			if (!itemsGroupByProperty_1_2.TryGetValue(
				itemKey,
				out items))
			{
				items = [];
				itemsGroupByProperty_1_2.Add(itemKey, items);
			}
			items.Add(testItem);
			////
			itemKey = StringUtil.StringWithInts([
					testItem.Property1,
					testItem.Property2,
					testItem.Property3]);
			if (!itemsGroupByProperty_1_2_3.TryGetValue(
				itemKey,
				out items))
			{
				items = [];
				itemsGroupByProperty_1_2_3.Add(itemKey, items);
			}
			items.Add(testItem);
			////
			itemKey = StringUtil.StringWithInts([
					testItem.Property1,
					testItem.Property2,
					testItem.Property3,
					testItem.Property4]);
			if (!itemsGroupByProperty_1_2_3_4.TryGetValue(
				itemKey,
				out items))
			{
				items = [];
				itemsGroupByProperty_1_2_3_4.Add(itemKey, items);
			}
			items.Add(testItem);
			////
			itemKey = StringUtil.StringWithInts([
					testItem.Property1,
					testItem.Property2,
					testItem.Property3,
					testItem.Property4,
					testItem.Property5]);
			if (!itemsGroupByProperty_1_2_3_4_5.TryGetValue(
				itemKey,
				out items))
			{
				items = [];
				itemsGroupByProperty_1_2_3_4_5.Add(itemKey, items);
			}
			items.Add(testItem);
			////
			itemKey = StringUtil.StringWithInts([
					testItem.Property1,
					testItem.Property2,
					testItem.Property3,
					testItem.Property4,
					testItem.Property5,
					testItem.Property6]);
			if (!itemsGroupByProperty_1_2_3_4_5_6.TryGetValue(
				itemKey,
				out items))
			{
				items = [];
				itemsGroupByProperty_1_2_3_4_5_6.Add(itemKey, items);
			}
			items.Add(testItem);
		}

		////////////////////////////////////////////////
		////////////////////////////////////////////////
		////////////////////////////////////////////////

		Assert.IsTrue(itemsCache.Keys.Count == testItems.Length);

		// itemsGroupByProperty_1
		foreach (var itemWithProperty_1 in itemsGroupByProperty_1)
		{
			var objectItems = itemWithProperty_1.Value;
			var objectItem = objectItems[^1];
			Assert.IsTrue(int.TryParse(itemWithProperty_1.Key, out var itemKey));
			var gotItem = itemIndexWith1Key.GetItem(itemKey);
			{
				//
				Assert.IsTrue(gotItem != null);
				Assert.IsTrue(gotItem.Id == objectItem.Id);
				//
			}
			var gotItems = itemsIndexWith1Key.GetItems(itemKey);
			{
				//
				Assert.IsTrue(gotItems?.Length == objectItems.Count);
				foreach (var itemInGotItems in gotItems)
				{
					Assert.IsTrue(itemInGotItems.Property1 == objectItem.Property1);
				}
				//
			}
		}
		// itemsGroupByProperty_1_2
		foreach (var itemWithProperty_1_2 in itemsGroupByProperty_1_2)
		{
			var objectItems = itemWithProperty_1_2.Value;
			var objectItem = objectItems[^1];
			var itemKeys = itemWithProperty_1_2.Key.ToIntArray();
			{
				Assert.IsTrue(itemKeys.Length == 2);
			}
			var gotItem = itemIndexWith2Keys.GetItem(itemKeys[0], itemKeys[1]);
			{
				//
				Assert.IsTrue(gotItem != null);
				Assert.IsTrue(gotItem.Id == objectItem.Id);
				//
			}
			var gotItems = itemsIndexWith2Keys.GetItems(itemKeys[0], itemKeys[1]);
			{
				//
				Assert.IsTrue(gotItems?.Length == objectItems.Count);
				foreach (var itemInGotItems in gotItems)
				{
					Assert.IsTrue(itemInGotItems.Property1 == objectItem.Property1);
					Assert.IsTrue(itemInGotItems.Property2 == objectItem.Property2);
				}
				//
			}
		}
		// itemsGroupByProperty_1_2_3
		foreach (var itemWithProperty_1_2_3 in itemsGroupByProperty_1_2_3)
		{
			var objectItems = itemWithProperty_1_2_3.Value;
			var objectItem = objectItems[^1];
			var itemKeys = itemWithProperty_1_2_3.Key.ToIntArray();
			{
				Assert.IsTrue(itemKeys.Length == 3);
			}
			var gotItem = itemIndexWith3Keys.GetItem(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2]);
			{
				//
				Assert.IsTrue(gotItem != null);
				Assert.IsTrue(gotItem.Id == objectItem.Id);
				//
			}
			var gotItems = itemsIndexWith3Keys.GetItems(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2]);
			{
				//
				Assert.IsTrue(gotItems?.Length == objectItems.Count);
				foreach (var itemInGotItems in gotItems)
				{
					Assert.IsTrue(itemInGotItems.Property1 == objectItem.Property1);
					Assert.IsTrue(itemInGotItems.Property2 == objectItem.Property2);
					Assert.IsTrue(itemInGotItems.Property3 == objectItem.Property3);
				}
				//
			}
		}
		// itemsGroupByProperty_1_2_3_4
		foreach (var itemWithProperty_1_2_3_4 in itemsGroupByProperty_1_2_3_4)
		{
			var objectItems = itemWithProperty_1_2_3_4.Value;
			var objectItem = objectItems[^1];
			var itemKeys = itemWithProperty_1_2_3_4.Key.ToIntArray();
			{
				Assert.IsTrue(itemKeys.Length == 4);
			}
			var gotItem = itemIndexWith4Keys.GetItem(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2],
				itemKeys[3]);
			{
				//
				Assert.IsTrue(gotItem != null);
				Assert.IsTrue(gotItem.Id == objectItem.Id);
				//
			}
			var gotItems = itemsIndexWith4Keys.GetItems(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2],
				itemKeys[3]);
			{
				//
				Assert.IsTrue(gotItems?.Length == objectItems.Count);
				foreach (var itemInGotItems in gotItems)
				{
					Assert.IsTrue(itemInGotItems.Property1 == objectItem.Property1);
					Assert.IsTrue(itemInGotItems.Property2 == objectItem.Property2);
					Assert.IsTrue(itemInGotItems.Property3 == objectItem.Property3);
					Assert.IsTrue(itemInGotItems.Property4 == objectItem.Property4);
				}
				//
			}
		}
		// itemsGroupByProperty_1_2_3_4_5
		foreach (var itemWithProperty_1_2_3_4_5 in itemsGroupByProperty_1_2_3_4_5)
		{
			var objectItems = itemWithProperty_1_2_3_4_5.Value;
			var objectItem = objectItems[^1];
			var itemKeys = itemWithProperty_1_2_3_4_5.Key.ToIntArray();
			{
				Assert.IsTrue(itemKeys.Length == 5);
			}
			var gotItem = itemIndexWith5Keys.GetItem(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2],
				itemKeys[3],
				itemKeys[4]);
			{
				//
				Assert.IsTrue(gotItem != null);
				Assert.IsTrue(gotItem.Id == objectItem.Id);
				//
			}
			var gotItems = itemsIndexWith5Keys.GetItems(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2],
				itemKeys[3],
				itemKeys[4]);
			{
				//
				Assert.IsTrue(gotItems?.Length == objectItems.Count);
				foreach (var itemInGotItems in gotItems)
				{
					Assert.IsTrue(itemInGotItems.Property1 == objectItem.Property1);
					Assert.IsTrue(itemInGotItems.Property2 == objectItem.Property2);
					Assert.IsTrue(itemInGotItems.Property3 == objectItem.Property3);
					Assert.IsTrue(itemInGotItems.Property4 == objectItem.Property4);
					Assert.IsTrue(itemInGotItems.Property5 == objectItem.Property5);
				}
				//
			}
		}
		// itemsGroupByProperty_1_2_3_4_5_6
		foreach (var itemWithProperty_1_2_3_4_5_6 in itemsGroupByProperty_1_2_3_4_5_6)
		{
			var objectItems = itemWithProperty_1_2_3_4_5_6.Value;
			var objectItem = objectItems[^1];
			var itemKeys = itemWithProperty_1_2_3_4_5_6.Key.ToIntArray();
			{
				Assert.IsTrue(itemKeys.Length == 6);
			}
			var gotItem = itemIndexWith6Keys.GetItem(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2],
				itemKeys[3],
				itemKeys[4],
				itemKeys[5]);
			{
				//
				Assert.IsTrue(gotItem != null);
				Assert.IsTrue(gotItem.Id == objectItem.Id);
				//
			}
			var gotItems = itemsIndexWith6Keys.GetItems(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2],
				itemKeys[3],
				itemKeys[4],
				itemKeys[5]);
			{
				//
				Assert.IsTrue(gotItems?.Length == objectItems.Count);
				foreach (var itemInGotItems in gotItems)
				{
					Assert.IsTrue(itemInGotItems.Property1 == objectItem.Property1);
					Assert.IsTrue(itemInGotItems.Property2 == objectItem.Property2);
					Assert.IsTrue(itemInGotItems.Property3 == objectItem.Property3);
					Assert.IsTrue(itemInGotItems.Property4 == objectItem.Property4);
					Assert.IsTrue(itemInGotItems.Property5 == objectItem.Property5);
					Assert.IsTrue(itemInGotItems.Property6 == objectItem.Property6);
				}
				//
			}
		}
	}

	[TestMethod]
	public void ItemCacheIndexByOperation_TryGet_Test()
	{
		var itemIndexWith1Key = new ItemIndexWith1Key<TestItemForIndexTest, int>(
			(item) => item.Property1);
		var itemIndexWith2Keys = new ItemIndexWith2Keys<TestItemForIndexTest, int, int>(
			(item) => item.Property1,
			(item) => item.Property2);
		var itemIndexWith3Keys = new ItemIndexWith3Keys<TestItemForIndexTest, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3);
		var itemIndexWith4Keys = new ItemIndexWith4Keys<TestItemForIndexTest, int, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3,
			(item) => item.Property4);
		var itemIndexWith5Keys = new ItemIndexWith5Keys<TestItemForIndexTest, int, int, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3,
			(item) => item.Property4,
			(item) => item.Property5);
		var itemIndexWith6Keys = new ItemIndexWith6Keys<TestItemForIndexTest, int, int, int, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3,
			(item) => item.Property4,
			(item) => item.Property5,
			(item) => item.Property6);

		////////////////////////////////////////////////

		var itemsIndexWith1Key = new ItemsIndexWith1Key<TestItemForIndexTest, int>(
			(item) => item.Property1,
			(itemA, itemB) => itemA.Id == itemB.Id,
			(itemA, itemB) => itemB.Id.CompareTo(itemA.Id));
		var itemsIndexWith2Keys = new ItemsIndexWith2Keys<TestItemForIndexTest, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(itemA, itemB) => itemA.Id == itemB.Id,
			(itemA, itemB) => itemB.Id.CompareTo(itemA.Id));
		var itemsIndexWith3Keys = new ItemsIndexWith3Keys<TestItemForIndexTest, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3,
			(itemA, itemB) => itemA.Id == itemB.Id,
			(itemA, itemB) => itemB.Id.CompareTo(itemA.Id));
		var itemsIndexWith4Keys = new ItemsIndexWith4Keys<TestItemForIndexTest, int, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3,
			(item) => item.Property4,
			(itemA, itemB) => itemA.Id == itemB.Id,
			(itemA, itemB) => itemB.Id.CompareTo(itemA.Id));
		var itemsIndexWith5Keys = new ItemsIndexWith5Keys<TestItemForIndexTest, int, int, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3,
			(item) => item.Property4,
			(item) => item.Property5,
			(itemA, itemB) => itemA.Id == itemB.Id,
			(itemA, itemB) => itemB.Id.CompareTo(itemA.Id));
		var itemsIndexWith6Keys = new ItemsIndexWith6Keys<TestItemForIndexTest, int, int, int, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3,
			(item) => item.Property4,
			(item) => item.Property5,
			(item) => item.Property6,
			(itemA, itemB) => itemA.Id == itemB.Id,
			(itemA, itemB) => itemB.Id.CompareTo(itemA.Id));

		////////////////////////////////////////////////

		var itemsCache = new ItemsCacheAsync<int, TestItemForIndexTest, TestItemForIndexTest>(
			async
			(itemId, itemSpecified) =>
			{
				return await Task.FromResult(itemSpecified);
			},
			null,
			null,
			[
				itemIndexWith1Key,
				itemIndexWith2Keys,
				itemIndexWith3Keys,
				itemIndexWith4Keys,
				itemIndexWith5Keys,
				itemIndexWith6Keys,
				//
				itemsIndexWith1Key,
				itemsIndexWith2Keys,
				itemsIndexWith3Keys,
				itemsIndexWith4Keys,
				itemsIndexWith5Keys,
				itemsIndexWith6Keys
			]);
		var testItems = new TestItemForIndexTest[100];
		var itemsGroupByProperty_1 = new Dictionary<string, List<TestItemForIndexTest>>();
		var itemsGroupByProperty_1_2 = new Dictionary<string, List<TestItemForIndexTest>>();
		var itemsGroupByProperty_1_2_3 = new Dictionary<string, List<TestItemForIndexTest>>();
		var itemsGroupByProperty_1_2_3_4 = new Dictionary<string, List<TestItemForIndexTest>>();
		var itemsGroupByProperty_1_2_3_4_5 = new Dictionary<string, List<TestItemForIndexTest>>();
		var itemsGroupByProperty_1_2_3_4_5_6 = new Dictionary<string, List<TestItemForIndexTest>>();
		for (var testItemIndex = 0;
			testItemIndex < testItems.Length;
			testItemIndex++)
		{
			var testItem = new TestItemForIndexTest(
				(testItemIndex + 1),
				Random.Shared.Next(testItems.Length),
				Random.Shared.Next(testItems.Length),
				Random.Shared.Next(testItems.Length),
				Random.Shared.Next(testItems.Length),
				Random.Shared.Next(testItems.Length),
				Random.Shared.Next(testItems.Length));
			testItems[testItemIndex] = testItem;
			////////////////////////////////////////////////
			// !!!
			itemsCache.TryGetAsync(testItem.Id, true, testItem).Wait();
			// !!!
			////////////////////////////////////////////////


			var itemKey = StringUtil.StringWithInts([
					testItem.Property1]);
			if (!itemsGroupByProperty_1.TryGetValue(
				itemKey,
				out var items))
			{
				items = [];
				itemsGroupByProperty_1.Add(itemKey, items);
			}
			items.Add(testItem);
			////
			itemKey = StringUtil.StringWithInts([
					testItem.Property1,
				testItem.Property2]);
			if (!itemsGroupByProperty_1_2.TryGetValue(
				itemKey,
				out items))
			{
				items = [];
				itemsGroupByProperty_1_2.Add(itemKey, items);
			}
			items.Add(testItem);
			////
			itemKey = StringUtil.StringWithInts([
					testItem.Property1,
				testItem.Property2,
				testItem.Property3]);
			if (!itemsGroupByProperty_1_2_3.TryGetValue(
				itemKey,
				out items))
			{
				items = [];
				itemsGroupByProperty_1_2_3.Add(itemKey, items);
			}
			items.Add(testItem);
			////
			itemKey = StringUtil.StringWithInts([
					testItem.Property1,
				testItem.Property2,
				testItem.Property3,
				testItem.Property4]);
			if (!itemsGroupByProperty_1_2_3_4.TryGetValue(
				itemKey,
				out items))
			{
				items = [];
				itemsGroupByProperty_1_2_3_4.Add(itemKey, items);
			}
			items.Add(testItem);
			////
			itemKey = StringUtil.StringWithInts([
					testItem.Property1,
				testItem.Property2,
				testItem.Property3,
				testItem.Property4,
				testItem.Property5]);
			if (!itemsGroupByProperty_1_2_3_4_5.TryGetValue(
				itemKey,
				out items))
			{
				items = [];
				itemsGroupByProperty_1_2_3_4_5.Add(itemKey, items);
			}
			items.Add(testItem);
			////
			itemKey = StringUtil.StringWithInts([
					testItem.Property1,
				testItem.Property2,
				testItem.Property3,
				testItem.Property4,
				testItem.Property5,
				testItem.Property6]);
			if (!itemsGroupByProperty_1_2_3_4_5_6.TryGetValue(
				itemKey,
				out items))
			{
				items = [];
				itemsGroupByProperty_1_2_3_4_5_6.Add(itemKey, items);
			}
			items.Add(testItem);
		}

		////////////////////////////////////////////////
		////////////////////////////////////////////////
		////////////////////////////////////////////////
		// 等待 1 秒。
		Task.Delay(1000).Wait();
		////////////////////////////////////////////////
		////////////////////////////////////////////////
		////////////////////////////////////////////////

		Assert.IsTrue(itemsCache.Keys.Count == testItems.Length);

		// itemsGroupByProperty_1
		foreach (var itemWithProperty_1 in itemsGroupByProperty_1)
		{
			var objectItems = itemWithProperty_1.Value;
			Assert.IsTrue(int.TryParse(itemWithProperty_1.Key, out var itemKey));
			var gotItem = itemIndexWith1Key.GetItem(itemKey);
			{
				//
				Assert.IsTrue(gotItem != null);
				Assert.IsTrue(objectItems.Find((item) => item.Id == gotItem.Id) != null);
				//
			}
			var gotItems = itemsIndexWith1Key.GetItems(itemKey);
			{
				//
				Assert.IsTrue(gotItems?.Length == objectItems.Count);
				var objectItemProperty1 = objectItems.First().Property1;
				foreach (var itemInGotItems in gotItems)
				{
					Assert.IsTrue(itemInGotItems.Property1 == objectItemProperty1);
				}
				//
			}
		}
		// itemsGroupByProperty_1_2
		foreach (var itemWithProperty_1_2 in itemsGroupByProperty_1_2)
		{
			var objectItems = itemWithProperty_1_2.Value;
			var itemKeys = itemWithProperty_1_2.Key.ToIntArray();
			{
				Assert.IsTrue(itemKeys.Length == 2);
			}
			var gotItem = itemIndexWith2Keys.GetItem(itemKeys[0], itemKeys[1]);
			{
				//
				Assert.IsTrue(gotItem != null);
				Assert.IsTrue(objectItems.Find((item) => item.Id == gotItem.Id) != null);
				//
			}
			var gotItems = itemsIndexWith2Keys.GetItems(itemKeys[0], itemKeys[1]);
			{
				//
				Assert.IsTrue(gotItems?.Length == objectItems.Count);
				var objectItemProperty1 = objectItems.First().Property1;
				var objectItemProperty2 = objectItems.First().Property2;
				foreach (var itemInGotItems in gotItems)
				{
					Assert.IsTrue(itemInGotItems.Property1 == objectItemProperty1);
					Assert.IsTrue(itemInGotItems.Property2 == objectItemProperty2);
				}
				//
			}
		}
		// itemsGroupByProperty_1_2_3
		foreach (var itemWithProperty_1_2_3 in itemsGroupByProperty_1_2_3)
		{
			var objectItems = itemWithProperty_1_2_3.Value;
			var itemKeys = itemWithProperty_1_2_3.Key.ToIntArray();
			{
				Assert.IsTrue(itemKeys.Length == 3);
			}
			var gotItem = itemIndexWith3Keys.GetItem(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2]);
			{
				//
				Assert.IsTrue(gotItem != null);
				Assert.IsTrue(objectItems.Find((item) => item.Id == gotItem.Id) != null);
				//
			}
			var gotItems = itemsIndexWith3Keys.GetItems(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2]);
			{
				//
				Assert.IsTrue(gotItems?.Length == objectItems.Count);
				var objectItemProperty1 = objectItems.First().Property1;
				var objectItemProperty2 = objectItems.First().Property2;
				var objectItemProperty3 = objectItems.First().Property3;
				foreach (var itemInGotItems in gotItems)
				{
					Assert.IsTrue(itemInGotItems.Property1 == objectItemProperty1);
					Assert.IsTrue(itemInGotItems.Property2 == objectItemProperty2);
					Assert.IsTrue(itemInGotItems.Property3 == objectItemProperty3);
				}
				//
			}
		}
		// itemsGroupByProperty_1_2_3_4
		foreach (var itemWithProperty_1_2_3_4 in itemsGroupByProperty_1_2_3_4)
		{
			var objectItems = itemWithProperty_1_2_3_4.Value;
			var itemKeys = itemWithProperty_1_2_3_4.Key.ToIntArray();
			{
				Assert.IsTrue(itemKeys.Length == 4);
			}
			var gotItem = itemIndexWith4Keys.GetItem(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2],
				itemKeys[3]);
			{
				//
				Assert.IsTrue(gotItem != null);
				Assert.IsTrue(objectItems.Find((item) => item.Id == gotItem.Id) != null);
				//
			}
			var gotItems = itemsIndexWith4Keys.GetItems(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2],
				itemKeys[3]);
			{
				//
				Assert.IsTrue(gotItems?.Length == objectItems.Count);
				var objectItemProperty1 = objectItems.First().Property1;
				var objectItemProperty2 = objectItems.First().Property2;
				var objectItemProperty3 = objectItems.First().Property3;
				var objectItemProperty4 = objectItems.First().Property4;
				foreach (var itemInGotItems in gotItems)
				{
					Assert.IsTrue(itemInGotItems.Property1 == objectItemProperty1);
					Assert.IsTrue(itemInGotItems.Property2 == objectItemProperty2);
					Assert.IsTrue(itemInGotItems.Property3 == objectItemProperty3);
					Assert.IsTrue(itemInGotItems.Property4 == objectItemProperty4);
				}
				//
			}
		}
		// itemsGroupByProperty_1_2_3_4_5
		foreach (var itemWithProperty_1_2_3_4_5 in itemsGroupByProperty_1_2_3_4_5)
		{
			var objectItems = itemWithProperty_1_2_3_4_5.Value;
			var itemKeys = itemWithProperty_1_2_3_4_5.Key.ToIntArray();
			{
				Assert.IsTrue(itemKeys.Length == 5);
			}
			var gotItem = itemIndexWith5Keys.GetItem(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2],
				itemKeys[3],
				itemKeys[4]);
			{
				//
				Assert.IsTrue(gotItem != null);
				Assert.IsTrue(objectItems.Find((item) => item.Id == gotItem.Id) != null);
				//
			}
			var gotItems = itemsIndexWith5Keys.GetItems(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2],
				itemKeys[3],
				itemKeys[4]);
			{
				//
				Assert.IsTrue(gotItems?.Length == objectItems.Count);
				var objectItemProperty1 = objectItems.First().Property1;
				var objectItemProperty2 = objectItems.First().Property2;
				var objectItemProperty3 = objectItems.First().Property3;
				var objectItemProperty4 = objectItems.First().Property4;
				var objectItemProperty5 = objectItems.First().Property5;
				foreach (var itemInGotItems in gotItems)
				{
					Assert.IsTrue(itemInGotItems.Property1 == objectItemProperty1);
					Assert.IsTrue(itemInGotItems.Property2 == objectItemProperty2);
					Assert.IsTrue(itemInGotItems.Property3 == objectItemProperty3);
					Assert.IsTrue(itemInGotItems.Property4 == objectItemProperty4);
					Assert.IsTrue(itemInGotItems.Property5 == objectItemProperty5);
				}
				//
			}
		}
		// itemsGroupByProperty_1_2_3_4_5_6
		foreach (var itemWithProperty_1_2_3_4_5_6 in itemsGroupByProperty_1_2_3_4_5_6)
		{
			var objectItems = itemWithProperty_1_2_3_4_5_6.Value;
			var itemKeys = itemWithProperty_1_2_3_4_5_6.Key.ToIntArray();
			{
				Assert.IsTrue(itemKeys.Length == 6);
			}
			var gotItem = itemIndexWith6Keys.GetItem(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2],
				itemKeys[3],
				itemKeys[4],
				itemKeys[5]);
			{
				//
				Assert.IsTrue(gotItem != null);
				Assert.IsTrue(objectItems.Find((item) => item.Id == gotItem.Id) != null);
				//
			}
			var gotItems = itemsIndexWith6Keys.GetItems(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2],
				itemKeys[3],
				itemKeys[4],
				itemKeys[5]);
			{
				//
				Assert.IsTrue(gotItems?.Length == objectItems.Count);
				var objectItemProperty1 = objectItems.First().Property1;
				var objectItemProperty2 = objectItems.First().Property2;
				var objectItemProperty3 = objectItems.First().Property3;
				var objectItemProperty4 = objectItems.First().Property4;
				var objectItemProperty5 = objectItems.First().Property5;
				var objectItemProperty6 = objectItems.First().Property6;
				foreach (var itemInGotItems in gotItems)
				{
					Assert.IsTrue(itemInGotItems.Property1 == objectItemProperty1);
					Assert.IsTrue(itemInGotItems.Property2 == objectItemProperty2);
					Assert.IsTrue(itemInGotItems.Property3 == objectItemProperty3);
					Assert.IsTrue(itemInGotItems.Property4 == objectItemProperty4);
					Assert.IsTrue(itemInGotItems.Property5 == objectItemProperty5);
					Assert.IsTrue(itemInGotItems.Property6 == objectItemProperty6);
				}
				//
			}
		}
	}

	[TestMethod]
	public void ItemCacheIndexByOperation_Add_Test()
	{
		var itemIndexWith1Key = new ItemIndexWith1Key<TestItemForIndexTest, int>(
			(item) => item.Property1);
		var itemIndexWith2Keys = new ItemIndexWith2Keys<TestItemForIndexTest, int, int>(
			(item) => item.Property1,
			(item) => item.Property2);
		var itemIndexWith3Keys = new ItemIndexWith3Keys<TestItemForIndexTest, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3);
		var itemIndexWith4Keys = new ItemIndexWith4Keys<TestItemForIndexTest, int, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3,
			(item) => item.Property4);
		var itemIndexWith5Keys = new ItemIndexWith5Keys<TestItemForIndexTest, int, int, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3,
			(item) => item.Property4,
			(item) => item.Property5);
		var itemIndexWith6Keys = new ItemIndexWith6Keys<TestItemForIndexTest, int, int, int, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3,
			(item) => item.Property4,
			(item) => item.Property5,
			(item) => item.Property6);

		////////////////////////////////////////////////

		var itemsIndexWith1Key = new ItemsIndexWith1Key<TestItemForIndexTest, int>(
			(item) => item.Property1,
			(itemA, itemB) => itemA.Id == itemB.Id,
			(itemA, itemB) => itemB.Id.CompareTo(itemA.Id));
		var itemsIndexWith2Keys = new ItemsIndexWith2Keys<TestItemForIndexTest, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(itemA, itemB) => itemA.Id == itemB.Id,
			(itemA, itemB) => itemB.Id.CompareTo(itemA.Id));
		var itemsIndexWith3Keys = new ItemsIndexWith3Keys<TestItemForIndexTest, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3,
			(itemA, itemB) => itemA.Id == itemB.Id,
			(itemA, itemB) => itemB.Id.CompareTo(itemA.Id));
		var itemsIndexWith4Keys = new ItemsIndexWith4Keys<TestItemForIndexTest, int, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3,
			(item) => item.Property4,
			(itemA, itemB) => itemA.Id == itemB.Id,
			(itemA, itemB) => itemB.Id.CompareTo(itemA.Id));
		var itemsIndexWith5Keys = new ItemsIndexWith5Keys<TestItemForIndexTest, int, int, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3,
			(item) => item.Property4,
			(item) => item.Property5,
			(itemA, itemB) => itemA.Id == itemB.Id,
			(itemA, itemB) => itemB.Id.CompareTo(itemA.Id));
		var itemsIndexWith6Keys = new ItemsIndexWith6Keys<TestItemForIndexTest, int, int, int, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3,
			(item) => item.Property4,
			(item) => item.Property5,
			(item) => item.Property6,
			(itemA, itemB) => itemA.Id == itemB.Id,
			(itemA, itemB) => itemB.Id.CompareTo(itemA.Id));

		////////////////////////////////////////////////

		var itemsCache = new ItemsCacheAsync<int, TestItemForIndexTest, TestItemForIndexTest>(
			async (itemId, itemSpecified) =>
			{
				return await Task.FromResult(itemSpecified);
			},
			null,
			null,
			[
				itemIndexWith1Key,
				itemIndexWith2Keys,
				itemIndexWith3Keys,
				itemIndexWith4Keys,
				itemIndexWith5Keys,
				itemIndexWith6Keys,
				//
				itemsIndexWith1Key,
				itemsIndexWith2Keys,
				itemsIndexWith3Keys,
				itemsIndexWith4Keys,
				itemsIndexWith5Keys,
				itemsIndexWith6Keys
				]);
		var testItems = new TestItemForIndexTest[100];
		var itemsGroupByProperty_1 = new Dictionary<string, List<TestItemForIndexTest>>();
		var itemsGroupByProperty_1_2 = new Dictionary<string, List<TestItemForIndexTest>>();
		var itemsGroupByProperty_1_2_3 = new Dictionary<string, List<TestItemForIndexTest>>();
		var itemsGroupByProperty_1_2_3_4 = new Dictionary<string, List<TestItemForIndexTest>>();
		var itemsGroupByProperty_1_2_3_4_5 = new Dictionary<string, List<TestItemForIndexTest>>();
		var itemsGroupByProperty_1_2_3_4_5_6 = new Dictionary<string, List<TestItemForIndexTest>>();
		for (var testItemIndex = 0;
			testItemIndex < testItems.Length;
			testItemIndex++)
		{
			var testItem = new TestItemForIndexTest(
				(testItemIndex + 1),
				Random.Shared.Next(testItems.Length),
				Random.Shared.Next(testItems.Length),
				Random.Shared.Next(testItems.Length),
				Random.Shared.Next(testItems.Length),
				Random.Shared.Next(testItems.Length),
				Random.Shared.Next(testItems.Length));
			testItems[testItemIndex] = testItem;
			////////////////////////////////////////////////
			// !!!
			itemsCache.AddAsync(testItem.Id, testItem).Wait();
			// !!!
			////////////////////////////////////////////////


			var itemKey = StringUtil.StringWithInts([
					testItem.Property1]);
			if (!itemsGroupByProperty_1.TryGetValue(
				itemKey,
				out var items))
			{
				items = [];
				itemsGroupByProperty_1.Add(itemKey, items);
			}
			items.Add(testItem);
			////
			itemKey = StringUtil.StringWithInts([
					testItem.Property1,
					testItem.Property2]);
			if (!itemsGroupByProperty_1_2.TryGetValue(
				itemKey,
				out items))
			{
				items = [];
				itemsGroupByProperty_1_2.Add(itemKey, items);
			}
			items.Add(testItem);
			////
			itemKey = StringUtil.StringWithInts([
					testItem.Property1,
					testItem.Property2,
					testItem.Property3]);
			if (!itemsGroupByProperty_1_2_3.TryGetValue(
				itemKey,
				out items))
			{
				items = [];
				itemsGroupByProperty_1_2_3.Add(itemKey, items);
			}
			items.Add(testItem);
			////
			itemKey = StringUtil.StringWithInts([
					testItem.Property1,
					testItem.Property2,
					testItem.Property3,
					testItem.Property4]);
			if (!itemsGroupByProperty_1_2_3_4.TryGetValue(
				itemKey,
				out items))
			{
				items = [];
				itemsGroupByProperty_1_2_3_4.Add(itemKey, items);
			}
			items.Add(testItem);
			////
			itemKey = StringUtil.StringWithInts([
					testItem.Property1,
					testItem.Property2,
					testItem.Property3,
					testItem.Property4,
					testItem.Property5]);
			if (!itemsGroupByProperty_1_2_3_4_5.TryGetValue(
				itemKey,
				out items))
			{
				items = [];
				itemsGroupByProperty_1_2_3_4_5.Add(itemKey, items);
			}
			items.Add(testItem);
			////
			itemKey = StringUtil.StringWithInts([
					testItem.Property1,
					testItem.Property2,
					testItem.Property3,
					testItem.Property4,
					testItem.Property5,
					testItem.Property6]);
			if (!itemsGroupByProperty_1_2_3_4_5_6.TryGetValue(
				itemKey,
				out items))
			{
				items = [];
				itemsGroupByProperty_1_2_3_4_5_6.Add(itemKey, items);
			}
			items.Add(testItem);
		}

		////////////////////////////////////////////////
		////////////////////////////////////////////////
		////////////////////////////////////////////////

		Assert.IsTrue(itemsCache.Keys.Count == testItems.Length);

		// itemsGroupByProperty_1
		foreach (var itemWithProperty_1 in itemsGroupByProperty_1)
		{
			var objectItems = itemWithProperty_1.Value;
			var objectItem = objectItems[^1];
			Assert.IsTrue(int.TryParse(itemWithProperty_1.Key, out var itemKey));
			var gotItem = itemIndexWith1Key.GetItem(itemKey);
			{
				//
				Assert.IsTrue(gotItem != null);
				Assert.IsTrue(gotItem.Id == objectItem.Id);
				//
			}
			var gotItems = itemsIndexWith1Key.GetItems(itemKey);
			{
				//
				Assert.IsTrue(gotItems?.Length == objectItems.Count);
				foreach (var itemInGotItems in gotItems)
				{
					Assert.IsTrue(itemInGotItems.Property1 == objectItem.Property1);
				}
				//
			}
		}
		// itemsGroupByProperty_1_2
		foreach (var itemWithProperty_1_2 in itemsGroupByProperty_1_2)
		{
			var objectItems = itemWithProperty_1_2.Value;
			var objectItem = objectItems[^1];
			var itemKeys = itemWithProperty_1_2.Key.ToIntArray();
			{
				Assert.IsTrue(itemKeys.Length == 2);
			}
			var gotItem = itemIndexWith2Keys.GetItem(itemKeys[0], itemKeys[1]);
			{
				//
				Assert.IsTrue(gotItem != null);
				Assert.IsTrue(gotItem.Id == objectItem.Id);
				//
			}
			var gotItems = itemsIndexWith2Keys.GetItems(itemKeys[0], itemKeys[1]);
			{
				//
				Assert.IsTrue(gotItems?.Length == objectItems.Count);
				foreach (var itemInGotItems in gotItems)
				{
					Assert.IsTrue(itemInGotItems.Property1 == objectItem.Property1);
					Assert.IsTrue(itemInGotItems.Property2 == objectItem.Property2);
				}
				//
			}
		}
		// itemsGroupByProperty_1_2_3
		foreach (var itemWithProperty_1_2_3 in itemsGroupByProperty_1_2_3)
		{
			var objectItems = itemWithProperty_1_2_3.Value;
			var objectItem = objectItems[^1];
			var itemKeys = itemWithProperty_1_2_3.Key.ToIntArray();
			{
				Assert.IsTrue(itemKeys.Length == 3);
			}
			var gotItem = itemIndexWith3Keys.GetItem(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2]);
			{
				//
				Assert.IsTrue(gotItem != null);
				Assert.IsTrue(gotItem.Id == objectItem.Id);
				//
			}
			var gotItems = itemsIndexWith3Keys.GetItems(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2]);
			{
				//
				Assert.IsTrue(gotItems?.Length == objectItems.Count);
				foreach (var itemInGotItems in gotItems)
				{
					Assert.IsTrue(itemInGotItems.Property1 == objectItem.Property1);
					Assert.IsTrue(itemInGotItems.Property2 == objectItem.Property2);
					Assert.IsTrue(itemInGotItems.Property3 == objectItem.Property3);
				}
				//
			}
		}
		// itemsGroupByProperty_1_2_3_4
		foreach (var itemWithProperty_1_2_3_4 in itemsGroupByProperty_1_2_3_4)
		{
			var objectItems = itemWithProperty_1_2_3_4.Value;
			var objectItem = objectItems[^1];
			var itemKeys = itemWithProperty_1_2_3_4.Key.ToIntArray();
			{
				Assert.IsTrue(itemKeys.Length == 4);
			}
			var gotItem = itemIndexWith4Keys.GetItem(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2],
				itemKeys[3]);
			{
				//
				Assert.IsTrue(gotItem != null);
				Assert.IsTrue(gotItem.Id == objectItem.Id);
				//
			}
			var gotItems = itemsIndexWith4Keys.GetItems(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2],
				itemKeys[3]);
			{
				//
				Assert.IsTrue(gotItems?.Length == objectItems.Count);
				foreach (var itemInGotItems in gotItems)
				{
					Assert.IsTrue(itemInGotItems.Property1 == objectItem.Property1);
					Assert.IsTrue(itemInGotItems.Property2 == objectItem.Property2);
					Assert.IsTrue(itemInGotItems.Property3 == objectItem.Property3);
					Assert.IsTrue(itemInGotItems.Property4 == objectItem.Property4);
				}
				//
			}
		}
		// itemsGroupByProperty_1_2_3_4_5
		foreach (var itemWithProperty_1_2_3_4_5 in itemsGroupByProperty_1_2_3_4_5)
		{
			var objectItems = itemWithProperty_1_2_3_4_5.Value;
			var objectItem = objectItems[^1];
			var itemKeys = itemWithProperty_1_2_3_4_5.Key.ToIntArray();
			{
				Assert.IsTrue(itemKeys.Length == 5);
			}
			var gotItem = itemIndexWith5Keys.GetItem(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2],
				itemKeys[3],
				itemKeys[4]);
			{
				//
				Assert.IsTrue(gotItem != null);
				Assert.IsTrue(gotItem.Id == objectItem.Id);
				//
			}
			var gotItems = itemsIndexWith5Keys.GetItems(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2],
				itemKeys[3],
				itemKeys[4]);
			{
				//
				Assert.IsTrue(gotItems?.Length == objectItems.Count);
				foreach (var itemInGotItems in gotItems)
				{
					Assert.IsTrue(itemInGotItems.Property1 == objectItem.Property1);
					Assert.IsTrue(itemInGotItems.Property2 == objectItem.Property2);
					Assert.IsTrue(itemInGotItems.Property3 == objectItem.Property3);
					Assert.IsTrue(itemInGotItems.Property4 == objectItem.Property4);
					Assert.IsTrue(itemInGotItems.Property5 == objectItem.Property5);
				}
				//
			}
		}
		// itemsGroupByProperty_1_2_3_4_5_6
		foreach (var itemWithProperty_1_2_3_4_5_6 in itemsGroupByProperty_1_2_3_4_5_6)
		{
			var objectItems = itemWithProperty_1_2_3_4_5_6.Value;
			var objectItem = objectItems[^1];
			var itemKeys = itemWithProperty_1_2_3_4_5_6.Key.ToIntArray();
			{
				Assert.IsTrue(itemKeys.Length == 6);
			}
			var gotItem = itemIndexWith6Keys.GetItem(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2],
				itemKeys[3],
				itemKeys[4],
				itemKeys[5]);
			{
				//
				Assert.IsTrue(gotItem != null);
				Assert.IsTrue(gotItem.Id == objectItem.Id);
				//
			}
			var gotItems = itemsIndexWith6Keys.GetItems(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2],
				itemKeys[3],
				itemKeys[4],
				itemKeys[5]);
			{
				//
				Assert.IsTrue(gotItems?.Length == objectItems.Count);
				foreach (var itemInGotItems in gotItems)
				{
					Assert.IsTrue(itemInGotItems.Property1 == objectItem.Property1);
					Assert.IsTrue(itemInGotItems.Property2 == objectItem.Property2);
					Assert.IsTrue(itemInGotItems.Property3 == objectItem.Property3);
					Assert.IsTrue(itemInGotItems.Property4 == objectItem.Property4);
					Assert.IsTrue(itemInGotItems.Property5 == objectItem.Property5);
					Assert.IsTrue(itemInGotItems.Property6 == objectItem.Property6);
				}
				//
			}
		}
	}

	[TestMethod]
	public void ItemCacheIndexByOperation_Update_Remove_Test()
	{
		var itemIndexWith1Key = new ItemIndexWith1Key<TestItemForIndexTest, int>(
			(item) => item.Property1);
		var itemIndexWith2Keys = new ItemIndexWith2Keys<TestItemForIndexTest, int, int>(
			(item) => item.Property1,
			(item) => item.Property2);
		var itemIndexWith3Keys = new ItemIndexWith3Keys<TestItemForIndexTest, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3);
		var itemIndexWith4Keys = new ItemIndexWith4Keys<TestItemForIndexTest, int, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3,
			(item) => item.Property4);
		var itemIndexWith5Keys = new ItemIndexWith5Keys<TestItemForIndexTest, int, int, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3,
			(item) => item.Property4,
			(item) => item.Property5);
		var itemIndexWith6Keys = new ItemIndexWith6Keys<TestItemForIndexTest, int, int, int, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3,
			(item) => item.Property4,
			(item) => item.Property5,
			(item) => item.Property6);

		////////////////////////////////////////////////

		var itemsIndexWith1Key = new ItemsIndexWith1Key<TestItemForIndexTest, int>(
			(item) => item.Property1,
			(itemA, itemB) => itemA.Id == itemB.Id,
			(itemA, itemB) => itemB.Id.CompareTo(itemA.Id));
		var itemsIndexWith2Keys = new ItemsIndexWith2Keys<TestItemForIndexTest, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(itemA, itemB) => itemA.Id == itemB.Id,
			(itemA, itemB) => itemB.Id.CompareTo(itemA.Id));
		var itemsIndexWith3Keys = new ItemsIndexWith3Keys<TestItemForIndexTest, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3,
			(itemA, itemB) => itemA.Id == itemB.Id,
			(itemA, itemB) => itemB.Id.CompareTo(itemA.Id));
		var itemsIndexWith4Keys = new ItemsIndexWith4Keys<TestItemForIndexTest, int, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3,
			(item) => item.Property4,
			(itemA, itemB) => itemA.Id == itemB.Id,
			(itemA, itemB) => itemB.Id.CompareTo(itemA.Id));
		var itemsIndexWith5Keys = new ItemsIndexWith5Keys<TestItemForIndexTest, int, int, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3,
			(item) => item.Property4,
			(item) => item.Property5,
			(itemA, itemB) => itemA.Id == itemB.Id,
			(itemA, itemB) => itemB.Id.CompareTo(itemA.Id));
		var itemsIndexWith6Keys = new ItemsIndexWith6Keys<TestItemForIndexTest, int, int, int, int, int, int>(
			(item) => item.Property1,
			(item) => item.Property2,
			(item) => item.Property3,
			(item) => item.Property4,
			(item) => item.Property5,
			(item) => item.Property6,
			(itemA, itemB) => itemA.Id == itemB.Id,
			(itemA, itemB) => itemB.Id.CompareTo(itemA.Id));

		////////////////////////////////////////////////

		var itemsCache = new ItemsCacheAsync<int, TestItemForIndexTest, TestItemForIndexTest>(
			async (itemId, itemSpecified) =>
			{
				return await Task.FromResult(itemSpecified);
			},
			null,
			null,
			[
				itemIndexWith1Key,
				itemIndexWith2Keys,
				itemIndexWith3Keys,
				itemIndexWith4Keys,
				itemIndexWith5Keys,
				itemIndexWith6Keys,
				//
				itemsIndexWith1Key,
				itemsIndexWith2Keys,
				itemsIndexWith3Keys,
				itemsIndexWith4Keys,
				itemsIndexWith5Keys,
				itemsIndexWith6Keys
				]);

		////////////////////////////////////////////////
		////////////////////////////////////////////////
		////////////////////////////////////////////////

		itemsCache.AddAsync(
			1,
			new(1, 1, 2, 3, 4, 5, 6))
			.Wait();
		{
			Assert.IsTrue(itemIndexWith1Key.GetItem(1)!.Id == 1);
			Assert.IsTrue(itemIndexWith2Keys.GetItem(1, 2)!.Id == 1);
			Assert.IsTrue(itemIndexWith3Keys.GetItem(1, 2, 3)!.Id == 1);
			Assert.IsTrue(itemIndexWith4Keys.GetItem(1, 2, 3, 4)!.Id == 1);
			Assert.IsTrue(itemIndexWith5Keys.GetItem(1, 2, 3, 4, 5)!.Id == 1);
			Assert.IsTrue(itemIndexWith6Keys.GetItem(1, 2, 3, 4, 5, 6)!.Id == 1);
		}
		itemsCache.UpdateAsync(
			1,
			new(1, 11, 12, 13, 14, 15, 16))
			.Wait();
		{
			Assert.IsTrue(itemIndexWith1Key.GetItem(1) == null);
			Assert.IsTrue(itemIndexWith2Keys.GetItem(1, 2) == null);
			Assert.IsTrue(itemIndexWith3Keys.GetItem(1, 2, 3) == null);
			Assert.IsTrue(itemIndexWith4Keys.GetItem(1, 2, 3, 4) == null);
			Assert.IsTrue(itemIndexWith5Keys.GetItem(1, 2, 3, 4, 5) == null);
			Assert.IsTrue(itemIndexWith6Keys.GetItem(1, 2, 3, 4, 5, 6) == null);
			//
			Assert.IsTrue(itemIndexWith1Key.GetItem(11)!.Id == 1);
			Assert.IsTrue(itemIndexWith2Keys.GetItem(11, 12)!.Id == 1);
			Assert.IsTrue(itemIndexWith3Keys.GetItem(11, 12, 13)!.Id == 1);
			Assert.IsTrue(itemIndexWith4Keys.GetItem(11, 12, 13, 14)!.Id == 1);
			Assert.IsTrue(itemIndexWith5Keys.GetItem(11, 12, 13, 14, 15)!.Id == 1);
			Assert.IsTrue(itemIndexWith6Keys.GetItem(11, 12, 13, 14, 15, 16)!.Id == 1);
		}
		itemsCache.RemoveAsync(1).Wait();
		{
			Assert.IsTrue(itemIndexWith1Key.GetItem(11) == null);
			Assert.IsTrue(itemIndexWith2Keys.GetItem(11, 12) == null);
			Assert.IsTrue(itemIndexWith3Keys.GetItem(11, 12, 13) == null);
			Assert.IsTrue(itemIndexWith4Keys.GetItem(11, 12, 13, 14) == null);
			Assert.IsTrue(itemIndexWith5Keys.GetItem(11, 12, 13, 14, 15) == null);
			Assert.IsTrue(itemIndexWith6Keys.GetItem(11, 12, 13, 14, 15, 16) == null);
		}

		////////////////////////////////////////////////
		itemsCache.Clear();
		////////////////////////////////////////////////

		itemsCache.AddAsync(
			1,
			new(1, 1, 2, 3, 4, 5, 6))
			.Wait();
		itemsCache.AddAsync(
			2,
			new(2, 1, 2, 3, 4, 5, 6))
			.Wait();
		{
			Assert.IsTrue(itemIndexWith1Key.GetItem(1)!.Id == 2);
			Assert.IsTrue(itemIndexWith2Keys.GetItem(1, 2)!.Id == 2);
			Assert.IsTrue(itemIndexWith3Keys.GetItem(1, 2, 3)!.Id == 2);
			Assert.IsTrue(itemIndexWith4Keys.GetItem(1, 2, 3, 4)!.Id == 2);
			Assert.IsTrue(itemIndexWith5Keys.GetItem(1, 2, 3, 4, 5)!.Id == 2);
			Assert.IsTrue(itemIndexWith6Keys.GetItem(1, 2, 3, 4, 5, 6)!.Id == 2);
			//
			Assert.IsTrue(itemsIndexWith1Key.GetItems(1)!.Length == 2);
			Assert.IsTrue(itemsIndexWith2Keys.GetItems(1, 2)!.Length == 2);
			Assert.IsTrue(itemsIndexWith3Keys.GetItems(1, 2, 3)!.Length == 2);
			Assert.IsTrue(itemsIndexWith4Keys.GetItems(1, 2, 3, 4)!.Length == 2);
			Assert.IsTrue(itemsIndexWith5Keys.GetItems(1, 2, 3, 4, 5)!.Length == 2);
			Assert.IsTrue(itemsIndexWith6Keys.GetItems(1, 2, 3, 4, 5, 6)!.Length == 2);

		}
		itemsCache.UpdateAsync(
			1,
			new(1, 11, 12, 13, 14, 15, 16))
			.Wait();
		{
			Assert.IsTrue(itemIndexWith1Key.GetItem(1) == null);
			Assert.IsTrue(itemIndexWith2Keys.GetItem(1, 2) == null);
			Assert.IsTrue(itemIndexWith3Keys.GetItem(1, 2, 3) == null);
			Assert.IsTrue(itemIndexWith4Keys.GetItem(1, 2, 3, 4) == null);
			Assert.IsTrue(itemIndexWith5Keys.GetItem(1, 2, 3, 4, 5) == null);
			Assert.IsTrue(itemIndexWith6Keys.GetItem(1, 2, 3, 4, 5, 6) == null);
			//
			Assert.IsTrue(itemIndexWith1Key.GetItem(11)!.Id == 1);
			Assert.IsTrue(itemIndexWith2Keys.GetItem(11, 12)!.Id == 1);
			Assert.IsTrue(itemIndexWith3Keys.GetItem(11, 12, 13)!.Id == 1);
			Assert.IsTrue(itemIndexWith4Keys.GetItem(11, 12, 13, 14)!.Id == 1);
			Assert.IsTrue(itemIndexWith5Keys.GetItem(11, 12, 13, 14, 15)!.Id == 1);
			Assert.IsTrue(itemIndexWith6Keys.GetItem(11, 12, 13, 14, 15, 16)!.Id == 1);
			//
			Assert.IsTrue(itemsIndexWith1Key.GetItems(1)!.Length == 1);
			Assert.IsTrue(itemsIndexWith2Keys.GetItems(1, 2)!.Length == 1);
			Assert.IsTrue(itemsIndexWith3Keys.GetItems(1, 2, 3)!.Length == 1);
			Assert.IsTrue(itemsIndexWith4Keys.GetItems(1, 2, 3, 4)!.Length == 1);
			Assert.IsTrue(itemsIndexWith5Keys.GetItems(1, 2, 3, 4, 5)!.Length == 1);
			Assert.IsTrue(itemsIndexWith6Keys.GetItems(1, 2, 3, 4, 5, 6)!.Length == 1);
		}
		itemsCache.RemoveAsync(1)
			.Wait();
		{
			Assert.IsTrue(itemIndexWith1Key.GetItem(11) == null);
			Assert.IsTrue(itemIndexWith2Keys.GetItem(11, 12) == null);
			Assert.IsTrue(itemIndexWith3Keys.GetItem(11, 12, 13) == null);
			Assert.IsTrue(itemIndexWith4Keys.GetItem(11, 12, 13, 14) == null);
			Assert.IsTrue(itemIndexWith5Keys.GetItem(11, 12, 13, 14, 15) == null);
			Assert.IsTrue(itemIndexWith6Keys.GetItem(11, 12, 13, 14, 15, 16) == null);
			//
			Assert.IsTrue(itemsIndexWith1Key.GetItems(1)!.Length == 1);
			Assert.IsTrue(itemsIndexWith2Keys.GetItems(1, 2)!.Length == 1);
			Assert.IsTrue(itemsIndexWith3Keys.GetItems(1, 2, 3)!.Length == 1);
			Assert.IsTrue(itemsIndexWith4Keys.GetItems(1, 2, 3, 4)!.Length == 1);
			Assert.IsTrue(itemsIndexWith5Keys.GetItems(1, 2, 3, 4, 5)!.Length == 1);
			Assert.IsTrue(itemsIndexWith6Keys.GetItems(1, 2, 3, 4, 5, 6)!.Length == 1);
		}
	}
}
