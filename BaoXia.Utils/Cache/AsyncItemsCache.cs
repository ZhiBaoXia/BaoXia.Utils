using BaoXia.Utils.ConcurrentTools;
using BaoXia.Utils.Constants;
using BaoXia.Utils.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Cache
{
        public class AsyncItemsCache<ItemKeyType, ItemType, ItemCacheCreateParamType> : IDisposable
                where ItemKeyType : notnull
                where ItemType : class
        {

                ////////////////////////////////////////////////
                // @静态常量
                ////////////////////////////////////////////////

                #region 静态常量

                protected class WillCreateItemCacheResult
                {
                        public bool IsItemCacheCreateContinue { get; set; }
                        public ItemType? ItemCreated { get; set; }

                        public WillCreateItemCacheResult(
                                bool isItemCacheCreateContinue,
                                ItemType? itemCreated)
                        {
                                IsItemCacheCreateContinue = isItemCacheCreateContinue;
                                ItemCreated = itemCreated;
                        }
                }

                #endregion


                ////////////////////////////////////////////////
                // @自身属性
                ////////////////////////////////////////////////

                protected CancellationTokenSource _cancellationTokenSource = new();

                protected readonly ConcurrentDictionary<
                        ItemKeyType,
                        AsyncItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?>>
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

                public ICollection<AsyncItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?>> ValueContainers
                {
                        get
                        {
                                return _itemContainersCache.Values;
                        }
                }

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

                public AsyncItemsCache(
                        Func<ItemKeyType, ItemCacheCreateParamType?, Task<ItemType?>> didCreateItemCacheAsync,
                        Func<ItemKeyType, ItemType?, ItemType?, ItemCacheOperation, Task<ItemType?>>? didItemCacheUpdatedAsync,
                        Func<double>? toDidGetIntervalSecondsToCleanItemCache,
                        Func<double>? toDidGetNoneReadSecondsToRemoveItemCache,
                        Func<double>? toDidGetNoneUpdateSecondsToUpdateItemCache,
                        Func<int>? toDidGetThreadsCountToCreateItemAsync)
                {
                        this.ToDidCreateItemCacheAsync = didCreateItemCacheAsync;
                        this.ToDidItemCacheUpdatedAsync = didItemCacheUpdatedAsync;
                        this.ToDidGetIntervalSecondsToCleanItemCache = toDidGetIntervalSecondsToCleanItemCache;
                        this.ToDidGetNoneReadSecondsToRemoveItemCache = toDidGetNoneReadSecondsToRemoveItemCache;
                        this.ToDidGetNoneUpdateSecondsToUpdateItemCache = toDidGetNoneUpdateSecondsToUpdateItemCache;

                        _tasksToCreateItemAsync = new Tasks(
                                toDidGetThreadsCountToCreateItemAsync);
                }

                public AsyncItemsCache(
                        Func<ItemKeyType, ItemCacheCreateParamType?, Task<ItemType?>> didCreateItemCache,
                        Func<ItemKeyType, ItemType?, ItemType?, ItemCacheOperation, Task<ItemType?>>? didItemCacheUpdatedAsync,
                        Func<double>? toDidGetIntervalAndNoneReadSecondsToRemoveItemCache,
                        Func<double>? toDidGetNoneUpdateSecondsToUpdateItemCache = null,
                        Func<int>? toDidGetThreadsCountToCreateItemAsync = null)
                        : this(didCreateItemCache,
                                  didItemCacheUpdatedAsync,
                                  toDidGetIntervalAndNoneReadSecondsToRemoveItemCache,
                                  toDidGetIntervalAndNoneReadSecondsToRemoveItemCache,
                                  toDidGetNoneUpdateSecondsToUpdateItemCache,
                                  toDidGetThreadsCountToCreateItemAsync)
                { }

                ~AsyncItemsCache()
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
                                ItemCacheOperation.Create);
                        if (itemNeedAdd == null
                                && this.IsNullValueValidToCache != true)
                        {
                                return default;
                        }
                        ////////////////////////////////////////////////

                        var now = DateTime.Now;
                        var newItemContainer
                                = new AsyncItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?>(
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


                public async Task<ItemType?> SetAsync(
                        ItemKeyType itemKey,
                        ItemType? item,
                        ItemCacheCreateParamType? itemCreateParam = default,
                        bool isNeedUpdateItemLastReadTime = true)
                {
                        return await this.AddAsync(
                                itemKey,
                                item,
                                itemCreateParam,
                                isNeedUpdateItemLastReadTime);
                }

                public async Task<ItemType?> UpdateAsync(
                        ItemKeyType itemKey,
                        ItemCacheCreateParamType? itemCreateParam = default,
                        bool isNeedUpdateItemLastReadTime = true)
                {
                        if (typeof(ItemKeyType).IsPointer
                                && itemKey == null)
                        {
                                return default;
                        }

                        AsyncItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?>?
                                itemContainer = null;
                        if (itemCreateParam == null)
                        {
                                if (_itemContainersCache.TryGetValue(itemKey, out itemContainer))
                                {
                                        // !!!
                                        itemCreateParam = itemContainer.ItemCreateParam;
                                        // !!!
                                }
                        }

                        var willCreateItemCacheAsyncResult
                                = await DidWillCreateItemCacheAsync(
                                itemKey,
                                itemCreateParam);
                        var newItem = willCreateItemCacheAsyncResult.ItemCreated;
                        var toDidCreateItemCacheAsync = this.ToDidCreateItemCacheAsync;
                        if (willCreateItemCacheAsyncResult.IsItemCacheCreateContinue
                                && toDidCreateItemCacheAsync != null)
                        {
                                newItem = await toDidCreateItemCacheAsync(
                                        itemKey,
                                        itemCreateParam);
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
                        return await this.AddAsync(itemKey, newItem); ;
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

                protected async Task<AsyncItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?>?> TryGetAsync(
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
                                                = new AsyncItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?>(
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
                                                        _itemContainersCountInCreatingItemAsync = itemContainersNeedCreateItem.Count;
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
                                                                        // !!!
                                                                        await toDidCreateItemsCacheAsync(itemContainersNeedCreateItem);
                                                                        // !!!
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
                                                                        var item = willCreateItemCacheAsyncResult.ItemCreated;
                                                                        if (willCreateItemCacheAsyncResult.IsItemCacheCreateContinue)
                                                                        {
                                                                                item
                                                                                = await toDidCreateItemCacheAsync(
                                                                                        itemContainerNeedCreateItem.Key,
                                                                                        itemContainerNeedCreateItem.ItemCreateParam);
                                                                        }
                                                                        itemContainerNeedCreateItem.SetItem(
                                                                                item,
                                                                                itemContainerNeedCreateItem.ItemCreateParam,
                                                                                true);
                                                                }
                                                        }
                                                        // !!!
                                                        _itemContainersCountInCreatingItemAsync = 0;
                                                        // !!!
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

                public async Task<AsyncTryGetItemResult<ItemKeyType, ItemType?, ItemCacheCreateParamType?>> TryGetAsync(
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
                                return new AsyncTryGetItemResult<ItemKeyType, ItemType?, ItemCacheCreateParamType?>
                                {
                                        IsGotSucess = false
                                };
                        }
                        return new AsyncTryGetItemResult<ItemKeyType, ItemType?, ItemCacheCreateParamType?>
                        {
                                IsGotSucess = true,
                                itemContainer = itemContainer,
                                Item = itemContainer.Item
                        };
                }

                public async Task<AsyncItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?>?> GetItemContainerAsync(
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

#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
                protected virtual async Task<List<ItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?>>> DidWillCreateItemsCacheAsync(List<ItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?>> itemCacheContainersNeedCreate)
#pragma warning restore CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
                {
                        return itemCacheContainersNeedCreate;
                }

#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
                protected virtual async Task<WillCreateItemCacheResult> DidWillCreateItemCacheAsync(
                        ItemKeyType itemKey,
                        ItemCacheCreateParamType? itemCacheCreateParam)
#pragma warning restore CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
                {
                        return new WillCreateItemCacheResult(true, default);
                }

                protected virtual Task<Task<ItemType?>> DidCreateTaskToCreateItemForItemContainer(
                        AsyncItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?> itemContainer,
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
}
