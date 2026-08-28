using System;
using System.Collections.Concurrent;

namespace BaoXia.Utils.Dictionaries;

public class ConcurrentDictionaryWith4Keys
    <PrimaryDictionaryKeyType,
    SecondaryDictionaryKeyType,
    ThirdaryDictionaryKeyType,
    FourthDictionaryKeyType,
    ItemType>
    where PrimaryDictionaryKeyType : notnull
    where SecondaryDictionaryKeyType : notnull
    where ThirdaryDictionaryKeyType : notnull
    where FourthDictionaryKeyType : notnull
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public readonly ConcurrentDictionary<PrimaryDictionaryKeyType,
	    ConcurrentDictionary<SecondaryDictionaryKeyType,
	    ConcurrentDictionary<ThirdaryDictionaryKeyType,
		ConcurrentDictionary<FourthDictionaryKeyType, DictionaryValueContainer<ItemType>>>>> PrimaryDictionaries = new();

	private string? _name = null;
	public string? Name { get => _name; set => _name = value; }

	#endregion


	////////////////////////////////////////////////
	// @自身实现，获取数据部分。
	////////////////////////////////////////////////

	#region 自身实现，获取数据部分。

	public ConcurrentDictionary<SecondaryDictionaryKeyType,
	    ConcurrentDictionary<ThirdaryDictionaryKeyType,
	    ConcurrentDictionary<FourthDictionaryKeyType, DictionaryValueContainer<ItemType>>>>? GetSecondaryDictionaries(PrimaryDictionaryKeyType primaryDictionaryKey)
	{
		_ = PrimaryDictionaries.TryGetValue(primaryDictionaryKey, out var secondaryDictionaries);
		{ }
		return secondaryDictionaries;
	}

	public ConcurrentDictionary<ThirdaryDictionaryKeyType,
	    ConcurrentDictionary<FourthDictionaryKeyType, DictionaryValueContainer<ItemType>>>? GetThirdaryDictionaries(
	    PrimaryDictionaryKeyType primaryDictionaryKey,
	    SecondaryDictionaryKeyType secondaryDictionaryKey)
	{
		var secondaryDictionaries = GetSecondaryDictionaries(primaryDictionaryKey);
		if (secondaryDictionaries == null)
		{
			return null;
		}
		_ = secondaryDictionaries.TryGetValue(secondaryDictionaryKey, out var thirdaryDictionaries);
		{ }
		return thirdaryDictionaries;
	}

	public ConcurrentDictionary<FourthDictionaryKeyType, DictionaryValueContainer<ItemType>>? GetFourthDictionaries(
	    PrimaryDictionaryKeyType primaryDictionaryKey,
	    SecondaryDictionaryKeyType secondaryDictionaryKey,
	    ThirdaryDictionaryKeyType thirdaryDictionaryKey)
	{
		var thirdaryDictionaries = GetThirdaryDictionaries(
		    primaryDictionaryKey,
		    secondaryDictionaryKey);
		if (thirdaryDictionaries == null)
		{
			return null;
		}
		_ = thirdaryDictionaries.TryGetValue(thirdaryDictionaryKey, out var fourthDictionaries);
		{ }
		return fourthDictionaries;
	}

	public ItemType? Get(
	    PrimaryDictionaryKeyType primaryDictionaryKey,
	    SecondaryDictionaryKeyType secondaryDictionaryKey,
	    ThirdaryDictionaryKeyType thirdaryDictionaryKey,
	    FourthDictionaryKeyType fourthDictionaryKey)
	{
		if (!PrimaryDictionaries.TryGetValue(
		    primaryDictionaryKey,
		    out var secondaryDictionaries))
		{
			return default;
		}
		if (!secondaryDictionaries.TryGetValue(
		    secondaryDictionaryKey,
		    out var thirdaryDictionaries))
		{
			return default;
		}
		if (!thirdaryDictionaries.TryGetValue(
		    thirdaryDictionaryKey,
		    out var fourthDictionaries))
		{
			return default;
		}
		if (fourthDictionaries.TryGetValue(
		    fourthDictionaryKey,
		    out var enityIndexInfo))
		{
			return enityIndexInfo.FirstItem;
		}
		return default;
	}

	public bool TryGet(
	    PrimaryDictionaryKeyType primaryDictionaryKey,
	    SecondaryDictionaryKeyType secondaryDictionaryKey,
	    ThirdaryDictionaryKeyType thirdaryDictionaryKey,
	    FourthDictionaryKeyType fourthDictionaryKey,
	    out ItemType? item)
	{
		item = default;
		if (!PrimaryDictionaries.TryGetValue(
		    primaryDictionaryKey,
		    out var secondaryDictionaries))
		{
			return false;
		}
		if (!secondaryDictionaries.TryGetValue(
		    secondaryDictionaryKey,
		    out var thirdaryDictionaries))
		{
			return false;
		}
		if (!thirdaryDictionaries.TryGetValue(
		    thirdaryDictionaryKey,
		    out var fourthDictionaries))
		{
			return false;
		}
		if (!fourthDictionaries.TryGetValue(
		    fourthDictionaryKey,
		    out var itemIndexInfo))
		{
			return false;
		}
		return itemIndexInfo.TryGetFirstItem(out item);
	}

	public int GetCount()
	{
		int allItemsCount = 0;
		foreach (var primaryDictionaryKeyValue in PrimaryDictionaries)
		{
			var secondaryDeictionaries = primaryDictionaryKeyValue.Value;
			foreach (var secondaryDictionaryKeyValue in secondaryDeictionaries)
			{
				var thirdaryDeictionaries = secondaryDictionaryKeyValue.Value;
				foreach (var thirdaryDictionaryKeyValue in thirdaryDeictionaries)
				{
					var fourthDictionary = thirdaryDictionaryKeyValue.Value;
					foreach (var fourthDictionaryKeyValue in fourthDictionary)
					{
						// !!!
						allItemsCount += fourthDictionaryKeyValue.Value.ItemsCount;
						// !!!
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
	    PrimaryDictionaryKeyType primaryDictionaryKey,
	    SecondaryDictionaryKeyType secondaryDictionaryKey,
	    ThirdaryDictionaryKeyType thirdaryDictionaryKey,
	    FourthDictionaryKeyType fourthDictionaryKey,
	    ItemType? item,
	    Func<ItemType?, ItemType?, ItemType?>? toUpdateIndexItemWithNewItem = null)
	{
		var secondaryDictionaries
		    = PrimaryDictionaries.GetOrAdd(
		    primaryDictionaryKey,
		    (_) => []);
		var thirdaryDictionaries
		    = secondaryDictionaries.GetOrAdd(
		    secondaryDictionaryKey,
		    (_) => []);
		var fourthDictionaries
		    = thirdaryDictionaries.GetOrAdd(
		    thirdaryDictionaryKey,
		    (_) => []);
		var itemIndexInfo
		    = fourthDictionaries.GetOrAdd(
		    fourthDictionaryKey,
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
			newIndexItem = WillUpdateIndexItemWithPrimaryDictionaryKey(
			    primaryDictionaryKey,
			    secondaryDictionaryKey,
			    thirdaryDictionaryKey,
			    fourthDictionaryKey,
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
	    PrimaryDictionaryKeyType primaryDictionaryKey,
	    SecondaryDictionaryKeyType secondaryDictionaryKey,
	    ThirdaryDictionaryKeyType thirdaryDictionaryKey,
	    FourthDictionaryKeyType fourthDictionaryKey,
	    Func<PrimaryDictionaryKeyType,
	    SecondaryDictionaryKeyType,
	    ThirdaryDictionaryKeyType,
	    FourthDictionaryKeyType,
	    ItemType?> toCreateItem,
	    Func<ItemType?, ItemType?, ItemType?>? toUpdateIndexItemWithNewItem = null)
	{
		var secondaryDictionaries
		    = PrimaryDictionaries.GetOrAdd(
		    primaryDictionaryKey,
		    (_) => []);
		var thirdaryDictionaries
		    = secondaryDictionaries.GetOrAdd(
		    secondaryDictionaryKey,
		    (_) => []);
		var fourthDictionaries
		    = thirdaryDictionaries.GetOrAdd(
		    thirdaryDictionaryKey,
		    (_) => []);
		var itemIndexInfo
		    = fourthDictionaries.GetOrAdd(
		    fourthDictionaryKey,
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
			    primaryDictionaryKey,
			    secondaryDictionaryKey,
			    thirdaryDictionaryKey,
			    fourthDictionaryKey);
			if (toUpdateIndexItemWithNewItem != null)
			{
				newIndexItem = toUpdateIndexItemWithNewItem(newIndexItem, lastIndexItem);
			}
			newIndexItem = WillUpdateIndexItemWithPrimaryDictionaryKey(
			    primaryDictionaryKey,
			    secondaryDictionaryKey,
			    thirdaryDictionaryKey,
			    fourthDictionaryKey,
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
	    PrimaryDictionaryKeyType primaryDictionaryKey,
	    SecondaryDictionaryKeyType secondaryDictionaryKey,
	    ThirdaryDictionaryKeyType thirdaryDictionaryKey,
	    FourthDictionaryKeyType fourthDictionaryKey,
	    ItemType newItem,
	    Func<ItemType?, ItemType?, ItemType?>? toUpdateIndexItemWithNewItem = null)
	{
		return GetOrAdd(
		    primaryDictionaryKey,
		    secondaryDictionaryKey,
		    thirdaryDictionaryKey,
		    fourthDictionaryKey,
		    (_, _, _, _) => newItem,
		    toUpdateIndexItemWithNewItem);
	}

	public bool TryRemove(
	    PrimaryDictionaryKeyType primaryDictionaryKey,
	    SecondaryDictionaryKeyType secondaryDictionaryKey,
	    ThirdaryDictionaryKeyType thirdaryDictionaryKey,
	    FourthDictionaryKeyType fourthDictionaryKey,
	    out ItemType? itemRemoved)
	{
		//
		itemRemoved = default;
		// 

		if (!PrimaryDictionaries.TryGetValue(
		    primaryDictionaryKey,
		    out var secondaryDictionaries))
		{
			return false;
		}
		if (!secondaryDictionaries.TryGetValue(
		    secondaryDictionaryKey,
		    out var thirdaryDictionaries))
		{
			return false;
		}
		if (!thirdaryDictionaries.TryGetValue(
		    thirdaryDictionaryKey,
		    out var fourthDictionaries))
		{
			return false;
		}
		if (!fourthDictionaries.TryGetValue(
		    fourthDictionaryKey,
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
	    PrimaryDictionaryKeyType primaryDictionaryKey,
	    SecondaryDictionaryKeyType secondaryDictionaryKey,
	    ThirdaryDictionaryKeyType thirdaryDictionaryKey,
	    FourthDictionaryKeyType fourthDictionaryKey,
	    out ItemType? itemRemoved)
	{
		_ = TryRemove(
		    primaryDictionaryKey,
		    secondaryDictionaryKey,
		    thirdaryDictionaryKey,
		    fourthDictionaryKey,
		    out itemRemoved);
	}

	public void Clear()
	{
		PrimaryDictionaries.Clear();
	}

	public void Clear(PrimaryDictionaryKeyType primaryDictionaryKey)
	{
		if (primaryDictionaryKey == null)
		{
			PrimaryDictionaries.Clear();
			return;
		}
		if (!PrimaryDictionaries.TryGetValue(
		    primaryDictionaryKey,
		    out var secondaryDictionaries))
		{
			return;
		}
		secondaryDictionaries.Clear();
	}

	public void Clear(
	    PrimaryDictionaryKeyType primaryDictionaryKey,
	    SecondaryDictionaryKeyType secondaryDictionaryKey)
	{
		if (primaryDictionaryKey == null)
		{
			Clear();
			return;
		}
		if (secondaryDictionaryKey == null)
		{
			Clear(primaryDictionaryKey);
			return;
		}
		if (!PrimaryDictionaries.TryGetValue(
		    primaryDictionaryKey,
		    out var secondaryDictionaries))
		{
			return;
		}
		if (!secondaryDictionaries.TryGetValue(
		    secondaryDictionaryKey,
		    out var thirdaryDeictionaries))
		{
			return;
		}
		thirdaryDeictionaries.Clear();
	}

	public void Clear(
	    PrimaryDictionaryKeyType primaryDictionaryKey,
	    SecondaryDictionaryKeyType secondaryDictionaryKey,
	    ThirdaryDictionaryKeyType thirdaryDictionaryKey)
	{
		if (primaryDictionaryKey == null)
		{
			Clear();
			return;
		}
		if (secondaryDictionaryKey == null)
		{
			Clear(primaryDictionaryKey);
			return;
		}
		if (thirdaryDictionaryKey == null)
		{
			Clear(primaryDictionaryKey, secondaryDictionaryKey);
			return;
		}
		if (!PrimaryDictionaries.TryGetValue(
		    primaryDictionaryKey,
		    out var secondaryDictionaries))
		{
			return;
		}
		if (!secondaryDictionaries.TryGetValue(
		    secondaryDictionaryKey,
		    out var thirdaryDeictionaries))
		{
			return;
		}
		if (!thirdaryDeictionaries.TryGetValue(
		    thirdaryDictionaryKey,
		    out var fourthDeictionaries))
		{
			return;
		}

		//
		fourthDeictionaries.Clear();
		//
	}


	#endregion


	////////////////////////////////////////////////
	// @事件节点
	////////////////////////////////////////////////

	#region 事件节点

	protected virtual ItemType? WillUpdateIndexItemWithPrimaryDictionaryKey(
		PrimaryDictionaryKeyType primaryDictionaryKey,
		SecondaryDictionaryKeyType secondaryDictionaryKey,
		ThirdaryDictionaryKeyType thirdaryDictionaryKey,
		FourthDictionaryKeyType fourthDictionaryKey,
		//
		ItemType? newIndexItem)
	{
		return newIndexItem;
	}

	#endregion
}
