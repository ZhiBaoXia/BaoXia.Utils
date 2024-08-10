using BaoXia.Utils.Constants;
using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BaoXia.Utils.Test.ExtensionsTest;

[TestClass]
public class DateTimeExtensionTest
{
	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	[TestMethod]
	public void ToDateTimeInTimeZoneTest()
	{
		var localTimeZoneInfo = TimeZoneInfo.Local;
		var localTimeZoneOffset = localTimeZoneInfo.BaseUtcOffset;

		var dateTimeUnspecified = new DateTime(1970, 1, 2);
		{
			var dateTimeInEast8 = dateTimeUnspecified.ToDateTimeInTimeZone(TimeZoneNumber.East8);
			// !!!
			Assert.IsTrue(
				(dateTimeInEast8 - dateTimeUnspecified).TotalHours
				== (8.0 - localTimeZoneOffset.TotalHours));
			// !!!

			var dateTimeInWest8 = dateTimeUnspecified.ToDateTimeInTimeZone(TimeZoneNumber.West8);
			// !!!
			Assert.IsTrue(
				(dateTimeInWest8 - dateTimeUnspecified).TotalHours
				== (-8.0 - localTimeZoneOffset.TotalHours));
			// !!!
		}

		var dateTimeUtc = DateTime.UtcNow;
		{
			var dateTimeInEast8 = dateTimeUtc.ToDateTimeInTimeZone(TimeZoneNumber.East8);
			// !!!
			Assert.IsTrue(
				(dateTimeInEast8 - dateTimeUtc).TotalHours == 8);
			// !!!

			var dateTimeInWest8 = dateTimeUtc.ToDateTimeInTimeZone(TimeZoneNumber.West8);
			// !!!
			Assert.IsTrue(
				(dateTimeInWest8 - dateTimeUtc).TotalHours == -8);
			// !!!
		}

		var dateTimeNow = DateTime.Now;
		{
			var dateTimeInEast8 = dateTimeNow.ToDateTimeInTimeZone(TimeZoneNumber.East8);
			// !!!
			Assert.IsTrue(
				(dateTimeInEast8 - dateTimeNow).TotalHours
				== (8.0 - localTimeZoneOffset.TotalHours));
			// !!!

			var dateTimeInWest8 = dateTimeNow.ToDateTimeInTimeZone(TimeZoneNumber.West8);
			// !!!
			Assert.IsTrue(
				(dateTimeInWest8 - dateTimeNow).TotalHours
				== (-8.0 - localTimeZoneOffset.TotalHours));
			// !!!
		}
	}

	[TestMethod]
	public void MillisecondsFrom1970Test()
	{
		var zeroLocal = new DateTime();
		{
			Assert.IsTrue(zeroLocal.MillisecondsFrom1970(TimeZoneNumber.Utc0, true) == 0);
		}
		var zeroLocalMillisecondsFrom1970 = zeroLocal.MillisecondsFrom1970(TimeZoneNumber.Utc0, false);
		var zeroLocal2 = DateTimeUtil.DateTimeWithMillisecondsAfter1970(zeroLocalMillisecondsFrom1970);
		{
			Assert.IsTrue(zeroLocal == zeroLocal2);
		}


		var zeroUtc = new DateTime(0, DateTimeKind.Utc);
		{
			Assert.IsTrue(zeroUtc.MillisecondsFrom1970(TimeZoneNumber.Utc0, true) == 0);
		}
		var zeroUtcMillisecondsFrom1970 = zeroLocal.MillisecondsFrom1970(TimeZoneNumber.Utc0, false);
		var zeroUtc2 = DateTimeUtil.DateTimeWithMillisecondsAfter1970(zeroLocalMillisecondsFrom1970);
		{
			Assert.IsTrue(zeroUtc == zeroUtc2);
		}




		var now = new DateTime(2024, 7, 10, 12, 0, 0);
		var millisecondsFrom1970InUtc0 = now.MillisecondsFrom1970(TimeZoneNumber.Utc0, true);
		{
			Assert.IsTrue(millisecondsFrom1970InUtc0 == 1720584000000);
		}
		var millisecondsFrom1970InEast8 = now.MillisecondsFrom1970(TimeZoneNumber.East8, true);
		{
			Assert.IsTrue((millisecondsFrom1970InEast8 - millisecondsFrom1970InUtc0)
				== (1000 * 3600 * 8));
		}
	}

