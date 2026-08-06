using BaoXia.Utils.Constants;
using System;

namespace BaoXia.Utils.Extensions;

/// <summary>
/// “DateTime”扩展类。
/// </summary>
public static class DateTimeExtension
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static DateTime ToDateTimeInTimeZone(
	    this DateTime dateTime,
	    TimeZoneNumber timeZoneNumber)
	{
		return DateTimeUtil.DateTimeByOffsetToTimeZone(
		    dateTime,
		    timeZoneNumber);
	}

	public static long MillisecondsFrom1970(
	    this DateTime dateTime,
	    TimeZoneNumber millisecondsZoneNumber = TimeZoneNumber.Utc0,
	    bool isMillisecondsMinZero = true)
	{
		return DateTimeUtil.GetMillisecondsFrom1970OfDateTime(
		    dateTime,
		    millisecondsZoneNumber,
		    isMillisecondsMinZero);
	}

	public static long SecondsFrom1970(
	    this DateTime dateTime,
	    TimeZoneNumber secondsZoneNumber = TimeZoneNumber.Utc0,
	    bool isSecondsMinZero = true)
	{
		return DateTimeUtil.GetSecondsFrom1970OfDateTime(
		    dateTime,
		    secondsZoneNumber,
		    isSecondsMinZero);
	}

	/// <summary>
	/// 返回当前时间零点的时间对象。
	/// </summary>
	/// <param name="dateTime">当前时间对象。</param>
	/// <returns>当前时间零点的时间对象。</returns>
	public static DateTime ZeroOfThisDay(this DateTime dateTime)
	{
		return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
	}

	/// <summary>
	/// 返回当前时间前一天零点的时间对象。
	/// </summary>
	/// <param name="dateTime">当前时间对象。</param>
	/// <returns>当前时间零点的时间对象。</returns>
	public static DateTime ZeroOfPrevDay(this DateTime dateTime)
	{
		var prevDay = dateTime.AddDays(-1);
		{ }
		return new DateTime(prevDay.Year, prevDay.Month, prevDay.Day);
	}

	/// <summary>
	/// 返回当前时间后一天零点的时间对象。
	/// </summary>
	/// <param name="dateTime">当前时间对象。</param>
	/// <returns>当前时间零点的时间对象。</returns>
	public static DateTime ZeroOfNextDay(this DateTime dateTime)
	{
		var nextDay = dateTime.AddDays(1);
		{ }
		return new DateTime(nextDay.Year, nextDay.Month, nextDay.Day);
	}


	public static DateTime ZeroOfThisHour(this DateTime dateTime)
	{
		return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
	}

	public static DateTime ZeroOfPrevHour(this DateTime dateTime)
	{
		dateTime = dateTime.AddHours(-1.0);
		return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
	}

	public static DateTime ZeroOfNextHour(this DateTime dateTime)
	{
		dateTime = dateTime.AddHours(1.0);
		return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
	}


	public static DateTime ZeroOfThisMinute(this DateTime dateTime)
	{
		return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
	}

	public static DateTime ZeroOfPrevMinute(this DateTime dateTime)
	{
		return dateTime.AddMinutes(-1.0).ZeroOfThisMinute();
	}

	public static DateTime ZeroOfNextMinute(this DateTime dateTime)
	{
		return dateTime.AddMinutes(1.0).ZeroOfThisMinute();
	}

	public static DateTime ZeroOfThisSecond(this DateTime dateTime)
	{
		return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
	}

	public static DateTime ZeroOfPrevSecond(this DateTime dateTime)
	{
		return dateTime.AddSeconds(-1.0).ZeroOfThisMinute();
	}

	public static DateTime ZeroOfNextSecond(this DateTime dateTime)
	{
		return dateTime.AddSeconds(1.0).ZeroOfThisMinute();
	}


	/// <summary>
	/// 返回当前时间所属周（以周日为第一天的周）的第一天（零点）的时间对象。
	/// </summary>
	/// <param name="dateTime">当前时间对象。</param>
	/// <returns>当前时间所属周（以周日为第一天的周）的第一天（零点）的时间对象。</returns>
	public static DateTime FirstDayOfThisWeekStartsWithSunday(this DateTime dateTime)
	{
		var dayOfWeek = dateTime.DayOfWeek;
		var firstDayOfThisWeek = dateTime.AddDays(0 - (int)dayOfWeek);
		{
			firstDayOfThisWeek = new DateTime(
			    firstDayOfThisWeek.Year,
			    firstDayOfThisWeek.Month,
			    firstDayOfThisWeek.Day);
		}
		return firstDayOfThisWeek;
	}

	/// <summary>
	/// 返回当前时间上一周（以周日为第一天的周）的第一天（零点）的时间对象。
	/// </summary>
	/// <param name="dateTime">当前时间对象。</param>
	/// <returns>当前时间上一周（以周日为第一天的周）的第一天（零点）的时间对象。</returns>
	public static DateTime FirstDayOfPrevWeekStartsWithSunday(this DateTime dateTime)
	{
		var prevWeek = dateTime.AddDays(-TimeConstants.DaysPerWeek);
		var firstDayOfPrevWeek = prevWeek.FirstDayOfThisWeekStartsWithSunday();
		{ }
		return firstDayOfPrevWeek;
	}

	/// <summary>
	/// 返回当前时间下一周（以周日为第一天的周）的第一天（零点）的时间对象。
	/// </summary>
	/// <param name="dateTime">当前时间对象。</param>
	/// <returns>当前时间下一周（以周日为第一天的周）的第一天（零点）的时间对象。</returns>
	public static DateTime FirstDayOfNextWeekStartsWithSunday(this DateTime dateTime)
	{

		var nextWeek = dateTime.AddDays(TimeConstants.DaysPerWeek);
		var firstDayOfNextWeek = nextWeek.FirstDayOfThisWeekStartsWithSunday();
		{ }
		return firstDayOfNextWeek;
	}

	/// <summary>
	/// 返回当前时间所属周（以周一为第一天的周）的第一天（零点）的时间对象。
	/// </summary>
	/// <param name="dateTime">当前时间对象。</param>
	/// <returns>当前时间所属周（以周一为第一天的周）的第一天（零点）的时间对象。</returns>
	public static DateTime FirstDayOfThisWeekStartsWithMonday(this DateTime dateTime)
	{
		var dayOfWeek = dateTime.DayOfWeek;
		var firstDayOfThisWeek = dateTime.AddDays(0 - (int)dayOfWeek + 1);
		{
			firstDayOfThisWeek = new DateTime(
			    firstDayOfThisWeek.Year,
			    firstDayOfThisWeek.Month,
			    firstDayOfThisWeek.Day);
		}
		return firstDayOfThisWeek;
	}

	/// <summary>
	/// 返回当前时间上一周（以周一为第一天的周）的第一天（零点）的时间对象。
	/// </summary>
	/// <param name="dateTime">当前时间对象。</param>
	/// <returns>当前时间上一周（以周一为第一天的周）的第一天（零点）的时间对象。</returns>
	public static DateTime FirstDayOfPrevWeekStartsWithMonday(this DateTime dateTime)
	{
		var prevWeek = dateTime.AddDays(-TimeConstants.DaysPerWeek);
		var firstDayOfPrevWeek = prevWeek.FirstDayOfThisWeekStartsWithMonday();
		{ }
		return firstDayOfPrevWeek;
	}

	/// <summary>
	/// 返回当前时间下一周（以周一为第一天的周）的第一天（零点）的时间对象。
	/// </summary>
	/// <param name="dateTime">当前时间对象。</param>
	/// <returns>当前时间下一周（以周一为第一天的周）的第一天（零点）的时间对象。</returns>
	public static DateTime FirstDayOfNextWeekStartsWithMonday(this DateTime dateTime)
	{
		var nextWeek = dateTime.AddDays(TimeConstants.DaysPerWeek);
		var firstDayOfNextWeek = nextWeek.FirstDayOfThisWeekStartsWithMonday();
		{ }
		return firstDayOfNextWeek;
	}

	/// <summary>
	/// 返回当前时间所属月份的第一天（零点）的时间对象。
	/// </summary>
	/// <param name="dateTime">当前时间对象。</param>
	/// <returns>当前时间所属月份的第一天（零点）的时间对象。</returns>
	public static DateTime FirstDayOfThisMonth(this DateTime dateTime)
	{
		return new DateTime(dateTime.Year, dateTime.Month, 1);
	}

	/// <summary>
	/// 返回当前时间上一个月份的第一天的时间（零点）对象。
	/// </summary>
	/// <param name="dateTime">当前时间对象。</param>
	/// <returns>当前时间上一个月份的第一天的时间（零点）对象。</returns>
	public static DateTime FirstDayOfPrevMonth(this DateTime dateTime)
	{
		return dateTime.AddMonths(-1).FirstDayOfThisMonth();
	}

	/// <summary>
	/// 返回当前时间下一个月份的第一天的时间（零点）对象。
	/// </summary>
	/// <param name="dateTime">当前时间对象。</param>
	/// <returns>当前时间下一个月份的第一天的时间（零点）对象。</returns>
	public static DateTime FirstDayOfNextMonth(this DateTime dateTime)
	{
		return dateTime.AddMonths(1).FirstDayOfThisMonth();
	}

	/// <summary>
	/// 返回当前时间所属季度的第一天（零点）的时间对象。
	/// </summary>
	/// <param name="dateTime">当前时间对象。</param>
	/// <returns>当前时间所属季度的第一天（零点）的时间对象。</returns>
	public static DateTime FirstDayOfThisQuarter(this DateTime dateTime)
	{
		var thisMonth = dateTime.Month;
		var firstMonthOfThisQuarter = thisMonth;
		if (thisMonth >= TimeConstants.FirstMonthOfQuarter1 && thisMonth <= TimeConstants.LastMonthOfQuarter1)
		{
			firstMonthOfThisQuarter = 1;
		}
		else if (thisMonth >= TimeConstants.FirstMonthOfQuarter2 && thisMonth <= TimeConstants.LastMonthOfQuarter2)
		{
			firstMonthOfThisQuarter = 4;
		}
		else if (thisMonth >= TimeConstants.FirstMonthOfQuarter3 && thisMonth <= TimeConstants.LastMonthOfQuarter3)
		{
			firstMonthOfThisQuarter = 7;
		}
		else if (thisMonth >= TimeConstants.FirstMonthOfQuarter4 && thisMonth <= TimeConstants.LastMonthOfQuarter4)
		{
			firstMonthOfThisQuarter = 10;
		}
		var firstDayOfThisQuarter = new DateTime(dateTime.Year, firstMonthOfThisQuarter, 1);
		{ }
		return firstDayOfThisQuarter;
	}

	/// <summary>
	/// 返回当前时间上一个季度的第一天（零点）的时间对象。
	/// </summary>
	/// <param name="dateTime">当前时间对象。</param>
	/// <returns>当前时间上一个季度的第一天（零点）的时间对象。</returns>
	public static DateTime FirstDayOfPrevQuarter(this DateTime dateTime)
	{
		return dateTime
		    .AddMonths(-TimeConstants.MonthsPerQuarter)
		    .FirstDayOfThisQuarter();
	}

	/// <summary>
	/// 返回当前时间下一个季度的第一天（零点）的时间对象。
	/// </summary>
	/// <param name="dateTime">当前时间对象。</param>
	/// <returns>当前时间下一个季度的第一天（零点）的时间对象。</returns>
	public static DateTime FirstDayOfNextQuarter(this DateTime dateTime)
	{
		return dateTime
		    .AddMonths(+TimeConstants.MonthsPerQuarter)
		    .FirstDayOfThisQuarter();
	}

	/// <summary>
	/// 返回当前时间所属年份的第一天（零点）的时间对象。
	/// </summary>
	/// <param name="dateTime">当前时间对象。</param>
	/// <returns>当前时间所属年份的第一天（零点）的时间对象。</returns>
	public static DateTime FirstDayOfThisYear(this DateTime dateTime)
	{
		return new DateTime(dateTime.Year, 1, 1);
	}

	/// <summary>
	/// 返回当前时间上一年的第一天（零点）的时间对象。
	/// </summary>
	/// <param name="dateTime">当前时间对象。</param>
	/// <returns>当前时间上一年的第一天（零点）的时间对象。</returns>
	public static DateTime FirstDayOfPrevYear(this DateTime dateTime)
	{
		return new DateTime(dateTime.Year - 1, 1, 1);
	}

	/// <summary>
	/// 返回当前时间下一年的第一天（零点）的时间对象。
	/// </summary>
	/// <param name="dateTime">当前时间对象。</param>
	/// <returns>当前时间下一年的第一天（零点）的时间对象。</returns>
	public static DateTime FirstDayOfNextYear(this DateTime dateTime)
	{
		return new DateTime(dateTime.Year + 1, 1, 1);
	}

	/// <summary>
	/// 在指定的时间精度上比较两个时间对象。
	/// </summary>
	/// <param name="dateTime">当前时间对象。</param>
	/// <param name="anotherDateTime">另一个时间对象。</param>
	/// <param name="compareFieldMin">指定的时间精度，类型为：DateTimeField。</param>
	/// <returns>“dateTime”小于“anotherDateTime”时，返回“-1”；“dateTime”等于“anotherDateTime”时，返回“0”；“dateTime”大于“anotherDateTime”时，返回“1”。</returns>
	public static int CompareTo(
	    this DateTime dateTime,
	    DateTime anotherDateTime,
	    DateTimeField compareFieldMin = DateTimeField.Millisecond)
	{
		if (compareFieldMin >= DateTimeField.Year)
		{
			if (dateTime.Year > anotherDateTime.Year)
			{
				return 1;
			}
			else if (dateTime.Year < anotherDateTime.Year)
			{
				return -1;
			}
		}
		if (compareFieldMin >= DateTimeField.Month)
		{
			if (dateTime.Month > anotherDateTime.Month)
			{
				return 1;
			}
			else if (dateTime.Month < anotherDateTime.Month)
			{
				return -1;
			}
		}
		if (compareFieldMin >= DateTimeField.Day)
		{
			if (dateTime.Day > anotherDateTime.Day)
			{
				return 1;
			}
			else if (dateTime.Day < anotherDateTime.Day)
			{
				return -1;
			}
		}
		if (compareFieldMin >= DateTimeField.Hour)
		{
			if (dateTime.Hour > anotherDateTime.Hour)
			{
				return 1;
			}
			else if (dateTime.Hour < anotherDateTime.Hour)
			{
				return -1;
			}
		}
		if (compareFieldMin >= DateTimeField.Minute)
		{
			if (dateTime.Minute > anotherDateTime.Minute)
			{
				return 1;
			}
			else if (dateTime.Minute < anotherDateTime.Minute)
			{
				return -1;
			}
		}
		if (compareFieldMin >= DateTimeField.Second)
		{
			if (dateTime.Second > anotherDateTime.Second)
			{
				return 1;
			}
			else if (dateTime.Second < anotherDateTime.Second)
			{
				return -1;
			}
		}
		if (compareFieldMin >= DateTimeField.Millisecond)
		{
			if (dateTime.Millisecond > anotherDateTime.Millisecond)
			{
				return 1;
			}
			else if (dateTime.Millisecond < anotherDateTime.Millisecond)
			{
				return -1;
			}
		}
		return 0;
	}


	/// <summary>
	/// 在指定的时间周期内比较两个时间对象，如：指定的时间范围为“Year”时，则比较时忽略时间对象的年份值。
	/// </summary>
	/// <param name="dateTime"></param>
	/// <param name="anotherDateTime"></param>
	/// <param name="compareCycle"></param>
	/// <returns></returns>
	public static int CompareTo(
	    this DateTime dateTime,
	    DateTime anotherDateTime,
	    DateTimeComparisonCycle compareCycle)
	{
		switch (compareCycle)
		{
			default:
			case DateTimeComparisonCycle.None:
			case DateTimeComparisonCycle.Century:
				{
					return dateTime.CompareTo(anotherDateTime);
				}
			case DateTimeComparisonCycle.Year:
				{
					if (dateTime.Month < anotherDateTime.Month)
					{
						return -1;
					}
					else if (dateTime.Month > anotherDateTime.Month)
					{
						return 1;
					}
					else if (dateTime.Day < anotherDateTime.Day)
					{
						return -1;
					}
					else if (dateTime.Day > anotherDateTime.Day)
					{
						return 1;
					}
					else if (dateTime.Hour < anotherDateTime.Hour)
					{
						return -1;
					}
					else if (dateTime.Hour > anotherDateTime.Hour)
					{
						return 1;
					}
					else if (dateTime.Minute < anotherDateTime.Minute)
					{
						return -1;
					}
					else if (dateTime.Minute > anotherDateTime.Minute)
					{
						return 1;
					}
					else if (dateTime.Second < anotherDateTime.Second)
					{
						return -1;
					}
					else if (dateTime.Second > anotherDateTime.Second)
					{
						return 1;
					}
					else if (dateTime.Millisecond < anotherDateTime.Millisecond)
					{
						return -1;
					}
					else if (dateTime.Millisecond > anotherDateTime.Millisecond)
					{
						return 1;
					}
				}
				break;
			case DateTimeComparisonCycle.Month:
				{
					if (dateTime.Day < anotherDateTime.Day)
					{
						return -1;
					}
					else if (dateTime.Day > anotherDateTime.Day)
					{
						return 1;
					}
					else if (dateTime.Hour < anotherDateTime.Hour)
					{
						return -1;
					}
					else if (dateTime.Hour > anotherDateTime.Hour)
					{
						return 1;
					}
					else if (dateTime.Minute < anotherDateTime.Minute)
					{
						return -1;
					}
					else if (dateTime.Minute > anotherDateTime.Minute)
					{
						return 1;
					}
					else if (dateTime.Second < anotherDateTime.Second)
					{
						return -1;
					}
					else if (dateTime.Second > anotherDateTime.Second)
					{
						return 1;
					}
					else if (dateTime.Millisecond < anotherDateTime.Millisecond)
					{
						return -1;
					}
					else if (dateTime.Millisecond > anotherDateTime.Millisecond)
					{
						return 1;
					}
				}
				break;
			case DateTimeComparisonCycle.Week:
				{
					if (dateTime.DayOfWeek < anotherDateTime.DayOfWeek)
					{
						return -1;
					}
					else if (dateTime.DayOfWeek > anotherDateTime.DayOfWeek)
					{
						return 1;
					}
					else if (dateTime.Hour < anotherDateTime.Hour)
					{
						return -1;
					}
					else if (dateTime.Hour > anotherDateTime.Hour)
					{
						return 1;
					}
					else if (dateTime.Minute < anotherDateTime.Minute)
					{
						return -1;
					}
					else if (dateTime.Minute > anotherDateTime.Minute)
					{
						return 1;
					}
					else if (dateTime.Second < anotherDateTime.Second)
					{
						return -1;
					}
					else if (dateTime.Second > anotherDateTime.Second)
					{
						return 1;
					}
					else if (dateTime.Millisecond < anotherDateTime.Millisecond)
					{
						return -1;
					}
					else if (dateTime.Millisecond > anotherDateTime.Millisecond)
					{
						return 1;
					}
				}
				break;
			case DateTimeComparisonCycle.Day:
				{
					if (dateTime.Hour < anotherDateTime.Hour)
					{
						return -1;
					}
					else if (dateTime.Hour > anotherDateTime.Hour)
					{
						return 1;
					}
					else if (dateTime.Minute < anotherDateTime.Minute)
					{
						return -1;
					}
					else if (dateTime.Minute > anotherDateTime.Minute)
					{
						return 1;
					}
					else if (dateTime.Second < anotherDateTime.Second)
					{
						return -1;
					}
					else if (dateTime.Second > anotherDateTime.Second)
					{
						return 1;
					}
					else if (dateTime.Millisecond < anotherDateTime.Millisecond)
					{
						return -1;
					}
					else if (dateTime.Millisecond > anotherDateTime.Millisecond)
					{
						return 1;
					}
				}
				break;
			case DateTimeComparisonCycle.Hour:
				{
					if (dateTime.Minute < anotherDateTime.Minute)
					{
						return -1;
					}
					else if (dateTime.Minute > anotherDateTime.Minute)
					{
						return 1;
					}
					else if (dateTime.Second < anotherDateTime.Second)
					{
						return -1;
					}
					else if (dateTime.Second > anotherDateTime.Second)
					{
						return 1;
					}
					else if (dateTime.Millisecond < anotherDateTime.Millisecond)
					{
						return -1;
					}
					else if (dateTime.Millisecond > anotherDateTime.Millisecond)
					{
						return 1;
					}
				}
				break;
			case DateTimeComparisonCycle.Minute:
				{
					if (dateTime.Second < anotherDateTime.Second)
					{
						return -1;
					}
					else if (dateTime.Second > anotherDateTime.Second)
					{
						return 1;
					}
					else if (dateTime.Millisecond < anotherDateTime.Millisecond)
					{
						return -1;
					}
					else if (dateTime.Millisecond > anotherDateTime.Millisecond)
					{
						return 1;
					}
				}
				break;
			case DateTimeComparisonCycle.Second:
				{
					if (dateTime.Millisecond < anotherDateTime.Millisecond)
					{
						return -1;
					}
					else if (dateTime.Millisecond > anotherDateTime.Millisecond)
					{
						return 1;
					}
				}
				break;
			case DateTimeComparisonCycle.Millisecond:
				{
					// !!!⚠ 毫秒以下不进行比较，永远相等。 ⚠!!!
				}
				break;
		}
		return 0;
	}

	public static bool IsEarlierInYear(
	       this DateTime dateTime,
	       DateTime anotherDateTime)
	{
		return dateTime.CompareTo(
		    anotherDateTime,
		    DateTimeField.Year)
		    < 0;
	}

	public static bool EqualsInYear(
	    this DateTime dateTime,
	    DateTime anotherDateTime)
	{
		return dateTime.CompareTo(
		    anotherDateTime,
		    DateTimeField.Year)
		    == 0;
	}

	public static bool IsLaterInYear(
	       this DateTime dateTime,
	       DateTime anotherDateTime)
	{
		return dateTime.CompareTo(
		    anotherDateTime,
		    DateTimeField.Year)
		    > 0;
	}

	public static bool IsEarlierInMonth(
	       this DateTime dateTime,
	       DateTime anotherDateTime)
	{
		return dateTime.CompareTo(
		    anotherDateTime,
		    DateTimeField.Month)
		    < 0;
	}

	public static bool EqualsInMonth(
	    this DateTime dateTime,
	    DateTime anotherDateTime)
	{
		return dateTime.CompareTo(
		    anotherDateTime,
		    DateTimeField.Month)
		    == 0;
	}

	public static bool IsLaterInMonth(
	       this DateTime dateTime,
	       DateTime anotherDateTime)
	{
		return dateTime.CompareTo(
		    anotherDateTime,
		    DateTimeField.Month)
		    > 0;
	}

	public static bool IsLastMonthOf(
	    this DateTime dateTime,
	       DateTime currentMonth)
	{
		var dateTimeBeforeMonth = currentMonth.AddMonths(-1);
		if (dateTimeBeforeMonth.EqualsInMonth(dateTime))
		{
			return true;
		}
		return false;
	}

	public static bool IsNextMonthOf(
	    this DateTime dateTime,
	       DateTime currentMonth)
	{
		var dateTimeAfterMonth = currentMonth.AddMonths(1);
		if (dateTimeAfterMonth.EqualsInMonth(dateTime))
		{
			return true;
		}
		return false;
	}

	public static bool IsLastWeekInStartsWithMondayOf(
	    this DateTime dateTime,
	       DateTime currentWeek)
	{
		var currentWeekBeginTime
		    = currentWeek.FirstDayOfThisWeekStartsWithMonday();
		var lastWeekBeginTime
		    = currentWeek.AddDays(-1 * TimeConstants.DaysPerWeek);
		if (dateTime >= lastWeekBeginTime
		    && dateTime < currentWeekBeginTime)
		{
			return true;
		}
		return false;
	}

	public static bool IsNextWeekInStartsWithMondayOf(
	    this DateTime dateTime,
	       DateTime currentWeek)
	{
		var nextWeekBeginTime
		    = currentWeek.FirstDayOfNextWeekStartsWithMonday();
		var nextWeekEndTime
		    = nextWeekBeginTime.AddDays(TimeConstants.DaysPerWeek);
		if (dateTime >= nextWeekBeginTime
		    && dateTime < nextWeekEndTime)
		{
			return true;
		}
		return false;
	}

	public static bool IsEarlierInDay(
	       this DateTime dateTime,
	       DateTime anotherDateTime)
	{
		return dateTime.CompareTo(
		    anotherDateTime,
		    DateTimeField.Day)
		    < 0;
	}

	public static bool EqualsInDay(
	    this DateTime dateTime,
	    DateTime anotherDateTime)
	{
		return dateTime.CompareTo(
		    anotherDateTime,
		    DateTimeField.Day)
		    == 0;
	}

	public static bool IsLaterInDay(
	       this DateTime dateTime,
	       DateTime anotherDateTime)
	{
		return dateTime.CompareTo(
		    anotherDateTime,
		    DateTimeField.Day)
		    > 0;
	}

	public static bool IsPrevDayOf(this DateTime dateTime, DateTime today)
	{
		var dateTimeBeforeDay = today.AddDays(-1);
		if (dateTimeBeforeDay.EqualsInDay(dateTime))
		{
			return true;
		}
		return false;
	}
	public static bool IsNextDayOf(this DateTime dateTime, DateTime today)
	{
		var dateTimeAfterDay = today.AddDays(1);
		if (dateTimeAfterDay.EqualsInDay(dateTime))
		{
			return true;
		}
		return false;
	}

	public static bool IsYesterdayOf(
	    this DateTime dateTime,
	       DateTime today)
	{
		return dateTime.IsPrevDayOf(today);
	}

	public static bool IsTomorrowOf(
	    this DateTime dateTime,
	       DateTime today)
	{
		return dateTime.IsNextDayOf(today);
	}

	public static bool IsEarlierInHour(
	       this DateTime dateTime,
	       DateTime anotherDateTime)
	{
		return dateTime.CompareTo(
		    anotherDateTime,
		    DateTimeField.Hour)
		    < 0;
	}

	public static bool EqualsInHour(
	    this DateTime dateTime,
	    DateTime anotherDateTime)
	{
		return dateTime.CompareTo(
		    anotherDateTime,
		    DateTimeField.Hour)
		    == 0;
	}

	public static bool IsLaterInHour(
	       this DateTime dateTime,
	       DateTime anotherDateTime)
	{
		return dateTime.CompareTo(
		    anotherDateTime,
		    DateTimeField.Hour)
		    > 0;
	}

	public static bool IsLastHourOf(this DateTime dateTime, DateTime currentDateTime)
	{
		if (dateTime.AddHours(1).EqualsInHour(currentDateTime))
		{
			return true;
		}
		return false;
	}

	public static bool IsNextHourOf(this DateTime dateTime, DateTime currentDateTime)
	{
		if (dateTime.AddHours(-1).EqualsInHour(currentDateTime))
		{
			return true;
		}
		return false;
	}


	public static bool IsEarlierInMinute(
	       this DateTime dateTime,
	       DateTime anotherDateTime)
	{
		return dateTime.CompareTo(
		    anotherDateTime,
		    DateTimeField.Minute)
		    < 0;
	}

	public static bool EqualsInMinute(
	    this DateTime dateTime,
	    DateTime anotherDateTime)
	{
		return dateTime.CompareTo(
		    anotherDateTime,
		    DateTimeField.Minute)
		    == 0;
	}

	public static bool IsLaterInMinute(
	       this DateTime dateTime,
	       DateTime anotherDateTime)
	{
		return dateTime.CompareTo(
		    anotherDateTime,
		    DateTimeField.Minute)
		    > 0;
	}

	public static bool IsLastMinuteOf(this DateTime dateTime, DateTime currentDateTime)
	{
		if (dateTime.AddMinutes(1).EqualsInMinute(currentDateTime))
		{
			return true;
		}
		return false;
	}

	public static bool IsNextMinuteOf(this DateTime dateTime, DateTime currentDateTime)
	{
		if (dateTime.AddMinutes(-1).EqualsInMinute(currentDateTime))
		{
			return true;
		}
		return false;
	}

	public static bool IsEarlierInSecond(
	       this DateTime dateTime,
	       DateTime anotherDateTime)
	{
		return dateTime.CompareTo(
		    anotherDateTime,
		    DateTimeField.Second)
		    < 0;
	}

	public static bool EqualsInSecond(
	    this DateTime dateTime,
	    DateTime anotherDateTime)
	{
		return dateTime.CompareTo(
		    anotherDateTime,
		    DateTimeField.Second)
		    == 0;
	}

	public static bool IsLaterInSecond(
	       this DateTime dateTime,
	       DateTime anotherDateTime)
	{
		return dateTime.CompareTo(
		    anotherDateTime,
		    DateTimeField.Second)
		    > 0;
	}

	public static bool IsLastSecondOf(this DateTime dateTime, DateTime currentDateTime)
	{
		if (dateTime.AddSeconds(1).EqualsInSecond(currentDateTime))
		{
			return true;
		}
		return false;
	}

	public static bool IsNextSecondOf(this DateTime dateTime, DateTime currentDateTime)
	{
		if (dateTime.AddSeconds(-1).EqualsInSecond(currentDateTime))
		{
			return true;
		}
		return false;
	}

	public static bool IsEarlierInMillisecond(
	       this DateTime dateTime,
	       DateTime anotherDateTime)
	{
		return dateTime.CompareTo(
		    anotherDateTime,
		    DateTimeField.Millisecond)
		    < 0;
	}

	public static bool EqualsInMillisecond(
	    this DateTime dateTime,
	    DateTime anotherDateTime)
	{
		return dateTime.CompareTo(
		    anotherDateTime,
		    DateTimeField.Millisecond)
		    == 0;
	}

	public static bool IsLaterInMillisecond(
	       this DateTime dateTime,
	       DateTime anotherDateTime)
	{
		return dateTime.CompareTo(
		    anotherDateTime,
		    DateTimeField.Millisecond)
		    > 0;
	}
	public static bool IsLastMillisecondOf(this DateTime dateTime, DateTime currentDateTime)
	{
		if (dateTime.AddMilliseconds(1).EqualsInMillisecond(currentDateTime))
		{
			return true;
		}
		return false;
	}

	public static bool IsNextMillisecondOf(this DateTime dateTime, DateTime currentDateTime)
	{
		if (dateTime.AddMilliseconds(-1).EqualsInMillisecond(currentDateTime))
		{
			return true;
		}
		return false;
	}

	public static bool IsBetween(
	    this DateTime dateTime,
	    DateTime? beginTime,
	    DateTime? endTime,
	    DateTimeField compareFieldMin)
	{
		if (beginTime != null
		    && endTime != null)
		{
			if (dateTime.CompareTo(beginTime.Value, compareFieldMin) >= 0
			    && dateTime.CompareTo(endTime.Value, compareFieldMin) < 0)
			{
				return true;
			}
		}
		else if (beginTime != null)
		{
			if (dateTime.CompareTo(beginTime.Value, compareFieldMin) >= 0)
			{
				return true;
			}
		}
		else if (endTime != null)
		{
			if (dateTime.CompareTo(endTime.Value, compareFieldMin) < 0)
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsContinuousAfter(
	    this DateTime currentDateTime,
	    DateTime lastDateTime,
	    DateTimeField continuousAccuracy,
	    bool isEqualsToContinuous = true)
	{
		switch (continuousAccuracy)
		{
			case DateTimeField.Year:
				{
					if (currentDateTime.Year == lastDateTime.Year + 1)
					{
						return true;
					}
					else if (currentDateTime.Year == lastDateTime.Year)
					{
						if (isEqualsToContinuous)
						{
							return true;
						}
					}
				}
				break;
			case DateTimeField.Month:
				{
					if (currentDateTime.IsNextMonthOf(lastDateTime))
					{
						return true;
					}
					else if (currentDateTime.EqualsInMonth(lastDateTime))
					{
						if (isEqualsToContinuous)
						{
							return true;
						}
					}
				}
				break;
			case DateTimeField.Day:
				{
					if (currentDateTime.IsNextDayOf(lastDateTime))
					{
						return true;
					}
					else if (currentDateTime.EqualsInDay(lastDateTime))
					{
						if (isEqualsToContinuous)
						{
							return true;
						}
					}
				}
				break;
			case DateTimeField.Hour:
				{
					if (currentDateTime.IsNextHourOf(lastDateTime))
					{
						return true;
					}
					else if (currentDateTime.EqualsInHour(lastDateTime))
					{
						if (isEqualsToContinuous)
						{
							return true;
						}
					}
				}
				break;
			case DateTimeField.Minute:
				{
					if (currentDateTime.IsNextMinuteOf(lastDateTime))
					{
						return true;
					}
					else if (currentDateTime.EqualsInMinute(lastDateTime))
					{
						if (isEqualsToContinuous)
						{
							return true;
						}
					}
				}
				break;
			case DateTimeField.Second:
				{
					if (currentDateTime.IsNextSecondOf(lastDateTime))
					{
						return true;
					}
					else if (currentDateTime.EqualsInSecond(lastDateTime))
					{
						if (isEqualsToContinuous)
						{
							return true;
						}
					}
				}
				break;
			case DateTimeField.Millisecond:
				{
					if (currentDateTime.IsNextMillisecondOf(lastDateTime))
					{

						return true;
					}
					else if (currentDateTime.EqualsInMillisecond(lastDateTime))
					{
						if (isEqualsToContinuous)
						{
							return true;
						}
					}
				}
				break;
			default:
				{ }
				break;
		}
		return false;
	}


	public static bool IsContinuousBefore(
	    this DateTime currentDateTime,
	    DateTime nextDateTime,
	    DateTimeField continuousAccuracy,
	    bool isEqualsToContinuous = true)
	{
		return nextDateTime.IsContinuousAfter(
		    currentDateTime,
		    continuousAccuracy,
		    isEqualsToContinuous);
	}


	public static string TitleOfListElementDefault(this DateTime dateTime, bool isNeedSecondsField = false)
	{
		string caption;
		var now = DateTime.Now;
		if (dateTime.EqualsInDay(now))
		{
			if (isNeedSecondsField)
			{
				caption = dateTime.ToString("今天 HH:mm:ss");
			}
			else
			{
				caption = dateTime.ToString("今天 HH:mm");
			}
		}
		else if (dateTime.IsYesterdayOf(now))
		{
			if (isNeedSecondsField)
			{
				caption = dateTime.ToString("昨天 HH:mm:ss");
			}
			else
			{
				caption = dateTime.ToString("昨天 HH:mm");
			}
		}
		else if (dateTime.IsTomorrowOf(now))
		{
			if (isNeedSecondsField)
			{
				caption = dateTime.ToString("明天 HH:mm:ss");
			}
			else
			{
				caption = dateTime.ToString("明天 HH:mm");
			}
		}
		else if (dateTime.EqualsInYear(now))
		{
			caption = dateTime.ToString("MM月dd日 HH:mm");
		}
		else
		{
			caption = dateTime.ToString("yyyy年MM月dd日");
		}
		return caption;
	}

	public static string TitleOfDetailPageDefault(this DateTime dateTime, bool isNeedSecondsField = false)
	{
		string caption;
		var now = DateTime.Now;
		if (dateTime.EqualsInDay(now))
		{
			if (isNeedSecondsField)
			{
				caption = dateTime.ToString("今天 HH:mm:ss");
			}
			else
			{
				caption = dateTime.ToString("今天 HH:mm");
			}
		}
		else if (dateTime.IsYesterdayOf(now))
		{
			if (isNeedSecondsField)
			{
				caption = dateTime.ToString("昨天 HH:mm:ss");
			}
			else
			{
				caption = dateTime.ToString("昨天 HH:mm");
			}
		}
		else if (dateTime.IsTomorrowOf(now))
		{
			if (isNeedSecondsField)
			{
				caption = dateTime.ToString("明天 HH:mm:ss");
			}
			else
			{
				caption = dateTime.ToString("明天 HH:mm");
			}
		}
		else if (dateTime.EqualsInYear(now))
		{
			if (isNeedSecondsField)
			{
				caption = dateTime.ToString("MM月dd日 HH:mm:ss");
			}
			else
			{
				caption = dateTime.ToString("MM月dd日 HH:mm");
			}
		}
		else
		{
			if (isNeedSecondsField)
			{
				caption = dateTime.ToString("yyyy年MM月dd日 HH:mm:ss");
			}
			else
			{
				caption = dateTime.ToString("yyyy年MM月dd日");
			}
		}
		return caption;
	}

	public static string TitleOfQuarter(this DateTime dateTime, bool isYearTitleEnable = false)
	{
		string quarterTitle;
		var month = dateTime.Month;
		if (month >= 1 && month <= 3)
		{
			quarterTitle = "一季度";
		}
		else if (month >= 4 && month <= 6)
		{
			quarterTitle = "二季度";
		}
		else if (month >= 7 && month <= 9)
		{
			quarterTitle = "三季度";
		}
		else if (month >= 10 && month <= 12)
		{
			quarterTitle = "四季度";
		}
		else
		{
			quarterTitle = "未知季度";
		}
		if (isYearTitleEnable)
		{
			quarterTitle = dateTime.ToString("yyyy") + "年_" + quarterTitle;
		}
		return quarterTitle;
	}

	public static string TitleOfMonthInChineseNumber(this DateTime dateTime)
	{
		switch (dateTime.Month)
		{
			default:
				{
					return "未知";
				}
			case 1:
				{
					return "一月";
				}
			case 2:
				{
					return "二月";
				}
			case 3:
				{
					return "三月";
				}
			case 4:
				{
					return "四月";
				}
			case 5:
				{
					return "五月";
				}
			case 6:
				{
					return "六月";
				}
			case 7:
				{
					return "七月";
				}
			case 8:
				{
					return "八月";
				}
			case 9:
				{
					return "九月";
				}
			case 10:
				{
					return "十月";
				}
			case 11:
				{
					return "十一月";
				}
			case 12:
				{
					return "十二月";
				}
		}
	}

	public static string TitleOfWeek(
		this DateTime dateTime, bool isMonthTitleEnable = false, bool isMonthChineseTitleEnable = true,
		bool isYearTitleEnable = false, bool isTitleForFileName = true)
	{
		var weekNumberInMonth = (dateTime.Day - 1) / TimeConstants.DaysPerWeek + 1;
		var weekTitle = weekNumberInMonth switch
		{
			1 => "第一周",
			2 => "第二周",
			3 => "第三周",
			4 => "第四周",
			5 => "第五周",
			_ => "未知周"
		};
		if (isYearTitleEnable)
		{
			if (isTitleForFileName)
			{
				return dateTime.ToString("yyyy年MM月_") + weekTitle;
			}
			else
			{
				return dateTime.ToString("yyyy年MM月 ") + weekTitle;
			}
		}
		if (isMonthTitleEnable)
		{
			if (isTitleForFileName)
			{
				if (isMonthChineseTitleEnable)
				{
					return dateTime.TitleOfMonthInChineseNumber() + "_" + weekTitle;
				}
				return dateTime.ToString("MM月_") + weekTitle;
			}
			else
			{
				if (isMonthChineseTitleEnable)
				{
					return dateTime.TitleOfMonthInChineseNumber() + " " + weekTitle;
				}
				return dateTime.ToString("MM月 ") + weekTitle;
			}
		}
		return weekTitle;
	}

	public static string TitleOfDateTimeWithAdaptivePrecision(this DateTime dateTime, bool isMillsecondsPrecisionEnable = false)
	{
		if (isMillsecondsPrecisionEnable)
		{
			if (dateTime.Hour == 0 && dateTime.Minute == 0 && dateTime.Second == 0 && dateTime.Millisecond == 0)
			{
				return dateTime.ToString("yyyy年MM月dd日");
			}
			else if (dateTime.Second == 0 && dateTime.Millisecond == 0)
			{
				return dateTime.ToString("yyyy年MM月dd日 HH:mm");
			}
			else if (dateTime.Millisecond == 0)
			{
				return dateTime.ToString("yyyy年MM月dd日 HH:mm:ss");
			}
			return dateTime.ToString("yyyy年MM月dd日 HH:mm:ss:fff");
		}
		if (dateTime.Hour == 0 && dateTime.Minute == 0 && dateTime.Second == 0)
		{
			return dateTime.ToString("yyyy年MM月dd日");
		}
		else if (dateTime.Second == 0)
		{
			return dateTime.ToString("yyyy年MM月dd日 HH:mm");
		}
		return dateTime.ToString("yyyy年MM月dd日 HH:mm:ss");
	}

	#endregion
}
