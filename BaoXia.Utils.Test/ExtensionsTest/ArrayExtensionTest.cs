using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace BaoXia.Utils.Test.ExtensionsTest
{
        [TestClass]
        public class ArrayExtensionTest
        {
                public static void CreateArrayByAddInsertAndRemoveTest<T>(
                        int testArrayLength,
                        T newValue)
                {
                        var items = new T?[testArrayLength];
                        for (var itemIndex = 0;
                                itemIndex < items.Length;
                                itemIndex++)
                        {
                                items[itemIndex] = default;
                        }

                        items = items.ArrayByAdd(newValue);
                        // !!!
                        Assert.IsTrue(object.Equals(items[^1], newValue) == true);
                        // !!!

                        items = items.ArrayByInsertAt(
                                0,
                                newValue);
                        // !!!
                        Assert.IsTrue(object.Equals(items[0], newValue) == true);
                        // !!!

                        var insertIndex = testArrayLength / 2;
                        items = items.ArrayByInsertAt(
                                insertIndex,
                                newValue);
                        // !!!
                        Assert.IsTrue(object.Equals(items[insertIndex], newValue) == true);
                        // !!!

                        items = items.ArrayByRemoveAt(insertIndex);
                        items = items.ArrayByRemoveAt(0);
                        items = items.ArrayByRemoveAt(items.Length - 1);
                        // !!!
                        Assert.IsTrue(items.Length == testArrayLength);
                        // !!!

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
                                        (testItem) =>
                                        {
                                                return testItem.Index.CompareTo(objectTestItem.Index);
                                        });
                                Assert.IsTrue(itemIndexFound == i);
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
                                        testSearchRangeBeginIndex,
                                        testSearchRangeLength,
                                        (testItem) =>
                                        {
                                                return testItem.Index.CompareTo(objectTestItem.Index);
                                        });
                                Assert.IsTrue(itemIndexFound == i);
                        }


                        for (var i = 0; i < items.Length; i++)
                        {
                                var objectTestItem = items[i];
                                var itemFound = items.FindItemWithDichotomy((testItem) =>
                                {
                                        return testItem.Index.CompareTo(objectTestItem.Index);
                                });
                                Assert.IsTrue(itemFound?.Index == objectTestItem.Index);
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
                                        testSearchRangeBeginIndex,
                                        testSearchRangeLength,
                                        (testItem) =>
                                        {
                                                return testItem.Index.CompareTo(objectTestItem.Index);
                                        });
                                Assert.IsTrue(itemFound?.Index == objectTestItem.Index);
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
                        Assert.IsTrue(testItems.Length == testItemsOriginally.Length);
                        // !!!


                        var removeItemIndex = 4;
                        testItems
                                = testItemsOriginally.ArrayByRemove(
                                        testItemsOriginally[removeItemIndex]);
                        // !!!
                        Assert.IsTrue((testItems.Length + 1) == testItemsOriginally.Length);
                        // !!!
                        foreach (var item in testItemsOriginally)
                        {
                                if (testItems.IndexOf(item) < 0)
                                {
                                        // !!!
                                        Assert.IsTrue(testItemsOriginally.IndexOf(item) == removeItemIndex);
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
                        Assert.IsTrue((testItems.Length + 1) == testItemsOriginally.Length);
                        // !!!
                        foreach (var item in testItemsOriginally)
                        {
                                if (testItems.IndexOf(item) < 0)
                                {
                                        // !!!
                                        Assert.IsTrue(item.Number == removeItemNumber);
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
                        Assert.IsTrue(testItems.Length * sameItemsRate == testItemsOriginally.Length);
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
                        Assert.IsTrue(testItems.Length == testItemsOriginally.Length);
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
                        Assert.IsTrue(testItems.Length + removeItemsCount == testItemsOriginally.Length);
                        // !!!

                }
        }
}
