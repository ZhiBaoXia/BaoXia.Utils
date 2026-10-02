using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Test.ExtensionsTest;

[TestClass]
public class IEnumerableExtensionTest
{
	[TestMethod]
	public void GetCountTest()
	{
		var itemArray = new int[] { 1, 2, 3, 4, 5 };
		var itemList = new List<int> { 1, 2, 3, 4, 5, 6 };

		IEnumerable<int> itemEnumerable = itemArray;
		{
			Assert.AreEqual(itemArray.Length, itemEnumerable.GetCount());
		}

		itemEnumerable = itemList;
		{
			Assert.AreEqual(itemList.Count, itemEnumerable.GetCount());
		}
	}

	[TestMethod]
	public void ForEachAtLeastOnceTest()
	{
		var items_Int = new int[] { 1, 2, 3, 4, 5 };
		var objectItem_Int = 0;
		items_Int.ForEachAtLeastOnce((item) =>
		{
			objectItem_Int++;
			{
				// !!!
				Assert.AreEqual(objectItem_Int, item);
				// !!!
			}
			return true;
		});
		items_Int = [];
		items_Int.ForEachAtLeastOnce((item) =>
		{
			// !!!
			Assert.AreEqual(0, item);
			// !!!
			return true;
		});
		items_Int = null;
		items_Int.ForEachAtLeastOnce((item) =>
		{
			// !!!
			Assert.AreEqual(0, item);
			// !!!
			return true;
		});


		var items_String = new string[] { "1", "2", "3", "4", "5" };
		var objectItem_String = 0;
		items_String.ForEachAtLeastOnce((item) =>
		{
			objectItem_String++;
			{
				// !!!
				Assert.AreEqual(objectItem_String.ToString(), item);
				// !!!
			}
			return true;
		});
		items_String = [];
		items_String.ForEachAtLeastOnce((item) =>
		{
			// !!!
			Assert.IsNull(item);
			// !!!
			return true;
		});
		items_String = null;
		items_String.ForEachAtLeastOnce((item) =>
		{
			// !!!
			Assert.IsNull(item);
			// !!!
			return true;
		});
	}


	[TestMethod]
	public async Task ForEachAtLeastOnceAsyncAsyncTest()
	{
		var items_Int = new int[] { 1, 2, 3, 4, 5 };
		var objectItem_Int = 0;
		await items_Int.ForEachAtLeastOnceAsync(async (item) =>
		{
			objectItem_Int++;
			{
				// !!!
				Assert.AreEqual(objectItem_Int, item);
				// !!!
			}
			return await Task.FromResult(true);
		});
		items_Int = [];
		await items_Int.ForEachAtLeastOnceAsync(async (item) =>
		{
			// !!!
			Assert.AreEqual(0, item);
			// !!!
			return await Task.FromResult(true);
		});
		items_Int = null;
		await items_Int.ForEachAtLeastOnceAsync(async (item) =>
		{
			// !!!
			Assert.AreEqual(0, item);
			// !!!
			return await Task.FromResult(true);
		});


		var items_String = new string[] { "1", "2", "3", "4", "5" };
		var objectItem_String = 0;
		await items_String.ForEachAtLeastOnceAsync(async (item) =>
		{
			objectItem_String++;
			{
				// !!!
				Assert.AreEqual(objectItem_String.ToString(), item);
				// !!!
			}
			return await Task.FromResult(true);
		});
		items_String = [];
		await items_String.ForEachAtLeastOnceAsync(async (item) =>
		{
			// !!!
			Assert.IsNull(item);
			// !!!
			return await Task.FromResult(true);
		});
		items_String = null;
		await items_String.ForEachAtLeastOnceAsync(async (item) =>
		{
			// !!!
			Assert.IsNull(item);
			// !!!
			return await Task.FromResult(true);
		});
	}

