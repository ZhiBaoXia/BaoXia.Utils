using BaoXia.Utils.ConcurrentTools;
using BaoXia.Utils.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace BaoXia.Utils.Extensions;

public static class IEnumerableExtension
{
	public static int GetCount(this IEnumerable? items)
	{
		if (items == null)
		{
			return 0;
		}

		if (items is ICollection collection)
		{
			return collection.Count;
		}

		var itemsCount = 0;
		foreach (var item in items)
		{
			itemsCount++;
		}
		return itemsCount;
	}

	public static void ForEachAtLeastOnce<ItemType>(
		this IEnumerable<ItemType>? items,
		Func<ItemType?, bool> toReceiveItem)
	{
		var itemsEnumerator = items?.GetEnumerator();
		for (var item
			= (itemsEnumerator?.MoveNext() == true
			? itemsEnumerator.Current
			: default)
			;
			;)
		{
			////////////////////////////////////////////////
			if (toReceiveItem(item) != true)
			{
				break;
			}
			////////////////////////////////////////////////

			if (itemsEnumerator?.MoveNext() != true)
			{
				break;
			}
			item = itemsEnumerator.Current;
		}
	}

	public static async Task ForEachAtLeastOnceAsync<ItemType>(
		this IEnumerable<ItemType>? items,
		Func<ItemType?, Task<bool>> toReceiveItemAsync)
	{
		var itemsEnumerator = items?.GetEnumerator();
		for (var item
			= (itemsEnumerator?.MoveNext() == true
			? itemsEnumerator.Current
			: default)
			;
			;)
		{
			////////////////////////////////////////////////
			if (await toReceiveItemAsync(item) != true)
			{
				break;
			}
			////////////////////////////////////////////////

			if (itemsEnumerator?.MoveNext() != true)
			{
				break;
			}
			item = itemsEnumerator.Current;
		}
	}

	public static void ForEach<ItemType>(
		this IEnumerable<ItemType>? items,
		Func<ItemType, bool> toEnumerateItem)
	{
		if (items == null)
		{
			return;
		}

		foreach (var item in items)
		{
			if (!toEnumerateItem(item))
			{
				break;
			}
		}
	}

	public static bool IsEmpty<ItemType>([NotNullWhen(false)] this IEnumerable<ItemType>? items)
	{
		if (items == null)
		{
			return true;
		}

		if (items is ICollection collection)
		{
			return collection.Count < 1;
		}

		foreach (var _ in items)
		{
			return false;
		}
		return true;
	}

	public static bool IsNotEmpty<ItemType>([NotNullWhen(true)] this IEnumerable<ItemType>? items)
	{
		return !IsEmpty(items);
	}

	public static bool IsContains<ItemType>(
		this IEnumerable<ItemType> items,
		ItemType? objectItem,
		out int objectItemIndexInItems)
		where ItemType : notnull
	{
		objectItemIndexInItems = -1;

		if (objectItem != null)
		{
			var keyIndex = 0;
			foreach (var item in items)
			{
				if (item.Equals(objectItem))
				{
					// !!!
					objectItemIndexInItems = keyIndex;
					// !!!
					return true;
				}
				keyIndex++;
			}
		}
		return false;
	}

	public static bool IsNotContains<ItemType>(
		this IEnumerable<ItemType> items,
		ItemType? objectItem,
		out int objectItemIndexInItems)
		where ItemType : notnull
	{
		return !IEnumerableExtension.IsContains(
			items,
			objectItem,
			out objectItemIndexInItems);
	}

	public static bool IsContains<ItemType>(
		this IEnumerable<ItemType> items,
		ItemType? objectItem)
		where ItemType : notnull
	{
		return IEnumerableExtension.IsContains(
			items,
			objectItem,
			out _);
	}

	public static bool IsNotContains<ItemType>(
		this IEnumerable<ItemType> items,
		ItemType? objectItem)
		where ItemType : notnull
	{
		return !IEnumerableExtension.IsContains(
			items,
			objectItem);
	}

