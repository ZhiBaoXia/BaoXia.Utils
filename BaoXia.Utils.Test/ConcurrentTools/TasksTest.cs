using BaoXia.Utils.ConcurrentTools;
using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Test.ConcurrentTools
{
        [TestClass]
        public class TasksTest
        {
                [TestMethod]
                public void ConcurrentRunMaxCountTest()
                {
                        var testTasksCount = 1000;
                        var testTasksCountRunning = 0;
                        var testTaskThreadsCountRunningMax = 10;
                        var tasks = new Tasks(testTaskThreadsCountRunningMax);
                        var testTaskList = new List<Task>();
                        var logsToCurrentTasksCount = new ConcurrentQueue<string>();
                        var logsToCompletedTasksCount = new ConcurrentQueue<string>();
                        var threadIds = new ConcurrentDictionary<int, bool>();
                        for (var testTaskIndex = 0;
                                testTaskIndex < testTasksCount;
                                testTaskIndex++)
                        {
                                var currentTestTaskId = testTaskIndex + 1;
                                testTaskList.Add(Task.Run(async () =>
                                {
                                        await tasks.RunAsync(() =>
                                        {
                                                // !!!
                                                threadIds.AddOrUpdateWithNewValue(
                                                        System.Environment.CurrentManagedThreadId,
                                                        true);
                                                // !!!


                                                Interlocked.Increment(ref testTasksCountRunning);
                                                logsToCurrentTasksCount.Enqueue("#ConcurrentRunMaxCountTest#，当前运行任务数量：" + testTasksCountRunning);
                                                {
                                                        // !!! 【同步】任务，【不会】在“Sleep”时再利用线程开启下个任务， !!!
                                                        // !!! 因此“testTasksCountRunning”一定小于等于“testTaskThreadsCountRunningMax”。 !!!
                                                        Assert.IsTrue(testTasksCountRunning <= testTaskThreadsCountRunningMax);
                                                        // !!!
                                                        {
                                                                System.Threading.Thread.Sleep(1);
                                                        }
                                                }
                                                Interlocked.Decrement(ref testTasksCountRunning);
                                                logsToCompletedTasksCount.Enqueue("#ConcurrentRunMaxCountTest#，已完成任务：" + currentTestTaskId);
                                        });
                                }));
                        }
                        // !!!
                        Task.WaitAll(testTaskList.ToArray());
                        // !!!

                        foreach (var log in logsToCompletedTasksCount)
                        {
                                System.Diagnostics.Debug.WriteLine(log);
                        }
                        foreach (var log in logsToCurrentTasksCount)
                        {
                                System.Diagnostics.Debug.WriteLine(log);
                        }
                }

                [TestMethod]
                public void ConcurrentRunMaxCountAsyncTest()
                {
                        var testTasksCount = 1000;
                        var testTasksCountRunning = 0;
                        // !!!
                        var testTaskThreadsCountRunningMax = 20;
                        var tasks = new Tasks(testTaskThreadsCountRunningMax);
                        // !!!
                        var testTaskList = new List<Task>();
                        var isTestTasksCountRunningHadGreaterThanTestTasksCountRunningMax = false;
                        var logsToCurrentTasksCount = new ConcurrentQueue<string>();
                        var logsToCompletedTasksCount = new ConcurrentQueue<string>();
                        var threadIds = new ConcurrentDictionary<int, bool>();
                        for (var testTaskIndex = 0;
                                testTaskIndex < testTasksCount;
                                testTaskIndex++)
                        {
                                var currentTestTaskId = testTaskIndex + 1;
                                testTaskList.Add(Task.Run(async () =>
                                {
                                        var taskResult = await tasks.RunAsync(async () =>
                                        {
                                                // !!!
                                                threadIds.AddOrUpdateWithNewValue(
                                                        System.Environment.CurrentManagedThreadId,
                                                        true);
                                                // !!!


                                                Interlocked.Increment(ref testTasksCountRunning);
                                                if (testTasksCountRunning > testTaskThreadsCountRunningMax)
                                                {
                                                        isTestTasksCountRunningHadGreaterThanTestTasksCountRunningMax = true;
                                                }


                                                logsToCurrentTasksCount.Enqueue("#ConcurrentRunMaxCountAsyncTest#，当前运行任务数量：" + testTasksCountRunning);

                                                Assert.IsTrue(tasks.TaskScheduler.RunningThreadsCount <= testTaskThreadsCountRunningMax);
                                                // !!!
                                                {
                                                        await Task.Delay(1);
                                                }

                                                Interlocked.Decrement(ref testTasksCountRunning);
                                                logsToCompletedTasksCount.Enqueue("#ConcurrentRunMaxCountAsyncTest#，已完成任务：" + currentTestTaskId);

                                                return currentTestTaskId;
                                        });
                                        // !!!
                                        //Assert.IsTrue(taskResult == currentTestTaskId);
                                        // !!!
                                }));
                        }
                        // !!!
                        Task.WaitAll(testTaskList.ToArray());
                        // !!!



                        // !!! 【异步】任务，【会】在“Dealy”时再利用线程开启下个任务， !!!
                        // !!! 因此“testTasksCountRunning”一定大于等于“testTaskThreadsCountRunningMax”。 !!!
                        Assert.IsTrue(isTestTasksCountRunningHadGreaterThanTestTasksCountRunningMax);

                        foreach (var log in logsToCompletedTasksCount)
                        {
                                System.Diagnostics.Debug.WriteLine(log);
                        }
                        foreach (var log in logsToCurrentTasksCount)
                        {
                                System.Diagnostics.Debug.WriteLine(log);
                        }
                }


                [TestMethod]
                public async Task TryRunTest()
                {
                        var testDurationSeconds = 1.0;
                        var testEndTime = DateTime.Now.AddSeconds(testDurationSeconds);
                        var testIntervalSeconds = 0.01;

                        var maxThreadsCount = 10;
                        var tasks = new Tasks(maxThreadsCount);
                        var currentThreadId = System.Environment.CurrentManagedThreadId;

                        var testTasks = new List<Task>();
                        while (DateTime.Now < testEndTime)
                        {
                                testTasks.Add(Task.Run(() =>
                                {
                                        var stepNumber = 1;
                                        var stepNumberLocker = new object();
                                        ////////////////////////////////////////////////
                                        // !!! 此处测试的目的在于”TryRun“不会卡住当前线程的继续进行。 !!!
                                        ////////////////////////////////////////////////
                                        _ = tasks.TryRun(() =>
                                        {
                                                // !!! 模拟消耗时间的操作。 !!!
                                                var endTime = DateTime.Now.AddSeconds(0.3);
                                                while (DateTime.Now < endTime)
                                                { }
                                                // !!!
                                                lock (stepNumberLocker)
                                                {
                                                        Assert.IsTrue(stepNumber == 2);
                                                        stepNumber = 3;
                                                }
                                        });
                                        lock (stepNumberLocker)
                                        {
                                                stepNumber = 2;
                                        }
                                }));

                                if (testIntervalSeconds > 0)
                                {
                                        await Task.Delay((int)(1000 * testIntervalSeconds));
                                }
                        }

                        // !!!
                        Task.WaitAll(testTasks.ToArray());
                        // !!!

                }
        }
}
