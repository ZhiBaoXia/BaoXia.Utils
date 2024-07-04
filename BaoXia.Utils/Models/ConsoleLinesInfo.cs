using System.Collections.Generic;

namespace BaoXia.Utils.Models;

public class ConsoleLinesInfo
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public IEnumerable<string>? LastLines { get; set; }

	public ConsoleCursorInfo OriginalCursorInfo { get; set; } = new();

	public ConsoleCursorInfo EndCursorInfo { get; set; } = new();

	public int RowWidthMax { get; set; }


	#endregion
}