	[TestMethod]
	public void IsContainsItemTest()
	{
		////////////////////////////////////////////////
		// 1/3，“items”没有元素：
		////////////////////////////////////////////////
		{
			var items = Array.Empty<int>();

			// 1/2，不包含：
			Assert.IsFalse(items.IsContains(0));
			Assert.IsTrue(items.IsNotContains(0));

			// 1/2，包含：
			Assert.IsFalse(items.IsContains(1));
			Assert.IsTrue(items.IsNotContains(1));
		}

		////////////////////////////////////////////////
		// 2/3，“items”含有1个元素：
		////////////////////////////////////////////////
		{
			var items = new int[]
			{
				1
			};

			// 1/2，不包含：
			Assert.IsFalse(items.IsContains(0));
			Assert.IsTrue(items.IsNotContains(0));

			// 1/2，包含：
			Assert.IsTrue(items.IsContains(1));
			Assert.IsFalse(items.IsNotContains(1));
		}

		////////////////////////////////////////////////
		// 3/3，“items”含有多个元素：
		////////////////////////////////////////////////
		{
			var items = new int[]
			{
				1,
				2,
				3
			};

			// 1/2，不包含：
			Assert.IsFalse(items.IsContains(0));
			Assert.IsTrue(items.IsNotContains(0));

			// 1/2，包含：
			Assert.IsTrue(items.IsContains(1));
			Assert.IsFalse(items.IsNotContains(1));
		}
	}

	[TestMethod]
	public void IsContainsItemsTest()
	{
		var items_1_2 = new int[]
		{
			1,
			2
		};
		var items_3_4 = new int[]
		{
			3,
			4
		};

		////////////////////////////////////////////////
		// 0/4，“空”集合：
		////////////////////////////////////////////////
		{
			var items = Array.Empty<int>();

			Assert.IsFalse(items.IsContains(null as int[]));
			Assert.IsFalse(items.IsContains([]));
		}

		////////////////////////////////////////////////
		// 1/4，“items”没有元素：
		////////////////////////////////////////////////
		{
			var items = Array.Empty<int>();

			Assert.IsFalse(items.IsContains(items_1_2));
			Assert.IsTrue(items.IsNotContains(items_1_2));

			Assert.IsFalse(items.IsContains(items_3_4));
			Assert.IsTrue(items.IsNotContains(items_3_4));
		}

		////////////////////////////////////////////////
		// 2/4，“items”含有1个元素：
		////////////////////////////////////////////////
		{
			var items = new int[]
			{
				1
			};

			Assert.IsFalse(items.IsContains(items_1_2));
			Assert.IsTrue(items.IsNotContains(items_1_2));

			Assert.IsFalse(items.IsContains(items_3_4));
			Assert.IsTrue(items.IsNotContains(items_3_4));
		}

		////////////////////////////////////////////////
		// 3/4，“items”含有同样多个元素：
		////////////////////////////////////////////////
		{
			var items = new int[]
			{
				1,
				2
			};

			Assert.IsTrue(items.IsContains(items_1_2));
			Assert.IsFalse(items.IsNotContains(items_1_2));

			Assert.IsFalse(items.IsContains(items_3_4));
			Assert.IsTrue(items.IsNotContains(items_3_4));
		}

		////////////////////////////////////////////////
		// 4/4，“items”含有多个元素：
		////////////////////////////////////////////////
		{
			var items = new int[]
			{
				1,
				2,
				3
			};

			Assert.IsTrue(items.IsContains(items_1_2));
			Assert.IsFalse(items.IsNotContains(items_1_2));

			Assert.IsFalse(items.IsContains(items_3_4));
			Assert.IsTrue(items.IsNotContains(items_3_4));

			items =
			[
				1,
				2,
				3,
				4
			];

			Assert.IsTrue(items.IsContains(items_1_2));
			Assert.IsFalse(items.IsNotContains(items_1_2));

			Assert.IsTrue(items.IsContains(items_3_4));
			Assert.IsFalse(items.IsNotContains(items_3_4));


			items =
			[
				1,
				2,
				3,
				4,
				5
			];

			Assert.IsTrue(items.IsContains(items_1_2));
			Assert.IsFalse(items.IsNotContains(items_1_2));

			Assert.IsTrue(items.IsContains(items_3_4));
			Assert.IsFalse(items.IsNotContains(items_3_4));
		}
	}

