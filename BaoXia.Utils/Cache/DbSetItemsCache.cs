using BaoXia.Utils.Constants;
using Microsoft.EntityFrameworkCore;
using System;

namespace BaoXia.Utils.Cache;

/// <summary>
/// 实体元素缓存。
/// </summary>
public class DbSetItemsCache<ItemGroupKeyType, ItemKeyType, ItemType>(
	    Func<ItemGroupKeyType, DbSet<ItemType>?, ItemType[]?> didCreateItemListCache,
	    Action<ItemGroupKeyType, ItemType[]?, ItemType[]?, ItemCacheOperation> didItemListUpdated,
	    Func<double> didGetNoneReadSecondsToRemoveItemListCache,
	    //
	    Func<ItemKeyType, DbSet<ItemType>?, ItemType?> didCreateItemCache,
	    Func<double> toDidGetNoneReadSecondsToRemoveItemCache,
	    Func<double>? toDidGetNoneUpdateSecondsToUpdateItemCache = null)
											where ItemGroupKeyType : notnull
											where ItemKeyType : notnull
											where ItemType : class
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	private readonly ListsCache<ItemGroupKeyType, ItemType, DbSet<ItemType>> _itemListsCache = new(
		    didCreateItemListCache,
		    didItemListUpdated,
		    didGetNoneReadSecondsToRemoveItemListCache);

	private readonly ItemsCache<ItemKeyType, ItemType, DbSet<ItemType>> _itemsCache = new(
		    didCreateItemCache,
		    null,
		    toDidGetNoneReadSecondsToRemoveItemCache,
		    toDidGetNoneUpdateSecondsToUpdateItemCache);

	#endregion
	#region 自身实现

	public DbSetItemsCache(
	    Func<ItemGroupKeyType, DbSet<ItemType>?, ItemType[]?> didCreateItemListCache,
	    Action<ItemGroupKeyType, ItemType[]?, ItemType[]?, ItemCacheOperation> didItemListUpdated,
	    //
	    Func<ItemKeyType, DbSet<ItemType>?, ItemType?> didCreateItemCache,
	    //
	    Func<double> didGetNoneReadSecondsToRemoveItemCache) : this(
		didCreateItemListCache,
		didItemListUpdated,
		didGetNoneReadSecondsToRemoveItemCache,
		//
		didCreateItemCache,
		didGetNoneReadSecondsToRemoveItemCache)
	{ }

	public ItemType[]? GetList(
	    ItemGroupKeyType itemGroupKey,
	    DbSet<ItemType> itemDbSet)
	{
		if (object.Equals(itemGroupKey, default(ItemGroupKeyType)))
		{
			return null;
		}

		var items = _itemListsCache.Get(itemGroupKey, itemDbSet);
		{ }
		return items;
	}

	public ItemType? Get(
	    ItemKeyType itemKey,
	    DbSet<ItemType> itemDbSet)
	{
		if (object.Equals(itemKey, default(ItemGroupKeyType)))
		{
			return null;
		}

		var item = _itemsCache.Get(itemKey, itemDbSet);
		{ }
		return item;
	}

	public ItemType[]? Add(
	    ItemGroupKeyType itemGroupKey,
	    ItemKeyType newItemKey,
	    ItemType newItem,
	    DbSet<ItemType> itemDbSet)
	{
		if (object.Equals(itemGroupKey, default(ItemGroupKeyType)))
		{
			return null;
		}

		var items = _itemListsCache.AddListItem(
		    itemGroupKey,
		    itemDbSet,
		    newItem);
		{
			_itemsCache.Add(
			    newItemKey,
			    newItem);
		}
		return items;
	}

	public ItemType[]? Remove(
	    ItemGroupKeyType itemGroupKey,
	    ItemKeyType newItemKey,
	    ItemType newItem,
	    DbSet<ItemType> itemDbSet)
	{
		if (object.Equals(itemGroupKey, default(ItemGroupKeyType)))
		{
			return null;
		}

		var items = _itemListsCache.RemoveListItem(
		    itemGroupKey,
		    itemDbSet,
		    newItem);
		{
			_itemsCache.Remove(
			    newItemKey);
		}
		return items;
	}

	#endregion
}
