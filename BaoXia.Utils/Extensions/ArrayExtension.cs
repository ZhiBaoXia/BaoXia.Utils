using System;
using System.Collections.Generic;

namespace BaoXia.Utils.Extensions;

/// <summary>
/// 数组扩展类。
/// </summary>
public static class ArrayExtension
{
	/// <summary>
	/// 比较两个数组中的每一个元素是否相等。
	/// </summary>
	/// <typeparam name="ItemType">指定的数组元素类型。</typeparam>
	/// <param name="items">当前数组对象。</param>
	/// <param name="anotherItems">另一个数组对象。</param>
	/// <param name="isNullEqualsNone">另一个数组对象为“null”时，如果当前数组对象没有元素，是否视为相等，默认为“true”。</param>
	/// <returns>两个数组中的每一个元素都相等时，返回“true”，否则返回“false”。</returns>
	public static bool IsItemsEqual<ItemType>(
		this ItemType[]? items,
		ItemType[]? anotherItems,
		bool isNullEqualsNone = true)
	{
		if (items == null)
		{
			if (anotherItems == null)
			{
				return true;
			}
			else if (isNullEqualsNone
				&& anotherItems.Length < 1)
			{
				return true;
			}
			return false;
		}
		if (anotherItems == null)
		{
			if (isNullEqualsNone == true
				&& items.Length < 1)
			{
				return false;
			}
			return false;
		}
		if (anotherItems.Length != items.Length)
		{
			return false;
		}

		for (var itemIndex = 0; itemIndex < items.Length; itemIndex++)
		{
			var item = items[itemIndex];
			var anotherItem = anotherItems[itemIndex];
			if (object.Equals(item, anotherItem) == false)
			{
				return false;
			}
		}
		return true;
	}

	/// <summary>
	/// 获取两个数组中相同元素的数量。
	/// </summary>
	/// <typeparam name="ItemType">指定的数组元素类型。</typeparam>
	/// <param name="items">当前数组对象。</param>
	/// <param name="isNullEqualsNone">另一个数组对象为“null”时，如果当前数组对象没有元素，是否视为相等，默认为“true”。</param>
	/// <returns>两个数组中相同元素的数量。</returns>
	public static int GetSameItemsCount<ItemType>(
		this ItemType[]? items,
		ItemType[]? anotherItems)
	{
		if (items == null
			|| items.Length < 1
			|| anotherItems == null
			|| anotherItems.Length < 1)
		{
			return 0;
		}

		var sameItemsCount = 0;
		foreach (var item in items)
		{
			foreach (var anotherItem in anotherItems)
			{
				if (object.Equals(item, anotherItem))
				{
					sameItemsCount++;
					break;
				}
			}
		}
		return sameItemsCount;
	}

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
	/// 通过在指定位置上插入多个元素，创建新的元素数组。
	/// </summary>
	/// <typeparam name="ItemType">数组元素类型。</typeparam>
	/// <param name="items">当前数组。</param>
	/// <param name="insertItemIndex">要插入元素的索引值。</param>
	/// <param name="itemNeedInserted">要插入的元素。</param>
	/// <returns>插入元素后新建的数组对象，即使新数组的长度为0，仍会返回有效的数组对象。 </returns>
	public static ItemType[] ArrayByInsertAt<ItemType>(
		this ItemType[] items,
		int insertItemIndex,
		ItemType itemNeedInserted)
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
			newItems[insertItemIndex] = itemNeedInserted;
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
	/// 通过在指定位置上插入多个元素，创建新的元素数组。
	/// </summary>
	/// <typeparam name="ItemType">数组元素类型。</typeparam>
	/// <param name="items">当前数组。</param>
	/// <param name="insertItemIndex">要插入元素的索引值。</param>
	/// <param name="itemsNeedInserted">要插入的多个元素。</param>
	/// <returns>插入元素后新建的数组对象，即使新数组的长度为0，仍会返回有效的数组对象。 </returns>
	public static ItemType[] ArrayByInsertAt<ItemType>(
		this ItemType[] items,
		int insertItemIndex,
		ICollection<ItemType> itemsNeedInserted)
	{
		if (insertItemIndex < 0
			|| insertItemIndex > items.Length)
		{
			throw new IndexOutOfRangeException();
		}

		////////////////////////////////////////////////

		var itemsNeedInsertedCount = itemsNeedInserted.Count;
		var newItems = new ItemType[items.Length + itemsNeedInsertedCount];
		{
			Array.Copy(
				items,
				newItems,
				insertItemIndex);
			// !!!
			var itemInsertIndex = 0;
			foreach (var itemNeedInserted in itemsNeedInserted)
			{
				newItems[insertItemIndex + itemInsertIndex] = itemNeedInserted;
				itemInsertIndex++;
			}
			// !!!
			Array.Copy(
				items,
				insertItemIndex,
				newItems,
				insertItemIndex + itemsNeedInsertedCount,
				items.Length - insertItemIndex);
		}
		return newItems;
	}

