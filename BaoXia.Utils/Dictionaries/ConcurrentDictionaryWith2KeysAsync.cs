using BaoXia.Utils.ConcurrentTools;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Dictionaries;

public class ConcurrentDictionaryWith2KeysAsync
	<ItemType,
	PrimaryDeictionaryKeyType,
	SecondaryDeictionaryKeyType>
	where PrimaryDeictionaryKeyType : notnull
	where SecondaryDeictionaryKeyType : notnull
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

	public readonly ConcurrentDictionary<PrimaryDeictionaryKeyType,
		ConcurrentDictionary<SecondaryDeictionaryKeyType, DictionaryValueContainer<ItemType, ItemOperateLocker>>> PrimaryDictionaries = new();

	private string? _name = null;
	public string? Name { get => _name; set => _name = value; }

	#endregion


	////////////////////////////////////////////////
	// @自身实现，获取数据部分。
	////////////////////////////////////////////////

	#region 自身实现，获取数据部分。

	public ConcurrentDictionary<SecondaryDeictionaryKeyType, DictionaryValueContainer<ItemType, ItemOperateLocker>>? GetSecondaryDictionaries(
		PrimaryDeictionaryKeyType primaryDeictionaryKey)
	{
		_ = PrimaryDictionaries.TryGetValue(primaryDeictionaryKey, out var secondaryDictionaries);
		{ }
		return secondaryDictionaries;
	}

	public ItemType? Get(
		PrimaryDeictionaryKeyType primaryDeictionaryKey,
		SecondaryDeictionaryKeyType secondaryDeictionaryKey)
	{
		if (!PrimaryDictionaries.TryGetValue(
			primaryDeictionaryKey,
			out var secondaryDictionaries))
		{
			return default;
		}
		if (secondaryDictionaries.TryGetValue(
			secondaryDeictionaryKey,
			out var enityIndexInfo))
		{
			return enityIndexInfo.FirstItem;
		}
		return default;
	}

	public bool TryGet(
		PrimaryDeictionaryKeyType primaryDeictionaryKey,
		SecondaryDeictionaryKeyType secondaryDeictionaryKey,
		out ItemType? item)
	{
		item = Get(
			primaryDeictionaryKey,
			secondaryDeictionaryKey);
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
				// !!!
				allItemsCount += secondaryDeictionaryKeyValue.Value.ItemsCount;
				// !!!
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
		PrimaryDeictionaryKeyType primaryDeictionaryKey,
		SecondaryDeictionaryKeyType secondaryDeictionaryKey,
		ItemType item,
		Func<ItemType, ItemType?, ItemType?>? toUpdateIndexItemWithNewItem = null)
	{
		var secondaryDictionaries
			= PrimaryDictionaries.GetOrAdd(
				primaryDeictionaryKey,
				(_) => []);
		var itemIndexInfo
			= secondaryDictionaries.GetOrAdd(
				secondaryDeictionaryKey,
				(_) => DidCreateDictionaryValueContainer());
		var newIndexItem
			= await AsyncLocker.LockAsync(
			itemIndexInfo.ItemOperateLocker,
			null,
			async (_) =>
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
				return await Task.FromResult(newIndexItem);
				// !!!
			});
		return newIndexItem;
	}

	public async Task<ItemType?> GetOrAddAsync(
		PrimaryDeictionaryKeyType primaryDeictionaryKey,
		SecondaryDeictionaryKeyType secondaryDeictionaryKey,
		Func<PrimaryDeictionaryKeyType,
			SecondaryDeictionaryKeyType,
			Task<ItemType?>> toCreateItemAsync,
		Func<ItemType?, ItemType?, ItemType?>? toUpdateIndexItemWithNewItem = null)
	{
		var secondaryDictionaries
			= PrimaryDictionaries.GetOrAdd(
				primaryDeictionaryKey,
				(_) => []);
		var itemIndexInfo
			= secondaryDictionaries.GetOrAdd(
				secondaryDeictionaryKey,
				(_) => DidCreateDictionaryValueContainer());
		var lastIndexItem = itemIndexInfo.FirstItem;
		if (lastIndexItem != null)
		{
			return lastIndexItem;
		}
		var newIndexItem
			= await AsyncLocker.LockAsync(
			itemIndexInfo.ItemOperateLocker,
			null,
			async (_) =>
			{
				lastIndexItem = itemIndexInfo.FirstItem;
				if (lastIndexItem != null)
				{
					return lastIndexItem;
				}

				// !!!
				var newIndexItem
				= await toCreateItemAsync(
					primaryDeictionaryKey,
					secondaryDeictionaryKey);
				if (toUpdateIndexItemWithNewItem != null)
				{
					newIndexItem = toUpdateIndexItemWithNewItem(newIndexItem, lastIndexItem);
				}
				newIndexItem = WillUpdateIndexItemWithPrimaryDeictionaryKey(
					primaryDeictionaryKey,
					secondaryDeictionaryKey,
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
			});
		return newIndexItem;
	}

	public async Task<ItemType?> GetOrAddAsync(
		PrimaryDeictionaryKeyType primaryDeictionaryKey,
		SecondaryDeictionaryKeyType secondaryDeictionaryKey,
		ItemType newItem,
		Func<ItemType?, ItemType?, ItemType?>? toUpdateIndexItemWithNewItem = null)
	{
		return await GetOrAddAsync(
			primaryDeictionaryKey,
			secondaryDeictionaryKey,
			async (_, _) => await Task.FromResult(newItem),
			toUpdateIndexItemWithNewItem);
	}

	public async Task<ItemType?> TryRemoveAsync(
		PrimaryDeictionaryKeyType primaryDeictionaryKey,
		SecondaryDeictionaryKeyType secondaryDeictionaryKey)
	{
		if (!PrimaryDictionaries.TryGetValue(
			primaryDeictionaryKey,
			out var secondaryDictionaries))
		{
			return default;
		}
		if (!secondaryDictionaries.TryGetValue(
			secondaryDeictionaryKey,
			out var itemIndexInfo))
		{
			return default;
		}

		var itemRemoved
			= await AsyncLocker.LockAsync(
				itemIndexInfo.ItemOperateLocker,
				null,
				async (_) =>
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
		PrimaryDeictionaryKeyType primaryDeictionaryKey,
		SecondaryDeictionaryKeyType secondaryDeictionaryKey)
	{
		return await TryRemoveAsync(
			primaryDeictionaryKey,
			secondaryDeictionaryKey);
	}

	public void Clear(
		PrimaryDeictionaryKeyType? primaryDeictionaryKey = default)
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

		//
		secondaryDictionaries.Clear();
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

	protected virtual ItemType? WillUpdateIndexItemWithPrimaryDeictionaryKey(
				PrimaryDeictionaryKeyType primaryDeictionaryKey,
				SecondaryDeictionaryKeyType secondaryDeictionaryKey,
				//
				ItemType? newIndexItem)
	{
		return newIndexItem;
	}

	#endregion
}