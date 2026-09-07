using BaoXia.Utils.Models;
using System;

namespace BaoXia.Utils.Extensions;

public static class TaskIntervalInfosByDurationSecondsArrayExntesion
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	extension(TaskIntervalInfoByDurationSeconds[] taskIntervalInfosByDurationSeconds)
	{
		public TaskIntervalInfoByDurationSeconds[] SortByDurationSeconds()
		{
			taskIntervalInfosByDurationSeconds.Sort((intervalInfoA, intervalInfoB) =>
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
			return taskIntervalInfosByDurationSeconds;
		}

		public double GetMinTaskIntervalSeconds()
		{
			var minIntervalSeconds = double.MaxValue;
			foreach (var taskIntervalInfoByDurationSeconds in taskIntervalInfosByDurationSeconds)
			{
				if (minIntervalSeconds > taskIntervalInfoByDurationSeconds.TaskIntervalSeconds)
				{
					minIntervalSeconds = taskIntervalInfoByDurationSeconds.TaskIntervalSeconds;
				}
			}
			return minIntervalSeconds;
		}

		public double? GetTaskIntervalSecondsWithDurationSeconds(double durationSeconds, bool isTaskIntervalInfosNeedResort = false)
		{
			if (isTaskIntervalInfosNeedResort)
			{
				taskIntervalInfosByDurationSeconds = SortByDurationSeconds(taskIntervalInfosByDurationSeconds);
			}
			foreach (var taskIntervalInfoByDurationSeconds in taskIntervalInfosByDurationSeconds)
			{
				var taskIntervalInfoMaxDurationSeconds = taskIntervalInfoByDurationSeconds.MaxDurationSeconds;
				if (taskIntervalInfoMaxDurationSeconds <= 0)
				{
					return taskIntervalInfoByDurationSeconds.TaskIntervalSeconds;
				}
				if (durationSeconds <= taskIntervalInfoMaxDurationSeconds)
				{
					return taskIntervalInfoByDurationSeconds.TaskIntervalSeconds;
				}
			}
			return null;
		}
	}

	#endregion
}
