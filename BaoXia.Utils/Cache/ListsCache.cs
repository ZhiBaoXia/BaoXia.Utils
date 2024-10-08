﻿using BaoXia.Utils.Extensions;
using System;

namespace BaoXia.Utils.Cache;

/// <summary>
/// 列表缓存。
/// </summary>
public class ListsCache<ListKeyType, ListItemType, CreateListCacheParamType>(
	    Func<ListKeyType, CreateListCacheParamType?, ListItemType[]?> didCreateList,
	    Func<ListKeyType, ListItemType[]?, ListItemType[]?, CreateListCacheParamType?, ListItemType[]?>? didWillUpdateList,
	    Action<ListKeyType, ListItemType[]?, ListItemType[]?, CreateListCacheParamType?>? didListUpdated,
	    Func<double>? toDidGetIntervalSecondsToCleanItemCache,
	    Func<double>? didGetNoneReadSecondsToRemoveListCache,
	    Func<double>? didGetNoneUpdateSecondsToUpdateItemCache,
	    Func<int>? toDidGetThreadsCountToCreateItemAsync)
	: ItemsCache<ListKeyType, ListItemType[], CreateListCacheParamType>(didCreateList,
		didWillUpdateList,
		didListUpdated,
		  toDidGetIntervalSecondsToCleanItemCache,
		  didGetNoneReadSecondsToRemoveListCache,
		  didGetNoneUpdateSecondsToUpdateItemCache,
		  toDidGetThreadsCountToCreateItemAsync)
	    where ListKeyType : notnull
{
	////////////////////////////////////////////////
	// @静态常量
	////////////////////////////////////////////////

	#region 静态常量

	protected enum ItemOperation
	{
		None,
		InsertOrUpdate,
		Remove
	}

	#endregion


	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public ListsCache(
		Func<ListKeyType, CreateListCacheParamType?, ListItemType[]?> didCreateList,
		Func<ListKeyType, ListItemType[]?, ListItemType[]?, CreateListCacheParamType?, ListItemType[]?>? didWillUpdateList,
		Action<ListKeyType, ListItemType[]?, ListItemType[]?, CreateListCacheParamType?>? didListUpdated,
		Func<double>? toDidGetIntervalAndNoneReadSecondsToRemoveItemCache,
		Func<double>? toDidGetNoneUpdateSecondsToUpdateItemCache = null,
		Func<int>? toDidGetThreadsCountToCreateItemAsync = null)
		: this(didCreateList,
			  didWillUpdateList,
			  didListUpdated,
			  toDidGetIntervalAndNoneReadSecondsToRemoveItemCache,
			  toDidGetIntervalAndNoneReadSecondsToRemoveItemCache,
			  toDidGetNoneUpdateSecondsToUpdateItemCache,
			  toDidGetThreadsCountToCreateItemAsync)
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
			var listElementListExisted = newList[listElementListExistedIndex]!;
			if (listElementListExisted.Equals(newItem))
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

	public ListItemType[]? AddListItem(
	    ListKeyType listKey,
	    CreateListCacheParamType? createListParam,
	    ListItemType newListItem,
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
		var listContainerNeedAddItem = TryGet(
		    listKey,
		    true,
		    false,
		    createListParam,
		    false,
		    default,
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
				currentList ??= [];
			}
			currentList
				= RecreateListWithItemOperation(
			    currentList,
			    newListItem,
			    ItemOperation.InsertOrUpdate);

			////////////////////////////////////////////////
			// 3/3，触发列表更新事件。
			////////////////////////////////////////////////

			////////////////////////////////////////////////
			// !!!
			currentList = DidWillUpdateItemCache(
				listKey,
				lastList,
				currentList,
				createListParam);
			// !!!
			////////////////////////////////////////////////

			if (currentList != null
				|| IsNullValueValidToCache)
			{
				// !!!
				listContainerNeedAddItem.SetItem(
					currentList,
					createListParam,
					isNeedUpdateItemLastReadTime);
				// !!!

				////////////////////////////////////////////////
				// !!!
				DidItemCacheUpdated(
					listKey,
					lastList,
					currentList,
					createListParam);
				// !!!
				////////////////////////////////////////////////
			}
		}
		return currentList;
	}

	public ListItemType[]? RemoveListItem(
	    ListKeyType listKey,
	    CreateListCacheParamType? createListParam,
	    ListItemType item)
	{
		if (typeof(ListKeyType).IsPointer
		    && listKey == null)
		{
			return null;
		}

		////////////////////////////////////////////////
		// 1/4，尝试获取列表对象（容器）。
		////////////////////////////////////////////////
		ListItemType[]? currentList = null;
		if (_itemContainersCache.TryGetValue(
		    listKey,
		    out var listContainer)
			&& listContainer != null)
		{
			////////////////////////////////////////////////
			// 2/4，排队更新列表对象（容器）。
			////////////////////////////////////////////////
			lock (listContainer)
			{
				currentList = Get(listKey, createListParam);
				if (currentList != null)
				{
					var lastList = currentList;
					currentList = RecreateListWithItemOperation(
					    currentList,
					    item,
					    ItemOperation.Remove);

					////////////////////////////////////////////////
					// 3/4，更新列表缓存。
					////////////////////////////////////////////////
					// !!!
					Update(listKey, currentList, createListParam);
					// !!!
				}
			}
		}
		return currentList;
	}

	#endregion
}
