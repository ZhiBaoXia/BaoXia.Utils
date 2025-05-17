using BaoXia.Utils.Constants;
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
			Assert.AreEqual(
-8.0, (dateTimeOffsetInEast8 - dateTimeOffsetStandard).TotalHours
);
		}

		var dateTimeOffsetInWest8 = dateTimeOffsetStandard.ToDateTimeOffsetBySetTimeZoneNumber(
			TimeZoneNumber.West8);
		{
			Assert.AreEqual(
8.0, (dateTimeOffsetInWest8 - dateTimeOffsetStandard).TotalHours
);
		}
	}

	[TestMethod]
	public void MillisecondsFrom1970Test()
	{
		var dateTimeDefaultWithOffsetZero = new DateTimeOffset();
		{
			Assert.AreEqual(0, dateTimeDefaultWithOffsetZero.MillisecondsFrom1970(TimeZoneNumber.Utc0, true));
		}
		var dateTimeDefaultAtLocal_MillisecondsFrom1970
			= dateTimeDefaultWithOffsetZero.MillisecondsFrom1970(TimeZoneNumber.Utc0, false);
		var dateTimeFromMillisecondsFrom1970
			= DateTimeOffsetUtil.DateTimeOffsetWithMillisecondsAfter1970(
				dateTimeDefaultAtLocal_MillisecondsFrom1970);
		{
			Assert.AreEqual(dateTimeDefaultWithOffsetZero, dateTimeFromMillisecondsFrom1970);
		}


		var dateTimeDefaultAtEast8 = new DateTimeOffset(TimeSpan.TicksPerHour * 8, new(8, 0, 0));
		{
			Assert.AreEqual(0, dateTimeDefaultAtEast8.MillisecondsFrom1970(TimeZoneNumber.Utc0, true));
		}
		var dateTimeDefaultAtEast8_MillisecondsFrom1970 = dateTimeDefaultAtEast8.MillisecondsFrom1970(TimeZoneNumber.Utc0, false);
		var dateTimeAtLocalFromMillisecondsFrom1970 = DateTimeOffsetUtil.DateTimeOffsetWithMillisecondsAfter1970(dateTimeDefaultAtEast8_MillisecondsFrom1970);
		{
			Assert.AreEqual(dateTimeDefaultAtEast8, dateTimeAtLocalFromMillisecondsFrom1970);
		}


		////////////////////////////////////////////////
		////////////////////////////////////////////////


		var dateTimeOffsetA = new DateTimeOffset(2024, 7, 10, 12, 0, 0, new TimeSpan(0, 0, 0));
		var dateTimeOffsetB = new DateTimeOffset(2024, 7, 10, 12, 0, 0, new TimeSpan(8, 0, 0));
		{
			Assert.AreEqual(dateTimeOffsetB.Ticks, dateTimeOffsetA.Ticks);
		}


		var now = new DateTimeOffset(2024, 7, 10, 12, 0, 0, new TimeSpan((int)TimeZoneNumber.East8, 0, 0));
		var millisecondsFrom1970InUtc0 = now.MillisecondsFrom1970(TimeZoneNumber.Utc0, true);
		{
			Assert.AreEqual(1720584000000, millisecondsFrom1970InUtc0);
		}
		var millisecondsFrom1970InEast8 = now.MillisecondsFrom1970(TimeZoneNumber.East8, true);
		{
			Assert.AreEqual(1000 * 3600 * (int)TimeZoneNumber.East8, millisecondsFrom1970InEast8 - millisecondsFrom1970InUtc0);
		}
		var now2 = DateTimeOffsetUtil.DateTimeOffsetWithMillisecondsAfter1970(
			millisecondsFrom1970InEast8,
			TimeZoneNumber.East8);
		{
			Assert.AreEqual(now, now2);
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
		Assert.AreEqual(
testHoursCount, (testTimeInLocal - utc1970_0_0_0_0_0).TotalHours
);
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
			Assert.AreEqual(2021, dateTimeOffsetFromStringInRFC3339.Year);
			Assert.AreEqual(9, dateTimeOffsetFromStringInRFC3339.Month);
			Assert.AreEqual(1, dateTimeOffsetFromStringInRFC3339.Day);
			Assert.AreEqual(1, dateTimeOffsetFromStringInRFC3339.Hour);
			Assert.AreEqual(2, dateTimeOffsetFromStringInRFC3339.Minute);
			Assert.AreEqual(3, dateTimeOffsetFromStringInRFC3339.Second);
			Assert.AreEqual(4, dateTimeOffsetFromStringInRFC3339.Offset.TotalHours);
		}
	}

	#endregion
}