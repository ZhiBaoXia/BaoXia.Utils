﻿using BaoXia.Utils.Cache.Index.Interfaces;
using BaoXia.Utils.ConcurrentTools;
using BaoXia.Utils.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaoXia.Utils.Cache.Index;

public class ItemIndexWith3Keys<ItemType, PrimaryIndexKeyType, SecondaryIndexKeyType, ThirdaryIndexKeyType>(
	Func<ItemType, PrimaryIndexKeyType> toGetPrimaryIndexKeyOfItem,
	Func<ItemType, SecondaryIndexKeyType> toGetSecondaryIndexKeyOfItem,
	Func<ItemType, ThirdaryIndexKeyType> toGetThirdaryIndexKeyOfItem)
	//
	: IItemCacheIndex<ItemType>
	//
	where PrimaryIndexKeyType : notnull
	where SecondaryIndexKeyType : notnull
	where ThirdaryIndexKeyType : notnull
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public readonly ConcurrentDictionary<PrimaryIndexKeyType,
		ConcurrentDictionary<SecondaryIndexKeyType,
			ConcurrentDictionary<ThirdaryIndexKeyType, ItemIndexNode<ItemType>>>> PrimaryIndexes = new();

	private string? _name = null;
	public string? Name { get => _name; set => _name = value; }

	#endregion


	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public ConcurrentDictionary<SecondaryIndexKeyType,
		ConcurrentDictionary<ThirdaryIndexKeyType, ItemIndexNode<ItemType>>>? GetSecondaryIndexes(PrimaryIndexKeyType primaryIndexKey)
	{
		_ = PrimaryIndexes.TryGetValue(primaryIndexKey, out var secondaryIndexes);
		{ }
		return secondaryIndexes;
	}

	public ConcurrentDictionary<ThirdaryIndexKeyType, ItemIndexNode<ItemType>>? GetThirdaryIndexes(
		PrimaryIndexKeyType primaryIndexKey,
		SecondaryIndexKeyType secondaryIndexKey)
	{
		var secondaryIndexes = GetSecondaryIndexes(primaryIndexKey);
		if (secondaryIndexes == null)
		{
			return null;
		}
		_ = secondaryIndexes.TryGetValue(secondaryIndexKey, out var thirdaryIndexes);
		{ }
		return thirdaryIndexes;
	}

	public void UpdateIndexItemsWithPrimaryIndexKey(
		PrimaryIndexKeyType primaryIndexKey,
		SecondaryIndexKeyType secondaryIndexKey,
		ThirdaryIndexKeyType thirdaryIndexKey,
		Func<ItemType?, ItemType?> toUpdateIndexItem)
	{
		var secondaryIndexes
			= PrimaryIndexes.GetOrAdd(
				primaryIndexKey,
				(_) => []);
		var thirdaryIndexes
			= secondaryIndexes.GetOrAdd(
				secondaryIndexKey,
				(_) => []);
		var itemIndexInfo
			= thirdaryIndexes.GetOrAdd(
				thirdaryIndexKey,
				(_) => new());
		lock (itemIndexInfo)
		{
			// !!!
			var newIndexItem
				= toUpdateIndexItem(itemIndexInfo.FirstItem);
			newIndexItem = WillUpdateIndexItemWithPrimaryIndexKey(
				primaryIndexKey,
				secondaryIndexKey,
				thirdaryIndexKey,
				//
				newIndexItem);
			if (newIndexItem != null)
			{
				if (itemIndexInfo.Items.Length == 1)
				{
					// !!!
					itemIndexInfo.Items[0] = newIndexItem;
					// !!!
				}
				else
				{
					// !!!
					itemIndexInfo.Items = [newIndexItem];
					// !!!
				}
			}
			else
			{
				// !!!
				itemIndexInfo.Items = [];
				// !!!
			}
		}
	}

	public ItemType? GetItem(
		PrimaryIndexKeyType primaryIndexKey,
		SecondaryIndexKeyType secondaryIndexKey,
		ThirdaryIndexKeyType thirdaryIndexKey)
	{
		if (!PrimaryIndexes.TryGetValue(
			primaryIndexKey,
			out var secondaryIndexes))
		{
			return default;
		}
		if (!secondaryIndexes.TryGetValue(
			secondaryIndexKey,
			out var thirdaryIndexes))
		{
			return default;
		}
		if (thirdaryIndexes.TryGetValue(
			thirdaryIndexKey,
			out var enityIndexInfo))
		{
			return enityIndexInfo.FirstItem;
		}
		return default;
	}

	#endregion


	////////////////////////////////////////////////
	// @事件节点
	////////////////////////////////////////////////

	#region 事件节点

	protected virtual ItemType? WillUpdateIndexItemWithPrimaryIndexKey(
				PrimaryIndexKeyType primaryIndexKey,
				SecondaryIndexKeyType secondaryIndexKey,
				ThirdaryIndexKeyType thirdaryIndexKey,
				//
				ItemType? newIndexItem)
	{
		return newIndexItem;
	}


	#endregion


	////////////////////////////////////////////////
	// @实现”IDbSetMemoryCacheIndex“
	////////////////////////////////////////////////

	#region 实现”IDbSetMemoryCacheIndex“

	public async Task CreateIndexOfItemsAsync(
		IEnumerable<ItemType> items,
		int tasksCountToCreateRecordIndexes)
	{
		// 临时使用“List”容器，加速缓存建立：
		var tmpPrimaryIndexes
			= new ConcurrentDictionary<
				PrimaryIndexKeyType,
				ConcurrentDictionary<SecondaryIndexKeyType,
				ConcurrentDictionary<ThirdaryIndexKeyType, List<ItemType>>>>();
		var tmpIndexesCreateQueue
			= new ItemsConcurrentProcessQueue<ItemType>(tasksCountToCreateRecordIndexes);
		foreach (var item in items)
		{
			tmpIndexesCreateQueue.ProcessItem(
				item,
				(itemNeedProcess) =>
				{
					var primaryIndexKey = toGetPrimaryIndexKeyOfItem(itemNeedProcess);
					var secondaryIndexes = tmpPrimaryIndexes.GetOrAdd(
						primaryIndexKey,
						(_) => []);
					var secondaryIndexKey = toGetSecondaryIndexKeyOfItem(itemNeedProcess);
					var thirdaryIndexes = secondaryIndexes.GetOrAdd(
						secondaryIndexKey,
						(_) => []);
					var thirdaryIndexKey = toGetThirdaryIndexKeyOfItem(itemNeedProcess);
					var tmpItemIndexNodeBuffer = thirdaryIndexes.GetOrAdd(
						thirdaryIndexKey,
						(_) => []);
					////////////////////////////////////////////////
					// !!!
					lock (tmpItemIndexNodeBuffer)
					{
						tmpItemIndexNodeBuffer.Clear();
						tmpItemIndexNodeBuffer.Add(item);
					}
					// !!!
					////////////////////////////////////////////////
				});
		}
		////////////////////////////////////////////////
		// !!!
		await tmpIndexesCreateQueue.WhenAll();
		// !!!
		////////////////////////////////////////////////


		var primaryIndexKeyQueue
			= new ItemsConcurrentProcessQueue<PrimaryIndexKeyType>(tasksCountToCreateRecordIndexes);
		var allPrimaryIndexKeys = tmpPrimaryIndexes.Keys;
		foreach (var primaryIndexKey in allPrimaryIndexKeys)
		{
			var primaryIndexKeyNeedCreate = primaryIndexKey;
			primaryIndexKeyQueue.ProcessItem(
				primaryIndexKey,
				(primaryIndexKey) =>
				{
					if (tmpPrimaryIndexes.TryGetValue(
						primaryIndexKey,
						out var tmpSecondaryIndexes))
					{
						var secondaryIndexes
							= PrimaryIndexes.GetOrAdd(
								primaryIndexKeyNeedCreate,
								(_) => []);
						foreach (var tmpSecondaryIndexInfo in tmpSecondaryIndexes)
						{
							var secondaryIndexKey = tmpSecondaryIndexInfo.Key;
							var thirdaryIndexes = secondaryIndexes.GetOrAdd(
								secondaryIndexKey,
								(_) => []);
							var tmpThirdaryIndexes = tmpSecondaryIndexInfo.Value;
							foreach (var tmpThirdaryIndexInfo in tmpThirdaryIndexes)
							{
								////////////////////////////////////////////////
								// !!!
								thirdaryIndexes.AddOrUpdateWithNewValue(
									tmpThirdaryIndexInfo.Key,
									new([.. tmpThirdaryIndexInfo.Value]));
								// !!!
								////////////////////////////////////////////////
							}
						}
					}
				});
		}
		////////////////////////////////////////////////
		// !!!
		await primaryIndexKeyQueue.WhenAll();
		// !!!
		////////////////////////////////////////////////
	}

	public void UpdateIndexItemsByInsertItem(ItemType item)
	{
		UpdateIndexItemsWithPrimaryIndexKey(
			toGetPrimaryIndexKeyOfItem(item),
			toGetSecondaryIndexKeyOfItem(item),
			toGetThirdaryIndexKeyOfItem(item),
			(_) =>
			{
				return item;
			});
	}

	public void UpdateIndexItemsByUpdateItemFrom(ItemType lastItem, ItemType currentItem)
	{
		var lastPrimaryIndexKey = toGetPrimaryIndexKeyOfItem(lastItem);
		var lastSecondaryIndexKey = toGetSecondaryIndexKeyOfItem(lastItem);
		var lastThirdaryIndexKey = toGetThirdaryIndexKeyOfItem(lastItem);
		//
		var currentPrimaryIndexKey = toGetPrimaryIndexKeyOfItem(currentItem);
		var currentSecondaryIndexKey = toGetSecondaryIndexKeyOfItem(currentItem);
		var currentThirdaryIndexKey = toGetThirdaryIndexKeyOfItem(currentItem);

		////////////////////////////////////////////////
		// 1/2，移除旧的索引：
		////////////////////////////////////////////////
		if (!lastPrimaryIndexKey.Equals(currentPrimaryIndexKey)
			|| !lastSecondaryIndexKey.Equals(currentSecondaryIndexKey)
			|| !lastThirdaryIndexKey.Equals(currentThirdaryIndexKey))
		{
			UpdateIndexItemsWithPrimaryIndexKey(
				lastPrimaryIndexKey,
				lastSecondaryIndexKey,
				lastThirdaryIndexKey,
				//
				(_) =>
				{
					return default;
				});
		}

		////////////////////////////////////////////////
		// 2/2，更新新的索引，
		// 注意：此时可能实体的其他属性发生了变化，
		// 因此无论索引关键字是否发生变化，都应当进行更新：
		////////////////////////////////////////////////
		UpdateIndexItemsWithPrimaryIndexKey(
			currentPrimaryIndexKey,
			currentSecondaryIndexKey,
			currentThirdaryIndexKey,
			(_) =>
			{
				return currentItem;
			});
	}

	public void UpdateIndexItemsByRemoveItem(ItemType item)
	{
		UpdateIndexItemsWithPrimaryIndexKey(
			toGetPrimaryIndexKeyOfItem(item),
			toGetSecondaryIndexKeyOfItem(item),
			toGetThirdaryIndexKeyOfItem(item),
			(_) =>
			{
				return default;
			});
	}

	public void Clear()
	{
		PrimaryIndexes.Clear();
	}

	public ItemType? GetItem(ItemType item)
	{
		return GetItem(
			toGetPrimaryIndexKeyOfItem(item),
			toGetSecondaryIndexKeyOfItem(item),
			toGetThirdaryIndexKeyOfItem(item));
	}

	public bool IsItemExisted(ItemType item)
	{
		return GetItem(item) != null;
	}

	#endregion
}