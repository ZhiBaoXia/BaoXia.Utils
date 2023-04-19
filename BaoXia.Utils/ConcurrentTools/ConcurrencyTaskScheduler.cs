using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.ConcurrentTools
{
        /// <summary>
        /// 并发任务调度器。
        /// </summary>
        public class ConcurrencyTaskScheduler : System.Threading.Tasks.TaskScheduler
        {

                ////////////////////////////////////////////////
                // @静态变量
                ////////////////////////////////////////////////

                #region 静态变量

                /// <summary>
                /// 每个工作线程都有一个属于自己的标志：是否为“任务工作线程”。
                /// </summary>
                [ThreadStatic]
                private static bool _isCurrentThreadIsTaskWorkerThread;

                #endregion


                ////////////////////////////////////////////////
                // @自身属性
                ////////////////////////////////////////////////

                #region @自身属性

                protected List<Task> _allTasks = new();

                protected List<Task> _taskQueue = new();

                public Task[] AllTasks
                {
                        get
                        {
                                Task[] allTasks;
                                lock (_allTasks)
                                {
                                        allTasks = _allTasks.ToArray();
                                }
                                return allTasks;
                        }
                }

                protected int _runningTasksCount = 0;

                public int RunningTasksCOunt => _runningTasksCount;

                protected int _runningThreadsCount = 0;

                public int RunningThreadsCount => _runningThreadsCount;
                public Func<int>? ToGetConcurrencyTasksCountMax { get; set; }

                protected int _concurrencyTasksCountMax = 0;
                public int ConcurrencyTasksCountMax
                {
                        get
                        {
                                var toGetConcurrencyTasksCountMax = ToGetConcurrencyTasksCountMax;
                                if (toGetConcurrencyTasksCountMax != null)
                                {
                                        return toGetConcurrencyTasksCountMax();
                                }
                                return _concurrencyTasksCountMax;
                        }

                        set
                        {
                                _concurrencyTasksCountMax = value;
                        }
                }

                public bool IsWorkerThreadIdle
                {
                        get
                        {
                                var concurrencyTasksCountMax = this.ConcurrencyTasksCountMax;
                                if (concurrencyTasksCountMax <= 0
                                    || _runningTasksCount < concurrencyTasksCountMax)
                                {
                                        return true;
                                }
                                return false;
                        }
                }

                #endregion


                ////////////////////////////////////////////////
                // @自身实现
                ////////////////////////////////////////////////

                #region @自身实现

                public ConcurrencyTaskScheduler()
                { }

                public ConcurrencyTaskScheduler(Func<int>? toGetConcurrencyTasksCountMax)
                {
                        ToGetConcurrencyTasksCountMax = toGetConcurrencyTasksCountMax;
                }

                public ConcurrencyTaskScheduler(int concurrencyTasksCountMax)
                {
                        ConcurrencyTasksCountMax = concurrencyTasksCountMax;
                }

                #endregion


                ////////////////////////////////////////////////
                // @实现“TaskScheduler”接口
                ////////////////////////////////////////////////

                #region @实现“TaskScheduler”接口

                public sealed override int MaximumConcurrencyLevel
                {
                        get
                        {
                                return ConcurrencyTasksCountMax;
                        }
                }

                protected sealed override void QueueTask(Task task)
                {
                        lock (_allTasks)
                        {
                                _allTasks.Add(task);
                        }
                        lock (_taskQueue)
                        {
                                _taskQueue.Add(task);

                                var maxConcurrencyTasksCount = ConcurrencyTasksCountMax;
                                if (maxConcurrencyTasksCount > 0
                                    && _runningTasksCount >= maxConcurrencyTasksCount)
                                {
                                        return;
                                }

                                // !!! ⚠️
                                _runningTasksCount++;
                                // !!! ⚠️

                                ThreadPool.UnsafeQueueUserWorkItem(_ =>
                                {
                                        try
                                        {
                                                _isCurrentThreadIsTaskWorkerThread = true;
                                                Interlocked.Increment(ref _runningThreadsCount);

                                                while (true)
                                                {
                                                        Task? taskNeedExecuted = null;
                                                        lock (_taskQueue)
                                                        {
                                                                if (_taskQueue.Any())
                                                                {
                                                                        taskNeedExecuted = _taskQueue[0];
                                                                        _taskQueue.RemoveAt(0);
                                                                }
                                                                else
                                                                {
                                                                        _runningTasksCount--;
                                                                        break;
                                                                }
                                                        }
                                                        ////////////////////////////////////////////////
                                                        if (taskNeedExecuted != null)
                                                        {
                                                                ////////////////////////////////////////////////
                                                                // !!!
                                                                base.TryExecuteTask(taskNeedExecuted);
                                                                // !!!
                                                                ////////////////////////////////////////////////
                                                                lock (_allTasks)
                                                                {
                                                                        _allTasks.RemoveAll(taskExisted => taskExisted.Equals(taskNeedExecuted));
                                                                }
                                                        }
                                                        ////////////////////////////////////////////////
                                                }
                                        }
                                        finally
                                        {
                                                _isCurrentThreadIsTaskWorkerThread = false;
                                                Interlocked.Decrement(ref _runningThreadsCount);
                                        }
                                },
                                null);
                        }
                }

                protected sealed override bool TryDequeue(Task task)
                {
                        lock (_taskQueue)
                        {
                                if (_taskQueue.Remove(task))
                                {
                                        return true;
                                }
                        }
                        return false;
                }

                protected sealed override bool TryExecuteTaskInline(
                    Task task,
                    bool taskWasPreviouslyQueued)
                {
                        // !!!⚠ 如果在“Task”任务中，开启了同一调度器的“新Task”，    ⚠!!!
                        // !!!⚠ 则调用此回调，尝试在同一线程中【继续】执行“Task”。⚠!!!
                        if (_isCurrentThreadIsTaskWorkerThread != true)
                        {
                                return false;
                        }

                        if (taskWasPreviouslyQueued)
                        {
                                if (!this.TryDequeue(task))
                                {
                                        return false;
                                }
                        }
                        else
                        {
                                lock (_allTasks)
                                {
                                        _allTasks.Add(task);
                                }
                        }

                        ////////////////////////////////////////////////
                        // !!!
                        var tryExecuteResult = base.TryExecuteTask(task);
                        // !!!
                        ////////////////////////////////////////////////
                        lock (_allTasks)
                        {
                                _allTasks.RemoveAll(taskExisted => taskExisted.Equals(task));
                        }
                        return tryExecuteResult;
                }

                protected sealed override IEnumerable<Task> GetScheduledTasks()
                {
                        lock (_taskQueue)
                        {
                                return _taskQueue.ToArray();
                        }
                }

                #endregion
        }
}
