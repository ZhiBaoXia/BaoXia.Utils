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


	public static void EnumerateWithRecursionStepsType<ItemType, RecursionStepType>(
		ItemType? rootItem,
		Func<ItemType, RecursionStepType, Stack<RecursionStepType>, IList<ItemType>?> toGetChildItems,
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
				PrevRecursionStep = null,
				//
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
				// !!!
				currentRecursionStep.CurrentItem = item;
				// !!!
				////////////////////////////////////////////////

				////////////////////////////////////////////////
				if (!toEnumerateItem(parentItem, item, currentRecursionStep))
				{
					return;
				}
				////////////////////////////////////////////////

				var childItems = toGetChildItems(item, currentRecursionStep, recursionSteps);
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
					nextRecursionStep.PrevRecursionStep = currentRecursionStep;
					//
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

	public static void EnumerateWithRecursionStepType<ItemType, RecursionStepType>(
		ItemType? rootItem,
		Func<ItemType, RecursionStepType, IList<ItemType>?> toGetChildItems,
		Func<ItemType?, ItemType, RecursionStepType, bool> toEnumerateItem,
		Func<RecursionStepType, RecursionStepType>? toCreateNextRecursionStepType)
		where RecursionStepType : RecursionStep<ItemType>, new()
	{
		EnumerateWithRecursionStepsType<ItemType, RecursionStepType>(
			rootItem,
			(item, currentRecursionStep, _) =>
			{
				return toGetChildItems(item, currentRecursionStep);
			},
			toEnumerateItem,
			toCreateNextRecursionStepType);
	}

	public static async Task EnumerateWithRecursionStepTypeAsync<ItemType, RecursionStepType>(
		ItemType? rootItem,
		Func<ItemType, RecursionStepType, Stack<RecursionStepType>, Task<IList<ItemType>?>> toGetChildItemsAsync,
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
				PrevRecursionStep = null,
				//
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
				// !!!
				currentRecursionStep.CurrentItem = item;
				// !!!
				////////////////////////////////////////////////

				////////////////////////////////////////////////
				if (!(await toEnumerateItemAsync(parentItem, item, currentRecursionStep)))
				{
					return;
				}
				////////////////////////////////////////////////

				var childItems = await toGetChildItemsAsync(item, currentRecursionStep, recursionSteps);
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
					nextRecursionStep.PrevRecursionStep = currentRecursionStep;
					//
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
		await EnumerateWithRecursionStepTypeAsync<ItemType, RecursionStepType>(
			rootItem,
			async (item, currentRecursionStep, secursionStepStack) =>
			{
				return await toGetChildItemsAsync(item, currentRecursionStep);
			},
			toEnumerateItemAsync,
			toCreateNextRecursionStepType);
	}

	public static void Enumerate<ItemType>(
		ItemType? rootItem,
		Func<ItemType, RecursionStep<ItemType>, Stack<RecursionStep<ItemType>>, IList<ItemType>?> toGetChildItems,
		Func<ItemType?, ItemType, RecursionStep<ItemType>, bool> toEnumerateItem)
	{
		EnumerateWithRecursionStepsType<ItemType, RecursionStep<ItemType>>(
			rootItem,
			(currentItem, currentRecursionStep, recursionSteps) =>
			{
				return toGetChildItems(currentItem, currentRecursionStep, recursionSteps);
			},
			(parentItem, currentItem, currentRecursionStep) =>
			{
				return toEnumerateItem(parentItem, currentItem, currentRecursionStep);
			},
			null);
	}

	public static void Enumerate<ItemType>(
		ItemType? rootItem,
		Func<ItemType, IList<ItemType>?> toGetChildItems,
		Func<ItemType?, ItemType, bool> toEnumerateItem)
	{
		EnumerateWithRecursionStepsType<ItemType, RecursionStep<ItemType>>(
			rootItem,
			(currentItem, _, _) =>
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
		Func<ItemType, RecursionStep<ItemType>, Stack<RecursionStep<ItemType>>, Task<IList<ItemType>?>> toGetChildItemsAsync,
		Func<ItemType?, ItemType, RecursionStep<ItemType>, Task<bool>> toEnumerateItemAsync)
	{
		await EnumerateWithRecursionStepTypeAsync<ItemType, RecursionStep<ItemType>>(
			rootItem,
			async (currentItem, currentRecursionStep, recursionSteps) =>
			{
				return await toGetChildItemsAsync(currentItem, currentRecursionStep, recursionSteps);
			},
			async (parentItem, currentItem, currentRecursionStep) =>
			{
				return await toEnumerateItemAsync(parentItem, currentItem, currentRecursionStep);
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
			async (currentItem, _, _) =>
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