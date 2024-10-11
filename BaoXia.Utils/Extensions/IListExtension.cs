using System;
using System.Collections.Generic;

namespace BaoXia.Utils.Extensions;

/// <summary>
/// List 扩展类。
/// </summary>
public static class IListExtension
{
	public static bool AddUnique<ItemType>(
		this IList<ItemType> list,
		ItemType item,
		Func<ItemType, ItemType, bool>? toIsSameItems = null,
		bool isAddNullItem = false)
	{
		if (item == null
			&& isAddNullItem == false)
		{
			return false;
		}
		if (toIsSameItems != null)
		{
			foreach (var itemExisted in list)
			{
				if (toIsSameItems(item, itemExisted))
				{
					return false;
				}
			}
		}
		else if (list.Contains(item))
		{
			return false;
		}

		// !!!
		list.Add(item);
		// !!!

		return true;
	}

	public static int AddItemsFrom<ItemType>(
		this IList<ItemType>? list,
		IEnumerable<ItemType>? sourceItems,
		Func<ItemType, bool> toIsItemNeedAdd)
	{
		if (list == null)
		{
			return 0;
		}

		if (sourceItems == null)
		{
			return 0;
		}

		var itemsCountJustAdd = 0;
		foreach (var sourceItem in sourceItems)
		{
			if (toIsItemNeedAdd(sourceItem) == true)
			{
				list.Add(sourceItem);
				itemsCountJustAdd++;
			}
		}
		return itemsCountJustAdd;
	}

	public static void InsertWithOrder<ItemType>(
		this IList<ItemType> list,
		ItemType? newItem,
		Func<ItemType, ItemType, int> toCompareItem)
	{
		if (newItem == null)
		{
			return;
		}

		if (list.Count < 1)
		{
			// !!!
			list.Add(newItem);
			return;
			// !!!
		}

		for (var itemIndex = 0;
			itemIndex < list.Count;
			itemIndex++)
		{
			var item = list[itemIndex];
			var compareResult = toCompareItem(newItem, item);
			if (compareResult < 0)
			{
				// !!!
				list.Insert(itemIndex, newItem);
				return;
				// !!!
			}
			if (itemIndex == (list.Count - 1))
			{
				// !!!
				list.Insert(itemIndex + 1, newItem);
				return;
				// !!!
			}
		}
	}

	public static void InsertWithOrderDescending<ItemType>(
		this IList<ItemType> list,
		ItemType? newItem,
		Func<ItemType, ItemType, int> toCompareItem)
	{
		if (newItem == null)
		{
			return;
		}

		if (list.Count < 1)
		{
			// !!!
			list.Add(newItem);
			return;
			// !!!
		}

		for (var itemIndex = 0;
			itemIndex < list.Count;
			itemIndex++)
		{
			var item = list[itemIndex];
			var compareResult = toCompareItem(newItem, item);
			if (compareResult > 0)
			{
				// !!!
				list.Insert(itemIndex, newItem);
				return;
				// !!!
			}
			if (itemIndex == (list.Count - 1))
			{
				// !!!
				list.Insert(itemIndex + 1, newItem);
				return;
				// !!!
			}
		}
	}

	/// <summary>
	/// 移除指定区间索引值的元素。
	/// </summary>
	/// <typeparam name="ItemType">列表元素类型。</typeparam>
	/// <param name="itemList">当前列表。</param>
	/// <param name="firstItemIndexNeedRemoved">第一个需要被移除的元素索引值。</param>
	/// <param name="lastItemIndexNeedRemoved">最后一个需要被移除的元素索引值。</param>
	public static int RemoveFrom<ItemType>(
		this List<ItemType> itemList,
		int firstItemIndexNeedRemoved,
		int lastItemIndexNeedRemoved = -1)
	{
		if (itemList == null
			|| itemList.Count < 1)
		{
			return 0;
		}

		var lastItemIndex = itemList.Count - 1;
		if (lastItemIndexNeedRemoved < 0)
		{
			lastItemIndexNeedRemoved = lastItemIndex;
		}
		else if (firstItemIndexNeedRemoved > lastItemIndexNeedRemoved)
		{
			(lastItemIndexNeedRemoved, firstItemIndexNeedRemoved)
				= (firstItemIndexNeedRemoved, lastItemIndexNeedRemoved);
		}

		if (firstItemIndexNeedRemoved < 0)
		{
			firstItemIndexNeedRemoved = 0;
		}
		else if (firstItemIndexNeedRemoved > lastItemIndex)
		{
			return 0;
		}

		if (lastItemIndexNeedRemoved > lastItemIndex)
		{
			lastItemIndexNeedRemoved = lastItemIndex;
		}

		var itemsCountNeedRemoved
			= lastItemIndexNeedRemoved
			- firstItemIndexNeedRemoved
			+ 1;
		{
			// !!!
			itemList.RemoveRange(
				firstItemIndexNeedRemoved,
				itemsCountNeedRemoved);
			// !!!
		}
		return itemsCountNeedRemoved;
	}

