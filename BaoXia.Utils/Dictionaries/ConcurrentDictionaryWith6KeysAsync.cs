using BaoXia.Utils.ConcurrentTools;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Dictionaries;

public class ConcurrentDictionaryWith6KeysAsync
	<PrimaryDeictionaryKeyType,
	SecondaryDeictionaryKeyType,
	ThirdaryDeictionaryKeyType,
	FourthDeictionaryKeyType,
	FifthDeictionaryKeyType,
	SixthDeictionaryKeyType,
	ItemType>
	where PrimaryDeictionaryKeyType : notnull
	where SecondaryDeictionaryKeyType : notnull
	where ThirdaryDeictionaryKeyType : notnull
	where FourthDeictionaryKeyType : notnull
	where FifthDeictionaryKeyType : notnull
	where SixthDeictionaryKeyType : notnull
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
			ConcurrentDictionary<ThirdaryDeictionaryKeyType,
				ConcurrentDictionary<FourthDeictionaryKeyType,
					ConcurrentDictionary<FifthDeictionaryKeyType,
						ConcurrentDictionary<SixthDeictionaryKeyType, DictionaryValueContainer<ItemType, ItemOperateLocker>>>>>>> PrimaryDictionaries = new();

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
					ConcurrentDictionary<SixthDeictionaryKeyType, DictionaryValueContainer<ItemType, ItemOperateLocker>>>>>>? GetSecondaryDictionaries(PrimaryDeictionaryKeyType primaryDeictionaryKey)
	{
		_ = PrimaryDictionaries.TryGetValue(primaryDeictionaryKey, out var secondaryDictionaries);
		{ }
		return secondaryDictionaries;
	}

	public ConcurrentDictionary<ThirdaryDeictionaryKeyType,
			ConcurrentDictionary<FourthDeictionaryKeyType,
				ConcurrentDictionary<FifthDeictionaryKeyType,
					ConcurrentDictionary<SixthDeictionaryKeyType, DictionaryValueContainer<ItemType, ItemOperateLocker>>>>>? GetThirdaryDictionaries(
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
					ConcurrentDictionary<SixthDeictionaryKeyType, DictionaryValueContainer<ItemType, ItemOperateLocker>>>>? GetFourthDictionaries(
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
					ConcurrentDictionary<SixthDeictionaryKeyType, DictionaryValueContainer<ItemType, ItemOperateLocker>>>? GetFifthDictionaries(
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

	public ConcurrentDictionary<SixthDeictionaryKeyType, DictionaryValueContainer<ItemType, ItemOperateLocker>>? GetSixthDictionaries(
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

	public async Task<ItemType?> AddAsync(
		PrimaryDeictionaryKeyType primaryDeictionaryKey,
		SecondaryDeictionaryKeyType secondaryDeictionaryKey,
		ThirdaryDeictionaryKeyType thirdaryDeictionaryKey,
		FourthDeictionaryKeyType fourthDeictionaryKey,
		FifthDeictionaryKeyType fifthDeictionaryKey,
		SixthDeictionaryKeyType sixthDeictionaryKey,
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
		var sixthDictionaries
			= fifthDictionaries.GetOrAdd(
				fifthDeictionaryKey,
				(_) => []);
		var itemIndexInfo = sixthDictionaries.GetOrAdd(
			sixthDeictionaryKey,
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
				return await Task.FromResult(newIndexItem);
				// !!!
			});
		return newIndexItem;
	}

	public async Task<ItemType?> GetOrAddAsync(
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
			});
		return newIndexItem;
	}

	public async Task<ItemType?> GetOrAddAsync(
		PrimaryDeictionaryKeyType primaryDeictionaryKey,
		SecondaryDeictionaryKeyType secondaryDeictionaryKey,
		ThirdaryDeictionaryKeyType thirdaryDeictionaryKey,
		FourthDeictionaryKeyType fourthDeictionaryKey,
		FifthDeictionaryKeyType fifthDeictionaryKey,
		SixthDeictionaryKeyType sixthDeictionaryKey,
		ItemType newItem,
		Func<ItemType?, ItemType?, ItemType?>? toUpdateIndexItemWithNewItem = null)
	{
		return await GetOrAddAsync(
			primaryDeictionaryKey,
			secondaryDeictionaryKey,
			thirdaryDeictionaryKey,
			fourthDeictionaryKey,
			fifthDeictionaryKey,
			sixthDeictionaryKey,
			async (_, _, _, _, _, _) => await Task.FromResult(newItem),
			toUpdateIndexItemWithNewItem);
	}

	public async Task<ItemType?> TryRemoveAsync(
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
		if (!sixthDictionaries.TryGetValue(
			sixthDeictionaryKey,
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
		ThirdaryDeictionaryKeyType thirdaryDeictionaryKey,
		FourthDeictionaryKeyType fourthDeictionaryKey,
		FifthDeictionaryKeyType fifthDeictionaryKey,
		SixthDeictionaryKeyType sixthDeictionaryKey)
	{
		return await TryRemoveAsync(
			primaryDeictionaryKey,
			secondaryDeictionaryKey,
			thirdaryDeictionaryKey,
			fourthDeictionaryKey,
			fifthDeictionaryKey,
			sixthDeictionaryKey);
	}

	public void Clear(
		PrimaryDeictionaryKeyType? primaryDeictionaryKey = default,
		SecondaryDeictionaryKeyType? secondaryDeictionaryKey = default,
		ThirdaryDeictionaryKeyType? thirdaryDeictionaryKey = default,
		FourthDeictionaryKeyType? fourthDeictionaryKey = default,
		FifthDeictionaryKeyType? fifthDeictionaryKey = default)
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


		if (secondaryDeictionaryKey == null)
		{
			secondaryDictionaries.Clear();
			return;
		}
		if (!secondaryDictionaries.TryGetValue(
			secondaryDeictionaryKey,
			out var thirdaryDeictionaries))
		{
			return;
		}


		if (thirdaryDeictionaryKey == null)
		{
			thirdaryDeictionaries.Clear();
			return;
		}
		if (!thirdaryDeictionaries.TryGetValue(
			thirdaryDeictionaryKey,
			out var fourthDeictionaries))
		{
			return;
		}


		if (fourthDeictionaryKey == null)
		{
			fourthDeictionaries.Clear();
			return;
		}
		if (!fourthDeictionaries.TryGetValue(
			fourthDeictionaryKey,
			out var fifthDeictionaries))
		{
			return;
		}


		if (fifthDeictionaryKey == null)
		{
			fifthDeictionaries.Clear();
			return;
		}
		if (!fifthDeictionaries.TryGetValue(
		       fifthDeictionaryKey,
		       out var sixthDeictionaries))
		{
			return;
		}

		//
		sixthDeictionaries.Clear();
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