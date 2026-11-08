using System;
using System.Collections.Generic;

namespace BaoXia.Utils.Extensions;

public static class ItemsOrderByTimeExtension
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	private static ItemType[]? GetItemsSortByTimeInTimeRange<ItemType>(
	    ItemType[] items,
	    bool isItemsSortedWithAscending,
	    DateTimeOffset beginTime,
	    DateTimeOffset endTime,
	    Func<ItemType, DateTimeOffset, int> toCompareTimeWithItem,
	    bool isGetObjectItemsCountOnly,
	    out int objectItemsCount)
	{
		return GetItemsSortByTimeInTimeRange((IList<ItemType>)items, isItemsSortedWithAscending,
			beginTime, endTime, toCompareTimeWithItem, isGetObjectItemsCountOnly, out objectItemsCount);
	}

	public static ItemType[]? GetItemsSortByTimeInTimeRange<ItemType>(
	    this ItemType[] items,
	    bool isItemsSortedWithAscending,
	    DateTimeOffset beginTime,
	    DateTimeOffset endTime,
	    Func<ItemType, DateTimeOffset, int> toCompareTimeWithItem)
	{
		return GetItemsSortByTimeInTimeRange(
		    items,
		    isItemsSortedWithAscending,
		    beginTime,
		    endTime,
		    toCompareTimeWithItem,
		    false,
		    out _);
	}

	public static int GetCountOfItemsSortByTimeInTimeRange<ItemType>(
	    this ItemType[] items,
	    bool isItemsSortedWithAscending,
	    DateTimeOffset beginTime,
	    DateTimeOffset endTime,
	    Func<ItemType, DateTimeOffset, int> toCompareTimeWithItem)
	{
		GetItemsSortByTimeInTimeRange<ItemType>(
		    items,
		    isItemsSortedWithAscending,
		    beginTime,
		    endTime,
		    toCompareTimeWithItem,
		    true,
		    out int objectItemsCount);
		{ }
		return objectItemsCount;
	}

	private static ItemType[]? GetItemsSortByTimeInTimeRange<ItemType>(
	    IList<ItemType> items,
	    bool isItemsSortedWithAscending,
	    DateTimeOffset beginTime,
	    DateTimeOffset endTime,
	    Func<ItemType, DateTimeOffset, int> toCompareTimeWithItem,
	    bool isGetObjectItemsCountOnly,
	    out int objectItemsCount)
	{
		objectItemsCount = 0;
		if (items.Count == 0 || beginTime >= endTime)
		{
			return null;
		}

		// 区间为 [beginTime, endTime)，相同时间的元素必须一起包含或排除。
		var firstTargetTime = isItemsSortedWithAscending ? beginTime : endTime;
		var lastTargetTime = isItemsSortedWithAscending ? endTime : beginTime;
		var lowerIndex = 0;
		var upperIndex = items.Count;
		while (lowerIndex < upperIndex)
		{
			var middleIndex = lowerIndex + (upperIndex - lowerIndex) / 2;
			var comparison = toCompareTimeWithItem(items[middleIndex], firstTargetTime);
			if (isItemsSortedWithAscending ? comparison < 0 : comparison >= 0)
			{
				lowerIndex = middleIndex + 1;
			}
			else
			{
				upperIndex = middleIndex;
			}
		}
		var firstItemIndex = lowerIndex;
		upperIndex = items.Count;
		while (lowerIndex < upperIndex)
		{
			var middleIndex = lowerIndex + (upperIndex - lowerIndex) / 2;
			var comparison = toCompareTimeWithItem(items[middleIndex], lastTargetTime);
			if (isItemsSortedWithAscending ? comparison < 0 : comparison >= 0)
			{
				lowerIndex = middleIndex + 1;
			}
			else
			{
				upperIndex = middleIndex;
			}
		}
		objectItemsCount = lowerIndex - firstItemIndex;
		if (objectItemsCount == 0 || isGetObjectItemsCountOnly)
		{
			return null;
		}
		var objectItems = new ItemType[objectItemsCount];
		for (var index = 0; index < objectItemsCount; index++)
		{
			objectItems[index] = items[firstItemIndex + index];
		}
		return objectItems;
	}

	public static ItemType[]? GetItemsSortByTimeInTimeRange<ItemType>(
	    this IList<ItemType> items,
	    bool isItemsSortedWithAscending,
	    DateTimeOffset beginTime,
	    DateTimeOffset endTime,
	    Func<ItemType, DateTimeOffset, int> toCompareTimeWithItem)
	{
		return GetItemsSortByTimeInTimeRange<ItemType>(
		    items,
		    isItemsSortedWithAscending,
		    beginTime,
		    endTime,
		    toCompareTimeWithItem,
		    false,
		    out _);
	}

	public static int GetCountOfItemsSortByTimeInTimeRange<ItemType>(
	    this IList<ItemType> items,
	    bool isItemsSortedWithAscending,
	    DateTimeOffset beginTime,
	    DateTimeOffset endTime,
	    Func<ItemType, DateTimeOffset, int> toCompareTimeWithItem)
	{
		GetItemsSortByTimeInTimeRange<ItemType>(
		    items,
		    isItemsSortedWithAscending,
		    beginTime,
		    endTime,
		    toCompareTimeWithItem,
		    false,
		    out var objectItemsCount);
		{ }
		return objectItemsCount;
	}

	#endregion
}