	public static bool IsContains<ItemType>(
		this IEnumerable<ItemType> items,
		IEnumerable<ItemType>? objectItems)
		where ItemType : notnull
	{
		if (objectItems != null)
		{
			var isObjectItemExisted = false;
			foreach (var objectItem in objectItems)
			{
				// !!!
				isObjectItemExisted = true;
				// !!!
				if (items.IsContains(objectItem) != true)
				{
					return false;
				}
			}
			if (isObjectItemExisted)
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsNotContains<ItemType>(
		this IEnumerable<ItemType> items,
		IEnumerable<ItemType>? objectItems)
		where ItemType : notnull
	{
		return !IEnumerableExtension.IsContains(
			items,
			objectItems);
	}

	public static bool IsContainsAny<ItemType>(
		this IEnumerable<ItemType> items,
		IEnumerable<ItemType>? objectItems,
		bool isNullEqualsEmpty = true)
		where ItemType : notnull
	{
		if (objectItems != null)
		{
			foreach (var objectItem in objectItems)
			{
				if (items.IsContains(objectItem) == true)
				{
					return true;
				}
			}
		}
		else if (isNullEqualsEmpty
			&& items.Any() == false)
		{
			return true;
		}
		return false;
	}

	public static bool IsNotContainsAny<ItemType>(
		this IEnumerable<ItemType> items,
		IEnumerable<ItemType>? objectItems,
		bool isNullEqualsEmpty = true)
		where ItemType : notnull
	{
		return !IEnumerableExtension.IsContainsAny(
			items,
			objectItems,
			isNullEqualsEmpty);
	}


	public static bool IsEquals<ItemType>(
		this IEnumerable<ItemType> items,
		IEnumerable<ItemType>? objectItems,
		bool isIgnoreSameItems = false,
		bool isNullEqualsEmpty = true)
		where ItemType : notnull
	{
		if (objectItems != null)
		{
			if (isIgnoreSameItems)
			{
				List<int>? itemIndexesMatched = null;
				var objectItemsCount = 0;
				foreach (var objectItem in objectItems)
				{
					if (items.IsContains(
						objectItem,
						out var itemIndexMatched) != true)
					{
						return false;
					}
					// !!!
					objectItemsCount++;
					// !!!
					{
						itemIndexesMatched ??= [];
					}
					itemIndexesMatched.Add(itemIndexMatched);
				}
				foreach (var item in items)
				{
					objectItemsCount--;
				}
				if (objectItemsCount < 0)
				{
					if (itemIndexesMatched != null)
					{
						var itemIndex = 0;
						foreach (var item in items)
						{
							if (itemIndexesMatched.Contains(itemIndex) != true)
							{
								if (objectItems.IsContains(item) != true)
								{
									return false;
								}
							}
							// !!!
							itemIndex++;
							// !!!
						}
						return true;
					}
					return false;
				}
			}
			else
			{
				var objectItemsCount = 0;
				foreach (var objectItem in objectItems)
				{
					if (items.IsContains(objectItem) != true)
					{
						return false;
					}
					// !!!
					objectItemsCount++;
					// !!!
				}
				foreach (var item in items)
				{
					objectItemsCount--;
				}
				if (objectItemsCount != 0)
				{
					return false;
				}
			}
			return true;
		}
		else if (isNullEqualsEmpty == true)
		{
			var itemingsEnumerator = items.GetEnumerator();
			if (itemingsEnumerator != null)
			{
				itemingsEnumerator.Reset();
				if (itemingsEnumerator.MoveNext() == false)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool IsNotEquals<ItemType>(
		this IEnumerable<ItemType> items,
		IEnumerable<ItemType>? objectItems,
		bool isIgnoreSameItems = false,
		bool isNullEqualsEmpty = true)
		where ItemType : notnull
	{
		return !IEnumerableExtension.IsEquals(
			items,
			objectItems,
			isIgnoreSameItems,
			isNullEqualsEmpty);
	}

	public static List<ItemType>? ToListBy<ItemType>(
		this IEnumerable<ItemType>? items,
		Func<ItemType, bool> toIsItemValidToList)
	{
		if (items == null)
		{
			return null;
		}

		var newList = new List<ItemType>();
		foreach (var item in items)
		{
			if (toIsItemValidToList(item))
			{
				newList.Add(item);
			}
		}
		return newList;
	}

	public static List<ItemType>[]? ToGroupsBy<ItemType, ItemGroupKeyType>(
		this IEnumerable<ItemType>? items,
		Func<ItemType, ItemGroupKeyType> toGetItemGroupKey)
		where ItemGroupKeyType : notnull
	{
		if (items == null)
		{
			return null;
		}

		var itemGroups = new Dictionary<ItemGroupKeyType, List<ItemType>>();
		foreach (var item in items)
		{
			var itemGroupKey = toGetItemGroupKey(item);
			if (!itemGroups.TryGetValue(
				itemGroupKey,
				out var itemGroup))
			{
				itemGroup = [];
				itemGroups.AddOrSet(itemGroupKey, itemGroup);
			}
			itemGroup.Add(item);
		}
		return [.. itemGroups.Values];
	}

	public static Dictionary<KeyType, bool>? ToDictionaryWithValueTrue<KeyType>(
		this IEnumerable<KeyType>? keys)
		where KeyType : notnull
	{
		if (keys == null)
		{
			return null;
		}

		var dictionary = new Dictionary<KeyType, bool>();
		foreach (var key in keys)
		{
			dictionary.AddOrSet(key, true);
		}
		return dictionary;
	}


	public static async Task ConcurrentProcessItemsAsync<ItemType>(
		this IEnumerable<ItemType> items,
		Action<ItemType> toProcessItem,
		int concurrentTasksCountMax = 10)
	{
		var itemsConcurrentProcessQueue
			= new ItemsConcurrentProcessQueue<ItemType>(concurrentTasksCountMax);
		foreach (var item in items)
		{
			// !!!
			itemsConcurrentProcessQueue.ProcessItem(item, toProcessItem);
			// !!!
		}
		// !!!
		await itemsConcurrentProcessQueue.WhenAll();
		// !!!
	}

	public static async Task ConcurrentProcessItemsAsync<ItemType>(
		this IEnumerable<ItemType> items,
		Func<ItemType, Task> toProcessItemAsync,
		int concurrentTasksCountMax = 10)
	{
		var itemsConcurrentProcessQueue
			= new ItemsConcurrentProcessQueue<ItemType>(concurrentTasksCountMax);
		foreach (var item in items)
		{
			// !!!
			itemsConcurrentProcessQueue.ProcessItem(item, toProcessItemAsync);
			// !!!
		}
		// !!!
		await itemsConcurrentProcessQueue.WhenAll();
		// !!!
	}

	public static async Task<ItemSearchResult<ItemType>?> SearchAsync<ItemType>(
		this IEnumerable<ItemType> items,
		//
		int searchTasksCount,
		Func<ItemType, double>? toGetItemSearchMatchedProgress,
		Func<List<ItemSearchMatchInfo<ItemType>>, List<ItemSearchMatchInfo<ItemType>>>? toSortItemSearchMatchInfes,
		Func<List<ItemSearchMatchInfo<ItemType>>, Task<List<ItemSearchMatchInfo<ItemType>>>>? toSortItemSearchMatchInfesAsync,
		//
		int pageIndex,
		int pageSize,
		//
		bool isGetItemSearchMatchInfesInPage = false)
	{
		var itemsCount = items.GetCount();
		if (itemsCount < 1)
		{
			return null;
		}

		var itemPageBeginItemIndex = pageIndex * pageSize;
		if (itemPageBeginItemIndex < 0)
		{
			itemPageBeginItemIndex = 0;
		}
		var itemPageEndItemIndex = itemPageBeginItemIndex + pageSize;
		if (itemPageEndItemIndex > itemsCount)
		{
			itemPageEndItemIndex = itemsCount;
		}
		if (itemPageBeginItemIndex >= itemsCount
		    || itemPageEndItemIndex <= itemPageBeginItemIndex)
		{
			return null;
		}

		////////////////////////////////////////////////
		// 1/，不需要搜索匹配和排序时，直接按有效的分页索引返回实体。
		////////////////////////////////////////////////

		List<ItemType> itemsSearchedInPage;
		List<ItemSearchMatchInfo<ItemType>>? itemSearchMatchInfesInPage = null;
		if (toGetItemSearchMatchedProgress == null
			&& toSortItemSearchMatchInfes == null
			&& toSortItemSearchMatchInfesAsync == null)
		{
			itemsSearchedInPage = [];
			if (isGetItemSearchMatchInfesInPage)
			{
				itemSearchMatchInfesInPage = [];
				if (items is ICollection<ItemType> itemsCollection)
				{
					for (var itemIndex = itemPageBeginItemIndex;
						itemIndex < itemPageEndItemIndex;
						itemIndex++)
					{
						var item = itemsCollection.ElementAt(itemIndex);
						// !!!
						itemsSearchedInPage.Add(item);
						itemSearchMatchInfesInPage.Add(new(item, 1.0));
						itemIndex++;
						// !!!
					}
				}
				else
				{
					var itemIndex = 0;
					foreach (var item in items)
					{
						if (itemIndex < itemPageBeginItemIndex)
						{
							itemIndex++;
							continue;
						}
						if (itemIndex >= itemPageEndItemIndex)
						{
							break;
						}
						// !!!
						itemsSearchedInPage.Add(item);
						itemSearchMatchInfesInPage.Add(new(item, 1.0));
						itemIndex++;
						// !!!
					}
				}
			}
			else
			{
				if (items is ICollection<ItemType> itemsCollection)
				{
					for (var itemIndex = itemPageBeginItemIndex;
						itemIndex < itemPageEndItemIndex;
						itemIndex++)
					{
						var item = itemsCollection.ElementAt(itemIndex);
						// !!!
						itemsSearchedInPage.Add(item);
						itemIndex++;
						// !!!
					}
				}
				else
				{
					var itemIndex = 0;
					foreach (var item in items)
					{
						if (itemIndex < itemPageBeginItemIndex)
						{
							itemIndex++;
							continue;
						}
						if (itemIndex >= itemPageEndItemIndex)
						{
							break;
						}
						// !!!
						itemsSearchedInPage.Add(item);
						itemIndex++;
						// !!!
					}
				}
			}
			//
			return new(
				itemsCount,
				itemsSearchedInPage,
				itemSearchMatchInfesInPage);
			//
		}

		////////////////////////////////////////////////
		// 2/，创建元素的搜索匹配信息。
		////////////////////////////////////////////////

		List<ItemSearchMatchInfo<ItemType>> itemSearchMatchInfes = [];
		await items.ConcurrentProcessItemsAsync(
			(item) =>
			{
				var itemSearchMatchedProgress = 0.0;
				if (toGetItemSearchMatchedProgress != null)
				{
					itemSearchMatchedProgress = toGetItemSearchMatchedProgress(item);
				}
				if (itemSearchMatchedProgress <= 0)
				{
					return;
				}
				lock (itemSearchMatchInfes)
				{
					itemSearchMatchInfes.Add(new(
						item,
						itemSearchMatchedProgress));
				}
			},
			searchTasksCount);

		var itemSearchMatchInfesCount = itemSearchMatchInfes.Count;
		if (itemPageEndItemIndex > itemSearchMatchInfesCount)
		{
			itemPageEndItemIndex = itemSearchMatchInfesCount;
		}
		if (itemPageBeginItemIndex >= itemSearchMatchInfesCount
		    || itemPageEndItemIndex <= itemPageBeginItemIndex)
		{
			return null;
		}


		////////////////////////////////////////////////
		// 3/，根据搜索匹配信息排序元素。
		////////////////////////////////////////////////
		if (toSortItemSearchMatchInfes != null)
		{
			itemSearchMatchInfes
			    = toSortItemSearchMatchInfes(itemSearchMatchInfes);
		}
		else if (toSortItemSearchMatchInfesAsync != null)
		{
			itemSearchMatchInfes = await toSortItemSearchMatchInfesAsync(itemSearchMatchInfes);
		}
		else
		{
			itemSearchMatchInfes.Sort((
			    searchMatchInfoA,
			    searchMatchInfoB) =>
			{
				return searchMatchInfoB.MatchedProgress.CompareTo(
					searchMatchInfoA.MatchedProgress);
			});
		}

		////////////////////////////////////////////////
		// 4/，根据搜索、排序后的结果进行分页。
		////////////////////////////////////////////////
		itemsSearchedInPage = [];
		if (isGetItemSearchMatchInfesInPage)
		{
			itemSearchMatchInfesInPage = [];
			for (var itemIndex = itemPageBeginItemIndex;
			    itemIndex < itemPageEndItemIndex;
			    itemIndex++)
			{
				var itemSearchMatchInfo = itemSearchMatchInfes[itemIndex];
				// !!!
				itemsSearchedInPage.Add(itemSearchMatchInfo.Item);
				itemSearchMatchInfesInPage.Add(itemSearchMatchInfo);
				// !!!
			}
		}
		else
		{
			for (var itemIndex = itemPageBeginItemIndex;
			    itemIndex < itemPageEndItemIndex;
			    itemIndex++)
			{
				var itemSearchMatchInfo = itemSearchMatchInfes[itemIndex];
				// !!!
				itemsSearchedInPage.Add(itemSearchMatchInfo.Item);
				// !!!
			}
		}
		//
		return new(
			itemSearchMatchInfes.Count,
			itemsSearchedInPage,
			itemSearchMatchInfesInPage);
		//
	}


	public static async Task<ItemSearchResult<ItemType>?> SearchAsync<ItemType>(
		this IEnumerable<ItemType> items,
		//
		int searchTasksCount,
		Func<ItemType, Task<double>>? toGetItemSearchMatchedProgressAsync,
		Func<List<ItemSearchMatchInfo<ItemType>>, List<ItemSearchMatchInfo<ItemType>>>? toSortItemSearchMatchInfes,
		Func<List<ItemSearchMatchInfo<ItemType>>, Task<List<ItemSearchMatchInfo<ItemType>>>>? toSortItemSearchMatchInfesAsync,
		//
		int pageIndex,
		int pageSize,
		//
		bool isGetItemSearchMatchInfesInPage = false)
	{
		var itemsCount = items.GetCount();
		if (itemsCount < 1)
		{
			return null;
		}

		var itemPageBeginItemIndex = pageIndex * pageSize;
		if (itemPageBeginItemIndex < 0)
		{
			itemPageBeginItemIndex = 0;
		}
		var itemPageEndItemIndex = itemPageBeginItemIndex + pageSize;
		if (itemPageEndItemIndex > itemsCount)
		{
			itemPageEndItemIndex = itemsCount;
		}
		if (itemPageBeginItemIndex >= itemsCount
		    || itemPageEndItemIndex <= itemPageBeginItemIndex)
		{
			return null;
		}

		////////////////////////////////////////////////
		// 1/，不需要搜索匹配和排序时，直接按有效的分页索引返回实体。
		////////////////////////////////////////////////

		List<ItemType> itemsSearchedInPage;
		List<ItemSearchMatchInfo<ItemType>>? itemSearchMatchInfesInPage = null;
		if (toGetItemSearchMatchedProgressAsync == null
			&& toSortItemSearchMatchInfes == null
			&& toSortItemSearchMatchInfesAsync == null)
		{
			itemsSearchedInPage = [];
			var itemIndex = 0;
			if (isGetItemSearchMatchInfesInPage)
			{
				itemSearchMatchInfesInPage = [];
				foreach (var item in items)
				{
					if (itemIndex < itemPageBeginItemIndex)
					{
						itemIndex++;
						continue;
					}
					if (itemIndex >= itemPageEndItemIndex)
					{
						break;
					}
					// !!!
					itemsSearchedInPage.Add(item);
					itemSearchMatchInfesInPage.Add(new(item, 1.0));
					itemIndex++;
					// !!!
				}
			}
			else
			{
				foreach (var item in items)
				{
					if (itemIndex < itemPageBeginItemIndex)
					{
						itemIndex++;
						continue;
					}
					if (itemIndex >= itemPageEndItemIndex)
					{
						break;
					}
					// !!!
					itemsSearchedInPage.Add(item);
					itemIndex++;
					// !!!
				}
			}
			//
			return new(
				itemsCount,
				itemsSearchedInPage,
				itemSearchMatchInfesInPage);
			//
		}

		////////////////////////////////////////////////
		// 2/，创建元素的搜索匹配信息。
		////////////////////////////////////////////////

		List<ItemSearchMatchInfo<ItemType>> itemSearchMatchInfes = [];
		await items.ConcurrentProcessItemsAsync(
			async (item) =>
			{
				var itemSearchMatchedProgress = 0.0;
				if (toGetItemSearchMatchedProgressAsync != null)
				{
					itemSearchMatchedProgress = await toGetItemSearchMatchedProgressAsync(item);
				}
				if (itemSearchMatchedProgress <= 0)
				{
					return;
				}
				lock (itemSearchMatchInfes)
				{
					itemSearchMatchInfes.Add(new(
						item,
						itemSearchMatchedProgress));
				}
			},
			searchTasksCount);

		var itemSearchMatchInfesCount = itemSearchMatchInfes.Count;
		if (itemPageEndItemIndex > itemSearchMatchInfesCount)
		{
			itemPageEndItemIndex = itemSearchMatchInfesCount;
		}
		if (itemPageBeginItemIndex >= itemSearchMatchInfesCount
		    || itemPageEndItemIndex <= itemPageBeginItemIndex)
		{
			return null;
		}


		////////////////////////////////////////////////
		// 3/，根据搜索匹配信息排序元素。
		////////////////////////////////////////////////
		if (toSortItemSearchMatchInfes != null)
		{
			itemSearchMatchInfes
			    = toSortItemSearchMatchInfes(itemSearchMatchInfes);
		}
		else if (toSortItemSearchMatchInfesAsync != null)
		{
			itemSearchMatchInfes = await toSortItemSearchMatchInfesAsync(itemSearchMatchInfes);
		}
		else
		{
			itemSearchMatchInfes.Sort((
			    searchMatchInfoA,
			    searchMatchInfoB) =>
			{
				return searchMatchInfoB.MatchedProgress.CompareTo(
					searchMatchInfoA.MatchedProgress);
			});
		}

		////////////////////////////////////////////////
		// 4/，根据搜索、排序后的结果进行分页。
		////////////////////////////////////////////////
		itemsSearchedInPage = [];
		if (isGetItemSearchMatchInfesInPage)
		{
			itemSearchMatchInfesInPage = [];
			for (var itemIndex = itemPageBeginItemIndex;
			    itemIndex < itemPageEndItemIndex;
			    itemIndex++)
			{
				var itemSearchMatchInfo = itemSearchMatchInfes[itemIndex];
				// !!!
				itemsSearchedInPage.Add(itemSearchMatchInfo.Item);
				itemSearchMatchInfesInPage.Add(itemSearchMatchInfo);
				// !!!
			}
		}
		else
		{
			for (var itemIndex = itemPageBeginItemIndex;
			    itemIndex < itemPageEndItemIndex;
			    itemIndex++)
			{
				var itemSearchMatchInfo = itemSearchMatchInfes[itemIndex];
				// !!!
				itemsSearchedInPage.Add(itemSearchMatchInfo.Item);
				// !!!
			}
		}
		//
		return new(
			itemSearchMatchInfes.Count,
			itemsSearchedInPage,
			itemSearchMatchInfesInPage);
		//
	}
}
