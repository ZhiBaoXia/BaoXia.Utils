namespace BaoXia.Utils.Cache.Index.Interfaces;

public interface IItemCacheIndex<ItemType>
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public string? Name { get; set; }

	#endregion


	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public void UpdateIndexItemsByUpdateItemFrom(
	    ItemType? lastItem,
	    ItemType? currentItem);

	public void Clear();

	#endregion
}