using System;
using System.Collections.Concurrent;

namespace BaoXia.Utils.Dictionaries;

public class ConcurrentDictionaryWith3Keys
    <PrimaryDictionaryKeyType,
    SecondaryDictionaryKeyType,
    ThirdaryDictionaryKeyType,
    ItemType>
    where PrimaryDictionaryKeyType : notnull
    where SecondaryDictionaryKeyType : notnull
    where ThirdaryDictionaryKeyType : notnull
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public readonly ConcurrentDictionary<PrimaryDictionaryKeyType,
	    ConcurrentDictionary<SecondaryDictionaryKeyType,
	    ConcurrentDictionary<ThirdaryDictionaryKeyType, DictionaryValueContainer<ItemType>>>> PrimaryDictionaries = new();

	private string? _name = null;
	public string? Name { get => _name; set => _name = value; }

	#endregion


	////////////////////////////////////////////////
	// @自身实现，获取数据部分。
	////////////////////////////////////////////////

	#region 自身实现，获取数据部分。

	public ConcurrentDictionary<SecondaryDictionaryKeyType,
	    ConcurrentDictionary<ThirdaryDictionaryKeyType, DictionaryValueContainer<ItemType>>>? GetSecondaryDictionaries(PrimaryDictionaryKeyType primaryDictionaryKey)
	{
		_ = PrimaryDictionaries.TryGetValue(primaryDictionaryKey, out var secondaryDictionaries);
		{ }
		return secondaryDictionaries;
	}

	public ConcurrentDictionary<ThirdaryDictionaryKeyType, DictionaryValueContainer<ItemType>>? GetThirdaryDictionaries(
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

	public ItemType? Get(
	    PrimaryDictionaryKeyType primaryDictionaryKey,
	    SecondaryDictionaryKeyType secondaryDictionaryKey,
	    ThirdaryDictionaryKeyType thirdaryDictionaryKey)
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
		if (thirdaryDictionaries.TryGetValue(
		    thirdaryDictionaryKey,
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
				var thirdaryDictionary = secondaryDictionaryKeyValue.Value;
				foreach (var thirdaryDictionaryKeyValue in thirdaryDictionary)
				{
					// !!!
					allItemsCount += thirdaryDictionaryKeyValue.Value.ItemsCount;
					// !!!
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
		var itemIndexInfo
		    = thirdaryDictionaries.GetOrAdd(
		    thirdaryDictionaryKey,
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
	    Func<PrimaryDictionaryKeyType,
	    SecondaryDictionaryKeyType,
	    ThirdaryDictionaryKeyType,
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
		var itemIndexInfo
		    = thirdaryDictionaries.GetOrAdd(
		    thirdaryDictionaryKey,
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
			    thirdaryDictionaryKey);
			if (toUpdateIndexItemWithNewItem != null)
			{
				newIndexItem = toUpdateIndexItemWithNewItem(newIndexItem, lastIndexItem);
			}
			newIndexItem = WillUpdateIndexItemWithPrimaryDictionaryKey(
			    primaryDictionaryKey,
			    secondaryDictionaryKey,
			    thirdaryDictionaryKey,
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
	    ItemType newItem,
	    Func<ItemType?, ItemType?, ItemType?>? toUpdateIndexItemWithNewItem = null)
	{
		return GetOrAdd(
		    primaryDictionaryKey,
		    secondaryDictionaryKey,
		    thirdaryDictionaryKey,
		    (_, _, _) => newItem,
		    toUpdateIndexItemWithNewItem);
	}

	public bool TryRemove(
	    PrimaryDictionaryKeyType primaryDictionaryKey,
	    SecondaryDictionaryKeyType secondaryDictionaryKey,
	    ThirdaryDictionaryKeyType thirdaryDictionaryKey,
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
	    out ItemType? itemRemoved)
	{
		_ = TryRemove(
		    primaryDictionaryKey,
		    secondaryDictionaryKey,
		    thirdaryDictionaryKey,
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

		//
		thirdaryDeictionaries.Clear();
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
		//
		ItemType? newIndexItem)
	{
		return newIndexItem;
	}

	#endregion
}
