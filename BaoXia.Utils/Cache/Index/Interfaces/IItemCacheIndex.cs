using System.Collections.Generic;
using System.Threading.Tasks;

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

	public Task CreateIndexOfItemsAsync(
	    IEnumerable<ItemType> items,
	    int tasksCountToCreateRecordIndexes);

	public void UpdateIndexItemsByInsertItem(ItemType itemInserted);

	public void UpdateIndexItemsByUpdateItemFrom(
	    ItemType lastItem,
	    ItemType currentItem);

	public void UpdateIndexItemsByRemoveItem(ItemType itemDeleted);

	public void Clear();

	#endregion
}