	[TestMethod]
	public void IsEqualsTest()
	{
		var items_1_2 = new int[]
		{
			1,
			2
		};
		var items_3_4 = new int[]
		{
			3,
			4
		};
		var items_1_1_2_2 = new int[]
		{
			1,
			1,
			2,
			2
		};
		var items_3_3_4_4_4 = new int[]
		{
			3,
			3,
			4,
			4
		};
		var items_1_2_2 = new int[]
		{
			1,
			2,
			2
		};



		////////////////////////////////////////////////
		// 0/4，“空”集合：
		////////////////////////////////////////////////
		{
			var items = Array.Empty<int>();

			Assert.IsTrue(items.IsEquals(null, true));
			Assert.IsTrue(items.IsEquals([], true));
		}

		////////////////////////////////////////////////
		// 1/4，“items”没有元素：
		////////////////////////////////////////////////
		{
			var items = Array.Empty<int>();

			Assert.IsFalse(items.IsEquals(items_1_2));
			Assert.IsTrue(items.IsNotEquals(items_1_2));

			Assert.IsFalse(items.IsEquals(items_3_4));
			Assert.IsTrue(items.IsNotEquals(items_3_4));
		}

		////////////////////////////////////////////////
		// 2/4，“items”含有1个元素：
		////////////////////////////////////////////////
		{
			var items = new int[]
			{
				1
			};

			Assert.IsFalse(items.IsEquals(items_1_2));
			Assert.IsTrue(items.IsNotEquals(items_1_2));

			Assert.IsFalse(items.IsEquals(items_3_4));
			Assert.IsTrue(items.IsNotEquals(items_3_4));
		}

		////////////////////////////////////////////////
		// 3/4，“items”含有同样多个元素：
		////////////////////////////////////////////////
		{
			var items = new int[]
			{
				1,
				2
			};

			Assert.IsTrue(items.IsEquals(items_1_2));
			Assert.IsFalse(items.IsNotEquals(items_1_2));

			Assert.IsFalse(items.IsEquals(items_3_4));
			Assert.IsTrue(items.IsNotEquals(items_3_4));
		}

		////////////////////////////////////////////////
		// 4/4，“items”含有多个元素：
		////////////////////////////////////////////////
		{
			var items = new int[]
			{
				1,
				2,
				3
			};

			Assert.IsFalse(items.IsEquals(items_1_2));
			Assert.IsTrue(items.IsNotEquals(items_1_2));

			Assert.IsFalse(items.IsEquals(items_3_4));
			Assert.IsTrue(items.IsNotEquals(items_3_4));

			items =
			[
				1,
				2,
				3,
				4
			];

			Assert.IsFalse(items.IsEquals(items_1_2));
			Assert.IsTrue(items.IsNotEquals(items_1_2));

			Assert.IsFalse(items.IsEquals(items_3_4));
			Assert.IsTrue(items.IsNotEquals(items_3_4));


			items =
			[
				1,
				2,
				3,
				4,
				5
			];

			Assert.IsFalse(items.IsEquals(items_1_2));
			Assert.IsTrue(items.IsNotEquals(items_1_2));

			Assert.IsFalse(items.IsEquals(items_3_4));
			Assert.IsTrue(items.IsNotEquals(items_3_4));


			////////////////////////////////////////////////
			////////////////////////////////////////////////
			////////////////////////////////////////////////


			items =
			[
				1,
				2,
				1,
				2
			];

			Assert.IsTrue(items.IsEquals(items_1_2, true));
			Assert.IsFalse(items.IsNotEquals(items_1_2, true));
			Assert.IsFalse(items.IsEquals(items_1_2, false));
			Assert.IsTrue(items.IsNotEquals(items_1_2, false));

			Assert.IsFalse(items.IsEquals(items_3_4, true));
			Assert.IsTrue(items.IsNotEquals(items_3_4, true));
			Assert.IsFalse(items.IsEquals(items_3_4, false));
			Assert.IsTrue(items.IsNotEquals(items_3_4, false));


			items =
			[
				3,
				3,
				4,
				4,
				4
			];

			Assert.IsFalse(items.IsEquals(items_1_2, true));
			Assert.IsTrue(items.IsNotEquals(items_1_2, true));
			Assert.IsFalse(items.IsEquals(items_1_2, false));
			Assert.IsTrue(items.IsNotEquals(items_1_2, false));

			Assert.IsTrue(items.IsEquals(items_3_4, true));
			Assert.IsFalse(items.IsNotEquals(items_3_4, true));
			Assert.IsFalse(items.IsEquals(items_3_4, false));
			Assert.IsTrue(items.IsNotEquals(items_3_4, false));


			////////////////////////////////////////////////
			////////////////////////////////////////////////
			////////////////////////////////////////////////


			items =
			[
				1,
				2
			];

			Assert.IsTrue(items.IsEquals(items_1_1_2_2, true));
			Assert.IsFalse(items.IsNotEquals(items_1_1_2_2, true));
			Assert.IsFalse(items.IsEquals(items_1_1_2_2, false));
			Assert.IsTrue(items.IsNotEquals(items_1_1_2_2, false));

			Assert.IsFalse(items.IsEquals(items_3_3_4_4_4, true));
			Assert.IsTrue(items.IsNotEquals(items_3_3_4_4_4, true));
			Assert.IsFalse(items.IsEquals(items_3_3_4_4_4, false));
			Assert.IsTrue(items.IsNotEquals(items_3_3_4_4_4, false));


			items =
			[
				3,
				4
			];

			Assert.IsFalse(items.IsEquals(items_1_1_2_2, true));
			Assert.IsTrue(items.IsNotEquals(items_1_1_2_2, true));
			Assert.IsFalse(items.IsEquals(items_1_1_2_2, false));
			Assert.IsTrue(items.IsNotEquals(items_1_1_2_2, false));

			Assert.IsTrue(items.IsEquals(items_3_3_4_4_4, true));
			Assert.IsFalse(items.IsNotEquals(items_3_3_4_4_4, true));
			Assert.IsFalse(items.IsEquals(items_3_3_4_4_4, false));
			Assert.IsTrue(items.IsNotEquals(items_3_3_4_4_4, false));


			////////////////////////////////////////////////
			////////////////////////////////////////////////
			////////////////////////////////////////////////


			items =
			[
				1,
				2
			];

			Assert.IsTrue(items.IsEquals(items_1_1_2_2, true));
			Assert.IsFalse(items.IsNotEquals(items_1_1_2_2, true));
			Assert.IsFalse(items.IsEquals(items_1_1_2_2, false));
			Assert.IsTrue(items.IsNotEquals(items_1_1_2_2, false));

			Assert.IsFalse(items.IsEquals(items_3_3_4_4_4, true));
			Assert.IsTrue(items.IsNotEquals(items_3_3_4_4_4, true));
			Assert.IsFalse(items.IsEquals(items_3_3_4_4_4, false));
			Assert.IsTrue(items.IsNotEquals(items_3_3_4_4_4, false));


			items =
			[
				3,
				4
			];

			Assert.IsFalse(items.IsEquals(items_1_1_2_2, true));
			Assert.IsTrue(items.IsNotEquals(items_1_1_2_2, true));
			Assert.IsFalse(items.IsEquals(items_1_1_2_2, false));
			Assert.IsTrue(items.IsNotEquals(items_1_1_2_2, false));

			Assert.IsTrue(items.IsEquals(items_3_3_4_4_4, true));
			Assert.IsFalse(items.IsNotEquals(items_3_3_4_4_4, true));
			Assert.IsFalse(items.IsEquals(items_3_3_4_4_4, false));
			Assert.IsTrue(items.IsNotEquals(items_3_3_4_4_4, false));


			////////////////////////////////////////////////
			////////////////////////////////////////////////
			////////////////////////////////////////////////


			items =
			[
				1,
				2
			];

			Assert.IsTrue(items.IsEquals(items_1_2_2, true));
			Assert.IsFalse(items.IsNotEquals(items_1_2_2, true));
			Assert.IsFalse(items.IsEquals(items_1_2_2, false));
			Assert.IsTrue(items.IsNotEquals(items_1_2_2, false));


			items =
			[
				3,
				4
			];

			Assert.IsFalse(items.IsEquals(items_1_1_2_2, true));
			Assert.IsTrue(items.IsNotEquals(items_1_1_2_2, true));
			Assert.IsFalse(items.IsEquals(items_1_1_2_2, false));
			Assert.IsTrue(items.IsNotEquals(items_1_1_2_2, false));

			Assert.IsTrue(items.IsEquals(items_3_3_4_4_4, true));
			Assert.IsFalse(items.IsNotEquals(items_3_3_4_4_4, true));
			Assert.IsFalse(items.IsEquals(items_3_3_4_4_4, false));
			Assert.IsTrue(items.IsNotEquals(items_3_3_4_4_4, false));
		}
	}

