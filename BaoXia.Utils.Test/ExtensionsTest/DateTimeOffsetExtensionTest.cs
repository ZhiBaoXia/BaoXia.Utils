﻿using BaoXia.Utils.Constants;
using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BaoXia.Utils.Test.ExtensionsTest;

[TestClass]
public class DateTimeOffsetExtensionTest
{
	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	[TestMethod]
	public void ToDateTimeOffsetByConvertToTimeZoneTest()
	{
		var dateTimeOffsetStandard = new DateTimeOffset(1970, 1, 2, 0, 0, 0, new TimeSpan(0, 0, 0));

		var dateTimeOffsetInEast8 = dateTimeOffsetStandard.ToDateTimeOffsetBySetTimeZoneNumber(
			TimeZoneNumber.East8);
		{
			Assert.IsTrue(
				(dateTimeOffsetInEast8 - dateTimeOffsetStandard).TotalHours
				== -8.0);
		}

		var dateTimeOffsetInWest8 = dateTimeOffsetStandard.ToDateTimeOffsetBySetTimeZoneNumber(
			TimeZoneNumber.West8);
		{
			Assert.IsTrue(
				(dateTimeOffsetInWest8 - dateTimeOffsetStandard).TotalHours
				== 8.0);
		}
	}

	[TestMethod]
	public void MillisecondsFrom1970Test()
	{
		var dateTimeDefaultWithOffsetZero = new DateTimeOffset();
		{
			Assert.IsTrue(dateTimeDefaultWithOffsetZero.MillisecondsFrom1970(TimeZoneNumber.Utc0, true) == 0);
		}
		var dateTimeDefaultAtLocal_MillisecondsFrom1970
			= dateTimeDefaultWithOffsetZero.MillisecondsFrom1970(TimeZoneNumber.Utc0, false);
		var dateTimeFromMillisecondsFrom1970
			= DateTimeOffsetUtil.DateTimeOffsetWithMillisecondsAfter1970(
				dateTimeDefaultAtLocal_MillisecondsFrom1970);
		{
			Assert.IsTrue(dateTimeFromMillisecondsFrom1970 == dateTimeDefaultWithOffsetZero);
		}


		var dateTimeDefaultAtEast8 = new DateTimeOffset(TimeSpan.TicksPerHour * 8, new(8, 0, 0));
		{
			Assert.IsTrue(dateTimeDefaultAtEast8.MillisecondsFrom1970(TimeZoneNumber.Utc0, true) == 0);
		}
		var dateTimeDefaultAtEast8_MillisecondsFrom1970 = dateTimeDefaultAtEast8.MillisecondsFrom1970(TimeZoneNumber.Utc0, false);
		var dateTimeAtLocalFromMillisecondsFrom1970 = DateTimeOffsetUtil.DateTimeOffsetWithMillisecondsAfter1970(dateTimeDefaultAtEast8_MillisecondsFrom1970);
		{
			Assert.IsTrue(dateTimeAtLocalFromMillisecondsFrom1970 == dateTimeDefaultAtEast8);
		}


		////////////////////////////////////////////////
		////////////////////////////////////////////////


		var dateTimeOffsetA = new DateTimeOffset(2024, 7, 10, 12, 0, 0, new TimeSpan(0, 0, 0));
		var dateTimeOffsetB = new DateTimeOffset(2024, 7, 10, 12, 0, 0, new TimeSpan(8, 0, 0));
		{
			Assert.IsTrue(dateTimeOffsetA.Ticks == dateTimeOffsetB.Ticks);
		}


		var now = new DateTimeOffset(2024, 7, 10, 12, 0, 0, new TimeSpan((int)TimeZoneNumber.East8, 0, 0));
		var millisecondsFrom1970InUtc0 = now.MillisecondsFrom1970(TimeZoneNumber.Utc0, true);
		{
			Assert.IsTrue(millisecondsFrom1970InUtc0 == 1720584000000);
		}
		var millisecondsFrom1970InEast8 = now.MillisecondsFrom1970(TimeZoneNumber.East8, true);
		{
			Assert.IsTrue((millisecondsFrom1970InEast8 - millisecondsFrom1970InUtc0)
				== (1000 * 3600 * (int)TimeZoneNumber.East8));
		}
		var now2 = DateTimeOffsetUtil.DateTimeOffsetWithMillisecondsAfter1970(
			millisecondsFrom1970InEast8,
			TimeZoneNumber.East8);
		{
			Assert.IsTrue(now2 == now);
		}
	}

	[TestMethod]
	public void DateTimeOffsetWithSecondsAfter1970Test()
	{
		var testHoursCount = 8;
		var testSecondsCount = 3600 * testHoursCount;
		var testTimeInLocal
			= DateTimeOffsetUtil
			.DateTimeOffsetWithSecondsAfter1970(
			testSecondsCount,
			TimeZoneNumber.Utc0);
		var utc1970_0_0_0_0_0
			= new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
		// !!!
		Assert.IsTrue(
			(testTimeInLocal - utc1970_0_0_0_0_0).TotalHours
			== testHoursCount);
		// !!!
	}

	[TestMethod]
	public void ToStringInRFC3339Test()
	{
		var dateTimeStringInRFC3339
			= "2021-09-01T01:02:03+04:00";
		var dateTimeOffsetFromStringInRFC3339
			= DateTimeOffsetUtil.DateTimeOffsetFromStringInRFC3339(dateTimeStringInRFC3339);
		var dateTimeStringInRFC3339FromDateTimeOffset
			= dateTimeOffsetFromStringInRFC3339.ToStringInRFC3339();
		{
			Assert.IsTrue(dateTimeStringInRFC3339.Equals(dateTimeStringInRFC3339FromDateTimeOffset));
			Assert.IsTrue(dateTimeOffsetFromStringInRFC3339.Year == 2021);
			Assert.IsTrue(dateTimeOffsetFromStringInRFC3339.Month == 9);
			Assert.IsTrue(dateTimeOffsetFromStringInRFC3339.Day == 1);
			Assert.IsTrue(dateTimeOffsetFromStringInRFC3339.Hour == 1);
			Assert.IsTrue(dateTimeOffsetFromStringInRFC3339.Minute == 2);
			Assert.IsTrue(dateTimeOffsetFromStringInRFC3339.Second == 3);
			Assert.IsTrue(dateTimeOffsetFromStringInRFC3339.Offset.TotalHours == 4);
		}
	}

	#endregion
}