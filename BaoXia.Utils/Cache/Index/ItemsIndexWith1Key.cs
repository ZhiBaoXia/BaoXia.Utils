﻿using BaoXia.Utils.Cache.Index.Interfaces;
using BaoXia.Utils.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

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
				(indexItems) =>
				{
					for (var indexItemIndex = 0;
					indexItemIndex < indexItems.Length;
					indexItemIndex++)
					{
						var indexItem = indexItems[indexItemIndex];
						if (toEqualItems(indexItem, lastItem!))
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
		if (isCurrentItemValid)
		{
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
						if (toEqualItems(indexItem, currentItem!))
						{
							// !!!
							indexItems[indexItemIndex] = currentItem!;
							break;
							// !!!
						}
					}
					if (indexItemIndex >= indexItems.Length)
					{
						indexItems = indexItems.ArrayByInsertWithOrderDescending(
							currentItem!,
							toGetOrderOfIndexItems);
					}
					return indexItems;
				});
		}
	}

	public void Clear()
	{
		PrimaryIndexes.Clear();
	}

	#endregion
}