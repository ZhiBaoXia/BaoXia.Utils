using BaoXia.Utils.Extensions;
using BaoXia.Utils.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace BaoXia.Utils;

public class TaskIntervalChecker<TTask, TTaskKey>(Func<TTask, TTaskKey> toGetKeyOfTask)
	where TTaskKey : notnull
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public readonly ConcurrentDictionary<TTaskKey, DateTimeOffset> ItemAutoCheckTimes = [];

	public Func<TTask, TTaskKey> ToGetKeyOfTask { get; set; } = toGetKeyOfTask;

	#endregion


	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public bool IsTaskNeedCheckAt(DateTimeOffset operateTime, TTaskKey taskKey, DateTimeOffset timeStartTime,
		IEnumerable<TaskIntervalInfoByTaskRunDuration> taskIntervalInfosByTaskRunDuration,
		double defaultTaskCheckIntervalSecondsMin,
		bool isTaskIntervalInfosNeedResort = false)
	{
		if (ItemAutoCheckTimes.TryGetValue(taskKey, out var lastTaskCheckTime))
		{
			var taskRunDuration = operateTime - timeStartTime;
			var taskCheckIntervalSecondsMin = taskIntervalInfosByTaskRunDuration.GetTaskIntervalSecondsWithTaskRunDuration(
				taskRunDuration, isTaskIntervalInfosNeedResort);
			var taskCheckIntervalSeconds = operateTime - lastTaskCheckTime;
			taskCheckIntervalSecondsMin ??= defaultTaskCheckIntervalSecondsMin;
			if (taskCheckIntervalSeconds.TotalSeconds < taskCheckIntervalSecondsMin)
			{
				return false;
			}
		}
		// !!!
		ItemAutoCheckTimes.AddOrUpdate(taskKey, operateTime, (paymentOrderId, lastPaymentOrderNeedUpdate) =>
		{
			return operateTime;
		});
		// !!!
		return true;
	}

	public void CleanTaskCheckTimesWithTaskKeys(IEnumerable<TTaskKey> taskKeysNeedCheck)
	{
		var invalidPaymentIdsToPaymentOrderAutoCheckTimes
			= ItemAutoCheckTimes.Keys.Except(taskKeysNeedCheck);
		if (invalidPaymentIdsToPaymentOrderAutoCheckTimes?.IsNotEmpty() == true)
		{
			foreach (var invalidPaymentId in invalidPaymentIdsToPaymentOrderAutoCheckTimes)
			{
				//
				ItemAutoCheckTimes.Remove(invalidPaymentId, out _);
				//
			}
		}
	}

	public void CleanTaskCheckTimesWithTasks(IEnumerable<TTask> tasksNeedCheck)
	{
		var taskKeys = new HashSet<TTaskKey>();
		foreach (var taskNeedCheck in tasksNeedCheck)
		{
			var taskKey = ToGetKeyOfTask(taskNeedCheck);
			//
			taskKeys.Add(taskKey);
			//
		}
		//
		CleanTaskCheckTimesWithTaskKeys(taskKeys);
		//
	}

	#endregion
}