using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.ConcurrentTools;

public class ItemsConcurrentProcessQueue<ItemType>
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	private readonly Lock _processStateLocker = new();

	private readonly Queue<(ItemType Item, Func<ItemType, Task> ToProcessItemAsync)> _itemsNeedProcessed = [];

	private readonly int? _tasksCountToProcessItemMaxSpecified = null;

	private readonly Func<int>? _toGetTasksCountToProcessItemMax = null;

	public int TasksCountToProcessItemMax
	{
		get
		{
			int tasksCountToProcessItemMax = 0;
			if (_toGetTasksCountToProcessItemMax != null)
			{
				tasksCountToProcessItemMax = _toGetTasksCountToProcessItemMax();
			}
			else if (_tasksCountToProcessItemMaxSpecified != null)
			{
				tasksCountToProcessItemMax = _tasksCountToProcessItemMaxSpecified.Value;
			}
			if (tasksCountToProcessItemMax <= 0)
			{
				return int.MaxValue;
			}
			return tasksCountToProcessItemMax;
		}
	}

	private int _tasksCountProcessingItem;

	private bool _isCanceled;

	private bool _isCurrentProcessCanceled;

	private readonly List<Exception> _currentProcessExceptions = [];

	private TaskCompletionSource<bool> _taskCompletionSourceToWhenAll
		= CreateTaskCompletionSourceCompleted();

	private TaskCompletionSource<bool> _taskCompletionSourceToWhenAny
		= CreateTaskCompletionSourceCompleted();

	private Task TaskToWhenAll
	{
		get
		{
			lock (_processStateLocker)
			{
				return _taskCompletionSourceToWhenAll.Task;
			}
		}
	}

	private Task TaskToWhenAny
	{
		get
		{
			lock (_processStateLocker)
			{
				return _taskCompletionSourceToWhenAny.Task;
			}
		}
	}

	#endregion


	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public ItemsConcurrentProcessQueue(int tasksCountToProcessItemMax)
	{
		_tasksCountToProcessItemMaxSpecified = tasksCountToProcessItemMax;
	}

	public ItemsConcurrentProcessQueue(Func<int>? toGetTasksCountToProcessItemMax)
	{
		_toGetTasksCountToProcessItemMax = toGetTasksCountToProcessItemMax;
	}

	private static TaskCompletionSource<bool> CreateTaskCompletionSource()
	{
		return new(TaskCreationOptions.RunContinuationsAsynchronously);
	}

	private static TaskCompletionSource<bool> CreateTaskCompletionSourceCompleted()
	{
		var taskCompletionSource = CreateTaskCompletionSource();
		taskCompletionSource.SetResult(true);
		return taskCompletionSource;
	}

	private void BeginNewProcessLocked()
	{
		_taskCompletionSourceToWhenAll = CreateTaskCompletionSource();
		_taskCompletionSourceToWhenAny = CreateTaskCompletionSource();
		_currentProcessExceptions.Clear();
		_isCurrentProcessCanceled = false;
	}

	private void StartTaskToProcessItemLocked()
	{
		_tasksCountProcessingItem++;
		_ = Task.Run(ProcessItemsAsync);
	}

	private void AddItemNeedProcessed(ItemType item, Func<ItemType, Task> toProcessItemAsync)
	{
		lock (_processStateLocker)
		{
			if (_isCanceled)
			{
				throw new OperationCanceledException();
			}
			if (_tasksCountProcessingItem < 1 && _itemsNeedProcessed.Count < 1)
			{
				BeginNewProcessLocked();
			}

			// !!!
			_itemsNeedProcessed.Enqueue((item, toProcessItemAsync));
			// !!!
			if (_tasksCountProcessingItem < TasksCountToProcessItemMax)
			{
				StartTaskToProcessItemLocked();
			}
		}
	}

	private void CompleteCurrentProcessLocked()
	{
		if (_currentProcessExceptions.Count > 0)
		{
			_taskCompletionSourceToWhenAll.TrySetException([.. _currentProcessExceptions]);
		}
		else if (_isCurrentProcessCanceled)
		{
			_taskCompletionSourceToWhenAll.TrySetCanceled();
		}
		else
		{
			_taskCompletionSourceToWhenAll.TrySetResult(true);
		}
	}

	private void EndTaskToProcessItemLocked()
	{
		_tasksCountProcessingItem--;
		_taskCompletionSourceToWhenAny.TrySetResult(true);

		if (_isCanceled != true
			&& _itemsNeedProcessed.Count > 0
			&& _tasksCountProcessingItem < TasksCountToProcessItemMax)
		{
			// !!!
			StartTaskToProcessItemLocked();
			// !!!
		}
		if (_tasksCountProcessingItem < 1
			&& _itemsNeedProcessed.Count < 1)
		{
			CompleteCurrentProcessLocked();
		}
	}

	private async Task ProcessItemsAsync()
	{
		while (true)
		{
			(ItemType Item, Func<ItemType, Task> ToProcessItemAsync) itemNeedProcessed;
			lock (_processStateLocker)
			{
				if (_isCanceled
					|| !_itemsNeedProcessed.TryDequeue(out itemNeedProcessed))
				{
					EndTaskToProcessItemLocked();
					return;
				}
			}

			try
			{
				// !!!
				await itemNeedProcessed.ToProcessItemAsync(itemNeedProcessed.Item);
				// !!!
			}
			catch (OperationCanceledException)
			{
				lock (_processStateLocker)
				{
					_isCurrentProcessCanceled = true;
				}
			}
			catch (Exception exception)
			{
				lock (_processStateLocker)
				{
					_currentProcessExceptions.Add(exception);
				}
			}
		}
	}

	////////////////////////////////////////////////

	public void ProcessItem(ItemType? item, Action<ItemType> toProcessItem)
	{
		if (item == null)
		{
			return;
		}
		ArgumentNullException.ThrowIfNull(toProcessItem);

		AddItemNeedProcessed(item, (itemNeedProcess) =>
		{
			toProcessItem(itemNeedProcess);
			return Task.CompletedTask;
		});
	}

	public void ProcessItem(ItemType? item, Func<ItemType, Task> toProcessItemAsync)
	{
		if (item == null)
		{
			return;
		}
		ArgumentNullException.ThrowIfNull(toProcessItemAsync);

		AddItemNeedProcessed(item, toProcessItemAsync);
	}

	public void WaitAll()
	{
		TaskToWhenAll.Wait();
	}

	public void WaitAny()
	{
		TaskToWhenAny.Wait();
	}

	public async Task WhenAll()
	{
		await TaskToWhenAll;
	}

	public async Task WhenAny()
	{
		await TaskToWhenAny;
	}

	public void Cancel()
	{
		lock (_processStateLocker)
		{
			if (_isCanceled)
			{
				return;
			}
			_isCanceled = true;

			if (_tasksCountProcessingItem > 0
				|| _itemsNeedProcessed.Count > 0)
			{
				_isCurrentProcessCanceled = true;
				_itemsNeedProcessed.Clear();
				if (_tasksCountProcessingItem < 1)
				{
					_taskCompletionSourceToWhenAny.TrySetResult(true);
					CompleteCurrentProcessLocked();
				}
			}
		}
	}

	public void CancelAndWaitAll()
	{
		Cancel();
		WaitAll();
	}

	public async Task CancelAndWhenAll()
	{
		Cancel();
		await WhenAll();
	}

	#endregion
}