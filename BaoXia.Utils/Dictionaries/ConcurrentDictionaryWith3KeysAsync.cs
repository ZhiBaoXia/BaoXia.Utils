using BaoXia.Utils.ConcurrentTools;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Dictionaries;

public class ConcurrentDictionaryWith3KeysAsync
	<ItemType,
	PrimaryDeictionaryKeyType,
	SecondaryDeictionaryKeyType,
	ThirdaryDeictionaryKeyType>
	where PrimaryDeictionaryKeyType : notnull
	where SecondaryDeictionaryKeyType : notnull
	where ThirdaryDeictionaryKeyType : notnull
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
		ConcurrentDictionary<SecondaryDeictionaryKeyType,
			ConcurrentDictionary<ThirdaryDeictionaryKeyType, DictionaryValueContainer<ItemType, ItemOperateLocker>>>> PrimaryDictionaries = new();

	private string? _name = null;
	public string? Name { get => _name; set => _name = value; }

	#endregion


	////////////////////////////////////////////////
	// @自身实现，获取数据部分。
	////////////////////////////////////////////////

	#region 自身实现，获取数据部分。

	public ConcurrentDictionary<SecondaryDeictionaryKeyType,
		ConcurrentDictionary<ThirdaryDeictionaryKeyType, DictionaryValueContainer<ItemType, ItemOperateLocker>>>? GetSecondaryDictionaries(PrimaryDeictionaryKeyType primaryDeictionaryKey)
	{
		_ = PrimaryDictionaries.TryGetValue(primaryDeictionaryKey, out var secondaryDictionaries);
		{ }
		return secondaryDictionaries;
	}

	public ConcurrentDictionary<ThirdaryDeictionaryKeyType, DictionaryValueContainer<ItemType, ItemOperateLocker>>? GetThirdaryDictionaries(
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

	public ItemType? Get(
		PrimaryDeictionaryKeyType primaryDeictionaryKey,
		SecondaryDeictionaryKeyType secondaryDeictionaryKey,
		ThirdaryDeictionaryKeyType thirdaryDeictionaryKey)
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
		if (thirdaryDictionaries.TryGetValue(
			thirdaryDeictionaryKey,
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
		out ItemType? item)
	{
		item = Get(
			primaryDeictionaryKey,
			secondaryDeictionaryKey,
			thirdaryDeictionaryKey);
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
					// !!!
					allItemsCount += thirdaryDeictionaryKeyValue.Value.ItemsCount;
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

	public async Task<ItemType?> AddAsync(
		PrimaryDeictionaryKeyType primaryDeictionaryKey,
		SecondaryDeictionaryKeyType secondaryDeictionaryKey,
		ThirdaryDeictionaryKeyType thirdaryDeictionaryKey,
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
		var itemIndexInfo
			= thirdaryDictionaries.GetOrAdd(
				thirdaryDeictionaryKey,
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
					thirdaryDeictionaryKey,
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
		ThirdaryDeictionaryKeyType thirdaryDeictionaryKey,
		Func<PrimaryDeictionaryKeyType,
			SecondaryDeictionaryKeyType,
			ThirdaryDeictionaryKeyType,
			Task<ItemType?>> toCreateItemAsync,
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
		var itemIndexInfo
			= thirdaryDictionaries.GetOrAdd(
				thirdaryDeictionaryKey,
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
					secondaryDeictionaryKey,
					thirdaryDeictionaryKey);
				if (toUpdateIndexItemWithNewItem != null)
				{
					newIndexItem = toUpdateIndexItemWithNewItem(newIndexItem, lastIndexItem);
				}
				newIndexItem = WillUpdateIndexItemWithPrimaryDeictionaryKey(
					primaryDeictionaryKey,
					secondaryDeictionaryKey,
					thirdaryDeictionaryKey,
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
		ThirdaryDeictionaryKeyType thirdaryDeictionaryKey,
		ItemType newItem,
		Func<ItemType?, ItemType?, ItemType?>? toUpdateIndexItemWithNewItem = null)
	{
		return await GetOrAddAsync(
			primaryDeictionaryKey,
			secondaryDeictionaryKey,
			thirdaryDeictionaryKey,
			async (_, _, _) => await Task.FromResult(newItem),
			toUpdateIndexItemWithNewItem);
	}

	public async Task<ItemType?> TryRemoveAsync(
		PrimaryDeictionaryKeyType primaryDeictionaryKey,
		SecondaryDeictionaryKeyType secondaryDeictionaryKey,
		ThirdaryDeictionaryKeyType thirdaryDeictionaryKey)
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
		SecondaryDeictionaryKeyType secondaryDeictionaryKey,
		ThirdaryDeictionaryKeyType thirdaryDeictionaryKey)
	{
		return await TryRemoveAsync(
			primaryDeictionaryKey,
			secondaryDeictionaryKey,
			thirdaryDeictionaryKey);
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

	protected virtual ItemType? WillUpdateIndexItemWithPrimaryDeictionaryKey(
				PrimaryDeictionaryKeyType primaryDeictionaryKey,
				SecondaryDeictionaryKeyType secondaryDeictionaryKey,
				ThirdaryDeictionaryKeyType thirdaryDeictionaryKey,
				//
				ItemType? newIndexItem)
	{
		return newIndexItem;
	}

	#endregion
}