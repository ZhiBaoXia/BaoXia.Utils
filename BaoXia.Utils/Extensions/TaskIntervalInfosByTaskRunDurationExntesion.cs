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

	extension(IEnumerable<TaskIntervalInfoByTaskRunDuration>? taskIntervalInfosByTaskRunDuration)
	{
		public bool IsTaskIntervalInfosValid(double taskIntervalSecondsMin, double taskIntervalSecondsMax)
		{
			if (taskIntervalInfosByTaskRunDuration == null
				|| !double.IsFinite(taskIntervalSecondsMin)
				|| !double.IsFinite(taskIntervalSecondsMax)
				|| taskIntervalSecondsMin <= 0.0
				|| taskIntervalSecondsMax < taskIntervalSecondsMin)
			{
				return false;
			}

			var isTaskIntervalInfoExisted = false;
			var taskIntervalSecondsActualMin = double.MaxValue;
			var taskIntervalInfosWithoutDurationSecondsMaxCount = 0;
			foreach (var taskIntervalInfoByTaskRunDuration in taskIntervalInfosByTaskRunDuration)
			{
				if (taskIntervalInfoByTaskRunDuration == null
					|| !double.IsFinite(taskIntervalInfoByTaskRunDuration.MaxDurationSeconds)
					|| !double.IsFinite(taskIntervalInfoByTaskRunDuration.TaskIntervalSeconds)
					|| taskIntervalInfoByTaskRunDuration.TaskIntervalSeconds <= 0.0)
				{
					return false;
				}
				isTaskIntervalInfoExisted = true;

				if (taskIntervalInfoByTaskRunDuration.MaxDurationSeconds <= 0.0)
				{
					taskIntervalInfosWithoutDurationSecondsMaxCount++;
					if (taskIntervalInfosWithoutDurationSecondsMaxCount > 1)
					{
						return false;
					}
				}
				if (taskIntervalSecondsActualMin > taskIntervalInfoByTaskRunDuration.TaskIntervalSeconds)
				{
					taskIntervalSecondsActualMin = taskIntervalInfoByTaskRunDuration.TaskIntervalSeconds;
				}
			}

			return isTaskIntervalInfoExisted
				&& taskIntervalInfosWithoutDurationSecondsMaxCount == 1
				&& taskIntervalSecondsActualMin >= taskIntervalSecondsMin
				&& taskIntervalSecondsActualMin <= taskIntervalSecondsMax;
		}
	}

	extension(IEnumerable<TaskIntervalInfoByTaskRunDuration> taskIntervalInfosByTaskRunDuration)
	{
		public double? GetMinTaskIntervalSeconds()
		{
			double? minIntervalSeconds = null;
			foreach (var taskIntervalInfoByDurationSeconds in taskIntervalInfosByTaskRunDuration)
			{
				if (minIntervalSeconds == null || minIntervalSeconds.Value > taskIntervalInfoByDurationSeconds.TaskIntervalSeconds)
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