	public static ItemType[] ArrayByInsertWithOrder<ItemType>(
		this ItemType[] items,
		ItemType newItem,
		Func<ItemType, ItemType, int> toCompareItem)
	{
		if (items.Length < 1)
		{
			// !!!
			return items.ArrayByAdd(newItem);
			// !!!
		}

		for (var itemIndex = 0;
			itemIndex < items.Length;
			itemIndex++)
		{
			var item = items[itemIndex];
			var compareResult = toCompareItem(newItem, item);
			if (compareResult < 0)
			{
				// !!!
				return items.ArrayByInsertAt(itemIndex, newItem);
				// !!!
			}
			if (itemIndex == (items.Length - 1))
			{
				// !!!
				return items.ArrayByInsertAt(itemIndex + 1, newItem);
				// !!!
			}
		}
		return items;
	}

	public static ItemType[] ArrayByInsertWithOrderDescending<ItemType>(
		this ItemType[] items,
		ItemType newItem,
		Func<ItemType, ItemType, int> toCompareItem)
	{
		if (items.Length < 1)
		{
			// !!!
			return items.ArrayByAdd(newItem);
			// !!!
		}

		for (var itemIndex = 0;
			itemIndex < items.Length;
			itemIndex++)
		{
			var item = items[itemIndex];
			var compareResult = toCompareItem(newItem, item);
			if (compareResult > 0)
			{
				// !!!
				return items.ArrayByInsertAt(itemIndex, newItem);
				// !!!
			}
			if (itemIndex == (items.Length - 1))
			{
				// !!!
				return items.ArrayByInsertAt(itemIndex + 1, newItem);
				// !!!
			}
		}
		return items;
	}


	/// <summary>
	/// 通过在新增多个元素，创建新的元素数组。
	/// </summary>
	/// <typeparam name="ItemType">数组元素类型。</typeparam>
	/// <param name="items">当前数组。</param>
	/// <param name="newItem">要加入的新的对象。</param>
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
	/// 通过在新增多个元素，创建新的元素数组。
	/// </summary>
	/// <typeparam name="ItemType">数组元素类型。</typeparam>
	/// <param name="items">当前数组。</param>
	/// <param name="newItems">要加入的新的多个对象。</param>
	/// <returns>新增元素后新建的数组对象，即使新数组的长度为0，仍会返回有效的数组对象。 </returns>
	public static ItemType[] ArrayByAdd<ItemType>(
		this ItemType[] items,
		ICollection<ItemType> newItems)
	{
		return items.ArrayByInsertAt(
			items.Length,
			newItems);
	}