	[TestMethod]
	public void DateTimeWithSecondsAfter1970Test()
	{
		var utc1970_0_0_0_0_0 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		var testHoursCount = 8;
		var testSecondsCount = 3600 * testHoursCount;

		var testTimeInLocal
			= DateTimeUtil
			.DateTimeWithSecondsAfter1970(
			testSecondsCount,
			TimeZoneNumber.Utc0);
		// !!!
		Assert.IsTrue(
			(testTimeInLocal - utc1970_0_0_0_0_0).TotalHours
			==
			TimeZoneInfo.Local.BaseUtcOffset.TotalHours + testHoursCount);
		// !!!
	}


	[TestMethod]
	public void QuickDateTimeTest()
	{
		var dateTime = new DateTime(2023, 09, 10, 16, 50, 30);

		var zeroOfPrevDay = dateTime.ZeroOfPrevDay();
		{
			Assert.IsTrue(zeroOfPrevDay.Equals(new DateTime(2023, 09, 9, 0, 0, 0, 0)));
		}
		var zeroOfThisDay = dateTime.ZeroOfThisDay();
		{
			Assert.IsTrue(zeroOfThisDay.Equals(new DateTime(2023, 09, 10, 0, 0, 0, 0)));
		}
		var zeroOfNextDay = dateTime.ZeroOfNextDay();
		{
			Assert.IsTrue(zeroOfNextDay.Equals(new DateTime(2023, 09, 11, 0, 0, 0, 0)));
		}

		var firstDayOfPrevMonth = dateTime.FirstDayOfPrevMonth();
		{
			Assert.IsTrue(firstDayOfPrevMonth.Equals(new DateTime(2023, 08, 1, 0, 0, 0, 0)));
		}
		var firstDayOfThisMonth = dateTime.FirstDayOfThisMonth();
		{
			Assert.IsTrue(firstDayOfThisMonth.Equals(new DateTime(2023, 09, 1, 0, 0, 0, 0)));
		}
		var firstDayOfNextMonth = dateTime.FirstDayOfNextMonth();
		{
			Assert.IsTrue(firstDayOfNextMonth.Equals(new DateTime(2023, 10, 1, 0, 0, 0, 0)));
		}

		var firstDayOfPrevSession = dateTime.FirstDayOfPrevSession();
		{
			Assert.IsTrue(firstDayOfPrevSession.Equals(new DateTime(2023, 4, 1, 0, 0, 0, 0)));
		}
		var firstDayOfThisSession = dateTime.FirstDayOfThisSession();
		{
			Assert.IsTrue(firstDayOfThisSession.Equals(new DateTime(2023, 7, 1, 0, 0, 0, 0)));
		}
		var firstDayOfNextSession = dateTime.FirstDayOfNextSession();
		{
			Assert.IsTrue(firstDayOfNextSession.Equals(new DateTime(2023, 10, 1, 0, 0, 0, 0)));
		}

		var firstDayOfPrevYear = dateTime.FirstDayOfPrevYear();
		{
			Assert.IsTrue(firstDayOfPrevYear.Equals(new DateTime(2022, 1, 1, 0, 0, 0, 0)));
		}
		var firstDayOfThisYear = dateTime.FirstDayOfThisYear();
		{
			Assert.IsTrue(firstDayOfThisYear.Equals(new DateTime(2023, 1, 1, 0, 0, 0, 0)));
		}
		var firstDayOfNextYear = dateTime.FirstDayOfNextYear();
		{
			Assert.IsTrue(firstDayOfNextYear.Equals(new DateTime(2024, 1, 1, 0, 0, 0, 0)));
		}
	}

