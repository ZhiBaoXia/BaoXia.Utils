using BaoXia.Utils.ConcurrentTools;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Dictionaries;

public class ConcurrentDictionaryWith4KeysAsync
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
	// @静态常量
	////////////////////////////////////////////////

	#region 静态常量
	public class ItemOperateLocker : SemaphoreSlim
	{
		////////////////////////////////////////////////
		// @自身实现
		////////////////////////////////////////////////

		#region 自身实现

		public ItemOperateLocker(
		    int initialCount)
		    : base(initialCount)
		{ }

		public ItemOperateLocker(
		    int initialCount, int maxCount)
		    : base(initialCount, maxCount)
		{ }

		#endregion
	}

	#endregion


	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public readonly ConcurrentDictionary<PrimaryDictionaryKeyType,
	    ConcurrentDictionary<SecondaryDictionaryKeyType,
	    ConcurrentDictionary<ThirdaryDictionaryKeyType,
		ConcurrentDictionary<FourthDictionaryKeyType, DictionaryValueContainer<ItemType, ItemOperateLocker>>>>> PrimaryDictionaries = new();

	private string? _name = null;
	public string? Name { get => _name; set => _name = value; }

	#endregion


	////////////////////////////////////////////////
	// @自身实现，获取数据部分。
	////////////////////////////////////////////////

	#region 自身实现，获取数据部分。

	public ConcurrentDictionary<SecondaryDictionaryKeyType,
	    ConcurrentDictionary<ThirdaryDictionaryKeyType,
	    ConcurrentDictionary<FourthDictionaryKeyType, DictionaryValueContainer<ItemType, ItemOperateLocker>>>>? GetSecondaryDictionaries(PrimaryDictionaryKeyType primaryDictionaryKey)
	{
		_ = PrimaryDictionaries.TryGetValue(primaryDictionaryKey, out var secondaryDictionaries);
		{ }
		return secondaryDictionaries;
	}

	public ConcurrentDictionary<ThirdaryDictionaryKeyType,
	    ConcurrentDictionary<FourthDictionaryKeyType, DictionaryValueContainer<ItemType, ItemOperateLocker>>>? GetThirdaryDictionaries(
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

	public ConcurrentDictionary<FourthDictionaryKeyType, DictionaryValueContainer<ItemType, ItemOperateLocker>>? GetFourthDictionaries(
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
		item = Get(
		    primaryDictionaryKey,
		    secondaryDictionaryKey,
		    thirdaryDictionaryKey,
		    fourthDictionaryKey);
		if (item != null)
		{
			return true;
		}
		return false;
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
					var fourthDeictionaries = thirdaryDictionaryKeyValue.Value;
					foreach (var fourthDictionaryKeyValue in fourthDeictionaries)
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

	public async Task<ItemType?> AddAsync(
	    PrimaryDictionaryKeyType primaryDictionaryKey,
	    SecondaryDictionaryKeyType secondaryDictionaryKey,
	    ThirdaryDictionaryKeyType thirdaryDictionaryKey,
	    FourthDictionaryKeyType fourthDictionaryKey,
	    ItemType? item,
	    Func<ItemType?, ItemType?, ItemType?>? toUpdateIndexItemWithNewItem = null)
	{
		var secondaryDictionaries = PrimaryDictionaries.GetOrAdd(primaryDictionaryKey, (_) => []);
		var thirdaryDictionaries = secondaryDictionaries.GetOrAdd(secondaryDictionaryKey, (_) => []);
		var fourthDictionaries = thirdaryDictionaries.GetOrAdd(thirdaryDictionaryKey, (_) => []);
		var itemIndexInfo = fourthDictionaries.GetOrAdd(fourthDictionaryKey, (_) => DidCreateDictionaryValueContainer());
		var newIndexItem = await AsyncLock.LockAsync(null, () => itemIndexInfo.ItemOperateLocker, async (_) =>
		{
			// !!!
			var lastIndexItem = itemIndexInfo.FirstItem;
			var newIndexItem = item;
			if (toUpdateIndexItemWithNewItem != null)
			{
				newIndexItem = toUpdateIndexItemWithNewItem(item, lastIndexItem);
			}
			newIndexItem = WillUpdateIndexItemWithPrimaryDictionaryKey(
				primaryDictionaryKey, secondaryDictionaryKey, thirdaryDictionaryKey, fourthDictionaryKey, newIndexItem);
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
			return await Task.FromResult(newIndexItem);
			// !!!
		});
		return newIndexItem;
	}

	public async Task<ItemType?> GetOrAddAsync(
	    PrimaryDictionaryKeyType primaryDictionaryKey,
	    SecondaryDictionaryKeyType secondaryDictionaryKey,
	    ThirdaryDictionaryKeyType thirdaryDictionaryKey,
	    FourthDictionaryKeyType fourthDictionaryKey,
	    Func<PrimaryDictionaryKeyType,
	    SecondaryDictionaryKeyType,
	    ThirdaryDictionaryKeyType,
	    FourthDictionaryKeyType,
	    Task<ItemType?>> toCreateItemAsync,
	    Func<ItemType?, ItemType?, ItemType?>? toUpdateIndexItemWithNewItem = null)
	{
		var secondaryDictionaries = PrimaryDictionaries.GetOrAdd(primaryDictionaryKey, (_) => []);
		var thirdaryDictionaries = secondaryDictionaries.GetOrAdd(secondaryDictionaryKey, (_) => []);
		var fourthDictionaries = thirdaryDictionaries.GetOrAdd(thirdaryDictionaryKey, (_) => []);
		var itemIndexInfo = fourthDictionaries.GetOrAdd(fourthDictionaryKey, (_) => DidCreateDictionaryValueContainer());
		var lastIndexItem = itemIndexInfo.FirstItem;
		if (lastIndexItem != null)
		{
			return lastIndexItem;
		}
		var newIndexItem = await AsyncLock.LockAsync(null, () => itemIndexInfo.ItemOperateLocker, async (_) =>
		{
			lastIndexItem = itemIndexInfo.FirstItem;
			if (lastIndexItem != null)
			{
				return lastIndexItem;
			}

			// !!!
			var newIndexItem = await toCreateItemAsync(primaryDictionaryKey, secondaryDictionaryKey, thirdaryDictionaryKey,
				fourthDictionaryKey);
			if (toUpdateIndexItemWithNewItem != null)
			{
				newIndexItem = toUpdateIndexItemWithNewItem(newIndexItem, lastIndexItem);
			}
			newIndexItem = WillUpdateIndexItemWithPrimaryDictionaryKey(primaryDictionaryKey, secondaryDictionaryKey,
				thirdaryDictionaryKey, fourthDictionaryKey, newIndexItem);
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
		});
		return newIndexItem;
	}

	public async Task<ItemType?> GetOrAddAsync(
	    PrimaryDictionaryKeyType primaryDictionaryKey,
	    SecondaryDictionaryKeyType secondaryDictionaryKey,
	    ThirdaryDictionaryKeyType thirdaryDictionaryKey,
	    FourthDictionaryKeyType fourthDictionaryKey,
	    ItemType newItem,
	    Func<ItemType?, ItemType?, ItemType?>? toUpdateIndexItemWithNewItem = null)
	{
		return await GetOrAddAsync(
		    primaryDictionaryKey,
		    secondaryDictionaryKey,
		    thirdaryDictionaryKey,
		    fourthDictionaryKey,
		    async (_, _, _, _) => await Task.FromResult(newItem),
		    toUpdateIndexItemWithNewItem);
	}

	public async Task<ItemType?> TryRemoveAsync(
	    PrimaryDictionaryKeyType primaryDictionaryKey,
	    SecondaryDictionaryKeyType secondaryDictionaryKey,
	    ThirdaryDictionaryKeyType thirdaryDictionaryKey,
	    FourthDictionaryKeyType fourthDictionaryKey)
	{
		if (!PrimaryDictionaries.TryGetValue(primaryDictionaryKey, out var secondaryDictionaries))
		{
			return default;
		}
		if (!secondaryDictionaries.TryGetValue(secondaryDictionaryKey, out var thirdaryDictionaries))
		{
			return default;
		}
		if (!thirdaryDictionaries.TryGetValue(thirdaryDictionaryKey, out var fourthDictionaries))
		{
			return default;
		}
		if (!fourthDictionaries.TryGetValue(fourthDictionaryKey, out var itemIndexInfo))
		{
			return default;
		}

		var itemRemoved = await AsyncLock.LockAsync(null, () => itemIndexInfo.ItemOperateLocker, async (_) =>
		{
			// !!!
			var itemRemoved = itemIndexInfo.FirstItem;
			// !!!
			if (itemRemoved == null)
			{
				return default;
			}
			// !!!
			itemIndexInfo.Items = [];
			// !!!
			return await Task.FromResult(itemRemoved);
		});
		return itemRemoved;
	}

	public async Task<ItemType?> RemoveAsync(
	    PrimaryDictionaryKeyType primaryDictionaryKey,
	    SecondaryDictionaryKeyType secondaryDictionaryKey,
	    ThirdaryDictionaryKeyType thirdaryDictionaryKey,
	    FourthDictionaryKeyType fourthDictionaryKey)
	{
		return await TryRemoveAsync(
		    primaryDictionaryKey,
		    secondaryDictionaryKey,
		    thirdaryDictionaryKey,
		    fourthDictionaryKey);
	}

	public void Clear(
	    PrimaryDictionaryKeyType? primaryDictionaryKey = default,
	    SecondaryDictionaryKeyType? secondaryDictionaryKey = default,
	    ThirdaryDictionaryKeyType? thirdaryDictionaryKey = default)
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


		if (secondaryDictionaryKey == null)
		{
			secondaryDictionaries.Clear();
			return;
		}
		if (!secondaryDictionaries.TryGetValue(
		    secondaryDictionaryKey,
		    out var thirdaryDeictionaries))
		{
			return;
		}


		if (thirdaryDictionaryKey == null)
		{
			thirdaryDeictionaries.Clear();
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

	protected DictionaryValueContainer<ItemType, ItemOperateLocker> DidCreateDictionaryValueContainer()
	{
		return new DictionaryValueContainer<ItemType, ItemOperateLocker>(new ItemOperateLocker(1));
	}

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
