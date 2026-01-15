using BaoXia.Utils.Constants;
using System;

namespace BaoXia.Utils.Extensions;

public static class TimeSpanExtension
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	[Obsolete("当前函数，已更名，推荐使用“TitleOfListElementDefault”方法替代。")]
	public static string CaptionOfListElementDefault(this TimeSpan timeSpan)
	{
		return TimeSpanExtension.TitleOfListElementDefault(timeSpan);
	}

	[Obsolete("当前函数，已更名，推荐使用“TitleOfDetailPageDefault”方法替代。")]
	public static string CaptionOfDetailPageDefault(this TimeSpan timeSpan)
	{
		return TimeSpanExtension.TitleOfDetailPageDefault(timeSpan);
	}

	public static string TitleOfListElementDefault(this TimeSpan timeSpan)
	{
		string caption;
		if (timeSpan.TotalMinutes < 1)
		{
			caption = $"{timeSpan.TotalSeconds:F0}秒";
		}
		else if (timeSpan.TotalHours < 1)
		{
			if (timeSpan.Seconds > 0)
			{
				caption = $"{timeSpan.Minutes:0}分钟, {timeSpan.Seconds:F0}秒";
			}
			else
			{
				caption = $"{timeSpan.Minutes:0}分钟";
			}
		}
		else if (timeSpan.TotalDays < 1)
		{
			if (timeSpan.Minutes > 0)
			{
				caption = $"{timeSpan.Hours:0}小时, {timeSpan.Minutes:0}分钟";
			}
			else
			{
				caption = $"{timeSpan.Hours:0}小时";
			}
		}
		else if (timeSpan.TotalDays < TimeDefinition.Forever.TotalDays)
		{
			if (timeSpan.Hours > 0)
			{
				caption = $"{timeSpan.Days:0}天, {timeSpan.Hours:0}小时";
			}
			else
			{
				caption = $"{timeSpan.Days:0}天";
			}
		}
		else
		{
			caption = "永久";
		}
		return caption;
	}

	public static string TitleOfListElementDefault(this double totalSeconds)
	{
		return TimeSpanExtension.TitleOfListElementDefault(TimeSpanUtil.FromSeconds(totalSeconds));
	}

	public static string TitleOfDetailPageDefault(this TimeSpan timeSpan)
	{
		string caption;
		if (timeSpan.TotalMinutes < 1)
		{
			caption = $"{timeSpan.TotalSeconds:F0}秒";
		}
		else if (timeSpan.TotalHours < 1)
		{
			if (timeSpan.Seconds > 0)
			{
				caption = $"{timeSpan.Minutes:0}分钟, {timeSpan.Seconds:0}秒";
			}
			else
			{
				caption = $"{timeSpan.Minutes:0}分钟";
			}
		}
		else if (timeSpan.TotalDays < 1)
		{
			if (timeSpan.Seconds > 0)
			{
				caption = $"{timeSpan.Hours:0}小时, {timeSpan.Minutes:0}分钟, {timeSpan.Seconds:0}秒";
			}
			else if (timeSpan.Minutes > 0)
			{
				caption = $"{timeSpan.Hours:0}小时, {timeSpan.Minutes:0}分钟";
			}
			else
			{
				caption = $"{timeSpan.Hours:0}小时";
			}
		}
		else if (timeSpan.TotalDays < TimeDefinition.Forever.TotalDays)
		{
			if (timeSpan.Seconds > 0)
			{
				caption = $"{timeSpan.Days:0}天, {timeSpan.Hours:0}小时, {timeSpan.Minutes:0}分钟, {timeSpan.Seconds:0}秒";
			}
			else if (timeSpan.Minutes > 0)
			{
				caption = $"{timeSpan.Days:0}天, {timeSpan.Hours:0}小时, {timeSpan.Minutes:0}分钟";
			}
			else if (timeSpan.Hours > 0)
			{
				caption = $"{timeSpan.Days:0}天, {timeSpan.Hours:0}小时";
			}
			else
			{
				caption = $"{timeSpan.Days:0}天";
			}
		}
		else
		{
			caption = "永久";
		}
		return caption;
	}

	public static string TitleOfDetailPageDefault(this double totalSeconds)
	{
		return TimeSpanExtension.TitleOfDetailPageDefault(TimeSpanUtil.FromSeconds(totalSeconds));
	}

	#endregion
}