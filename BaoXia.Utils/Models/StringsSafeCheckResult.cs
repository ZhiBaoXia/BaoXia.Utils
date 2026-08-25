using BaoXia.Utils.Constants;

namespace BaoXia.Utils.Models;

public class StringsSafeCheckResult
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public string? UnsafeStringName { get; set; }

	public StringUnsafeType UnsafeType { get; set; }

	public string? UnsafeDescription { get; set; }

	#endregion
}