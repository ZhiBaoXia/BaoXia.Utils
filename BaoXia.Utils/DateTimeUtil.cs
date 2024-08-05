using BaoXia.Utils.Constants;
using BaoXia.Utils.Extensions;
using System;

namespace BaoXia.Utils;

public class DateTimeUtil
{
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
		TimeZoneNumber millisecondsZoneNumber = TimeZoneNumber.Utc0)
	{
		var dateTimeInTimeZone = dateTime.ToDateTimeInTimeZone(millisecondsZoneNumber);
		var dateTimeZero = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		var milliseconds = (long)(dateTimeInTimeZone - dateTimeZero).TotalMilliseconds;
		{ }
		return milliseconds;
	}

	public static long GetSecondsFrom1970OfDateTime(
		DateTime dateTime,
		TimeZoneNumber secondsZoneNumber = TimeZoneNumber.Utc0)
	{
		return GetMillisecondsFrom1970OfDateTime(dateTime, secondsZoneNumber)
			/ 1000;
	}

	public static DateTime DateTimeWithMillisecondsAfter1970(
		long milliseconds,
		TimeZoneNumber millisecondsTimeZoneNumber = TimeZoneNumber.Utc0)
	{
		var dateTime
			= new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local)
			.AddMilliseconds(milliseconds);
		var timeSpanToObjectTimeZone
			= GetTimeSpanFromLocalToObjectTimeZone(
				millisecondsTimeZoneNumber);
		if (timeSpanToObjectTimeZone.Ticks == 0)
		{
			return dateTime;
		}
		dateTime = dateTime.Subtract(timeSpanToObjectTimeZone);
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
		DateTime objectDateTime;
		if (dateTime.Kind == DateTimeKind.Utc)
		{
			objectDateTime = dateTime.AddHours((int)timeZoneNumber);
		}
		else
		{
			objectDateTime
				= dateTime.AddHours(
					-TimeZoneInfo.Local.GetUtcOffset(dateTime).TotalHours
					+ (int)timeZoneNumber);
		}
		return objectDateTime;
	}

	#endregion
}
