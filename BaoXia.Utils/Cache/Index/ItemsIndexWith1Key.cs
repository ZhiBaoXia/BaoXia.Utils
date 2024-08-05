using BaoXia.Utils.Cache.Index.Interfaces;
using BaoXia.Utils.ConcurrentTools;
using BaoXia.Utils.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaoXia.Utils.Cache.Index;

public class ItemsIndexWith1Key<ItemType, PrimaryIndexKeyType>(
	Func<ItemType, PrimaryIndexKeyType> toGetPrimaryIndexKeyOfItem,
	Func<ItemType, ItemType, bool> toEqualItems,
	Func<ItemType, ItemType, int> toGetOrderOfIndexItems)
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
		Func<ItemType[], ItemType[]?> toUpdateIndexItems)
	{
		var itemIndexInfo
			= PrimaryIndexes.GetOrAdd(
				primaryIndexKey,
				(_) => new());
		lock (itemIndexInfo)
		{
			// !!!
			var newIndexItems
				= toUpdateIndexItems(itemIndexInfo.Items);
			// !!!
			itemIndexInfo.Items = WillUpdateIndexItemsWithPrimaryIndexKey(
				primaryIndexKey,
				//
				newIndexItems)
				?? [];
			// !!!
		}
	}

	public ItemType[]? GetItems(
		PrimaryIndexKeyType primaryIndexKey)
	{
		if (PrimaryIndexes.TryGetValue(
			primaryIndexKey,
			out var enityIndexInfo))
		{
			return enityIndexInfo.Items;
		}
		return null;
	}

	#endregion


	////////////////////////////////////////////////
	// @事件节点
	////////////////////////////////////////////////

	#region 事件节点

	protected virtual ItemType[]? WillUpdateIndexItemsWithPrimaryIndexKey(
				PrimaryIndexKeyType primaryIndexKey,
				ItemType[]? newIndexItems)
	{
		return newIndexItems;
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
						tmpItemIndexNodeBuffer.InsertWithOrderDescending(
							item,
							toGetOrderOfIndexItems);
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
			(indexItems) =>
			{
				indexItems = indexItems.ArrayByInsertWithOrder(
					item,
					toGetOrderOfIndexItems);
				{ }
				return indexItems;
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
				(indexItems) =>
				{
					for (var indexItemIndex = 0;
					indexItemIndex < indexItems.Length;
					indexItemIndex++)
					{
						var indexItem = indexItems[indexItemIndex];
						if (toEqualItems(indexItem, lastItem))
						{
							// !!!
							indexItems = indexItems.ArrayByRemoveAt(indexItemIndex);
							break;
							// !!!
						}
					}
					return indexItems;
				});
		}


		// 2/2，更新新的索引，
		// 注意：此时可能实体的其他属性发生了变化，
		// 因此无论索引关键字是否发生变化，都应当进行更新：
		UpdateIndexItemsWithPrimaryIndexKey(
			currentPrimaryIndexKey,
			(indexItems) =>
			{
				var indexItemIndex = 0;
				for (;
				indexItemIndex < indexItems.Length;
				indexItemIndex++)
				{
					var indexItem = indexItems[indexItemIndex];
					if (toEqualItems(indexItem, currentItem))
					{
						// !!!
						indexItems[indexItemIndex] = currentItem;
						break;
						// !!!
					}
				}
				if (indexItemIndex >= indexItems.Length)
				{
					indexItems = indexItems.ArrayByInsertWithOrderDescending(
						currentItem,
						toGetOrderOfIndexItems);
				}
				return indexItems;
			});
	}

	public void UpdateIndexItemsByRemoveItem(ItemType item)
	{
		UpdateIndexItemsWithPrimaryIndexKey(
			toGetPrimaryIndexKeyOfItem(item),
			(indexItems) =>
			{
				for (var indexItemIndex = 0;
				indexItemIndex < indexItems.Length;
				indexItemIndex++)
				{
					var indexItem = indexItems[indexItemIndex];
					if (toEqualItems(indexItem, item))
					{
						// !!!
						indexItems = indexItems.ArrayByRemoveAt(indexItemIndex);
						break;
						// !!!
					}
				}
				return indexItems;
			});
	}

	public void Clear()
	{
		PrimaryIndexes.Clear();
	}

	#endregion
}