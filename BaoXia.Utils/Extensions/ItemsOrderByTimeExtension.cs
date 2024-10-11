using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoXia.Utils.Extensions;

public static class ItemsOrderByTimeExtension
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	private static ItemType[]? GetItemsSortByTimeInTimeSection<ItemType>(
		ItemType[] items,
		bool isItemsSortedWithAscending,
		DateTimeOffset beginTime,
		DateTimeOffset endTime,
		Func<ItemType, DateTimeOffset, int> toCompareTimeWithItem,
		bool isGetObjectItemsCountOnly,
		out int objectItemsCount)
	{
		//
		objectItemsCount = 0;
		//
		if (items.Length <= 0)
		{
			return null;
		}
		// 【注意】beginTime和endTime含义不同，不可互换。
		if (beginTime >= endTime)
		{
			return null;
		}

		var itemsCount = items.Length;

		var firstItemIndex = items.FindItemIndexWithDichotomy(
			isItemsSortedWithAscending,
			(item, itemIndex) =>
			{
				return toCompareTimeWithItem(item, beginTime);
			},
			// 无论升序、降序，
			// 都要找到大于等于“beginTime”的第一个元素。
			!isItemsSortedWithAscending,
			out var firstItemIndex_NearestAndGreatThan,
			out _);
		if (firstItemIndex_NearestAndGreatThan == null)
		{
			return null;
		}
		if (firstItemIndex < 0)
		{
			if (firstItemIndex_NearestAndGreatThan >= 0
				&& firstItemIndex_NearestAndGreatThan < itemsCount)
			{
				firstItemIndex = firstItemIndex_NearestAndGreatThan.Value;
			}
			else if (isItemsSortedWithAscending)
			{
				if (firstItemIndex_NearestAndGreatThan < 0)
				{
					// 此时所有“items”都大于“beginTime”。
					firstItemIndex = 0;
				}
				else if (firstItemIndex_NearestAndGreatThan >= itemsCount)
				{
					// 此时所有“items”都小于“beginTime”。
					return null;
				}
			}
			else
			{
				if (firstItemIndex_NearestAndGreatThan < 0)
				{
					// 此时所有“items”都小于“beginTime”。
					return null;
				}
				else if (firstItemIndex_NearestAndGreatThan >= itemsCount)
				{
					// 此时所有“items”都小于“beginTime”。
					firstItemIndex = itemsCount - 1;
				}
			}
		}


		var endItemIndex = items.FindItemIndexWithDichotomy(
			isItemsSortedWithAscending,
			(item, itemIndex) =>
			{
				return toCompareTimeWithItem(item, endTime);
			},
			//
			// 无论升序、降序，
			// 都要找到大于等于“endTime”的第一个元素。
			!isItemsSortedWithAscending,
			out var endItemIndex_NearestAndGreatThan,
			out _);
		if (endItemIndex_NearestAndGreatThan == null)
		{
			return null;
		}
		if (endItemIndex < 0)
		{
			if (endItemIndex_NearestAndGreatThan >= 0
				&& endItemIndex_NearestAndGreatThan < itemsCount)
			{
				endItemIndex = endItemIndex_NearestAndGreatThan.Value;
			}
			else if (isItemsSortedWithAscending)
			{
				if (endItemIndex_NearestAndGreatThan < 0)
				{
					// 此时所有“items”都大于“endTime”。
					return null;
				}
				else if (endItemIndex_NearestAndGreatThan >= itemsCount)
				{
					// 此时所有“items”都小于“endTime”。
					endItemIndex = itemsCount;
				}
			}
			else
			{
				if (endItemIndex_NearestAndGreatThan < 0)
				{
					// 此时所有“items”都小于“endTime”。
					endItemIndex = -1;
				}
				else if (endItemIndex_NearestAndGreatThan >= itemsCount)
				{
					// 此时所有“items”大于“endTime”。
					return null;
				}
			}
		}


		if (isItemsSortedWithAscending != true)
		{
			(firstItemIndex, endItemIndex) = (endItemIndex + 1, firstItemIndex + 1);
		}
		objectItemsCount = endItemIndex - firstItemIndex;
		if (objectItemsCount <= 0
			|| isGetObjectItemsCountOnly)
		{
			return null;
		}

		var objectItems = new ItemType[objectItemsCount];
		{
			Array.Copy(
				items,
				firstItemIndex,
				objectItems,
				0,
				objectItemsCount);
		}
		return objectItems;
	}

	public static ItemType[]? GetItemsSortByTimeInTimeSection<ItemType>(
		this ItemType[] items,
		bool isItemsSortedWithAscending,
		DateTimeOffset beginTime,
		DateTimeOffset endTime,
		Func<ItemType, DateTimeOffset, int> toCompareTimeWithItem)
	{
		return GetItemsSortByTimeInTimeSection(
			items,
			isItemsSortedWithAscending,
			beginTime,
			endTime,
			toCompareTimeWithItem,
			false,
			out _);
	}

	public static int GetCountOfItemsSortByTimeInTimeSection<ItemType>(
		this ItemType[] items,
		bool isItemsSortedWithAscending,
		DateTimeOffset beginTime,
		DateTimeOffset endTime,
		Func<ItemType, DateTimeOffset, int> toCompareTimeWithItem)
	{
		GetItemsSortByTimeInTimeSection<ItemType>(
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

	private static ItemType[]? GetItemsSortByTimeInTimeSection<ItemType>(
		IList<ItemType> items,
		bool isItemsSortedWithAscending,
		DateTimeOffset beginTime,
		DateTimeOffset endTime,
		Func<ItemType, DateTimeOffset, int> toCompareTimeWithItem,
		bool isGetObjectItemsCountOnly,
		out int objectItemsCount)
	{
		//
		objectItemsCount = 0;
		//
		var itemsCount = items.Count;
		if (itemsCount <= 0)
		{
			return null;
		}
		// 【注意】beginTime和endTime含义不同，不可互换。
		if (beginTime >= endTime)
		{
			return null;
		}


		var firstItemIndex = items.FindItemIndexWithDichotomy(
			isItemsSortedWithAscending,
			(item, itemIndex) =>
			{
				return toCompareTimeWithItem(item, beginTime);
			},
			//
			!isItemsSortedWithAscending,
			out var firstItemIndex_NearestAndGreatThan,
			out _);
		if (firstItemIndex_NearestAndGreatThan == null)
		{
			return null;
		}
		if (firstItemIndex < 0)
		{
			if (firstItemIndex_NearestAndGreatThan >= 0
				&& firstItemIndex_NearestAndGreatThan < itemsCount)
			{
				firstItemIndex = firstItemIndex_NearestAndGreatThan.Value;
			}
			else if (firstItemIndex_NearestAndGreatThan < 0)
			{
				// 此时所有“items”都大于“beginTime”。
				firstItemIndex = 0;
			}
			else if (firstItemIndex_NearestAndGreatThan >= itemsCount)
			{
				// 此时所有“items”都小于“beginTime”。
				return null;
			}
		}


		var endItemIndex = items.FindItemIndexWithDichotomy(
			isItemsSortedWithAscending,
			(item, itemIndex) =>
			{
				return toCompareTimeWithItem(item, endTime);
			},
			//
			!isItemsSortedWithAscending,
			out var endItemIndex_NearestAndGreatThan,
			out _);
		if (endItemIndex_NearestAndGreatThan == null)
		{
			return null;
		}
		if (endItemIndex < 0)
		{
			if (endItemIndex_NearestAndGreatThan >= 0
				&& endItemIndex_NearestAndGreatThan < itemsCount)
			{
				endItemIndex = endItemIndex_NearestAndGreatThan.Value;
			}
			else if (endItemIndex_NearestAndGreatThan < 0)
			{
				// 此时所有“items”都小于“endTime”。
				return null;
			}
			else if (endItemIndex_NearestAndGreatThan >= itemsCount)
			{
				// 此时所有“items”都大于“endTime”。
				return null;
			}
		}


		if (isItemsSortedWithAscending != true)
		{
			(firstItemIndex, endItemIndex) = (endItemIndex + 1, firstItemIndex + 1);
		}
		objectItemsCount = endItemIndex - firstItemIndex;
		if (objectItemsCount <= 0
			|| isGetObjectItemsCountOnly)
		{
			return null;
		}

		var objectItems = new ItemType[objectItemsCount];
		{
			items.CopyTo(
				objectItems,
				firstItemIndex);
		}
		return objectItems;
	}

	public static ItemType[]? GetItemsSortByTimeInTimeSection<ItemType>(
		this IList<ItemType> items,
		bool isItemsSortedWithAscending,
		DateTimeOffset beginTime,
		DateTimeOffset endTime,
		Func<ItemType, DateTimeOffset, int> toCompareTimeWithItem)
	{
		return GetItemsSortByTimeInTimeSection<ItemType>(
			items,
			isItemsSortedWithAscending,
			beginTime,
			endTime,
			toCompareTimeWithItem,
			false,
			out _);
	}

	public static int GetCountOfItemsSortByTimeInTimeSection<ItemType>(
		this IList<ItemType> items,
		bool isItemsSortedWithAscending,
		DateTimeOffset beginTime,
		DateTimeOffset endTime,
		Func<ItemType, DateTimeOffset, int> toCompareTimeWithItem)
	{
		GetItemsSortByTimeInTimeSection<ItemType>(
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