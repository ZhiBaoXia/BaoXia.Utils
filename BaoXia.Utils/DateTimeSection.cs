using BaoXia.Utils.Constants;
using BaoXia.Utils.Extensions;
using System;

namespace BaoXia.Utils;

public class DateTimeSection(
    DateTimeComparisonCycle type = DateTimeComparisonCycle.None,
    DateTime? beginTime = null,
    DateTime? endTime = null)
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public DateTimeComparisonCycle Type { get; set; } = type;

	public DateTime? BeginTime { get; set; } = beginTime;

	public DateTime? EndTime { get; set; } = endTime;

	#endregion


	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static bool IsTimeInSection(
	    DateTime dateTime,
	    DateTimeComparisonCycle timeSectionType,
	    DateTime? beginTime,
	    DateTime? endTime)
	{
		if (beginTime != null
		    && endTime != null)
		{
			return DidIsTimeInSection(
			    dateTime,
			    beginTime.Value,
			    endTime.Value,
			    timeSectionType);
		}
		else if (beginTime != null)
		{
			return DidIsBeginTimeLessOrEqualTime(
			    dateTime,
			    beginTime.Value,
			    timeSectionType);
		}
		else if (endTime != null)
		{
			return DidIsEndTimeGreatThanTime(
			    dateTime,
			    endTime.Value,
			    timeSectionType);
		}
		// 起始时间和结束时间都为空时，
		// 表示不对时间进行限制。
		// else if (beginTime == null
		//         && endTime == null)
		return true;
	}

	private static bool DidIsTimeInSection(
	    DateTime dateTime,
	    DateTime beginTime,
	    DateTime endTime,
	    DateTimeComparisonCycle dateTimeCompareCycle)
	{
		if (dateTimeCompareCycle == DateTimeComparisonCycle.None)
		{
			if (dateTime >= beginTime
			    && dateTime < endTime)
			{
				return true;
			}
			return false;
		}

		if (beginTime.CompareTo(
		    endTime,
		    dateTimeCompareCycle)
		    <= 0)
		{
			if (dateTime.CompareTo(
			    beginTime,
			    dateTimeCompareCycle) >= 0
			&& dateTime.CompareTo(
			    endTime,
			    dateTimeCompareCycle) < 0)
			{
				return true;
			}
		}
		else
		{
			if (dateTime.CompareTo(
			    beginTime,
			    dateTimeCompareCycle)
			    >= 0
			    // && dateTime <= 当前时间范围最大值
			    )
			{
				return true;
			}
			if (dateTime.CompareTo(
			    endTime,
			    dateTimeCompareCycle)
			    < 0
			    // && dateTime >= 当前时间范围最小值
			    )
			{
				return true;
			}
		}
		return false;
	}

	private static bool DidIsBeginTimeLessOrEqualTime(
		DateTime dateTime, DateTime beginTime, DateTimeComparisonCycle dateTimeComparisonCycle)
	{
		if (dateTime.CompareTo(beginTime, dateTimeComparisonCycle) >= 0)
		{
			return true;
		}
		return false;
	}

	private static bool DidIsEndTimeGreatThanTime(
		DateTime dateTime, DateTime endTime, DateTimeComparisonCycle dateTimeCompareCycle)
	{
		if (dateTime.CompareTo(endTime, dateTimeCompareCycle) <= 0)
		{
			return true;
		}
		return false;
	}

	#endregion



	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public bool IsTimeInSection(DateTime dateTime)
	{
		return IsTimeInSection(
		    dateTime,
		    Type,
		    BeginTime,
		    EndTime);
	}

	#endregion
}