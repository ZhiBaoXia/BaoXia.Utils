using BaoXia.Utils.TestUtils.Attributes;
using System;
using System.Diagnostics;

namespace BaoXia.Utils.TestUtils
{
	public abstract class TestCase
	{
		////////////////////////////////////////////////
		// @静态常量
		////////////////////////////////////////////////

		#region 静态常量

		public class Assertor
		{
			public static Action? ToAssertFailed { get; set; }

			public bool IsHadAssertFailed { get; set; }

			public void IsTrue(bool trueResult)
			{
				// !!!
				System.Diagnostics.Debug.Assert(trueResult);
				// !!!
				if (trueResult != true)
				{
					IsHadAssertFailed = true;
					// !!!
					ToAssertFailed?.Invoke();
					throw new System.ApplicationException("测试断言失败。");
					// !!!
				}
			}

			public void IsFalse(bool falseResult)
			{
				IsTrue(!falseResult);
			}
		}

		#endregion


		////////////////////////////////////////////////
		// @自身属性
		////////////////////////////////////////////////

		#region 自身属性

		protected string? _name;

		public string? Name
		{
			get
			{
				return _name;
			}
		}

		public Assertor Assert { get; set; } = new();

		public bool IsHadAssertFailed
		{
			get
			{
				return Assert.IsHadAssertFailed;
			}
			set
			{
				Assert.IsHadAssertFailed = value;
			}
		}

		public double TestElapsedSeconds { get; set; }

		public string? TestResult { get; set; }

		public Action<string>? ToOutput { get; set; }

		#endregion

		////////////////////////////////////////////////
		// @自身实现
		////////////////////////////////////////////////

		#region 自身实现

		public TestCase(Action<string>? toOutput = null)
		{
			var allTestCases = TestCases.AllTestCases;
			lock (allTestCases)
			{
				// !!!
				allTestCases.Add(this);
				// !!!
			}

			var testCaseType = this.GetType();
			var nameAttributes
				= testCaseType.GetCustomAttributes(
					typeof(NameAttribute),
					false);
			if (nameAttributes?.Length > 0)
			{
				for (var nameAttributeIndex = nameAttributes.Length - 1;
					nameAttributeIndex >= 0;
					nameAttributeIndex--)
				{
					var nameAttributeObject = nameAttributes[nameAttributeIndex];
					if (nameAttributeObject is NameAttribute nameAttribute
						&& !string.IsNullOrEmpty(nameAttribute.Name))
					{
						// !!!
						_name = nameAttribute.Name;
						break;
						// !!!
					}
				}
			}
			if (string.IsNullOrEmpty(_name))
			{
				_name = testCaseType.FullName;
			}

			ToOutput = toOutput;
		}

		public void Output(
			string? testInfo,
			bool isWriteNewLine = true)
		{
			if (testInfo?.Length > 0)
			{
				testInfo = DateTime.Now.ToString("HH:mm:ss.fff " + testInfo);

				var toOutput = ToOutput;
				if (toOutput != null)
				{
					toOutput(testInfo);
				}
				else
				{
					System.Diagnostics.Debug.WriteLine(testInfo);
					if (isWriteNewLine)
					{
						System.Console.WriteLine(testInfo);
					}
					else
					{
						ConsoleUtil.WriteCurrentLine(testInfo);
					}
				}
			}
			else
			{
				if (isWriteNewLine)
				{
					System.Console.WriteLine();
				}
				else
				{
					ConsoleUtil.ClearCurrentLine();
				}
			}
		}

#pragma warning disable CA1822 // 将成员标记为 static
		public string? Input()
#pragma warning restore CA1822 // 将成员标记为 static
		{
			return System.Console.ReadLine();
		}

		public void OutputBlankRow()
		{
			Output(string.Empty);
		}

		public bool Test()
		{
			var stopwatch = new Stopwatch();
			OutputBlankRow();
			Output("////////////////////////////////////////////////");
			Output("// 【测试用例】：" + Name);
			Output("////////////////////////////////////////////////");
			OutputBlankRow();
			stopwatch.Start();
			{
				this.DidTest();
			}
			stopwatch.Stop();
			// !!!
			TestElapsedSeconds = stopwatch.Elapsed.TotalSeconds;
			// !!!

			OutputBlankRow();

			Output("------------------------------------------------");
			var testResult
				= (IsHadAssertFailed
				? $"× 测试【失败】，"
				: $"√ 测试通过，")
				+ Name
				+ $"，耗时：{TestElapsedSeconds} 秒。";
			{ }
			Output(testResult);
			TestResult = testResult;

			return IsHadAssertFailed == false;
		}

		#endregion

		////////////////////////////////////////////////
		// @事件节点
		////////////////////////////////////////////////

		#region 事件节点

		protected abstract void DidTest();

		#endregion
	}
}
