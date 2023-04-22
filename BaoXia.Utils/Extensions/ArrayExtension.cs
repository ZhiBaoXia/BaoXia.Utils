﻿using System;
using System.Collections.Generic;

namespace BaoXia.Utils.Extensions
{
        /// <summary>
        /// 数组扩展类。
        /// </summary>
        public static class ArrayExtension
        {
                /// <summary>
                /// 获取目标元素在当前数组中第一次出现的索引值。
                /// </summary>
                /// <typeparam name="ItemType">当前数组元素类型。</typeparam>
                /// <param name="items">当前数组。</param>
                /// <param name="objectItem">目标元素。</param>
                /// <returns>返回目标元素在数组中第一次出现的索引值，目标元素不存在时返回“-1”。</returns>
                public static int IndexOf<ItemType>(
                        this ItemType[] items,
                        ItemType? objectItem)
                {
                        return Array.IndexOf(items, objectItem);
                }

                /// <summary>
                /// 通过在指定位置上插入元素，创建新的元素数组。
                /// </summary>
                /// <typeparam name="ItemType">数组元素类型。</typeparam>
                /// <param name="items">当前数组。</param>
                /// <param name="insertItemIndex">要插入元素的索引值。</param>
                /// <returns>插入元素后新建的数组对象，即使新数组的长度为0，仍会返回有效的数组对象。 </returns>
                public static ItemType[] ArrayByInsertAt<ItemType>(
                        this ItemType[] items,
                        int insertItemIndex,
                        ItemType newItem)
                {
                        if (insertItemIndex < 0
                                || insertItemIndex > items.Length)
                        {
                                throw new IndexOutOfRangeException();
                        }

                        ////////////////////////////////////////////////

                        var newItems = new ItemType[items.Length + 1];
                        {
                                Array.Copy(
                                        items,
                                        newItems,
                                        insertItemIndex);
                                // !!!
                                newItems[insertItemIndex] = newItem;
                                // !!!
                                Array.Copy(
                                        items,
                                        insertItemIndex,
                                        newItems,
                                        insertItemIndex + 1,
                                        items.Length - insertItemIndex);
                        }
                        return newItems;
                }

                /// <summary>
                /// 通过在新增元素，创建新的元素数组。
                /// </summary>
                /// <typeparam name="ItemType">数组元素类型。</typeparam>
                /// <param name="items">当前数组。</param>
                /// <returns>新增元素后新建的数组对象，即使新数组的长度为0，仍会返回有效的数组对象。 </returns>
                public static ItemType[] ArrayByAdd<ItemType>(
                        this ItemType[] items,
                        ItemType newItem)
                {
                        return items.ArrayByInsertAt(
                                items.Length,
                                newItem);
                }

                /// <summary>
                /// 通过移除指定位置上的元素，创建新的元素数组。
                /// </summary>
                /// <typeparam name="ItemType">数组元素类型。</typeparam>
                /// <param name="items">当前数组。</param>
                /// <param name="removeItemIndex">要移除元素的索引值。</param>
                /// <returns>移除元素后新建的数组对象，即使新数组的长度为0，仍会返回有效的数组对象。 </returns>
                public static ItemType[] ArrayByRemoveAt<ItemType>(
                        this ItemType[] items,
                        int removeItemIndex)
                {
                        if (removeItemIndex < 0
                                || removeItemIndex >= items.Length)
                        {
                                throw new IndexOutOfRangeException();
                        }

                        var newItems = new ItemType[items.Length - 1];
                        {
                                Array.Copy(
                                        items,
                                        newItems,
                                        removeItemIndex);
                                Array.Copy(
                                        items,
                                        removeItemIndex + 1,
                                        newItems,
                                        removeItemIndex,
                                        newItems.Length - removeItemIndex);
                        }
                        return newItems;
                }

