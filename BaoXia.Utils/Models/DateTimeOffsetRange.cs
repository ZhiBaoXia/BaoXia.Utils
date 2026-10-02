using System;

namespace BaoXia.Utils.Models;

public class DateTimeOffsetRange
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public DateTimeOffset BeginTime { get; set; }

	public DateTimeOffset EndTime { get; set; }

	#endregion


	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public DateTimeOffsetRange()
	{ }

	public DateTimeOffsetRange(DateTimeOffset beginTime, DateTimeOffset endTime)
	{
		BeginTime = beginTime;
		EndTime = endTime;
	}

	#endregion
}