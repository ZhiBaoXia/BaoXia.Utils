using BaoXia.Utils.Extensions;
using System.Collections.Generic;

namespace BaoXia.Utils;

public static class ItemsUtil
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static IEnumerable<ItemType>? ItemsOf<ItemType>(
		params IEnumerable<ItemType>?[] itemSets)
	{
		List<ItemType>? itemList = null;
		IEnumerable<ItemType>? firstItemSetNotEmpty = null;
		foreach (var itemSet in itemSets)
		{
			if (itemSet.IsEmpty())
			{
				continue;
			}

			if (itemList == null)
			{
				if (firstItemSetNotEmpty == null)
				{
					firstItemSetNotEmpty = itemSet;
					continue;
				}
				else
				{
					itemList ??= [];
					itemList.AddRange(firstItemSetNotEmpty);
					firstItemSetNotEmpty = null;
				}
			}

			// !!!
			itemList.AddRange(itemSet);
			// !!!
		}

		if (itemList != null)
		{
			return itemList;
		}

		return firstItemSetNotEmpty;
	}

	#endregion
}