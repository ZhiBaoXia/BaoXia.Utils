﻿using BaoXia.Utils.Constants;
using BaoXia.Utils.Extensions;
using System;

namespace BaoXia.Utils;

public class DateTimeSection(
	TimeSectionType type = TimeSectionType.NotLoop,
	DateTime? beginTime = null,
	DateTime? endTime = null)
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public TimeSectionType Type { get; set; } = type;

	public DateTime? BeginTime { get; set; } = beginTime;

	public DateTime? EndTime { get; set; } = endTime;

	#endregion


	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static bool IsTimeInSection(
		DateTime dateTime,
		TimeSectionType timeSectionType,
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
		TimeSectionType timeSectionType)
	{
		DateTimeCycle dateTimeCompareCycle;
		switch (timeSectionType)
		{
			default:
			case TimeSectionType.NotLoop:
				{
					if (dateTime >= beginTime
						&& dateTime < endTime)
					{
						return true;
					}
					return false;
				}
			case TimeSectionType.LoopInCentury:
				{
					dateTimeCompareCycle = DateTimeCycle.Century;
				}
				break;
			case TimeSectionType.LoopInYear:
				{
					dateTimeCompareCycle = DateTimeCycle.Year;
				}
				break;
			case TimeSectionType.LoopInMonth:
				{
					dateTimeCompareCycle = DateTimeCycle.Month;
				}
				break;
			case TimeSectionType.LoopInWeek:
				{
					dateTimeCompareCycle = DateTimeCycle.Week;
				}
				break;
			case TimeSectionType.LoopInDay:
				{
					dateTimeCompareCycle = DateTimeCycle.Day;
				}
				break;
			case TimeSectionType.LoopInHour:
				{
					dateTimeCompareCycle = DateTimeCycle.Hour;
				}
				break;
			case TimeSectionType.LoopInMinute:
				{
					dateTimeCompareCycle = DateTimeCycle.Minute;
				}
				break;
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
		DateTime dateTime,
		DateTime beginTime,
		TimeSectionType timeSectionType)
	{
		DateTimeCycle dateTimeCompareCycle;
		switch (timeSectionType)
		{
			default:
			case TimeSectionType.NotLoop:
				{
					dateTimeCompareCycle = DateTimeCycle.All;
				}
				break;
			case TimeSectionType.LoopInCentury:
				{
					dateTimeCompareCycle = DateTimeCycle.Century;
				}
				break;
			case TimeSectionType.LoopInYear:
				{
					dateTimeCompareCycle = DateTimeCycle.Year;
				}
				break;
			case TimeSectionType.LoopInMonth:
				{
					dateTimeCompareCycle = DateTimeCycle.Month;
				}
				break;
			case TimeSectionType.LoopInWeek:
				{
					dateTimeCompareCycle = DateTimeCycle.Week;
				}
				break;
			case TimeSectionType.LoopInDay:
				{
					dateTimeCompareCycle = DateTimeCycle.Day;
				}
				break;
			case TimeSectionType.LoopInHour:
				{
					dateTimeCompareCycle = DateTimeCycle.Hour;
				}
				break;
			case TimeSectionType.LoopInMinute:
				{
					dateTimeCompareCycle = DateTimeCycle.Minute;
				}
				break;
		}

		if (dateTime.CompareTo(
			beginTime,
			dateTimeCompareCycle)
			>= 0)
		{
			return true;
		}
		return false;
	}

	private static bool DidIsEndTimeGreatThanTime(
		DateTime dateTime,
		DateTime endTime,
		TimeSectionType timeSectionType)
	{
		DateTimeCycle dateTimeCompareCycle;
		switch (timeSectionType)
		{
			default:
			case TimeSectionType.NotLoop:
				{
					dateTimeCompareCycle = DateTimeCycle.All;
				}
				break;
			case TimeSectionType.LoopInCentury:
				{
					dateTimeCompareCycle = DateTimeCycle.Century;
				}
				break;
			case TimeSectionType.LoopInYear:
				{
					dateTimeCompareCycle = DateTimeCycle.Year;
				}
				break;
			case TimeSectionType.LoopInMonth:
				{
					dateTimeCompareCycle = DateTimeCycle.Month;
				}
				break;
			case TimeSectionType.LoopInWeek:
				{
					dateTimeCompareCycle = DateTimeCycle.Week;
				}
				break;
			case TimeSectionType.LoopInDay:
				{
					dateTimeCompareCycle = DateTimeCycle.Day;
				}
				break;
			case TimeSectionType.LoopInHour:
				{
					dateTimeCompareCycle = DateTimeCycle.Hour;
				}
				break;
			case TimeSectionType.LoopInMinute:
				{
					dateTimeCompareCycle = DateTimeCycle.Minute;
				}
				break;
		}

		if (dateTime.CompareTo(
			endTime,
			dateTimeCompareCycle)
			<= 0)
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