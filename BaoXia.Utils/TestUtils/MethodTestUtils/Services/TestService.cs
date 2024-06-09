using BaoXia.Utils.TestUtils.MethodTestUtils.Constants;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.TestUtils.MethodTestUtils.Services;

public abstract class TestService(string testModuleTitle)
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	protected SemaphoreSlim _testLocker = new(1);

	public int TestsCount { get; protected set; }

	public TestModule TestModule { get; set; } = new(testModuleTitle);

	public DateTime TestBeginTime { get; set; }

	public DateTime TestEndTime { get; set; }

	#endregion


	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public async Task TestAsync(
		int testsCount,
		double testIntervalSeconds)
	{
		// !!!
		TestsCount = testsCount;
		// !!!

		switch (TestModule.State)
		{
			case TestUnitState.Unknow:
			case TestUnitState.TestSuccess:
				{ }
				break;
			case TestUnitState.WaitingTest:
			case TestUnitState.InTesting:
			case TestUnitState.TestFailed:
				{
					return;
				}
		}

		await _testLocker.WaitAsync();
		try
		{
			while (TestModule.State == TestUnitState.Unknow)
			{
				TestBeginTime = DateTime.Now;
				var now = DateTime.Now;
				var testIdentity = now.ToString("yyyyMMddHHmmssfff");
				// 1/2，准备测试：
				await DidTestAsync(testIdentity, now, TestModule);
				// 2/2，实际测试：
				// !!!
				await DidTestAsync(testIdentity, now, TestModule);
				// !!!
				TestEndTime = DateTime.Now;

				if (TestModule.State == TestUnitState.TestSuccess
					&& TestModule.TestNumber < TestsCount)
				{
					if (testIntervalSeconds > 0)
					{
						await Task.Delay((int)(1000 * testIntervalSeconds));
					}

					TestModule = new TestModule(testModuleTitle)
					{
						TestNumber = TestModule.TestNumber + 1
					};
				}
				else
				{
					break;
				}
			}
		}
		catch (Exception exception)
		{
			TestModule.EndTestUnit(
				TestUnitState.TestFailed,
				"当前测试失败，程序异常：\r\n" + exception.ToString());
		}
		finally
		{
			_testLocker.Release();
		}
	}

	#endregion


	////////////////////////////////////////////////
	// @事件节点
	////////////////////////////////////////////////

	#region 事件节点

	protected abstract Task DidTestAsync(
		string testIdentity,
		DateTime testBeginTime,
		TestModule testUnitInfes);

	#endregion
}