using BaoXia.Utils.ConcurrentTools;
using BaoXia.Utils.Constants;
using System;

namespace BaoXia.Utils;

public class DateTimeUtil
{
	////////////////////////////////////////////////
	// @静态常量
	////////////////////////////////////////////////

	#region 静态常量

	public static readonly DateTime DateTimeAtUTCZero = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	public static readonly DateTime DateTimeAtLocalZero = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);

	#endregion


	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static TimeSpan GetTimeSpanFromLocalToObjectTimeZone(
		TimeZoneNumber objectTimeZoneNumber)
	{
		var timeSpan
			= new TimeSpan((int)objectTimeZoneNumber, 0, 0)
			- TimeZoneInfo.Local.BaseUtcOffset;
		{ }
		return timeSpan;
	}

	public static long GetMillisecondsFrom1970OfDateTime(
		DateTime dateTime,
		TimeZoneNumber millisecondsZoneNumber,// = TimeZoneNumber.Utc0,
		bool isMillisecondsMinZero)
	{
		long dateTimeTicks = dateTime.Ticks - DateTimeAtUTCZero.Ticks;
		if (dateTime.Kind != DateTimeKind.Utc)
		{
			dateTimeTicks -= TimeZoneInfo.Local.BaseUtcOffset.Ticks;
		}

		var milliseconds = dateTimeTicks / TimeSpan.TicksPerMillisecond;
		if (milliseconds < 0
			&& isMillisecondsMinZero)
		{
			milliseconds = 0;
		}
		return milliseconds;
	}

	public static long GetSecondsFrom1970OfDateTime(
		DateTime dateTime,
		TimeZoneNumber secondsZoneNumber,// = TimeZoneNumber.Utc0,
		bool isMillisecondsMinZero)
	{
		return GetMillisecondsFrom1970OfDateTime(
			dateTime,
			secondsZoneNumber,
			isMillisecondsMinZero)
			/ 1000;
	}

	public static DateTime DateTimeWithMillisecondsAfter1970(
		long milliseconds,
		TimeZoneNumber millisecondsTimeZoneNumber = TimeZoneNumber.Utc0)
	{
		var dateTimeTicks
			= TimeSpan.TicksPerMicrosecond * milliseconds
			- TimeSpan.TicksPerHour * (int)millisecondsTimeZoneNumber
			+ TimeZoneInfo.Local.BaseUtcOffset.Ticks;
		if (dateTimeTicks<DateTime.MinValue.Ticks)
		{
			dateTimeTicks = DateTime.MinValue.Ticks;
		}
		else if (dateTimeTicks > DateTime.MinValue.Ticks)
		{
			dateTimeTicks = DateTime.MaxValue.Ticks;
		}
		var dateTime = new DateTime(dateTimeTicks, DateTimeKind.Local);
		{ }
		return dateTime;
	}

	public static DateTime DateTimeWithSecondsAfter1970(
		long seconds,
		TimeZoneNumber secondsTimeZoneNumber = TimeZoneNumber.Utc0)
	{
		return DateTimeWithMillisecondsAfter1970(
			seconds * 1000,
			secondsTimeZoneNumber);
	}

	public static DateTime DateTimeByOffsetToTimeZone(
		DateTime dateTime,
		TimeZoneNumber timeZoneNumber)
	{
		long dateTimeOffsetHours = 0;
		int timeZoneNumberHours = (int)timeZoneNumber;
		if (dateTime.Kind != DateTimeKind.Utc)
		{
			dateTimeOffsetHours -= (long)TimeZoneInfo.Local.GetUtcOffset(dateTime).TotalHours;
		}
		dateTimeOffsetHours += timeZoneNumberHours;

		var dateTimeOffsetTicks
			= TimeSpan.TicksPerHour * dateTimeOffsetHours;
		var dateTimeTicks
			= dateTime.Ticks;
		var objectDateTimeTicks
			= dateTimeTicks + dateTimeOffsetTicks;
		if (objectDateTimeTicks < DateTime.MinValue.Ticks)
		{
			objectDateTimeTicks = DateTime.MinValue.Ticks;
		}
		else if (objectDateTimeTicks > DateTime.MaxValue.Ticks)
		{
			objectDateTimeTicks = DateTime.MaxValue.Ticks;
		}
		var objectDateTime = new DateTime(objectDateTimeTicks, dateTime.Kind);
		{ }
		return objectDateTime;
	}

	#endregion
}