	/// <summary>
	/// 保留指定的元素，删除其他的元素。
	/// </summary>
	/// <typeparam name="ItemType">列表元素类型。</typeparam>
	/// <param name="list">要删除元素的列表对象。</param>
	/// <param name="toIsItemNotNeedRemoved">判断元素是否需要被保留的回调方法，返回“true”时，对应的元素会被保留，否则被删除。</param>
	public static List<ItemType> NotRemoveIf<ItemType>(
		this List<ItemType> list,
		Func<ItemType, bool> toIsItemNotNeedRemoved)
	{
		for (var itemIndex = list.Count - 1;
			itemIndex >= 0;
			itemIndex--)
		{
			var item = list[itemIndex];
			if (toIsItemNotNeedRemoved(item) == false)
			{
				list.RemoveAt(itemIndex);
			}
		}
		return list;
	}

	/// <summary>
	/// 删除指定的元素。
	/// </summary>
	/// <typeparam name="ItemType">列表元素类型。</typeparam>
	/// <param name="list">要删除元素的列表对象。</param>
	/// <param name="toIsItemNeedRemoved">判断元素是否需要被删除的回调方法，返回“true”时，对应的元素会被删除，否则被保留。</param>
	public static List<ItemType> RemoveIf<ItemType>(
		this List<ItemType> list,
		Func<ItemType, bool> toIsItemNeedRemoved)
	{
		for (var itemIndex = list.Count - 1;
			itemIndex >= 0;
			itemIndex--)
		{
			var item = list[itemIndex];
			if (toIsItemNeedRemoved(item) == true)
			{
				list.RemoveAt(itemIndex);
			}
		}
		return list;
	}

	/// <summary>
	/// 通过移除重复元素，创建新的元素数组。
	/// </summary>
	/// <typeparam name="ItemType">列表元素类型。</typeparam>
	/// <param name="items">当前列表。</param>
	/// <returns>移除重复元素后新建的数组对象，即使新数组的长度为0，仍会返回有效的数组对象。</returns>
	public static List<ItemType> RemoveSameItems<ItemType>(
		this List<ItemType> itemList,
		Func<ItemType, ItemType, bool>? toIsSameItems = null,
		bool isClearNull = true)
	{
		if (itemList.Count < 1)
		{
			return itemList;
		}

		if (toIsSameItems != null)
		{
			for (var itemIndex = 0;
				itemIndex < itemList.Count;
				itemIndex++)
			{
				var item = itemList[itemIndex];
				for (var anotherItemIndex = itemList.Count - 1;
					anotherItemIndex > itemIndex;
					anotherItemIndex--)
				{
					var anotherItem = itemList[anotherItemIndex];
					if (toIsSameItems(anotherItem, item))
					{
						itemList.RemoveAt(anotherItemIndex);
					}
				}
				if (item == null
					&& isClearNull)
				{
					itemList.RemoveAt(itemIndex);
					itemIndex--;
				}
			}
		}
		else
		{
			for (var itemIndex = 0;
				itemIndex < itemList.Count;
				itemIndex++)
			{
				var item = itemList[itemIndex];
				for (var anotherItemIndex = itemList.Count - 1;
					anotherItemIndex > itemIndex;
					anotherItemIndex--)
				{
					var anotherItem = itemList[anotherItemIndex];
					if ((item == null && anotherItem == null)
						|| (item != null && item.Equals(anotherItem)))
					{
						itemList.RemoveAt(anotherItemIndex);
					}
				}
				if (item == null
					&& isClearNull)
				{
					itemList.RemoveAt(itemIndex);
					itemIndex--;
				}
			}
		}
		return itemList;
	}




