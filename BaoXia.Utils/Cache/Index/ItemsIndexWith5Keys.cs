using BaoXia.Utils.Cache.Index.Interfaces;
using BaoXia.Utils.ConcurrentTools;
using BaoXia.Utils.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaoXia.Utils.Cache.Index;

public class ItemsIndexWith5Keys<ItemType, PrimaryIndexKeyType, SecondaryIndexKeyType, ThirdaryIndexKeyType, FourthIndexKeyType, FifthIndexKeyType>(
	Func<ItemType, PrimaryIndexKeyType> toGetPrimaryIndexKeyOfItem,
	Func<ItemType, SecondaryIndexKeyType> toGetSecondaryIndexKeyOfItem,
	Func<ItemType, ThirdaryIndexKeyType> toGetThirdaryIndexKeyOfItem,
	Func<ItemType, FourthIndexKeyType> toGetFourthIndexKeyOfItem,
	Func<ItemType, FifthIndexKeyType> toGetFifthIndexKeyOfItem,
	Func<ItemType, ItemType, bool> toEqualItems,
	Func<ItemType, ItemType, int> toGetOrderOfIndexItems)
	//
	: IItemCacheIndex<ItemType>
	//
	where PrimaryIndexKeyType : notnull
	where SecondaryIndexKeyType : notnull
	where ThirdaryIndexKeyType : notnull
	where FourthIndexKeyType : notnull
	where FifthIndexKeyType : notnull
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public readonly ConcurrentDictionary<PrimaryIndexKeyType,
		ConcurrentDictionary<SecondaryIndexKeyType,
			ConcurrentDictionary<ThirdaryIndexKeyType,
				ConcurrentDictionary<FourthIndexKeyType,
					ConcurrentDictionary<FifthIndexKeyType, ItemIndexNode<ItemType>>>>>> PrimaryIndexes = new();

	private string? _name = null;
	public string? Name { get => _name; set => _name = value; }

	#endregion


	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public ConcurrentDictionary<SecondaryIndexKeyType,
		ConcurrentDictionary<ThirdaryIndexKeyType,
			ConcurrentDictionary<FourthIndexKeyType,
				ConcurrentDictionary<FifthIndexKeyType, ItemIndexNode<ItemType>>>>>? GetSecondaryIndexes(PrimaryIndexKeyType primaryIndexKey)
	{
		_ = PrimaryIndexes.TryGetValue(primaryIndexKey, out var secondaryIndexes);
		{ }
		return secondaryIndexes;
	}

	public ConcurrentDictionary<ThirdaryIndexKeyType,
			ConcurrentDictionary<FourthIndexKeyType,
				ConcurrentDictionary<FifthIndexKeyType, ItemIndexNode<ItemType>>>>? GetThirdaryIndexes(
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

	public ConcurrentDictionary<FourthIndexKeyType,
				ConcurrentDictionary<FifthIndexKeyType, ItemIndexNode<ItemType>>>? GetFourthIndexes(
		PrimaryIndexKeyType primaryIndexKey,
		SecondaryIndexKeyType secondaryIndexKey,
		ThirdaryIndexKeyType thirdaryIndexKey)
	{
		var thirdaryIndexes = GetThirdaryIndexes(
			primaryIndexKey,
			secondaryIndexKey);
		if (thirdaryIndexes == null)
		{
			return null;
		}
		_ = thirdaryIndexes.TryGetValue(thirdaryIndexKey, out var fourthIndexes);
		{ }
		return fourthIndexes;
	}

	public ConcurrentDictionary<FifthIndexKeyType, ItemIndexNode<ItemType>>? GetFifthIndexes(
		PrimaryIndexKeyType primaryIndexKey,
		SecondaryIndexKeyType secondaryIndexKey,
		ThirdaryIndexKeyType thirdaryIndexKey,
		FourthIndexKeyType fourthIndexKey)
	{
		var fourthIndexes = GetFourthIndexes(
			primaryIndexKey,
			secondaryIndexKey,
			thirdaryIndexKey);
		if (fourthIndexes == null)
		{
			return null;
		}
		_ = fourthIndexes.TryGetValue(fourthIndexKey, out var fifthIndexes);
		{ }
		return fifthIndexes;
	}

	public void UpdateIndexItemsWithPrimaryIndexKey(
		PrimaryIndexKeyType primaryIndexKey,
		SecondaryIndexKeyType secondaryIndexKey,
		ThirdaryIndexKeyType thirdaryIndexKey,
		FourthIndexKeyType fourthIndexKey,
		FifthIndexKeyType fifthIndexKey,
		Func<ItemType[], ItemType[]?> toUpdateIndexItems)
	{
		var secondaryIndexes
			= PrimaryIndexes.GetOrAdd(
				primaryIndexKey,
				(_) => []);
		var thirdaryIndexes
			= secondaryIndexes.GetOrAdd(
				secondaryIndexKey,
				(_) => []);
		var fourthIndexes
			= thirdaryIndexes.GetOrAdd(
				thirdaryIndexKey,
				(_) => []);
		var fithIndexes
			= fourthIndexes.GetOrAdd(
				fourthIndexKey,
				(_) => []);
		var itemIndexInfo = fithIndexes.GetOrAdd(
			fifthIndexKey,
			(_) => new());
		lock (itemIndexInfo)
		{
			// !!!
			var newIndexItems
				= toUpdateIndexItems(itemIndexInfo.Items);
			// !!!
			itemIndexInfo.Items = WillUpdateIndexItemsWithPrimaryIndexKey(
				primaryIndexKey,
				secondaryIndexKey,
				thirdaryIndexKey,
				fourthIndexKey,
				fifthIndexKey,
				//
				newIndexItems)
				?? [];
			// !!!
		}
	}

	public ItemType[]? GetItems(
		PrimaryIndexKeyType primaryIndexKey,
		SecondaryIndexKeyType secondaryIndexKey,
		ThirdaryIndexKeyType thirdaryIndexKey,
		FourthIndexKeyType fourthIndexKey,
		FifthIndexKeyType fifthIndexKey)
	{
		if (!PrimaryIndexes.TryGetValue(
			primaryIndexKey,
			out var secondaryIndexes))
		{
			return null;
		}
		if (!secondaryIndexes.TryGetValue(
			secondaryIndexKey,
			out var thirdaryIndexes))
		{
			return null;
		}
		if (!thirdaryIndexes.TryGetValue(
			thirdaryIndexKey,
			out var fourthIndexes))
		{
			return null;
		}
		if (!fourthIndexes.TryGetValue(
			fourthIndexKey,
			out var fifthIndexes))
		{
			return null;
		}

		if (fifthIndexes.TryGetValue(
			fifthIndexKey,
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
				SecondaryIndexKeyType secondaryIndexKey,
				ThirdaryIndexKeyType thirdaryIndexKey,
				FourthIndexKeyType fourthIndexKey,
				FifthIndexKeyType fifthIndexKey,
				//
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
		SecondaryIndexKeyType lastSecondaryIndexKey = default!;
		ThirdaryIndexKeyType lastThirdaryIndexKey = default!;
		FourthIndexKeyType lastFourthIndexKey = default!;
		FifthIndexKeyType lastFifthIndexKey = default!;
		if (!EqualityComparer<ItemType>.Default.Equals(lastItem, default))
		{
			isLastItemValid = true;
			lastPrimaryIndexKey = toGetPrimaryIndexKeyOfItem(lastItem!);
			lastSecondaryIndexKey = toGetSecondaryIndexKeyOfItem(lastItem!);
			lastThirdaryIndexKey = toGetThirdaryIndexKeyOfItem(lastItem!);
			lastFourthIndexKey = toGetFourthIndexKeyOfItem(lastItem!);
			lastFifthIndexKey = toGetFifthIndexKeyOfItem(lastItem!);
		}
		//
		var isCurrentItemValid = false;
		PrimaryIndexKeyType currentPrimaryIndexKey = default!;
		SecondaryIndexKeyType currentSecondaryIndexKey = default!;
		ThirdaryIndexKeyType currentThirdaryIndexKey = default!;
		FourthIndexKeyType currentFourthIndexKey = default!;
		FifthIndexKeyType currentFifthIndexKey = default!;
		if (!EqualityComparer<ItemType>.Default.Equals(currentItem, default))
		{
			isCurrentItemValid = true;
			currentPrimaryIndexKey = toGetPrimaryIndexKeyOfItem(currentItem!);
			currentSecondaryIndexKey = toGetSecondaryIndexKeyOfItem(currentItem!);
			currentThirdaryIndexKey = toGetThirdaryIndexKeyOfItem(currentItem!);
			currentFourthIndexKey = toGetFourthIndexKeyOfItem(currentItem!);
			currentFifthIndexKey = toGetFifthIndexKeyOfItem(currentItem!);
		}

		////////////////////////////////////////////////
		// 1/2，移除旧的索引：
		////////////////////////////////////////////////
		if (isLastItemValid
			&& (!lastPrimaryIndexKey.Equals(currentPrimaryIndexKey)
			|| !lastSecondaryIndexKey.Equals(currentSecondaryIndexKey)
			|| !lastThirdaryIndexKey.Equals(currentThirdaryIndexKey)
			|| !lastFourthIndexKey.Equals(currentFourthIndexKey)
			|| !lastFifthIndexKey.Equals(currentFifthIndexKey)))
		{
			UpdateIndexItemsWithPrimaryIndexKey(
				lastPrimaryIndexKey,
				lastSecondaryIndexKey,
				lastThirdaryIndexKey,
				lastFourthIndexKey,
				lastFifthIndexKey,
				//
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

		////////////////////////////////////////////////
		// 2/2，更新新的索引，
		// 注意：此时可能实体的其他属性发生了变化，
		// 因此无论索引关键字是否发生变化，都应当进行更新：
		////////////////////////////////////////////////
		if (isCurrentItemValid)
		{
			UpdateIndexItemsWithPrimaryIndexKey(
				currentPrimaryIndexKey,
				currentSecondaryIndexKey,
				currentThirdaryIndexKey,
				currentFourthIndexKey,
				currentFifthIndexKey,
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