using BaoXia.Utils.Constants;
using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace BaoXia.Utils.Test.ExtensionsTest;

[TestClass]
public class ListExtensionTest
{
	[TestMethod]
	public void AddUniqueTest()
	{
		var list = new List<int>();

		for (int i = 0; i < 10; i++)
		{
			if (i % 2 != 0)
			{
				list.Add(i);
			}
		}

		for (int i = 0; i < 10; i++)
		{
			if (i % 2 != 0)
			{
				Assert.IsFalse(list.AddUnique(i));
			}
			else
			{
				Assert.IsTrue(list.AddUnique(i));
			}
		}
	}

	[TestMethod]
	public void InsertByOrderTest()
	{
		int[] itemsInOrder = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
		int[] itemsInOrderDescending = itemsInOrder.Reverse().ToArray();
		var itemInRandomOrder = new int[itemsInOrder.Length];
		for (int itemIndex = 0;
			itemIndex < itemInRandomOrder.Length;
			itemIndex++)
		{
			itemInRandomOrder[itemIndex] = 0;
		}
		for (int itemIndex = 0;
			itemIndex < itemsInOrder.Length;
			itemIndex++)
		{
			var item = itemsInOrder[itemIndex];
			while (true)
			{
				var itemRandomIndex
					= System.Random.Shared.Next(itemInRandomOrder.Length);
				if (itemInRandomOrder[itemRandomIndex] == 0)
				{
					// !!!
					itemInRandomOrder[itemRandomIndex] = item;
					break;
					// !!!
				}
			}
		}

		var listWithOrder = new List<int>();
		foreach (var item in itemInRandomOrder)
		{
			listWithOrder.InsertWithOrder(
				item,
				(itemA, itemB) =>
				{
					return itemA.CompareTo(itemB);
				});
		}
		////////////////////////////////////////////////
		// !!!
		Assert.IsTrue(listWithOrder.SequenceEqual(itemsInOrder));
		// !!!
		////////////////////////////////////////////////
		///

		var listWithOrderDescending = new List<int>();
		foreach (var item in itemInRandomOrder)
		{
			listWithOrderDescending.InsertWithOrderDescending(
				item,
				(itemA, itemB) =>
				{
					return itemA.CompareTo(itemB);
				});
		}
		////////////////////////////////////////////////
		// !!!
		Assert.IsTrue(listWithOrderDescending.SequenceEqual(itemsInOrderDescending));
		// !!!
		////////////////////////////////////////////////
	}

	[TestMethod]
	public void RemoveFromTest()
	{
		var TestItems = new int[]
		{
			0,
			1,
			2,
			3,
			4
		};


		// !!!
		List<int> testList = [..TestItems];
		Assert.IsTrue(testList.RemoveFrom(-1, 1) == 2);
		// !!!

		// !!!
		testList = [..TestItems];
		Assert.IsTrue(testList.RemoveFrom(-1) == 5);
		// !!!

		// !!!
		testList = [..TestItems];
		Assert.IsTrue(testList.RemoveFrom(0, 1) == 2);
		// !!!

		// !!!
		testList = [..TestItems];
		Assert.IsTrue(testList.RemoveFrom(1) == 4);
		// !!!

		// !!!
		testList = [..TestItems];
		Assert.IsTrue(testList.RemoveFrom(3, 5) == 2);
		// !!!

		// !!!
		testList = [..TestItems];
		Assert.IsTrue(testList.RemoveFrom(3) == 2);
		// !!!

		// !!!
		testList = [..TestItems];
		Assert.IsTrue(testList.RemoveFrom(4) == 1);
		// !!!

		// !!!
		testList = [..TestItems];
		Assert.IsTrue(testList.RemoveFrom(5) == 0);
		// !!!
	}


	class TestItem(int index)
	{
		public int Index { get; set; } = index;
	}

