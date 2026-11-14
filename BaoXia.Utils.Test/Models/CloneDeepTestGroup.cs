using System.Collections.Generic;

namespace BaoXia.Utils.Test.Models;

public class CloneDeepTestGroup : CloneDeepTestItem
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public List<CloneDeepTestItem> Items { get; set; } = [];

	#endregion
}
