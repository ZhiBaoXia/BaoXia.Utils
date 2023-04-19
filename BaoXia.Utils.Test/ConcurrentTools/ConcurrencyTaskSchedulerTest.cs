using BaoXia.Utils.ConcurrentTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Test.ConcurrentTools
{
        [TestClass]
        public class ConcurrencyTaskSchedulerTest
        {
                [TestMethod]
                public void ConcurrencyControllTest()
                {
                        var concurrencyCountMax = 5;
                        var testsCount = 1000;
                        var taskScheduler = new ConcurrencyTaskScheduler(concurrencyCountMax);
                        var tasks = new List<Task>();

                        var taskFactory = new TaskFactory(taskScheduler);
                        var cancellationTokenSource = new CancellationTokenSource();
                        var allThreadIds = new List<int>();

                        var locker = new Object();
                        int outputNumber = 0;


                        for (int taskId = 1;
                                taskId <= 5;
                                taskId++)
                        {
                                int currentTaskId = taskId;
                                Task task = taskFactory.StartNew(() =>
                                {
                                        for (int testIndex = 0;
                                                testIndex < testsCount;
                                                testIndex++)
                                        {
                                                lock (locker)
                                                {
                                                        var threadId = Thread.CurrentThread.ManagedThreadId;
                                                        lock (allThreadIds)
                                                        {
                                                                allThreadIds.Add(threadId);
                                                        }

                                                        //Console.Write("{0} in taskId({1}) on threadId({2})",
                                                        //testIndex,
                                                        //currentTaskId,
                                                        //threadId);
                                                        //outputNumber++;
                                                        //if (outputNumber % 3 == 0)
                                                        //{
                                                        //        Console.WriteLine();
                                                        //}
                                                }
                                        }
                                },
                                        cancellationTokenSource.Token);
                                tasks.Add(task);
                        }

                        // Use it to run a second set of tasks.
                        for (int taskId = 1;
                                taskId <= 5;
                                taskId++)
                        {
                                int currentTaskId = taskId;
                                Task task = taskFactory.StartNew(() =>
                                {
                                        for (int testIndex = 0;
                                        testIndex <= 10;
                                        testIndex++)
                                        {
                                                for (int character = 0x21;
                                                character <= 0x7E;
                                                character++)
                                                {
                                                        lock (locker)
                                                        {
                                                                var threadId = Thread.CurrentThread.ManagedThreadId;
                                                                lock (allThreadIds)
                                                                {
                                                                        allThreadIds.Add(threadId);
                                                                }

                                                                //Console.Write(
                                                                //"'{0}' in task takId({1}) on threadId({2})",
                                                                //Convert.ToChar(character),
                                                                //currentTaskId,
                                                                //threadId);

                                                                outputNumber++;
                                                                //if (outputNumber % 3 == 0)
                                                                //{
                                                                //        Console.WriteLine();
                                                                //}
                                                        }
                                                }
                                        }
                                },
                                cancellationTokenSource.Token);
                                tasks.Add(task);
                        }

                        // Wait for the tasks to complete before displaying a completion message.
                        Task.WaitAll(tasks.ToArray());
                        cancellationTokenSource.Dispose();

                        for (var threadIdIndex = allThreadIds.Count - 1;
                                threadIdIndex >= 0;
                                threadIdIndex--)
                        {
                                var threadId = allThreadIds[threadIdIndex];
                                for (var prevThreadIdIndex = threadIdIndex - 1;
                                        prevThreadIdIndex >= 0;
                                        prevThreadIdIndex--)
                                {
                                        if (allThreadIds[prevThreadIdIndex] == threadId)
                                        {
                                                allThreadIds.RemoveAt(threadIdIndex);
                                                break;
                                        }
                                }
                        }

                        //Console.WriteLine("\r\n测试完成，全部线程Id为：");
                        //foreach (var threadId in allThreadIds)
                        //{
                        //        Console.WriteLine(threadId.ToString());
                        //}
                        //Console.ReadLine();

                        // !!!
                        Assert.IsTrue(allThreadIds.Count <= concurrencyCountMax);
                        // !!!
                        System.Diagnostics.Debug.WriteLine(
                                $"#ConcurrencyControllTest#，限制任务线程数： {allThreadIds.Count}/{concurrencyCountMax} 。");
                }
        }
}
