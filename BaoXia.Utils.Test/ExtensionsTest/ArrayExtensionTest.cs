﻿using BaoXia.Utils.Constants;
using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BaoXia.Utils.Test.ExtensionsTest;

[TestClass]
public class ArrayExtensionTest
{
	public static void CreateArrayByAddInsertAndRemoveTest<T>(
		int testArrayLength,
		params T[] newValues)
	{
		var items = new T[testArrayLength];
		for (var itemIndex = 0;
			itemIndex < items.Length;
			itemIndex++)
		{
			items[itemIndex] = default!;
		}

		foreach (var newValue in newValues)
		{
			items = items.ArrayByAdd(newValue);
			// !!!
			Assert.IsTrue(object.Equals(items[^1], newValue));
			// !!!
		}

		foreach (var newValue in newValues)
		{
			items = items.ArrayByInsertAt(
			0,
			newValue);
			// !!!
			Assert.IsTrue(object.Equals(items[0], newValue));
			// !!!
		}

		foreach (var newValue in newValues)
		{
			var insertIndex = testArrayLength / 2;
			items = items.ArrayByInsertAt(
				insertIndex,
				newValue);
			// !!!
			Assert.IsTrue(object.Equals(items[insertIndex], newValue));
			// !!!

			items = items.ArrayByRemoveAt(insertIndex);
			items = items.ArrayByRemoveAt(0);
			items = items.ArrayByRemoveAt(items.Length - 1);
			// !!!
			Assert.AreEqual(testArrayLength, items.Length);
			// !!!
		}

		foreach (var newValuesInsertIndex
			in
			new int[] { 0, items.Length, items.Length / 2 })
		{
			items = items.ArrayByInsertAt(newValuesInsertIndex, newValues);
			for (var valueIndex = 0;
				valueIndex < newValues.Length;
				valueIndex++)
			{
				var newValue = newValues[valueIndex];
				// !!!
				Assert.IsTrue(object.Equals(items[newValuesInsertIndex + valueIndex], newValue));
				// !!!
			}
			items = items.ArrayByRemoveFrom(newValuesInsertIndex, newValues.Length);
		}


		for (var itemIndex = 0;
			itemIndex < items.Length;
			itemIndex++)
		{
			// !!!
			Assert.IsTrue(object.Equals(items[itemIndex], default(T)));
			// !!!
		}
	}

	[TestMethod]
	public void InsertByOrderTest()
	{
		int[] itemsInOrder = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
		int[] itemsInOrderDescending = [.. itemsInOrder.Reverse()];
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

		var arrayWithOrder = Array.Empty<int>();
		foreach (var item in itemInRandomOrder)
		{
			arrayWithOrder
				= arrayWithOrder.ArrayByInsertWithOrder(
				item,
				(itemA, itemB) =>
				{
					return itemA.CompareTo(itemB);
				});
		}
		////////////////////////////////////////////////
		// !!!
		Assert.IsTrue(arrayWithOrder.SequenceEqual(itemsInOrder));
		// !!!
		////////////////////////////////////////////////
		///

		var arrayWithOrderDescending = Array.Empty<int>();
		foreach (var item in itemInRandomOrder)
		{
			arrayWithOrderDescending
				= arrayWithOrderDescending.ArrayByInsertWithOrderDescending(
				item,
				(itemA, itemB) =>
				{
					return itemA.CompareTo(itemB);
				});
		}
		////////////////////////////////////////////////
		// !!!
		Assert.IsTrue(arrayWithOrderDescending.SequenceEqual(itemsInOrderDescending));
		// !!!
		////////////////////////////////////////////////
	}

	[TestMethod]
	public void CreateArrayByAddInsertAndRemoveTest()
	{
		const int testArrayLength = 10;

		// int[]
		CreateArrayByAddInsertAndRemoveTest(
			testArrayLength,
			1);

		// string[]
		CreateArrayByAddInsertAndRemoveTest(
			testArrayLength,
			"Abc");

		// object[]
		CreateArrayByAddInsertAndRemoveTest(
			testArrayLength,
			new object());

	}

