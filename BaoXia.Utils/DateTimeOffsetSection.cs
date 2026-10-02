using BaoXia.Utils.Constants;
using BaoXia.Utils.Extensions;
using System;

namespace BaoXia.Utils;

public class DateTimeOffsetSection(
    DateTimeComparisonCycle type = DateTimeComparisonCycle.None,
    DateTimeOffset? beginTime = null,
    DateTimeOffset? endTime = null)
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public DateTimeComparisonCycle Type { get; set; } = type;

	public DateTimeOffset? BeginTime { get; set; } = beginTime;

	public DateTimeOffset? EndTime { get; set; } = endTime;

	#endregion


	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static bool IsTimeInSection(
	    DateTimeOffset dateTime,
	    DateTimeComparisonCycle timeSectionType,
	    DateTimeOffset? beginTime,
	    DateTimeOffset? endTime)
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

	private static bool DidIsTimeInSection(DateTimeOffset dateTime, DateTimeOffset beginTime, DateTimeOffset endTime,
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

		if (beginTime.CompareTo(endTime, dateTimeCompareCycle) <= 0)
		{
			if (dateTime.CompareTo(beginTime, dateTimeCompareCycle) >= 0 && dateTime.CompareTo(endTime, dateTimeCompareCycle) < 0)
			{
				return true;
			}
		}
		else
		{
			if (dateTime.CompareTo(beginTime, dateTimeCompareCycle) >= 0
			    // && dateTime <= 当前时间范围最大值
			    )
			{
				return true;
			}
			if (dateTime.CompareTo(endTime, dateTimeCompareCycle)
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
		DateTimeOffset dateTime, DateTimeOffset beginTime, DateTimeComparisonCycle timeSectionType)
	{

		if (dateTime.CompareTo(beginTime, timeSectionType) >= 0)
		{
			return true;
		}
		return false;
	}

	private static bool DidIsEndTimeGreatThanTime(
	    DateTimeOffset dateTime, DateTimeOffset endTime, DateTimeComparisonCycle dateTimeCompareCycle)
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

	public bool IsTimeInSection(DateTimeOffset dateTime)
	{
		return IsTimeInSection(
		    dateTime,
		    Type,
		    BeginTime,
		    EndTime);
	}

	#endregion
}
