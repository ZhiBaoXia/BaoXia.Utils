namespace BaoXia.Utils.Dictionaries;

public class DictionaryValueContainer<ItemType>
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public ItemType[] Items { get; set; }

	public ItemType? FirstItem
	{
		get
		{
			var items = Items;
			if (items.Length > 0)
			{
				return items[0];
			}
			return default;
		}
	}

	public ItemType? LastItem
	{
		get
		{
			var items = Items;
			if (items.Length > 0)
			{
				return items[^1];
			}
			return default;
		}
	}

	public int ItemsCount => Items.Length;

	#endregion


	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public DictionaryValueContainer()
	{
		Items = [];
	}

	public DictionaryValueContainer(ItemType[] items)
	{
		Items = items;
	}

	public bool TryGetFirstItem(out ItemType? item)
	{
		var items = Items;
		if (items.Length > 0)
		{
			item = items[0];
			return true;
		}
		item = default;
		return false;
	}

	#endregion
}