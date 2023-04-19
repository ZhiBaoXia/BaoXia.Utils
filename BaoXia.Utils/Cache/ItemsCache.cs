using BaoXia.Utils.ConcurrentTools;
using BaoXia.Utils.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BaoXia.Utils.Cache
{
        public class ItemsCache<ItemKeyType, ItemType, ItemCacheCreateParamType> : IDisposable
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
                                if (itemContainers?.Any() == true)
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

                public bool IsNullValueValidToCache { get; set; } = false;

                public Func<ItemKeyType, ItemCacheCreateParamType?, ItemType?> ToDidCreateItemCache { get; set; }

                public Action<IEnumerable<ItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?>>>? ToDidCreateItemsCache { get; set; }

                public Func<ItemKeyType, ItemType?, ItemType?, ItemType?>? ToDidItemCacheUpdated { get; set; }



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
                                                                        if (cancellationToken.IsCancellationRequested)
                                                                        {

                                                                        }
                                                                        return true;
                                                                },
                                                                _toDidGetIntervalSecondsToCleanItemCache);
                                                        }
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
                                                        if (_taskToAutoUpdate == null)
                                                        {
                                                                _taskToAutoUpdate = new LoopTask(
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
                                                                        this.UpdateAllItems(cancellationToken);
                                                                        // !!!
#if DEBUG
									System.Diagnostics.Trace.WriteLine(myType + "，自动更新缓存元素任务，结束。");
#endif

                                                                        return true;
                                                                },
                                                                this.ToDidGetNoneUpdateSecondsToUpdateItemCache);
                                                        }
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

                public ItemsCache(
                        Func<ItemKeyType, ItemCacheCreateParamType?, ItemType?> didCreateItemCache,
                        Func<ItemKeyType, ItemType?, ItemType?, ItemType?>? didItemCacheUpdated,
                        Func<double>? toDidGetIntervalSecondsToCleanItemCache,
                        Func<double>? toDidGetNoneReadSecondsToRemoveItemCache,
                        Func<double>? toDidGetNoneUpdateSecondsToUpdateItemCache,
                        Func<int>? toDidGetThreadsCountToCreateItemAsync)
                {
                        this.ToDidCreateItemCache = didCreateItemCache;
                        this.ToDidItemCacheUpdated = didItemCacheUpdated;
                        this.ToDidGetIntervalSecondsToCleanItemCache = toDidGetIntervalSecondsToCleanItemCache;
                        this.ToDidGetNoneReadSecondsToRemoveItemCache = toDidGetNoneReadSecondsToRemoveItemCache;
                        this.ToDidGetNoneUpdateSecondsToUpdateItemCache = toDidGetNoneUpdateSecondsToUpdateItemCache;

                        _tasksToCreateItemAsync = new Tasks(
                                toDidGetThreadsCountToCreateItemAsync);
                }

                public ItemsCache(
                        Func<ItemKeyType, ItemCacheCreateParamType?, ItemType?> didCreateItemCache,
                        Func<ItemKeyType, ItemType?, ItemType?, ItemType?>? didItemCacheUpdated,
                        Func<double>? toDidGetIntervalAndNoneReadSecondsToRemoveItemCache,
                        Func<double>? toDidGetNoneUpdateSecondsToUpdateItemCache = null,
                        Func<int>? toDidGetThreadsCountToCreateItemAsync = null)
                        : this(didCreateItemCache,
                                  didItemCacheUpdated,
                                  toDidGetIntervalAndNoneReadSecondsToRemoveItemCache,
                                  toDidGetIntervalAndNoneReadSecondsToRemoveItemCache,
                                  toDidGetNoneUpdateSecondsToUpdateItemCache,
                                  toDidGetThreadsCountToCreateItemAsync)
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

                        ////////////////////////////////////////////////
                        var itemNeedAdd = this.DidItemCacheUpdated(
                                itemKey,
                                default,
                                item);
                        if (itemNeedAdd == null
                                && this.IsNullValueValidToCache != true)
                        {
                                return default;
                        }
                        ////////////////////////////////////////////////

                        var now = DateTime.Now;
                        var newItemContainer
                                = new ItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?>(
                                        itemKey,
                                        itemNeedAdd,
                                        true,
                                        itemCreateParam,
                                        now,
                                        now);
                        var itemContainer
                                = _itemContainersCache.AddOrUpdateWithNewValue(
                                        itemKey,
                                        newItemContainer);
                        if (itemContainer != null
                                && itemContainer != newItemContainer)
                        {
                                itemContainer.SetItem(
                                        itemNeedAdd,
                                        itemCreateParam,
                                        isNeedUpdateItemLastReadTime);
                        }
                        return itemNeedAdd;
                }

                public ItemType? Update(
                        ItemKeyType itemKey,
                        ItemCacheCreateParamType? itemCreateParam = default,
                        bool isNeedUpdateItemLastReadTime = true)
                {
                        if (typeof(ItemKeyType).IsPointer
                                && itemKey == null)
                        {
                                return default;
                        }
                        if (this.ToDidCreateItemCache == null)
                        {
                                return default;
                        }

                        ItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?>? itemContainer = null;
                        if (itemCreateParam == null)
                        {
                                if (_itemContainersCache.TryGetValue(itemKey, out itemContainer)
                                        && itemContainer != null)
                                {
                                        // !!!
                                        itemCreateParam = itemContainer.ItemCreateParam;
                                        // !!!
                                }
                        }

                        var newItem = this.ToDidCreateItemCache(
                                        itemKey,
                                        itemCreateParam);
                        if (itemContainer != null)
                        {
                                ////////////////////////////////////////////////
                                newItem = this.DidItemCacheUpdated(
                                        itemKey,
                                        itemContainer.Item,
                                        newItem);
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
                        return this.Add(itemKey, newItem);
                }

                public ItemType? Remove(ItemKeyType itemKey)
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
                        this.DidItemCacheUpdated(
                                itemKey,
                                itemRemoved,
                                default);
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
                        this.Clean(this.NoneReadSecondsToRemoveItemCache, cancellationToken);
                }

                public void UpdateAllItems(
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
                                this.DidUpdate(noneUpdateSecondsToUpdateItemCache, cancellationToken);
                        }
                }

                public void UpdateAllItems(CancellationToken cancellationToken)
                {
                        this.UpdateAllItems(this.NoneUpdateSecondsToUpdateItemCache, cancellationToken);
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
                                                if (isItemSpecifiedValid)
                                                {
                                                        itemContainer.SetItem(
                                                                itemSpecified,
                                                                itemCacheCreateParam,
                                                                true);
                                                }
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
                                                                        Interlocked.Add(
                                                                                ref _itemContainersCountInCreatingItemAsync,
                                                                                itemContainersNeedCreateItem.Count);
                                                                        // !!!
                                                                        var toDidCreateItemsCache = this.ToDidCreateItemsCache;
                                                                        var toDidCreateItemCache = this.ToDidCreateItemCache;
                                                                        if (toDidCreateItemsCache != null)
                                                                        {
                                                                                // !!!
                                                                                toDidCreateItemsCache(itemContainersNeedCreateItem);
                                                                                // !!!
                                                                                Interlocked.Add(
                                                                                        ref _itemContainersCountInCreatingItemAsync,
                                                                                        -1 * itemContainersNeedCreateItem.Count);
                                                                                // !!!
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

                                                                                        var item
                                                                                        = toDidCreateItemCache(
                                                                                                itemContainerNeedCreateItem.Key,
                                                                                                itemContainerNeedCreateItem.ItemCreateParam);
                                                                                        // !!!
                                                                                        itemContainerNeedCreateItem.SetItem(
                                                                                                item,
                                                                                                itemContainerNeedCreateItem.ItemCreateParam,
                                                                                                true);
                                                                                        // !!!
                                                                                        Interlocked.Decrement(ref _itemContainersCountInCreatingItemAsync);
                                                                                        // !!!
                                                                                }
                                                                        }
                                                                });
                                                }
                                                else if (this.ToDidCreateItemCache != null)
                                                {
                                                        var newItem = this.ToDidCreateItemCache(
                                                                itemCacheKey,
                                                                itemCacheCreateParam);
                                                        if (newItem != null
                                                                || this.IsNullValueValidToCache)
                                                        {
                                                                // !!!
                                                                itemContainer.SetItem(
                                                                        newItem,
                                                                        itemCacheCreateParam,
                                                                        true);
                                                                // !!!
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

                        itemContainer = this.TryGet(
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
                        return this.TryGet(
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
                        itemContainer = this.TryGet(
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
                        return this.Get(
                                itemCacheKey,
                                itemCacheCreateParam,
                                out _);
                }

                ////////////////////////////////////////////////
                // @事件节点
                ////////////////////////////////////////////////

                public virtual ItemType? DidItemCacheUpdated(
                        ItemKeyType itemKey,
                        ItemType? lastItem,
                        ItemType? currentItem)
                {
                        if (this.ToDidItemCacheUpdated != null)
                        {
                                return this.ToDidItemCacheUpdated(
                                        itemKey,
                                        lastItem,
                                        currentItem);
                        }
                        return currentItem;
                }

                public virtual void DidUpdate(
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
                                                        this.Update(itemKey);
                                                }
                                        }
                                }
                        }
                }
                public virtual void DidClean(
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
}
