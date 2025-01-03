﻿using System.Collections.Generic;

namespace BaoXia.Utils.Models;

public class RecursionStep<ItemType>
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	public RecursionStep<ItemType>? PrevRecursionStep { get; set; }

	public int RecursionDepthIndex { get; set; }

	public ItemType? ParentItem { get; set; }

	public IList<ItemType> Items { get; set; } = default!;

	public ItemType? CurrentItem { get; set; }

	public int NextItemIndex { get; set; }


	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	public RecursionStep()
	{ }

	public RecursionStep(
		ItemType? parentItem,
		IList<ItemType> steps,
		int nextStepIndex)
	{
		ParentItem = parentItem;
		Items = steps;
		NextItemIndex = nextStepIndex;
	}
}
