using System;

namespace BaoXia.Utils.Constants;

public class TimeDefinition
{
	////////////////////////////////////////////////
	// @静态常量
	////////////////////////////////////////////////

	#region 静态常量

	public class Forever
	{
		public const double TotalYears = 100;

		public const double TotalMonths = 12 * TotalYears;

		public const double TotalDays = 365 * TotalYears;

		public const double TotalHours = 24 * TotalDays;

		public const double TotalMinutes = 60 * TotalHours;

		public const double TotalSeconds = 60 * TotalMinutes;

		public const double TotalMilliseconds = 1000 * TotalSeconds;

		public const long TotalTicks = (long)(TimeSpan.TicksPerMillisecond * TotalMilliseconds);
	}

	#endregion
}
