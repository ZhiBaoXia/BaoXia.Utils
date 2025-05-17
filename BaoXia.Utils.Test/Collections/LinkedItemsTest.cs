using BaoXia.Utils.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaoXia.Utils.Test.Collections
{
	[TestClass]
	public class LinkedItemsTest
	{
		static int _itemId = 0;

		public class Item : LinkedItem<Item>
		{
			public int Id { get; set; }

			public Item()
			{
				this.Id = _itemId++;
			}
		}


		static readonly LinkedItems<Item> _testLinkedItems =
		[
			new Item(),
			new Item(),
			new Item(),
		];

		[TestMethod]
		public void AddTest()
		{
			var lastTestLinkedItemsCount = _testLinkedItems.Count;
			var testAddCount = 3;
			for (var i = 0;
				i < testAddCount;
				i++)
			{
				var itemNeedAdded = new Item();
				var itemAdded = _testLinkedItems.Add(itemNeedAdded);
				{
					Assert.AreEqual(itemAdded, itemNeedAdded);
				}
			}
			Assert.AreEqual(_testLinkedItems.Count, lastTestLinkedItemsCount + testAddCount);
		}

		[TestMethod]
		public void InsertBeforeTest()
		{
			_testLinkedItems.Clear();
			{
				Assert.AreEqual(0, _testLinkedItems.Count);
				Assert.IsNull(_testLinkedItems.First);
				Assert.IsNull(_testLinkedItems.Last);
			}

			var lastTestLinkedItemsCount = _testLinkedItems.Count;
			var testInsertCount = 0;
			{
				Item? prevItem = null;
				Item? nextItem = null;
				var itemNeedInserted = new Item();
				var itemInserted = _testLinkedItems.InsertBefore(nextItem, itemNeedInserted);
				{
					Assert.AreEqual(itemNeedInserted, itemInserted);
					Assert.AreEqual(prevItem, itemInserted.Prev);
					Assert.AreEqual(nextItem, itemInserted.Next);
					Assert.AreEqual(itemInserted, _testLinkedItems.First);
					Assert.AreEqual(itemInserted, _testLinkedItems.Last);
				}
				testInsertCount++;

				prevItem = _testLinkedItems.Last;
				nextItem = null;
				itemNeedInserted = new Item();
				itemInserted = _testLinkedItems.InsertBefore(nextItem, itemNeedInserted);
				{
					Assert.AreEqual(itemNeedInserted, itemInserted);
					Assert.AreEqual(prevItem, itemInserted.Prev);
					Assert.AreEqual(nextItem, itemInserted.Next);
					Assert.AreEqual(itemInserted, prevItem!.Next);
					Assert.AreEqual(itemInserted, _testLinkedItems.Last);
				}
				testInsertCount++;

				prevItem = itemInserted.Prev;
				nextItem = itemInserted;
				itemNeedInserted = new Item();
				itemInserted = _testLinkedItems.InsertBefore(nextItem, itemNeedInserted);
				{
					Assert.AreEqual(itemNeedInserted, itemInserted);
					Assert.AreEqual(prevItem, itemInserted.Prev);
					Assert.AreEqual(nextItem, itemInserted.Next);
					Assert.AreEqual(itemInserted, nextItem.Prev);
				}
				testInsertCount++;

				prevItem = null;
				nextItem = _testLinkedItems.First;
				itemNeedInserted = new Item();
				itemInserted = _testLinkedItems.InsertBefore(nextItem, itemNeedInserted);
				{
					Assert.AreEqual(itemNeedInserted, itemInserted);
					Assert.AreEqual(prevItem, itemInserted.Prev);
					Assert.AreEqual(nextItem, itemInserted.Next);
					Assert.AreEqual(itemInserted, nextItem!.Prev);
					Assert.AreEqual(itemInserted, _testLinkedItems.First);
				}
				testInsertCount++;
			}
			Assert.AreEqual(_testLinkedItems.Count, lastTestLinkedItemsCount + testInsertCount);
		}

		[TestMethod]
		public void InsertAfterTest()
		{
			_testLinkedItems.Clear();
			{
				Assert.AreEqual(0, _testLinkedItems.Count);
				Assert.IsNull(_testLinkedItems.First);
				Assert.IsNull(_testLinkedItems.Last);
			}

			var lastTestLinkedItemsCount = _testLinkedItems.Count;
			var testInsertCount = 0;
			{
				Item? prevItem = null;
				Item? nextItem = null;
				var itemNeedInserted = new Item();
				var itemInserted = _testLinkedItems.InsertAfter(prevItem, itemNeedInserted);
				{
					Assert.AreEqual(itemNeedInserted, itemInserted);
					Assert.AreEqual(prevItem, itemInserted.Prev);
					Assert.AreEqual(nextItem, itemInserted.Next);
					Assert.AreEqual(itemInserted, _testLinkedItems.First);
					Assert.AreEqual(itemInserted, _testLinkedItems.Last);
				}
				testInsertCount++;

				prevItem = _testLinkedItems.Last;
				nextItem = null;
				itemNeedInserted = new Item();
				itemInserted = _testLinkedItems.InsertAfter(prevItem, itemNeedInserted);
				{
					Assert.AreEqual(itemNeedInserted, itemInserted);
					Assert.AreEqual(prevItem, itemInserted.Prev);
					Assert.AreEqual(nextItem, itemInserted.Next);
					Assert.AreEqual(itemInserted, prevItem!.Next);
					Assert.AreEqual(itemInserted, _testLinkedItems.Last);
				}
				testInsertCount++;

				prevItem = itemInserted.Prev;
				nextItem = itemInserted;
				itemNeedInserted = new Item();
				itemInserted = _testLinkedItems.InsertAfter(prevItem, itemNeedInserted);
				{
					Assert.AreEqual(itemNeedInserted, itemInserted);
					Assert.AreEqual(prevItem, itemInserted.Prev);
					Assert.AreEqual(nextItem, itemInserted.Next);
					Assert.AreEqual(itemInserted, nextItem.Prev);
				}
				testInsertCount++;

				prevItem = null;
				nextItem = _testLinkedItems.First;
				itemNeedInserted = new Item();
				itemInserted = _testLinkedItems.InsertAfter(prevItem, itemNeedInserted);
				{
					Assert.AreEqual(itemNeedInserted, itemInserted);
					Assert.AreEqual(prevItem, itemInserted.Prev);
					Assert.AreEqual(nextItem, itemInserted.Next);
					Assert.AreEqual(itemInserted, nextItem!.Prev);
					Assert.AreEqual(itemInserted, _testLinkedItems.First);
				}
				testInsertCount++;
			}
			Assert.AreEqual(_testLinkedItems.Count, lastTestLinkedItemsCount + testInsertCount);
		}

		[TestMethod]
		public void RemoveTest()
		{
			for (var testIndex = 0;
				testIndex < 3;
				testIndex++)
			{
				_testLinkedItems.Clear();

				var firstItem = new Item();
				_testLinkedItems.Add(firstItem);

				var midItem = new Item();
				_testLinkedItems.Add(midItem);

				var lastItem = new Item();
				_testLinkedItems.Add(lastItem);

				switch (testIndex)
				{
					default:
					case 0:
						{
							var itemRemoved = _testLinkedItems.Remove(firstItem);
							{
								Assert.AreEqual(firstItem, itemRemoved);
								Assert.AreEqual(2, _testLinkedItems.Count);
								Assert.AreEqual(midItem, _testLinkedItems.First);
								Assert.AreEqual(lastItem, _testLinkedItems.Last);
							}
						}
						break;
					case 1:
						{
							var itemRemoved = _testLinkedItems.Remove(midItem);
							{
								Assert.AreEqual(midItem, itemRemoved);
								Assert.AreEqual(2, _testLinkedItems.Count);
								Assert.AreEqual(firstItem, _testLinkedItems.First);
								Assert.AreEqual(lastItem, _testLinkedItems.Last);
							}
						}
						break;
					case 2:
						{
							var itemRemoved = _testLinkedItems.Remove(lastItem);
							{
								Assert.AreEqual(lastItem, itemRemoved);
								Assert.AreEqual(2, _testLinkedItems.Count);
								Assert.AreEqual(firstItem, _testLinkedItems.First);
								Assert.AreEqual(midItem, _testLinkedItems.Last);
							}
						}
						break;
				}
				Assert.IsNull(_testLinkedItems.First!.Prev);
				Assert.AreEqual(_testLinkedItems.Last, _testLinkedItems.First.Next);
				Assert.AreEqual(_testLinkedItems.First, _testLinkedItems.Last!.Prev);
				Assert.IsNull(_testLinkedItems.Last.Next);
			}
		}

		[TestMethod]
		public void ForEachTest()
		{
			_testLinkedItems.Clear();

			var testsCount = 99;
			for (var testIndex = 0;
				testIndex < testsCount;
				testIndex++)
			{
				_testLinkedItems.Add(new Item());
			}

			Item? lastItem = null;
			foreach (var item in _testLinkedItems)
			{
				if (lastItem == null)
				{
					Assert.AreEqual(_testLinkedItems.First, item);
					Assert.IsNull(item.Prev);
				}
				else
				{
					Assert.AreEqual(item.Id, lastItem.Id + 1);
				}
				lastItem = item;
				testsCount--;
			}
			Assert.AreEqual(0, testsCount);
		}
	}
}
