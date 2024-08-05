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
			Assert.IsTrue(dateTimeOffset.Year == 2021);
			Assert.IsTrue(dateTimeOffset.Month == 9);
			Assert.IsTrue(dateTimeOffset.Day == 1);
			Assert.IsTrue(dateTimeOffset.Hour == 1);
			Assert.IsTrue(dateTimeOffset.Minute == 2);
			Assert.IsTrue(dateTimeOffset.Second == 3);
			Assert.IsTrue(dateTimeOffset.Offset.Hours == 8);
		}

		dateTimeStringInRFC3339 = "2021/09/01T01:02:03+08:00";
		dateTimeOffset = DateTimeOffsetUtil.DateTimeOffsetFromStringInRFC3339(dateTimeStringInRFC3339);
		{
			Assert.IsTrue(dateTimeOffset.Year == 2021);
			Assert.IsTrue(dateTimeOffset.Month == 9);
			Assert.IsTrue(dateTimeOffset.Day == 1);
			Assert.IsTrue(dateTimeOffset.Hour == 1);
			Assert.IsTrue(dateTimeOffset.Minute == 2);
			Assert.IsTrue(dateTimeOffset.Second == 3);
			Assert.IsTrue(dateTimeOffset.Offset.Hours == 8);
		}


		dateTimeStringInRFC3339 = "2021/09/01T01:02:03";
		dateTimeOffset = DateTimeOffsetUtil.DateTimeOffsetFromStringInRFC3339(dateTimeStringInRFC3339);
		{
			Assert.IsTrue(dateTimeOffset.Year == 2021);
			Assert.IsTrue(dateTimeOffset.Month == 9);
			Assert.IsTrue(dateTimeOffset.Day == 1);
			Assert.IsTrue(dateTimeOffset.Hour == 1);
			Assert.IsTrue(dateTimeOffset.Minute == 2);
			Assert.IsTrue(dateTimeOffset.Second == 3);
			//
			Assert.IsTrue(dateTimeOffset.Offset.Hours == 0);
			//
		}

		dateTimeOffset = dateTimeOffset.ToDateTimeOffsetByConvertToTimeZone(TimeZoneNumber.East8);
		{
			Assert.IsTrue(dateTimeOffset.Year == 2021);
			Assert.IsTrue(dateTimeOffset.Month == 9);
			Assert.IsTrue(dateTimeOffset.Day == 1);
			Assert.IsTrue(dateTimeOffset.Hour == 9);
			Assert.IsTrue(dateTimeOffset.Minute == 2);
			Assert.IsTrue(dateTimeOffset.Second == 3);
			//
			Assert.IsTrue(dateTimeOffset.Offset.Hours == 8);
			//
		}
	}
}