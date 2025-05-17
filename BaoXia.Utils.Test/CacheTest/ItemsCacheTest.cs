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
public class ItemsCacheTest
{
	////////////////////////////////////////////////
	// @测试方法
	////////////////////////////////////////////////

	[TestMethod]
	public void AddUpdateAndQueryTest_IntKey()
	{
		var itemId = 0;
		var tester = new ItemsCacheTester<int>(
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
		var tester = new ItemsCacheTester<int>(
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
	public void TryGetTest_IntKey()
	{
		var itemId = 0;
		var tester = new ItemsCacheTester<int>(
			"整数Key",
			() =>
			{
				return Interlocked.Increment(ref itemId);
			});
		// !!!
		tester.TryGetTest();
		// !!!
	}

	[TestMethod]
	public void AddUpdateAndQueryTest_StringKey()
	{
		var itemId = 0;
		var tester = new ItemsCacheTester<string>(
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
		var tester = new ItemsCacheTester<string>(
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
	public void TryGetTest_StringKey()
	{
		var itemId = 0;
		var tester = new ItemsCacheTester<string>(
			"字符串Key",
			() =>
			{
				return Interlocked.Increment(ref itemId).ToString();
			});
		// !!!
		tester.TryGetTest();
		// !!!
	}


	[TestMethod]
	public void InitializeWithNullTest()
	{
		var itemCache = new ItemsCache<string, object, object>(
			(key, _) =>
			{
				return true;
			},
			null,
			null,
			null);
		//Assert.IsTrue(true);
	}

	[TestMethod]
	public async Task AutoRemoveNoneReadCacheItems_AutoRemove()
	{
		var itemsCache = new ItemsCache<int, DateTime, object>(
			(id, _) =>
			{
				return DateTime.Now;
			},
			null,
			null,
			() => 1.0);
		for (var itemId = 1;
			itemId <= 100;
			itemId++)
		{
			itemsCache.Add(itemId, DateTime.Now);
		}

		await Task.Delay(3000);

		Assert.AreEqual(0, itemsCache.Count);
	}

	[TestMethod]
	public async Task AutoRemoveNoneReadCacheItems_AutoUpdateReadTime()
	{
		var itemsCache = new ItemsCache<int, DateTime, object>(
			(id, _) =>
			{
				return DateTime.Now;
			},
			null,
			null,
			() => 2.0);

		const int itemsCount = 100;
		for (var itemId = 1;
			itemId <= itemsCount;
			itemId++)
		{
			itemsCache.Add(itemId, DateTime.Now);
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
					_ = itemsCache.Get(itemId, null);
				}
			}
		});

		await Task.Delay(3000);
		retainTaskCancelSource.Cancel();

		Assert.AreEqual(itemsCount, itemsCache.Count);
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

		var itemsCache = new ItemsCache<int, TestItemForIndexTest, TestItemForIndexTest>(
			(itemId, itemSpecified) =>
			{
				return itemSpecified;
			},
			null,
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
			itemsCache.Get(testItem.Id, testItem);
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

		Assert.AreEqual(testItems.Length, itemsCache.Keys.Count);

		// itemsGroupByProperty_1
		foreach (var itemWithProperty_1 in itemsGroupByProperty_1)
		{
			var objectItems = itemWithProperty_1.Value;
			var objectItem = objectItems[^1];
			Assert.IsTrue(int.TryParse(itemWithProperty_1.Key, out var itemKey));
			var gotItem = itemIndexWith1Key.GetItem(itemKey);
			{
				//
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItem.Id, gotItem.Id);
				//
			}
			var gotItems = itemsIndexWith1Key.GetItems(itemKey);
			{
				//
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItems.Count, gotItems!.Length);
				foreach (var itemInGotItems in gotItems)
				{
					Assert.AreEqual(objectItem.Property1, itemInGotItems.Property1);
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
				Assert.AreEqual(2, itemKeys.Length);
			}
			var gotItem = itemIndexWith2Keys.GetItem(itemKeys[0], itemKeys[1]);
			{
				//
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItem.Id, gotItem.Id);
				//
			}
			var gotItems = itemsIndexWith2Keys.GetItems(itemKeys[0], itemKeys[1]);
			{
				//
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItems.Count, gotItems!.Length);
				foreach (var itemInGotItems in gotItems)
				{
					Assert.AreEqual(objectItem.Property1, itemInGotItems.Property1);
					Assert.AreEqual(objectItem.Property2, itemInGotItems.Property2);
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
				Assert.AreEqual(3, itemKeys.Length);
			}
			var gotItem = itemIndexWith3Keys.GetItem(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2]);
			{
				//
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItem.Id, gotItem.Id);
				//
			}
			var gotItems = itemsIndexWith3Keys.GetItems(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2]);
			{
				//
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItems.Count, gotItems!.Length);
				foreach (var itemInGotItems in gotItems)
				{
					Assert.AreEqual(objectItem.Property1, itemInGotItems.Property1);
					Assert.AreEqual(objectItem.Property2, itemInGotItems.Property2);
					Assert.AreEqual(objectItem.Property3, itemInGotItems.Property3);
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
				Assert.AreEqual(4, itemKeys.Length);
			}
			var gotItem = itemIndexWith4Keys.GetItem(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2],
				itemKeys[3]);
			{
				//
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItem.Id, gotItem.Id);
				//
			}
			var gotItems = itemsIndexWith4Keys.GetItems(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2],
				itemKeys[3]);
			{
				//
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItems.Count, gotItems!.Length);
				foreach (var itemInGotItems in gotItems)
				{
					Assert.AreEqual(objectItem.Property1, itemInGotItems.Property1);
					Assert.AreEqual(objectItem.Property2, itemInGotItems.Property2);
					Assert.AreEqual(objectItem.Property3, itemInGotItems.Property3);
					Assert.AreEqual(objectItem.Property4, itemInGotItems.Property4);
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
				Assert.AreEqual(5, itemKeys.Length);
			}
			var gotItem = itemIndexWith5Keys.GetItem(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2],
				itemKeys[3],
				itemKeys[4]);
			{
				//
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItem.Id, gotItem.Id);
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
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItems.Count, gotItems!.Length);
				foreach (var itemInGotItems in gotItems)
				{
					Assert.AreEqual(objectItem.Property1, itemInGotItems.Property1);
					Assert.AreEqual(objectItem.Property2, itemInGotItems.Property2);
					Assert.AreEqual(objectItem.Property3, itemInGotItems.Property3);
					Assert.AreEqual(objectItem.Property4, itemInGotItems.Property4);
					Assert.AreEqual(objectItem.Property5, itemInGotItems.Property5);
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
				Assert.AreEqual(6, itemKeys.Length);
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
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItem.Id, gotItem.Id);
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
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItems.Count, gotItems!.Length);
				foreach (var itemInGotItems in gotItems)
				{
					Assert.AreEqual(objectItem.Property1, itemInGotItems.Property1);
					Assert.AreEqual(objectItem.Property2, itemInGotItems.Property2);
					Assert.AreEqual(objectItem.Property3, itemInGotItems.Property3);
					Assert.AreEqual(objectItem.Property4, itemInGotItems.Property4);
					Assert.AreEqual(objectItem.Property5, itemInGotItems.Property5);
					Assert.AreEqual(objectItem.Property6, itemInGotItems.Property6);
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

		var itemsCache = new ItemsCache<int, TestItemForIndexTest, TestItemForIndexTest>(
			(itemId, itemSpecified) =>
			{
				return itemSpecified;
			},
			null,
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
			itemsCache.TryGet(testItem.Id, out _, true, testItem);
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

		Assert.AreEqual(testItems.Length, itemsCache.Keys.Count);

		// itemsGroupByProperty_1
		foreach (var itemWithProperty_1 in itemsGroupByProperty_1)
		{
			var objectItems = itemWithProperty_1.Value;
			Assert.IsTrue(int.TryParse(itemWithProperty_1.Key, out var itemKey));
			var gotItem = itemIndexWith1Key.GetItem(itemKey);
			{
				//
				Assert.IsNotNull(gotItem);
				Assert.IsNotNull(objectItems.Find((item) => item.Id == gotItem.Id));
				//
			}
			var gotItems = itemsIndexWith1Key.GetItems(itemKey);
			{
				//
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItems.Count, gotItems!.Length);
				var objectItemProperty1 = objectItems.First().Property1;
				foreach (var itemInGotItems in gotItems)
				{
					Assert.AreEqual(objectItemProperty1, itemInGotItems.Property1);
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
				Assert.AreEqual(2, itemKeys.Length);
			}
			var gotItem = itemIndexWith2Keys.GetItem(itemKeys[0], itemKeys[1]);
			{
				//
				Assert.IsNotNull(gotItem);
				Assert.IsNotNull(objectItems.Find((item) => item.Id == gotItem.Id));
				//
			}
			var gotItems = itemsIndexWith2Keys.GetItems(itemKeys[0], itemKeys[1]);
			{
				//
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItems.Count, gotItems!.Length);
				var objectItemProperty1 = objectItems.First().Property1;
				var objectItemProperty2 = objectItems.First().Property2;
				foreach (var itemInGotItems in gotItems)
				{
					Assert.AreEqual(objectItemProperty1, itemInGotItems.Property1);
					Assert.AreEqual(objectItemProperty2, itemInGotItems.Property2);
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
				Assert.AreEqual(3, itemKeys.Length);
			}
			var gotItem = itemIndexWith3Keys.GetItem(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2]);
			{
				//
				Assert.IsNotNull(gotItem);
				Assert.IsNotNull(objectItems.Find((item) => item.Id == gotItem.Id));
				//
			}
			var gotItems = itemsIndexWith3Keys.GetItems(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2]);
			{
				//
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItems.Count, gotItems!.Length);
				var objectItemProperty1 = objectItems.First().Property1;
				var objectItemProperty2 = objectItems.First().Property2;
				var objectItemProperty3 = objectItems.First().Property3;
				foreach (var itemInGotItems in gotItems)
				{
					Assert.AreEqual(objectItemProperty1, itemInGotItems.Property1);
					Assert.AreEqual(objectItemProperty2, itemInGotItems.Property2);
					Assert.AreEqual(objectItemProperty3, itemInGotItems.Property3);
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
				Assert.AreEqual(4, itemKeys.Length);
			}
			var gotItem = itemIndexWith4Keys.GetItem(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2],
				itemKeys[3]);
			{
				//
				Assert.IsNotNull(gotItem);
				Assert.IsNotNull(objectItems.Find((item) => item.Id == gotItem.Id));
				//
			}
			var gotItems = itemsIndexWith4Keys.GetItems(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2],
				itemKeys[3]);
			{
				//
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItems.Count, gotItems!.Length);
				var objectItemProperty1 = objectItems.First().Property1;
				var objectItemProperty2 = objectItems.First().Property2;
				var objectItemProperty3 = objectItems.First().Property3;
				var objectItemProperty4 = objectItems.First().Property4;
				foreach (var itemInGotItems in gotItems)
				{
					Assert.AreEqual(objectItemProperty1, itemInGotItems.Property1);
					Assert.AreEqual(objectItemProperty2, itemInGotItems.Property2);
					Assert.AreEqual(objectItemProperty3, itemInGotItems.Property3);
					Assert.AreEqual(objectItemProperty4, itemInGotItems.Property4);
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
				Assert.AreEqual(5, itemKeys.Length);
			}
			var gotItem = itemIndexWith5Keys.GetItem(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2],
				itemKeys[3],
				itemKeys[4]);
			{
				//
				Assert.IsNotNull(gotItem);
				Assert.IsNotNull(objectItems.Find((item) => item.Id == gotItem.Id));
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
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItems.Count, gotItems!.Length);
				var objectItemProperty1 = objectItems.First().Property1;
				var objectItemProperty2 = objectItems.First().Property2;
				var objectItemProperty3 = objectItems.First().Property3;
				var objectItemProperty4 = objectItems.First().Property4;
				var objectItemProperty5 = objectItems.First().Property5;
				foreach (var itemInGotItems in gotItems)
				{
					Assert.AreEqual(objectItemProperty1, itemInGotItems.Property1);
					Assert.AreEqual(objectItemProperty2, itemInGotItems.Property2);
					Assert.AreEqual(objectItemProperty3, itemInGotItems.Property3);
					Assert.AreEqual(objectItemProperty4, itemInGotItems.Property4);
					Assert.AreEqual(objectItemProperty5, itemInGotItems.Property5);
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
				Assert.AreEqual(6, itemKeys.Length);
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
				Assert.IsNotNull(gotItem);
				Assert.IsNotNull(objectItems.Find((item) => item.Id == gotItem.Id));
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
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItems.Count, gotItems!.Length);
				var objectItemProperty1 = objectItems.First().Property1;
				var objectItemProperty2 = objectItems.First().Property2;
				var objectItemProperty3 = objectItems.First().Property3;
				var objectItemProperty4 = objectItems.First().Property4;
				var objectItemProperty5 = objectItems.First().Property5;
				var objectItemProperty6 = objectItems.First().Property6;
				foreach (var itemInGotItems in gotItems)
				{
					Assert.AreEqual(objectItemProperty1, itemInGotItems.Property1);
					Assert.AreEqual(objectItemProperty2, itemInGotItems.Property2);
					Assert.AreEqual(objectItemProperty3, itemInGotItems.Property3);
					Assert.AreEqual(objectItemProperty4, itemInGotItems.Property4);
					Assert.AreEqual(objectItemProperty5, itemInGotItems.Property5);
					Assert.AreEqual(objectItemProperty6, itemInGotItems.Property6);
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

		var itemsCache = new ItemsCache<int, TestItemForIndexTest, TestItemForIndexTest>(
			(itemId, itemSpecified) =>
			{
				return itemSpecified;
			},
			null,
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
			itemsCache.Add(testItem.Id, testItem);
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

		Assert.AreEqual(testItems.Length, itemsCache.Keys.Count);

		// itemsGroupByProperty_1
		foreach (var itemWithProperty_1 in itemsGroupByProperty_1)
		{
			var objectItems = itemWithProperty_1.Value;
			var objectItem = objectItems[^1];
			Assert.IsTrue(int.TryParse(itemWithProperty_1.Key, out var itemKey));
			var gotItem = itemIndexWith1Key.GetItem(itemKey);
			{
				//
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItem.Id, gotItem.Id);
				//
			}
			var gotItems = itemsIndexWith1Key.GetItems(itemKey);
			{
				//
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItems.Count, gotItems!.Length);
				foreach (var itemInGotItems in gotItems)
				{
					Assert.AreEqual(objectItem.Property1, itemInGotItems.Property1);
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
				Assert.AreEqual(2, itemKeys.Length);
			}
			var gotItem = itemIndexWith2Keys.GetItem(itemKeys[0], itemKeys[1]);
			{
				//
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItem.Id, gotItem.Id);
				//
			}
			var gotItems = itemsIndexWith2Keys.GetItems(itemKeys[0], itemKeys[1]);
			{
				//
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItems.Count, gotItems!.Length);
				foreach (var itemInGotItems in gotItems)
				{
					Assert.AreEqual(objectItem.Property1, itemInGotItems.Property1);
					Assert.AreEqual(objectItem.Property2, itemInGotItems.Property2);
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
				Assert.AreEqual(3, itemKeys.Length);
			}
			var gotItem = itemIndexWith3Keys.GetItem(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2]);
			{
				//
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItem.Id, gotItem.Id);
				//
			}
			var gotItems = itemsIndexWith3Keys.GetItems(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2]);
			{
				//
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItems.Count, gotItems!.Length);
				foreach (var itemInGotItems in gotItems)
				{
					Assert.AreEqual(objectItem.Property1, itemInGotItems.Property1);
					Assert.AreEqual(objectItem.Property2, itemInGotItems.Property2);
					Assert.AreEqual(objectItem.Property3, itemInGotItems.Property3);
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
				Assert.AreEqual(4, itemKeys.Length);
			}
			var gotItem = itemIndexWith4Keys.GetItem(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2],
				itemKeys[3]);
			{
				//
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItem.Id, gotItem.Id);
				//
			}
			var gotItems = itemsIndexWith4Keys.GetItems(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2],
				itemKeys[3]);
			{
				//
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItems.Count, gotItems!.Length);
				foreach (var itemInGotItems in gotItems)
				{
					Assert.AreEqual(objectItem.Property1, itemInGotItems.Property1);
					Assert.AreEqual(objectItem.Property2, itemInGotItems.Property2);
					Assert.AreEqual(objectItem.Property3, itemInGotItems.Property3);
					Assert.AreEqual(objectItem.Property4, itemInGotItems.Property4);
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
				Assert.AreEqual(5, itemKeys.Length);
			}
			var gotItem = itemIndexWith5Keys.GetItem(
				itemKeys[0],
				itemKeys[1],
				itemKeys[2],
				itemKeys[3],
				itemKeys[4]);
			{
				//
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItem.Id, gotItem.Id);
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
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItems.Count, gotItems!.Length);
				foreach (var itemInGotItems in gotItems)
				{
					Assert.AreEqual(objectItem.Property1, itemInGotItems.Property1);
					Assert.AreEqual(objectItem.Property2, itemInGotItems.Property2);
					Assert.AreEqual(objectItem.Property3, itemInGotItems.Property3);
					Assert.AreEqual(objectItem.Property4, itemInGotItems.Property4);
					Assert.AreEqual(objectItem.Property5, itemInGotItems.Property5);
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
				Assert.AreEqual(6, itemKeys.Length);
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
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItem.Id, gotItem.Id);
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
				Assert.IsNotNull(gotItem);
				Assert.AreEqual(objectItems.Count, gotItems!.Length);
				foreach (var itemInGotItems in gotItems)
				{
					Assert.AreEqual(objectItem.Property1, itemInGotItems.Property1);
					Assert.AreEqual(objectItem.Property2, itemInGotItems.Property2);
					Assert.AreEqual(objectItem.Property3, itemInGotItems.Property3);
					Assert.AreEqual(objectItem.Property4, itemInGotItems.Property4);
					Assert.AreEqual(objectItem.Property5, itemInGotItems.Property5);
					Assert.AreEqual(objectItem.Property6, itemInGotItems.Property6);
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

		var itemsCache = new ItemsCache<int, TestItemForIndexTest, TestItemForIndexTest>(
			(itemId, itemSpecified) =>
			{
				return itemSpecified;
			},
			null,
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

		itemsCache.Add(
			1,
			new(1, 1, 2, 3, 4, 5, 6));
		{
			Assert.AreEqual(1, itemIndexWith1Key.GetItem(1)!.Id);
			Assert.AreEqual(1, itemIndexWith2Keys.GetItem(1, 2)!.Id);
			Assert.AreEqual(1, itemIndexWith3Keys.GetItem(1, 2, 3)!.Id);
			Assert.AreEqual(1, itemIndexWith4Keys.GetItem(1, 2, 3, 4)!.Id);
			Assert.AreEqual(1, itemIndexWith5Keys.GetItem(1, 2, 3, 4, 5)!.Id);
			Assert.AreEqual(1, itemIndexWith6Keys.GetItem(1, 2, 3, 4, 5, 6)!.Id);
		}
		itemsCache.Update(
			1,
			new(1, 11, 12, 13, 14, 15, 16));
		{
			Assert.IsNull(itemIndexWith1Key.GetItem(1));
			Assert.IsNull(itemIndexWith2Keys.GetItem(1, 2));
			Assert.IsNull(itemIndexWith3Keys.GetItem(1, 2, 3));
			Assert.IsNull(itemIndexWith4Keys.GetItem(1, 2, 3, 4));
			Assert.IsNull(itemIndexWith5Keys.GetItem(1, 2, 3, 4, 5));
			Assert.IsNull(itemIndexWith6Keys.GetItem(1, 2, 3, 4, 5, 6));
			//
			Assert.AreEqual(1, itemIndexWith1Key.GetItem(11)!.Id);
			Assert.AreEqual(1, itemIndexWith2Keys.GetItem(11, 12)!.Id);
			Assert.AreEqual(1, itemIndexWith3Keys.GetItem(11, 12, 13)!.Id);
			Assert.AreEqual(1, itemIndexWith4Keys.GetItem(11, 12, 13, 14)!.Id);
			Assert.AreEqual(1, itemIndexWith5Keys.GetItem(11, 12, 13, 14, 15)!.Id);
			Assert.AreEqual(1, itemIndexWith6Keys.GetItem(11, 12, 13, 14, 15, 16)!.Id);
		}
		itemsCache.Remove(1);
		{
			Assert.IsNull(itemIndexWith1Key.GetItem(11));
			Assert.IsNull(itemIndexWith2Keys.GetItem(11, 12));
			Assert.IsNull(itemIndexWith3Keys.GetItem(11, 12, 13));
			Assert.IsNull(itemIndexWith4Keys.GetItem(11, 12, 13, 14));
			Assert.IsNull(itemIndexWith5Keys.GetItem(11, 12, 13, 14, 15));
			Assert.IsNull(itemIndexWith6Keys.GetItem(11, 12, 13, 14, 15, 16));
		}

		////////////////////////////////////////////////
		itemsCache.Clear();
		////////////////////////////////////////////////

		itemsCache.Add(
			1,
			new(1, 1, 2, 3, 4, 5, 6));
		itemsCache.Add(
			2,
			new(2, 1, 2, 3, 4, 5, 6));
		{
			Assert.AreEqual(2, itemIndexWith1Key.GetItem(1)!.Id);
			Assert.AreEqual(2, itemIndexWith2Keys.GetItem(1, 2)!.Id);
			Assert.AreEqual(2, itemIndexWith3Keys.GetItem(1, 2, 3)!.Id);
			Assert.AreEqual(2, itemIndexWith4Keys.GetItem(1, 2, 3, 4)!.Id);
			Assert.AreEqual(2, itemIndexWith5Keys.GetItem(1, 2, 3, 4, 5)!.Id);
			Assert.AreEqual(2, itemIndexWith6Keys.GetItem(1, 2, 3, 4, 5, 6)!.Id);
			//
			Assert.AreEqual(2, itemsIndexWith1Key.GetItems(1)!.Length);
			Assert.AreEqual(2, itemsIndexWith2Keys.GetItems(1, 2)!.Length);
			Assert.AreEqual(2, itemsIndexWith3Keys.GetItems(1, 2, 3)!.Length);
			Assert.AreEqual(2, itemsIndexWith4Keys.GetItems(1, 2, 3, 4)!.Length);
			Assert.AreEqual(2, itemsIndexWith5Keys.GetItems(1, 2, 3, 4, 5)!.Length);
			Assert.AreEqual(2, itemsIndexWith6Keys.GetItems(1, 2, 3, 4, 5, 6)!.Length);

		}
		itemsCache.Update(
			1,
			new(1, 11, 12, 13, 14, 15, 16));
		{
			Assert.IsNull(itemIndexWith1Key.GetItem(1));
			Assert.IsNull(itemIndexWith2Keys.GetItem(1, 2));
			Assert.IsNull(itemIndexWith3Keys.GetItem(1, 2, 3));
			Assert.IsNull(itemIndexWith4Keys.GetItem(1, 2, 3, 4));
			Assert.IsNull(itemIndexWith5Keys.GetItem(1, 2, 3, 4, 5));
			Assert.IsNull(itemIndexWith6Keys.GetItem(1, 2, 3, 4, 5, 6));
			//
			Assert.AreEqual(1, itemIndexWith1Key.GetItem(11)!.Id);
			Assert.AreEqual(1, itemIndexWith2Keys.GetItem(11, 12)!.Id);
			Assert.AreEqual(1, itemIndexWith3Keys.GetItem(11, 12, 13)!.Id);
			Assert.AreEqual(1, itemIndexWith4Keys.GetItem(11, 12, 13, 14)!.Id);
			Assert.AreEqual(1, itemIndexWith5Keys.GetItem(11, 12, 13, 14, 15)!.Id);
			Assert.AreEqual(1, itemIndexWith6Keys.GetItem(11, 12, 13, 14, 15, 16)!.Id);
			//
			Assert.AreEqual(1, itemsIndexWith1Key.GetItems(1)!.Length);
			Assert.AreEqual(1, itemsIndexWith2Keys.GetItems(1, 2)!.Length);
			Assert.AreEqual(1, itemsIndexWith3Keys.GetItems(1, 2, 3)!.Length);
			Assert.AreEqual(1, itemsIndexWith4Keys.GetItems(1, 2, 3, 4)!.Length);
			Assert.AreEqual(1, itemsIndexWith5Keys.GetItems(1, 2, 3, 4, 5)!.Length);
			Assert.AreEqual(1, itemsIndexWith6Keys.GetItems(1, 2, 3, 4, 5, 6)!.Length);
		}
		itemsCache.Remove(1);
		{
			Assert.IsNull(itemIndexWith1Key.GetItem(11));
			Assert.IsNull(itemIndexWith2Keys.GetItem(11, 12));
			Assert.IsNull(itemIndexWith3Keys.GetItem(11, 12, 13));
			Assert.IsNull(itemIndexWith4Keys.GetItem(11, 12, 13, 14));
			Assert.IsNull(itemIndexWith5Keys.GetItem(11, 12, 13, 14, 15));
			Assert.IsNull(itemIndexWith6Keys.GetItem(11, 12, 13, 14, 15, 16));
			//
			Assert.AreEqual(1, itemsIndexWith1Key.GetItems(1)!.Length);
			Assert.AreEqual(1, itemsIndexWith2Keys.GetItems(1, 2)!.Length);
			Assert.AreEqual(1, itemsIndexWith3Keys.GetItems(1, 2, 3)!.Length);
			Assert.AreEqual(1, itemsIndexWith4Keys.GetItems(1, 2, 3, 4)!.Length);
			Assert.AreEqual(1, itemsIndexWith5Keys.GetItems(1, 2, 3, 4, 5)!.Length);
			Assert.AreEqual(1, itemsIndexWith6Keys.GetItems(1, 2, 3, 4, 5, 6)!.Length);
		}
	}

}
