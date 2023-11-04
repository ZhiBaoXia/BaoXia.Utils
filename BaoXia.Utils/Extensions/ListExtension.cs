﻿using System;
using System.Collections.Generic;

namespace BaoXia.Utils.Extensions
{
	/// <summary>
	/// List 扩展类。
	/// </summary>
	public static class ListExtension
	{
		public static bool AddUnique<ItemType>(
			this List<ItemType> list,
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
			this List<ItemType>? list,
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
		/// <param name="itemListSorted">要进行查找的列表对象，注意：列表应当已被正确的排序。</param>
		/// <param name="searchRangeBeginIndex">开始查找的对象索引值。</param>
		/// <param name="searchRangeEndIndex">结束查找的对象索引值。</param>
		/// <param name="toComparerToObjectItemWith">当前元素和目标元素的比较结果，当前元素小于模板元素时，返回：-1，等于时，返回：0，大于时返回：1 。</param>
		/// <returns>查找到目标元素后，返回目标元素在列表中的索引值，否则返回：-1 。</returns>
		public static int FindItemIndexWithDichotomyInRange<ItemType>(
			this List<ItemType>? itemListSorted,
			int searchRangeBeginIndex,
			int searchRangeLength,
			Func<ItemType, int> toComparerToObjectItemWith)
		{
			if (itemListSorted == null
				|| itemListSorted.Count < 1)
			{
				return -1;
			}

			var items = itemListSorted;
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
		/// <param name="itemListSorted">要进行查找的列表对象，注意：列表应当已被正确的排序。</param>
		/// <param name="toComparerToObjectItemWith">当前元素和目标元素的比较结果，当前元素小于模板元素时，返回：-1，等于时，返回：0，大于时返回：1 。</param>
		/// <returns>查找到目标元素后，返回目标元素在列表中的索引值，否则返回：-1 。</returns>
		public static int FindItemIndexWithDichotomy<ItemType>(
			this List<ItemType>? itemListSorted,
			Func<ItemType, int> toComparerToObjectItemWith)
		{
			return ListExtension.FindItemIndexWithDichotomyInRange<ItemType>(
				itemListSorted,
				-1,
				-1,
				toComparerToObjectItemWith);
		}

		/// <summary>
		/// 使用二分法查找目标元素在列表中的索引值。
		/// </summary>
		/// <typeparam name="ItemType">列表中的元素类型。</typeparam>
		/// <param name="itemListSorted">要进行查找的列表对象，注意：列表应当已被正确的排序。</param>
		/// <param name="searchRangeBeginIndex">开始查找的对象索引值。</param>
		/// <param name="searchRangeEndIndex">结束查找的对象索引值。</param>
		/// <param name="toComparerToObjectItemWith">当前元素和目标元素的比较结果，当前元素小于模板元素时，返回：-1，等于时，返回：0，大于时返回：1 。</param>
		/// <returns>查找到目标元素后，返回目标元素，否则返回：default 。</returns>
		public static ItemType? FindItemWithDichotomyInRange<ItemType>(
			this List<ItemType>? itemListSorted,
			int searchRangeBeginIndex,
			int searchRangeEndIndex,
			Func<ItemType, int> toComparerToObjectItemWith)
		{
			var itemIndex = ListExtension.FindItemIndexWithDichotomyInRange(
				itemListSorted,
				searchRangeBeginIndex,
				searchRangeEndIndex,
				toComparerToObjectItemWith);
			if (itemListSorted != null
				&& itemIndex >= 0
			       && itemIndex < itemListSorted.Count)
			{
				return itemListSorted[itemIndex];
			}
			return default;
		}

		/// <summary>
		/// 使用二分法查找目标元素。
		/// </summary>
		/// <typeparam name="ItemType">列表中的元素类型。</typeparam>
		/// <param name="itemListSorted">要进行查找的列表对象，注意：列表应当已被正确的排序。</param>
		/// <param name="toComparerToObjectItemWith">当前元素和目标元素的比较结果，当前元素小于模板元素时，返回：-1，等于时，返回：0，大于时返回：1 。</param>
		/// <returns>查找到目标元素后，返回目标元素，否则返回：default 。</returns>
		public static ItemType? FindItemWithDichotomy<ItemType>(
			this List<ItemType>? itemListSorted,
			Func<ItemType, int> toComparerToObjectItemWith)
		{
			return ListExtension.FindItemWithDichotomyInRange<ItemType>(
				itemListSorted,
				-1,
				-1,
				toComparerToObjectItemWith);
		}
	}
}
