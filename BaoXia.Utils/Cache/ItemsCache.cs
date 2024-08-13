using BaoXia.Utils.Cache.Index.Interfaces;
using BaoXia.Utils.ConcurrentTools;
using BaoXia.Utils.Constants;
using BaoXia.Utils.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace BaoXia.Utils.Cache;

public class ItemsCache<ItemKeyType, ItemType, ItemCacheCreateParamType>
	: IDisposable
	where ItemKeyType : notnull
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	protected CancellationTokenSource _cancellationTokenSource = new();

	protected readonly ConcurrentDictionary<ItemKeyType, ItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?>?>
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
			if (itemContainers?.Count > 0)
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

	public ICollection<ItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?>?> ValueContainers
	{
		get
		{
			return _itemContainersCache.Values;
		}
	}

	public IItemCacheIndex<ItemType>[]? ItemCacheIndexes { get; init; }

	public bool IsNullValueValidToCache { get; set; } = false;

	public Func<ItemKeyType, ItemCacheCreateParamType?, ItemType?> ToDidCreateItemCache { get; set; }

	public Action<IEnumerable<ItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?>>>? ToDidCreateItemsCache { get; set; }


	public Func<ItemKeyType, ItemType?, ItemType?, ItemType?>? ToWillUpdateItemCache { get; set; }

	public Action<ItemKeyType, ItemType?, ItemType?>? ToDidItemCacheUpdated { get; set; }



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
						_taskToAutoClean ??= new LoopTask(
							(CancellationToken cancellationToken) =>
							{
#if DEBUG
								var myType = GetType();
								var myName
											= Name?.Length > 0
											? Name
											: myType.Namespace + "." + myType.Name;
								System.Diagnostics.Trace.WriteLine(myType + "，自动清理缓存元素任务，开始：");
#endif
								// !!!⚠ 开始清理无效的缓存 ⚠!!!
								Clean(cancellationToken);
								// !!!
#if DEBUG
								System.Diagnostics.Trace.WriteLine(myType + "，自动清理缓存元素任务，结束。");
#endif
								if (cancellationToken.IsCancellationRequested)
								{

								}
								return true;
							},
							_toDidGetIntervalSecondsToCleanItemCache);
					}
				}
				else
				{
					_taskToAutoClean.ToDidGetIntervalSeconds = _toDidGetIntervalSecondsToCleanItemCache;
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
				= ToDidGetIntervalSecondsToCleanItemCache;
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
				= ToDidGetNoneReadSecondsToRemoveItemCache;
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
								var myType = GetType();
								var myName
											= Name?.Length > 0
											? Name
											: myType.Namespace + "." + myType.Name;
								System.Diagnostics.Trace.WriteLine(myType + "，自动更新缓存元素任务，开始：");
#endif
								// !!!⚠ 开始更新无效的缓存 ⚠!!!
								UpdateAllItemCache(cancellationToken);
								// !!!
#if DEBUG
								System.Diagnostics.Trace.WriteLine(myType + "，自动更新缓存元素任务，结束。");
#endif

								return true;
							},
							ToDidGetNoneUpdateSecondsToUpdateItemCache);
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
				= ToDidGetNoneUpdateSecondsToUpdateItemCache;
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

	public ItemsCache(
		Func<ItemKeyType, ItemCacheCreateParamType?, ItemType?> didCreateItemCache,
		Action<ItemKeyType, ItemType?, ItemType?>? toDidItemCacheUpdated,
		Func<double>? toDidGetIntervalSecondsToCleanItemCache,
		Func<double>? toDidGetNoneReadSecondsToRemoveItemCache,
		Func<double>? toDidGetNoneUpdateSecondsToUpdateItemCache,
		Func<int>? toDidGetThreadsCountToCreateItemAsync,
		params IItemCacheIndex<ItemType>[]? itemCacheIndexes)
	{
		ItemCacheIndexes = itemCacheIndexes;

		ToDidCreateItemCache = didCreateItemCache;
		ToDidItemCacheUpdated = toDidItemCacheUpdated;
		ToDidGetIntervalSecondsToCleanItemCache = toDidGetIntervalSecondsToCleanItemCache;
		ToDidGetNoneReadSecondsToRemoveItemCache = toDidGetNoneReadSecondsToRemoveItemCache;
		ToDidGetNoneUpdateSecondsToUpdateItemCache = toDidGetNoneUpdateSecondsToUpdateItemCache;

		_tasksToCreateItemAsync = new Tasks(
			toDidGetThreadsCountToCreateItemAsync);
	}

	public ItemsCache(
		Func<ItemKeyType, ItemCacheCreateParamType?, ItemType?> didCreateItemCache,
		Action<ItemKeyType, ItemType?, ItemType?>? toDidItemCacheUpdated,
		Func<double>? toDidGetIntervalAndNoneReadSecondsToRemoveItemCache,
		Func<double>? toDidGetNoneUpdateSecondsToUpdateItemCache = null,
		Func<int>? toDidGetThreadsCountToCreateItemAsync = null,
		params IItemCacheIndex<ItemType>[]? itemCacheIndexes)
		: this(didCreateItemCache,
			  toDidItemCacheUpdated,
			  toDidGetIntervalAndNoneReadSecondsToRemoveItemCache,
			  toDidGetIntervalAndNoneReadSecondsToRemoveItemCache,
			  toDidGetNoneUpdateSecondsToUpdateItemCache,
			  toDidGetThreadsCountToCreateItemAsync,
			  itemCacheIndexes)
	{ }

	public ItemsCache(
		Func<ItemKeyType, ItemCacheCreateParamType?, ItemType?> didCreateItemCache,
		Action<ItemKeyType, ItemType?, ItemType?>? toDidItemCacheUpdated,
		Func<double>? toDidGetIntervalAndNoneReadSecondsToRemoveItemCache,
		params IItemCacheIndex<ItemType>[]? itemCacheIndexes)
		: this(didCreateItemCache,
			  toDidItemCacheUpdated,
			  toDidGetIntervalAndNoneReadSecondsToRemoveItemCache,
			  toDidGetIntervalAndNoneReadSecondsToRemoveItemCache,
			  null,
			  null,
			  itemCacheIndexes)
	{ }

	~ItemsCache()
	{
		_cancellationTokenSource.Cancel();
		_itemContainersCache.Clear();
		// !!!
		_taskToAutoUpdate?.Dispose();
		_taskToAutoClean?.Dispose();
		_tasksToCreateItemAsync.Dispose();
		// !!!
	}

	public ItemType? Add(
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

		var now = DateTime.Now;
		var newItemContainer
			= new ItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?>(
				itemKey,
				item,
				true,
				itemCreateParam,
				now,
				now);
		var itemContainer
			= _itemContainersCache.AddOrUpdateWithNewValue(
				itemKey,
				newItemContainer);
		ItemType? lastItem = default;
		////////////////////////////////////////////////
		// !!!
		item = DidWillUpdateItemCache(
			itemKey,
			lastItem,
			item);
		// !!!
		////////////////////////////////////////////////
		if (itemContainer != null
			&& itemContainer != newItemContainer)
		{
			// !!!
			lastItem = itemContainer.Item;
			// !!!
			itemContainer.SetItem(
				item,
				itemCreateParam,
				isNeedUpdateItemLastReadTime);
			////////////////////////////////////////////////
			// !!!
			DidItemCacheUpdated(
				itemKey,
				lastItem,
				item);
			// !!!
			////////////////////////////////////////////////
		}
		return item;
	}

	protected ItemType? Update(
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
			var toDidCreateItemCache = ToDidCreateItemCache;
			if (DidWillCreateItemCache(
				itemKey,
				itemCreateParam,
				out newItem)
				&& toDidCreateItemCache != null)
			{
				newItem = toDidCreateItemCache(
					itemKey,
					itemCreateParam);
			}
		}

		if (itemContainer != null)
		{
			var lastItem = itemContainer.Item;
			////////////////////////////////////////////////
			// !!!
			newItem = DidWillUpdateItemCache(
				itemKey,
				lastItem,
				newItem);
			// !!!
			////////////////////////////////////////////////
			if (newItem != null
				|| IsNullValueValidToCache)
			{
				////////////////////////////////////////////////
				itemContainer.SetItem(
					newItem,
					itemCreateParam,
					isNeedUpdateItemLastReadTime);
				////////////////////////////////////////////////
				// !!!
				DidItemCacheUpdated(
					itemKey,
					lastItem,
					newItem);
				// !!!
				////////////////////////////////////////////////
			}
			////////////////////////////////////////////////
			return newItem;
			////////////////////////////////////////////////
		}
		return Add(
			itemKey,
			newItem,
			itemCreateParam,
			isNeedUpdateItemLastReadTime);
	}

	public ItemType? Update(
		ItemKeyType itemKey,
		ItemCacheCreateParamType? itemCreateParam = default,
		bool isNeedUpdateItemLastReadTime = true)
	{
		return Update(
			itemKey,
			default,
			itemCreateParam,
			isNeedUpdateItemLastReadTime);
	}

	public ItemType? Set(
		ItemKeyType itemKey,
		ItemType? item,
		ItemCacheCreateParamType? itemCreateParam = default,
		bool isNeedUpdateItemLastReadTime = true)
	{
		return Update(
			itemKey,
			item,
			itemCreateParam,
			isNeedUpdateItemLastReadTime);
	}

	public ItemType? Remove(ItemKeyType itemKey)
	{
		if (typeof(ItemKeyType).IsPointer
			&& itemKey == null)
		{
			return default;
		}

		if (this.TryGet(itemKey, out var itemNeedRemoved) != true)
		{
			return default;
		}

		////////////////////////////////////////////////
		// !!!
		itemNeedRemoved = DidWillUpdateItemCache(
			itemKey,
			itemNeedRemoved,
			default);
		if (itemNeedRemoved == null)
		{
			return default;
		}
		// !!!
		////////////////////////////////////////////////

		_itemContainersCache.TryRemove(
			itemKey,
			out var itemContainerRemoved);

		////////////////////////////////////////////////
		var itemRemoved
			= itemContainerRemoved != null
			? itemContainerRemoved.Item
			: default;
		////////////////////////////////////////////////
		// !!!
		DidItemCacheUpdated(
			itemKey,
			itemRemoved,
			default);
		// !!!
		////////////////////////////////////////////////
		return itemRemoved;
	}

	public void Clear()
	{
		_itemContainersCache.Clear();

		if (ItemCacheIndexes is IItemCacheIndex<ItemType>[] itemCacheIndexes)
		{
			foreach (var itemCacheINdex in itemCacheIndexes)
			{
				itemCacheINdex.Clear();
			}
		}
	}

	public void Clean(
		double noneReadSecondsToRemoveItemCache,
		CancellationToken cancellationToken)
	{
		if (noneReadSecondsToRemoveItemCache <= 0.0)
		{
			noneReadSecondsToRemoveItemCache
				= NoneReadSecondsToRemoveItemCache;
		}
		if (noneReadSecondsToRemoveItemCache > 0.0)
		{
			DidClean(
				noneReadSecondsToRemoveItemCache,
				cancellationToken);
		}
	}

	public void Clean(CancellationToken cancellationToken)
	{
		Clean(NoneReadSecondsToRemoveItemCache, cancellationToken);
	}

	public void UpdateAllItemCache(
		double noneUpdateSecondsToUpdateItemCache,
		CancellationToken cancellationToken)
	{
		if (noneUpdateSecondsToUpdateItemCache <= 0.0)
		{
			noneUpdateSecondsToUpdateItemCache
				= NoneUpdateSecondsToUpdateItemCache;
		}
		if (noneUpdateSecondsToUpdateItemCache > 0.0)
		{
			DidUpdateAllItemCache(noneUpdateSecondsToUpdateItemCache, cancellationToken);
		}
	}

	public void UpdateAllItemCache(CancellationToken cancellationToken)
	{
		UpdateAllItemCache(NoneUpdateSecondsToUpdateItemCache, cancellationToken);
	}

	protected ItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?>? TryGet(
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
			return default;
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
			itemContainer ??= _itemContainersCache.GetOrAdd(
				itemCacheKey,
				new ItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?>(
					itemCacheKey,
					itemCacheCreateParam));
			lock (itemContainer!)
			{
				if (itemContainer.IsItemValid != true)
				{
					// 指定了缓存元素：
					if (isItemSpecifiedValid)
					{
						////////////////////////////////////////////////
						// !!!
						itemSpecified = DidWillUpdateItemCache(
							itemCacheKey,
							default,
							itemSpecified);
						// !!!
						////////////////////////////////////////////////
						if (itemSpecified == null
							&& IsNullValueValidToCache != true)
						{
							return default;
						}
						// !!!
						itemContainer.SetItem(
							itemSpecified,
							itemCacheCreateParam,
							true);
						////////////////////////////////////////////////
						// !!!
						DidItemCacheUpdated(
							itemCacheKey,
							default,
							itemSpecified);
						// !!!
						////////////////////////////////////////////////
					}
					// 异步创建缓存元素
					else if (isNeedCreateItemAsync)
					{
						////////////////////////////////////////////////
						// 1/2，将需要异步创建元素的容器，压入需要处理的队列。
						////////////////////////////////////////////////
						// !!!
						_itemContainersNeedCreateItemAsyncQueue.Enqueue(itemContainer);
						// !!!

						////////////////////////////////////////////////
						// 2/2，尝试开启异步创建元素的处理任务。
						////////////////////////////////////////////////
						var cancellationToken = _cancellationTokenSource.Token;
						_ = _tasksToCreateItemAsync.TryRun(
							() =>
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
								var toDidCreateItemsCache = ToDidCreateItemsCache;
								var toDidCreateItemCache = ToDidCreateItemCache;
								if (toDidCreateItemsCache != null)
								{
									// !!!
									DidWillCreateItemsCache(ref itemContainersNeedCreateItem);
									var itemContainersNeedCreateItemCount = itemContainersNeedCreateItem.Count;
									if (itemContainersNeedCreateItemCount > 0)
									// !!!
									{
										////////////////////////////////////////////////
										// !!!
										toDidCreateItemsCache(itemContainersNeedCreateItem);
										// !!!
										////////////////////////////////////////////////

										foreach (var itemContainerNeedCreateItem in itemContainersNeedCreateItem)
										{
											////////////////////////////////////////////////
											// !!!
											itemSpecified = DidWillUpdateItemCache(
												itemCacheKey,
												default,
												itemSpecified);
											// !!!
											////////////////////////////////////////////////
											if (itemSpecified != null
												|| IsNullValueValidToCache == true)
											{
												// !!!
												itemContainerNeedCreateItem.SetItem(
													itemContainerNeedCreateItem.Item,
													itemContainerNeedCreateItem.ItemCreateParam,
													true);
												// !!!
												////////////////////////////////////////////////
												// !!!
												DidItemCacheUpdated(
													itemContainerNeedCreateItem.Key,
													default,
													itemContainerNeedCreateItem.Item);
												// !!!
												////////////////////////////////////////////////
											}
										}
									}
								}
								else if (toDidCreateItemCache != null)
								{
									foreach (var itemContainerNeedCreateItem
									in
									itemContainersNeedCreateItem)
									{
										if (cancellationToken.IsCancellationRequested)
										{
											return;
										}

										if (DidWillCreateItemCache(
											itemContainerNeedCreateItem.Key,
											itemContainerNeedCreateItem.ItemCreateParam,
											out var newItem))
										{
											newItem = toDidCreateItemCache(
												itemContainerNeedCreateItem.Key,
												itemContainerNeedCreateItem.ItemCreateParam);
										}

										if (newItem != null
										|| IsNullValueValidToCache == true)
										{
											// !!!
											itemContainerNeedCreateItem.SetItem(
												newItem,
												itemContainerNeedCreateItem.ItemCreateParam,
												true);
											// !!!
											////////////////////////////////////////////////
											// !!!
											DidItemCacheUpdated(
												itemContainerNeedCreateItem.Key,
												default,
												newItem);
											// !!!
											////////////////////////////////////////////////
										}
									}
								}
								// !!!
								Interlocked.Add(
									ref _itemContainersCountInCreatingItemAsync,
									-1 * itemContainersCountInCreatingItemAsyncAddend);
								// !!!
							});
					}
					else if (ToDidCreateItemCache != null)
					{
						if (DidWillCreateItemCache(
							itemCacheKey,
							itemCacheCreateParam,
							out var newItem))
						{
							newItem = ToDidCreateItemCache(
								itemCacheKey,
								itemCacheCreateParam);
						}

						////////////////////////////////////////////////
						// !!!
						newItem = DidWillUpdateItemCache(
							itemCacheKey,
							default,
							newItem);
						// !!!
						////////////////////////////////////////////////

						if (newItem != null
							|| IsNullValueValidToCache)
						{
							// !!!
							itemContainer.SetItem(
								newItem,
								itemCacheCreateParam,
								true);
							// !!!
							////////////////////////////////////////////////
							// !!!
							DidItemCacheUpdated(
								itemCacheKey,
								default,
								newItem);
							// !!!
							////////////////////////////////////////////////
						}
					}
				}
			}
		}
		else if (itemContainer != null)
		{
			itemContainer.LastReadTime = DateTime.Now;
		}
		return itemContainer;
	}

	public bool TryGet(
		ItemKeyType itemCacheKey,
		out ItemType? item,
		out ItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?>? itemContainer,
		bool isNeedCreateItemAsync = false,
		ItemCacheCreateParamType? itemCacheCreateParam = default)
	{
		item = default;

		itemContainer = TryGet(
			itemCacheKey,
			isNeedCreateItemAsync,
			isNeedCreateItemAsync,
			itemCacheCreateParam,
			false,
			default);
		if (itemContainer == null
			|| itemContainer.IsItemValid == false)
		{
			return false;
		}
		//
		item = itemContainer.Item;
		//
		return true;
	}

	public bool TryGet(
		ItemKeyType itemCacheKey,
		out ItemType? item,
		bool isNeedCreateItemAsync = false,
		ItemCacheCreateParamType? itemCacheCreateParam = default)
	{
		return TryGet(
			itemCacheKey,
			out item,
			out _,
			isNeedCreateItemAsync,
			itemCacheCreateParam);
	}

	public ItemType? Get(
		ItemKeyType itemCacheKey,
		ItemCacheCreateParamType? itemCacheCreateParam,
		out ItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?>? itemContainer)
	{
		itemContainer = TryGet(
			itemCacheKey,
			true,
			false,
			itemCacheCreateParam,
			false,
			default);
		if (itemContainer == null)
		{
			return default;
		}
		return itemContainer.Item;
	}

	public ItemType? Get(
		ItemKeyType itemCacheKey,
		ItemCacheCreateParamType? itemCacheCreateParam)
	{
		return Get(
			itemCacheKey,
			itemCacheCreateParam,
			out _);
	}

	////////////////////////////////////////////////
	// @事件节点
	////////////////////////////////////////////////

	protected virtual void DidWillCreateItemsCache(
		ref List<ItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?>> itemCacheContainersNeedCreate)
	{ }

	protected virtual bool DidWillCreateItemCache(
		ItemKeyType itemKey,
		ItemCacheCreateParamType? itemCacheCreateParam,
		out ItemType? item)
	{
		// !!!
		item = default;
		// !!!
		return true;
	}
	protected virtual ItemType? DidWillUpdateItemCache(
		ItemKeyType itemKey,
		ItemType? lastItem,
		ItemType? currentItem)
	{
		return currentItem;
	}

	protected virtual void DidItemCacheUpdated(
		ItemKeyType itemKey,
		ItemType? lastItem,
		ItemType? currentItem)
	{
		if (ItemCacheIndexes is IItemCacheIndex<ItemType>[] itemCacheIndexes)
		{
			foreach (var itemCacheIndex in itemCacheIndexes)
			{
				itemCacheIndex.UpdateIndexItemsByUpdateItemFrom(
					lastItem,
					currentItem);
			}
		}

		////////////////////////////////////////////////
		// !!
		ToDidItemCacheUpdated?.Invoke(
				itemKey,
				lastItem,
				currentItem);
		// !!!
		////////////////////////////////////////////////
	}

	protected virtual void DidUpdateAllItemCache(
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
			if (cancellationToken.IsCancellationRequested)
			{
				return;
			}
			if (_itemContainersCache.TryGetValue(
				itemKey,
				out var itemContainer))
			{
				if (itemContainer != null)
				{
					if ((DateTime.Now - itemContainer.LastUpdateTime).TotalMilliseconds
					  >= noneUpdateSecondsToUpdateItemCache)
					{
						Update(itemKey);
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
