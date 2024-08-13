using BaoXia.Utils.Constants;
using BaoXia.Utils.Extensions;
using System;

namespace BaoXia.Utils.Cache;

/// <summary>
/// 列表缓存。
/// </summary>
public class ListsCache<ListKeyType, ListItemType, CreateListCacheParamType>(
	    Func<ListKeyType, CreateListCacheParamType?, ListItemType[]?> didCreateList,
	    Action<ListKeyType, ListItemType[]?, ListItemType[]?>? didListUpdated,
	    Func<double>? toDidGetIntervalSecondsToCleanItemCache,
	    Func<double>? didGetNoneReadSecondsToRemoveListCache,
	    Func<double>? didGetNoneUpdateSecondsToUpdateItemCache,
	    Func<int>? toDidGetThreadsCountToCreateItemAsync)
	: ItemsCache<ListKeyType, ListItemType[], CreateListCacheParamType>(didCreateList,
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
	    Action<ListKeyType, ListItemType[]?, ListItemType[]?>? didListUpdated,
		Func<double>? toDidGetIntervalAndNoneReadSecondsToRemoveItemCache,
		Func<double>? toDidGetNoneUpdateSecondsToUpdateItemCache = null,
		Func<int>? toDidGetThreadsCountToCreateItemAsync = null)
		: this(didCreateList,
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
		var listContainerNeedAddItem = this.TryGet(
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
				currentList ??= [];
			}
			currentList
				= this.RecreateListWithItemOperation(
			    currentList,
			    newListItem,
			    ItemOperation.InsertOrUpdate);

			////////////////////////////////////////////////
			// 3/3，触发列表更新事件。
			////////////////////////////////////////////////
			// !!!
			listContainerNeedAddItem.SetItem(
				currentList,
				createListParam,
				isNeedUpdateItemLastReadTime);
			// !!!
			DidItemCacheUpdated(
				listKey,
				lastList,
				currentList);
			// !!!
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
				currentList = this.Get(listKey, createListParam);
				if (currentList != null)
				{
					var lastList = currentList;
					currentList = this.RecreateListWithItemOperation(
					    currentList,
					    item,
					    ItemOperation.Remove);
		
					////////////////////////////////////////////////
					// 3/4，更新列表缓存。
					////////////////////////////////////////////////
					// !!!
					this.Add(listKey, currentList);
					// !!!

					////////////////////////////////////////////////
					// 4/4，触发列表更新事件。
					////////////////////////////////////////////////
					DidItemCacheUpdated(
						listKey,
						lastList,
						currentList);
					// !!!
				}
			}
		}
		return currentList;
	}

	#endregion
}
