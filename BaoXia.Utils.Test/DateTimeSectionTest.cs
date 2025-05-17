using BaoXia.Utils.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BaoXia.Utils.Test;

[TestClass]
public class DateTimeSectionTest
{
	[TestMethod]
	public void IsTimeInSectionWith_NotLoop_AscTimeSection_Test()
	{
		var timeSection = new DateTimeSection(
			TimeSectionType.NotLoop,
			new DateTime(2023, 10, 18, 20, 0, 0),
			new DateTime(2023, 10, 18, 22, 0, 0));
		var testCases = new (DateTime Time, bool TestResult)[]
		{
			(Time: new DateTime(2023, 10, 17, 21, 0, 0), TestResult: false),
			(Time: new DateTime(2023, 10, 18, 12, 0, 0), TestResult: false),
			(Time: new DateTime(2023, 10, 18, 20, 0, 0), TestResult: true),
			(Time: new DateTime(2023, 10, 18, 21, 0, 0), TestResult: true),
			(Time: new DateTime(2023, 10, 18, 22, 0, 0), TestResult: false),
			(Time: new DateTime(2023, 10, 18, 23, 0, 0), TestResult: false),
			(Time: new DateTime(2023, 10, 19, 12, 0, 0), TestResult: false),
			(Time: new DateTime(2023, 10, 19, 21, 0, 0), TestResult: false)
		};
		foreach (var testCase in testCases)
		{
			Assert.AreEqual(
testCase.TestResult, timeSection.IsTimeInSection(testCase.Time)
);
		}
	}

	[TestMethod]
	public void IsTimeInSectionWith_NotLoop_DescTimeSection_Test()
	{
		var timeSection = new DateTimeSection(
			TimeSectionType.NotLoop,
			new DateTime(2023, 10, 18, 22, 0, 0),
			new DateTime(2023, 10, 18, 20, 0, 0));
		var testCases = new (DateTime Time, bool TestResult)[]
		{
			(Time: new DateTime(2023, 10, 17, 21, 0, 0), TestResult: false),
			(Time: new DateTime(2023, 10, 18, 12, 0, 0), TestResult: false),
			(Time: new DateTime(2023, 10, 18, 20, 0, 0), TestResult: false),
			(Time: new DateTime(2023, 10, 18, 21, 0, 0), TestResult: false),
			(Time: new DateTime(2023, 10, 18, 22, 0, 0), TestResult: false),
			(Time: new DateTime(2023, 10, 18, 23, 0, 0), TestResult: false),
			(Time: new DateTime(2023, 10, 19, 12, 0, 0), TestResult: false),
			(Time: new DateTime(2023, 10, 19, 21, 0, 0), TestResult: false)
		};
		foreach (var testCase in testCases)
		{
			Assert.AreEqual(
testCase.TestResult, timeSection.IsTimeInSection(testCase.Time)
);
		}
	}

	[TestMethod]
	public void IsTimeInSectionWith_Year_AscTimeSection_Test()
	{
		var timeSection = new DateTimeSection(
			TimeSectionType.LoopInYear,
			new DateTime(2023, 5, 1, 0, 0, 0),
			new DateTime(2023, 6, 1, 0, 0, 0));
		var testCases = new (DateTime Time, bool TestResult)[]
		{
			(Time: new DateTime(2023, 1, 15, 0, 0, 0), TestResult: false),
			(Time: new DateTime(2023, 4, 15, 0, 0, 0), TestResult: false),
			(Time: new DateTime(2023, 5, 1, 0, 0, 0), TestResult: true),
			(Time: new DateTime(2023, 5, 15, 0, 0, 0), TestResult: true),
			(Time: new DateTime(2023, 6, 1, 0, 0, 0), TestResult: false),
			(Time: new DateTime(2023, 6, 15, 0, 0, 0), TestResult: false),
			(Time: new DateTime(2023, 12, 15, 0, 0, 0), TestResult: false),
		};
		foreach (var testCase in testCases)
		{
			Assert.AreEqual(
testCase.TestResult, timeSection.IsTimeInSection(testCase.Time)
);
		}
	}

	[TestMethod]
	public void IsTimeInSectionWith_Year_DescTimeSection_Test()
	{
		var timeSection = new DateTimeSection(
			TimeSectionType.LoopInYear,
			new DateTime(2023, 6, 1, 0, 0, 0),
			new DateTime(2023, 5, 1, 0, 0, 0));
		var testCases = new (DateTime Time, bool TestResult)[]
		{
			(Time: new DateTime(2023, 1, 15, 0, 0, 0), TestResult: true),
			(Time: new DateTime(2023, 4, 15, 0, 0, 0), TestResult: true),
			(Time: new DateTime(2023, 5, 1, 0, 0, 0), TestResult: false),
			(Time: new DateTime(2023, 5, 15, 0, 0, 0), TestResult: false),
			(Time: new DateTime(2023, 6, 1, 0, 0, 0), TestResult: true),
			(Time: new DateTime(2023, 6, 15, 0, 0, 0), TestResult: true),
			(Time: new DateTime(2023, 12, 15, 0, 0, 0), TestResult: true),
		};
		foreach (var testCase in testCases)
		{
			Assert.AreEqual(
testCase.TestResult, timeSection.IsTimeInSection(testCase.Time)
);
		}
	}