	[TestMethod]
	public void CompareInDateTimeFieldTest()
	{
		DateTime standardTime = DateTime.Now;

		DateTime earlierTime_Year = standardTime.AddYears(-1);
		DateTime laterTime_Year = standardTime.AddYears(1);
		{
			Assert.IsTrue(standardTime.CompareTo(earlierTime_Year, DateTimeField.Year) > 0);
			Assert.IsTrue(standardTime.CompareTo(standardTime, DateTimeField.Year) == 0);
			Assert.IsTrue(standardTime.CompareTo(laterTime_Year, DateTimeField.Year) < 0);
		}


		DateTime earlierTime_Month = standardTime.AddMonths(-1);
		DateTime laterTime_Month = standardTime.AddMonths(1);
		{
			Assert.IsTrue(standardTime.CompareTo(earlierTime_Month, DateTimeField.Month) > 0);
			Assert.IsTrue(standardTime.CompareTo(standardTime, DateTimeField.Month) == 0);
			Assert.IsTrue(standardTime.CompareTo(laterTime_Month, DateTimeField.Month) < 0);
		}


		DateTime earlierTime_Day = standardTime.AddDays(-1);
		DateTime laterTime_Day = standardTime.AddDays(1);
		{
			Assert.IsTrue(standardTime.CompareTo(earlierTime_Day, DateTimeField.Day) > 0);
			Assert.IsTrue(standardTime.CompareTo(standardTime, DateTimeField.Day) == 0);
			Assert.IsTrue(standardTime.CompareTo(laterTime_Day, DateTimeField.Day) < 0);
		}


		DateTime earlierTime_Hour = standardTime.AddHours(-1);
		DateTime laterTime_Hour = standardTime.AddHours(1);
		{
			Assert.IsTrue(standardTime.CompareTo(earlierTime_Hour, DateTimeField.Hour) > 0);
			Assert.IsTrue(standardTime.CompareTo(standardTime, DateTimeField.Hour) == 0);
			Assert.IsTrue(standardTime.CompareTo(laterTime_Hour, DateTimeField.Hour) < 0);
		}


		DateTime earlierTime_Minute = standardTime.AddMinutes(-1);
		DateTime laterTime_Minute = standardTime.AddMinutes(1);
		{
			Assert.IsTrue(standardTime.CompareTo(earlierTime_Minute, DateTimeField.Minute) > 0);
			Assert.IsTrue(standardTime.CompareTo(standardTime, DateTimeField.Minute) == 0);
			Assert.IsTrue(standardTime.CompareTo(laterTime_Minute, DateTimeField.Minute) < 0);
		}


		DateTime earlierTime_Second = standardTime.AddSeconds(-1);
		DateTime laterTime_Second = standardTime.AddSeconds(1);
		{
			Assert.IsTrue(standardTime.CompareTo(earlierTime_Second, DateTimeField.Second) > 0);
			Assert.IsTrue(standardTime.CompareTo(standardTime, DateTimeField.Second) == 0);
			Assert.IsTrue(standardTime.CompareTo(laterTime_Second, DateTimeField.Second) < 0);
		}


		DateTime earlierTime_Millisecond = standardTime.AddMilliseconds(-1);
		DateTime laterTime_Millisecond = standardTime.AddMilliseconds(1);
		{
			Assert.IsTrue(standardTime.CompareTo(earlierTime_Millisecond, DateTimeField.Millisecond) > 0);
			Assert.IsTrue(standardTime.CompareTo(standardTime, DateTimeField.Millisecond) == 0);
			Assert.IsTrue(standardTime.CompareTo(laterTime_Millisecond, DateTimeField.Millisecond) < 0);
		}
	}

	[TestMethod]
	public void IsContinuousTest()
	{
		var dateTimes = new DateTime[]
		{
			new(2024, 02, 19),
			new(2024, 02, 20),
			new(2024, 02, 21),
			new(2024, 02, 22),
			new(2024, 02, 23),
			new(2024, 02, 25)
		};

		var dateTimeContinuousCount = 0;
		var lastDateTime = dateTimes[0];
		foreach (var dateTime in dateTimes)
		{
			if (dateTime.IsContinuousAfter(lastDateTime, DateTimeField.Day))
			{
				dateTimeContinuousCount++;
				lastDateTime = dateTime;
			}
		}
		// !!!
		Assert.IsTrue(dateTimeContinuousCount == 5);
		// !!!


		dateTimes =
		[
			new(2024, 02, 25),
			new(2024, 02, 24),
			new(2024, 02, 23),
			new(2024, 02, 22),
			new(2024, 02, 21),
			new(2024, 02, 19)
		];

		dateTimeContinuousCount = 0;
		lastDateTime = dateTimes[0];
		foreach (var dateTime in dateTimes)
		{
			if (dateTime.IsContinuousBefore(lastDateTime, DateTimeField.Day))
			{
				dateTimeContinuousCount++;
				lastDateTime = dateTime;
			}
		}
		// !!!
		Assert.IsTrue(dateTimeContinuousCount == 5);
		// !!!
	}

	#endregion
}