                /// <summary>
                /// 通过移除重复元素，创建新的元素数组。
                /// </summary>
                /// <typeparam name="ItemType">数组元素类型。</typeparam>
                /// <param name="items">当前数组。</param>
                /// <param name="objectItem">要删除的目标数组。</param>
                /// <param name="toIsObjectItem">指定的判断对象是否相同的回调函数。</param>
                /// <param name="isClearNull">是否清除“null”元素。</param>
                /// <returns>移除重复元素后新建的数组对象，即使新数组的长度为0，仍会返回有效的数组对象。</returns>
                public static ItemType[] ArrayByRemove<ItemType>(
                        this ItemType[] items,
                        ItemType? objectItem,
                        Func<ItemType, bool>? toIsObjectItem = null,
                        bool isClearNull = true)
                {
                        if (items.Length < 1)
                        {
                                return items;
                        }

                        var itemList = new List<ItemType>();
                        if (toIsObjectItem != null)
                        {
                                foreach (var item in items)
                                {
                                        if (item != null
                                                || isClearNull == false)
                                        {
                                                if (!toIsObjectItem(item))
                                                {
                                                        itemList.Add(item);
                                                }
                                        }
                                }
                        }
                        else
                        {
                                foreach (var item in items)
                                {
                                        if (item != null
                                                || isClearNull == false)
                                        {
                                                var isItemsEquals = false;
                                                if ((item == null && objectItem == null)
                                                        || (item != null && item.Equals(objectItem)))
                                                {
                                                        isItemsEquals = true;
                                                }
                                                if (!isItemsEquals)
                                                {
                                                        itemList.Add(item);
                                                }
                                        }
                                }
                        }
                        return itemList.ToArray();
                }

                /// <summary>
                /// 通过移除重复元素，创建新的元素数组。
                /// </summary>
                /// <typeparam name="ItemType">数组元素类型。</typeparam>
                /// <param name="items">当前数组。</param>
                /// <param name="toIsItemsEquals">指定的判断对象是否相同的回调函数。</param>
                /// <param name="isClearNull">是否清除“null”元素。</param>
                /// <returns>移除重复元素后新建的数组对象，即使新数组的长度为0，仍会返回有效的数组对象。</returns>
                public static ItemType[] ArrayByRemoveDuplicateItems<ItemType>(
                        this ItemType[] items,
                        Func<ItemType, ItemType, bool>? toIsItemsEquals = null,
                        bool isClearNull = true)
                {
                        if (items.Length < 1)
                        {
                                return items;
                        }

                        var itemList = new List<ItemType>();
                        if (toIsItemsEquals != null)
                        {
                                foreach (var item in items)
                                {
                                        if (item != null
                                                || isClearNull == false)
                                        {
                                                var isValidItem = true;
                                                for (var itemExistedIndex = itemList.Count - 1;
                                                        itemExistedIndex >= 0;
                                                        itemExistedIndex--)
                                                {
                                                        var itemExisted = itemList[itemExistedIndex];
                                                        if (toIsItemsEquals(item, itemExisted))
                                                        {
                                                                isValidItem = false;
                                                        }
                                                }
                                                if (isValidItem)
                                                {
                                                        itemList.Add(item);
                                                }
                                        }
                                }
                        }
                        else
                        {
                                foreach (var item in items)
                                {
                                        if (item != null
                                                || isClearNull == false)
                                        {
                                                var isValidItem = true;
                                                for (var itemExistedIndex = itemList.Count - 1;
                                                        itemExistedIndex >= 0;
                                                        itemExistedIndex--)
                                                {
                                                        var itemExisted = itemList[itemExistedIndex];
                                                        if ((item == null && itemExisted == null)
                                                                || (item != null && item.Equals(itemExisted)))
                                                        {
                                                                isValidItem = false;
                                                        }
                                                }
                                                if (isValidItem)
                                                {
                                                        itemList.Add(item);
                                                }
                                        }
                                }
                        }
                        return itemList.ToArray();
                }


                /// <summary>
                /// 使用二分法查找目标元素在列表中的索引值。
                /// </summary>
                /// <typeparam name="ItemType">列表中的元素类型。</typeparam>
                /// <param name="itemsSorted">要进行查找的列表对象，注意：列表应当已被正确的排序。</param>
                /// <param name="searchRangeBeginIndex">开始查找的对象索引值。</param>
                /// <param name="searchRangeEndIndex">结束查找的对象索引值。</param>
                /// <param name="toComparerToObjectItemWith">当前元素和目标元素的比较结果，当前元素小于模板元素时，返回：-1，等于时，返回：0，大于时返回：1 。</param>
                /// <returns>查找到目标元素后，返回目标元素在列表中的索引值，否则返回：-1 。</returns>
                public static int FindItemIndexWithDichotomyInRange<ItemType>(
                        this ItemType[]? itemsSorted,
                        int searchRangeBeginIndex,
                        int searchRangeLength,
                        Func<ItemType, int> toComparerToObjectItemWith)
                {
                        if (itemsSorted == null
                                || itemsSorted.Length < 1)
                        {
                                return -1;
                        }

                        var items = itemsSorted;
                        var itemsCount = items.Length;
                        if (searchRangeBeginIndex < 0)
                        {
                                searchRangeBeginIndex = 0;
                        }
                        var searchRangeEndIndex = searchRangeBeginIndex + searchRangeLength;
                        if (searchRangeEndIndex < 0
                                || searchRangeEndIndex > itemsCount)
                        {
                                searchRangeEndIndex = itemsCount;
                        }


                        var objectItemIndexMatched = -1;
                        while (searchRangeEndIndex > searchRangeBeginIndex)
                        {
                                searchRangeLength
                                        = searchRangeEndIndex - searchRangeBeginIndex;
                                var searchShotIndex
                                        = searchRangeBeginIndex
                                        + searchRangeLength / 2;

                                var item = items[searchShotIndex];
                                var resultOfComparerItemToObjectItem = toComparerToObjectItemWith(item);
                                if (resultOfComparerItemToObjectItem == 0)
                                {
                                        // !!!
                                        objectItemIndexMatched = searchShotIndex;
                                        // !!!
                                        break;
                                }
                                else if (searchRangeLength == 1)
                                {
                                        break;
                                }
                                else if (resultOfComparerItemToObjectItem < 0)
                                {
                                        searchRangeBeginIndex = searchShotIndex;
                                        // searchRangeEndIndex = searchRangeEndIndex;
                                }
                                else if (resultOfComparerItemToObjectItem > 0)
                                {
                                        // searchRangeBeginIndex = searchRangeBeginIndex;
                                        searchRangeEndIndex = searchShotIndex;
                                }
                        }
                        return objectItemIndexMatched;
                }