	/// <summary>
	/// 使用二分法查找目标元素在列表中的索引值。
	/// </summary>
	/// <typeparam name="ItemType">列表中的元素类型。</typeparam>
	/// <param name="itemsSorted">要进行查找的列表对象，注意：列表应当已被正确的排序。</param>
	/// <param name="searchRangeBeginIndex">开始查找的对象索引值。</param>
	/// <param name="searchRangeLength">查找范围的对象数量。</param>
	/// <param name="toComparerToObjectItemWith">当前元素和目标元素的比较结果，当前元素小于模板元素时，返回：-1，等于时，返回：0，大于时返回：1 。</param>
	/// <param name="isGetItemNearestLeft">是否获取最接近目标的左侧对象。</param>
	/// <param name="itemIndexNearest">最接近目标的左侧对象索引值。</param>
	/// <param name="itemNearest">最接近目标的左侧对象。</param>
	/// <returns>查找到目标元素后，返回目标元素在列表中的索引值，否则返回：-1 。</returns>
	public static int FindItemIndexWithDichotomyInRange<ItemType>(
		this IList<ItemType>? itemsSorted,
		bool isItemsSortedWithAscending,
		int searchRangeBeginIndex,
		int searchRangeLength,
		Func<ItemType, int, int> toComparerToObjectItemWith,
		bool isGetItemNearestLeft,
		out int? itemNearestIndex,
		out ItemType? itemNearest)
	{
		itemNearestIndex = null;
		itemNearest = default;

		if (itemsSorted == null
			|| itemsSorted.Count < 1)
		{
			return -1;
		}

		var items = itemsSorted;
		var itemsCount = items.Count;
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
		if (searchRangeEndIndex <= searchRangeBeginIndex)
		{
			return -1;
		}

		var compareNumberDirection
			= isItemsSortedWithAscending
			? 1
			: -1;

		var objectItemIndexMatched = -1;
		while (searchRangeEndIndex > searchRangeBeginIndex)
		{
			searchRangeLength
				= searchRangeEndIndex - searchRangeBeginIndex;
			var searchShotIndex
				= searchRangeBeginIndex
				+ searchRangeLength / 2;

			var item = items[searchShotIndex];
			var resultOfComparerItemToObjectItem
				= toComparerToObjectItemWith(item, searchShotIndex)
				* compareNumberDirection;
			if (resultOfComparerItemToObjectItem == 0)
			{
				// !!!
				objectItemIndexMatched = searchShotIndex;
				// !!!
				break;
			}
			else
			{
				// !!! 不是目标时，记录最接近目标的元素信息。 !!!
				itemNearestIndex = searchShotIndex;
				itemNearest = item;
				// !!!
				if (searchRangeLength == 1)
				{
					break;
				}
				else
				{
					if (resultOfComparerItemToObjectItem < 0)
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
			}
		}


		if (isGetItemNearestLeft)
		{
			if (objectItemIndexMatched >= 0)
			{
				itemNearestIndex = objectItemIndexMatched - 1;
				if (itemNearestIndex >= 0)
				{
					itemNearest = items[itemNearestIndex.Value];
				}
				else
				{
					itemNearest = default;
				}
			}
			else if (
				itemNearest != null
				&& itemNearestIndex != null
				&& (toComparerToObjectItemWith(itemNearest, itemNearestIndex.Value)
				* compareNumberDirection) > 0)
			{
				itemNearestIndex--;
				if (itemNearestIndex >= 0)
				{
					itemNearest = items[itemNearestIndex.Value];
				}
				else
				{
					itemNearest = default;
				}
			}
		}
		else
		{
			if (objectItemIndexMatched >= 0)
			{
				itemNearestIndex = objectItemIndexMatched + 1;
				if (itemNearestIndex < itemsCount)
				{
					itemNearest = items[itemNearestIndex.Value];
				}
				else
				{
					itemNearest = default;
				}
			}
			else if (
				itemNearest != null
				&& itemNearestIndex != null
				&& (toComparerToObjectItemWith(itemNearest, itemNearestIndex.Value)
				* compareNumberDirection) < 0)
			{
				itemNearestIndex++;
				if (itemNearestIndex < itemsCount)
				{
					itemNearest = items[itemNearestIndex.Value];
				}
				else
				{
					itemNearest = default;
				}
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
	/// <param name="isGetItemNearestLeft">是否获取最接近目标的左侧对象。</param>
	/// <param name="itemIndexNearest">最接近目标的左侧对象索引值。</param>
	/// <param name="itemNearest">最接近目标的左侧对象。</param>
	/// <returns>查找到目标元素后，返回目标元素在列表中的索引值，否则返回：-1 。</returns>
	public static int FindItemIndexWithDichotomy<ItemType>(
		this IList<ItemType>? itemsSorted,
		bool isItemsSortedWithAscending,
		Func<ItemType, int, int> toComparerToObjectItemWith,
		bool isGetItemNearestLeft,
		out int? itemIndexNearest,
		out ItemType? itemNearest)
	{
		return IListExtension.FindItemIndexWithDichotomyInRange<ItemType>(
			itemsSorted,
			isItemsSortedWithAscending,
			- 1,
			-1,
			toComparerToObjectItemWith,
			isGetItemNearestLeft,
			out itemIndexNearest,
			out itemNearest);
	}

	/// <summary>
	/// 使用二分法查找目标元素在列表中的索引值。
	/// </summary>
	/// <typeparam name="ItemType">列表中的元素类型。</typeparam>
	/// <param name="itemsSorted">要进行查找的列表对象，注意：列表应当已被正确的排序。</param>
	/// <param name="searchRangeBeginIndex">开始查找的对象索引值。</param>
	/// <param name="searchRangeEndIndex">结束查找的对象索引值。</param>
	/// <param name="toComparerToObjectItemWith">当前元素和目标元素的比较结果，当前元素小于模板元素时，返回：-1，等于时，返回：0，大于时返回：1 。</param>
	/// <param name="isGetItemNearestLeft">是否获取最接近目标的左侧对象。</param>
	/// <param name="itemIndexNearest">最接近目标的左侧对象索引值。</param>
	/// <param name="itemNearest">最接近目标的左侧对象。</param>
	/// <returns>查找到目标元素后，返回目标元素，否则返回：default 。</returns>
	public static ItemType? FindItemWithDichotomyInRange<ItemType>(
		this IList<ItemType>? itemsSorted,
		bool isItemsSortedWithAscending,
		int searchRangeBeginIndex,
		int searchRangeEndIndex,
		Func<ItemType, int, int> toComparerToObjectItemWith,
		bool isGetItemNearestLeft,
		out int? itemIndexNearest,
		out ItemType? itemNearest)
	{
		var itemIndex = IListExtension.FindItemIndexWithDichotomyInRange(
			itemsSorted,
			isItemsSortedWithAscending,
			searchRangeBeginIndex,
			searchRangeEndIndex,
			toComparerToObjectItemWith,
			isGetItemNearestLeft,
			out itemIndexNearest,
			out itemNearest);
		if (itemsSorted != null
			&& itemIndex >= 0
		       && itemIndex < itemsSorted.Count)
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
	/// <param name="isGetItemNearestLeft">是否获取最接近目标的左侧对象。</param>
	/// <param name="itemIndexNearest">最接近目标的左侧对象索引值。</param>
	/// <param name="itemNearest">最接近目标的左侧对象。</param>
	/// <returns>查找到目标元素后，返回目标元素，否则返回：default 。</returns>
	public static ItemType? FindItemWithDichotomy<ItemType>(
		this IList<ItemType>? itemsSorted,
		bool isItemsSortedWithAscending,
		Func<ItemType, int, int> toComparerToObjectItemWith,
		bool isGetItemNearestLeft,
		out int? itemIndexNearest,
		out ItemType? itemNearest)
	{
		return IListExtension.FindItemWithDichotomyInRange<ItemType>(
			itemsSorted,
			isItemsSortedWithAscending,
			-1,
			-1,
			toComparerToObjectItemWith,
			isGetItemNearestLeft,
			out itemIndexNearest,
			out itemNearest);
	}


	public static int FindItemIndexWithDichotomyInRange<ItemType>(
		this IList<ItemType>? itemsSorted,
		bool isItemsSortedWithAscending,
		int searchRangeBeginIndex,
		int searchRangeLength,
		Func<ItemType, int, int> toComparerToObjectItemWith)
	{
		return FindItemIndexWithDichotomyInRange(
			itemsSorted,
			isItemsSortedWithAscending,
			searchRangeBeginIndex,
			searchRangeLength,
			toComparerToObjectItemWith,
			//
			true,
			out _,
			out _);
	}

	public static int FindItemIndexWithDichotomy<ItemType>(
		this IList<ItemType>? itemsSorted,
		bool isItemsSortedWithAscending,
		Func<ItemType, int, int> toComparerToObjectItemWith)
	{
		return FindItemIndexWithDichotomy(
			itemsSorted,
			isItemsSortedWithAscending,
			toComparerToObjectItemWith,
			//
			true,
			out _,
			out _);
	}

	public static ItemType? FindItemWithDichotomyInRange<ItemType>(
		this IList<ItemType>? itemsSorted,
		bool isItemsSortedWithAscending,
		int searchRangeBeginIndex,
		int searchRangeEndIndex,
		Func<ItemType, int, int> toComparerToObjectItemWith)
	{
		return FindItemWithDichotomyInRange(
			itemsSorted,
			isItemsSortedWithAscending,
			searchRangeBeginIndex,
			searchRangeEndIndex,
			toComparerToObjectItemWith,
			//
			true,
			out _,
			out _);
	}

	public static ItemType? FindItemWithDichotomy<ItemType>(
		this IList<ItemType>? itemsSorted,
		bool isItemsSortedWithAscending,
		Func<ItemType, int, int> toComparerToObjectItemWith)
	{
		return FindItemWithDichotomy(
			itemsSorted,
			isItemsSortedWithAscending,
			toComparerToObjectItemWith,
			//
			true,
			out _,
			out _);
	}

	public static ItemType? FirstItemOrDefault<ItemType>(this IList<ItemType> items)
	{
		if (items.Count > 0)
		{
			return items[0];
		}
		return default;
	}
	public static ItemType? LastItemOrDefault<ItemType>(this IList<ItemType> items)
	{
		if (items.Count > 0)
		{
			return items[^1];
		}
		return default;
	}

	public static List<ItemType> GetPageItems<ItemType>(
	    this IList<ItemType> items,
	    //
	    Func<List<ItemType>, List<ItemType>>? toSortItems,
	    //
	    int pageIndex,
	    int pageSize)
	{
		int itemsCount = items.Count;
		if (itemsCount < 1)
		{
			return [];
		}

		var itemPageBeginItemIndex = pageIndex * pageSize;
		if (itemPageBeginItemIndex < 0)
		{
			itemPageBeginItemIndex = 0;
		}
		var itemPageEndItemIndex = itemPageBeginItemIndex + pageSize;
		if (itemPageEndItemIndex > itemsCount)
		{
			itemPageEndItemIndex = itemsCount;
		}
		if (itemPageBeginItemIndex >= itemsCount
		    || itemPageEndItemIndex <= itemPageBeginItemIndex)
		{
			return [];
		}

		////////////////////////////////////////////////
		// 1/，不需要搜索匹配和排序时，直接按有效的分页索引返回实体。
		////////////////////////////////////////////////

		List<ItemType> pageItems;
		if (toSortItems == null)
		{
			pageItems = [];
			var itemIndex = 0;
			foreach (var item in items)
			{
				if (itemIndex < itemPageBeginItemIndex)
				{
					itemIndex++;
					continue;
				}
				if (itemIndex >= itemPageEndItemIndex)
				{
					break;
				}
				// !!!
				pageItems.Add(item);
				itemIndex++;
				// !!!
			}
			return pageItems;
		}

		////////////////////////////////////////////////
		// 2/，根据搜索匹配信息排序元素。
		////////////////////////////////////////////////
		var itemList = new List<ItemType>(items);
		if (toSortItems != null)
		{
			itemList = toSortItems(itemList);
		}

		////////////////////////////////////////////////
		// 3/，根据搜索、排序后的结果进行分页。
		////////////////////////////////////////////////
		pageItems = [];
		for (var itemIndex = itemPageBeginItemIndex;
		    itemIndex < itemPageEndItemIndex;
		    itemIndex++)
		{
			var item = itemList[itemIndex];
			// !!!
			pageItems.Add(item);
			// !!!
		}
		return pageItems;
	}
}