	class TestItem(
		int groupId,
		string value)
	{
		public int GroupId { get; set; } = groupId;

		public string Value { get; set; } = value;
	}

	[TestMethod]
	public void ToGroupsGroupByTest()
	{
		// 生成测试数据：
		var items = new List<TestItem>();
		{
			var maxItemGroupId = 100;
			for (var groupIndex = 0;
				groupIndex < 100;
				groupIndex++)
			{
				var itemsCountInGroup = new Random().Next(100);
				for (var itemIndex = 0;
					itemIndex < itemsCountInGroup;
					itemIndex++)
				{
					var itemGroupId = new Random().Next(maxItemGroupId);
					var item = new TestItem(
						itemGroupId,
						itemGroupId.ToString());
					{ }
					items.Add(item);
				}
			}
		}

		// 开始测试：
		var itemGroups = items.ToGroupsBy((item) => item.GroupId);
		var itemsCount = 0;
		{
			Assert.IsGreaterThan(0, itemGroups!.Length);
			foreach (var itemGroup in itemGroups)
			{
				itemsCount += itemGroup.Count;
				if (itemGroup.Count > 0)
				{
					var itemGroupId = itemGroup[0].GroupId;
					foreach (var item in itemGroup)
					{
						// !!!
						Assert.AreEqual(itemGroupId, item.GroupId);
						// !!!
					}
				}
			}
		}
		// !!!
		Assert.AreEqual(items.Count, itemsCount);
		// !!!
	}

