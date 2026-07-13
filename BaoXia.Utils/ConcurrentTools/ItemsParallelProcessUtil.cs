using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaoXia.Utils.ConcurrentTools;

public class ItemsParallelProcessUtil
{

	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static void ForEach<ItemType>(IEnumerable<ItemType> items, int tasksCountToProcessItemsMax, Action<ItemType> toProcessItem)
	{
		if (tasksCountToProcessItemsMax <= 1)
		{
			foreach (var item in items)
			{
				toProcessItem(item);
			}
			return;
		}

		Parallel.ForEach(items, new ParallelOptions
		{
			MaxDegreeOfParallelism = tasksCountToProcessItemsMax
		}, toProcessItem);
	}

	#endregion


}