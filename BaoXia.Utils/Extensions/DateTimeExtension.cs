using BaoXia.Utils.Constants;
using System;

namespace BaoXia.Utils.Extensions
{
	/// <summary>
	/// “DateTime”扩展类。
	/// </summary>
	public static class DateTimeExtension
	{
		public enum DateTimeField
		{
			Year,
			Month,
			Day,
			Hour,
			Minute,
			Second,
			Millisecond
		}

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

		/// <summary>
		/// 获取当前时间的UTC时间戳，当前时间的UTC时间距离1970年1月1日零时的毫秒数。
		/// </summary>
		/// <param name="dateTime">当前时间对象。</param>
		/// <returns>返回当前时间的UTC时间距离1970年1月1日零时的毫秒数。</returns>
		public static long MillisecondsFrom1970(this DateTime dateTime)
		{
			var dateTimeUtc = TimeZoneInfo.ConvertTimeToUtc(dateTime);
			var zeroTime = new DateTime(1970, 1, 1);

			var millisecondsFrom1970 = (long)(dateTimeUtc - zeroTime).TotalMilliseconds;
			{ }
			return millisecondsFrom1970;
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
			{}
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
		/// 在置顶的时间精度上比较两个时间对象。
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
	}
}