	class ItemEnumratesCount
	{
		public int Number;
	}

	[TestMethod]
	public async Task ConcurrentProcessItemsAsyncTest()
	{
		var sourceItems = new int[10000];
		for (var itemindex = 0;
			itemindex < sourceItems.Length;
			itemindex++)
		{
			sourceItems[itemindex] = itemindex;
		}


		ConcurrentDictionary<int, ItemEnumratesCount> itemEnumratesCounts = new();

		await sourceItems.ConcurrentProcessItemsAsync(
			       (item) =>
			       {
				       var itemEnumratesCount
				       = itemEnumratesCounts.GetOrAdd(item, new ItemEnumratesCount());
				       // !!!
				       Interlocked.Increment(ref itemEnumratesCount.Number);
				       // !!!
			       });
		foreach (var sourceItem in sourceItems)
		{
			Assert.IsTrue(itemEnumratesCounts.TryGetValue(
				sourceItem,
				out var itemEnumratesCount)
);
			// !!!
			Assert.AreEqual(1, itemEnumratesCount.Number);
			// !!!
		}
	}


	[TestMethod]
	public async Task SearchAsyncTest()
	{
		var testItems = new TestItem[10000];
		for (var testIndex = 0;
			testIndex < testItems.Length;
			testIndex++)
		{
			testItems[testIndex] = new(
				testIndex,
				(testIndex + 1).ToString());
		}
		var searchKey = "99";

		var searchPage = await testItems.SearchAsync(
			10,
			(item) =>
			{
				return item.Value.GetMatchProgressValueOf(
					searchKey,
					StringComparison.OrdinalIgnoreCase,
					false);
			},
			(itemSearchResults) =>
			{
				itemSearchResults.Sort((searchResultA, searchResultB) =>
				{
					var compareResult
					= searchResultB.MatchedProgress.CompareTo(
						searchResultA.MatchedProgress);
					if (compareResult != 0)
					{
						return compareResult;
					}

					compareResult
					= searchResultB.Item.Value.Length.CompareTo(
						searchResultA.Item.Value.Length);
					if (compareResult != 0)
					{
						return compareResult;
					}

					compareResult
					= searchResultB.Item.GroupId.CompareTo(
						searchResultA.Item.GroupId);
					if (compareResult != 0)
					{
						return compareResult;
					}

					return 0;
				});
				return itemSearchResults;
			},
			null,
			0,
			20);
		Assert.IsGreaterThan(0, searchPage!.ItemsCountSearchMatched);
		var itemPage = searchPage?.ItemsInPage!;
		var firstItemInPage = itemPage[0].Value;
		Assert.AreEqual("9999", firstItemInPage);

		searchPage = await testItems.SearchAsync(
			10,
			(item) =>
			{
				return item.Value.GetMatchProgressValueOf(
					searchKey,
					StringComparison.OrdinalIgnoreCase,
					false);
			},
			(itemSearchResults) =>
			{
				itemSearchResults.Sort((searchResultA, searchResultB) =>
				{
					var compareResult
					= searchResultB.MatchedProgress.CompareTo(
						searchResultA.MatchedProgress);
					if (compareResult != 0)
					{
						return compareResult;
					}

					compareResult
					= searchResultB.Item.Value.Length.CompareTo(
						searchResultA.Item.Value.Length);
					if (compareResult != 0)
					{
						return compareResult;
					}

					compareResult
					= searchResultB.Item.GroupId.CompareTo(
						searchResultA.Item.GroupId);
					if (compareResult != 0)
					{
						return compareResult;
					}

					return 0;
				});
				return itemSearchResults;
			},
			null,
			1,
			20);
		Assert.IsGreaterThan(0, searchPage!.ItemsCountSearchMatched);
		itemPage = searchPage?.ItemsInPage!;
		firstItemInPage = itemPage[0].Value;
		Assert.AreEqual("9998", firstItemInPage);
	}

