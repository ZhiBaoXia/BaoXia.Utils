using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Test
{
	[TestClass]
	public class LoopTaskTest
	{
		static int _testLoopTaskThreadId = 0;
		static readonly LoopTask _testLoopTask = new LoopTask((CancellationToken cancellationToken) =>
			{
				// !!!
				var currentThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
				if (_testLoopTaskThreadId == 0)
				{
					_testLoopTaskThreadId = currentThreadId;
				}
				// Loop Task使用 Task.Delay，因此线程Id可能会有变更。
				// Assert.IsTrue(_testLoopTaskThreadId == currentThreadId);
				// !!!

				// Thread.Sleep(100);

				return true;
			},
			1.0F,
			true,
			true);

		[TestMethod]
		public void ConcurrentStartTest()
		{
			var testTasks = new List<Task>();
			for (var i = 0;
				i < 100;
				i++)
			{
				testTasks.Add(Task.Run(() =>
				{
					_testLoopTask.Start();
				}));
			}
			//
			Task.WaitAll(testTasks.ToArray());
			//

			var beginTime = DateTime.Now;
			while ((DateTime.Now - beginTime).TotalSeconds <= 3.0)
			{
				Thread.Sleep(100);
			}
			_testLoopTask.Cancel();

			beginTime = DateTime.Now;
			while ((DateTime.Now - beginTime).TotalSeconds <= 3.0)
			{
				Thread.Sleep(100);
			}

			System.Diagnostics.Debug.WriteLine("LoopTestTest._loopTask.ThreadTaskIdSeed = " + _testLoopTask.ThreadTaskIdSeed + ";");
			Assert.IsTrue(_testLoopTask.CurrentState == LoopTask.LoopTaskState.StoppedWithCancel);

		}
	}
}
