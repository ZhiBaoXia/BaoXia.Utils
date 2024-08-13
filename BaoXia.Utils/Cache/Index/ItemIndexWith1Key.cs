using BaoXia.Utils.Cache.Index.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

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

	public void UpdateIndexItemsByUpdateItemFrom(
		ItemType? lastItem,
		ItemType? currentItem)
	{
		var isLastItemValid = false;
		PrimaryIndexKeyType lastPrimaryIndexKey = default!;
		if (!EqualityComparer<ItemType>.Default.Equals(lastItem, default))
		{
			isLastItemValid = true;
			lastPrimaryIndexKey = toGetPrimaryIndexKeyOfItem(lastItem!);
		}
		//
		var isCurrentItemValid = false;
		PrimaryIndexKeyType currentPrimaryIndexKey = default!;
		if (!EqualityComparer<ItemType>.Default.Equals(currentItem, default))
		{
			isCurrentItemValid = true;
			currentPrimaryIndexKey = toGetPrimaryIndexKeyOfItem(currentItem!);
		}


		////////////////////////////////////////////////
		// 1/2，移除旧的索引：
		////////////////////////////////////////////////
		if (isLastItemValid
			&& !lastPrimaryIndexKey.Equals(currentPrimaryIndexKey))
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
		if (isCurrentItemValid)
		{
			UpdateIndexItemsWithPrimaryIndexKey(
				currentPrimaryIndexKey,
				(_) =>
				{
					return currentItem;
				});
		}
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