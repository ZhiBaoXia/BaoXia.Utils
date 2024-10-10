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

	private static ItemType[]? GetItemsSortByTime_Asc_InTimeSection<ItemType>(
		ItemType[] items,
		DateTimeOffset beginTime,
		DateTimeOffset endTime,
		Func<ItemType, DateTimeOffset, int> toCompareTimeWithItem,
		bool isGetObjectItemsCountOnly,
		out int objectItemsCount)
	{
		//
		objectItemsCount = 0;
		//

		// 【注意】beginTime和endTime含义不同，不可呼唤。
		//if (beginTime > endTime)
		//{
		//	(beginTime, endTime) = (endTime, beginTime);
		//}

		var firstOrderIndexMatched = items.FindItemIndexWithDichotomy(
			(item, itemIndex) =>
			{
				return toCompareTimeWithItem(item, beginTime);
			},
			//
			true,
			out var firstOrderIndex,
			out _);

		if (firstOrderIndexMatched >= 0)
		{
			firstOrderIndex = firstOrderIndexMatched;
		}
		else if (firstOrderIndex < 0)
		{
			return null;
		}
		for (;
			firstOrderIndex >= 0;
			firstOrderIndex--)
		{
			var item = items[firstOrderIndex];
			if (toCompareTimeWithItem(item, beginTime) < 0)
			{
				// !!!
				firstOrderIndex++;
				break;
				// !!!
			}
		}
		var itemsCount = items.Length;
		if (firstOrderIndex >= itemsCount)
		{
			return null;
		}

		var endOrderIndexMatched = items.FindItemIndexWithDichotomy(
			(item, itemIndex) =>
			{
				return toCompareTimeWithItem(item, endTime);
			},
			//
			true, // 注意这里是true
			out var endOrderIndex,
			out _);
		if (endOrderIndexMatched >= 0)
		{
			endOrderIndex = endOrderIndexMatched;
		}
		else if (endOrderIndex < 0)
		{
			return null;
		}
		for (;
			endOrderIndex >= 0;
			endOrderIndex--)
		{
			var item = items[endOrderIndex];
			if (toCompareTimeWithItem(item, endTime) < 0)
			{
				// !!!
				endOrderIndex++;
				break;
				// !!!
			}
		}

		objectItemsCount = endOrderIndex - firstOrderIndex;
		if (objectItemsCount <= 0
			|| isGetObjectItemsCountOnly)
		{
			return null;
		}

		var objectItems = new ItemType[objectItemsCount];
		{
			Array.Copy(
				items,
				firstOrderIndex,
				objectItems,
				0,
				objectItemsCount);
		}
		return objectItems;
	}

	public static ItemType[]? GetItemsSortByTime_Asc_InTimeSection<ItemType>(
		this ItemType[] items,
		DateTimeOffset beginTime,
		DateTimeOffset endTime,
		Func<ItemType, DateTimeOffset, int> toCompareTimeWithItem)
	{
		return GetItemsSortByTime_Asc_InTimeSection(
			items,
			beginTime,
			endTime,
			toCompareTimeWithItem,
			false,
			out _);
	}

	public static int GetCountOfItemsSortByTime_Asc_InTimeSection<ItemType>(
		this ItemType[] items,
		DateTimeOffset beginTime,
		DateTimeOffset endTime,
		Func<ItemType, DateTimeOffset, int> toCompareTimeWithItem)
	{
		GetItemsSortByTime_Asc_InTimeSection<ItemType>(
			items,
			beginTime,
			endTime,
			toCompareTimeWithItem,
			true,
			out int objectItemsCount);
		{ }
		return objectItemsCount;
	}

	private static ItemType[]? GetItemsSortByTime_Asc_InTimeSection<ItemType>(
		List<ItemType> items,
		DateTimeOffset beginTime,
		DateTimeOffset endTime,
		Func<ItemType, DateTimeOffset, int> toCompareTimeWithItem,
		bool isGetObjectItemsCountOnly,
		out int objectItemsCount)
	{
		//
		objectItemsCount = 0;
		//

		// 【注意】beginTime和endTime含义不同，不可呼唤。
		//if (beginTime > endTime)
		//{
		//	(beginTime, endTime) = (endTime, beginTime);
		//}

		var firstOrderIndexMatched = items.FindItemIndexWithDichotomy(
			(item, itemIndex) =>
			{
				return toCompareTimeWithItem(item, beginTime);
			},
			//
			true,
			out var firstOrderIndex,
			out _);
		if (firstOrderIndexMatched >= 0)
		{
			firstOrderIndex = firstOrderIndexMatched;
		}
		else if (firstOrderIndex < 0)
		{
			return null;
		}
		for (;
			firstOrderIndex >= 0;
			firstOrderIndex--)
		{
			var item = items[firstOrderIndex];
			if (toCompareTimeWithItem(item, beginTime) < 0)
			{
				// !!!
				firstOrderIndex++;
				break;
				// !!!
			}
		}
		var itemsCount = items.Count;
		if (firstOrderIndex >= itemsCount)
		{
			return null;
		}

		var endOrderIndexMatched = items.FindItemIndexWithDichotomy(
			(item, itemIndex) =>
			{
				return toCompareTimeWithItem(item, endTime);
			},
			//
			true, // 注意这里是true
			out var endOrderIndex,
			out _);
		if (endOrderIndexMatched >= 0)
		{
			endOrderIndex = endOrderIndexMatched;
		}
		else if (endOrderIndex < 0)
		{
			return null;
		}
		for (;
			endOrderIndex >= 0;
			endOrderIndex--)
		{
			var item = items[endOrderIndex];
			if (toCompareTimeWithItem(item, endTime) < 0)
			{
				// !!!
				endOrderIndex++;
				break;
				// !!!
			}
		}

		objectItemsCount = endOrderIndex - firstOrderIndex;
		if (objectItemsCount <= 0
			|| isGetObjectItemsCountOnly)
		{
			return null;
		}

		var objectItems = new ItemType[objectItemsCount];
		{
			items.CopyTo(
				objectItems,
				firstOrderIndex);
		}
		return objectItems;
	}

	public static ItemType[]? GetItemsSortByTime_Asc_InTimeSection<ItemType>(
		this List<ItemType> items,
		DateTimeOffset beginTime,
		DateTimeOffset endTime,
		Func<ItemType, DateTimeOffset, int> toCompareTimeWithItem)
	{
		return GetItemsSortByTime_Asc_InTimeSection<ItemType>(
			items,
			beginTime,
			endTime,
			toCompareTimeWithItem,
			false,
			out _);
	}

	public static int GetCountOfItemsSortByTime_Asc_InTimeSection<ItemType>(
		this List<ItemType> items,
		DateTimeOffset beginTime,
		DateTimeOffset endTime,
		Func<ItemType, DateTimeOffset, int> toCompareTimeWithItem)
	{
		GetItemsSortByTime_Asc_InTimeSection<ItemType>(
			items,
			beginTime,
			endTime,
			toCompareTimeWithItem,
			false,
			out var objectItemsCount);
		{ }
		return objectItemsCount;
	}

	private static ItemType[]? GetItemsSortByTime_Desc_InTimeSection<ItemType>(
		ItemType[] items,
		DateTimeOffset beginTime,
		DateTimeOffset endTime,
		Func<ItemType, DateTimeOffset, int> toCompareTimeWithItem,
		bool isGetObjectItemsCountOnly,
		out int objectItemsCount)
	{
		//
		objectItemsCount = 0;
		//

		// 【注意】beginTime和endTime含义不同，不可呼唤。
		//if (beginTime > endTime)
		//{
		//	(beginTime, endTime) = (endTime, beginTime);
		//}


		var firstOrderIndexMatched = items.FindItemIndexWithDichotomy(
			(item, itemIndex) =>
			{
				return -1 * toCompareTimeWithItem(
					item,
					// !!!
					beginTime
					// !!!
					);
			},
			//
			true,
			out var firstOrderIndex,
			out _);
		if (firstOrderIndexMatched >= 0)
		{
			firstOrderIndex = firstOrderIndexMatched + 1;
		}
		else if (firstOrderIndex < 0)
		{
			return null;
		}
		for (;
			firstOrderIndex >= 0;
			firstOrderIndex--)
		{
			var item = items[firstOrderIndex];
			if (toCompareTimeWithItem(item, beginTime) < 0)
			{
				// !!!
				//firstOrderIndex += 0;
				break;
				// !!!
			}
		}
		var itemsCount = items.Length;
		if (firstOrderIndex >= itemsCount)
		{
			return null;
		}

		var endOrderIndexMatched = items.FindItemIndexWithDichotomy(
			(item, itemIndex) =>
			{
				return -1 * toCompareTimeWithItem(
					item,
					// !!!
					beginTime
					// !!!
					);
			},
			//
			true, // 注意这里是true
			out var endOrderIndex,
			out _);
		if (endOrderIndexMatched >= 0)
		{
			endOrderIndex = endOrderIndexMatched + 1;
		}
		else if (endOrderIndex < 0)
		{
			return null;
		}
		for (;
			endOrderIndex < itemsCount;
			endOrderIndex++)
		{
			var item = items[endOrderIndex];
			if (toCompareTimeWithItem(item, beginTime) < 0)
			{
				// !!!
				//endOrderIndex += 0;
				break;
				// !!!
			}
		}

		objectItemsCount = endOrderIndex - firstOrderIndex;
		if (objectItemsCount <= 0
			|| isGetObjectItemsCountOnly)
		{
			return null;
		}

		var objectItems = new ItemType[objectItemsCount];
		{
			Array.Copy(
				items,
				firstOrderIndex,
				objectItems,
				0,
				objectItemsCount);
		}
		return objectItems;
	}

	public static ItemType[]? GetItemsSortByTime_Desc_InTimeSection<ItemType>(
		this ItemType[] items,
		DateTimeOffset beginTime,
		DateTimeOffset endTime,
		Func<ItemType, DateTimeOffset, int> toCompareTimeWithItem)
	{
		return GetItemsSortByTime_Desc_InTimeSection(
			items,
			beginTime,
			endTime,
			toCompareTimeWithItem,
			false,
			out _);
	}

	public static int GetCountOfItemsSortByTime_Desc_InTimeSection<ItemType>(
		this ItemType[] items,
		DateTimeOffset beginTime,
		DateTimeOffset endTime,
		Func<ItemType, DateTimeOffset, int> toCompareTimeWithItem)
	{
		GetItemsSortByTime_Desc_InTimeSection(
			items,
			beginTime,
			endTime,
			toCompareTimeWithItem,
			true,
			out var objectItemsCount);
		{ }
		return objectItemsCount;
	}

	private static ItemType[]? GetItemsSortByTime_Desc_InTimeSection<ItemType>(
		List<ItemType> items,
		DateTimeOffset beginTime,
		DateTimeOffset endTime,
		Func<ItemType, DateTimeOffset, int> toCompareTimeWithItem,
		bool isGetObjectItemsCountOnly,
		out int objectItemsCount)
	{
		//
		objectItemsCount = 0;
		//
		if (beginTime > endTime)
		{
			(beginTime, endTime) = (endTime, beginTime);
		}

		var firstOrderIndexMatched = items.FindItemIndexWithDichotomy(
			(item, itemIndex) =>
			{
				return toCompareTimeWithItem(
					item,
					// !!!
					endTime
					// !!!
					);
			},
			//
			true,
			out var firstOrderIndex,
			out _);
		if (firstOrderIndexMatched >= 0)
		{
			firstOrderIndex = firstOrderIndexMatched + 1;
		}
		else if (firstOrderIndex < 0)
		{
			return null;
		}
		var itemsCount = items.Count;
		for (;
			firstOrderIndex < itemsCount;
			firstOrderIndex++)
		{
			var item = items[firstOrderIndex];
			if (toCompareTimeWithItem(item, endTime) < 0)
			{
				// !!!
				//firstOrderIndex += 0;
				break;
				// !!!
			}
		}
		if (firstOrderIndex >= itemsCount)
		{
			return null;
		}

		var endOrderIndexMatched = items.FindItemIndexWithDichotomy(
			(item, itemIndex) =>
			{
				return toCompareTimeWithItem(
					item,
					// !!!
					beginTime
					// !!!
					);
			},
			//
			true, // 注意这里是true
			out var endOrderIndex,
			out _);
		if (endOrderIndexMatched >= 0)
		{
			endOrderIndex = endOrderIndexMatched + 1;
		}
		else if (endOrderIndex < 0)
		{
			return null;
		}
		for (;
			endOrderIndex < itemsCount;
			endOrderIndex++)
		{
			var item = items[endOrderIndex];
			if (toCompareTimeWithItem(item, beginTime) < 0)
			{
				// !!!
				//endOrderIndex += 0;
				break;
				// !!!
			}
		}

		objectItemsCount = endOrderIndex - firstOrderIndex;
		if (objectItemsCount <= 0
			|| isGetObjectItemsCountOnly)
		{
			return null;
		}

		var objectItems = new ItemType[objectItemsCount];
		{
			items.CopyTo(
				objectItems,
				firstOrderIndex);
		}
		return objectItems;
	}

	public static ItemType[]? GetItemsSortByTime_Desc_InTimeSection<ItemType>(
		this List<ItemType> items,
		DateTimeOffset beginTime,
		DateTimeOffset endTime,
		Func<ItemType, DateTimeOffset, int> toCompareTimeWithItem)
	{
		return GetItemsSortByTime_Desc_InTimeSection<ItemType>(
			items,
			beginTime,
			endTime,
			toCompareTimeWithItem,
			false,
			out _);
	}

	public static int GetCountOfItemsSortByTime_Desc_InTimeSection<ItemType>(
		this List<ItemType> items,
		DateTimeOffset beginTime,
		DateTimeOffset endTime,
		Func<ItemType, DateTimeOffset, int> toCompareTimeWithItem)
	{
		GetItemsSortByTime_Desc_InTimeSection<ItemType>(
		       items,
		       beginTime,
		       endTime,
		       toCompareTimeWithItem,
		       true,
		       out var objectItemsCount);
		{ }
		return objectItemsCount;
	}

	#endregion
}