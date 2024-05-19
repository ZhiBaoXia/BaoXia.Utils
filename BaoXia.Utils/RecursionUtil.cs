using BaoXia.Utils.Models;
using System;
using System.Collections.Generic;

namespace BaoXia.Utils;
public class RecursionUtil
{
        ////////////////////////////////////////////////
        // @类方法
        ////////////////////////////////////////////////

        #region 类方法

        public static void Enumerate<ItemType>(
                ItemType? rootItem,
                Func<ItemType, IList<ItemType>?> ToGetChildItems,
                Func<ItemType?, ItemType, bool> ToEnumerateItem)
        {
                if (rootItem == null)
                {
                        return;
                }

                var rootItems = new List<ItemType>();
                {
                        rootItems.Add(rootItem);
                }

                var recursionSteps = new Stack<RecursionStep<ItemType>>();
                {
                        recursionSteps.Push(new(default, rootItems, 0));
                }

                while (recursionSteps.Count > 0)
                {
                        var currentRecursionStep = recursionSteps.Peek();
                        if (currentRecursionStep == null)
                        {
                                // !!!
                                recursionSteps.Pop();
                                continue;
                                // !!!
                        }

                        var parentItem = currentRecursionStep.ParentItem;
                        var items = currentRecursionStep.Items;
                        var itemIndex = currentRecursionStep.NextItemIndex;
                        var itemsCount = items.Count;
                        if (itemIndex < 0
                                || itemIndex >= itemsCount)
                        {
                                // !!!
                                recursionSteps.Pop();
                                continue;
                                // !!!
                        }

                        for (;
                                itemIndex < itemsCount;
                                itemIndex++)
                        {
                                var item = items[itemIndex];

                                ////////////////////////////////////////////////
                                if (!ToEnumerateItem(parentItem, item))
                                {
                                        return;
                                }
                                ////////////////////////////////////////////////

                                var childItems = ToGetChildItems(item);
                                if (childItems == null
                                        || childItems.Count < 1)
                                {
                                        continue;
                                }

                                // !!!
                                currentRecursionStep.NextItemIndex = itemIndex + 1;
                                // !!!
                                recursionSteps.Push(new(item, childItems, 0));
                                break;
                                // !!!
                        }
                        if (itemIndex >= itemsCount)
                        {
                                // !!!
                                recursionSteps.Pop();
                                // !!!
                        }
                }
        }

        #endregion
}