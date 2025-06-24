using BaoXia.Utils.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaoXia.Utils.Test;

[TestClass]
public class TimeSpanUtilTest
{
	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	[TestMethod]
	public void FromSecondsTest()
	{
		var testTimeSpan = TimeSpanUtil.FromMilliseconds(double.MaxValue);
		{
			Assert.AreEqual(
				TimeDefinition.Forever.TotalMilliseconds,
				testTimeSpan.TotalMilliseconds);
		}

		testTimeSpan = TimeSpanUtil.FromSeconds(double.MaxValue);
		{
			Assert.AreEqual(
				TimeDefinition.Forever.TotalSeconds,
				testTimeSpan.TotalSeconds);
		}

		testTimeSpan = TimeSpanUtil.FromMinutes(double.MaxValue);
		{
			Assert.AreEqual(
				TimeDefinition.Forever.TotalMinutes,
				testTimeSpan.TotalMinutes);
		}

		testTimeSpan = TimeSpanUtil.FromHours(double.MaxValue);
		{
			Assert.AreEqual(
				TimeDefinition.Forever.TotalHours,
				testTimeSpan.TotalHours);
		}

		testTimeSpan = TimeSpanUtil.FromDays(double.MaxValue);
		{
			Assert.AreEqual(
				TimeDefinition.Forever.TotalDays,
				testTimeSpan.TotalDays);
		}
	}

	#endregion
}