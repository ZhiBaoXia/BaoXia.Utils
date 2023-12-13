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
		var testList = TestItems.ToList();
		Assert.IsTrue(testList.RemoveFrom(-1, 1) == 2);
		// !!!

		// !!!
		testList = TestItems.ToList();
		Assert.IsTrue(testList.RemoveFrom(-1) == 5);
		// !!!

		// !!!
		testList = TestItems.ToList();
		Assert.IsTrue(testList.RemoveFrom(0, 1) == 2);
		// !!!

		// !!!
		testList = TestItems.ToList();
		Assert.IsTrue(testList.RemoveFrom(1) == 4);
		// !!!

		// !!!
		testList = TestItems.ToList();
		Assert.IsTrue(testList.RemoveFrom(3, 5) == 2);
		// !!!

		// !!!
		testList = TestItems.ToList();
		Assert.IsTrue(testList.RemoveFrom(3) == 2);
		// !!!

		// !!!
		testList = TestItems.ToList();
		Assert.IsTrue(testList.RemoveFrom(4) == 1);
		// !!!

		// !!!
		testList = TestItems.ToList();
		Assert.IsTrue(testList.RemoveFrom(5) == 0);
		// !!!
	}


	class TestItem
	{
		public int Index { get; set; }

		public TestItem(int index)
		{
			this.Index = index;
		}
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
				(testItem) =>
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
				testSearchRangeBeginIndex,
				testSearchRangeLength,
				(testItem) =>
				{
					return testItem.Index.CompareTo(objectTestItem.Index);
				});
			Assert.IsTrue(itemIndexFound == i);
		}


		for (var i = 0; i < list.Count; i++)
		{
			var objectTestItem = list[i];
			var itemFound = list.FindItemWithDichotomy((testItem) =>
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
				testSearchRangeBeginIndex,
				testSearchRangeLength,
				(testItem) =>
				{
					return testItem.Index.CompareTo(objectTestItem.Index);
				});
			Assert.IsTrue(itemFound?.Index == objectTestItem.Index);
		}
	}
}