	/// <summary>
	/// 通过在新增多个元素，创建新的元素数组。
	/// </summary>
	/// <typeparam name="ItemType">数组元素类型。</typeparam>
	/// <param name="items">当前数组。</param>
	/// <param name="newItems">要加入的新的多个对象。</param>
	/// <returns>新增元素后新建的数组对象，即使新数组的长度为0，仍会返回有效的数组对象。 </returns>
	public static ItemType[] ArrayByAdd<ItemType>(
		this ItemType[] items,
		params ItemType[] newItems)
	{
		return items.ArrayByInsertAt(items.Length, newItems);
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
	public static ItemType[] ArrayByRemoveFrom<ItemType>(
		this ItemType[] items,
		int firstItemIndexNeedRemove,
		int itemsCountNeedRemove)
	{
		if (items.Length < 1)
		{
			return items;
		}

		var endItemIndexNeedRemove
			= firstItemIndexNeedRemove + itemsCountNeedRemove;
		var itemList = new List<ItemType>();
		for (var itemIndex = 0;
			itemIndex < items.Length;
			itemIndex++)
		{
			var item = items[itemIndex];
			if (itemIndex < firstItemIndexNeedRemove
				|| itemIndex >= endItemIndexNeedRemove)
			{
				itemList.Add(item);
			}
		}

		return [.. itemList];
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
		return [.. itemList];
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
		return [.. itemList];
	}

	/// <summary>
	/// 在指定的数组中，查找目标元素。
	/// </summary>
	/// <typeparam name="ItemType">数组元素的类型。</typeparam>
	/// <param name="items">指定的数组对象。</param>
	/// <param name="searchRangeBeginIndex">指定查找区域的起始数组索引。</param>
	/// <param name="searchRangeLength">指定查找区域的长度。</param>
	/// <param name="toIsObjectItem">判断元素是否为目标元素的函数。</param>
	/// <param name="objectItemIndex">目标元素在数组中的索引值。</param>
	/// <returns>如果查找到目标元素，则返回对应的元素，否则返回“default”。</returns>
	public static ItemType? Find<ItemType>(
		this ItemType[]? items,
		int searchRangeBeginIndex,
		int searchRangeLength,
		Func<ItemType, int, bool> toIsObjectItem,
		out int objectItemIndex)
	{
		// !!!
		objectItemIndex = -1;
		// !!!

		if (ArrayUtil.IsEmpty(items))
		{
			return default;
		}

		var searchRangeEndIndex = searchRangeBeginIndex + searchRangeLength;
		for (var itemIndex = searchRangeBeginIndex;
			itemIndex < searchRangeEndIndex;
			itemIndex++)
		{
			var item = items[itemIndex];
			if (toIsObjectItem(item, itemIndex))
			{
				// !!!
				objectItemIndex = itemIndex;
				// !!!
				return item;
			}
		}
		return default;
	}

	/// <summary>
	/// 在指定的数组中，查找目标元素。
	/// </summary>
	/// <typeparam name="ItemType">数组元素的类型。</typeparam>
	/// <param name="items">指定的数组对象。</param>
	/// <param name="searchRangeBeginIndex">指定查找区域的起始数组索引。</param>
	/// <param name="searchRangeLength">指定查找区域的长度。</param>
	/// <param name="toIsObjectItem">判断元素是否为目标元素的函数。</param>
	/// <param name="objectItemIndex">目标元素在数组中的索引值。</param>
	/// <returns>如果查找到目标元素，则返回对应的元素，否则返回“default”。</returns>
	public static ItemType? Find<ItemType>(
		this ItemType[]? items,
		Func<ItemType, int, bool> toIsObjectItem,
		out int objectItemIndex)
	{
		// !!!
		objectItemIndex = -1;
		// !!!

		if (ArrayUtil.IsEmpty(items))
		{
			return default;
		}

		return Find(
			items,
			0,
			items.Length,
			toIsObjectItem,
			out objectItemIndex);
	}



	/// <summary>
	/// 使用二分法查找目标元素在列表中的索引值。
	/// </summary>
	/// <typeparam name="ItemType">列表中的元素类型。</typeparam>
	/// <param name="itemsSorted">要进行查找的列表对象，注意：元素集合必须是正序排列。</param>
	/// <param name="searchRangeBeginIndex">开始查找的对象索引值。</param>
	/// <param name="searchRangeLength">查找范围的对象数量。</param>
	/// <param name="toComparerToObjectItemWith">当前元素和目标元素的比较结果，当前元素小于模板元素时，返回：-1，等于时，返回：0，大于时返回：1 。</param>
	/// <param name="isGetItemNearestLeft">是否获取最接近目标的左侧对象。</param>
	/// <param name="itemIndexNearest">最接近目标的左侧对象索引值。</param>
	/// <param name="itemNearest">最接近目标的左侧对象。</param>
	/// <returns>查找到目标元素后，返回目标元素在列表中的索引值，否则返回：-1 。</returns>
	public static int FindItemIndexWithDichotomyInRange<ItemType>(
		this ItemType[]? itemsSorted,
		bool isItemsSortedWithAscending,
		int searchRangeBeginIndex,
		int searchRangeLength,
		Func<ItemType, int, int> toComparerToObjectItemWith,
		bool isGetItemNearestLeft,
		out int itemIndexNearest,
		out ItemType? itemNearest)
	{
		itemIndexNearest = -1;
		itemNearest = default;

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
			var resultOfComparerItemToObjectItem = toComparerToObjectItemWith(item, searchShotIndex);
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
				itemIndexNearest = searchShotIndex;
				itemNearest = item;
				// !!!
				if (searchRangeLength == 1)
				{
					break;
				}
				else
				{
					if (isItemsSortedWithAscending == false)
					{
						resultOfComparerItemToObjectItem *= -1;
					}
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
				itemIndexNearest = objectItemIndexMatched - 1;
				if (itemIndexNearest >= 0)
				{
					itemNearest = items[itemIndexNearest];
				}
				else
				{
					itemNearest = default;
				}
			}
			else if (itemNearest == null)
			{
				itemIndexNearest = 0;
				itemNearest = items[itemIndexNearest];
			}
			else if (toComparerToObjectItemWith(itemNearest, itemIndexNearest) > 0)
			{
				itemIndexNearest--;
				if (itemIndexNearest >= 0)
				{
					itemNearest = items[itemIndexNearest];
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
				itemIndexNearest = objectItemIndexMatched + 1;
				if (itemIndexNearest < items.Length)
				{
					itemNearest = items[itemIndexNearest];
				}
				else
				{
					itemNearest = default;
				}
			}
			else if (itemNearest == null)
			{
				itemIndexNearest = items.Length;
				itemNearest = default;
			}
			else if (toComparerToObjectItemWith(itemNearest, itemIndexNearest) < 0)
			{
				itemIndexNearest++;
				if (itemIndexNearest < items.Length)
				{
					itemNearest = items[itemIndexNearest];
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
		this ItemType[]? itemsSorted,
		bool isItemsSortedWithAscending,
		Func<ItemType, int, int> toComparerToObjectItemWith,
		bool isGetItemNearestLeft,
		out int itemIndexNearest,
		out ItemType? itemNearest)
	{
		return ArrayExtension.FindItemIndexWithDichotomyInRange<ItemType>(
			itemsSorted,
			isItemsSortedWithAscending,
			-1,
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
		this ItemType[]? itemsSorted,
		bool isItemsSortedWithAscending,
		int searchRangeBeginIndex,
		int searchRangeEndIndex,
		Func<ItemType, int, int> toComparerToObjectItemWith,
		bool isGetItemNearestLeft,
		out int itemIndexNearest,
		out ItemType? itemNearest)
	{
		var itemIndex = ArrayExtension.FindItemIndexWithDichotomyInRange(
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
	/// <param name="isGetItemNearestLeft">是否获取最接近目标的左侧对象。</param>
	/// <param name="itemIndexNearest">最接近目标的左侧对象索引值。</param>
	/// <param name="itemNearest">最接近目标的左侧对象。</param>
	/// <returns>查找到目标元素后，返回目标元素，否则返回：default 。</returns>
	public static ItemType? FindItemWithDichotomy<ItemType>(
		this ItemType[]? itemsSorted,
		bool isItemsSortedWithAscending,
		Func<ItemType, int, int> toComparerToObjectItemWith,
		bool isGetItemNearestLeft,
		out int itemIndexNearest,
		out ItemType? itemNearest)
	{
		return ArrayExtension.FindItemWithDichotomyInRange<ItemType>(
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
		this ItemType[]? itemsSorted,
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
		this ItemType[]? itemsSorted,
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
		this ItemType[]? itemsSorted,
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
		this ItemType[]? itemsSorted,
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


	public static ItemType? FirstItemOrDefault<ItemType>(this ItemType[] items)
	{
		if (items.Length > 0)
		{
			return items[0];
		}
		return default;
	}
	public static ItemType? LastItemOrDefault<ItemType>(this ItemType[] items)
	{
		if (items.Length > 0)
		{
			return items[^1];
		}
		return default;
	}

	public static List<ItemType> GetPageItems<ItemType>(
	    this ItemType[] items,
	    //
	    Func<List<ItemType>, List<ItemType>>? toSortItems,
	    //
	    int pageIndex,
	    int pageSize)
	{
		int itemsCount = items.Length;
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