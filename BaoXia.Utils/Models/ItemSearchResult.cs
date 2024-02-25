using System.Collections.Generic;

namespace BaoXia.Utils.Models;

public class ItemSearchResult<ItemType>
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public int ItemsCountSearchMatched { get; set; }

	public List<ItemType> ItemsInPage { get; set; }

	public List<ItemSearchMatchInfo<ItemType>>? ItemSearchMatchInfesInPage { get; set; }

	#endregion


	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public ItemSearchResult(
		int itemsCountSearchMatched,
		List<ItemType> itemsInPage,
		List<ItemSearchMatchInfo<ItemType>>? itemSearchMatchInfesInPage)
	{
		ItemsCountSearchMatched = itemsCountSearchMatched;
		ItemsInPage = itemsInPage;
		ItemSearchMatchInfesInPage = itemSearchMatchInfesInPage;
	}

	#endregion
}
