namespace BaoXia.Utils.Models;

public class ItemSearchMatchInfo<ItemType>
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public ItemType Item { get; set; }

	public double MatchedProgress { get; set; }

	#endregion

	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public ItemSearchMatchInfo(
	    ItemType item,
	    double matchedProgress)
	{
		Item = item;
		MatchedProgress = matchedProgress;
	}

	#endregion
}
