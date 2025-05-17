using BaoXia.Utils.Constants;
using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaoXia.Utils.Test;

[TestClass]
public class DateTimeOffsetUtilTest
{
	[TestMethod]
	public void DateTimeOffsetFromStringInRFC3339Test()
	{
		var dateTimeStringInRFC3339 = "2021-09-01T01:02:03+08:00";
		var dateTimeOffset = DateTimeOffsetUtil.DateTimeOffsetFromStringInRFC3339(dateTimeStringInRFC3339);
		{
			Assert.AreEqual(2021, dateTimeOffset.Year);
			Assert.AreEqual(9, dateTimeOffset.Month);
			Assert.AreEqual(1, dateTimeOffset.Day);
			Assert.AreEqual(1, dateTimeOffset.Hour);
			Assert.AreEqual(2, dateTimeOffset.Minute);
			Assert.AreEqual(3, dateTimeOffset.Second);
			Assert.AreEqual(8, dateTimeOffset.Offset.Hours);
		}

		dateTimeStringInRFC3339 = "2021/09/01T01:02:03+08:00";
		dateTimeOffset = DateTimeOffsetUtil.DateTimeOffsetFromStringInRFC3339(dateTimeStringInRFC3339);
		{
			Assert.AreEqual(2021, dateTimeOffset.Year);
			Assert.AreEqual(9, dateTimeOffset.Month);
			Assert.AreEqual(1, dateTimeOffset.Day);
			Assert.AreEqual(1, dateTimeOffset.Hour);
			Assert.AreEqual(2, dateTimeOffset.Minute);
			Assert.AreEqual(3, dateTimeOffset.Second);
			Assert.AreEqual(8, dateTimeOffset.Offset.Hours);
		}


		dateTimeStringInRFC3339 = "2021/09/01T01:02:03";
		dateTimeOffset = DateTimeOffsetUtil.DateTimeOffsetFromStringInRFC3339(dateTimeStringInRFC3339);
		{
			Assert.AreEqual(2021, dateTimeOffset.Year);
			Assert.AreEqual(9, dateTimeOffset.Month);
			Assert.AreEqual(1, dateTimeOffset.Day);
			Assert.AreEqual(1, dateTimeOffset.Hour);
			Assert.AreEqual(2, dateTimeOffset.Minute);
			Assert.AreEqual(3, dateTimeOffset.Second);
			//
			Assert.AreEqual(0, dateTimeOffset.Offset.Hours);
			//
		}

		dateTimeOffset = dateTimeOffset.ToDateTimeOffsetByConvertToTimeZone(TimeZoneNumber.East8);
		{
			Assert.AreEqual(2021, dateTimeOffset.Year);
			Assert.AreEqual(9, dateTimeOffset.Month);
			Assert.AreEqual(1, dateTimeOffset.Day);
			Assert.AreEqual(9, dateTimeOffset.Hour);
			Assert.AreEqual(2, dateTimeOffset.Minute);
			Assert.AreEqual(3, dateTimeOffset.Second);
			//
			Assert.AreEqual(8, dateTimeOffset.Offset.Hours);
			//
		}
	}
}