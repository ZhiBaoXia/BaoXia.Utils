using BaoXia.Utils.Cache.Index.Interfaces;
using BaoXia.Utils.ConcurrentTools;
using BaoXia.Utils.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaoXia.Utils.Cache.Index;

public class ItemIndexWith6Keys<ItemType,
	PrimaryIndexKeyType, SecondaryIndexKeyType, ThirdaryIndexKeyType,
	FourthIndexKeyType, FifthIndexKeyType, SixthIndexKeyType>(
	Func<ItemType, PrimaryIndexKeyType> toGetPrimaryIndexKeyOfItem,
	Func<ItemType, SecondaryIndexKeyType> toGetSecondaryIndexKeyOfItem,
	Func<ItemType, ThirdaryIndexKeyType> toGetThirdaryIndexKeyOfItem,
	Func<ItemType, FourthIndexKeyType> toGetFourthIndexKeyOfItem,
	Func<ItemType, FifthIndexKeyType> toGetFifthIndexKeyOfItem,
	Func<ItemType, SixthIndexKeyType> toGetSixthIndexKeyOfItem)
	//
	: IItemCacheIndex<ItemType>
	//
	where PrimaryIndexKeyType : notnull
	where SecondaryIndexKeyType : notnull
	where ThirdaryIndexKeyType : notnull
	where FourthIndexKeyType : notnull
	where FifthIndexKeyType : notnull
	where SixthIndexKeyType : notnull
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public readonly ConcurrentDictionary<PrimaryIndexKeyType,
		ConcurrentDictionary<SecondaryIndexKeyType,
			ConcurrentDictionary<ThirdaryIndexKeyType,
				ConcurrentDictionary<FourthIndexKeyType,
					ConcurrentDictionary<FifthIndexKeyType,
						ConcurrentDictionary<SixthIndexKeyType, ItemIndexNode<ItemType>>>>>>> PrimaryIndexes = new();

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
				ConcurrentDictionary<FifthIndexKeyType,
					ConcurrentDictionary<SixthIndexKeyType, ItemIndexNode<ItemType>>>>>>? GetSecondaryIndexes(PrimaryIndexKeyType primaryIndexKey)
	{
		_ = PrimaryIndexes.TryGetValue(primaryIndexKey, out var secondaryIndexes);
		{ }
		return secondaryIndexes;
	}

	public ConcurrentDictionary<ThirdaryIndexKeyType,
			ConcurrentDictionary<FourthIndexKeyType,
				ConcurrentDictionary<FifthIndexKeyType,
					ConcurrentDictionary<SixthIndexKeyType, ItemIndexNode<ItemType>>>>>? GetThirdaryIndexes(
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
				ConcurrentDictionary<FifthIndexKeyType,
					ConcurrentDictionary<SixthIndexKeyType, ItemIndexNode<ItemType>>>>? GetFourthIndexes(
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

	public ConcurrentDictionary<FifthIndexKeyType,
					ConcurrentDictionary<SixthIndexKeyType, ItemIndexNode<ItemType>>>? GetFifthIndexes(
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

	public ConcurrentDictionary<SixthIndexKeyType, ItemIndexNode<ItemType>>? GetSixthIndexes(
		PrimaryIndexKeyType primaryIndexKey,
		SecondaryIndexKeyType secondaryIndexKey,
		ThirdaryIndexKeyType thirdaryIndexKey,
		FourthIndexKeyType fourthIndexKey,
		FifthIndexKeyType fifthIndexKey)
	{
		var fifthIndexes = GetFifthIndexes(
			primaryIndexKey,
			secondaryIndexKey,
			thirdaryIndexKey,
			fourthIndexKey);
		if (fifthIndexes == null)
		{
			return null;
		}
		_ = fifthIndexes.TryGetValue(fifthIndexKey, out var sixthIndexes);
		{ }
		return sixthIndexes;
	}

	public void UpdateIndexItemsWithPrimaryIndexKey(
		PrimaryIndexKeyType primaryIndexKey,
		SecondaryIndexKeyType secondaryIndexKey,
		ThirdaryIndexKeyType thirdaryIndexKey,
		FourthIndexKeyType fourthIndexKey,
		FifthIndexKeyType fifthIndexKey,
		SixthIndexKeyType sixthIndexKey,
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
		var fourthIndexes
			= thirdaryIndexes.GetOrAdd(
				thirdaryIndexKey,
				(_) => []);
		var fifthIndexes
			= fourthIndexes.GetOrAdd(
				fourthIndexKey,
				(_) => []);
		var sixthIndexes
			= fifthIndexes.GetOrAdd(
				fifthIndexKey,
				(_) => []);
		var itemIndexInfo = sixthIndexes.GetOrAdd(
			sixthIndexKey,
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
				fourthIndexKey,
				fifthIndexKey,
				sixthIndexKey,
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
		ThirdaryIndexKeyType thirdaryIndexKey,
		FourthIndexKeyType fourthIndexKey,
		FifthIndexKeyType fifthIndexKey,
		SixthIndexKeyType sixthIndexKey)
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
		if (!thirdaryIndexes.TryGetValue(
			thirdaryIndexKey,
			out var fourthIndexes))
		{
			return default;
		}
		if (!fourthIndexes.TryGetValue(
			fourthIndexKey,
			out var fifthIndexes))
		{
			return default;
		}
		if (!fifthIndexes.TryGetValue(
			fifthIndexKey,
			out var sixthIndexes))
		{
			return default;
		}
		if (sixthIndexes.TryGetValue(
			sixthIndexKey,
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
				FourthIndexKeyType fourthIndexKey,
				FifthIndexKeyType fifthIndexKey,
				SixthIndexKeyType sixthIndexKey,
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
		SecondaryIndexKeyType lastSecondaryIndexKey = default!;
		ThirdaryIndexKeyType lastThirdaryIndexKey = default!;
		FourthIndexKeyType lastFourthIndexKey = default!;
		FifthIndexKeyType lastFifthIndexKey = default!;
		SixthIndexKeyType lastSixthIndexKey = default!;
		if (!EqualityComparer<ItemType>.Default.Equals(lastItem, default))
		{
			isLastItemValid = true;
			lastPrimaryIndexKey = toGetPrimaryIndexKeyOfItem(lastItem!);
			lastSecondaryIndexKey = toGetSecondaryIndexKeyOfItem(lastItem!);
			lastThirdaryIndexKey = toGetThirdaryIndexKeyOfItem(lastItem!);
			lastFourthIndexKey = toGetFourthIndexKeyOfItem(lastItem!);
			lastFifthIndexKey = toGetFifthIndexKeyOfItem(lastItem!);
			lastSixthIndexKey = toGetSixthIndexKeyOfItem(lastItem!);
		}
		//
		var isCurrentItemValid = false;
		PrimaryIndexKeyType currentPrimaryIndexKey = default!;
		SecondaryIndexKeyType currentSecondaryIndexKey = default!;
		ThirdaryIndexKeyType currentThirdaryIndexKey = default!;
		FourthIndexKeyType currentFourthIndexKey = default!;
		FifthIndexKeyType currentFifthIndexKey = default!;
		SixthIndexKeyType currentSixthIndexKey = default!;
		if (!EqualityComparer<ItemType>.Default.Equals(currentItem, default))
		{
			isCurrentItemValid = true;
			currentPrimaryIndexKey = toGetPrimaryIndexKeyOfItem(currentItem!);
			currentSecondaryIndexKey = toGetSecondaryIndexKeyOfItem(currentItem!);
			currentThirdaryIndexKey = toGetThirdaryIndexKeyOfItem(currentItem!);
			currentFourthIndexKey = toGetFourthIndexKeyOfItem(currentItem!);
			currentFifthIndexKey = toGetFifthIndexKeyOfItem(currentItem!);
			currentSixthIndexKey = toGetSixthIndexKeyOfItem(currentItem!);
		}

		////////////////////////////////////////////////
		// 1/2，移除旧的索引：
		////////////////////////////////////////////////
		if (isLastItemValid
			&& (!lastPrimaryIndexKey.Equals(currentPrimaryIndexKey)
			|| !lastSecondaryIndexKey.Equals(currentSecondaryIndexKey)
			|| !lastThirdaryIndexKey.Equals(currentThirdaryIndexKey)
			|| !lastFourthIndexKey.Equals(currentFourthIndexKey)
			|| !lastFifthIndexKey.Equals(currentFifthIndexKey)
			|| !lastSixthIndexKey.Equals(currentSixthIndexKey)))
		{
			UpdateIndexItemsWithPrimaryIndexKey(
				lastPrimaryIndexKey,
				lastSecondaryIndexKey,
				lastThirdaryIndexKey,
				lastFourthIndexKey,
				lastFifthIndexKey,
				lastSixthIndexKey,
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
				currentSecondaryIndexKey,
				currentThirdaryIndexKey,
				currentFourthIndexKey,
				currentFifthIndexKey,
				currentSixthIndexKey,
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
		return GetItem(
			toGetPrimaryIndexKeyOfItem(item),
			toGetSecondaryIndexKeyOfItem(item),
			toGetThirdaryIndexKeyOfItem(item),
			toGetFourthIndexKeyOfItem(item),
			toGetFifthIndexKeyOfItem(item),
			toGetSixthIndexKeyOfItem(item));
	}

	public bool IsItemExisted(ItemType item)
	{
		return GetItem(item) != null;
	}

	#endregion
}