	[TestMethod]
	public void ToItemHashSetTest()
	{
		var items = new string[] {
			"a",
			"b",
			"b",
			"c"
		};
		var itemHashSet = items.ToItemHashSet();
		{
			Assert.HasCount(3, itemHashSet);
			Assert.Contains("a", itemHashSet);
			Assert.Contains("b", itemHashSet);
			Assert.Contains("c", itemHashSet);
		}
	}

	[TestMethod]
	public async Task SortAsyncTest()
	{
		////////////////////////////////////////////////
		// 排序测试，01：
		////////////////////////////////////////////////
		var itemsSet = new HashSet<int>();
		{
			itemsSet.Add(5);
			itemsSet.Add(4);
			itemsSet.Add(3);
			itemsSet.Add(2);
			itemsSet.Add(1);
			itemsSet.Add(0);
		}
		var testList = await itemsSet.SortToListAsync(async (a, b) =>
		{
			await Task.CompletedTask;

			return a.CompareTo(b);
		});
		for (var listItemIndex = 0; listItemIndex < testList.Count; listItemIndex++)
		{
			Assert.AreEqual(listItemIndex, testList[listItemIndex]);
		}

		////////////////////////////////////////////////
		// 排序测试，02：
		////////////////////////////////////////////////
		var itemsBag = new ConcurrentBag<int>();
		{
			itemsBag.Add(0);
			itemsBag.Add(5);
			itemsBag.Add(1);
			itemsBag.Add(4);
			itemsBag.Add(3);
			itemsBag.Add(2);
		}
		testList = await itemsBag.SortToListAsync(async (a, b) =>
		{
			await Task.CompletedTask;

			return a.CompareTo(b);
		});
		for (var listItemIndex = 0; listItemIndex < testList.Count; listItemIndex++)
		{
			Assert.AreEqual(listItemIndex, testList[listItemIndex]);
		}

		////////////////////////////////////////////////
		// 排序测试，03：
		////////////////////////////////////////////////
		var itemDictionary = new Dictionary<string, int>();
		{
			itemDictionary.Add("a", 0);
			itemDictionary.Add("b", 5);
			itemDictionary.Add("c", 1);
			itemDictionary.Add("d", 4);
			itemDictionary.Add("e", 3);
			itemDictionary.Add("f", 2);
		}
		testList = await itemDictionary.Values.SortToListAsync(async (a, b) =>
		{
			await Task.CompletedTask;

			return a.CompareTo(b);
		});
		for (var listItemIndex = 0; listItemIndex < testList.Count; listItemIndex++)
		{
			Assert.AreEqual(listItemIndex, testList[listItemIndex]);
		}
	}
}
