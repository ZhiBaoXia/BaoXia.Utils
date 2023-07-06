using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BaoXia.Utils.Test.ExtensionsTest
{
	[TestClass]
	public class DateTimeExtensionTest
	{
		[TestMethod]
		public void MillisecondsFrom1970()
		{
			var dateTime = DateTime.Now;

			var dateTimeStamp = dateTime.MillisecondsFrom1970();

			var dateTimeFromTimeStamp
				= DateTimeExtension.DateTimeWithMillisecondsFrom1970(dateTimeStamp);

			Assert.IsTrue(dateTime.Year == dateTimeFromTimeStamp.Year);
			Assert.IsTrue(dateTime.Month == dateTimeFromTimeStamp.Month);
			Assert.IsTrue(dateTime.Day == dateTimeFromTimeStamp.Day);
			Assert.IsTrue(dateTime.Hour == dateTimeFromTimeStamp.Hour);
			Assert.IsTrue(dateTime.Minute == dateTimeFromTimeStamp.Minute);
			Assert.IsTrue(dateTime.Second == dateTimeFromTimeStamp.Second);
			Assert.IsTrue(System.Math.Abs(dateTime.Millisecond - dateTimeFromTimeStamp.Millisecond) <= 1);
		}

		[TestMethod]
		public void CompareInDateTimeField()
		{
			DateTime standardTime = DateTime.Now;

			DateTime earlierTime_Year = standardTime.AddYears(-1);
			DateTime laterTime_Year = standardTime.AddYears(1);
			{
				Assert.IsTrue(standardTime.CompareTo(earlierTime_Year, DateTimeExtension.DateTimeField.Year) > 0);
				Assert.IsTrue(standardTime.CompareTo(standardTime, DateTimeExtension.DateTimeField.Year) == 0);
				Assert.IsTrue(standardTime.CompareTo(laterTime_Year, DateTimeExtension.DateTimeField.Year) < 0);
			}


			DateTime earlierTime_Month = standardTime.AddMonths(-1);
			DateTime laterTime_Month = standardTime.AddMonths(1);
			{
				Assert.IsTrue(standardTime.CompareTo(earlierTime_Month, DateTimeExtension.DateTimeField.Month) > 0);
				Assert.IsTrue(standardTime.CompareTo(standardTime, DateTimeExtension.DateTimeField.Month) == 0);
				Assert.IsTrue(standardTime.CompareTo(laterTime_Month, DateTimeExtension.DateTimeField.Month) < 0);
			}


			DateTime earlierTime_Day = standardTime.AddDays(-1);
			DateTime laterTime_Day = standardTime.AddDays(1);
			{
				Assert.IsTrue(standardTime.CompareTo(earlierTime_Day, DateTimeExtension.DateTimeField.Day) > 0);
				Assert.IsTrue(standardTime.CompareTo(standardTime, DateTimeExtension.DateTimeField.Day) == 0);
				Assert.IsTrue(standardTime.CompareTo(laterTime_Day, DateTimeExtension.DateTimeField.Day) < 0);
			}


			DateTime earlierTime_Hour = standardTime.AddHours(-1);
			DateTime laterTime_Hour = standardTime.AddHours(1);
			{
				Assert.IsTrue(standardTime.CompareTo(earlierTime_Hour, DateTimeExtension.DateTimeField.Hour) > 0);
				Assert.IsTrue(standardTime.CompareTo(standardTime, DateTimeExtension.DateTimeField.Hour) == 0);
				Assert.IsTrue(standardTime.CompareTo(laterTime_Hour, DateTimeExtension.DateTimeField.Hour) < 0);
			}


			DateTime earlierTime_Minute = standardTime.AddMinutes(-1);
			DateTime laterTime_Minute = standardTime.AddMinutes(1);
			{
				Assert.IsTrue(standardTime.CompareTo(earlierTime_Minute, DateTimeExtension.DateTimeField.Minute) > 0);
				Assert.IsTrue(standardTime.CompareTo(standardTime, DateTimeExtension.DateTimeField.Minute) == 0);
				Assert.IsTrue(standardTime.CompareTo(laterTime_Minute, DateTimeExtension.DateTimeField.Minute) < 0);
			}


			DateTime earlierTime_Second = standardTime.AddSeconds(-1);
			DateTime laterTime_Second = standardTime.AddSeconds(1);
			{
				Assert.IsTrue(standardTime.CompareTo(earlierTime_Second, DateTimeExtension.DateTimeField.Second) > 0);
				Assert.IsTrue(standardTime.CompareTo(standardTime, DateTimeExtension.DateTimeField.Second) == 0);
				Assert.IsTrue(standardTime.CompareTo(laterTime_Second, DateTimeExtension.DateTimeField.Second) < 0);
			}


			DateTime earlierTime_Millisecond = standardTime.AddMilliseconds(-1);
			DateTime laterTime_Millisecond = standardTime.AddMilliseconds(1);
			{
				Assert.IsTrue(standardTime.CompareTo(earlierTime_Millisecond, DateTimeExtension.DateTimeField.Millisecond) > 0);
				Assert.IsTrue(standardTime.CompareTo(standardTime, DateTimeExtension.DateTimeField.Millisecond) == 0);
				Assert.IsTrue(standardTime.CompareTo(laterTime_Millisecond, DateTimeExtension.DateTimeField.Millisecond) < 0);
			}
		}
	}
}
