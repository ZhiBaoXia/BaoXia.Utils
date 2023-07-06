using System;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils
{
	public class LoopTask : IDisposable
	{

		////////////////////////////////////////////////
		// @静态常量
		////////////////////////////////////////////////

		#region 静态常量

		public enum LoopTaskState
		{
			/// <summary>
			/// 初始化状态。
			/// </summary>
			Initialized = 0x000000,

			/// <summary>
			/// 任务开始。
			/// </summary>
			Started = 0x000001,

			/// <summary>
			/// 任务开始。
			/// </summary>
			Running = 0x000002,

			/// <summary>
			/// 任务处理中。
			/// </summary>
			RunningAndTaskProcessing = Running | 0x000004,


			/// <summary>
			/// 任务完成。
			/// </summary>
			Stopped = 0x000010,

			/// <summary>
			/// 任务完成，执行成功。
			/// </summary>
			StoppedWithCompleted = Stopped | 0x000020,

			/// <summary>
			/// 任务完成，执行异常。
			/// </summary>
			StoppedWithException = Stopped | 0x000040,

			/// <summary>
			/// 任务完成，任务被取消。
			/// </summary>
			StoppedWithCancel = Stopped | 0x000080
		}


		private static int _countOfLoopTasksProcssingAtSameTime = 0;

		public const int CountOfLoopTasksProcssingAtSameTimeMaxDefault = 0;

		public static int CountOfLoopTasksProcssingAtSameTimeMax { get; set; } = LoopTask.CountOfLoopTasksProcssingAtSameTimeMaxDefault;

		public static double SecondsToSartTaskProcessingByMaxCountOfProcssingTaskAtSameTime { get; set; } = 1.0F;

		#endregion


		////////////////////////////////////////////////
		// @自身属性
		////////////////////////////////////////////////

		#region 自身属性

		public LoopTaskState _objectState = LoopTaskState.Initialized;

		public LoopTaskState _currentStateInTask = LoopTaskState.Initialized;
		public LoopTaskState CurrentState
		{
			get
			{
				return _currentStateInTask;
			}
		}

		protected CancellationTokenSource? _taskCancellationTokenSource;

		protected uint _threadTaskIdSeed;

		public uint ThreadTaskIdSeed
		{
			get
			{
				return _threadTaskIdSeed;
			}
		}

		protected uint _currentThreadTaskId;
		public uint CurrentThreadTaskId
		{
			get
			{
				return _currentThreadTaskId;
			}
		}

		protected Task? _task;

		public Func<CancellationToken, bool>? ToDidProcessTask { get; set; }

		public Func<CancellationToken, Task<bool>>? ToDidProcessTaskAsync { get; set; }



		public Func<double>? ToDidGetIntervalSeconds { get; set; }

		protected double _intervalSeconds = 0.0F;

		public double IntervalSeconds
		{
			get
			{
				double intervalSeconds = _intervalSeconds;
				var didGetIntervalSeconds = this.ToDidGetIntervalSeconds;
				if (intervalSeconds <= 0.0F
					&& didGetIntervalSeconds != null)
				{
					intervalSeconds = didGetIntervalSeconds();
				}
				return intervalSeconds;
			}
			set
			{
				_intervalSeconds = value;
			}
		}


		public Func<DateTime>? ToDidGetStopTime { get; set; }

		protected DateTime _stopTime = DateTime.MaxValue;

		public DateTime StopTime
		{
			get
			{
				DateTime stopTime = _stopTime;
				var didGetStopTime = this.ToDidGetStopTime;
				if (stopTime == DateTime.MaxValue
					&& didGetStopTime != null)
				{
					stopTime = didGetStopTime();
				}
				return stopTime;
			}
			set
			{
				_stopTime = value;
			}
		}


		protected int _processingIndex;

		public int ProcessingIndex { get { return _processingIndex; } }

		public int ProcessedCount { get { return _processingIndex + 1; } }



		#endregion


		////////////////////////////////////////////////
		// @自身实现
		////////////////////////////////////////////////

		#region 自身实现

		public LoopTask(
			Func<CancellationToken, bool> didProcessTask,
			Func<double>? didGetIntervalSeconds,
			bool isAutoRun = true,
			bool isRunImmediately = false)
		{
			this.ToDidProcessTask = didProcessTask;
			this.ToDidGetIntervalSeconds = didGetIntervalSeconds;


			if (isAutoRun)
			{
				this.Start(
					isRunImmediately
					? 0.0
					: this.IntervalSeconds);
			}
		}

		public LoopTask(
			Func<CancellationToken, bool> didProcessTask,
			double intervalSeconds,
			bool isAutoRun = true,
			bool isRunImmediately = false)
		{
			this.ToDidProcessTask = didProcessTask;
			this.IntervalSeconds = intervalSeconds;


			if (isAutoRun)
			{
				this.Start(
					isRunImmediately
					? 0.0
					: this.IntervalSeconds);
			}
		}


		public LoopTask(
			Func<CancellationToken, Task<bool>> didProcessTaskAsync,
			Func<double>? didGetIntervalSeconds,
			bool isAutoRun = true,
			bool isRunImmediately = false)
		{
			this.ToDidProcessTaskAsync = didProcessTaskAsync;
			this.ToDidGetIntervalSeconds = didGetIntervalSeconds;

			if (isAutoRun)
			{
				this.Start(
					isRunImmediately
					? 0.0
					: this.IntervalSeconds);
			}
		}

		public LoopTask(
			Func<CancellationToken, Task<bool>> didProcessTaskAsync,
			double intervalSeconds,
			bool isAutoRun = true,
			bool isRunImmediately = false)
		{
			this.ToDidProcessTaskAsync = didProcessTaskAsync;
			this.IntervalSeconds = intervalSeconds;


			if (isAutoRun)
			{
				this.Start(
					isRunImmediately
					? 0.0
					: this.IntervalSeconds);
			}
		}

		~LoopTask()
		{
			this.Cancel();
		}


		public void Start(double delaySeconds = 0.0)
		{
			lock (this)
			{
				if ((_currentStateInTask & LoopTaskState.Running) != 0
				|| _objectState == LoopTaskState.Started)
				{
					// !!!
					_objectState = LoopTaskState.Started;
					// !!!
					if (this.StopTime <= DateTime.Now)
					{
						this.StopTime = DateTime.MaxValue;
					}
					//⚠ 当前任务线程正在运行，无需额外操作 ⚠
				}
				else
				{
					// !!!
					_objectState = LoopTaskState.Started;
					// !!!
					if (this.StopTime <= DateTime.Now)
					{
						this.StopTime = DateTime.MaxValue;
					}

					// !!!⚠ 逻辑上不存在此种可能，冗余，避免出现野线程 ⚠!!!
					_taskCancellationTokenSource?.Cancel();
					_taskCancellationTokenSource = new CancellationTokenSource();
					var taskCancellationToken = _taskCancellationTokenSource.Token;
					// !!!
					Interlocked.Increment(ref _threadTaskIdSeed);
					if (_threadTaskIdSeed == 0)
					{
						_threadTaskIdSeed = 1;
					}
					_currentThreadTaskId = _threadTaskIdSeed;
					var currentTaskId = _threadTaskIdSeed;
					_task = Task.Run(async () =>
					{
						lock (this)
						{
							if (currentTaskId == _currentThreadTaskId)
							{
								_currentStateInTask = LoopTaskState.Running;
							}
							else
							{
								return;
							}
						}
						try
						{
							if (delaySeconds > 0.0
							&& !taskCancellationToken.IsCancellationRequested)
							{
								await Task.Delay(
									(int)(1000.0 * delaySeconds),
									taskCancellationToken);
							}
						}
						catch (TaskCanceledException)
						{
							lock (this)
							{
								if (currentTaskId == _currentThreadTaskId)
								{
									// !!!
									_currentStateInTask = LoopTaskState.StoppedWithException;
									// !!!
								}
								else
								{
									return;
								}
							}
						}

						lock (this)
						{
							if (currentTaskId == _currentThreadTaskId)
							{
								// !!!⚠ 重置运行次数 ⚠!!!
								_processingIndex = 0;
								// !!!
							}
							else
							{
								return;
							}
						}

						while (true)
						{
							lock (this)
							{
								if (currentTaskId == _currentThreadTaskId)
								{
									if (taskCancellationToken.IsCancellationRequested == true
									|| (_currentStateInTask & LoopTaskState.Stopped) != 0)
									{
										// 如果关闭期间重新标记了任务开始，则继续下一轮任务。
										if (_objectState == LoopTaskState.Started)
										{
											_currentStateInTask = LoopTaskState.Running;
										}
										else
										{
											if ((_currentStateInTask & LoopTaskState.Stopped) == 0)
											{
												_currentStateInTask = LoopTaskState.StoppedWithCancel;
											}

											// !!!⚠ 结束循环 ⚠!!!
											break;
											// !!!⚠ 结束循环 ⚠!!!
										}
									}
								}
								else
								{
									return;
								}
							}


							try
							{
								////////////////////////////////////////////////
								// 全局循环任务同时进行数限制：
								////////////////////////////////////////////////
								if (LoopTask.CountOfLoopTasksProcssingAtSameTimeMax > 0
									&& _countOfLoopTasksProcssingAtSameTime >= LoopTask.CountOfLoopTasksProcssingAtSameTimeMax)
								{
									await Task.Delay(
										(int)(1000.0 * LoopTask.SecondsToSartTaskProcessingByMaxCountOfProcssingTaskAtSameTime),
										taskCancellationToken);
									// !!!
									continue;
									// !!!
								}

								var isNeedProcessTaskContinue = false;
								var didProcessTask = this.ToDidProcessTask;
								var didProcessTaskAsync = this.ToDidProcessTaskAsync;
								{
									// !!!
									Interlocked.Increment(ref _countOfLoopTasksProcssingAtSameTime);
									// !!!

									////////////////////////////////////////////////
									////////////////////////////////////////////////
									////////////////////////////////////////////////
									lock (this)
									{
										if (currentTaskId == _currentThreadTaskId)
										{
											_currentStateInTask = LoopTaskState.RunningAndTaskProcessing;
										}
										else
										{
											return;
										}
									}
									////////////////////////////////////////////////
									if (didProcessTask != null)
									{
										isNeedProcessTaskContinue = didProcessTask(taskCancellationToken);
									}
									else if (didProcessTaskAsync != null)
									{
										isNeedProcessTaskContinue = await didProcessTaskAsync(taskCancellationToken);
									}
									////////////////////////////////////////////////
									lock (this)
									{
										if (currentTaskId == _currentThreadTaskId)
										{
											_currentStateInTask = LoopTaskState.Running;
										}
										else
										{
											return;
										}
									}
									////////////////////////////////////////////////
									////////////////////////////////////////////////
									////////////////////////////////////////////////

									// !!!
									Interlocked.Decrement(ref _countOfLoopTasksProcssingAtSameTime);

									lock (this)
									{
										if (currentTaskId == _currentThreadTaskId)
										{
											Interlocked.Increment(ref _processingIndex);
											if (_objectState == LoopTaskState.Stopped
											|| DateTime.Now >= this.StopTime)
											{
												// !!!
												isNeedProcessTaskContinue = false;
												// !!!
											}
											// !!!
										}
										else
										{
											return;
										}
									}
								}

								////////////////////////////////////////////////
								////////////////////////////////////////////////
								////////////////////////////////////////////////
								// !!!⚠ 关键节点加锁判断目标状态 ⚠!!!
								if (isNeedProcessTaskContinue)
								{
									var taskIntervalSeconds = 0.0;
									lock (this)
									{
										if (currentTaskId == _currentThreadTaskId)
										{
											taskIntervalSeconds = this.IntervalSeconds;
										}
										else
										{
											return;
										}
									}
									if (taskIntervalSeconds > 0.0
									&& !taskCancellationToken.IsCancellationRequested)
									{
										//  !!!
										await Task.Delay(
											(int)(1000 * taskIntervalSeconds),
											taskCancellationToken);
										// !!!
									}
								}
								else
								{
									lock (this)
									{
										if (currentTaskId == _currentThreadTaskId)
										{
											// !!!  
											_currentStateInTask = LoopTaskState.StoppedWithCompleted;
											// !!!
										}
										else
										{
											return;
										}
									}
								}
								////////////////////////////////////////////////
								////////////////////////////////////////////////
								////////////////////////////////////////////////

							}
							catch (System.Threading.Tasks.TaskCanceledException)
							{
								lock (this)
								{
									if (currentTaskId == _currentThreadTaskId)
									{
										// !!!
										_currentStateInTask = LoopTaskState.StoppedWithException;
										// !!!
									}
									else
									{
										return;
									}
								}
							}
						}
					});
				}
			}
		}

		public Task? Cancel()
		{
			Task? task = null;
			lock (this)
			{
				if (_taskCancellationTokenSource != null)
				{
					// ⚠重要
					// 要先重置任务状态，再取消任务运行，
					// 因为某些清空下，“Stop”调用会【同步】触发“await Task.Delay”，
					// 从而错误的在触发了“await Task.Delay”之后【无法重置任务状态】。

					////////////////////////////////////////////////
					// 1/3，记录任务取消令牌源：
					////////////////////////////////////////////////
					var taskCancellationTokenSource = _taskCancellationTokenSource;
					task = _task;

					////////////////////////////////////////////////
					// 2/3，重置任务状态：
					////////////////////////////////////////////////
					_taskCancellationTokenSource = null;
					_currentThreadTaskId = 0;
					_task = null;
					this.StopTime = DateTime.Now;

					////////////////////////////////////////////////
					// 3/3，使用任务取消令牌，取消任务运行：
					////////////////////////////////////////////////
					taskCancellationTokenSource.Cancel();
				}

				// !!!
				_objectState = LoopTaskState.StoppedWithCancel;
				// !!!⚠ 唯一的，非Task中的“_currentStateInTask”改变 ⚠!!!
				// !!!⚠ 正在运行的Task会变为“自由Task”，自然运行结束。 ⚠!!!
				_currentStateInTask = LoopTaskState.StoppedWithCancel;
				// !!!
			}
			return task;
		}

		public async Task CancelAsync()
		{
			var task = this.Cancel();
			if (task != null)
			{
				await task;
			}
		}

		#endregion


		////////////////////////////////////////////////
		// @实现“IDisposable”
		////////////////////////////////////////////////

		#region 实现“IDisposable”

		public void Dispose()
		{
			this.Cancel();

			////////////////////////////////////////////////
			GC.SuppressFinalize(this);
			////////////////////////////////////////////////
		}

		#endregion
	}
}
