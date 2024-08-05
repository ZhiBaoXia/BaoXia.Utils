namespace BaoXia.Utils.Cache.Index;

public class ItemIndexNode<ItemType>
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
			if (Items.Length > 0)
			{
				return Items[0];
			}
			return default;
		}
	}

	public ItemType? LastItem
	{
		get
		{
			if (Items.Length > 0)
			{
				return Items[^1];
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

	public ItemIndexNode()
	{
		Items = [];
	}

	public ItemIndexNode(ItemType[] items)
	{
		Items = items;
	}

	#endregion
}