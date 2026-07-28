namespace BaoXia.Utils.Constants;

/// <summary>
/// 定时时间类型。
/// </summary>
public enum DateTimeComparisonCycle
{
	/// <summary>
	/// 不循环。
	/// </summary>
	NotLoop,

	/// <summary>
	/// 每世纪循环。
	/// </summary>
	LoopInCentury,

	/// <summary>
	/// 每年循环。
	/// </summary>
	LoopInYear,

	/// <summary>
	/// 每月循环。
	/// </summary>
	LoopInMonth,

	/// <summary>
	/// 每周循环。
	/// </summary>
	LoopInWeek,

	/// <summary>
	/// 每日循环。
	/// </summary>
	LoopInDay,

	/// <summary>
	/// 每小时循环。
	/// </summary>
	LoopInHour,

	/// <summary>
	/// 每分钟循环。
	/// </summary>
	LoopInMinute,

	/// <summary>
	/// 每秒循环。
	/// </summary>
	LoopInSecond,

	/// <summary>
	/// 每毫秒循环。
	/// </summary>
	LoopInMillisecond
}
