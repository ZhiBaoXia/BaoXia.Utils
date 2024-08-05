using BaoXia.Utils.Cache.Index.Interfaces;
using BaoXia.Utils.ConcurrentTools;
using BaoXia.Utils.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaoXia.Utils.Cache.Index;

public class ItemIndexWith2Keys<ItemType, PrimaryIndexKeyType, SecondaryIndexKeyType>(
	Func<ItemType, PrimaryIndexKeyType> toGetPrimaryIndexKeyOfItem,
	Func<ItemType, SecondaryIndexKeyType> toGetSecondaryIndexKeyOfItem)
	//
	: IItemCacheIndex<ItemType>
	//
	where PrimaryIndexKeyType : notnull
	where SecondaryIndexKeyType : notnull
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public readonly ConcurrentDictionary<PrimaryIndexKeyType,
		ConcurrentDictionary<SecondaryIndexKeyType, ItemIndexNode<ItemType>>> PrimaryIndexes = new();

	public string? Name { get; set; }

	#endregion


	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public ConcurrentDictionary<SecondaryIndexKeyType, ItemIndexNode<ItemType>>? GetSecondaryIndexes(PrimaryIndexKeyType primaryIndexKey)
	{
		_ = PrimaryIndexes.TryGetValue(primaryIndexKey, out var secondaryIndexes);
		{ }
		return secondaryIndexes;
	}

	public void UpdateIndexItemsWithPrimaryIndexKey(
		PrimaryIndexKeyType primaryIndexKey,
		SecondaryIndexKeyType secondaryIndexKey,
		Func<ItemType?, ItemType?> toUpdateIndexItem)
	{
		var secondaryIndexes
			= PrimaryIndexes.GetOrAdd(
				primaryIndexKey,
				(_) => []);
		var itemIndexInfo
			= secondaryIndexes.GetOrAdd(
				secondaryIndexKey,
				(_) => new());
		lock (itemIndexInfo)
		{
			// !!!
			var newIndexItem
				= toUpdateIndexItem(itemIndexInfo.FirstItem);
			newIndexItem = WillUpdateIndexItemWithPrimaryIndexKey(
				primaryIndexKey,
				secondaryIndexKey,
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
		SecondaryIndexKeyType secondaryIndexKey)
	{
		if (!PrimaryIndexes.TryGetValue(
			primaryIndexKey,
			out var secondaryIndexes))
		{
			return default;
		}
		if (secondaryIndexes.TryGetValue(
			secondaryIndexKey,
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
				ConcurrentDictionary<SecondaryIndexKeyType, List<ItemType>>>();
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
					var tmpItemIndexNodeBuffer = secondaryIndexes.GetOrAdd(
						secondaryIndexKey,
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
							////////////////////////////////////////////////
							// !!!
							secondaryIndexes.AddOrUpdateWithNewValue(
								tmpSecondaryIndexInfo.Key,
								new([.. tmpSecondaryIndexInfo.Value]));
							// !!!
							////////////////////////////////////////////////
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
			(_) =>
			{
				return item;
			});
	}

	public void UpdateIndexItemsByUpdateItemFrom(ItemType lastItem, ItemType currentItem)
	{
		var lastPrimaryIndexKey = toGetPrimaryIndexKeyOfItem(lastItem);
		var lastSecondaryIndexKey = toGetSecondaryIndexKeyOfItem(lastItem);
		//
		var currentPrimaryIndexKey = toGetPrimaryIndexKeyOfItem(currentItem);
		var currentSecondaryIndexKey = toGetSecondaryIndexKeyOfItem(currentItem);

		////////////////////////////////////////////////
		// 1/2，移除旧的索引：
		////////////////////////////////////////////////
		if (!lastPrimaryIndexKey.Equals(currentPrimaryIndexKey)
			|| !lastSecondaryIndexKey.Equals(currentSecondaryIndexKey))
		{
			UpdateIndexItemsWithPrimaryIndexKey(
				lastPrimaryIndexKey,
				lastSecondaryIndexKey,
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
			toGetSecondaryIndexKeyOfItem(item));
	}

	public bool IsItemExisted(ItemType item)
	{
		return GetItem(item) != null;
	}

	#endregion
}