	[TestMethod]
	public void IsTimeInSectionWith_Day_AscTimeSection_Test()
	{
		// 每天的，04:30 - 23:30
		var timeSections = new DateTimeSection[]
			{
				new(
					TimeSectionType.LoopInDay,
					new DateTime(2023, 10, 18, 4, 30, 0),
					new DateTime(2023, 10, 19, 23, 30, 0)),
				new(
					TimeSectionType.LoopInDay,
					new DateTime(2023, 10, 19, 4, 30, 0),
					new DateTime(2023, 10, 18, 23, 30, 0))
			};

		var testCases = new (DateTime Time, bool TestResult)[]
		{
			(Time: new DateTime(2023, 10, 17, 20, 0, 0), TestResult: true),
			(Time: new DateTime(2023, 10, 17, 23, 29, 59), TestResult: true),
			(Time: new DateTime(2023, 10, 17, 23, 30, 0), TestResult: false),
			(Time: new DateTime(2023, 10, 18, 1, 0, 0), TestResult: false),
			(Time: new DateTime(2023, 10, 18, 4, 30, 0), TestResult: true),
			(Time: new DateTime(2023, 10, 18, 4, 30, 1), TestResult: true),

			(Time: new DateTime(2023, 10, 18, 20, 0, 0), TestResult: true),
			(Time: new DateTime(2023, 10, 18, 23, 29, 59), TestResult: true),
			(Time: new DateTime(2023, 10, 18, 23, 30, 0), TestResult: false),
			(Time: new DateTime(2023, 10, 19, 1, 0, 0), TestResult: false),
			(Time: new DateTime(2023, 10, 19, 4, 30, 0), TestResult: true),
			(Time: new DateTime(2023, 10, 19, 4, 30, 1), TestResult: true),

			(Time: new DateTime(2023, 10, 19, 20, 0, 0), TestResult: true),
			(Time: new DateTime(2023, 10, 19, 23, 29, 59), TestResult: true),
			(Time: new DateTime(2023, 10, 19, 23, 30, 0), TestResult: false),
			(Time: new DateTime(2023, 10, 20, 1, 0, 0), TestResult: false),
			(Time: new DateTime(2023, 10, 20, 4, 30, 0), TestResult: true),
			(Time: new DateTime(2023, 10, 20, 4, 30, 1), TestResult: true)
		};

		foreach (var timeSection in timeSections)
		{
			foreach (var testCase in testCases)
			{
				Assert.AreEqual(
testCase.TestResult, timeSection.IsTimeInSection(testCase.Time)
);
			}
		}
	}


	[TestMethod]
	public void IsTimeInSectionWith_Day_DescTimeSection_Test()
	{
		// 每日 23:30 - 04:30 
		var timeSections = new DateTimeSection[]
			{
				new (
					TimeSectionType.LoopInDay,
					new DateTime(2023, 10, 18, 23, 30, 0),
					new DateTime(2023, 10, 19, 4, 30, 0)),
				new(
					TimeSectionType.LoopInDay,
					new DateTime(2023, 10, 19, 23, 30, 0),
					new DateTime(2023, 10, 18, 4, 30, 0))
			};
		var testCases = new (DateTime Time, bool TestResult)[]
		{
			(Time: new DateTime(2023, 10, 17, 20, 0, 0), TestResult: false),
			(Time: new DateTime(2023, 10, 17, 23, 29, 59), TestResult: false),
			(Time: new DateTime(2023, 10, 17, 23, 30, 0), TestResult: true),
			(Time: new DateTime(2023, 10, 18, 1, 0, 0), TestResult: true),
			(Time: new DateTime(2023, 10, 18, 4, 30, 0), TestResult: false),
			(Time: new DateTime(2023, 10, 18, 4, 30, 1), TestResult: false),

			(Time: new DateTime(2023, 10, 18, 20, 0, 0), TestResult: false),
			(Time: new DateTime(2023, 10, 18, 23, 29, 59), TestResult: false),
			(Time: new DateTime(2023, 10, 18, 23, 30, 0), TestResult: true),
			(Time: new DateTime(2023, 10, 19, 1, 0, 0), TestResult: true),
			(Time: new DateTime(2023, 10, 19, 4, 30, 0), TestResult: false),
			(Time: new DateTime(2023, 10, 19, 4, 30, 1), TestResult: false),

			(Time: new DateTime(2023, 10, 19, 20, 0, 0), TestResult: false),
			(Time: new DateTime(2023, 10, 19, 23, 29, 59), TestResult: false),
			(Time: new DateTime(2023, 10, 19, 23, 30, 0), TestResult: true),
			(Time: new DateTime(2023, 10, 20, 1, 0, 0), TestResult: true),
			(Time: new DateTime(2023, 10, 20, 4, 30, 0), TestResult: false),
			(Time: new DateTime(2023, 10, 20, 4, 30, 1), TestResult: false)
		};
		foreach (var timeSection in timeSections)
		{
			foreach (var testCase in testCases)
			{
				Assert.AreEqual(
testCase.TestResult, timeSection.IsTimeInSection(testCase.Time)
);
			}
		}
	}


	public void Test()
	{
		IsTimeInSectionWith_NotLoop_AscTimeSection_Test();
		IsTimeInSectionWith_NotLoop_DescTimeSection_Test();
	}
}
