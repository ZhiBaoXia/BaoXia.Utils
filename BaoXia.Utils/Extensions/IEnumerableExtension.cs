using System;
using System.Collections.Generic;
using System.Linq;

namespace BaoXia.Utils.Extensions
{
        public static class IEnumerableExtension
        {
                public static bool IsContains<DataType>(
                        this IEnumerable<DataType> items,
                        DataType? objectItem,
                        out int objectItemIndexInItems)
                        where DataType : notnull
                {
                        objectItemIndexInItems = -1;

                        if (objectItem != null)
                        {
                                var keyIndex = 0;
                                foreach (var item in items)
                                {
                                        if (item.Equals(objectItem))
                                        {
                                                // !!!
                                                objectItemIndexInItems = keyIndex;
                                                // !!!
                                                return true;
                                        }
                                        keyIndex++;
                                }
                        }
                        return false;
                }

                public static bool IsNotContains<DataType>(
                        this IEnumerable<DataType> items,
                        DataType? objectItem,
                        out int objectItemIndexInItems)
                        where DataType : notnull
                {
                        return !IEnumerableExtension.IsContains(
                                items,
                                objectItem,
                                out objectItemIndexInItems);
                }

                public static bool IsContains<DataType>(
                        this IEnumerable<DataType> items,
                        DataType? objectItem)
                        where DataType : notnull
                {
                        return IEnumerableExtension.IsContains(
                                items,
                                objectItem,
                                out _);
                }

                public static bool IsNotContains<DataType>(
                        this IEnumerable<DataType> items,
                        DataType? objectItem)
                        where DataType : notnull
                {
                        return !IEnumerableExtension.IsContains(
                                items,
                                objectItem);
                }

                public static bool IsContains<DataType>(
                        this IEnumerable<DataType> items,
                        IEnumerable<DataType>? objectItems)
                        where DataType : notnull
                {
                        if (objectItems != null)
                        {
                                var isObjectItemExisted = false;
                                foreach (var objectItem in objectItems)
                                {
                                        // !!!
                                        isObjectItemExisted = true;
                                        // !!!
                                        if (items.IsContains(objectItem) != true)
                                        {
                                                return false;
                                        }
                                }
                                if (isObjectItemExisted)
                                {
                                        return true;
                                }
                        }
                        return false;
                }

                public static bool IsNotContains<DataType>(
                        this IEnumerable<DataType> items,
                        IEnumerable<DataType>? objectItems)
                        where DataType : notnull
                {
                        return !IEnumerableExtension.IsContains(
                                items,
                                objectItems);
                }

                public static bool IsContainsAny<DataType>(
                        this IEnumerable<DataType> items,
                        IEnumerable<DataType>? objectItems,
                        bool isNullEqualsEmpty = true)
                        where DataType : notnull
                {
                        if (objectItems != null)
                        {
                                foreach (var objectItem in objectItems)
                                {
                                        if (items.IsContains(objectItem) == true)
                                        {
                                                return true;
                                        }
                                }
                        }
                        else if (isNullEqualsEmpty
                                && items.Any() == false)
                        {
                                return true;
                        }
                        return false;
                }

                public static bool IsNotContainsAny<DataType>(
                        this IEnumerable<DataType> items,
                        IEnumerable<DataType>? objectItems,
                        bool isNullEqualsEmpty = true)
                        where DataType : notnull
                {
                        return !IEnumerableExtension.IsContainsAny(
                                items,
                                objectItems,
                                isNullEqualsEmpty);
                }


                public static bool IsEquals<DataType>(
                        this IEnumerable<DataType> items,
                        IEnumerable<DataType>? objectItems,
                        bool isIgnoreSameItems = false,
                        bool isNullEqualsEmpty = true)
                        where DataType : notnull
                {
                        if (objectItems != null)
                        {
                                if (isIgnoreSameItems)
                                {
                                        List<int>? itemIndexesMatched = null;
                                        var objectItemsCount = 0;
                                        foreach (var objectItem in objectItems)
                                        {
                                                if (items.IsContains(
                                                        objectItem,
                                                        out var itemIndexMatched) != true)
                                                {
                                                        return false;
                                                }
                                                // !!!
                                                objectItemsCount++;
                                                // !!!
                                                {
                                                        itemIndexesMatched ??= new List<int>();
                                                }
                                                itemIndexesMatched.Add(itemIndexMatched);
                                        }
                                        foreach (var item in items)
                                        {
                                                objectItemsCount--;
                                        }
                                        if (objectItemsCount < 0)
                                        {
                                                if (itemIndexesMatched != null)
                                                {
                                                        var itemIndex = 0;
                                                        foreach (var item in items)
                                                        {
                                                                if (itemIndexesMatched.Contains(itemIndex) != true)
                                                                {
                                                                        if (objectItems.IsContains(item) != true)
                                                                        {
                                                                                return false;
                                                                        }
                                                                }
                                                                // !!!
                                                                itemIndex++;
                                                                // !!!
                                                        }
                                                        return true;
                                                }
                                                return false;
                                        }
                                }
                                else
                                {
                                        var objectItemsCount = 0;
                                        foreach (var objectItem in objectItems)
                                        {
                                                if (items.IsContains(objectItem) != true)
                                                {
                                                        return false;
                                                }
                                                // !!!
                                                objectItemsCount++;
                                                // !!!
                                        }
                                        foreach (var item in items)
                                        {
                                                objectItemsCount--;
                                        }
                                        if (objectItemsCount != 0)
                                        {
                                                return false;
                                        }
                                }
                                return true;
                        }
                        else if (isNullEqualsEmpty == true)
                        {
                                var itemingsEnumerator = items.GetEnumerator();
                                if (itemingsEnumerator != null)
                                {
                                        itemingsEnumerator.Reset();
                                        if (itemingsEnumerator.MoveNext() == false)
                                        {
                                                return true;
                                        }
                                }
                        }
                        return false;
                }

                public static bool IsNotEquals<DataType>(
                        this IEnumerable<DataType> items,
                        IEnumerable<DataType>? objectItems,
                        bool isIgnoreSameItems = false,
                        bool isNullEqualsEmpty = true)
                        where DataType : notnull
                {
                        return !IEnumerableExtension.IsEquals(
                                items,
                                objectItems,
                                isIgnoreSameItems,
                                isNullEqualsEmpty);
                }

                public static List<ItemType>? ToListBy<ItemType>(
                        this IEnumerable<ItemType>? items,
                        Func<ItemType, bool> toIsItemValidToList)
                {
                        if (items == null)
                        {
                                return null;
                        }

                        var newList = new List<ItemType>();
                        foreach (var item in items)
                        {
                                if (toIsItemValidToList(item))
                                {
                                        newList.Add(item);
                                }
                        }
                        return newList;
                }

                public static List<ItemType>[]? ToGroupsBy<ItemType, ItemGroupKeyType>(
                        this IEnumerable<ItemType>? items,
                        Func<ItemType, ItemGroupKeyType> toGetItemGroupKey)
                        where ItemGroupKeyType : notnull
                {
                        if (items == null)
                        {
                                return null;
                        }

                        var itemGroups = new Dictionary<ItemGroupKeyType, List<ItemType>>();
                        foreach (var item in items)
                        {
                                var itemGroupKey = toGetItemGroupKey(item);
                                if (!itemGroups.TryGetValue(
                                        itemGroupKey,
                                        out var itemGroup))
                                {
                                        itemGroup = new List<ItemType>();
                                        itemGroups.AddOrSet(itemGroupKey, itemGroup);
                                }
                                itemGroup.Add(item);
                        }
                        return itemGroups.Values.ToArray();
                }
        }
}
