using BaoXia.Utils.ConcurrentTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Test.ConcurrentTools;

[TestClass]
public class ItemsConcurrentProcessQueueTest
{
	[TestMethod]
	public async Task ProcessItem_ShouldUseHandlerOfCurrentItem()
	{
		var processQueue = new ItemsConcurrentProcessQueue<int>(1);
		using var isFirstItemProcessing = new ManualResetEventSlim();
		var processTypes = new ConcurrentDictionary<int, string>();
		using var canFirstItemProcessCompleted = new ManualResetEventSlim();

		processQueue.ProcessItem(1, (item) =>
		{
			processTypes[item] = "First";
			isFirstItemProcessing.Set();
			canFirstItemProcessCompleted.Wait(TimeSpan.FromSeconds(5), TestContext.CancellationTokenSource.Token);
		});
		try
		{
			Assert.IsTrue(isFirstItemProcessing.Wait(TimeSpan.FromSeconds(5)));
			processQueue.ProcessItem(2, (item) =>
			{
				processTypes[item] = "Second";
			});
		}
		finally
		{
			canFirstItemProcessCompleted.Set();
		}

		await processQueue.WhenAll().WaitAsync(TimeSpan.FromSeconds(5), TestContext.CancellationTokenSource.Token);

		Assert.AreEqual("First", processTypes[1]);
		Assert.AreEqual("Second", processTypes[2]);
	}

	[TestMethod]
	public async Task WhenAll_ShouldAwaitAsyncHandlersAndLimitConcurrency()
	{
		var concurrencyCountMax = 4;
		var itemsCount = 200;
		var processQueue = new ItemsConcurrentProcessQueue<int>(concurrencyCountMax);
		var itemsCountProcessing = 0;
		var itemsCountProcessingMax = 0;
		var itemsCountProcessed = 0;

		for (var itemIndex = 0;
			itemIndex < itemsCount;
			itemIndex++)
		{
			processQueue.ProcessItem(itemIndex, async (_) =>
			{
				var currentItemsCountProcessing = Interlocked.Increment(ref itemsCountProcessing);
				UpdateValueMax(ref itemsCountProcessingMax, currentItemsCountProcessing);
				try
				{
					await Task.Delay(2);
					Interlocked.Increment(ref itemsCountProcessed);
				}
				finally
				{
					Interlocked.Decrement(ref itemsCountProcessing);
				}
			});
		}

		await processQueue.WhenAll().WaitAsync(TimeSpan.FromSeconds(10));

		Assert.AreEqual(itemsCount, itemsCountProcessed);
		Assert.IsTrue(itemsCountProcessingMax > 1);
		Assert.IsTrue(itemsCountProcessingMax <= concurrencyCountMax);
	}

	[TestMethod]
	public async Task ProcessItem_ShouldProcessAllItemsInRepeatedBatches()
	{
		var processQueue = new ItemsConcurrentProcessQueue<int>(4);
		var batchesCount = 100;
		var itemsCountInBatch = 100;
		var itemsCountProcessed = 0;

		for (var batchIndex = 0;
			batchIndex < batchesCount;
			batchIndex++)
		{
			for (var itemIndex = 0;
				itemIndex < itemsCountInBatch;
				itemIndex++)
			{
				processQueue.ProcessItem(itemIndex, (_) =>
				{
					Interlocked.Increment(ref itemsCountProcessed);
				});
			}
			await processQueue.WhenAll().WaitAsync(TimeSpan.FromSeconds(5));
		}

		Assert.AreEqual(
			batchesCount * itemsCountInBatch,
			itemsCountProcessed);
	}

	private static void UpdateValueMax(ref int valueMax, int valueCurrent)
	{
		var valueMaxCurrent = Volatile.Read(ref valueMax);
		while (valueCurrent > valueMaxCurrent)
		{
			var valueMaxPrev = Interlocked.CompareExchange(
				ref valueMax,
				valueCurrent,
				valueMaxCurrent);
			if (valueMaxPrev == valueMaxCurrent)
			{
				return;
			}
			valueMaxCurrent = valueMaxPrev;
		}
	}

	public TestContext TestContext { get; set; }
}
