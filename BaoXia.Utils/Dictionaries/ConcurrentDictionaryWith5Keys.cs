using System;
using System.Collections.Concurrent;

namespace BaoXia.Utils.Dictionaries;

public class ConcurrentDictionaryWith5Keys
    <PrimaryDeictionaryKeyType,
    SecondaryDeictionaryKeyType,
    ThirdaryDeictionaryKeyType,
    FourthDeictionaryKeyType,
    FifthDeictionaryKeyType,
    ItemType>
    where PrimaryDeictionaryKeyType : notnull
    where SecondaryDeictionaryKeyType : notnull
    where ThirdaryDeictionaryKeyType : notnull
    where FourthDeictionaryKeyType : notnull
    where FifthDeictionaryKeyType : notnull
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public readonly ConcurrentDictionary<PrimaryDeictionaryKeyType,
	    ConcurrentDictionary<SecondaryDeictionaryKeyType,
	    ConcurrentDictionary<ThirdaryDeictionaryKeyType,
		ConcurrentDictionary<FourthDeictionaryKeyType,
		ConcurrentDictionary<FifthDeictionaryKeyType, DictionaryValueContainer<ItemType>>>>>> PrimaryDictionaries = new();

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
		ConcurrentDictionary<FifthDeictionaryKeyType, DictionaryValueContainer<ItemType>>>>>? GetSecondaryDictionaries(PrimaryDeictionaryKeyType primaryDeictionaryKey)
	{
		_ = PrimaryDictionaries.TryGetValue(primaryDeictionaryKey, out var secondaryDictionaries);
		{ }
		return secondaryDictionaries;
	}

	public ConcurrentDictionary<ThirdaryDeictionaryKeyType,
	    ConcurrentDictionary<FourthDeictionaryKeyType,
		ConcurrentDictionary<FifthDeictionaryKeyType, DictionaryValueContainer<ItemType>>>>? GetThirdaryDictionaries(
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
		ConcurrentDictionary<FifthDeictionaryKeyType, DictionaryValueContainer<ItemType>>>? GetFourthDictionaries(
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

	public ConcurrentDictionary<FifthDeictionaryKeyType, DictionaryValueContainer<ItemType>>? GetFifthDictionaries(
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

	public ItemType? Get(
	    PrimaryDeictionaryKeyType primaryDeictionaryKey,
	    SecondaryDeictionaryKeyType secondaryDeictionaryKey,
	    ThirdaryDeictionaryKeyType thirdaryDeictionaryKey,
	    FourthDeictionaryKeyType fourthDeictionaryKey,
	    FifthDeictionaryKeyType fifthDeictionaryKey)
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
		if (fifthDictionaries.TryGetValue(
		    fifthDeictionaryKey,
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
	    out ItemType? item)
	{
		item = default;
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
		    out var itemIndexInfo))
		{
			return false;
		}
		return itemIndexInfo.TryGetFirstItem(out item);
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
						var fifthDeictionary = fourthDeictionaryKeyValue.Value;
						foreach (var fifthDeictionaryKeyValue in fifthDeictionary)
						{
							// !!!
							allItemsCount += fifthDeictionaryKeyValue.Value.ItemsCount;
							// !!!
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
	    ItemType? item,
	    Func<ItemType?, ItemType?, ItemType?>? toUpdateIndexItemWithNewItem = null)
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
		var itemIndexInfo
		    = fifthDictionaries.GetOrAdd(
		    fifthDeictionaryKey,
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
	    Func<PrimaryDeictionaryKeyType,
	    SecondaryDeictionaryKeyType,
	    ThirdaryDeictionaryKeyType,
	    FourthDeictionaryKeyType,
	    FifthDeictionaryKeyType,
	    ItemType?> toCreateItem,
	    Func<ItemType?, ItemType?, ItemType?>? toUpdateIndexItemWithNewItem = null)
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
		var itemIndexInfo
		    = fifthDictionaries.GetOrAdd(
		    fifthDeictionaryKey,
		    (_) => new());
		if (itemIndexInfo.TryGetFirstItem(out var lastIndexItem))
		{
			return lastIndexItem;
		}
		lock (itemIndexInfo)
		{
			if (itemIndexInfo.TryGetFirstItem(out lastIndexItem))
			{
				return lastIndexItem;
			}

			// !!!
			var newIndexItem = toCreateItem(
			    primaryDeictionaryKey,
			    secondaryDeictionaryKey,
			    thirdaryDeictionaryKey,
			    fourthDeictionaryKey,
			    fifthDeictionaryKey);
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
	    ItemType newItem,
	    Func<ItemType?, ItemType?, ItemType?>? toUpdateIndexItemWithNewItem = null)
	{
		return GetOrAdd(
		    primaryDeictionaryKey,
		    secondaryDeictionaryKey,
		    thirdaryDeictionaryKey,
		    fourthDeictionaryKey,
		    fifthDeictionaryKey,
		    (_, _, _, _, _) => newItem,
		    toUpdateIndexItemWithNewItem);
	}

	public bool TryRemove(
	    PrimaryDeictionaryKeyType primaryDeictionaryKey,
	    SecondaryDeictionaryKeyType secondaryDeictionaryKey,
	    ThirdaryDeictionaryKeyType thirdaryDeictionaryKey,
	    FourthDeictionaryKeyType fourthDeictionaryKey,
	    FifthDeictionaryKeyType fifthDeictionaryKey,
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
		    out var itemIndexInfo))
		{
			return false;
		}

		lock (itemIndexInfo)
		{
			if (!itemIndexInfo.TryGetFirstItem(out itemRemoved))
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
	    out ItemType? itemRemoved)
	{
		_ = TryRemove(
		    primaryDeictionaryKey,
		    secondaryDeictionaryKey,
		    thirdaryDeictionaryKey,
		    fourthDeictionaryKey,
		    fifthDeictionaryKey,
		    out itemRemoved);
	}

	public void Clear()
	{
		PrimaryDictionaries.Clear();
	}

	public void Clear(PrimaryDeictionaryKeyType primaryDeictionaryKey)
	{
		if (primaryDeictionaryKey == null)
		{
			PrimaryDictionaries.Clear();
			return;
		}
		if (!PrimaryDictionaries.TryGetValue(
		    primaryDeictionaryKey,
		    out var secondaryDictionaries))
		{
			return;
		}
		secondaryDictionaries.Clear();
	}

	public void Clear(
	    PrimaryDeictionaryKeyType primaryDeictionaryKey,
	    SecondaryDeictionaryKeyType secondaryDeictionaryKey)
	{
		if (primaryDeictionaryKey == null)
		{
			Clear();
			return;
		}
		if (secondaryDeictionaryKey == null)
		{
			Clear(primaryDeictionaryKey);
			return;
		}
		if (!PrimaryDictionaries.TryGetValue(
		    primaryDeictionaryKey,
		    out var secondaryDictionaries))
		{
			return;
		}
		if (!secondaryDictionaries.TryGetValue(
		    secondaryDeictionaryKey,
		    out var thirdaryDeictionaries))
		{
			return;
		}
		thirdaryDeictionaries.Clear();
	}

	public void Clear(
	    PrimaryDeictionaryKeyType primaryDeictionaryKey,
	    SecondaryDeictionaryKeyType secondaryDeictionaryKey,
	    ThirdaryDeictionaryKeyType thirdaryDeictionaryKey)
	{
		if (primaryDeictionaryKey == null)
		{
			Clear();
			return;
		}
		if (secondaryDeictionaryKey == null)
		{
			Clear(primaryDeictionaryKey);
			return;
		}
		if (thirdaryDeictionaryKey == null)
		{
			Clear(primaryDeictionaryKey, secondaryDeictionaryKey);
			return;
		}
		if (!PrimaryDictionaries.TryGetValue(
		    primaryDeictionaryKey,
		    out var secondaryDictionaries))
		{
			return;
		}
		if (!secondaryDictionaries.TryGetValue(
		    secondaryDeictionaryKey,
		    out var thirdaryDeictionaries))
		{
			return;
		}
		if (!thirdaryDeictionaries.TryGetValue(
		    thirdaryDeictionaryKey,
		    out var fourthDeictionaries))
		{
			return;
		}
		fourthDeictionaries.Clear();
	}

	public void Clear(
	    PrimaryDeictionaryKeyType primaryDeictionaryKey,
	    SecondaryDeictionaryKeyType secondaryDeictionaryKey,
	    ThirdaryDeictionaryKeyType thirdaryDeictionaryKey,
	    FourthDeictionaryKeyType fourthDeictionaryKey)
	{
		if (primaryDeictionaryKey == null)
		{
			Clear();
			return;
		}
		if (secondaryDeictionaryKey == null)
		{
			Clear(primaryDeictionaryKey);
			return;
		}
		if (thirdaryDeictionaryKey == null)
		{
			Clear(primaryDeictionaryKey, secondaryDeictionaryKey);
			return;
		}
		if (fourthDeictionaryKey == null)
		{
			Clear(primaryDeictionaryKey, secondaryDeictionaryKey, thirdaryDeictionaryKey);
			return;
		}
		if (!PrimaryDictionaries.TryGetValue(
		    primaryDeictionaryKey,
		    out var secondaryDictionaries))
		{
			return;
		}
		if (!secondaryDictionaries.TryGetValue(
		    secondaryDeictionaryKey,
		    out var thirdaryDeictionaries))
		{
			return;
		}
		if (!thirdaryDeictionaries.TryGetValue(
		    thirdaryDeictionaryKey,
		    out var fourthDeictionaries))
		{
			return;
		}
		if (!fourthDeictionaries.TryGetValue(
		    fourthDeictionaryKey,
		    out var fifthDeictionaries))
		{
			return;
		}

		//
		fifthDeictionaries.Clear();
		//
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
		//
		ItemType? newIndexItem)
	{
		return newIndexItem;
	}

	#endregion
}
