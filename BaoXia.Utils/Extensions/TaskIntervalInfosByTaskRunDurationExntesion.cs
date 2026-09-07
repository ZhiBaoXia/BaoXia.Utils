using BaoXia.Utils.Models;
using System;
using System.Collections.Generic;

namespace BaoXia.Utils.Extensions;

public static class TaskIntervalInfosByDurationSecondsArrayExntesion
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	extension(IEnumerable<TaskIntervalInfoByTaskRunDuration> taskIntervalInfosByTaskRunDuration)
	{
		public double? GetMinTaskIntervalSeconds()
		{
			double? minIntervalSeconds = null;
			foreach (var taskIntervalInfoByDurationSeconds in taskIntervalInfosByTaskRunDuration)
			{
				if (minIntervalSeconds > taskIntervalInfoByDurationSeconds.TaskIntervalSeconds)
				{
					minIntervalSeconds = taskIntervalInfoByDurationSeconds.TaskIntervalSeconds;
				}
			}
			return minIntervalSeconds;
		}

		public double? GetTaskIntervalSecondsWithTaskRunDuration(TimeSpan taskRunDuration, bool isTaskIntervalInfosNeedResort = false)
		{
			if (isTaskIntervalInfosNeedResort)
			{
				var taskIntervalInfoByDurationList = new List<TaskIntervalInfoByTaskRunDuration>();
				{
					taskIntervalInfoByDurationList.AddRange(taskIntervalInfosByTaskRunDuration);
				}
				taskIntervalInfoByDurationList.SortByTaskRunDuration();
				taskIntervalInfosByTaskRunDuration = taskIntervalInfoByDurationList;
			}
			var taskRunDurationSeconds = taskRunDuration.TotalSeconds;
			foreach (var taskIntervalInfoByDurationSeconds in taskIntervalInfosByTaskRunDuration)
			{
				var taskIntervalInfoMaxDurationSeconds = taskIntervalInfoByDurationSeconds.MaxDurationSeconds;
				if (taskIntervalInfoMaxDurationSeconds <= 0)
				{
					return taskIntervalInfoByDurationSeconds.TaskIntervalSeconds;
				}
				if (taskRunDurationSeconds <= taskIntervalInfoMaxDurationSeconds)
				{
					return taskIntervalInfoByDurationSeconds.TaskIntervalSeconds;
				}
			}
			return null;
		}
	}

	#endregion
}
