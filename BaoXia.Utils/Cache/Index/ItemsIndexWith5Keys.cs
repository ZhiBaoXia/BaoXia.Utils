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

	public async Task CreateIndexOfItemsAsync(
		IEnumerable<ItemType> items,
		int tasksCountToCreateRecordIndexes)
	{
		// 临时使用“List”容器，加速缓存建立：
		var tmpPrimaryIndexes
			= new ConcurrentDictionary<
				PrimaryIndexKeyType,
				ConcurrentDictionary<SecondaryIndexKeyType,
				ConcurrentDictionary<ThirdaryIndexKeyType,
				ConcurrentDictionary<FourthIndexKeyType,
				ConcurrentDictionary<FifthIndexKeyType, List<ItemType>>>>>>();
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
					var fourthIndexes = thirdaryIndexes.GetOrAdd(
						thirdaryIndexKey,
						(_) => []);
					var fourthIndexKey = toGetFourthIndexKeyOfItem(itemNeedProcess);
					var fifthIndexes = fourthIndexes.GetOrAdd(
						fourthIndexKey,
						(_) => []);
					var fifthIndexKey = toGetFifthIndexKeyOfItem(itemNeedProcess);
					var tmpItemIndexNodeBuffer = fifthIndexes.GetOrAdd(
						fifthIndexKey,
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
								var thirdaryIndexKey = tmpThirdaryIndexInfo.Key;
								var fourthIndexes = thirdaryIndexes.GetOrAdd(
									thirdaryIndexKey,
									(_) => []);
								var tmpFourthIndexes = tmpThirdaryIndexInfo.Value;
								foreach (var tmpFourthIndexInfo in tmpFourthIndexes)
								{
									var fourthIndexKey = tmpFourthIndexInfo.Key;
									var fifthIndexes = fourthIndexes.GetOrAdd(
										fourthIndexKey,
										(_) => []);
									var tmpFifthIndexes = tmpFourthIndexInfo.Value;
									foreach (var tmpFifthIndexInfo in tmpFifthIndexes)
									{
										////////////////////////////////////////////////
										// !!!
										fifthIndexes.AddOrUpdateWithNewValue(
											tmpFifthIndexInfo.Key,
											new([.. tmpFifthIndexInfo.Value]));
										// !!!
										////////////////////////////////////////////////
									}
								}
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
			toGetFourthIndexKeyOfItem(item),
			toGetFifthIndexKeyOfItem(item),
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
		var lastSecondaryIndexKey = toGetSecondaryIndexKeyOfItem(lastItem);
		var lastThirdaryIndexKey = toGetThirdaryIndexKeyOfItem(lastItem);
		var lastFourthIndexKey = toGetFourthIndexKeyOfItem(lastItem);
		var lastFifthIndexKey = toGetFifthIndexKeyOfItem(lastItem);
		//
		var currentPrimaryIndexKey = toGetPrimaryIndexKeyOfItem(currentItem);
		var currentSecondaryIndexKey = toGetSecondaryIndexKeyOfItem(currentItem);
		var currentThirdaryIndexKey = toGetThirdaryIndexKeyOfItem(currentItem);
		var currentFourthIndexKey = toGetFourthIndexKeyOfItem(currentItem);
		var currentFifthIndexKey = toGetFifthIndexKeyOfItem(currentItem);

		////////////////////////////////////////////////
		// 1/2，移除旧的索引：
		////////////////////////////////////////////////
		if (!lastPrimaryIndexKey.Equals(currentPrimaryIndexKey)
			|| !lastSecondaryIndexKey.Equals(currentSecondaryIndexKey)
			|| !lastThirdaryIndexKey.Equals(currentThirdaryIndexKey)
			|| !lastFourthIndexKey.Equals(currentFourthIndexKey)
			|| !lastFifthIndexKey.Equals(currentFifthIndexKey))
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

		////////////////////////////////////////////////
		// 2/2，更新新的索引，
		// 注意：此时可能实体的其他属性发生了变化，
		// 因此无论索引关键字是否发生变化，都应当进行更新：
		////////////////////////////////////////////////
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
			toGetSecondaryIndexKeyOfItem(item),
			toGetThirdaryIndexKeyOfItem(item),
			toGetFourthIndexKeyOfItem(item),
			toGetFifthIndexKeyOfItem(item),
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