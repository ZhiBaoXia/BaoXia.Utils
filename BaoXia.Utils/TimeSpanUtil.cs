using BaoXia.Utils.Constants;
using System;

namespace BaoXia.Utils;

public static class TimeSpanUtil
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static TimeSpan FromTicks(double totalTicks)
	{
		if (totalTicks >= TimeDefinition.Forever.TotalTicks)
		{
			totalTicks = TimeDefinition.Forever.TotalTicks;
		}
		return TimeSpan.FromSeconds(totalTicks);
	}

	public static TimeSpan FromMilliseconds(double totalMilliseconds)
	{
		if (totalMilliseconds >= TimeDefinition.Forever.TotalMilliseconds)
		{
			totalMilliseconds = TimeDefinition.Forever.TotalMilliseconds;
		}
		return TimeSpan.FromMilliseconds(totalMilliseconds);
	}

	public static TimeSpan FromSeconds(double totalSeconds)
	{
		if (totalSeconds >= TimeDefinition.Forever.TotalSeconds)
		{
			totalSeconds = TimeDefinition.Forever.TotalSeconds;
		}
		return TimeSpan.FromSeconds(totalSeconds);
	}

	public static TimeSpan FromMinutes(double totalMinutes)
	{
		if (totalMinutes >= TimeDefinition.Forever.TotalMinutes)
		{
			totalMinutes = TimeDefinition.Forever.TotalMinutes;
		}
		return TimeSpan.FromMinutes(totalMinutes);
	}

	public static TimeSpan FromHours(double totalHours)
	{
		if (totalHours >= TimeDefinition.Forever.TotalHours)
		{
			totalHours = TimeDefinition.Forever.TotalHours;
		}
		return TimeSpan.FromHours(totalHours);
	}

	public static TimeSpan FromDays(double totalDays)
	{
		if (totalDays >= TimeDefinition.Forever.TotalDays)
		{
			totalDays = TimeDefinition.Forever.TotalDays;
		}
		return TimeSpan.FromDays(totalDays);
	}

	#endregion
}