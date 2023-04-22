
#nullable enable

using System;
using System.Threading;
using System.Threading.Tasks;


namespace BaoXia.Utils.ConcurrentTools
{
        /// <summary>
        /// ⚠【注意】，Tasks的内部实现，均使用“Task.Run”来开启新的任务，⚠
        /// ⚠ 避免了“new Task” 不可等待的“错误”情况。 ⚠
        /// </summary>
        public class Tasks : IDisposable
        {
                ////////////////////////////////////////////////
                // @自身属性
                ////////////////////////////////////////////////

                #region 自身属性

                protected ConcurrencyTaskScheduler _taskScheduler;

                public ConcurrencyTaskScheduler TaskScheduler => _taskScheduler;

                public Func<int>? ToGetConcurrentTasksCountMax
                {
                        get
                        {
                                return _taskScheduler.ToGetConcurrencyTasksCountMax;
                        }
                        set
                        {
                                _taskScheduler.ToGetConcurrencyTasksCountMax = value;
                        }
                }

                public int ConcurrencyTasksCountMax
                {
                        get
                        {
                                return _taskScheduler.ConcurrencyTasksCountMax;
                        }
                        set
                        {
                                _taskScheduler.ConcurrencyTasksCountMax = value;
                        }
                }

                public bool IsWorkerThreadIdle => _taskScheduler.IsWorkerThreadIdle;

                protected TaskFactory _taskFactory;

                protected CancellationTokenSource _cancellationTokenSource = new();

                public CancellationToken CancellationToken => _cancellationTokenSource.Token;

                #endregion


                ////////////////////////////////////////////////
                // @自身实现
                ////////////////////////////////////////////////

                #region 自身实现

                public Tasks(
                        Func<int>? toGetConcurrentTasksCountMax,
                        TaskCreationOptions taskCreationOptions = TaskCreationOptions.LongRunning,
                        TaskContinuationOptions taskContinuationOptions = TaskContinuationOptions.None)
                {
                        _taskScheduler = new ConcurrencyTaskScheduler(toGetConcurrentTasksCountMax);
                        _taskFactory = new TaskFactory(
                                _cancellationTokenSource.Token,
                                taskCreationOptions,
                                taskContinuationOptions,
                                _taskScheduler);
                }

                public Tasks(
                        int concurrentTasksCountMax,
                        TaskCreationOptions taskCreationOptions = TaskCreationOptions.None,
                        TaskContinuationOptions taskContinuationOptions = TaskContinuationOptions.None)
                {
                        _taskScheduler = new ConcurrencyTaskScheduler(concurrentTasksCountMax);
                        _taskFactory = new TaskFactory(
                                _cancellationTokenSource.Token,
                                taskCreationOptions,
                                taskContinuationOptions,
                                _taskScheduler);
                }

                ////////////////////////////////////////////////

                ////////////////////////////////////////////////

                public async Task<TaskResultType?> RunAsync<TaskResultType>(
                        Func<Task<TaskResultType?>> function)
                {
                        var taskResult = await _taskFactory.StartNew(function);
                        { }
                        return await taskResult;
                }

                public async Task RunAsync(Func<Task?> function)
                {
                        await _taskFactory.StartNew(function);
                }

                public async Task RunAsync(Action action)
                {
                        await _taskFactory.StartNew(() =>
                        {
                                action.Invoke();
                        });
                }


                ////////////////////////////////////////////////


                public Task<Task<TaskResultType?>>? TryRun<TaskResultType>(
                        Func<Task<TaskResultType?>> function)
                {
                        if (IsWorkerThreadIdle)
                        {
                                return _taskFactory.StartNew(function);
                        }
                        return null;
                }

                public Task? TryRun(Func<Task> function)
                {
                        if (IsWorkerThreadIdle)
                        {
                                return _taskFactory.StartNew(function);
                        }
                        return null;
                }

                public Task? TryRun(Action action)
                {
                        if (IsWorkerThreadIdle)
                        {
                                return _taskFactory.StartNew(action);
                        }
                        return null;
                }

                ////////////////////////////////////////////////

                public void WaitAll()
                {
                        var allTask = _taskScheduler.AllTasks;
                        if (allTask.Length > 0)
                        {
                                Task.WaitAll(allTask);
                        }
                }

                public void WaitAny()
                {
                        var allTask = _taskScheduler.AllTasks;
                        if (allTask.Length > 0)
                        {
                                Task.WaitAny(allTask);
                        }
                }

                public async Task WhenAll()
                {
                        var allTask = _taskScheduler.AllTasks;
                        if (allTask.Length > 0)
                        {
                                await Task.WhenAll(allTask);
                        }
                }

                public async Task WhenAny()
                {
                        var allTask = _taskScheduler.AllTasks;
                        if (allTask.Length > 0)
                        {
                                await Task.WhenAny(allTask);
                        }
                }

                public void Cancel()
                {
                        _cancellationTokenSource.Cancel();
                }

                public void CancelAndWaitAll()
                {
                        _cancellationTokenSource.Cancel();
                        this.WaitAll();
                }

                public async Task CancelAndWhenAll()
                {
                        _cancellationTokenSource.Cancel();
                        await this.WhenAll();
                }

                #endregion


                ////////////////////////////////////////////////
                // @实现“IDisposable”
                ////////////////////////////////////////////////

                #region 实现“IDisposable”

                public void Dispose()
                {
                        GC.SuppressFinalize(this);

                        this.Cancel();
                }

                #endregion
        }
}
