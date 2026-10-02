using BaoXia.Utils.ConcurrentTools;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Dictionaries;

public class ConcurrentDictionaryWith1KeyAsync
    <PrimaryDictionaryKeyType, ItemType>
    where PrimaryDictionaryKeyType : notnull
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

	public readonly ConcurrentDictionary<PrimaryDictionaryKeyType, DictionaryValueContainer<ItemType, ItemOperateLocker>> PrimaryDictionaries = new();

	private string? _name = null;
	public string? Name { get => _name; set => _name = value; }

	#endregion


	////////////////////////////////////////////////
	// @自身实现，获取数据部分。
	////////////////////////////////////////////////

	#region 自身实现，获取数据部分。

	public ItemType? Get(
	    PrimaryDictionaryKeyType primaryDictionaryKey)
	{
		if (PrimaryDictionaries.TryGetValue(
		    primaryDictionaryKey,
		    out var enityIndexInfo))
		{
			return enityIndexInfo.FirstItem;
		}
		return default;
	}

	public bool TryGet(
	    PrimaryDictionaryKeyType primaryDictionaryKey,
	    out ItemType? item)
	{
		item = Get(
		    primaryDictionaryKey);
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
			// !!!
			allItemsCount += primaryDictionaryKeyValue.Value.ItemsCount;
			// !!!
		}
		return allItemsCount;
	}

	#endregion


	////////////////////////////////////////////////
	// @自身实现，更新数据部分。
	////////////////////////////////////////////////

	#region 自身实现，更新数据部分。

	public async Task<ItemType?> AddAsync(PrimaryDictionaryKeyType primaryDictionaryKey, ItemType? item,
	    Func<ItemType?, ItemType?, ItemType?>? toUpdateIndexItemWithNewItem = null)
	{
		var itemIndexInfo = PrimaryDictionaries.GetOrAdd(primaryDictionaryKey, (_) => DidCreateDictionaryValueContainer());
		var newIndexItem = await AsyncLock.LockAsync(null, () => itemIndexInfo.ItemOperateLocker, async (_) =>
		{
			// !!!
			var lastIndexItem = itemIndexInfo.FirstItem;
			var newIndexItem = item;
			if (toUpdateIndexItemWithNewItem != null)
			{
				newIndexItem = toUpdateIndexItemWithNewItem(item, lastIndexItem);
			}
			newIndexItem = WillUpdateIndexItemWithPrimaryDictionaryKey(primaryDictionaryKey, newIndexItem);
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

	public async Task<ItemType?> GetOrAddAsync(PrimaryDictionaryKeyType primaryDictionaryKey,
	    Func<PrimaryDictionaryKeyType, Task<ItemType?>> toCreateItemAsync,
	    Func<ItemType?, ItemType?, ItemType?>? toUpdateIndexItemWithNewItem = null)
	{
		var itemIndexInfo = PrimaryDictionaries.GetOrAdd(primaryDictionaryKey, (_) => DidCreateDictionaryValueContainer());
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
			var newIndexItem = await toCreateItemAsync(primaryDictionaryKey);
			if (toUpdateIndexItemWithNewItem != null)
			{
				newIndexItem = toUpdateIndexItemWithNewItem(newIndexItem, lastIndexItem);
			}
			newIndexItem = WillUpdateIndexItemWithPrimaryDictionaryKey(primaryDictionaryKey, newIndexItem);
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
	    ItemType newItem,
	    Func<ItemType?, ItemType?, ItemType?>? toUpdateIndexItemWithNewItem = null)
	{
		return await GetOrAddAsync(
		    primaryDictionaryKey,
		    async (_) => await Task.FromResult(newItem),
		    toUpdateIndexItemWithNewItem);
	}

	public async Task<ItemType?> TryRemoveAsync(
	    PrimaryDictionaryKeyType primaryDictionaryKey)
	{
		if (!PrimaryDictionaries.TryGetValue(primaryDictionaryKey, out var itemIndexInfo))
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
	    PrimaryDictionaryKeyType primaryDictionaryKey)
	{
		return await TryRemoveAsync(primaryDictionaryKey);
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

	protected DictionaryValueContainer<ItemType, ItemOperateLocker> DidCreateDictionaryValueContainer()
	{
		return new DictionaryValueContainer<ItemType, ItemOperateLocker>(new ItemOperateLocker(1));
	}

	protected virtual ItemType? WillUpdateIndexItemWithPrimaryDictionaryKey
	    (PrimaryDictionaryKeyType primaryDictionaryKey,
	    //
	    ItemType? newIndexItem)
	{
		return newIndexItem;
	}

	#endregion
}