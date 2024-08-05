using BaoXia.Utils.Constants;
using System;

namespace BaoXia.Utils.Extensions;

/// <summary>
/// “DateTime”扩展类。
/// </summary>
public static class DateTimeExtension
{
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
		TimeZoneNumber millisecondsZoneNumber = TimeZoneNumber.Utc0)
	{
		return DateTimeUtil.GetMillisecondsFrom1970OfDateTime(
			dateTime,
			millisecondsZoneNumber);
	}

	public static long SecondsFrom1970(
		this DateTime dateTime,
		TimeZoneNumber secondsZoneNumber = TimeZoneNumber.Utc0)
	{
		return DateTimeUtil.GetSecondsFrom1970OfDateTime(
			dateTime,
			secondsZoneNumber);
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
		var prevWeek = dateTime.AddDays(-TimeConstants.DaysToWeek);
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

		var nextWeek = dateTime.AddDays(TimeConstants.DaysToWeek);
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
		var prevWeek = dateTime.AddDays(-TimeConstants.DaysToWeek);
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
		var nextWeek = dateTime.AddDays(TimeConstants.DaysToWeek);
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
	public static DateTime FirstDayOfThisSession(this DateTime dateTime)
	{
		var thisMonth = dateTime.Month;
		var firstMonthOfThisSession = thisMonth;
		if (thisMonth >= TimeConstants.FirstMonthOfSession1 && thisMonth <= TimeConstants.LastMonthOfSession1)
		{
			firstMonthOfThisSession = 1;
		}
		else if (thisMonth >= TimeConstants.FirstMonthOfSession2 && thisMonth <= TimeConstants.LastMonthOfSession2)
		{
			firstMonthOfThisSession = 4;
		}
		else if (thisMonth >= TimeConstants.FirstMonthOfSession3 && thisMonth <= TimeConstants.LastMonthOfSession3)
		{
			firstMonthOfThisSession = 7;
		}
		else if (thisMonth >= TimeConstants.FirstMonthOfSession4 && thisMonth <= TimeConstants.LastMonthOfSession4)
		{
			firstMonthOfThisSession = 10;
		}
		var firstDayOfThisSession = new DateTime(dateTime.Year, firstMonthOfThisSession, 1);
		{ }
		return firstDayOfThisSession;
	}

	/// <summary>
	/// 返回当前时间上一个季度的第一天（零点）的时间对象。
	/// </summary>
	/// <param name="dateTime">当前时间对象。</param>
	/// <returns>当前时间上一个季度的第一天（零点）的时间对象。</returns>
	public static DateTime FirstDayOfPrevSession(this DateTime dateTime)
	{
		return dateTime
			.AddMonths(-TimeConstants.MonthsToSession)
			.FirstDayOfThisSession();
	}

	/// <summary>
	/// 返回当前时间下一个季度的第一天（零点）的时间对象。
	/// </summary>
	/// <param name="dateTime">当前时间对象。</param>
	/// <returns>当前时间下一个季度的第一天（零点）的时间对象。</returns>
	public static DateTime FirstDayOfNextSession(this DateTime dateTime)
	{
		return dateTime
			.AddMonths(+TimeConstants.MonthsToSession)
			.FirstDayOfThisSession();
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
		DateTimeCycle compareCycle)
	{
		switch (compareCycle)
		{
			default:
			case DateTimeCycle.All:
			case DateTimeCycle.Century:
				{
					return dateTime.CompareTo(anotherDateTime);
				}
			case DateTimeCycle.Year:
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
			case DateTimeCycle.Month:
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
			case DateTimeCycle.Week:
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
			case DateTimeCycle.Day:
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
			case DateTimeCycle.Hour:
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
			case DateTimeCycle.Minute:
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
			case DateTimeCycle.Second:
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
			case DateTimeCycle.Millisecond:
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
			= currentWeek.AddDays(-1 * TimeConstants.DaysToWeek);
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
			= nextWeekBeginTime.AddDays(TimeConstants.DaysToWeek);
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

	public static bool IsLastDayOf(this DateTime dateTime, DateTime today)
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
		return dateTime.IsLastDayOf(today);
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

	public static string CaptionOfListElementDefault(
		this DateTime dateTime,
		bool isNeedSecondsField = false)
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
			caption = dateTime.ToString("MM月dd天 HH:mm");
		}
		else
		{
			caption = dateTime.ToString("yyyy年MM月dd日");
		}
		return caption;
	}

	public static string CaptionOfDetailPageDefault(
		this DateTime dateTime,
		bool isNeedSecondsField = false)
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

}