	class TestItem(int index)
	{
		public int Index { get; set; } = index;
	}

	[TestMethod]
	public void FindItemIndexWithDichotomyTest()
	{
		var itemList = new List<TestItem>();
		var random = new System.Random();
		for (int i = 0; i < 100; i++)
		{
			itemList.Add(new TestItem(random.Next()));
		}
		itemList.Sort((itemA, itemB) =>
		{
			return itemA.Index.CompareTo(itemB.Index);
		});

		var items = itemList.ToArray();
		for (var i = 0; i < items.Length; i++)
		{
			var objectTestItem = items[i];
			var itemIndexFound = items.FindItemIndexWithDichotomy(
				true,
				(testItem, testItemIndex) =>
				{
					return testItem.Index.CompareTo(objectTestItem.Index);
				});
			Assert.AreEqual(i, itemIndexFound);
		}

		var testSearchRangeBeginIndex
			= random.Next(items.Length);
		var testSearchRangeLength
			= random.Next(items.Length - testSearchRangeBeginIndex);
		for (var i = testSearchRangeBeginIndex;
			i < testSearchRangeBeginIndex + testSearchRangeLength;
			i++)
		{
			var objectTestItem = items[i];
			var itemIndexFound = items.FindItemIndexWithDichotomyInRange(
				true,
				testSearchRangeBeginIndex,
				testSearchRangeLength,
				(testItem, testItemIndex) =>
				{
					return testItem.Index.CompareTo(objectTestItem.Index);
				});
			Assert.AreEqual(i, itemIndexFound);
		}


		for (var i = 0; i < items.Length; i++)
		{
			var objectTestItem = items[i];
			var itemFound = items.FindItemWithDichotomy(
				true,
				(testItem, testItemIndex) =>
				{
					return testItem.Index.CompareTo(objectTestItem.Index);
				});
			Assert.IsNotNull(itemFound);
			Assert.AreEqual(objectTestItem.Index, itemFound.Index);
		}

		testSearchRangeBeginIndex
			= random.Next(items.Length);
		testSearchRangeLength
			= random.Next(items.Length - testSearchRangeBeginIndex);
		for (var i = testSearchRangeBeginIndex;
			i < testSearchRangeBeginIndex + testSearchRangeLength;
			i++)
		{
			var objectTestItem = items[i];
			var itemFound = items.FindItemWithDichotomyInRange(
				true,
				testSearchRangeBeginIndex,
				testSearchRangeLength,
				(testItem, testItemIndex) =>
				{
					return testItem.Index.CompareTo(objectTestItem.Index);
				});
			Assert.IsNotNull(itemFound);
			Assert.AreEqual(objectTestItem.Index, itemFound.Index);
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

		var items = itemList.ToArray();

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
				Assert.AreEqual((objectTestItemIndex / 2) - 1, itemIndexNearestAtLeft);
				if (itemIndexNearestAtLeft >= 0)
				{
					Assert.AreEqual(objectTestItemIndex - 2, itemNearestAtLeft);
				}
				else if (itemIndexNearestAtLeft == -1)
				{
					Assert.AreEqual(firsttTestItemEven, itemNearestAtLeft);
				}
				else
				{
					Assert.Fail();
				}
			}
			else
			{
				Assert.AreEqual((objectTestItemIndex / 2), itemIndexNearestAtLeft);
				Assert.AreEqual(objectTestItemIndex - 1, itemNearestAtLeft);
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
				Assert.AreEqual((objectTestItemIndex / 2) + 1, itemIndexNearestAtRight);
				if (itemIndexNearestAtRight < items.Length)
				{
					Assert.AreEqual(objectTestItemIndex + 2, itemNearestAtRight);
				}
				else if (itemIndexNearestAtRight == items.Length)
				{
					Assert.AreEqual(lastTestItemEven, objectTestItemIndex);
				}
				else
				{
					Assert.Fail();
				}
			}
			else
			{
				Assert.AreEqual((objectTestItemIndex / 2) + 1, itemIndexNearestAtRight);
				if (itemIndexNearestAtRight < items.Length)
				{
					Assert.AreEqual(objectTestItemIndex + 1, itemNearestAtRight);
				}
				else if (itemIndexNearestAtRight == items.Length)
				{
					Assert.AreEqual(lastTestItemOdd, objectTestItemIndex);
				}
				else
				{
					Assert.Fail();
				}
			}
		}
	}

	class TestObjectWithoutEqual : object
	{
		public int Number { get; set; }
	}

	class TestObjectWithEqual : object
	{
		public int Number { get; set; }

		public override bool Equals(object? obj)
		{
			if (obj is TestObjectWithEqual testObject)
			{
				if (testObject.Number == Number)
				{
					return true;
				}
			}
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return Number;
		}
	}

	[TestMethod]
	public void ArrayByRemoveTest()
	{
		var testItemsOriginally = new TestObjectWithoutEqual[10];
		for (var i = 0; i < testItemsOriginally.Length; i++)
		{
			testItemsOriginally[i] = new TestObjectWithoutEqual()
			{
				Number = i + 1,
			};
		}

		var testItems
			= testItemsOriginally.ArrayByRemove(
			new TestObjectWithoutEqual()
			{
				Number = 5
			});
		// !!!
		Assert.AreEqual(testItemsOriginally.Length, testItems.Length);
		// !!!


		var removeItemIndex = 4;
		testItems
			= testItemsOriginally.ArrayByRemove(
				testItemsOriginally[removeItemIndex]);
		// !!!
		Assert.AreEqual(testItemsOriginally.Length, (testItems.Length + 1));
		// !!!
		foreach (var item in testItemsOriginally)
		{
			if (testItems.IndexOf(item) < 0)
			{
				// !!!
				Assert.AreEqual(removeItemIndex, testItemsOriginally.IndexOf(item));
				// !!!
			}
		}


		var removeItemNumber = 5;
		testItems
			= testItemsOriginally.ArrayByRemove(
				null,
				(item) =>
				{
					if (item.Number == removeItemNumber)
					{
						return true;
					}
					return false;
				});
		// !!!
		Assert.AreEqual(testItemsOriginally.Length, (testItems.Length + 1));
		// !!!
		foreach (var item in testItemsOriginally)
		{
			if (testItems.IndexOf(item) < 0)
			{
				// !!!
				Assert.AreEqual(removeItemNumber, item.Number);
				// !!!
			}
		}
	}


	[TestMethod]
	public void ArrayByRemoveDuplicateItemsTest()
	{
		var sameItemsRate = 2;
		var testItemsOriginally = new object[sameItemsRate * 5];


		for (var i = 0; i < testItemsOriginally.Length; i++)
		{
			testItemsOriginally[i] = new TestObjectWithEqual()
			{
				Number = i / sameItemsRate,
			};
		}
		var testItems = testItemsOriginally.ArrayByRemoveDuplicateItems();
		// !!!
		Assert.AreEqual(testItemsOriginally.Length, testItems.Length * sameItemsRate);
		// !!!


		for (var i = 0; i < testItemsOriginally.Length; i++)
		{
			testItemsOriginally[i] = new TestObjectWithoutEqual()
			{
				Number = i / sameItemsRate,
			};
		}
		testItems = testItemsOriginally.ArrayByRemoveDuplicateItems();
		// !!!
		Assert.AreEqual(testItemsOriginally.Length, testItems.Length);
		// !!!


		var removeItemsCountMax = 4;
		var removeItemsCount = 0;
		testItems
			= testItemsOriginally.ArrayByRemoveDuplicateItems(
				(itemA, itemB) =>
				{
					if (removeItemsCount < removeItemsCountMax)
					{
						removeItemsCount++;
						return true;
					}
					return false;
				});
		// !!!
		Assert.AreEqual(testItemsOriginally.Length, testItems.Length + removeItemsCount);
		// !!!

	}
}
