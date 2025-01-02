using BaoXia.Utils.Constants;
using System;

namespace BaoXia.Utils.Extensions;

public static class DateTimeOffsetExtension
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	// <summary>
	/// 获取当前时间的UTC时间戳，当前时间的UTC时间距离1970年1月1日零时的毫秒数。
	/// </summary>
	/// <param name="dateTimeOffset">当前时间对象。</param>
	/// <returns>返回当前时间的UTC时间距离1970年1月1日零时的毫秒数。</returns>
	public static long MillisecondsFrom1970(
		this DateTimeOffset dateTimeOffset,
		TimeZoneNumber millisecondsZoneNumber = TimeZoneNumber.Utc0,
		bool isMillisecondsMinValueZero = true)
	{
		return DateTimeOffsetUtil.GetMillisecondsFrom1970OfDateTimeOffset(
			dateTimeOffset,
			millisecondsZoneNumber,
			isMillisecondsMinValueZero);
	}

	public static long SecondsFrom1970(
		this DateTimeOffset dateTimeOffset,
		TimeZoneNumber secondsZoneNumber = TimeZoneNumber.Utc0,
		bool isSecondsMinValueZero = true)
	{
		return DateTimeOffsetUtil.GetSecondsFrom1970OfDateTimeOffset(
			dateTimeOffset,
			secondsZoneNumber,
			isSecondsMinValueZero);
	}

	public static DateTimeOffset ZeroOf(
		this DateTimeOffset dateTimeOffset,
		DateTimeField dateTimeField)
	{
		switch (dateTimeField)
		{
			default:
				{ }
				break;
			case DateTimeField.Year:
				{
					dateTimeOffset = new DateTimeOffset(
					dateTimeOffset.Year, 1, 1, 0, 0, 0, dateTimeOffset.Offset);
				}
				break;
			case DateTimeField.Month:
				{
					dateTimeOffset = new DateTimeOffset(
					dateTimeOffset.Year, dateTimeOffset.Month, 1, 0, 0, 0, dateTimeOffset.Offset);
				}
				break;
			case DateTimeField.Day:
				{
					dateTimeOffset = new DateTimeOffset(
					dateTimeOffset.Year, dateTimeOffset.Month, dateTimeOffset.Day, 0, 0, 0, dateTimeOffset.Offset);
				}
				break;
			case DateTimeField.Hour:
				{
					dateTimeOffset = new DateTimeOffset(
					dateTimeOffset.Year, dateTimeOffset.Month, dateTimeOffset.Day, dateTimeOffset.Hour, 0, 0, dateTimeOffset.Offset);
				}
				break;
			case DateTimeField.Minute:
				{
					dateTimeOffset = new DateTimeOffset(
					dateTimeOffset.Year, dateTimeOffset.Month, dateTimeOffset.Day, dateTimeOffset.Hour, dateTimeOffset.Minute, 0, dateTimeOffset.Offset);
				}
				break;
			case DateTimeField.Second:
				{
					dateTimeOffset = new DateTimeOffset(
					dateTimeOffset.Year, dateTimeOffset.Month, dateTimeOffset.Day, dateTimeOffset.Hour, dateTimeOffset.Minute, dateTimeOffset.Second, dateTimeOffset.Offset);
				}
				break;
			case DateTimeField.Millisecond:
				{
					dateTimeOffset = new DateTimeOffset(
					dateTimeOffset.Year, dateTimeOffset.Month, dateTimeOffset.Day, dateTimeOffset.Hour, dateTimeOffset.Minute, dateTimeOffset.Second, 0, dateTimeOffset.Offset);
				}
				break;
		}
		return dateTimeOffset;
	}

	/// <summary>
	/// 返回当前时间零点的时间对象。
	/// </summary>
	/// <param name="dateTimeOffset">当前时间对象。</param>
	/// <returns>当前时间零点的时间对象。</returns>
	public static DateTimeOffset ZeroOfThisDay(this DateTimeOffset dateTimeOffset)
	{
		return new DateTimeOffset(
			dateTimeOffset.Year,
			dateTimeOffset.Month,
			dateTimeOffset.Day,
			0,
			0,
			0,
			dateTimeOffset.Offset);
	}

	/// <summary>
	/// 返回当前时间前一天零点的时间对象。
	/// </summary>
	/// <param name="dateTimeOffset">当前时间对象。</param>
	/// <returns>当前时间零点的时间对象。</returns>
	public static DateTimeOffset ZeroOfPrevDay(this DateTimeOffset dateTimeOffset)
	{
		var prevDay = dateTimeOffset.AddDays(-1);
		{ }
		return new DateTimeOffset(
			prevDay.Year,
			prevDay.Month,
			prevDay.Day,
			0,
			0,
			0,
			prevDay.Offset);
	}

	/// <summary>
	/// 返回当前时间后一天零点的时间对象。
	/// </summary>
	/// <param name="dateTimeOffset">当前时间对象。</param>
	/// <returns>当前时间零点的时间对象。</returns>
	public static DateTimeOffset ZeroOfNextDay(this DateTimeOffset dateTimeOffset)
	{
		var nextDay = dateTimeOffset.AddDays(1);
		{ }
		return new DateTimeOffset(
			nextDay.Year,
			nextDay.Month,
			nextDay.Day,
			0,
			0,
			0,
			nextDay.Offset);
	}


	public static DateTimeOffset ZeroOfThisHour(this DateTimeOffset dateTimeOffset)
	{
		return new DateTimeOffset(
			dateTimeOffset.Year,
			dateTimeOffset.Month,
			dateTimeOffset.Day,
			dateTimeOffset.Hour,
			0,
			0,
			dateTimeOffset.Offset);
	}

	public static DateTimeOffset ZeroOfPrevHour(this DateTimeOffset dateTimeOffset)
	{
		dateTimeOffset = dateTimeOffset.AddHours(-1.0);
		return new DateTimeOffset(dateTimeOffset.Year,
dateTimeOffset.Month,
dateTimeOffset.Day,
 dateTimeOffset.Hour, 0, 0,
 dateTimeOffset.Offset);
	}

	public static DateTimeOffset ZeroOfNextHour(this DateTimeOffset dateTimeOffset)
	{
		dateTimeOffset = dateTimeOffset.AddHours(1.0);
		return new DateTimeOffset(dateTimeOffset.Year,
dateTimeOffset.Month,
dateTimeOffset.Day,
 dateTimeOffset.Hour, 0, 0,
 dateTimeOffset.Offset);
	}


	public static DateTimeOffset ZeroOfThisMinute(this DateTimeOffset dateTimeOffset)
	{
		return new DateTimeOffset(dateTimeOffset.Year,
dateTimeOffset.Month,
dateTimeOffset.Day,
 dateTimeOffset.Hour,
 dateTimeOffset.Minute, 0,
 dateTimeOffset.Offset);
	}

	public static DateTimeOffset ZeroOfPrevMinute(this DateTimeOffset dateTimeOffset)
	{
		return dateTimeOffset.AddMinutes(-1.0).ZeroOfThisMinute();
	}

	public static DateTimeOffset ZeroOfNextMinute(this DateTimeOffset dateTimeOffset)
	{
		return dateTimeOffset.AddMinutes(1.0).ZeroOfThisMinute();
	}

	public static DateTimeOffset ZeroOfThisSecond(this DateTimeOffset dateTimeOffset)
	{
		return new DateTimeOffset(dateTimeOffset.Year,
dateTimeOffset.Month,
dateTimeOffset.Day,
 dateTimeOffset.Hour,
 dateTimeOffset.Minute,
 dateTimeOffset.Second,
 dateTimeOffset.Offset);
	}

	public static DateTimeOffset ZeroOfPrevSecond(this DateTimeOffset dateTimeOffset)
	{
		return dateTimeOffset.AddSeconds(-1.0).ZeroOfThisMinute();
	}

	public static DateTimeOffset ZeroOfNextSecond(this DateTimeOffset dateTimeOffset)
	{
		return dateTimeOffset.AddSeconds(1.0).ZeroOfThisMinute();
	}


	/// <summary>
	/// 返回当前时间所属周（以周日为第一天的周）的第一天（零点）的时间对象。
	/// </summary>
	/// <param name="dateTimeOffset">当前时间对象。</param>
	/// <returns>当前时间所属周（以周日为第一天的周）的第一天（零点）的时间对象。</returns>
	public static DateTimeOffset FirstDayOfThisWeekStartsWithSunday(this DateTimeOffset dateTimeOffset)
	{
		var dayOfWeek = dateTimeOffset.DayOfWeek;
		var firstDayOfThisWeek = dateTimeOffset.AddDays(0 - (int)dayOfWeek);
		{
			firstDayOfThisWeek = new DateTimeOffset(
				firstDayOfThisWeek.Year,
				firstDayOfThisWeek.Month,
				firstDayOfThisWeek.Day,
				0,
				0,
				0,
				dateTimeOffset.Offset);
		}
		return firstDayOfThisWeek;
	}

	/// <summary>
	/// 返回当前时间上一周（以周日为第一天的周）的第一天（零点）的时间对象。
	/// </summary>
	/// <param name="dateTimeOffset">当前时间对象。</param>
	/// <returns>当前时间上一周（以周日为第一天的周）的第一天（零点）的时间对象。</returns>
	public static DateTimeOffset FirstDayOfPrevWeekStartsWithSunday(this DateTimeOffset dateTimeOffset)
	{
		var prevWeek = dateTimeOffset.AddDays(-TimeConstants.DaysPerWeek);
		var firstDayOfPrevWeek = prevWeek.FirstDayOfThisWeekStartsWithSunday();
		{ }
		return firstDayOfPrevWeek;
	}

	/// <summary>
	/// 返回当前时间下一周（以周日为第一天的周）的第一天（零点）的时间对象。
	/// </summary>
	/// <param name="dateTimeOffset">当前时间对象。</param>
	/// <returns>当前时间下一周（以周日为第一天的周）的第一天（零点）的时间对象。</returns>
	public static DateTimeOffset FirstDayOfNextWeekStartsWithSunday(this DateTimeOffset dateTimeOffset)
	{

		var nextWeek = dateTimeOffset.AddDays(TimeConstants.DaysPerWeek);
		var firstDayOfNextWeek = nextWeek.FirstDayOfThisWeekStartsWithSunday();
		{ }
		return firstDayOfNextWeek;
	}

	/// <summary>
	/// 返回当前时间所属周（以周一为第一天的周）的第一天（零点）的时间对象。
	/// </summary>
	/// <param name="dateTimeOffset">当前时间对象。</param>
	/// <returns>当前时间所属周（以周一为第一天的周）的第一天（零点）的时间对象。</returns>
	public static DateTimeOffset FirstDayOfThisWeekStartsWithMonday(this DateTimeOffset dateTimeOffset)
	{
		var dayOfWeek = dateTimeOffset.DayOfWeek;
		var firstDayOfThisWeek = dateTimeOffset.AddDays(0 - (int)dayOfWeek + 1);
		{
			firstDayOfThisWeek = new DateTimeOffset(
				firstDayOfThisWeek.Year,
				firstDayOfThisWeek.Month,
				firstDayOfThisWeek.Day,
				0,
				0,
				0,
				dateTimeOffset.Offset);
		}
		return firstDayOfThisWeek;
	}

	/// <summary>
	/// 返回当前时间上一周（以周一为第一天的周）的第一天（零点）的时间对象。
	/// </summary>
	/// <param name="dateTimeOffset">当前时间对象。</param>
	/// <returns>当前时间上一周（以周一为第一天的周）的第一天（零点）的时间对象。</returns>
	public static DateTimeOffset FirstDayOfPrevWeekStartsWithMonday(this DateTimeOffset dateTimeOffset)
	{
		var prevWeek = dateTimeOffset.AddDays(-TimeConstants.DaysPerWeek);
		var firstDayOfPrevWeek = prevWeek.FirstDayOfThisWeekStartsWithMonday();
		{ }
		return firstDayOfPrevWeek;
	}

	/// <summary>
	/// 返回当前时间下一周（以周一为第一天的周）的第一天（零点）的时间对象。
	/// </summary>
	/// <param name="dateTimeOffset">当前时间对象。</param>
	/// <returns>当前时间下一周（以周一为第一天的周）的第一天（零点）的时间对象。</returns>
	public static DateTimeOffset FirstDayOfNextWeekStartsWithMonday(this DateTimeOffset dateTimeOffset)
	{
		var nextWeek = dateTimeOffset.AddDays(TimeConstants.DaysPerWeek);
		var firstDayOfNextWeek = nextWeek.FirstDayOfThisWeekStartsWithMonday();
		{ }
		return firstDayOfNextWeek;
	}

	/// <summary>
	/// 返回当前时间所属月份的第一天（零点）的时间对象。
	/// </summary>
	/// <param name="dateTimeOffset">当前时间对象。</param>
	/// <returns>当前时间所属月份的第一天（零点）的时间对象。</returns>
	public static DateTimeOffset FirstDayOfThisMonth(this DateTimeOffset dateTimeOffset)
	{
		return new DateTimeOffset(
			dateTimeOffset.Year,
			dateTimeOffset.Month,
			1,
			0,
			0,
			0,
			dateTimeOffset.Offset);
	}

	/// <summary>
	/// 返回当前时间上一个月份的第一天的时间（零点）对象。
	/// </summary>
	/// <param name="dateTimeOffset">当前时间对象。</param>
	/// <returns>当前时间上一个月份的第一天的时间（零点）对象。</returns>
	public static DateTimeOffset FirstDayOfPrevMonth(this DateTimeOffset dateTimeOffset)
	{
		return dateTimeOffset.AddMonths(-1).FirstDayOfThisMonth();
	}

	/// <summary>
	/// 返回当前时间下一个月份的第一天的时间（零点）对象。
	/// </summary>
	/// <param name="dateTimeOffset">当前时间对象。</param>
	/// <returns>当前时间下一个月份的第一天的时间（零点）对象。</returns>
	public static DateTimeOffset FirstDayOfNextMonth(this DateTimeOffset dateTimeOffset)
	{
		return dateTimeOffset.AddMonths(1).FirstDayOfThisMonth();
	}

	/// <summary>
	/// 返回当前时间所属季度的第一天（零点）的时间对象。
	/// </summary>
	/// <param name="dateTimeOffset">当前时间对象。</param>
	/// <returns>当前时间所属季度的第一天（零点）的时间对象。</returns>
	public static DateTimeOffset FirstDayOfThisSession(this DateTimeOffset dateTimeOffset)
	{
		var thisMonth = dateTimeOffset.Month;
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
		var firstDayOfThisSession = new DateTimeOffset(
			dateTimeOffset.Year,
			firstMonthOfThisSession,
			1,
			0,
			0,
			0,
			dateTimeOffset.Offset);
		{ }
		return firstDayOfThisSession;
	}

	/// <summary>
	/// 返回当前时间上一个季度的第一天（零点）的时间对象。
	/// </summary>
	/// <param name="dateTimeOffset">当前时间对象。</param>
	/// <returns>当前时间上一个季度的第一天（零点）的时间对象。</returns>
	public static DateTimeOffset FirstDayOfPrevSession(this DateTimeOffset dateTimeOffset)
	{
		return dateTimeOffset
			.AddMonths(-TimeConstants.MonthsPerSession)
			.FirstDayOfThisSession();
	}

	/// <summary>
	/// 返回当前时间下一个季度的第一天（零点）的时间对象。
	/// </summary>
	/// <param name="dateTimeOffset">当前时间对象。</param>
	/// <returns>当前时间下一个季度的第一天（零点）的时间对象。</returns>
	public static DateTimeOffset FirstDayOfNextSession(this DateTimeOffset dateTimeOffset)
	{
		return dateTimeOffset
			.AddMonths(+TimeConstants.MonthsPerSession)
			.FirstDayOfThisSession();
	}


	/// <summary>
	/// 返回当前时间所属年份的第一天（零点）的时间对象。
	/// </summary>
	/// <param name="dateTimeOffset">当前时间对象。</param>
	/// <returns>当前时间所属年份的第一天（零点）的时间对象。</returns>
	public static DateTimeOffset FirstDayOfThisYear(this DateTimeOffset dateTimeOffset)
	{
		return new DateTimeOffset(
			dateTimeOffset.Year,
			1,
			1,
			0,
			0,
			0,
			dateTimeOffset.Offset);
	}

	/// <summary>
	/// 返回当前时间上一年的第一天（零点）的时间对象。
	/// </summary>
	/// <param name="dateTimeOffset">当前时间对象。</param>
	/// <returns>当前时间上一年的第一天（零点）的时间对象。</returns>
	public static DateTimeOffset FirstDayOfPrevYear(this DateTimeOffset dateTimeOffset)
	{
		return new DateTimeOffset(
			dateTimeOffset.Year - 1,
			1,
			1,
			0,
			0,
			0,
			dateTimeOffset.Offset);
	}

	/// <summary>
	/// 返回当前时间下一年的第一天（零点）的时间对象。
	/// </summary>
	/// <param name="dateTimeOffset">当前时间对象。</param>
	/// <returns>当前时间下一年的第一天（零点）的时间对象。</returns>
	public static DateTimeOffset FirstDayOfNextYear(this DateTimeOffset dateTimeOffset)
	{
		return new DateTimeOffset(
			dateTimeOffset.Year + 1,
			1,
			1,
			0,
			0,
			0,
			dateTimeOffset.Offset);
	}

	/// <summary>
	/// 在指定的时间精度上比较两个时间对象。
	/// </summary>
	/// <param name="dateTimeOffset">当前时间对象。</param>
	/// <param name="anotherDateTime">另一个时间对象。</param>
	/// <param name="compareFieldMin">指定的时间精度，类型为：DateTimeField。</param>
	/// <returns>“dateTimeOffset”小于“anotherDateTime”时，返回“-1”；“dateTimeOffset”等于“anotherDateTime”时，返回“0”；“dateTimeOffset”大于“anotherDateTime”时，返回“1”。</returns>
	public static int CompareTo(
		this DateTimeOffset dateTimeOffset,
		DateTimeOffset anotherDateTime,
		DateTimeField compareFieldMin = DateTimeField.Millisecond)
	{
		if (compareFieldMin >= DateTimeField.Year)
		{
			if (dateTimeOffset.Year > anotherDateTime.Year)
			{
				return 1;
			}
			else if (dateTimeOffset.Year < anotherDateTime.Year)
			{
				return -1;
			}
		}
		if (compareFieldMin >= DateTimeField.Month)
		{
			if (dateTimeOffset.Month > anotherDateTime.Month)
			{
				return 1;
			}
			else if (dateTimeOffset.Month < anotherDateTime.Month)
			{
				return -1;
			}
		}
		if (compareFieldMin >= DateTimeField.Day)
		{
			if (dateTimeOffset.Day > anotherDateTime.Day)
			{
				return 1;
			}
			else if (dateTimeOffset.Day < anotherDateTime.Day)
			{
				return -1;
			}
		}
		if (compareFieldMin >= DateTimeField.Hour)
		{
			if (dateTimeOffset.Hour > anotherDateTime.Hour)
			{
				return 1;
			}
			else if (dateTimeOffset.Hour < anotherDateTime.Hour)
			{
				return -1;
			}
		}
		if (compareFieldMin >= DateTimeField.Minute)
		{
			if (dateTimeOffset.Minute > anotherDateTime.Minute)
			{
				return 1;
			}
			else if (dateTimeOffset.Minute < anotherDateTime.Minute)
			{
				return -1;
			}
		}
		if (compareFieldMin >= DateTimeField.Second)
		{
			if (dateTimeOffset.Second > anotherDateTime.Second)
			{
				return 1;
			}
			else if (dateTimeOffset.Second < anotherDateTime.Second)
			{
				return -1;
			}
		}
		if (compareFieldMin >= DateTimeField.Millisecond)
		{
			if (dateTimeOffset.Millisecond > anotherDateTime.Millisecond)
			{
				return 1;
			}
			else if (dateTimeOffset.Millisecond < anotherDateTime.Millisecond)
			{
				return -1;
			}
		}
		return 0;
	}


	/// <summary>
	/// 在指定的时间周期内比较两个时间对象，如：指定的时间范围为“Year”时，则比较时忽略时间对象的年份值。
	/// </summary>
	/// <param name="dateTimeOffset"></param>
	/// <param name="anotherDateTime"></param>
	/// <param name="compareCycle"></param>
	/// <returns></returns>
	public static int CompareTo(
		this DateTimeOffset dateTimeOffset,
		DateTimeOffset anotherDateTime,
		DateTimeCycle compareCycle)
	{
		switch (compareCycle)
		{
			default:
			case DateTimeCycle.All:
			case DateTimeCycle.Century:
				{
					return dateTimeOffset.CompareTo(anotherDateTime);
				}
			case DateTimeCycle.Year:
				{
					if (dateTimeOffset.Month < anotherDateTime.Month)
					{
						return -1;
					}
					else if (dateTimeOffset.Month > anotherDateTime.Month)
					{
						return 1;
					}
					else if (dateTimeOffset.Day < anotherDateTime.Day)
					{
						return -1;
					}
					else if (dateTimeOffset.Day > anotherDateTime.Day)
					{
						return 1;
					}
					else if (dateTimeOffset.Hour < anotherDateTime.Hour)
					{
						return -1;
					}
					else if (dateTimeOffset.Hour > anotherDateTime.Hour)
					{
						return 1;
					}
					else if (dateTimeOffset.Minute < anotherDateTime.Minute)
					{
						return -1;
					}
					else if (dateTimeOffset.Minute > anotherDateTime.Minute)
					{
						return 1;
					}
					else if (dateTimeOffset.Second < anotherDateTime.Second)
					{
						return -1;
					}
					else if (dateTimeOffset.Second > anotherDateTime.Second)
					{
						return 1;
					}
					else if (dateTimeOffset.Millisecond < anotherDateTime.Millisecond)
					{
						return -1;
					}
					else if (dateTimeOffset.Millisecond > anotherDateTime.Millisecond)
					{
						return 1;
					}
				}
				break;
			case DateTimeCycle.Month:
				{
					if (dateTimeOffset.Day < anotherDateTime.Day)
					{
						return -1;
					}
					else if (dateTimeOffset.Day > anotherDateTime.Day)
					{
						return 1;
					}
					else if (dateTimeOffset.Hour < anotherDateTime.Hour)
					{
						return -1;
					}
					else if (dateTimeOffset.Hour > anotherDateTime.Hour)
					{
						return 1;
					}
					else if (dateTimeOffset.Minute < anotherDateTime.Minute)
					{
						return -1;
					}
					else if (dateTimeOffset.Minute > anotherDateTime.Minute)
					{
						return 1;
					}
					else if (dateTimeOffset.Second < anotherDateTime.Second)
					{
						return -1;
					}
					else if (dateTimeOffset.Second > anotherDateTime.Second)
					{
						return 1;
					}
					else if (dateTimeOffset.Millisecond < anotherDateTime.Millisecond)
					{
						return -1;
					}
					else if (dateTimeOffset.Millisecond > anotherDateTime.Millisecond)
					{
						return 1;
					}
				}
				break;
			case DateTimeCycle.Week:
				{
					if (dateTimeOffset.DayOfWeek < anotherDateTime.DayOfWeek)
					{
						return -1;
					}
					else if (dateTimeOffset.DayOfWeek > anotherDateTime.DayOfWeek)
					{
						return 1;
					}
					else if (dateTimeOffset.Hour < anotherDateTime.Hour)
					{
						return -1;
					}
					else if (dateTimeOffset.Hour > anotherDateTime.Hour)
					{
						return 1;
					}
					else if (dateTimeOffset.Minute < anotherDateTime.Minute)
					{
						return -1;
					}
					else if (dateTimeOffset.Minute > anotherDateTime.Minute)
					{
						return 1;
					}
					else if (dateTimeOffset.Second < anotherDateTime.Second)
					{
						return -1;
					}
					else if (dateTimeOffset.Second > anotherDateTime.Second)
					{
						return 1;
					}
					else if (dateTimeOffset.Millisecond < anotherDateTime.Millisecond)
					{
						return -1;
					}
					else if (dateTimeOffset.Millisecond > anotherDateTime.Millisecond)
					{
						return 1;
					}
				}
				break;
			case DateTimeCycle.Day:
				{
					if (dateTimeOffset.Hour < anotherDateTime.Hour)
					{
						return -1;
					}
					else if (dateTimeOffset.Hour > anotherDateTime.Hour)
					{
						return 1;
					}
					else if (dateTimeOffset.Minute < anotherDateTime.Minute)
					{
						return -1;
					}
					else if (dateTimeOffset.Minute > anotherDateTime.Minute)
					{
						return 1;
					}
					else if (dateTimeOffset.Second < anotherDateTime.Second)
					{
						return -1;
					}
					else if (dateTimeOffset.Second > anotherDateTime.Second)
					{
						return 1;
					}
					else if (dateTimeOffset.Millisecond < anotherDateTime.Millisecond)
					{
						return -1;
					}
					else if (dateTimeOffset.Millisecond > anotherDateTime.Millisecond)
					{
						return 1;
					}
				}
				break;
			case DateTimeCycle.Hour:
				{
					if (dateTimeOffset.Minute < anotherDateTime.Minute)
					{
						return -1;
					}
					else if (dateTimeOffset.Minute > anotherDateTime.Minute)
					{
						return 1;
					}
					else if (dateTimeOffset.Second < anotherDateTime.Second)
					{
						return -1;
					}
					else if (dateTimeOffset.Second > anotherDateTime.Second)
					{
						return 1;
					}
					else if (dateTimeOffset.Millisecond < anotherDateTime.Millisecond)
					{
						return -1;
					}
					else if (dateTimeOffset.Millisecond > anotherDateTime.Millisecond)
					{
						return 1;
					}
				}
				break;
			case DateTimeCycle.Minute:
				{
					if (dateTimeOffset.Second < anotherDateTime.Second)
					{
						return -1;
					}
					else if (dateTimeOffset.Second > anotherDateTime.Second)
					{
						return 1;
					}
					else if (dateTimeOffset.Millisecond < anotherDateTime.Millisecond)
					{
						return -1;
					}
					else if (dateTimeOffset.Millisecond > anotherDateTime.Millisecond)
					{
						return 1;
					}
				}
				break;
			case DateTimeCycle.Second:
				{
					if (dateTimeOffset.Millisecond < anotherDateTime.Millisecond)
					{
						return -1;
					}
					else if (dateTimeOffset.Millisecond > anotherDateTime.Millisecond)
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
	       this DateTimeOffset dateTimeOffset,
	       DateTimeOffset anotherDateTime)
	{
		return dateTimeOffset.CompareTo(
			anotherDateTime,
			DateTimeField.Year)
			< 0;
	}

	public static bool EqualsInYear(
		this DateTimeOffset dateTimeOffset,
		DateTimeOffset anotherDateTime)
	{
		return dateTimeOffset.CompareTo(
			anotherDateTime,
			DateTimeField.Year)
			== 0;
	}

	public static bool IsLaterInYear(
	       this DateTimeOffset dateTimeOffset,
	       DateTimeOffset anotherDateTime)
	{
		return dateTimeOffset.CompareTo(
			anotherDateTime,
			DateTimeField.Year)
			> 0;
	}

	public static bool IsEarlierInMonth(
	       this DateTimeOffset dateTimeOffset,
	       DateTimeOffset anotherDateTime)
	{
		return dateTimeOffset.CompareTo(
			anotherDateTime,
			DateTimeField.Month)
			< 0;
	}

	public static bool EqualsInMonth(
		this DateTimeOffset dateTimeOffset,
		DateTimeOffset anotherDateTime)
	{
		return dateTimeOffset.CompareTo(
			anotherDateTime,
			DateTimeField.Month)
			== 0;
	}

	public static bool IsLaterInMonth(
	       this DateTimeOffset dateTimeOffset,
	       DateTimeOffset anotherDateTime)
	{
		return dateTimeOffset.CompareTo(
			anotherDateTime,
			DateTimeField.Month)
			> 0;
	}

	public static bool IsLastMonthOf(
		this DateTimeOffset dateTimeOffset,
	       DateTimeOffset currentMonth)
	{
		var dateTimeBeforeMonth = currentMonth.AddMonths(-1);
		if (dateTimeBeforeMonth.EqualsInMonth(dateTimeOffset))
		{
			return true;
		}
		return false;
	}

	public static bool IsNextMonthOf(
		this DateTimeOffset dateTimeOffset,
	       DateTimeOffset currentMonth)
	{
		var dateTimeAfterMonth = currentMonth.AddMonths(1);
		if (dateTimeAfterMonth.EqualsInMonth(dateTimeOffset))
		{
			return true;
		}
		return false;
	}

	public static bool IsLastWeekInStartsWithMondayOf(
		this DateTimeOffset dateTimeOffset,
	       DateTimeOffset currentWeek)
	{
		var currentWeekBeginTime
			= currentWeek.FirstDayOfThisWeekStartsWithMonday();
		var lastWeekBeginTime
			= currentWeek.AddDays(-1 * TimeConstants.DaysPerWeek);
		if (dateTimeOffset >= lastWeekBeginTime
			&& dateTimeOffset < currentWeekBeginTime)
		{
			return true;
		}
		return false;
	}

	public static bool IsNextWeekInStartsWithMondayOf(
		this DateTimeOffset dateTimeOffset,
	       DateTimeOffset currentWeek)
	{
		var nextWeekBeginTime
			= currentWeek.FirstDayOfNextWeekStartsWithMonday();
		var nextWeekEndTime
			= nextWeekBeginTime.AddDays(TimeConstants.DaysPerWeek);
		if (dateTimeOffset >= nextWeekBeginTime
			&& dateTimeOffset < nextWeekEndTime)
		{
			return true;
		}
		return false;
	}

	public static bool IsEarlierInDay(
	       this DateTimeOffset dateTimeOffset,
	       DateTimeOffset anotherDateTime)
	{
		return dateTimeOffset.CompareTo(
			anotherDateTime,
			DateTimeField.Day)
			< 0;
	}

	public static bool EqualsInDay(
		this DateTimeOffset dateTimeOffset,
		DateTimeOffset anotherDateTime)
	{
		return dateTimeOffset.CompareTo(
			anotherDateTime,
			DateTimeField.Day)
			== 0;
	}

	public static bool IsLaterInDay(
	       this DateTimeOffset dateTimeOffset,
	       DateTimeOffset anotherDateTime)
	{
		return dateTimeOffset.CompareTo(
			anotherDateTime,
			DateTimeField.Day)
			> 0;
	}

	public static bool IsLastDayOf(this DateTimeOffset dateTimeOffset, DateTimeOffset today)
	{
		var dateTimeBeforeDay = today.AddDays(-1);
		if (dateTimeBeforeDay.EqualsInDay(dateTimeOffset))
		{
			return true;
		}
		return false;
	}
	public static bool IsNextDayOf(this DateTimeOffset dateTimeOffset, DateTimeOffset today)
	{
		var dateTimeAfterDay = today.AddDays(1);
		if (dateTimeAfterDay.EqualsInDay(dateTimeOffset))
		{
			return true;
		}
		return false;
	}

	public static bool IsYesterdayOf(
		this DateTimeOffset dateTimeOffset,
	       DateTimeOffset today)
	{
		return dateTimeOffset.IsLastDayOf(today);
	}

	public static bool IsTomorrowOf(
		this DateTimeOffset dateTimeOffset,
	       DateTimeOffset today)
	{
		return dateTimeOffset.IsNextDayOf(today);
	}

	public static bool IsEarlierInHour(
	       this DateTimeOffset dateTimeOffset,
	       DateTimeOffset anotherDateTime)
	{
		return dateTimeOffset.CompareTo(
			anotherDateTime,
			DateTimeField.Hour)
			< 0;
	}

	public static bool EqualsInHour(
		this DateTimeOffset dateTimeOffset,
		DateTimeOffset anotherDateTime)
	{
		return dateTimeOffset.CompareTo(
			anotherDateTime,
			DateTimeField.Hour)
			== 0;
	}

	public static bool IsLaterInHour(
	       this DateTimeOffset dateTimeOffset,
	       DateTimeOffset anotherDateTime)
	{
		return dateTimeOffset.CompareTo(
			anotherDateTime,
			DateTimeField.Hour)
			> 0;
	}

	public static bool IsLastHourOf(this DateTimeOffset dateTimeOffset, DateTimeOffset currentDateTime)
	{
		if (dateTimeOffset.AddHours(1).EqualsInHour(currentDateTime))
		{
			return true;
		}
		return false;
	}

	public static bool IsNextHourOf(this DateTimeOffset dateTimeOffset, DateTimeOffset currentDateTime)
	{
		if (dateTimeOffset.AddHours(-1).EqualsInHour(currentDateTime))
		{
			return true;
		}
		return false;
	}


	public static bool IsEarlierInMinute(
	       this DateTimeOffset dateTimeOffset,
	       DateTimeOffset anotherDateTime)
	{
		return dateTimeOffset.CompareTo(
			anotherDateTime,
			DateTimeField.Minute)
			< 0;
	}

	public static bool EqualsInMinute(
		this DateTimeOffset dateTimeOffset,
		DateTimeOffset anotherDateTime)
	{
		return dateTimeOffset.CompareTo(
			anotherDateTime,
			DateTimeField.Minute)
			== 0;
	}

	public static bool IsLaterInMinute(
	       this DateTimeOffset dateTimeOffset,
	       DateTimeOffset anotherDateTime)
	{
		return dateTimeOffset.CompareTo(
			anotherDateTime,
			DateTimeField.Minute)
			> 0;
	}

	public static bool IsLastMinuteOf(this DateTimeOffset dateTimeOffset, DateTimeOffset currentDateTime)
	{
		if (dateTimeOffset.AddMinutes(1).EqualsInMinute(currentDateTime))
		{
			return true;
		}
		return false;
	}

	public static bool IsNextMinuteOf(this DateTimeOffset dateTimeOffset, DateTimeOffset currentDateTime)
	{
		if (dateTimeOffset.AddMinutes(-1).EqualsInMinute(currentDateTime))
		{
			return true;
		}
		return false;
	}

	public static bool IsEarlierInSecond(
	       this DateTimeOffset dateTimeOffset,
	       DateTimeOffset anotherDateTime)
	{
		return dateTimeOffset.CompareTo(
			anotherDateTime,
			DateTimeField.Second)
			< 0;
	}

	public static bool EqualsInSecond(
		this DateTimeOffset dateTimeOffset,
		DateTimeOffset anotherDateTime)
	{
		return dateTimeOffset.CompareTo(
			anotherDateTime,
			DateTimeField.Second)
			== 0;
	}

	public static bool IsLaterInSecond(
	       this DateTimeOffset dateTimeOffset,
	       DateTimeOffset anotherDateTime)
	{
		return dateTimeOffset.CompareTo(
			anotherDateTime,
			DateTimeField.Second)
			> 0;
	}

	public static bool IsLastSecondOf(this DateTimeOffset dateTimeOffset, DateTimeOffset currentDateTime)
	{
		if (dateTimeOffset.AddSeconds(1).EqualsInSecond(currentDateTime))
		{
			return true;
		}
		return false;
	}

	public static bool IsNextSecondOf(this DateTimeOffset dateTimeOffset, DateTimeOffset currentDateTime)
	{
		if (dateTimeOffset.AddSeconds(-1).EqualsInSecond(currentDateTime))
		{
			return true;
		}
		return false;
	}

	public static bool IsEarlierInMillisecond(
	       this DateTimeOffset dateTimeOffset,
	       DateTimeOffset anotherDateTime)
	{
		return dateTimeOffset.CompareTo(
			anotherDateTime,
			DateTimeField.Millisecond)
			< 0;
	}

	public static bool EqualsInMillisecond(
		this DateTimeOffset dateTimeOffset,
		DateTimeOffset anotherDateTime)
	{
		return dateTimeOffset.CompareTo(
			anotherDateTime,
			DateTimeField.Millisecond)
			== 0;
	}

	public static bool IsLaterInMillisecond(
	       this DateTimeOffset dateTimeOffset,
	       DateTimeOffset anotherDateTime)
	{
		return dateTimeOffset.CompareTo(
			anotherDateTime,
			DateTimeField.Millisecond)
			> 0;
	}
	public static bool IsLastMillisecondOf(this DateTimeOffset dateTimeOffset, DateTimeOffset currentDateTime)
	{
		if (dateTimeOffset.AddMilliseconds(1).EqualsInMillisecond(currentDateTime))
		{
			return true;
		}
		return false;
	}

	public static bool IsNextMillisecondOf(this DateTimeOffset dateTimeOffset, DateTimeOffset currentDateTime)
	{
		if (dateTimeOffset.AddMilliseconds(-1).EqualsInMillisecond(currentDateTime))
		{
			return true;
		}
		return false;
	}

	public static bool IsBetween(
		this DateTimeOffset dateTimeOffset,
		DateTimeOffset? beginTime,
		DateTimeOffset? endTime,
		DateTimeField compareFieldMin)
	{
		if (beginTime != null
			&& endTime != null)
		{
			if (dateTimeOffset.CompareTo(beginTime.Value, compareFieldMin) >= 0
				&& dateTimeOffset.CompareTo(endTime.Value, compareFieldMin) < 0)
			{
				return true;
			}
		}
		else if (beginTime != null)
		{
			if (dateTimeOffset.CompareTo(beginTime.Value, compareFieldMin) >= 0)
			{
				return true;
			}
		}
		else if (endTime != null)
		{
			if (dateTimeOffset.CompareTo(endTime.Value, compareFieldMin) < 0)
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsContinuousAfter(
		this DateTimeOffset currentDateTime,
		DateTimeOffset lastDateTime,
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
		this DateTimeOffset currentDateTime,
		DateTimeOffset nextDateTime,
		DateTimeField continuousAccuracy,
		bool isEqualsToContinuous = true)
	{
		return nextDateTime.IsContinuousAfter(
			currentDateTime,
			continuousAccuracy,
			isEqualsToContinuous);
	}

	public static string CaptionOfListElementDefault(
		this DateTimeOffset dateTimeOffset,
		bool isNeedSecondsField = false)
	{
		string caption;
		var now = DateTimeOffset.Now;
		if (dateTimeOffset.EqualsInDay(now))
		{
			if (isNeedSecondsField)
			{
				caption = dateTimeOffset.ToString("今天 HH:mm:ss");
			}
			else
			{
				caption = dateTimeOffset.ToString("今天 HH:mm");
			}
		}
		else if (dateTimeOffset.IsYesterdayOf(now))
		{
			if (isNeedSecondsField)
			{
				caption = dateTimeOffset.ToString("昨天 HH:mm:ss");
			}
			else
			{
				caption = dateTimeOffset.ToString("昨天 HH:mm");
			}
		}
		else if (dateTimeOffset.IsTomorrowOf(now))
		{
			if (isNeedSecondsField)
			{
				caption = dateTimeOffset.ToString("明天 HH:mm:ss");
			}
			else
			{
				caption = dateTimeOffset.ToString("明天 HH:mm");
			}
		}
		else if (dateTimeOffset.EqualsInYear(now))
		{
			caption = dateTimeOffset.ToString("MM月dd天 HH:mm");
		}
		else
		{
			caption = dateTimeOffset.ToString("yyyy年MM月dd日");
		}
		return caption;
	}

	public static string CaptionOfDetailPageDefault(
		this DateTimeOffset dateTimeOffset,
		bool isNeedSecondsField = false)
	{
		string caption;
		var now = DateTimeOffset.Now;
		if (dateTimeOffset.EqualsInDay(now))
		{
			if (isNeedSecondsField)
			{
				caption = dateTimeOffset.ToString("今天 HH:mm:ss");
			}
			else
			{
				caption = dateTimeOffset.ToString("今天 HH:mm");
			}
		}
		else if (dateTimeOffset.IsYesterdayOf(now))
		{
			if (isNeedSecondsField)
			{
				caption = dateTimeOffset.ToString("昨天 HH:mm:ss");
			}
			else
			{
				caption = dateTimeOffset.ToString("昨天 HH:mm");
			}
		}
		else if (dateTimeOffset.IsTomorrowOf(now))
		{
			if (isNeedSecondsField)
			{
				caption = dateTimeOffset.ToString("明天 HH:mm:ss");
			}
			else
			{
				caption = dateTimeOffset.ToString("明天 HH:mm");
			}
		}
		else if (dateTimeOffset.EqualsInYear(now))
		{
			if (isNeedSecondsField)
			{
				caption = dateTimeOffset.ToString("MM月dd日 HH:mm:ss");
			}
			else
			{
				caption = dateTimeOffset.ToString("MM月dd日 HH:mm");
			}
		}
		else
		{
			if (isNeedSecondsField)
			{
				caption = dateTimeOffset.ToString("yyyy年MM月dd日 HH:mm:ss");
			}
			else
			{
				caption = dateTimeOffset.ToString("yyyy年MM月dd日");
			}
		}
		return caption;
	}

	public static DateTimeOffset ToDateTimeOffsetBySetTimeZoneValue(
		this DateTimeOffset dateTimeOffset,
		TimeSpan timeZoneValue)
	{
		return new DateTimeOffset(
			dateTimeOffset.DateTime,
			timeZoneValue);
	}

	public static DateTimeOffset ToDateTimeOffsetBySetTimeZoneNumber(
		this DateTimeOffset dateTimeOffset,
		TimeZoneNumber timeZoneNumber)
	{
		return ToDateTimeOffsetBySetTimeZoneValue(
			dateTimeOffset,
			new TimeSpan((int)timeZoneNumber, 0, 0));
	}

	public static DateTimeOffset ToDateTimeOffsetByConvertToTimeZone(
			this DateTimeOffset dateTimeOffset,
			TimeZoneNumber timeZoneNumber)
	{
		return DateTimeOffsetUtil.DateTimeOffsetByConvertToTimeZone(
			dateTimeOffset,
			timeZoneNumber);
	}

	/// <summary>
	/// 使用遵循【RFC3339】标准格式的字符串，生成指定时区的日期时间对象的RFC3339字符串。
	/// 【RFC3339】标准格式为：
	/// “yyyy-MM-DDTHH:mm:ss+TIMEZONE“，
	/// “yyyy-MM-DD“，表示年月日，
	/// “T“，出现在字符串中，表示time元素的开头，
	/// “HH:mm:ss”，表示时分秒，
	/// TIMEZONE表示时区（+08:00表示东八区时间，领先UTC 8小时，即北京时间），
	/// 
	/// 例如：2015-05-20T13:29:35+08:00表示，北京时间2015年5月20日 13点29分35秒。
	/// 样例数据："success_time":"2018-06-08T10:34:56+08:00" 。
	/// 
	/// 转换失败时返回指定的日期时间默认值，未指定日期时间默认值时，默认返回”DateTimeOffset.MinValue“。
	/// </summary>
	/// <param name="dateTimeOffset">原始的时间信息对象。</param>
	/// <returns>返回遵循【RFC3339】标准格式的字符串。 </returns>
	public static string ToStringInRFC3339(this DateTimeOffset dateTimeOffset)
	{
		var stringInRFC3339 = dateTimeOffset.ToString("yyyy-MM-ddTHH:mm:ss");
		{
			var timeZoneOffset = dateTimeOffset.Offset;
			if (timeZoneOffset.Ticks >= 0)
			{
				stringInRFC3339 += $"+{timeZoneOffset.Hours:D2}:{timeZoneOffset.Minutes:D2}";
			}
			else
			{
				stringInRFC3339 += $"-{timeZoneOffset.Hours:D2}:{timeZoneOffset.Minutes:D2}";
			}
		}
		return stringInRFC3339;
	}

	/// <summary>
	/// 将“DateTimeOffset”转为本地时间的“DateTime“。
	/// </summary>
	/// <param name="dateTimeOffset">当前“DateTimeOffset”对象。</param>
	/// <param name="dateTimeDefault">当前“DateTimeOffset”对象为“null”时，返回的默认“DateTime”值，默认为“null”，返回“DateTime.MinValue”。</param>
	/// <returns>当前“DateTimeOffset”对象对应的本地时间的“DateTime“。</returns>
	public static DateTime ToLocalDateTime(
		this DateTimeOffset? dateTimeOffset,
		DateTime? dateTimeDefault = null)
	{
		var dateTime = dateTimeDefault ?? DateTime.MinValue;
		if (dateTimeOffset != null)
		{
			dateTime = dateTimeOffset.Value.LocalDateTime;
		}
		return dateTime;
	}

	#endregion
}