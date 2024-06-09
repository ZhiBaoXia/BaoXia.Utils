using BaoXia.Utils.Extensions;
using BaoXia.Utils.TestUtils.MethodTestUtils.Constants;
using System;
using System.Collections.Generic;

namespace BaoXia.Utils.TestUtils.MethodTestUtils;

public class TestModule(string name)
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public string? Name { get; init; } = name;

	public int TestNumber { get; set; } = 1;

	public TestUnitState State
	{
		get
		{
			if (TestUnitInfes is not IEnumerable<TestUnitInfo> testUnitInfes)
			{
				return TestUnitState.Unknow;
			}

			var testUnitsCount = 0;
			var testUnitsCount_WaitingTest = 0;
			var testUnitsCount_TestSuccess = 0;
			var testUnitsCount_TestFailed = 0;
			foreach (var testUnitInfo in testUnitInfes)
			{
				testUnitsCount++;
				switch (testUnitInfo.State)
				{
					default:
					case TestUnitState.Unknow:
						{
							return TestUnitState.Unknow;
						}
					case TestUnitState.WaitingTest:
						{
							testUnitsCount_WaitingTest++;
						}
						break;
					case TestUnitState.InTesting:
						{
							return TestUnitState.InTesting;
						}
					case TestUnitState.TestSuccess:
						{
							testUnitsCount_TestSuccess++;
						}
						break;
					case TestUnitState.TestFailed:
						{
							testUnitsCount_TestFailed++;
						}
						break;
				}
			}
			if (testUnitsCount < 1)
			{
				return TestUnitState.Unknow;
			}
			if (testUnitsCount_TestSuccess + testUnitsCount_TestFailed == testUnitsCount)
			{
				if (testUnitsCount_WaitingTest > 0)
				{
					return TestUnitState.WaitingTest;
				}
				if (testUnitsCount_TestFailed > 0)
				{
					return TestUnitState.TestFailed;
				}
				return TestUnitState.TestSuccess;
			}

			return TestUnitState.Unknow;
		}
	}

	protected readonly Dictionary<string, TestUnitInfo> _testUnitInfes = new();

	public IEnumerable<TestUnitInfo>? TestUnitInfes => _testUnitInfes.Values;

	public int TestUnitsCount => TestUnitInfes?.GetCount() ?? 0;

	protected TestUnitInfo? _currentTestUnitInfo = null;

	public TestUnitState CurrentTestUnitState
	{
		get
		{
			return _currentTestUnitInfo?.State ?? TestUnitState.Unknow;
		}
	}


	#endregion


	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public bool BeginTestUnit(string name, string? description = null)
	{
		if (_testUnitInfes.TryGetValue(name, out var testInfo))
		{
			//
			testInfo.Description = description;
			if (testInfo.State == TestUnitState.Unknow
				|| testInfo.State == TestUnitState.WaitingTest)
			{
				testInfo.State = TestUnitState.InTesting;
				testInfo.TestBeginTime = DateTime.Now;
			}
			// !!!
			_currentTestUnitInfo = testInfo;
			// !!!
			return true;
		}
		_testUnitInfes.Add(name, new TestUnitInfo()
		{
			Name = name,
			Description = description,
			State = TestUnitState.WaitingTest,
			WaitingTestBeginTime = DateTime.Now
		});
		return false;
	}


	public void UpdateDescription(string? description)
	{
		if (_currentTestUnitInfo == null)
		{
			return;
		}
		// !!!
		_currentTestUnitInfo.Description = description;
		// !!!
	}

	public void EndTestUnit(
	    TestUnitState testUnitState,
	    string? description = null)
	{
		if (_currentTestUnitInfo == null)
		{
			return;
		}

		// !!!
		_currentTestUnitInfo.State = testUnitState;
		_currentTestUnitInfo.TestEndTime = DateTime.Now;
		_currentTestUnitInfo.Description = description;
		_currentTestUnitInfo = null;
		// !!!

	}

	#endregion
}