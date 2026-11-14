namespace BaoXia.Utils.Test.Models;

public class CloneDeepTestItem
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public int Id { get; set; }

	public string Name { get; set; } = string.Empty;

	public CloneDeepTestItem? RelatedItem { get; set; }

	#endregion


	////////////////////////////////////////////////
	// @重载
	////////////////////////////////////////////////

	#region 重载

	public override bool Equals(object? obj)
	{
		return obj is CloneDeepTestItem anotherItem && Id == anotherItem.Id;
	}

	public override int GetHashCode()
	{
		return Id.GetHashCode();
	}

	#endregion
}
