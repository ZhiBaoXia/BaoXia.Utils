using BaoXia.Utils.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaoXia.Utils.Test.Collections
{
        [TestClass]
        public class LinkedItemsTest
        {
                public static int _itemId = 0;

                public class Item : LinkedItem<Item>
                {
                        public int Id { get; set; }

                        public Item()
                        {
                                this.Id = _itemId++;
                        }
                }


                public static LinkedItems<Item> _testLinkedItems = new()
                {
                        new Item(),
                        new Item(),
                        new Item(),
                };

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
                                        Assert.IsTrue(itemNeedAdded == itemAdded);
                                }
                        }
                        Assert.IsTrue((lastTestLinkedItemsCount + testAddCount)
                                == _testLinkedItems.Count);
                }

                [TestMethod]
                public void InsertBeforeTest()
                {
                        _testLinkedItems.Clear();
                        {
                                Assert.IsTrue(_testLinkedItems.Count == 0);
                                Assert.IsTrue(_testLinkedItems.First == null);
                                Assert.IsTrue(_testLinkedItems.Last == null);
                        }

                        var lastTestLinkedItemsCount = _testLinkedItems.Count;
                        var testInsertCount = 0;
                        {
                                Item? prevItem = null;
                                Item? nextItem = null;
                                var itemNeedInserted = new Item();
                                var itemInserted = _testLinkedItems.InsertBefore(nextItem, itemNeedInserted);
                                {
                                        Assert.IsTrue(itemInserted == itemNeedInserted);
                                        Assert.IsTrue(itemInserted.Prev == prevItem);
                                        Assert.IsTrue(itemInserted.Next == nextItem);
                                        Assert.IsTrue(_testLinkedItems.First == itemInserted);
                                        Assert.IsTrue(_testLinkedItems.Last == itemInserted);
                                }
                                testInsertCount++;

                                prevItem = _testLinkedItems.Last;
                                nextItem = null;
                                itemNeedInserted = new Item();
                                itemInserted = _testLinkedItems.InsertBefore(nextItem, itemNeedInserted);
                                {
                                        Assert.IsTrue(itemInserted == itemNeedInserted);
                                        Assert.IsTrue(itemInserted.Prev == prevItem);
                                        Assert.IsTrue(itemInserted.Next == nextItem);
                                        Assert.IsTrue(prevItem.Next == itemInserted);
                                        Assert.IsTrue(_testLinkedItems.Last == itemInserted);
                                }
                                testInsertCount++;

                                prevItem = itemInserted.Prev;
                                nextItem = itemInserted;
                                itemNeedInserted = new Item();
                                itemInserted = _testLinkedItems.InsertBefore(nextItem, itemNeedInserted);
                                {
                                        Assert.IsTrue(itemInserted == itemNeedInserted);
                                        Assert.IsTrue(itemInserted.Prev == prevItem);
                                        Assert.IsTrue(itemInserted.Next == nextItem);
                                        Assert.IsTrue(nextItem.Prev == itemInserted);
                                }
                                testInsertCount++;

                                prevItem = null;
                                nextItem = _testLinkedItems.First;
                                itemNeedInserted = new Item();
                                itemInserted = _testLinkedItems.InsertBefore(nextItem, itemNeedInserted);
                                {
                                        Assert.IsTrue(itemInserted == itemNeedInserted);
                                        Assert.IsTrue(itemInserted.Prev == prevItem);
                                        Assert.IsTrue(itemInserted.Next == nextItem);
                                        Assert.IsTrue(nextItem.Prev == itemInserted);
                                        Assert.IsTrue(_testLinkedItems.First == itemInserted);
                                }
                                testInsertCount++;
                        }
                        Assert.IsTrue((lastTestLinkedItemsCount + testInsertCount)
                                == _testLinkedItems.Count);
                }

                [TestMethod]
                public void InsertAfterTest()
                {
                        _testLinkedItems.Clear();
                        {
                                Assert.IsTrue(_testLinkedItems.Count == 0);
                                Assert.IsTrue(_testLinkedItems.First == null);
                                Assert.IsTrue(_testLinkedItems.Last == null);
                        }

                        var lastTestLinkedItemsCount = _testLinkedItems.Count;
                        var testInsertCount = 0;
                        {
                                Item? prevItem = null;
                                Item? nextItem = null;
                                var itemNeedInserted = new Item();
                                var itemInserted = _testLinkedItems.InsertAfter(prevItem, itemNeedInserted);
                                {
                                        Assert.IsTrue(itemInserted == itemNeedInserted);
                                        Assert.IsTrue(itemInserted.Prev == prevItem);
                                        Assert.IsTrue(itemInserted.Next == nextItem);
                                        Assert.IsTrue(_testLinkedItems.First == itemInserted);
                                        Assert.IsTrue(_testLinkedItems.Last == itemInserted);
                                }
                                testInsertCount++;

                                prevItem = _testLinkedItems.Last;
                                nextItem = null;
                                itemNeedInserted = new Item();
                                itemInserted = _testLinkedItems.InsertAfter(prevItem, itemNeedInserted);
                                {
                                        Assert.IsTrue(itemInserted == itemNeedInserted);
                                        Assert.IsTrue(itemInserted.Prev == prevItem);
                                        Assert.IsTrue(itemInserted.Next == nextItem);
                                        Assert.IsTrue(prevItem.Next == itemInserted);
                                        Assert.IsTrue(_testLinkedItems.Last == itemInserted);
                                }
                                testInsertCount++;

                                prevItem = itemInserted.Prev;
                                nextItem = itemInserted;
                                itemNeedInserted = new Item();
                                itemInserted = _testLinkedItems.InsertAfter(prevItem, itemNeedInserted);
                                {
                                        Assert.IsTrue(itemInserted == itemNeedInserted);
                                        Assert.IsTrue(itemInserted.Prev == prevItem);
                                        Assert.IsTrue(itemInserted.Next == nextItem);
                                        Assert.IsTrue(nextItem.Prev == itemInserted);
                                }
                                testInsertCount++;

                                prevItem = null;
                                nextItem = _testLinkedItems.First;
                                itemNeedInserted = new Item();
                                itemInserted = _testLinkedItems.InsertAfter(prevItem, itemNeedInserted);
                                {
                                        Assert.IsTrue(itemInserted == itemNeedInserted);
                                        Assert.IsTrue(itemInserted.Prev == prevItem);
                                        Assert.IsTrue(itemInserted.Next == nextItem);
                                        Assert.IsTrue(nextItem.Prev == itemInserted);
                                        Assert.IsTrue(_testLinkedItems.First == itemInserted);
                                }
                                testInsertCount++;
                        }
                        Assert.IsTrue((lastTestLinkedItemsCount + testInsertCount)
                                == _testLinkedItems.Count);
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
                                                                Assert.IsTrue(itemRemoved == firstItem);
                                                                Assert.IsTrue(_testLinkedItems.Count == 2);
                                                                Assert.IsTrue(_testLinkedItems.First == midItem);
                                                                Assert.IsTrue(_testLinkedItems.Last == lastItem);
                                                        }
                                                }
                                                break;
                                        case 1:
                                                {
                                                        var itemRemoved = _testLinkedItems.Remove(midItem);
                                                        {
                                                                Assert.IsTrue(itemRemoved == midItem);
                                                                Assert.IsTrue(_testLinkedItems.Count == 2);
                                                                Assert.IsTrue(_testLinkedItems.First == firstItem);
                                                                Assert.IsTrue(_testLinkedItems.Last == lastItem);
                                                        }
                                                }
                                                break;
                                        case 2:
                                                {
                                                        var itemRemoved = _testLinkedItems.Remove(lastItem);
                                                        {
                                                                Assert.IsTrue(itemRemoved == lastItem);
                                                                Assert.IsTrue(_testLinkedItems.Count == 2);
                                                                Assert.IsTrue(_testLinkedItems.First == firstItem);
                                                                Assert.IsTrue(_testLinkedItems.Last == midItem);
                                                        }
                                                }
                                                break;
                                }
                                Assert.IsTrue(_testLinkedItems.First.Prev == null);
                                Assert.IsTrue(_testLinkedItems.First.Next == _testLinkedItems.Last);
                                Assert.IsTrue(_testLinkedItems.Last.Prev == _testLinkedItems.First);
                                Assert.IsTrue(_testLinkedItems.Last.Next == null);
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
                                        Assert.IsTrue(item == _testLinkedItems.First);
                                        Assert.IsTrue(item.Prev == null);
                                }
                                else
                                {
                                        Assert.IsTrue((lastItem.Id + 1) == item.Id);
                                }
                                lastItem = item;
                                testsCount--;
                        }
                        Assert.IsTrue(testsCount == 0);
                }
        }
}
