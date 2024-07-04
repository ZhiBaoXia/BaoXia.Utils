using BaoXia.Utils.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaoXia.Utils;
public class RecursionUtil
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static void EnumerateWithRecursionStepType<ItemType, RecursionStepType>(
		ItemType? rootItem,
		Func<ItemType, RecursionStepType, IList<ItemType>?> toGetChildItems,
		Func<ItemType?, ItemType, RecursionStepType, bool> toEnumerateItem,
		Func<RecursionStepType, RecursionStepType>? toCreateNextRecursionStepType)
		where RecursionStepType : RecursionStep<ItemType>, new()
	{
		if (rootItem == null)
		{
			return;
		}

		var rootItems = new List<ItemType>();
		{
			rootItems.Add(rootItem);
		}

		var recursionSteps = new Stack<RecursionStepType>();
		{
			recursionSteps.Push(new()
			{
				ParentItem = default,
				Items = rootItems,
				NextItemIndex = 0
			});
		}

		while (recursionSteps.Count > 0)
		{
			var currentRecursionStep = recursionSteps.Peek();
			if (currentRecursionStep == null)
			{
				// !!!
				recursionSteps.Pop();
				continue;
				// !!!
			}

			var parentItem = currentRecursionStep.ParentItem;
			var items = currentRecursionStep.Items;
			var itemIndex = currentRecursionStep.NextItemIndex;
			var itemsCount = items.Count;
			if (itemIndex < 0
				|| itemIndex >= itemsCount)
			{
				// !!!
				recursionSteps.Pop();
				continue;
				// !!!
			}

			for (;
				itemIndex < itemsCount;
				itemIndex++)
			{
				var item = items[itemIndex];

				////////////////////////////////////////////////
				if (!toEnumerateItem(parentItem, item, currentRecursionStep))
				{
					return;
				}
				////////////////////////////////////////////////

				var childItems = toGetChildItems(item, currentRecursionStep);
				if (childItems == null
					|| childItems.Count < 1)
				{
					continue;
				}

				// !!!
				currentRecursionStep.NextItemIndex = itemIndex + 1;
				// !!!

				var nextRecursionStep
					= (toCreateNextRecursionStepType?.Invoke(currentRecursionStep)
					?? new());
				{
					nextRecursionStep.ParentItem = item;
					nextRecursionStep.Items = childItems;
					nextRecursionStep.NextItemIndex = 0;
				}
				recursionSteps.Push(nextRecursionStep);
				break;
				// !!!
			}
			if (itemIndex >= itemsCount)
			{
				// !!!
				recursionSteps.Pop();
				// !!!
			}
		}
	}

	public static async Task EnumerateWithRecursionStepTypeAsync<ItemType, RecursionStepType>(
		ItemType? rootItem,
		Func<ItemType, RecursionStepType, Task<IList<ItemType>?>> toGetChildItemsAsync,
		Func<ItemType?, ItemType, RecursionStepType, Task<bool>> toEnumerateItemAsync,
		Func<RecursionStepType, RecursionStepType>? toCreateNextRecursionStepType)
		where RecursionStepType : RecursionStep<ItemType>, new()
	{
		if (rootItem == null)
		{
			return;
		}

		var rootItems = new List<ItemType>();
		{
			rootItems.Add(rootItem);
		}

		var recursionSteps = new Stack<RecursionStepType>();
		{
			recursionSteps.Push(new()
			{
				ParentItem = default,
				Items = rootItems,
				NextItemIndex = 0
			});
		}

		while (recursionSteps.Count > 0)
		{
			var currentRecursionStep = recursionSteps.Peek();
			if (currentRecursionStep == null)
			{
				// !!!
				recursionSteps.Pop();
				continue;
				// !!!
			}

			var parentItem = currentRecursionStep.ParentItem;
			var items = currentRecursionStep.Items;
			var itemIndex = currentRecursionStep.NextItemIndex;
			var itemsCount = items.Count;
			if (itemIndex < 0
				|| itemIndex >= itemsCount)
			{
				// !!!
				recursionSteps.Pop();
				continue;
				// !!!
			}

			for (;
				itemIndex < itemsCount;
				itemIndex++)
			{
				var item = items[itemIndex];

				////////////////////////////////////////////////
				if (!(await toEnumerateItemAsync(parentItem, item, currentRecursionStep)))
				{
					return;
				}
				////////////////////////////////////////////////

				var childItems = await toGetChildItemsAsync(item, currentRecursionStep);
				if (childItems == null
					|| childItems.Count < 1)
				{
					continue;
				}

				// !!!
				currentRecursionStep.NextItemIndex = itemIndex + 1;
				// !!!

				var nextRecursionStep
					= (toCreateNextRecursionStepType?.Invoke(currentRecursionStep)
					?? new());
				{
					nextRecursionStep.ParentItem = item;
					nextRecursionStep.Items = childItems;
					nextRecursionStep.NextItemIndex = 0;
				}
				recursionSteps.Push(nextRecursionStep);
				break;
				// !!!
			}
			if (itemIndex >= itemsCount)
			{
				// !!!
				recursionSteps.Pop();
				// !!!
			}
		}
	}

	public static void Enumerate<ItemType>(
		ItemType? rootItem,
		Func<ItemType, IList<ItemType>?> toGetChildItems,
		Func<ItemType?, ItemType, bool> toEnumerateItem)
	{
		EnumerateWithRecursionStepType<ItemType, RecursionStep<ItemType>>(
			rootItem,
			(currentItem, _) =>
			{
				return toGetChildItems(currentItem);
			},
			(parentItem, currentItem, _) =>
			{
				return toEnumerateItem(parentItem, currentItem);
			},
			null);
	}

	public static async Task EnumerateAsync<ItemType>(
		ItemType? rootItem,
		Func<ItemType, Task<IList<ItemType>?>> toGetChildItemsAsync,
		Func<ItemType?, ItemType, Task<bool>> toEnumerateItemAsync)
	{
		await EnumerateWithRecursionStepTypeAsync<ItemType, RecursionStep<ItemType>>(
			rootItem,
			async (currentItem, _) =>
			{
				return await toGetChildItemsAsync(currentItem);
			},
			async (parentItem, currentItem, _) =>
			{
				return await toEnumerateItemAsync(parentItem, currentItem);
			},
			null);
	}

	#endregion
}