using BaoXia.Utils.Models;
using System.Collections.Generic;

namespace BaoXia.Utils.Extensions;

public static class TaskIntervalInfoByTaskRunDurationListExntesion
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	extension(List<TaskIntervalInfoByTaskRunDuration> taskIntervalInfosByTaskRunDuration)
	{
		public List<TaskIntervalInfoByTaskRunDuration> SortByTaskRunDuration()
		{
			taskIntervalInfosByTaskRunDuration.Sort((intervalInfoA, intervalInfoB) =>
			{
				var secondsAfterOpenOrderA = intervalInfoA.MaxDurationSeconds;
				if (secondsAfterOpenOrderA <= 0)
				{
					secondsAfterOpenOrderA = double.MaxValue;
				}
				var secondsAfterOpenOrderB = intervalInfoB.MaxDurationSeconds;
				if (secondsAfterOpenOrderB <= 0)
				{
					secondsAfterOpenOrderB = double.MaxValue;
				}
				return secondsAfterOpenOrderA.CompareTo(secondsAfterOpenOrderB);
			});
			return taskIntervalInfosByTaskRunDuration;
		}
	}

	#endregion
}
