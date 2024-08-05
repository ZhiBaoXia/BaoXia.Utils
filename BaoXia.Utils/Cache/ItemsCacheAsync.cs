using BaoXia.Utils.Cache.Index.Interfaces;
using BaoXia.Utils.ConcurrentTools;
using BaoXia.Utils.Constants;
using BaoXia.Utils.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Cache;

public class ItemsCacheAsync<ItemKeyType, ItemType, ItemCacheCreateParamType> : IDisposable
	where ItemKeyType : notnull
	where ItemType : class
{

	////////////////////////////////////////////////
	// @静态常量
	////////////////////////////////////////////////

	#region 静态常量

	protected struct WillCreateItemCacheResult(
		bool isItemCacheCreateContinue,
		ItemType? itemCreated)
	{
		public bool IsItemCacheCreateContinue { get; set; } = isItemCacheCreateContinue;
		public ItemType? ItemCreated { get; set; } = itemCreated;
	}

	#endregion


	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	protected CancellationTokenSource _cancellationTokenSource = new();

	protected readonly ConcurrentDictionary<
		ItemKeyType,
		ItemCacheItemContainerAsync<ItemKeyType, ItemType?, ItemCacheCreateParamType?>>
		_itemContainersCache = new();

	public string? Name { get; set; }

	public ICollection<ItemKeyType> Keys
	{
		get
		{
			return _itemContainersCache.Keys;
		}
	}

	public List<ItemType?> Values
	{
		get
		{
			var values = new List<ItemType?>();
			var itemContainers = _itemContainersCache.Values;
			if (itemContainers != null)
			{
				foreach (var itemContainer in itemContainers)
				{
					if (itemContainer == null)
					{
						continue;
					}
					values.Add(itemContainer.Item);
				}
			}
			return values;
		}
	}

	public ICollection<ItemCacheItemContainerAsync<ItemKeyType, ItemType?, ItemCacheCreateParamType?>> ValueContainers
	{
		get
		{
			return _itemContainersCache.Values;
		}
	}

	public IItemCacheIndex<ItemType>[]? ItemCacheIndexes { get; init; }

	public bool IsNullValueValidToCache { get; set; } = false;

	public Func<ItemKeyType, ItemCacheCreateParamType?, Task<ItemType?>> ToDidCreateItemCacheAsync { get; set; }

	public Func<IEnumerable<ItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?>>, Task>? ToDidCreateItemsCacheAsync { get; set; }

	public Func<ItemKeyType, ItemType?, ItemType?, ItemCacheOperation, Task<ItemType?>>? ToDidItemCacheUpdatedAsync { get; set; }



	protected LoopTask? _taskToAutoClean;

	Func<double>? _toDidGetIntervalSecondsToCleanItemCache;
	public Func<double>? ToDidGetIntervalSecondsToCleanItemCache
	{
		get
		{
			return _toDidGetIntervalSecondsToCleanItemCache;
		}
		set
		{
			_toDidGetIntervalSecondsToCleanItemCache = value;
			if (_toDidGetIntervalSecondsToCleanItemCache != null)
			{
				if (_taskToAutoClean == null)
				{
					lock (this)
					{
						if (_taskToAutoClean == null)
						{
							_taskToAutoClean = new LoopTask(
								(CancellationToken cancellationToken) =>
								{
#if DEBUG
									var myType = this.GetType();
									var myName
									= this.Name?.Length > 0
									? this.Name
									: myType.Namespace + "." + myType.Name;
									System.Diagnostics.Trace.WriteLine(myType + "，自动清理缓存元素任务，开始：");
#endif
									// !!!⚠ 开始清理无效的缓存 ⚠!!!
									this.Clean(cancellationToken);
									// !!!
#if DEBUG
									System.Diagnostics.Trace.WriteLine(myType + "，自动清理缓存元素任务，结束。");
#endif

									return true;
								},
								_toDidGetIntervalSecondsToCleanItemCache);
						}
						else
						{
							_taskToAutoClean.ToDidGetIntervalSeconds = _toDidGetIntervalSecondsToCleanItemCache;
						}
					}
				}
			}
			else if (_taskToAutoClean != null)
			{
				lock (this)
				{
					if (_taskToAutoClean != null)
					{
						_taskToAutoClean.Cancel();
						_taskToAutoClean = null;
					}
				}
			}
		}
	}
	protected double IntervalSecondsToCleanItemCache
	{
		get
		{
			var itemCacheNoneReadToRemoveIntervalSeconds = 0.0;
			var didGetIntervalSecondsToCleanItemCache
				= this.ToDidGetIntervalSecondsToCleanItemCache;
			if (didGetIntervalSecondsToCleanItemCache != null)
			{
				itemCacheNoneReadToRemoveIntervalSeconds
				= didGetIntervalSecondsToCleanItemCache();
			}
			return itemCacheNoneReadToRemoveIntervalSeconds;
		}
	}

	public Func<double>? ToDidGetNoneReadSecondsToRemoveItemCache { get; set; }
	protected double NoneReadSecondsToRemoveItemCache
	{
		get
		{
			var intervalSecondsToCleanItemCache = 0.0;
			var didGetNoneReadSecondsToRemoveItemCache
				= this.ToDidGetNoneReadSecondsToRemoveItemCache;
			if (didGetNoneReadSecondsToRemoveItemCache != null)
			{
				intervalSecondsToCleanItemCache
				= didGetNoneReadSecondsToRemoveItemCache();
			}
			return intervalSecondsToCleanItemCache;
		}
	}

	protected LoopTask? _taskToAutoUpdate;
	Func<double>? _toDidGetNoneUpdateSecondsToUpdateItemCache;
	public Func<double>? ToDidGetNoneUpdateSecondsToUpdateItemCache
	{
		get
		{
			return _toDidGetNoneUpdateSecondsToUpdateItemCache;
		}
		set
		{
			_toDidGetNoneUpdateSecondsToUpdateItemCache = value;
			if (_toDidGetNoneUpdateSecondsToUpdateItemCache != null)
			{
				if (_taskToAutoUpdate == null)
				{
					lock (this)
					{
						_taskToAutoUpdate ??= new LoopTask(
								(CancellationToken cancellationToken) =>
								{
#if DEBUG
									var myType = this.GetType();
									var myName
									= this.Name?.Length > 0
									? this.Name
									: myType.Namespace + "." + myType.Name;
									System.Diagnostics.Trace.WriteLine(myType + "，自动更新缓存元素任务，开始：");
#endif
									// !!!⚠ 开始更新无效的缓存 ⚠!!!
									this.UpdateAllItemCache(cancellationToken);
									// !!!
#if DEBUG
									System.Diagnostics.Trace.WriteLine(myType + "，自动更新缓存元素任务，结束。");
#endif

									return true;
								},
								this.ToDidGetNoneUpdateSecondsToUpdateItemCache);
					}
				}
				else
				{
					_taskToAutoUpdate.ToDidGetIntervalSeconds = _toDidGetNoneUpdateSecondsToUpdateItemCache;
				}
			}
			else if (_taskToAutoUpdate != null)
			{
				lock (this)
				{
					if (_taskToAutoUpdate != null)
					{
						_taskToAutoUpdate.Cancel();
						_taskToAutoUpdate = null;
					}
				}
			}
		}
	}

	protected double NoneUpdateSecondsToUpdateItemCache
	{
		get
		{
			var itemCacheNoneReadToUpdateIntervalSeconds = 0.0;
			var didGetNoneUpdateSecondsToUpdateItemCache
				= this.ToDidGetNoneUpdateSecondsToUpdateItemCache;
			if (didGetNoneUpdateSecondsToUpdateItemCache != null)
			{
				itemCacheNoneReadToUpdateIntervalSeconds
				= didGetNoneUpdateSecondsToUpdateItemCache();
			}
			return itemCacheNoneReadToUpdateIntervalSeconds;
		}
	}


	protected readonly ConcurrentQueue<ItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?>> _itemContainersNeedCreateItemAsyncQueue = new();
	public ConcurrentQueue<ItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?>> ItemContainersNeedCreateItemAsyncQueue
		=> _itemContainersNeedCreateItemAsyncQueue;

	protected int _itemContainersCountInCreatingItemAsync;
	public int ItemContainersCountInCreatingItemAsync
	{
		get
		{
			return _itemContainersNeedCreateItemAsyncQueue != null
				? _itemContainersNeedCreateItemAsyncQueue.Count + _itemContainersCountInCreatingItemAsync
				: _itemContainersCountInCreatingItemAsync;
		}
	}

	protected readonly Tasks _tasksToCreateItemAsync;

	public Tasks TasksToCreateItemAsync => _tasksToCreateItemAsync;

	public int Count
	{
		get
		{
			if (_itemContainersCache != null)
			{
				return _itemContainersCache.Count;
			}
			return 0;
		}
	}

	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	public ItemsCacheAsync(
		Func<ItemKeyType, ItemCacheCreateParamType?, Task<ItemType?>> didCreateItemCacheAsync,
		Func<ItemKeyType, ItemType?, ItemType?, ItemCacheOperation, Task<ItemType?>>? didItemCacheUpdatedAsync,
		Func<double>? toDidGetIntervalSecondsToCleanItemCache,
		Func<double>? toDidGetNoneReadSecondsToRemoveItemCache,
		Func<double>? toDidGetNoneUpdateSecondsToUpdateItemCache,
		Func<int>? toDidGetThreadsCountToCreateItemAsync,
		params IItemCacheIndex<ItemType>[]? itemCacheIndexes)
	{
		ItemCacheIndexes = itemCacheIndexes;

		this.ToDidCreateItemCacheAsync = didCreateItemCacheAsync;
		this.ToDidItemCacheUpdatedAsync = didItemCacheUpdatedAsync;
		this.ToDidGetIntervalSecondsToCleanItemCache = toDidGetIntervalSecondsToCleanItemCache;
		this.ToDidGetNoneReadSecondsToRemoveItemCache = toDidGetNoneReadSecondsToRemoveItemCache;
		this.ToDidGetNoneUpdateSecondsToUpdateItemCache = toDidGetNoneUpdateSecondsToUpdateItemCache;

		_tasksToCreateItemAsync = new Tasks(
			toDidGetThreadsCountToCreateItemAsync);
	}

	public ItemsCacheAsync(
		Func<ItemKeyType, ItemCacheCreateParamType?, Task<ItemType?>> didCreateItemCache,
		Func<ItemKeyType, ItemType?, ItemType?, ItemCacheOperation, Task<ItemType?>>? didItemCacheUpdatedAsync,
		Func<double>? toDidGetIntervalAndNoneReadSecondsToRemoveItemCache,
		Func<double>? toDidGetNoneUpdateSecondsToUpdateItemCache = null,
		Func<int>? toDidGetThreadsCountToCreateItemAsync = null,
		params IItemCacheIndex<ItemType>[]? itemCacheIndexes)
		: this(didCreateItemCache,
			  didItemCacheUpdatedAsync,
			  toDidGetIntervalAndNoneReadSecondsToRemoveItemCache,
			  toDidGetIntervalAndNoneReadSecondsToRemoveItemCache,
			  toDidGetNoneUpdateSecondsToUpdateItemCache,
			  toDidGetThreadsCountToCreateItemAsync,
			  itemCacheIndexes)
	{ }

	public ItemsCacheAsync(
		Func<ItemKeyType, ItemCacheCreateParamType?, Task<ItemType?>> didCreateItemCache,
		Func<ItemKeyType, ItemType?, ItemType?, ItemCacheOperation, Task<ItemType?>>? didItemCacheUpdatedAsync,
		Func<double>? toDidGetIntervalAndNoneReadSecondsToRemoveItemCache,
		params IItemCacheIndex<ItemType>[]? itemCacheIndexes)
		: this(didCreateItemCache,
			  didItemCacheUpdatedAsync,
			  toDidGetIntervalAndNoneReadSecondsToRemoveItemCache,
			  toDidGetIntervalAndNoneReadSecondsToRemoveItemCache,
			  null,
			  null,
			  itemCacheIndexes)
	{ }

	~ItemsCacheAsync()
	{
		_cancellationTokenSource.Cancel();
		_itemContainersCache.Clear();
		// !!!
		_taskToAutoClean?.Dispose();
		_taskToAutoUpdate?.Dispose();
		_tasksToCreateItemAsync.Dispose();
		// !!!
	}

	public async Task<ItemType?> AddAsync(
		ItemKeyType itemKey,
		ItemType? item,
		ItemCacheCreateParamType? itemCreateParam = default,
		bool isNeedUpdateItemLastReadTime = true)
	{
		if (typeof(ItemKeyType).IsPointer
			&& itemKey == null)
		{
			return default;
		}

		////////////////////////////////////////////////
		var itemNeedAdd = await this.DidItemCacheUpdatedAsync(
			itemKey,
			default,
			item,
			ItemCacheOperation.Add);
		if (itemNeedAdd == null
			&& this.IsNullValueValidToCache != true)
		{
			return default;
		}
		////////////////////////////////////////////////

		var now = DateTime.Now;
		var newItemContainer
			= new ItemCacheItemContainerAsync<ItemKeyType, ItemType?, ItemCacheCreateParamType?>(
				itemKey,
				itemNeedAdd,
				true,
				itemCreateParam,
				now,
				now,
				null);
		var itemContainer
			= _itemContainersCache.AddOrUpdateWithNewValue(
			itemKey,
			   newItemContainer);
		if (itemContainer != newItemContainer)
		{
			itemContainer.SetItem(
				itemNeedAdd,
				itemCreateParam,
				isNeedUpdateItemLastReadTime);
		}
		return item;
	}

	protected async Task<ItemType?> UpdateAsync(
		ItemKeyType itemKey,
		ItemType? itemSpecified,
		ItemCacheCreateParamType? itemCreateParam = default,
		bool isNeedUpdateItemLastReadTime = true)
	{
		if (typeof(ItemKeyType).IsPointer
			&& itemKey == null)
		{
			return default;
		}

		if (_itemContainersCache.TryGetValue(itemKey, out var itemContainer)
			&& itemContainer != null)
		{
			// !!!
			itemCreateParam ??= itemContainer.ItemCreateParam;
			// !!!
		}

		ItemType? newItem = itemSpecified;
		if (newItem == null)
		{
			var willCreateItemCacheAsyncResult
			= await DidWillCreateItemCacheAsync(
			itemKey,
			itemCreateParam);
			newItem = willCreateItemCacheAsyncResult.ItemCreated;
			var toDidCreateItemCacheAsync = this.ToDidCreateItemCacheAsync;
			if (willCreateItemCacheAsyncResult.IsItemCacheCreateContinue
				&& toDidCreateItemCacheAsync != null)
			{
				newItem = await toDidCreateItemCacheAsync(
					itemKey,
					itemCreateParam);
			}
		}

		if (itemContainer != null)
		{
			////////////////////////////////////////////////
			newItem = await this.DidItemCacheUpdatedAsync(
				itemKey,
				itemContainer.Item,
				newItem,
				ItemCacheOperation.Update);
			if (newItem != null
				|| this.IsNullValueValidToCache)
			{
				itemContainer.SetItem(
					newItem,
					itemCreateParam,
					isNeedUpdateItemLastReadTime);
			}
			////////////////////////////////////////////////
			return newItem;
		}
		return await this.AddAsync(
			itemKey,
			newItem,
			itemCreateParam,
			isNeedUpdateItemLastReadTime);
	}

	public async Task<ItemType?> UpdateAsync(
		ItemKeyType itemKey,
		ItemCacheCreateParamType? itemCreateParam = default,
		bool isNeedUpdateItemLastReadTime = true)
	{
		return await UpdateAsync(
			itemKey,
			null,
			itemCreateParam,
			isNeedUpdateItemLastReadTime);
	}

	public async Task<ItemType?> SetAsync(
		ItemKeyType itemKey,
		ItemType? item,
		ItemCacheCreateParamType? itemCreateParam = default,
		bool isNeedUpdateItemLastReadTime = true)
	{
		return await UpdateAsync(
			itemKey,
			item,
			itemCreateParam,
			isNeedUpdateItemLastReadTime);
	}


	public async Task<ItemType?> RemoveAsync(ItemKeyType itemKey)
	{
		if (typeof(ItemKeyType).IsPointer
			&& itemKey == null)
		{
			return default;
		}

		_itemContainersCache.TryRemove(
			itemKey,
			out var itemContainerRemoved);

		var itemRemoved
			= itemContainerRemoved != null
			? itemContainerRemoved.Item
			: default;

		////////////////////////////////////////////////
		await this.DidItemCacheUpdatedAsync(
			itemKey,
			itemRemoved,
			default,
			ItemCacheOperation.Remove);
		////////////////////////////////////////////////
		return itemRemoved;
	}

	public void Clear()
	{
		_itemContainersCache.Clear();
	}

	public void Clean(
		double noneReadSecondsToRemoveItemCache,
		CancellationToken cancellationToken)
	{
		if (noneReadSecondsToRemoveItemCache <= 0.0)
		{
			noneReadSecondsToRemoveItemCache
				= this.NoneReadSecondsToRemoveItemCache;
		}
		if (noneReadSecondsToRemoveItemCache > 0.0)
		{
			this.DidClean(
				noneReadSecondsToRemoveItemCache,
				cancellationToken);
		}
	}
	public void Clean(CancellationToken cancellationToken)
	{
		this.Clean(
			this.NoneReadSecondsToRemoveItemCache,
			cancellationToken);
	}

	public void UpdateAllItemCache(
		double noneUpdateSecondsToUpdateItemCache,
		CancellationToken cancellationToken)
	{
		if (noneUpdateSecondsToUpdateItemCache <= 0.0)
		{
			noneUpdateSecondsToUpdateItemCache
				= this.NoneUpdateSecondsToUpdateItemCache;
		}
		if (noneUpdateSecondsToUpdateItemCache > 0.0)
		{
			this.DidUpdateAllItemCache(
				noneUpdateSecondsToUpdateItemCache,
				cancellationToken);
		}
	}

	public void UpdateAllItemCache(CancellationToken cancellationToken)
	{
		this.UpdateAllItemCache(
			this.NoneUpdateSecondsToUpdateItemCache,
			cancellationToken);
	}

	protected async Task<ItemCacheItemContainerAsync<ItemKeyType, ItemType?, ItemCacheCreateParamType?>?> TryGetAsync(
		ItemKeyType itemCacheKey,
		bool isNeedTryToCreateItem,
		bool isNeedCreateItemAsync,
		ItemCacheCreateParamType? itemCacheCreateParam,
		bool isItemSpecifiedValid,
		ItemType? itemSpecified)
	{
		if (typeof(ItemKeyType).IsPointer
			&& itemCacheKey == null)
		{
			return null;
		}

		var isItemContainerShot
			= _itemContainersCache.TryGetValue(
			itemCacheKey,
			out var itemContainer);
		var isItemContainerInvalid
			= isItemContainerShot == false
			|| itemContainer == null
			|| !itemContainer.IsItemValid;
		if (isItemContainerInvalid
			&& isNeedTryToCreateItem)
		{
			Task<Task<ItemType?>>? taskToCreateItemJustCreate = null;
			if (itemContainer == null)
			{
				var now = DateTime.Now;
				itemContainer
					= new ItemCacheItemContainerAsync<ItemKeyType, ItemType?, ItemCacheCreateParamType?>(
						itemCacheKey,
						default,
						false,
						itemCacheCreateParam,
						now,
						now,
						null);
				{
					taskToCreateItemJustCreate
						= isItemSpecifiedValid
						? null
						: this.DidCreateTaskToCreateItemForItemContainer(
							itemContainer,
							itemCacheCreateParam,
							true);
				}
				itemContainer.ItemCacheCreateTask
					= taskToCreateItemJustCreate;

				////////////////////////////////////////////////
				// !!!
				itemContainer = _itemContainersCache.GetOrAdd(
					itemCacheKey,
					itemContainer);
				// !!!
				////////////////////////////////////////////////
			}

			if (isItemSpecifiedValid)
			{
				itemSpecified = await DidItemCacheUpdatedAsync(
					itemCacheKey,
					default,
					itemSpecified,
					ItemCacheOperation.Add);
				if (itemSpecified == null
					&& this.IsNullValueValidToCache != true)
				{
					return null;
				}
				// !!!
				itemContainer!.SetItem(
					itemSpecified,
					itemCacheCreateParam,
					true);
				// !!!
			}
			else if (isNeedCreateItemAsync)
			{
				////////////////////////////////////////////////
				// 1/2，将需要异步创建元素的容器，压入需要处理的队列。
				////////////////////////////////////////////////
				// !!!
				_itemContainersNeedCreateItemAsyncQueue.Enqueue(itemContainer!);
				// !!!

				////////////////////////////////////////////////
				// 2/2，尝试开启异步创建元素的处理任务。
				////////////////////////////////////////////////
				var cancellationToken = _cancellationTokenSource.Token;
				_ = _tasksToCreateItemAsync.TryRun(
					async () =>
					{
						var itemContainersNeedCreateItem
						= new List<ItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?>>();
						while (_itemContainersNeedCreateItemAsyncQueue.TryDequeue(
							out var itemContainerNeedCreateItem))
						{
							if (cancellationToken.IsCancellationRequested)
							{
								return;
							}

							// !!!
							itemContainersNeedCreateItem.AddUnique(itemContainerNeedCreateItem);
							// !!!
						}
						// !!!
						var itemContainersCountInCreatingItemAsyncAddend
						= itemContainersNeedCreateItem.Count;
						Interlocked.Add(
							ref _itemContainersCountInCreatingItemAsync,
							itemContainersCountInCreatingItemAsyncAddend);
						// !!!
						var toDidCreateItemsCacheAsync = this.ToDidCreateItemsCacheAsync;
						var toDidCreateItemCacheAsync = this.ToDidCreateItemCacheAsync;
						if (toDidCreateItemsCacheAsync != null)
						{
							// !!!
							itemContainersNeedCreateItem
							= await DidWillCreateItemsCacheAsync(itemContainersNeedCreateItem);
							if (itemContainersNeedCreateItem.Count > 0)
							// !!!
							{
								////////////////////////////////////////////////
								// !!!
								await toDidCreateItemsCacheAsync(itemContainersNeedCreateItem);
								// !!!
								////////////////////////////////////////////////

								foreach (var itemContainerNeedCreateItem in itemContainersNeedCreateItem)
								{
									var itemUpdated = await DidItemCacheUpdatedAsync(
										itemContainerNeedCreateItem.Key,
										default,
										itemContainerNeedCreateItem.Item,
										ItemCacheOperation.Add);
									if (itemSpecified != null
										|| this.IsNullValueValidToCache == true)
									{
										// !!!
										itemContainerNeedCreateItem.SetItem(
											itemUpdated,
											itemContainerNeedCreateItem.ItemCreateParam,
											true);
										// !!!
									}
								}
							}
						}
						else if (toDidCreateItemCacheAsync != null)
						{
							foreach (var itemContainerNeedCreateItem
							in
							itemContainersNeedCreateItem)
							{
								if (cancellationToken.IsCancellationRequested)
								{
									return;
								}

								var willCreateItemCacheAsyncResult
								= await DidWillCreateItemCacheAsync(
									itemContainerNeedCreateItem.Key,
									itemContainerNeedCreateItem.ItemCreateParam);
								var newItem = willCreateItemCacheAsyncResult.ItemCreated;
								if (willCreateItemCacheAsyncResult.IsItemCacheCreateContinue)
								{
									newItem
									= await toDidCreateItemCacheAsync(
										itemContainerNeedCreateItem.Key,
										itemContainerNeedCreateItem.ItemCreateParam);
								}

								newItem = await DidItemCacheUpdatedAsync(
									itemContainerNeedCreateItem.Key,
									default,
									newItem,
									ItemCacheOperation.Add);
								if (newItem != null
								|| IsNullValueValidToCache == true)
								{
									itemContainerNeedCreateItem.SetItem(
										newItem,
										itemContainerNeedCreateItem.ItemCreateParam,
										true);
								}
							}
						}
						Interlocked.Add(
							ref _itemContainersCountInCreatingItemAsync,
							-1 * itemContainersCountInCreatingItemAsyncAddend);
					});
			}
			else
			{
				var itemCacheCreateTask = itemContainer.ItemCacheCreateTask;
				if (itemCacheCreateTask != null)
				{
					if (itemCacheCreateTask == taskToCreateItemJustCreate)
					{
						// !!!

						// RunSynchronously()求调度器同步运行它，但是调度器很可能忽略这个提示，在线程池线程中运行它，您当前的线程将会同步阻塞，直到它完成为止。
						// itemCacheCreateTask.RunSynchronously();
						// Start()尝试并将任务排队到调度程序，而RunSynchronously()将尝试并内联执行它，如果失败(返回false)，它将只是排队。
						itemCacheCreateTask.Start();
						// !!!
					}
					// !!!
					await await itemCacheCreateTask;
					// !!!
				}
			}
		}
		else if (itemContainer != null)
		{
			itemContainer.LastReadTime = DateTime.Now;
		}
		return itemContainer;
	}

	public async Task<TryGetItemResultAsync<ItemKeyType, ItemType?, ItemCacheCreateParamType?>> TryGetAsync(
		ItemKeyType itemCacheKey,
		bool isNeedCreateItemAsync = false,
		ItemCacheCreateParamType? itemCacheCreateParam = default)
	{
		var itemContainer = await this.TryGetAsync(
			itemCacheKey,
			isNeedCreateItemAsync,
			isNeedCreateItemAsync,
			itemCacheCreateParam,
			false,
			default);
		if (itemContainer == null
			|| itemContainer.IsItemValid == false)
		{
			return new TryGetItemResultAsync<ItemKeyType, ItemType?, ItemCacheCreateParamType?>
			{
				IsGotSucess = false
			};
		}
		return new TryGetItemResultAsync<ItemKeyType, ItemType?, ItemCacheCreateParamType?>
		{
			IsGotSucess = true,
			itemContainer = itemContainer,
			Item = itemContainer.Item
		};
	}

	public async Task<ItemCacheItemContainerAsync<ItemKeyType, ItemType?, ItemCacheCreateParamType?>?> GetItemContainerAsync(
		ItemKeyType itemCacheKey,
		ItemCacheCreateParamType? itemCacheCreateParam)
	{
		var itemContainer = await this.TryGetAsync(
			itemCacheKey,
			true,
			false,
			itemCacheCreateParam,
			false,
			default);
		{ }
		return itemContainer;
	}

	public async Task<ItemType?> GetAsync(
		ItemKeyType itemCacheKey,
		ItemCacheCreateParamType? itemCacheCreateParam)
	{
		var itemContainer = await this.GetItemContainerAsync(itemCacheKey, itemCacheCreateParam);
		{ }
		return itemContainer?.Item;
	}

	////////////////////////////////////////////////
	// @事件节点
	////////////////////////////////////////////////

	protected virtual async Task<List<ItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?>>> DidWillCreateItemsCacheAsync(List<ItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?>> itemCacheContainersNeedCreate)
	{
		return await Task.FromResult(itemCacheContainersNeedCreate);
	}

	protected virtual async Task<WillCreateItemCacheResult> DidWillCreateItemCacheAsync(
		ItemKeyType itemKey,
		ItemCacheCreateParamType? itemCacheCreateParam)
	{
		return await Task.FromResult(new WillCreateItemCacheResult(true, default));
	}

	protected virtual Task<Task<ItemType?>> DidCreateTaskToCreateItemForItemContainer(
		ItemCacheItemContainerAsync<ItemKeyType, ItemType?, ItemCacheCreateParamType?> itemContainer,
		ItemCacheCreateParamType? itemCacheCreateParam,
		bool isNeedUpdateItemLastReadTime)
	{
		var task = new Task<Task<ItemType?>>(async () =>
		{
			if (itemContainer == null)
			{
				throw new KeyNotFoundException("itemContainer 不应为“null”。");
			}

			if (itemContainer.Item == null)
			{
				var willCreateItemCacheAsyncResult
				= await DidWillCreateItemCacheAsync(
					itemContainer.Key,
					itemCacheCreateParam);
				var newItem = willCreateItemCacheAsyncResult.ItemCreated;
				if (willCreateItemCacheAsyncResult.IsItemCacheCreateContinue)
				{
					newItem
					= await this.ToDidCreateItemCacheAsync(
						itemContainer.Key,
						itemCacheCreateParam);
				}

				newItem = await DidItemCacheUpdatedAsync(
					itemContainer.Key,
					default,
					newItem,
					ItemCacheOperation.Add);
				if (newItem != null
				|| this.IsNullValueValidToCache)
				{
					// !!!
					itemContainer.SetItem(
						newItem,
						itemCacheCreateParam,
						isNeedUpdateItemLastReadTime);
					// !!!
				}
			}
			return itemContainer.Item;
		});
		return task;
	}

	protected virtual async Task<ItemType?> DidItemCacheUpdatedAsync(
		ItemKeyType itemKey,
		ItemType? lastItem,
		ItemType? currentItem,
		ItemCacheOperation itemCacheOperation)
	{
		if (ItemCacheIndexes is IItemCacheIndex<ItemType>[] itemCacheIndexes)
		{
			switch (itemCacheOperation)
			{
				default:
				case ItemCacheOperation.None:
				case ItemCacheOperation.Read:
					{ }
					break;
				case ItemCacheOperation.Add:
					{
						foreach (var itemCacheIndex in itemCacheIndexes)
						{
							itemCacheIndex.UpdateIndexItemsByInsertItem(currentItem!);
						}
					}
					break;
				case ItemCacheOperation.Update:
					{
						foreach (var itemCacheIndex in itemCacheIndexes)
						{
							itemCacheIndex.UpdateIndexItemsByUpdateItemFrom(lastItem!, currentItem!);
						}
					}
					break;
				case ItemCacheOperation.Remove:
					{
						foreach (var itemCacheIndex in itemCacheIndexes)
						{
							itemCacheIndex.UpdateIndexItemsByRemoveItem(lastItem!);
						}
					}
					break;
			}
		}

		var toDidItemCacheUpdatedAsync
			= this.ToDidItemCacheUpdatedAsync;
		if (toDidItemCacheUpdatedAsync != null)
		{
			return await toDidItemCacheUpdatedAsync(
				itemKey,
				lastItem,
				currentItem,
				itemCacheOperation);
		}
		return currentItem;
	}

	protected virtual async void DidUpdateAllItemCache(
		double noneUpdateSecondsToUpdateItemCache,
		CancellationToken cancellationToken)
	{
		var itemKeys = _itemContainersCache.Keys;
		if (itemKeys == null)
		{
			return;
		}

		foreach (var itemKey
			in
			itemKeys)
		{
			if (_itemContainersCache.TryGetValue(
				itemKey,
				out var itemContainer))
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return;
				}
				if (itemContainer != null)
				{
					if ((DateTime.Now - itemContainer.LastUpdateTime).TotalMilliseconds
					   >= noneUpdateSecondsToUpdateItemCache)
					{
						await this.UpdateAsync(itemKey);
					}
				}
			}
		}
	}

	protected virtual void DidClean(
		double noneReadSecondsToRemoveItemCache,
		CancellationToken cancellationToken)
	{
		var itemKeys = _itemContainersCache.Keys;
		if (itemKeys == null)
		{
			return;
		}

		foreach (var itemKey
			in
			itemKeys)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return;
			}
			if (_itemContainersCache.TryGetValue(
				itemKey,
				out var itemContainer))
			{
				var isItemContainerNeedRemove = false;
				if (itemContainer == null)
				{
					isItemContainerNeedRemove = true;
				}
				else if ((DateTime.Now - itemContainer.LastReadTime).TotalSeconds
					>= noneReadSecondsToRemoveItemCache)
				{
					isItemContainerNeedRemove = true;
				}
				if (isItemContainerNeedRemove)
				{
					_itemContainersCache.TryRemove(itemKey, out _);
				}
			}
		}
	}

	////////////////////////////////////////////////
	// @实现“IDisposable”
	////////////////////////////////////////////////

	public void Dispose()
	{
		_cancellationTokenSource.Cancel();
		_itemContainersCache.Clear();
		// !!!
		_taskToAutoClean?.Dispose();
		_taskToAutoUpdate?.Dispose();
		_tasksToCreateItemAsync.Dispose();
		// !!!

		////////////////////////////////////////////////
		GC.SuppressFinalize(this);
		////////////////////////////////////////////////
	}
}
