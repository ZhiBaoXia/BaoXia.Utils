using BaoXia.Utils.TestUtils.MethodTestUtils.Constants;
using System;

namespace BaoXia.Utils.TestUtils.MethodTestUtils;
public class TestUnitInfo
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public string? Name { get; set; }

	public TestUnitState State { get; set; }

	public string? Description { get; set; }

	public DateTime WaitingTestBeginTime { get; set; }

	public DateTime TestBeginTime { get; set; }

	public DateTime TestEndTime { get; set; }

	public double TestDurationSeconds
	{
		get
		{
			DateTime beginTime;
			DateTime endTime;
			switch (State)
			{
				default:
				case TestUnitState.Unknow:
					{
						return 0.0;
					}
				case TestUnitState.WaitingTest:
					{
						beginTime = WaitingTestBeginTime;
						endTime = DateTime.Now;
					}
					break;
				case TestUnitState.InTesting:
					{
						beginTime = TestBeginTime;
						endTime = DateTime.Now;
					}
					break;
				case TestUnitState.TestSuccess:
				case TestUnitState.TestFailed:
					{
						beginTime = TestBeginTime;
						endTime = TestEndTime;
					}
					break;
			}

			var durationSeconds = (endTime - beginTime).TotalSeconds;
			{ }
			return durationSeconds;
		}
	}

	public string TestDurationSecondsCaption => TestDurationSeconds.ToString("F2") + "秒";

	#endregion
}