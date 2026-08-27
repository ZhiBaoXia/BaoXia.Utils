using System;

namespace BaoXia.Utils.Models;

public class DateTimeRange
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public DateTime BeginTime { get; set; }

	public DateTime EndTime { get; set; }

	#endregion


	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public DateTimeRange()
	{ }

	public DateTimeRange(DateTime beginTime, DateTime endTime)
	{
		BeginTime = beginTime;
		EndTime = endTime;
	}

	#endregion
}