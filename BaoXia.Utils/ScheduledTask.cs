using BaoXia.Utils.Constants;
using BaoXia.Utils.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils
{
        public class ScheduledTask
        {
                ////////////////////////////////////////////////
                // @静态常量
                ////////////////////////////////////////////////

                #region 静态常量

                public const double TimerAccuracySecondsDefault = 1.0;

                #endregion



                ////////////////////////////////////////////////
                // @自身属性
                ////////////////////////////////////////////////

                #region 自身属性

                public Func<CancellationToken, bool>? ToProcessTask { get; set; }

                public Func<CancellationToken, Task<bool>>? ToProcessTaskAsync { get; set; }

                public Func<ScheduledTaskConfig?> ToGetScheduledTaskConfig { get; set; }

#pragma warning disable IDE0052 // 删除未读的私有成员
                private readonly LoopTask _taskToCheckTimeToStartOrStopTask;
#pragma warning restore IDE0052 // 删除未读的私有成员

                public CancellationTokenSource? LastScheduledTaskCancellationTokenSource { get; private set; }
                public DateTime LastScheduledTaskBeginTime { get; private set; }
                public DateTime LastScheduledTaskEndTime { get; private set; }
                public bool IsInScheduledTaskProcessing
                {
                        get
                        {
                                var now = DateTime.Now;
                                if (now >= LastScheduledTaskBeginTime
                                        && now < LastScheduledTaskEndTime)
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

                #region 自身实现

                public ScheduledTask(
                                Func<CancellationToken, bool> toProcessTask,
                                Func<ScheduledTaskConfig?> toGetScheduledTaskConfig,
                                bool isAutoRun = true,
                                bool isRunImmediately = false,
                               double timerAccuracySeconds = TimerAccuracySecondsDefault)
                {
                        ToProcessTask = toProcessTask;

                        ToGetScheduledTaskConfig = toGetScheduledTaskConfig;

                        _taskToCheckTimeToStartOrStopTask
                                = new(
                                        DidCheckTimeToStartTask,
                                        timerAccuracySeconds,
                                        isAutoRun,
                                        isRunImmediately);
                }

                public ScheduledTask(
                                Func<CancellationToken, Task<bool>> toProcessTaskAsync,
                                Func<ScheduledTaskConfig?> toGetScheduledTaskConfig,
                                bool isAutoRun = true,
                                bool isRunImmediately = false,
                               double timerAccuracySeconds = TimerAccuracySecondsDefault)
                {
                        ToProcessTaskAsync = toProcessTaskAsync;

                        ToGetScheduledTaskConfig = toGetScheduledTaskConfig;

                        _taskToCheckTimeToStartOrStopTask
                                = new(
                                        DidCheckTimeToStartTaskAsync,
                                        timerAccuracySeconds,
                                        isAutoRun,
                                        isRunImmediately);
                }

                #endregion


                ////////////////////////////////////////////////
                // @事件节点，同步任务
                ////////////////////////////////////////////////

                #region 事件节点，同步任务

                protected bool DidCheckTimeToStartTask(
                        CancellationToken cancellationToken)
                {
                        if (ToProcessTask is not Func<CancellationToken, bool> toProcessTask)
                        {
                                return true;
                        }
                        var scheduledTaskConfig = ToGetScheduledTaskConfig?.Invoke();
                        if (scheduledTaskConfig == null)
                        {
                                return true;
                        }
                        var now = DateTime.Now;
                        if (scheduledTaskConfig.TimeSection != null
                                && !scheduledTaskConfig.TimeSection.IsTimeInSection(now))
                        {
                                return true;
                        }

                        LastScheduledTaskCancellationTokenSource
                                ??= CancellationTokenSource
                                .CreateLinkedTokenSource(cancellationToken);
                        switch (scheduledTaskConfig.ScheduledType)
                        {
                                case ScheduledType.FixedTaskStartIntervalSeconds:
                                        {
                                                if ((now - LastScheduledTaskBeginTime).TotalSeconds
                                                        >= scheduledTaskConfig.TaskIntervalSeconds)
                                                {
                                                        DidProcessTaskInFixedTaskStartIntervalSeconds(
                                                                toProcessTask,
                                                                cancellationToken);
                                                }
                                        }
                                        break;
                                default:
                                case ScheduledType.FixedTaskIntervalSeconds:
                                        {
                                                if ((now - LastScheduledTaskEndTime).TotalSeconds
                                                        >= scheduledTaskConfig.TaskIntervalSeconds)
                                                {
                                                        DidProcessTaskInFixedTaskIntervalSeconds(
                                                                toProcessTask,
                                                                cancellationToken);
                                                }
                                        }
                                        break;
                                case ScheduledType.FixedTimeSection:
                                        {
                                                DidProcessTaskInFixedTimeSection(
                                                                toProcessTask,
                                                                cancellationToken);
                                        }
                                        break;
                        }
                        return true;
                }

                protected void DidProcessTaskInFixedTaskStartIntervalSeconds(
                        Func<CancellationToken, bool> toProcessTask,
                        CancellationToken cancellationToken)
                {
                        // !!!
                        LastScheduledTaskBeginTime = DateTime.Now;
                        // !!!
                        Task.Run(() =>
                        {
                                try
                                {
                                        ////////////////////////////////////////////////
                                        toProcessTask(cancellationToken);
                                        ////////////////////////////////////////////////
                                }
                                finally
                                {
                                        // !!!
                                        LastScheduledTaskEndTime = DateTime.Now;
                                        // !!!
                                }
                        },
                        cancellationToken);
                }

                protected void DidProcessTaskInFixedTaskIntervalSeconds(
                        Func<CancellationToken, bool> toProcessTask,
                        CancellationToken cancellationToken)
                {
                        // !!!
                        LastScheduledTaskBeginTime = DateTime.Now;
                        // !!!
                        try
                        {
                                ////////////////////////////////////////////////
                                toProcessTask(cancellationToken);
                                ////////////////////////////////////////////////
                        }
                        finally
                        {
                                // !!!
                                LastScheduledTaskEndTime = DateTime.Now;
                                // !!!
                        }
                }

                protected void DidProcessTaskInFixedTimeSection(
                        Func<CancellationToken, bool> toProcessTask,
                        CancellationToken cancellationToken)
                {
                        // !!!
                        LastScheduledTaskBeginTime = DateTime.Now;
                        // !!!
                        try
                        {
                                ////////////////////////////////////////////////
                                toProcessTask(cancellationToken);
                                ////////////////////////////////////////////////
                        }
                        finally
                        {
                                // !!!
                                LastScheduledTaskEndTime = DateTime.Now;
                                // !!!
                        }
                }

                #endregion


                ////////////////////////////////////////////////
                // @事件节点，异步任务
                ////////////////////////////////////////////////

                #region 事件节点，异步任务

                protected async Task<bool> DidCheckTimeToStartTaskAsync(
                        CancellationToken cancellationToken)
                {
                        if (ToProcessTaskAsync
                                is not Func<CancellationToken, Task<bool>> toProcessTaskAsync)
                        {
                                return true;
                        }
                        var scheduledTaskConfig = ToGetScheduledTaskConfig?.Invoke();
                        if (scheduledTaskConfig == null)
                        {
                                return true;
                        }
                        var now = DateTime.Now;
                        if (scheduledTaskConfig.TimeSection != null
                                && !scheduledTaskConfig.TimeSection.IsTimeInSection(now))
                        {
                                return true;
                        }

                        LastScheduledTaskCancellationTokenSource
                                ??= CancellationTokenSource
                                .CreateLinkedTokenSource(cancellationToken);
                        switch (scheduledTaskConfig.ScheduledType)
                        {
                                case ScheduledType.FixedTaskStartIntervalSeconds:
                                        {
                                                if ((now - LastScheduledTaskBeginTime).TotalSeconds
                                                        >= scheduledTaskConfig.TaskIntervalSeconds)
                                                {
                                                        DidProcessTaskInFixedTaskStartIntervalSecondsAsync(
                                                                toProcessTaskAsync,
                                                                cancellationToken);
                                                }
                                        }
                                        break;
                                default:
                                case ScheduledType.FixedTaskIntervalSeconds:
                                        {
                                                if ((now - LastScheduledTaskEndTime).TotalSeconds
                                                        >= scheduledTaskConfig.TaskIntervalSeconds)
                                                {
                                                        await DidProcessTaskInFixedTaskIntervalSecondsAsync(
                                                                toProcessTaskAsync,
                                                                cancellationToken);
                                                }
                                        }
                                        break;
                                case ScheduledType.FixedTimeSection:
                                        {
                                                await DidProcessTaskInFixedTimeSectionAsync(
                                                                toProcessTaskAsync,
                                                                cancellationToken);
                                        }
                                        break;
                        }
                        return true;
                }

                protected void DidProcessTaskInFixedTaskStartIntervalSecondsAsync(
                        Func<CancellationToken, Task<bool>> toProcessTaskAsync,
                        CancellationToken cancellationToken)
                {
                        // !!!
                        LastScheduledTaskBeginTime = DateTime.Now;
                        // !!!
                        Task.Run(async () =>
                        {
                                try
                                {
                                        ////////////////////////////////////////////////
                                        await toProcessTaskAsync(cancellationToken);
                                        ////////////////////////////////////////////////
                                }
                                finally
                                {
                                        // !!!
                                        LastScheduledTaskEndTime = DateTime.Now;
                                        // !!!
                                }
                        },
                        cancellationToken);
                }

                protected async Task DidProcessTaskInFixedTaskIntervalSecondsAsync(
                        Func<CancellationToken, Task<bool>> toProcessTaskAsync,
                        CancellationToken cancellationToken)
                {
                        // !!!
                        LastScheduledTaskBeginTime = DateTime.Now;
                        // !!!
                        try
                        {
                                ////////////////////////////////////////////////
                                await toProcessTaskAsync(cancellationToken);
                                ////////////////////////////////////////////////
                        }
                        finally
                        {
                                // !!!
                                LastScheduledTaskEndTime = DateTime.Now;
                                // !!!
                        }
                }

                protected async Task DidProcessTaskInFixedTimeSectionAsync(
                        Func<CancellationToken, Task<bool>> toProcessTaskAsync,
                        CancellationToken cancellationToken)
                {
                        // !!!
                        LastScheduledTaskBeginTime = DateTime.Now;
                        // !!!
                        try
                        {
                                ////////////////////////////////////////////////
                                await toProcessTaskAsync(cancellationToken);
                                ////////////////////////////////////////////////
                        }
                        finally
                        {
                                // !!!
                                LastScheduledTaskEndTime = DateTime.Now;
                                // !!!
                        }
                }

                #endregion
        }
}