                /// <summary>
                /// 使用二分法查找目标元素在列表中的索引值。
                /// </summary>
                /// <typeparam name="ItemType">列表中的元素类型。</typeparam>
                /// <param name="itemsSorted">要进行查找的列表对象，注意：列表应当已被正确的排序。</param>
                /// <param name="toComparerToObjectItemWith">当前元素和目标元素的比较结果，当前元素小于模板元素时，返回：-1，等于时，返回：0，大于时返回：1 。</param>
                /// <returns>查找到目标元素后，返回目标元素在列表中的索引值，否则返回：-1 。</returns>
                public static int FindItemIndexWithDichotomy<ItemType>(
                        this ItemType[]? itemsSorted,
                        Func<ItemType, int> toComparerToObjectItemWith)
                {
                        return ArrayExtension.FindItemIndexWithDichotomyInRange<ItemType>(
                                itemsSorted,
                                -1,
                                -1,
                                toComparerToObjectItemWith);
                }

                /// <summary>
                /// 使用二分法查找目标元素在列表中的索引值。
                /// </summary>
                /// <typeparam name="ItemType">列表中的元素类型。</typeparam>
                /// <param name="itemsSorted">要进行查找的列表对象，注意：列表应当已被正确的排序。</param>
                /// <param name="searchRangeBeginIndex">开始查找的对象索引值。</param>
                /// <param name="searchRangeEndIndex">结束查找的对象索引值。</param>
                /// <param name="toComparerToObjectItemWith">当前元素和目标元素的比较结果，当前元素小于模板元素时，返回：-1，等于时，返回：0，大于时返回：1 。</param>
                /// <returns>查找到目标元素后，返回目标元素，否则返回：default 。</returns>
                public static ItemType? FindItemWithDichotomyInRange<ItemType>(
                        this ItemType[]? itemsSorted,
                        int searchRangeBeginIndex,
                        int searchRangeEndIndex,
                        Func<ItemType, int> toComparerToObjectItemWith)
                {
                        var itemIndex = ArrayExtension.FindItemIndexWithDichotomyInRange(
                                itemsSorted,
                                searchRangeBeginIndex,
                                searchRangeEndIndex,
                                toComparerToObjectItemWith);
                        if (itemsSorted != null
                                && itemIndex >= 0
                               && itemIndex < itemsSorted.Length)
                        {
                                return itemsSorted[itemIndex];
                        }
                        return default;
                }

                /// <summary>
                /// 使用二分法查找目标元素。
                /// </summary>
                /// <typeparam name="ItemType">列表中的元素类型。</typeparam>
                /// <param name="itemsSorted">要进行查找的列表对象，注意：列表应当已被正确的排序。</param>
                /// <param name="toComparerToObjectItemWith">当前元素和目标元素的比较结果，当前元素小于模板元素时，返回：-1，等于时，返回：0，大于时返回：1 。</param>
                /// <returns>查找到目标元素后，返回目标元素，否则返回：default 。</returns>
                public static ItemType? FindItemWithDichotomy<ItemType>(
                        this ItemType[]? itemsSorted,
                        Func<ItemType, int> toComparerToObjectItemWith)
                {
                        return ArrayExtension.FindItemWithDichotomyInRange<ItemType>(
                                itemsSorted,
                                -1,
                                -1,
                                toComparerToObjectItemWith);
                }
        }
}