	[TestMethod]
	public void FindItemIndexWithDichotomyTest()
	{
		var list = new List<TestItem>();
		var random = new System.Random();
		for (int i = 0; i < 100; i++)
		{
			list.Add(new TestItem(random.Next()));
		}
		list.Sort((itemA, itemB) =>
		{
			return itemA.Index.CompareTo(itemB.Index);
		});

		for (var i = 0; i < list.Count; i++)
		{
			var objectTestItem = list[i];
			var itemIndexFound = list.FindItemIndexWithDichotomy(
				true,
				(testItem, testItemIndex) =>
				{
					return testItem.Index.CompareTo(objectTestItem.Index);
				});
			Assert.IsTrue(itemIndexFound == i);
		}

		var testSearchRangeBeginIndex
			= random.Next(list.Count);
		var testSearchRangeLength
			= random.Next(list.Count - testSearchRangeBeginIndex);
		for (var i = testSearchRangeBeginIndex;
			i < testSearchRangeBeginIndex + testSearchRangeLength;
			i++)
		{
			var objectTestItem = list[i];
			var itemIndexFound = list.FindItemIndexWithDichotomyInRange(
				true,
				testSearchRangeBeginIndex,
				testSearchRangeLength,
				(testItem, testItemIndex) =>
				{
					return testItem.Index.CompareTo(objectTestItem.Index);
				});
			Assert.IsTrue(itemIndexFound == i);
		}


		for (var i = 0; i < list.Count; i++)
		{
			var objectTestItem = list[i];
			var itemFound = list.FindItemWithDichotomy(
				true,
				(testItem, testItemIndex) =>
				{
					return testItem.Index.CompareTo(objectTestItem.Index);
				});
			Assert.IsTrue(itemFound?.Index == objectTestItem.Index);
		}

		testSearchRangeBeginIndex
			= random.Next(list.Count);
		testSearchRangeLength
			= random.Next(list.Count - testSearchRangeBeginIndex);
		for (var i = testSearchRangeBeginIndex;
			i < testSearchRangeBeginIndex + testSearchRangeLength;
			i++)
		{
			var objectTestItem = list[i];
			var itemFound = list.FindItemWithDichotomyInRange(
				true,
				testSearchRangeBeginIndex,
				testSearchRangeLength,
				(testItem, testItemIndex) =>
				{
					return testItem.Index.CompareTo(objectTestItem.Index);
				});
			Assert.IsTrue(itemFound?.Index == objectTestItem.Index);
		}
	}


	[TestMethod]
	public void FindNearestItemIndexWithDichotomyTest()
	{
		var itemList = new List<int>();
		var random = new System.Random();
		const int testItemCount = 100;
		for (int testItemIndex = 0;
			testItemIndex < testItemCount;
			testItemIndex++)
		{
			if (testItemIndex % 2 == 0)
			{
				itemList.Add(testItemIndex);
			}
		}
		itemList.Sort((itemA, itemB) =>
		{
			return itemA.CompareTo(itemB);
		});

		var items = itemList.ToList();

		//var firstTestItemOdd = 1;
		var firsttTestItemEven = 0;
		for (int objectTestItemIndex = 0;
			objectTestItemIndex < testItemCount;
			objectTestItemIndex++)
		{
			items.FindItemIndexWithDichotomy(
				true,
				(testItem, testItemIndex) =>
				{
					return testItem.CompareTo(objectTestItemIndex);
				},
				//
				DichotomyClosestItemType.LessThanObjectMax,
				out var itemIndexNearestAtLeft,
				out var itemNearestAtLeft);


			if ((objectTestItemIndex % 2) == 0)
			{
				Assert.IsTrue(itemIndexNearestAtLeft == (objectTestItemIndex / 2) - 1);
				if (itemIndexNearestAtLeft >= 0)
				{
					Assert.IsTrue(itemNearestAtLeft == objectTestItemIndex - 2);
				}
				else if (itemIndexNearestAtLeft == -1)
				{
					Assert.IsTrue(itemNearestAtLeft == firsttTestItemEven);
				}
				else
				{
					Assert.Fail();
				}
			}
			else
			{
				Assert.IsTrue(itemIndexNearestAtLeft == (objectTestItemIndex / 2));
				Assert.IsTrue(itemNearestAtLeft == objectTestItemIndex - 1);
			}
		}

		var lastTestItemOdd = testItemCount - 1;
		if (lastTestItemOdd % 2 == 0)
		{
			lastTestItemOdd -= 1;
		}
		var lastTestItemEven = testItemCount - 1;
		if (lastTestItemEven % 2 != 0)
		{
			lastTestItemEven -= 1;
		}
		for (int objectTestItemIndex = 0;
			objectTestItemIndex < testItemCount;
			objectTestItemIndex++)
		{
			items.FindItemIndexWithDichotomy(
				true,
				(testItem, testItemIndex) =>
				{
					return testItem.CompareTo(objectTestItemIndex);
				},
				//
				DichotomyClosestItemType.GreaterThanObjectMin,
				out var itemIndexNearestAtRight,
				out var itemNearestAtRight);
			if ((objectTestItemIndex % 2) == 0)
			{
				Assert.IsTrue(itemIndexNearestAtRight == (objectTestItemIndex / 2) + 1);
				if (itemIndexNearestAtRight < items.Count)
				{
					Assert.IsTrue(itemNearestAtRight == objectTestItemIndex + 2);
				}
				else if (itemIndexNearestAtRight == items.Count)
				{
					Assert.IsTrue(objectTestItemIndex == lastTestItemEven);
				}
				else
				{
					Assert.Fail();
				}
			}
			else
			{
				Assert.IsTrue(itemIndexNearestAtRight == (objectTestItemIndex / 2) + 1);
				if (itemIndexNearestAtRight < items.Count)
				{
					Assert.IsTrue(itemNearestAtRight == objectTestItemIndex + 1);
				}
				else if (itemIndexNearestAtRight == items.Count)
				{
					Assert.IsTrue(objectTestItemIndex == lastTestItemOdd);
				}
				else
				{
					Assert.Fail();
				}
			}
		}
	}
}