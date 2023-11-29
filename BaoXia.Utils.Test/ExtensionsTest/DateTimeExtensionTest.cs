using BaoXia.Utils.Constants;
using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BaoXia.Utils.Test.ExtensionsTest
{
	[TestClass]
	public class DateTimeExtensionTest
	{
		[TestMethod]
		public void MillisecondsFrom1970Test()
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
	}
}
