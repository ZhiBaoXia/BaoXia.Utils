using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BaoXia.Utils.Cache
{
	/// <summary>
	/// 实体元素缓存。
	/// </summary>
	public class DbSetItemsCacheAsync<ItemGroupKeyType, ItemKeyType, ItemType>(
		    Func<ItemGroupKeyType, DbSet<ItemType>?, Task<ItemType[]?>> didCreateItemListCache,
		    Func<ItemGroupKeyType, ItemType[]?, ItemType[]?, DbSet<ItemType>?, Task<ItemType[]?>> didWillUpdateItemListAsync,
		    Func<ItemGroupKeyType, ItemType[]?, ItemType[]?, DbSet<ItemType>?, Task> didItemListUpdatedAsync,
		    Func<double>? toDidGetIntervalSecondsToCleanItemListCache,
		    Func<double> didGetNoneReadSecondsToRemoveItemListCache,
		    //
		    Func<ItemKeyType, DbSet<ItemType>?, Task<ItemType?>> didCreateItemCache,
		    Func<double>? toDidGetIntervalSecondsToCleanItemCache,
		    Func<double> toDidGetNoneReadSecondsToRemoveItemCache)
		where ItemGroupKeyType : notnull
		where ItemKeyType : notnull
		where ItemType : class
	{
		////////////////////////////////////////////////
		// @自身属性
		////////////////////////////////////////////////

		#region 自身属性

		private readonly ListsCachAsync<ItemGroupKeyType, ItemType, DbSet<ItemType>> _itemListsCache = new(
			    didCreateItemListCache,
			    didWillUpdateItemListAsync,
			    didItemListUpdatedAsync,
			    toDidGetIntervalSecondsToCleanItemListCache,
			    didGetNoneReadSecondsToRemoveItemListCache);

		private readonly ItemsCacheAsync<ItemKeyType, ItemType, DbSet<ItemType>> _itemsCache = new(
			    didCreateItemCache,
			    null,
			    null,
			    toDidGetIntervalSecondsToCleanItemCache,
			    toDidGetNoneReadSecondsToRemoveItemCache);

		#endregion


		////////////////////////////////////////////////
		// @自身实现
		////////////////////////////////////////////////

		#region 自身实现

		public DbSetItemsCacheAsync(
		    Func<ItemGroupKeyType, DbSet<ItemType>?, Task<ItemType[]?>> didCreateItemListCache,
		    Func<ItemGroupKeyType, ItemType[]?, ItemType[]?, DbSet<ItemType>?, Task<ItemType[]?>> didWillUpdateItemListAsync,
		    Func<ItemGroupKeyType, ItemType[]?, ItemType[]?, DbSet<ItemType>?, Task> didItemListUpdatedAsync,
		    //
		    Func<double>? toDidGetIntervalSecondsToCleanItemListCache,
		    Func<ItemKeyType, DbSet<ItemType>?, Task<ItemType?>> didCreateItemCache,
		    //
		    Func<double>? toDidGetIntervalSecondsToCleanItemCache,
		    Func<double> didGetNoneReadSecondsToRemoveItemCache) : this(
			didCreateItemListCache,
			didWillUpdateItemListAsync,
			didItemListUpdatedAsync,
			toDidGetIntervalSecondsToCleanItemListCache,
			didGetNoneReadSecondsToRemoveItemCache,
			//
			didCreateItemCache,
			toDidGetIntervalSecondsToCleanItemCache,
			didGetNoneReadSecondsToRemoveItemCache)
		{ }

		public async Task<ItemType[]?> GetList(
		    ItemGroupKeyType itemGroupKey,
		    DbSet<ItemType> itemDbSet)
		{
			if (object.Equals(itemGroupKey, default(ItemGroupKeyType)))
			{
				return null;
			}

			var items = await _itemListsCache.GetAsync(itemGroupKey, itemDbSet);
			{ }
			return items;
		}

		public async Task<ItemType?> Get(
		    ItemKeyType itemKey,
		    DbSet<ItemType> itemDbSet)
		{
			if (object.Equals(itemKey, default(ItemGroupKeyType)))
			{
				return null;
			}

			var item = await _itemsCache.GetAsync(itemKey, itemDbSet);
			{ }
			return item;
		}

		public async Task<ItemType[]?> AddAsync(
		    ItemGroupKeyType itemGroupKey,
		    ItemKeyType newItemKey,
		    ItemType newItem,
		    DbSet<ItemType> itemDbSet)
		{
			if (object.Equals(itemGroupKey, default(ItemGroupKeyType)))
			{
				return null;
			}

			ItemType[]? items = await _itemListsCache.AddListItemAsync(
			    itemGroupKey,
			    itemDbSet,
			    newItem);
			{
				await _itemsCache.AddAsync(
					newItemKey,
					newItem);
			}
			return items;
		}

		public async Task<ItemType[]?> RemoveAsync(
		    ItemGroupKeyType itemGroupKey,
		    ItemKeyType newItemKey,
		    ItemType newItem,
		    DbSet<ItemType> itemDbSet)
		{
			if (object.Equals(itemGroupKey, default(ItemGroupKeyType)))
			{
				return null;
			}

			var items = await _itemListsCache.RemoveListItemAsync(
			    itemGroupKey,
			    itemDbSet,
			    newItem);
			{
				await _itemsCache.RemoveAsync(newItemKey);
			}
			return items;
		}
	}

	#endregion

}
