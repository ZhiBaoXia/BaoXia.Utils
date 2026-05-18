using System;

namespace BaoXia.Utils;

public class CompareUtil
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static int GetCompareResultOf<ItemType>(
		ItemType itemA, ItemType itemB, params Func<ItemType, ItemType, int>[] toCompareResults)
	{
		foreach (var toCompareResult in toCompareResults)
		{
			var compareResult = toCompareResult(itemA, itemB);
			if (compareResult != 0)
			{
				return compareResult;
			}
		}
		return 0;
	}

	#endregion
}