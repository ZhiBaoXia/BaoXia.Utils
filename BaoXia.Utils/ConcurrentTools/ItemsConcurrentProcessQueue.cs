using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace BaoXia.Utils.ConcurrentTools;

public class ItemsConcurrentProcessQueue<ItemType>(int tasksCountToProcessItemMax)
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public ConcurrentQueue<ItemType> ItemQueueNeedProcessed { get; set; } = new();

	public Tasks TaskToProcessItem { get; set; } = new(tasksCountToProcessItemMax);

	#endregion


	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public void ProcessItem(
	    ItemType? item,
	    Action<ItemType> toProcessItem)
	{
		if (item == null)
		{
			return;
		}
		// !!!
		ItemQueueNeedProcessed.Enqueue(item);
		//
		TaskToProcessItem.TryRun(() =>
		{
			while (ItemQueueNeedProcessed.TryDequeue(
				out var itemNeedProcess))
			{
				// !!!
				toProcessItem(itemNeedProcess);
				// !!!
			}
		});
		// !!!
	}

	public void ProcessItem(
	    ItemType? item,
	    Func<ItemType, Task> toProcessItemAsync)
	{
		if (item == null)
		{
			return;
		}
		// !!!
		ItemQueueNeedProcessed.Enqueue(item);
		//
		TaskToProcessItem.TryRun(async () =>
		{
			while (ItemQueueNeedProcessed.TryDequeue(
				out var itemNeedProcess))
			{
				// !!!
				await toProcessItemAsync(itemNeedProcess);
				// !!!
			}
		});
		// !!!
	}

	public void WaitAll()
	{
		// !!!
		TaskToProcessItem.WaitAll();
		// !!!
	}

	public void WaitAny()
	{
		// !!!
		TaskToProcessItem.WaitAny();
		// !!!
	}

	public async Task WhenAll()
	{
		// !!!
		await TaskToProcessItem.WhenAll();
		// !!!
	}

	public async Task WhenAny()
	{
		// !!!
		await TaskToProcessItem.WhenAny();
		// !!!
	}

	public void Cancel()
	{
		TaskToProcessItem.Cancel();
	}

	public void CancelAndWaitAll()
	{
		TaskToProcessItem.Cancel();
		WaitAll();
	}

	public async Task CancelAndWhenAll()
	{
		TaskToProcessItem.Cancel();
		await WhenAll();
	}

	#endregion
}