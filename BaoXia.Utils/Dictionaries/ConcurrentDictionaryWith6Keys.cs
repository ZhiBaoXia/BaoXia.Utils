using BaoXia.Utils.Cache.Index;
using System;
using System.Collections.Concurrent;

namespace BaoXia.Utils.Dictionaries;

public class ConcurrentDictionaryWith6Keys
	<ItemType,
	PrimaryDeictionaryKeyType,
	SecondaryDeictionaryKeyType,
	ThirdaryDeictionaryKeyType,
	FourthDeictionaryKeyType,
	FifthDeictionaryKeyType,
	SixthDeictionaryKeyType>
	where PrimaryDeictionaryKeyType : notnull
	where SecondaryDeictionaryKeyType : notnull
	where ThirdaryDeictionaryKeyType : notnull
	where FourthDeictionaryKeyType : notnull
	where FifthDeictionaryKeyType : notnull
	where SixthDeictionaryKeyType : notnull
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public readonly ConcurrentDictionary<PrimaryDeictionaryKeyType,
		ConcurrentDictionary<SecondaryDeictionaryKeyType,
			ConcurrentDictionary<ThirdaryDeictionaryKeyType,
				ConcurrentDictionary<FourthDeictionaryKeyType,
					ConcurrentDictionary<FifthDeictionaryKeyType,
						ConcurrentDictionary<SixthDeictionaryKeyType, ItemIndexNode<ItemType>>>>>>> PrimaryDictionaries = new();

	private string? _name = null;
	public string? Name { get => _name; set => _name = value; }

	#endregion


	////////////////////////////////////////////////
	// @自身实现，获取数据部分。
	////////////////////////////////////////////////

	#region 自身实现，获取数据部分。

	public ConcurrentDictionary<SecondaryDeictionaryKeyType,
		ConcurrentDictionary<ThirdaryDeictionaryKeyType,
			ConcurrentDictionary<FourthDeictionaryKeyType,
				ConcurrentDictionary<FifthDeictionaryKeyType,
					ConcurrentDictionary<SixthDeictionaryKeyType, ItemIndexNode<ItemType>>>>>>? GetSecondaryDictionaries(PrimaryDeictionaryKeyType primaryDeictionaryKey)
	{
		_ = PrimaryDictionaries.TryGetValue(primaryDeictionaryKey, out var secondaryDictionaries);
		{ }
		return secondaryDictionaries;
	}

	public ConcurrentDictionary<ThirdaryDeictionaryKeyType,
			ConcurrentDictionary<FourthDeictionaryKeyType,
				ConcurrentDictionary<FifthDeictionaryKeyType,
					ConcurrentDictionary<SixthDeictionaryKeyType, ItemIndexNode<ItemType>>>>>? GetThirdaryDictionaries(
		PrimaryDeictionaryKeyType primaryDeictionaryKey,
		SecondaryDeictionaryKeyType secondaryDeictionaryKey)
	{
		var secondaryDictionaries = GetSecondaryDictionaries(primaryDeictionaryKey);
		if (secondaryDictionaries == null)
		{
			return null;
		}
		_ = secondaryDictionaries.TryGetValue(secondaryDeictionaryKey, out var thirdaryDictionaries);
		{ }
		return thirdaryDictionaries;
	}

	public ConcurrentDictionary<FourthDeictionaryKeyType,
				ConcurrentDictionary<FifthDeictionaryKeyType,
					ConcurrentDictionary<SixthDeictionaryKeyType, ItemIndexNode<ItemType>>>>? GetFourthDictionaries(
		PrimaryDeictionaryKeyType primaryDeictionaryKey,
		SecondaryDeictionaryKeyType secondaryDeictionaryKey,
		ThirdaryDeictionaryKeyType thirdaryDeictionaryKey)
	{
		var thirdaryDictionaries = GetThirdaryDictionaries(
			primaryDeictionaryKey,
			secondaryDeictionaryKey);
		if (thirdaryDictionaries == null)
		{
			return null;
		}
		_ = thirdaryDictionaries.TryGetValue(thirdaryDeictionaryKey, out var fourthDictionaries);
		{ }
		return fourthDictionaries;
	}

	public ConcurrentDictionary<FifthDeictionaryKeyType,
					ConcurrentDictionary<SixthDeictionaryKeyType, ItemIndexNode<ItemType>>>? GetFifthDictionaries(
		PrimaryDeictionaryKeyType primaryDeictionaryKey,
		SecondaryDeictionaryKeyType secondaryDeictionaryKey,
		ThirdaryDeictionaryKeyType thirdaryDeictionaryKey,
		FourthDeictionaryKeyType fourthDeictionaryKey)
	{
		var fourthDictionaries = GetFourthDictionaries(
			primaryDeictionaryKey,
			secondaryDeictionaryKey,
			thirdaryDeictionaryKey);
		if (fourthDictionaries == null)
		{
			return null;
		}
		_ = fourthDictionaries.TryGetValue(fourthDeictionaryKey, out var fifthDictionaries);
		{ }
		return fifthDictionaries;
	}

	public ConcurrentDictionary<SixthDeictionaryKeyType, ItemIndexNode<ItemType>>? GetSixthDictionaries(
		PrimaryDeictionaryKeyType primaryDeictionaryKey,
		SecondaryDeictionaryKeyType secondaryDeictionaryKey,
		ThirdaryDeictionaryKeyType thirdaryDeictionaryKey,
		FourthDeictionaryKeyType fourthDeictionaryKey,
		FifthDeictionaryKeyType fifthDeictionaryKey)
	{
		var fifthDictionaries = GetFifthDictionaries(
			primaryDeictionaryKey,
			secondaryDeictionaryKey,
			thirdaryDeictionaryKey,
			fourthDeictionaryKey);
		if (fifthDictionaries == null)
		{
			return null;
		}
		_ = fifthDictionaries.TryGetValue(fifthDeictionaryKey, out var sixthDictionaries);
		{ }
		return sixthDictionaries;
	}

	public ItemType? Get(
		PrimaryDeictionaryKeyType primaryDeictionaryKey,
		SecondaryDeictionaryKeyType secondaryDeictionaryKey,
		ThirdaryDeictionaryKeyType thirdaryDeictionaryKey,
		FourthDeictionaryKeyType fourthDeictionaryKey,
		FifthDeictionaryKeyType fifthDeictionaryKey,
		SixthDeictionaryKeyType sixthDeictionaryKey)
	{
		if (!PrimaryDictionaries.TryGetValue(
			primaryDeictionaryKey,
			out var secondaryDictionaries))
		{
			return default;
		}
		if (!secondaryDictionaries.TryGetValue(
			secondaryDeictionaryKey,
			out var thirdaryDictionaries))
		{
			return default;
		}
		if (!thirdaryDictionaries.TryGetValue(
			thirdaryDeictionaryKey,
			out var fourthDictionaries))
		{
			return default;
		}
		if (!fourthDictionaries.TryGetValue(
			fourthDeictionaryKey,
			out var fifthDictionaries))
		{
			return default;
		}
		if (!fifthDictionaries.TryGetValue(
			fifthDeictionaryKey,
			out var sixthDictionaries))
		{
			return default;
		}
		if (sixthDictionaries.TryGetValue(
			sixthDeictionaryKey,
			out var enityIndexInfo))
		{
			return enityIndexInfo.FirstItem;
		}
		return default;
	}

	public bool TryGet(
		PrimaryDeictionaryKeyType primaryDeictionaryKey,
		SecondaryDeictionaryKeyType secondaryDeictionaryKey,
		ThirdaryDeictionaryKeyType thirdaryDeictionaryKey,
		FourthDeictionaryKeyType fourthDeictionaryKey,
		FifthDeictionaryKeyType fifthDeictionaryKey,
		SixthDeictionaryKeyType sixthDeictionaryKey,
		out ItemType? item)
	{
		item = Get(
			primaryDeictionaryKey,
			secondaryDeictionaryKey,
			thirdaryDeictionaryKey,
			fourthDeictionaryKey,
			fifthDeictionaryKey,
			sixthDeictionaryKey);
		if (item != null)
		{
			return true;
		}
		return false;
	}

	public int GetCount()
	{
		int allItemsCount = 0;
		foreach (var primaryDeictionaryKeyValue in PrimaryDictionaries)
		{
			var secondaryDeictionaries = primaryDeictionaryKeyValue.Value;
			foreach (var secondaryDeictionaryKeyValue in secondaryDeictionaries)
			{
				var thirdaryDeictionaries = secondaryDeictionaryKeyValue.Value;
				foreach (var thirdaryDeictionaryKeyValue in thirdaryDeictionaries)
				{
					var fourthDeictionaries = thirdaryDeictionaryKeyValue.Value;
					foreach (var fourthDeictionaryKeyValue in fourthDeictionaries)
					{
						var fifthDeictionaries = fourthDeictionaryKeyValue.Value;
						foreach (var fifthDeictionaryKeyValue in fifthDeictionaries)
						{
							var sixthDeictionary = fifthDeictionaryKeyValue.Value;
							foreach (var sixthDeictionaryKeyValue in sixthDeictionary)
							{
								// !!!
								allItemsCount += sixthDeictionaryKeyValue.Value.ItemsCount;
								// !!!
							}
						}
					}
				}
			}
		}
		return allItemsCount;
	}

	#endregion


	////////////////////////////////////////////////
	// @自身实现，更新数据部分。
	////////////////////////////////////////////////

	#region 自身实现，更新数据部分。

	public ItemType? Add(
		PrimaryDeictionaryKeyType primaryDeictionaryKey,
		SecondaryDeictionaryKeyType secondaryDeictionaryKey,
		ThirdaryDeictionaryKeyType thirdaryDeictionaryKey,
		FourthDeictionaryKeyType fourthDeictionaryKey,
		FifthDeictionaryKeyType fifthDeictionaryKey,
		SixthDeictionaryKeyType sixthDeictionaryKey,
		ItemType item,
		Func<ItemType, ItemType?, ItemType?>? toUpdateIndexItemWithNewItem = null)
	{
		var secondaryDictionaries
			= PrimaryDictionaries.GetOrAdd(
				primaryDeictionaryKey,
				(_) => []);
		var thirdaryDictionaries
			= secondaryDictionaries.GetOrAdd(
				secondaryDeictionaryKey,
				(_) => []);
		var fourthDictionaries
			= thirdaryDictionaries.GetOrAdd(
				thirdaryDeictionaryKey,
				(_) => []);
		var fifthDictionaries
			= fourthDictionaries.GetOrAdd(
				fourthDeictionaryKey,
				(_) => []);
		var sixthDictionaries
			= fifthDictionaries.GetOrAdd(
				fifthDeictionaryKey,
				(_) => []);
		var itemIndexInfo = sixthDictionaries.GetOrAdd(
			sixthDeictionaryKey,
			(_) => new());
		lock (itemIndexInfo)
		{
			// !!!
			var lastIndexItem = itemIndexInfo.FirstItem;
			var newIndexItem = item;
			if (toUpdateIndexItemWithNewItem != null)
			{
				newIndexItem = toUpdateIndexItemWithNewItem(item, lastIndexItem);
			}
			newIndexItem = WillUpdateIndexItemWithPrimaryDeictionaryKey(
				primaryDeictionaryKey,
				secondaryDeictionaryKey,
				thirdaryDeictionaryKey,
				fourthDeictionaryKey,
				fifthDeictionaryKey,
				sixthDeictionaryKey,
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
			// !!!
			return newIndexItem;
			// !!!
		}
	}

	public ItemType? GetOrAdd(
		PrimaryDeictionaryKeyType primaryDeictionaryKey,
		SecondaryDeictionaryKeyType secondaryDeictionaryKey,
		ThirdaryDeictionaryKeyType thirdaryDeictionaryKey,
		FourthDeictionaryKeyType fourthDeictionaryKey,
		FifthDeictionaryKeyType fifthDeictionaryKey,
		SixthDeictionaryKeyType sixthDeictionaryKey,
		Func<PrimaryDeictionaryKeyType,
			SecondaryDeictionaryKeyType,
			ThirdaryDeictionaryKeyType,
			FourthDeictionaryKeyType,
			FifthDeictionaryKeyType,
			SixthDeictionaryKeyType,
			ItemType> toCreateItem,
		Func<ItemType, ItemType?, ItemType?>? toUpdateIndexItemWithNewItem = null)
	{
		var secondaryDictionaries
			= PrimaryDictionaries.GetOrAdd(
				primaryDeictionaryKey,
				(_) => []);
		var thirdaryDictionaries
			= secondaryDictionaries.GetOrAdd(
				secondaryDeictionaryKey,
				(_) => []);
		var fourthDictionaries
			= thirdaryDictionaries.GetOrAdd(
				thirdaryDeictionaryKey,
				(_) => []);
		var fifthDictionaries
			= fourthDictionaries.GetOrAdd(
				fourthDeictionaryKey,
				(_) => []);
		var sixthDictionaries
			= fifthDictionaries.GetOrAdd(
				fifthDeictionaryKey,
				(_) => []);
		var itemIndexInfo = sixthDictionaries.GetOrAdd(
			sixthDeictionaryKey,
			(_) => new());
		var lastIndexItem = itemIndexInfo.FirstItem;
		if (lastIndexItem != null)
		{
			return lastIndexItem;
		}
		lock (itemIndexInfo)
		{
			lastIndexItem = itemIndexInfo.FirstItem;
			if (lastIndexItem != null)
			{
				return lastIndexItem;
			}

			// !!!
			var newIndexItem = toCreateItem(
				primaryDeictionaryKey,
				secondaryDeictionaryKey,
				thirdaryDeictionaryKey,
				fourthDeictionaryKey,
				fifthDeictionaryKey,
				sixthDeictionaryKey);
			if (toUpdateIndexItemWithNewItem != null)
			{
				newIndexItem = toUpdateIndexItemWithNewItem(newIndexItem, lastIndexItem);
			}
			newIndexItem = WillUpdateIndexItemWithPrimaryDeictionaryKey(
				primaryDeictionaryKey,
				secondaryDeictionaryKey,
				thirdaryDeictionaryKey,
				fourthDeictionaryKey,
				fifthDeictionaryKey,
				sixthDeictionaryKey,
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
			// !!!
			return newIndexItem;
			// !!!
		}
	}

	public ItemType? GetOrAdd(
		PrimaryDeictionaryKeyType primaryDeictionaryKey,
		SecondaryDeictionaryKeyType secondaryDeictionaryKey,
		ThirdaryDeictionaryKeyType thirdaryDeictionaryKey,
		FourthDeictionaryKeyType fourthDeictionaryKey,
		FifthDeictionaryKeyType fifthDeictionaryKey,
		SixthDeictionaryKeyType sixthDeictionaryKey,
		ItemType newItem,
		Func<ItemType, ItemType?, ItemType?>? toUpdateIndexItemWithNewItem = null)
	{
		return GetOrAdd(
			primaryDeictionaryKey,
			secondaryDeictionaryKey,
			thirdaryDeictionaryKey,
			fourthDeictionaryKey,
			fifthDeictionaryKey,
			sixthDeictionaryKey,
			(_, _, _, _, _, _) => newItem,
			toUpdateIndexItemWithNewItem);
	}

	public bool TryRemove(
		PrimaryDeictionaryKeyType primaryDeictionaryKey,
		SecondaryDeictionaryKeyType secondaryDeictionaryKey,
		ThirdaryDeictionaryKeyType thirdaryDeictionaryKey,
		FourthDeictionaryKeyType fourthDeictionaryKey,
		FifthDeictionaryKeyType fifthDeictionaryKey,
		SixthDeictionaryKeyType sixthDeictionaryKey,
		out ItemType? itemRemoved)
	{
		//
		itemRemoved = default;
		// 

		if (!PrimaryDictionaries.TryGetValue(
			primaryDeictionaryKey,
			out var secondaryDictionaries))
		{
			return false;
		}
		if (!secondaryDictionaries.TryGetValue(
			secondaryDeictionaryKey,
			out var thirdaryDictionaries))
		{
			return false;
		}
		if (!thirdaryDictionaries.TryGetValue(
			thirdaryDeictionaryKey,
			out var fourthDictionaries))
		{
			return false;
		}
		if (!fourthDictionaries.TryGetValue(
			fourthDeictionaryKey,
			out var fifthDictionaries))
		{
			return false;
		}
		if (!fifthDictionaries.TryGetValue(
			fifthDeictionaryKey,
			out var sixthDictionaries))
		{
			return false;
		}
		if (!sixthDictionaries.TryGetValue(
			sixthDeictionaryKey,
			out var itemIndexInfo))
		{
			return false;
		}

		lock (itemIndexInfo)
		{
			// !!!
			itemRemoved = itemIndexInfo.FirstItem;
			// !!!
			if (itemRemoved == null)
			{
				return false;
			}
			// !!!
			itemIndexInfo.Items = [];
			// !!!
			return true;
		}
	}

	public void Remove(
		PrimaryDeictionaryKeyType primaryDeictionaryKey,
		SecondaryDeictionaryKeyType secondaryDeictionaryKey,
		ThirdaryDeictionaryKeyType thirdaryDeictionaryKey,
		FourthDeictionaryKeyType fourthDeictionaryKey,
		FifthDeictionaryKeyType fifthDeictionaryKey,
		SixthDeictionaryKeyType sixthDeictionaryKey,
		out ItemType? itemRemoved)
	{
		_ = TryRemove(
			primaryDeictionaryKey,
			secondaryDeictionaryKey,
			thirdaryDeictionaryKey,
			fourthDeictionaryKey,
			fifthDeictionaryKey,
			sixthDeictionaryKey,
			out itemRemoved);
	}

	public void Remove(
		PrimaryDeictionaryKeyType primaryDeictionaryKey,
		SecondaryDeictionaryKeyType secondaryDeictionaryKey,
		ThirdaryDeictionaryKeyType thirdaryDeictionaryKey,
		FourthDeictionaryKeyType fourthDeictionaryKey,
		FifthDeictionaryKeyType fifthDeictionaryKey,
		SixthDeictionaryKeyType sixthDeictionaryKey)
	{
		Remove(
			primaryDeictionaryKey,
			secondaryDeictionaryKey,
			thirdaryDeictionaryKey,
			fourthDeictionaryKey,
			fifthDeictionaryKey,
			sixthDeictionaryKey,
			out _);
	}

	public void Clear()
	{
		PrimaryDictionaries.Clear();
	}

	#endregion


	////////////////////////////////////////////////
	// @事件节点
	////////////////////////////////////////////////

	#region 事件节点

	protected virtual ItemType? WillUpdateIndexItemWithPrimaryDeictionaryKey(
				PrimaryDeictionaryKeyType primaryDeictionaryKey,
				SecondaryDeictionaryKeyType secondaryDeictionaryKey,
				ThirdaryDeictionaryKeyType thirdaryDeictionaryKey,
				FourthDeictionaryKeyType fourthDeictionaryKey,
				FifthDeictionaryKeyType fifthDeictionaryKey,
				SixthDeictionaryKeyType sixthDeictionaryKey,
				//
				ItemType? newIndexItem)
	{
		return newIndexItem;
	}

	#endregion
}