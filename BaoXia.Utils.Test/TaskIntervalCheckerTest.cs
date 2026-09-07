using BaoXia.Utils.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Test;

[TestClass]
public class TaskIntervalCheckerTest
{
	private static IEnumerable<TaskIntervalInfoByTaskRunDuration> GetTaskIntervalInfosAfterConcurrentTasksReady(
		CountdownEvent concurrentTasksReady,
		ManualResetEventSlim beginToCheckTaskInterval)
	{
		concurrentTasksReady.Signal();
		if (!beginToCheckTaskInterval.Wait(TimeSpan.FromSeconds(10.0)))
		{
			throw new TimeoutException("等待开始检查任务间隔超时。");
		}
		yield return new TaskIntervalInfoByTaskRunDuration(0.0, 1.0);
	}

	[TestMethod]
	public void IsTaskNeedCheckAtWithConcurrentCallsForSameTaskTest()
	{
		const int concurrentTasksCount = 16;
		const int taskKey = 1;

		var taskIntervalChecker = new TaskIntervalChecker<int, int>(task => task);
		var lastTaskCheckTime = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
		var operateTime = lastTaskCheckTime.AddSeconds(2.0);
		var timeStartTime = lastTaskCheckTime.AddMinutes(-1.0);
		Assert.IsTrue(taskIntervalChecker.IsTaskNeedCheckAt(
			lastTaskCheckTime,
			taskKey,
			timeStartTime,
			[new TaskIntervalInfoByTaskRunDuration(0.0, 1.0)],
			60.0));

		using var concurrentTasksReady = new CountdownEvent(concurrentTasksCount);
		using var beginToCheckTaskInterval = new ManualResetEventSlim(false);
		var tasksToCheckInterval = new Task<bool>[concurrentTasksCount];
		for (var taskIndex = 0; taskIndex < tasksToCheckInterval.Length; taskIndex++)
		{
			tasksToCheckInterval[taskIndex] = Task.Factory.StartNew(() =>
			{
				return taskIntervalChecker.IsTaskNeedCheckAt(
					operateTime,
					taskKey,
					timeStartTime,
					GetTaskIntervalInfosAfterConcurrentTasksReady(concurrentTasksReady, beginToCheckTaskInterval),
					60.0);
			},
			CancellationToken.None,
			TaskCreationOptions.LongRunning,
			TaskScheduler.Default);
		}
		try
		{
			Assert.IsTrue(concurrentTasksReady.Wait(TimeSpan.FromSeconds(10.0)));
		}
		finally
		{
			beginToCheckTaskInterval.Set();
		}
		Assert.IsTrue(Task.WaitAll(tasksToCheckInterval, TimeSpan.FromSeconds(10.0)));

		Assert.AreEqual(1, tasksToCheckInterval.Count(task => task.Result));
		Assert.AreEqual(operateTime, taskIntervalChecker.ItemAutoCheckTimes[taskKey]);
	}
}
