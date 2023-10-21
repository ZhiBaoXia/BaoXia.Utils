using BaoXia.Utils.Constants;
using BaoXia.Utils.Extensions;
using System;
using System.Threading.Tasks;

namespace BaoXia.Utils.Cache
{
        /// <summary>
        /// 列表缓存。
        /// </summary>
        public class AsyncListsCache<ListKeyType, ListItemType, CreateListCacheParamType>
                : AsyncItemsCache<ListKeyType, ListItemType[], CreateListCacheParamType>
                    where ListKeyType : notnull
        {
                ////////////////////////////////////////////////
                // @静态常量
                ////////////////////////////////////////////////

                protected enum ItemOperation
                {
                        None,
                        InsertOrUpdate,
                        Remove
                }

                ////////////////////////////////////////////////
                // @自身属性
                ////////////////////////////////////////////////


                ////////////////////////////////////////////////
                // @自身实现
                ////////////////////////////////////////////////

                public AsyncListsCache(
                    Func<ListKeyType, CreateListCacheParamType?, Task<ListItemType[]?>> didCreateList,
                    Func<ListKeyType, ListItemType[]?, ListItemType[]?, ItemCacheOperation, ListItemType[]?>? didListUpdated,
                    Func<double>? toDidGetIntervalSecondsToCleanItemCache,
                    Func<double>? toDidGetNoneReadSecondsToRemoveListCache,
                    Func<double>? toDidGetNoneUpdateSecondsToUpdateItemCache,
                    Func<int>? toDidGetThreadsCountToCreateItemAsync)
                    : base(didCreateList,
                          didListUpdated,
                          toDidGetIntervalSecondsToCleanItemCache,
                          toDidGetNoneReadSecondsToRemoveListCache,
                          toDidGetNoneUpdateSecondsToUpdateItemCache,
                          toDidGetThreadsCountToCreateItemAsync)
                { }

                public AsyncListsCache(
                        Func<ListKeyType, CreateListCacheParamType?, Task<ListItemType[]?>> didCreateList,
                        Func<ListKeyType, ListItemType[]?, ListItemType[]?, ItemCacheOperation, ListItemType[]?>? didListUpdated,
                        Func<double>? toDidGetIntervalAndNoneReadSecondsToRemoveItemCache,
                        Func<double>? toDidGetNoneUpdateSecondsToUpdateItemCache = null,
                        Func<int>? toDidGetThreadsCountToCreateItemAs = null)
                        : this(didCreateList,
                                  didListUpdated,
                                  toDidGetIntervalAndNoneReadSecondsToRemoveItemCache,
                                  toDidGetIntervalAndNoneReadSecondsToRemoveItemCache,
                                  toDidGetNoneUpdateSecondsToUpdateItemCache,
                                  toDidGetThreadsCountToCreateItemAs)
                { }

                protected ListItemType[] RecreateListWithItemOperation(
                    ListItemType[] list,
                    ListItemType newItem,
                    ItemOperation newItemOperation)
                {
                        if (newItemOperation == ItemOperation.None)
                        {
                                return list;
                        }

                        ListItemType[] newList = list;
                        var objectListItemIndex = -1;
                        for (var listElementListExistedIndex = newList.Length - 1;
                            listElementListExistedIndex >= 0;
                            listElementListExistedIndex--)
                        {
                                var listElementListExisted = newList[listElementListExistedIndex];
                                if (listElementListExisted?.Equals(newItem) == true)
                                {
                                        //
                                        objectListItemIndex = listElementListExistedIndex;
                                        //
                                        break;
                                }
                        }

                        if (objectListItemIndex >= 0)
                        {
                                if (newItemOperation == ItemOperation.InsertOrUpdate)
                                {
                                        newList[objectListItemIndex] = newItem;
                                }
                                else if (newItemOperation == ItemOperation.Remove)
                                {
                                        // !!!
                                        newList = newList.ArrayByRemoveAt(objectListItemIndex);
                                        // !!!
                                }
                        }
                        else if (newItemOperation == ItemOperation.InsertOrUpdate)
                        {
                                // !!!
                                newList = newList.ArrayByAdd(newItem);
                                // !!!
                        }
                        return newList;
                }

                public async Task<ListItemType[]?> AddListItemAsync(
                    ListKeyType listKey,
                    CreateListCacheParamType? createListParam,
                    ListItemType item,
                    bool isNeedUpdateItemLastReadTime = true)
                {
                        if (typeof(ListKeyType).IsPointer
                            && listKey == null)
                        {
                                return null;
                        }

                        ////////////////////////////////////////////////
                        // 1/3，尝试获取列表对象（容器）。
                        ////////////////////////////////////////////////

                        var listContainerNeedAddItem = await this.TryGetAsync(
                            listKey,
                            true,
                            false,
                            createListParam,
                            false,
                            default);
                        if (listContainerNeedAddItem == null)
                        {
                                return null;
                        }

                        ////////////////////////////////////////////////
                        // 2/3，排队更新列表对象（容器）。
                        ////////////////////////////////////////////////

                        ListItemType[]? currentList;
                        lock (listContainerNeedAddItem)
                        {
                                currentList = listContainerNeedAddItem.Item;
                                var lastList = currentList;
                                {
                                        currentList ??= Array.Empty<ListItemType>();
                                }
                                currentList
                                        = this.RecreateListWithItemOperation(
                                    currentList,
                                    item,
                                    ItemOperation.InsertOrUpdate);

                                ////////////////////////////////////////////////
                                // 3/3，触发列表更新事件。
                                ////////////////////////////////////////////////
                                var toDidItemCacheUpdated = this.ToDidItemCacheUpdated;
                                if (toDidItemCacheUpdated != null)
                                {
                                        currentList = toDidItemCacheUpdated(
                                                listKey,
                                                lastList,
                                                currentList,
                                                ItemCacheOperation.Update);
                                }

                                // !!!
                                listContainerNeedAddItem.SetItem(
                                        currentList,
                                        createListParam,
                                        isNeedUpdateItemLastReadTime);
                                // !!!
                        }
                        return currentList;
                }

                public async Task<ListItemType[]?> RemoveListItemAsync(
                    ListKeyType listKey,
                    CreateListCacheParamType? createListParam,
                    ListItemType item,
                    bool isNeedUpdateItemLastReadTime = true)
                {
                        if (typeof(ListKeyType).IsPointer
                            && listKey == null)
                        {
                                return null;
                        }

                        ////////////////////////////////////////////////
                        // 1/4，尝试获取列表对象（容器）。
                        ////////////////////////////////////////////////
                        var listContainerNeedRemoveItem = await this.TryGetAsync(
                            listKey,
                            true,
                            false,
                            createListParam,
                            false,
                            default);
                        if (listContainerNeedRemoveItem == null
                            || listContainerNeedRemoveItem.Item == null)
                        {
                                return null;
                        }

                        ////////////////////////////////////////////////
                        // 2/4，排队更新列表对象（容器）。
                        ////////////////////////////////////////////////
                        lock (listContainerNeedRemoveItem)
                        {
                                var currentList = listContainerNeedRemoveItem.Item;
                                if (currentList != null)
                                {
                                        var lastList = currentList;
                                        currentList = this.RecreateListWithItemOperation(
                                            currentList,
                                            item,
                                            ItemOperation.Remove);

                                        ////////////////////////////////////////////////
                                        // 3/4，触发列表更新事件。
                                        ////////////////////////////////////////////////
                                        var toDidItemCacheUpdated = this.ToDidItemCacheUpdated;
                                        if (toDidItemCacheUpdated != null)
                                        {
                                                currentList = toDidItemCacheUpdated(
                                                        listKey,
                                                        lastList,
                                                        currentList,
                                                        ItemCacheOperation.Update);
                                        }

                                        ////////////////////////////////////////////////
                                        // 4/4，更新列表缓存。
                                        ////////////////////////////////////////////////
                                        listContainerNeedRemoveItem.SetItem(
                                                currentList,
                                                createListParam,
                                                isNeedUpdateItemLastReadTime);
                                }
                                return currentList;
                        }
                }
        }
}
