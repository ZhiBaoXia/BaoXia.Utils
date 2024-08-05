using BaoXia.Utils.Constants;
using BaoXia.Utils.Extensions;
using System;

namespace BaoXia.Utils;

public class DateTimeOffsetUtil
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

	public static long GetMillisecondsFrom1970OfDateTimeOffset(
		DateTimeOffset dateTimeOffset,
		TimeZoneNumber millisecondsZoneNumber = TimeZoneNumber.Utc0)
	{
		var dateTimeInTimeZone = DateTimeOffsetUtil.DateTimeOffsetByConvertToTimeZone(
			dateTimeOffset,
			millisecondsZoneNumber);
		var dateTimeZero = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.FromHours((int)millisecondsZoneNumber));
		var milliseconds = (long)(dateTimeInTimeZone - dateTimeZero).TotalMilliseconds;
		{ }
		return milliseconds;
	}

	public static long GetSecondsFrom1970OfDateTimeOffset(
		DateTimeOffset dateTimeOffset,
		TimeZoneNumber secondsZoneNumber = TimeZoneNumber.Utc0)
	{
		return GetMillisecondsFrom1970OfDateTimeOffset(dateTimeOffset, secondsZoneNumber)
			/ 1000;
	}

	public static DateTimeOffset DateTimeOffsetWithMillisecondsAfter1970(
		long milliseconds,
		TimeZoneNumber millisecondsTimeZoneNumber = TimeZoneNumber.Utc0)
	{
		var dateTimeOffset
			= new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)
			.AddMilliseconds(milliseconds);
		var timeSpanToObjectTimeZone
			= GetTimeSpanFromLocalToObjectTimeZone(
				millisecondsTimeZoneNumber);
		if (timeSpanToObjectTimeZone.Ticks == 0)
		{
			return dateTimeOffset;
		}
		dateTimeOffset = dateTimeOffset.Subtract(timeSpanToObjectTimeZone);
		{ }
		return dateTimeOffset;
	}

	public static DateTimeOffset DateTimeOffsetWithSecondsAfter1970(
		long seconds,
		TimeZoneNumber secondsTimeZoneNumber = TimeZoneNumber.Utc0)
	{
		return DateTimeOffsetWithMillisecondsAfter1970(
			seconds * 1000,
			secondsTimeZoneNumber);
	}

	public static DateTimeOffset DateTimeOffsetByConvertToTimeZone(
		DateTimeOffset dateTimeOffset,
		TimeZoneNumber timeZoneNumber)
	{
		DateTimeOffset objectDateTime = new(
			dateTimeOffset.DateTime,
			new TimeSpan((int)timeZoneNumber, 0, 0));
		{
			double objectTimeZoneOffsetHours = (double)timeZoneNumber;
			var hoursNeedOffset = objectTimeZoneOffsetHours - dateTimeOffset.Offset.TotalHours;
			// !!!
			objectDateTime = objectDateTime.AddHours(hoursNeedOffset);
			// !!!
		}
		return objectDateTime;
	}

	/// <summary>
	/// 使用遵循【RFC3339】标准格式的字符串，生成指定时区的日期时间对象。
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
	/// <param name="dateTimeStringInRFC3339">遵循【RFC3339】标准格式的字符串。</param>
	/// <param name="defaultValue">解析失败时返回的默认值，未指定日期时间默认值时，默认返回”DateTimeOffset.MinValue“。</param>
	/// <returns>返回解析字符串生成的DateTimeOffset对象。</returns>
	public static DateTimeOffset DateTimeOffsetFromStringInRFC3339(
		string? dateTimeStringInRFC3339,
		DateTimeOffset? defaultValue = null)
	{
		defaultValue ??= DateTimeOffset.MinValue;
		if (string.IsNullOrEmpty(dateTimeStringInRFC3339))
		{
			return defaultValue.Value;
		}

		var indexOf_T = dateTimeStringInRFC3339.IndexOf('T', StringComparison.OrdinalIgnoreCase);
		string[] dateTimeStringInRFC3339Sections
			= indexOf_T < 0
			? [dateTimeStringInRFC3339]
			: [dateTimeStringInRFC3339[..indexOf_T], dateTimeStringInRFC3339[(indexOf_T + 1)..]];
		if (dateTimeStringInRFC3339Sections.Length < 1)
		{
			return defaultValue.Value;
		}

		string dateString = dateTimeStringInRFC3339Sections[0];
		string timeString = string.Empty;
		if (dateTimeStringInRFC3339Sections.Length >= 2)
		{
			timeString = dateTimeStringInRFC3339Sections[1];
		}

		var dateStringSections = dateString.SplitWithOptionalSeparatorsIgnoreCase(
			'-',
			'/',
			'\\',
			' ');
		if (dateStringSections == null
			|| dateStringSections.Count < 3)
		{
			return defaultValue.Value;
		}

		_ = int.TryParse(dateStringSections[0], out var year);
		_ = int.TryParse(dateStringSections[1], out var month);
		_ = int.TryParse(dateStringSections[2], out var day);

		var hour = 0;
		var minute = 0;
		var second = 0;
		var timeZoneOffset = TimeSpan.Zero;
		if (timeString.Length >= 0)
		{
			var timeWithTimeZoneStringSections = timeString.Split('+');
			if (timeWithTimeZoneStringSections.Length > 0)
			{
				var timeStringSections = timeWithTimeZoneStringSections[0].Split(':');
				if (timeStringSections.Length > 0)
				{
					_ = int.TryParse(timeStringSections[0], out hour);
				}
				if (timeStringSections.Length > 1)
				{
					_ = int.TryParse(timeStringSections[1], out minute);
				}
				if (timeStringSections.Length > 2)
				{
					_ = int.TryParse(timeStringSections[2], out second);
				}
			}
			if (timeWithTimeZoneStringSections.Length > 1)
			{
				var timeZoneString = timeWithTimeZoneStringSections[1];
				var timeZoneStringSections = timeZoneString.Split(':');

				var timeOffsetHour = 0;
				if (timeZoneStringSections.Length > 0)
				{
					_ = int.TryParse(timeZoneStringSections[0], out timeOffsetHour);
				}

				var timeOffsetMinute = 0;
				if (timeZoneStringSections.Length > 1)
				{
					_ = int.TryParse(timeZoneStringSections[1], out timeOffsetMinute);
				}

				var timeOffsetSecond = 0;
				var timeOffsetMillisecond = 0;
				if (timeZoneStringSections.Length > 2)
				{
					var timeOffsetSecondString = timeZoneStringSections[2];
					var indexOf_Dot = timeOffsetSecondString.IndexOf('.');
					if (indexOf_Dot < 0)
					{
						_ = int.TryParse(timeOffsetSecondString, out timeOffsetSecond);
					}
					else
					{
						timeOffsetSecondString = timeOffsetSecondString[..indexOf_Dot];
						_ = int.TryParse(timeOffsetSecondString, out timeOffsetSecond);

						var timeOffsetMillisecondString = timeOffsetSecondString[(indexOf_Dot + 1)..];
						_ = int.TryParse(timeOffsetMillisecondString, out timeOffsetMillisecond);
					}
				}

				////////////////////////////////////////////////
				// !!!
				timeZoneOffset = new TimeSpan(
					0,
					timeOffsetHour,
					timeOffsetMinute,
					timeOffsetSecond,
					 timeOffsetMillisecond);
				// !!!
				////////////////////////////////////////////////
			}
		}

		var dateTimeOffset = new DateTimeOffset(
			year,
			month,
			day,
			hour,
			minute,
			second,
			timeZoneOffset);
		{ }
		return dateTimeOffset;
	}

	#endregion
}