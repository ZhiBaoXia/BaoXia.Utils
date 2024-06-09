using System;

namespace BaoXia.Utils;

public class DateTimeUtil
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	/// <summary>
	/// 通过指定距离1970年1月1日零时的毫秒数，创建当前时区的时间。
	/// </summary>
	/// <param name="millisecondsFrom1970">距离1970年1月1日零时的毫秒数。</param>
	/// <returns>距离1970年1月1日零时的毫秒数对应的当前时区时间。</returns>
	public static DateTime DateTimeWithMillisecondsFrom1970(
	    long millisecondsFrom1970)
	{
		var dateTime = new DateTime(1970, 1, 1);
		{
			dateTime = dateTime.AddMilliseconds(millisecondsFrom1970);
			//
			dateTime = dateTime.ToLocalTime();
		}
		return dateTime;
	}

	#endregion
}
