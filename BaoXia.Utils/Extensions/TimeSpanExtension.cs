using BaoXia.Utils.Constants;
using System;

namespace BaoXia.Utils.Extensions;

public static class TimeSpanExtension
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

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
		string title;
		if (timeSpan.TotalMinutes < 1)
		{
			title = $"{timeSpan.TotalSeconds:F0}秒";
		}
		else if (timeSpan.TotalHours < 1)
		{
			if (timeSpan.Seconds > 0)
			{
				title = $"{timeSpan.Minutes:0}分钟, {timeSpan.Seconds:0}秒";
			}
			else
			{
				title = $"{timeSpan.Minutes:0}分钟";
			}
		}
		else if (timeSpan.TotalDays < 1)
		{
			if (timeSpan.Seconds > 0)
			{
				title = $"{timeSpan.Hours:0}小时, {timeSpan.Minutes:0}分钟, {timeSpan.Seconds:0}秒";
			}
			else if (timeSpan.Minutes > 0)
			{
				title = $"{timeSpan.Hours:0}小时, {timeSpan.Minutes:0}分钟";
			}
			else
			{
				title = $"{timeSpan.Hours:0}小时";
			}
		}
		else if (timeSpan.TotalDays < TimeDefinition.Forever.TotalDays)
		{
			if (timeSpan.Seconds > 0)
			{
				title = $"{timeSpan.Days:0}天, {timeSpan.Hours:0}小时, {timeSpan.Minutes:0}分钟, {timeSpan.Seconds:0}秒";
			}
			else if (timeSpan.Minutes > 0)
			{
				title = $"{timeSpan.Days:0}天, {timeSpan.Hours:0}小时, {timeSpan.Minutes:0}分钟";
			}
			else if (timeSpan.Hours > 0)
			{
				title = $"{timeSpan.Days:0}天, {timeSpan.Hours:0}小时";
			}
			else
			{
				title = $"{timeSpan.Days:0}天";
			}
		}
		else
		{
			title = "永久";
		}
		return title;
	}

	public static string TitleOfDetailPageDefault(this double totalSeconds)
	{
		return TimeSpanExtension.TitleOfDetailPageDefault(TimeSpanUtil.FromSeconds(totalSeconds));
	}

	public static string TitleOfRemainingTime(this TimeSpan remainingTime)
	{
		string remainingTimeTitle;
		if (remainingTime.TotalMinutes < 1)
		{
			remainingTimeTitle = $"{remainingTime.TotalMinutes:0}秒";
		}
		else if (remainingTime.TotalHours < 1)
		{
			remainingTimeTitle = $"{remainingTime.TotalMinutes:0}分钟";
		}
		else if (remainingTime.TotalDays < 1)
		{
			remainingTimeTitle = $"{remainingTime.TotalHours:0}小时";
		}
		else
		{
			remainingTimeTitle = $"{remainingTime.TotalHours:1}天";
		}
		return remainingTimeTitle;
	}

	#endregion
}