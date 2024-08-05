using BaoXia.Utils.Cache.Index.Interfaces;
using BaoXia.Utils.ConcurrentTools;
using BaoXia.Utils.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaoXia.Utils.Cache.Index;

public class ItemIndexWith1Key<ItemType, PrimaryIndexKeyType>(
	Func<ItemType, PrimaryIndexKeyType> toGetPrimaryIndexKeyOfItem)
	//
	: IItemCacheIndex<ItemType>
	//
	where PrimaryIndexKeyType : notnull
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public readonly ConcurrentDictionary<PrimaryIndexKeyType, ItemIndexNode<ItemType>> PrimaryIndexes = new();

	private string? _name = null;
	public string? Name { get => _name; set => _name = value; }

	#endregion


	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public void UpdateIndexItemsWithPrimaryIndexKey(
		PrimaryIndexKeyType primaryIndexKey,
		Func<ItemType?, ItemType?> toUpdateIndexItem)
	{
		var itemIndexInfo
			= PrimaryIndexes.GetOrAdd(
				primaryIndexKey,
				(_) => new());
		lock (itemIndexInfo)
		{
			// !!!
			var newIndexItem
				= toUpdateIndexItem(itemIndexInfo.FirstItem);
			newIndexItem = WillUpdateIndexItemWithPrimaryIndexKey(
				primaryIndexKey,
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

	public ItemType? GetItem(PrimaryIndexKeyType primaryIndexKey)
	{
		if (PrimaryIndexes.TryGetValue(
			primaryIndexKey,
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
				PrimaryIndexKeyType, List<ItemType>>();
		var tmpIndexesCreateQueue
			= new ItemsConcurrentProcessQueue<ItemType>(tasksCountToCreateRecordIndexes);
		foreach (var item in items)
		{
			tmpIndexesCreateQueue.ProcessItem(
				item,
				(itemNeedProcess) =>
				{
					var primaryIndexKey = toGetPrimaryIndexKeyOfItem(itemNeedProcess);
					var tmpItemIndexNodeBuffer = tmpPrimaryIndexes.GetOrAdd(
						primaryIndexKey,
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
						out var tmpItemIndexNodeBuffer))
					{
						////////////////////////////////////////////////
						// !!!
						PrimaryIndexes.AddOrUpdateWithNewValue(
							primaryIndexKey,
							new([.. tmpItemIndexNodeBuffer]));
						// !!!
						////////////////////////////////////////////////
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
			(_) =>
			{
				return item;
			});
	}

	public void UpdateIndexItemsByUpdateItemFrom(ItemType lastItem, ItemType currentItem)
	{
		var lastPrimaryIndexKey = toGetPrimaryIndexKeyOfItem(lastItem);
		//
		var currentPrimaryIndexKey = toGetPrimaryIndexKeyOfItem(currentItem);

		////////////////////////////////////////////////
		// 1/2，移除旧的索引：
		////////////////////////////////////////////////
		if (!lastPrimaryIndexKey.Equals(currentPrimaryIndexKey))
		{
			UpdateIndexItemsWithPrimaryIndexKey(
				lastPrimaryIndexKey,
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
			(_) =>
			{
				return currentItem;
			});
	}

	public void UpdateIndexItemsByRemoveItem(ItemType item)
	{
		UpdateIndexItemsWithPrimaryIndexKey(
			toGetPrimaryIndexKeyOfItem(item),
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
		return GetItem(toGetPrimaryIndexKeyOfItem(item));
	}

	public bool IsItemExisted(ItemType item)
	{
		return GetItem(item) != null;
	}

	#endregion
}