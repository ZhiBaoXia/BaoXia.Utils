namespace BaoXia.Utils.Dictionaries;

public class DictionaryValueContainer
	<ItemType, ItemOperateLockerType>
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public ItemType[] Items { get; set; }

	public ItemOperateLockerType ItemOperateLocker { get; }

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

	public DictionaryValueContainer(
		ItemOperateLockerType itemOperateLocker)
	{
		ItemOperateLocker = itemOperateLocker;

		Items = [];
	}

	public DictionaryValueContainer(
		ItemOperateLockerType itemOperateLocker,
		ItemType[] items)
	{
		ItemOperateLocker = itemOperateLocker;

		Items = items;
	}

	#endregion
}