namespace BaoXia.Utils.Constants;

/// <summary>
/// 定时时间类型。
/// </summary>
public enum DateTimeComparisonCycle
{
	/// <summary>
	/// 不循环。
	/// </summary>
	None,

	/// <summary>
	/// 每世纪循环。
	/// </summary>
	Century,

	/// <summary>
	/// 每年循环。
	/// </summary>
	Year,

	/// <summary>
	/// 每月循环。
	/// </summary>
	Month,

	/// <summary>
	/// 每周循环。
	/// </summary>
	Week,

	/// <summary>
	/// 每日循环。
	/// </summary>
	Day,

	/// <summary>
	/// 每小时循环。
	/// </summary>
	Hour,

	/// <summary>
	/// 每分钟循环。
	/// </summary>
	Minute,

	/// <summary>
	/// 每秒循环。
	/// </summary>
	Second,

	/// <summary>
	/// 每毫秒循环。
	/// </summary>
	Millisecond
}
