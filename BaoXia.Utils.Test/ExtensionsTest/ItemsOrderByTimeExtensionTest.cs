using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BaoXia.Utils.Test.ExtensionsTest;

[TestClass]
public class ItemsOrderByTimeExtensionTest
{
	public class TestItem
	{
		public DateTimeOffset Time { get; set; }
		public int IntValue { get; set; }
	}

	/// <summary>
	/// 【目标起始时间】等于【测试集合】中的任意值，
	/// 【目标结束时间】等于【测试集合】中的任意值。
	/// </summary>
	[TestMethod]
	public void GetItemsSortByTimeInTimeSection_01_Test()
	{
		var testItems = new TestItem[100];
		var now = DateTimeOffset.Now;
		for (int i = 0; i < testItems.Length; i++)
		{
			testItems[i] = new TestItem
			{
				Time = now.AddDays(i),
				IntValue = i
			};
		}

		////////////////////////////////////////////////
		// 【升序】集合测试
		////////////////////////////////////////////////
		Array.Sort(
			testItems,
			(testItemA, testItemB) =>
		{
			//
			return testItemA.Time.CompareTo(testItemB.Time);
			//
		});

		var objectBeginTime = testItems[1].Time;
		var objectEndTime = testItems[^1].Time;
		var objectItems = testItems.GetItemsSortByTimeInTimeSection(
			true,
			objectBeginTime,
			objectEndTime,
			(testItem, time) =>
			{
				return testItem.Time.CompareTo(time);
			});
		{
			// !!!
			Assert.AreEqual(testItems.Length - 2, objectItems!.Length);
			Assert.AreEqual(testItems[1], objectItems[0]);
			Assert.AreEqual(testItems[^2], objectItems[^1]);
			// !!!
		}


		////////////////////////////////////////////////
		// 【降序】集合测试
		////////////////////////////////////////////////
		Array.Sort(
			testItems,
			(testItemA, testItemB) =>
			{
				//
				return testItemB.Time.CompareTo(testItemA.Time);
				//
			});
		objectBeginTime = testItems[^2].Time;
		objectEndTime = testItems[0].Time;
		objectItems = testItems.GetItemsSortByTimeInTimeSection(
			false,
			objectBeginTime,
			objectEndTime,
			(testItem, time) =>
			{
				return testItem.Time.CompareTo(time);
			});
		{
			// !!!
			Assert.AreEqual(testItems.Length - 2, objectItems!.Length);
			Assert.AreEqual(testItems[1], objectItems[0]);
			Assert.AreEqual(testItems[^2], objectItems[^1]);
			// !!!
		}
	}

	/// <summary>
	/// 【目标起始时间】大于【测试集合】最小值，
	/// 【目标结束时间】小于【测试集合】最大值。
	/// </summary>
	[TestMethod]
	public void GetItemsSortByTimeInTimeSection_02_Test()
	{
		var testItems = new TestItem[100];
		var now = DateTimeOffset.Now;
		for (int i = 0; i < testItems.Length; i++)
		{
			testItems[i] = new TestItem
			{
				Time = now.AddDays(i),
				IntValue = i
			};
		}

		////////////////////////////////////////////////
		// 【升序】集合测试
		////////////////////////////////////////////////
		Array.Sort(
			testItems,
			(testItemA, testItemB) =>
			{
				//
				return testItemA.Time.CompareTo(testItemB.Time);
				//
			});

		var objectBeginTime = testItems[1].Time.AddHours(-1);
		var objectEndTime = testItems[^1].Time.AddHours(-1);
		var objectItems = testItems.GetItemsSortByTimeInTimeSection(
			true,
			objectBeginTime,
			objectEndTime,
			(testItem, time) =>
			{
				return testItem.Time.CompareTo(time);
			});
		{
			// !!!
			Assert.AreEqual(testItems.Length - 2, objectItems!.Length);
			Assert.AreEqual(testItems[1], objectItems[0]);
			Assert.AreEqual(testItems[^2], objectItems[^1]);
			// !!!
		}


		////////////////////////////////////////////////
		// 【降序】集合测试
		////////////////////////////////////////////////
		Array.Sort(
			testItems,
			(testItemA, testItemB) =>
			{
				//
				return testItemB.Time.CompareTo(testItemA.Time);
				//
			});
		objectBeginTime = testItems[^2].Time.AddHours(-1);
		objectEndTime = testItems[0].Time.AddHours(-1);
		objectItems = testItems.GetItemsSortByTimeInTimeSection(
			false,
			objectBeginTime,
			objectEndTime,
			(testItem, time) =>
			{
				return testItem.Time.CompareTo(time);
			});
		{
			// !!!
			Assert.AreEqual(testItems.Length - 2, objectItems!.Length);
			Assert.AreEqual(testItems[1], objectItems[0]);
			Assert.AreEqual(testItems[^2], objectItems[^1]);
			// !!!
		}
	}

	/// <summary>
	/// 【目标起始时间】小于【测试集合】最小值，
	/// 【目标结束时间】大于【测试集合】最大值。
	/// </summary>
	[TestMethod]
	public void GetItemsSortByTimeInTimeSection_03_Test()
	{
		var testItems = new TestItem[100];
		var now = DateTimeOffset.Now;
		for (int i = 0; i < testItems.Length; i++)
		{
			testItems[i] = new TestItem
			{
				Time = now.AddDays(i),
				IntValue = i
			};
		}

		////////////////////////////////////////////////
		// 【升序】集合测试
		////////////////////////////////////////////////
		Array.Sort(
			testItems,
			(testItemA, testItemB) =>
			{
				//
				return testItemA.Time.CompareTo(testItemB.Time);
				//
			});

		var objectBeginTime = testItems[0].Time.AddHours(-1);
		var objectEndTime = testItems[^1].Time.AddHours(1);
		var objectItems = testItems.GetItemsSortByTimeInTimeSection(
			true,
			objectBeginTime,
			objectEndTime,
			(testItem, time) =>
			{
				return testItem.Time.CompareTo(time);
			});
		{
			// !!!
			Assert.AreEqual(testItems.Length - 0, objectItems!.Length);
			Assert.AreEqual(testItems[0], objectItems[0]);
			Assert.AreEqual(testItems[^1], objectItems[^1]);
			// !!!
		}


		////////////////////////////////////////////////
		// 【降序】集合测试
		////////////////////////////////////////////////
		Array.Sort(
			testItems,
			(testItemA, testItemB) =>
			{
				//
				return testItemB.Time.CompareTo(testItemA.Time);
				//
			});
		objectBeginTime = testItems[^1].Time.AddHours(-1);
		objectEndTime = testItems[0].Time.AddHours(1);
		objectItems = testItems.GetItemsSortByTimeInTimeSection(
			false,
			objectBeginTime,
			objectEndTime,
			(testItem, time) =>
			{
				return testItem.Time.CompareTo(time);
			});
		{
			// !!!
			Assert.AreEqual(testItems.Length - 0, objectItems!.Length);
			Assert.AreEqual(testItems[0], objectItems[0]);
			Assert.AreEqual(testItems[^1], objectItems[^1]);
			// !!!
		}
	}

	/// <summary>
	/// 【目标起始时间】等于【测试集合】中的任意值，
	/// 【目标结束时间】大于【测试集合】最大值。
	/// </summary>
	[TestMethod]
	public void GetItemsSortByTimeInTimeSection_04_Test()
	{
		var testItems = new TestItem[100];
		var now = DateTimeOffset.Now;
		for (int i = 0; i < testItems.Length; i++)
		{
			testItems[i] = new TestItem
			{
				Time = now.AddDays(i),
				IntValue = i
			};
		}

		////////////////////////////////////////////////
		// 【升序】集合测试
		////////////////////////////////////////////////
		Array.Sort(
			testItems,
			(testItemA, testItemB) =>
			{
				//
				return testItemA.Time.CompareTo(testItemB.Time);
				//
			});

		var objectBeginTime = testItems[1].Time;
		var objectEndTime = testItems[^1].Time.AddDays(1);
		var objectItems = testItems.GetItemsSortByTimeInTimeSection(
			true,
			objectBeginTime,
			objectEndTime,
			(testItem, time) =>
			{
				return testItem.Time.CompareTo(time);
			});
		{
			// !!!
			Assert.AreEqual(testItems.Length - 1, objectItems!.Length);
			Assert.AreEqual(testItems[1], objectItems[0]);
			Assert.AreEqual(testItems[^1], objectItems[^1]);
			// !!!
		}


		////////////////////////////////////////////////
		// 【降序】集合测试
		////////////////////////////////////////////////
		Array.Sort(
			testItems,
			(testItemA, testItemB) =>
			{
				//
				return testItemB.Time.CompareTo(testItemA.Time);
				//
			});
		objectBeginTime = testItems[^2].Time;
		objectEndTime = testItems[0].Time.AddDays(1);
		objectItems = testItems.GetItemsSortByTimeInTimeSection(
			false,
			objectBeginTime,
			objectEndTime,
			(testItem, time) =>
			{
				return testItem.Time.CompareTo(time);
			});
		{
			// !!!
			Assert.AreEqual(testItems.Length - 1, objectItems!.Length);
			Assert.AreEqual(testItems[0], objectItems[0]);
			Assert.AreEqual(testItems[^2], objectItems[^1]);
			// !!!
		}
	}

	/// <summary>
	/// 【目标起始时间】小于【测试集合】最小值，
	/// 【目标结束时间】等于【测试集合】中的任意值。
	/// </summary>
	[TestMethod]
	public void GetItemsSortByTimeInTimeSection_05_Test()
	{
		var testItems = new TestItem[100];
		var now = DateTimeOffset.Now;
		for (int i = 0; i < testItems.Length; i++)
		{
			testItems[i] = new TestItem
			{
				Time = now.AddDays(i),
				IntValue = i
			};
		}

		////////////////////////////////////////////////
		// 【升序】集合测试
		////////////////////////////////////////////////
		Array.Sort(
			testItems,
			(testItemA, testItemB) =>
			{
				//
				return testItemA.Time.CompareTo(testItemB.Time);
				//
			});

		var objectBeginTime = testItems[0].Time.AddDays(-1);
		var objectEndTime = testItems[^1].Time;
		var objectItems = testItems.GetItemsSortByTimeInTimeSection(
			true,
			objectBeginTime,
			objectEndTime,
			(testItem, time) =>
			{
				return testItem.Time.CompareTo(time);
			});
		{
			// !!!
			Assert.AreEqual(testItems.Length - 1, objectItems!.Length);
			Assert.AreEqual(testItems[0], objectItems[0]);
			Assert.AreEqual(testItems[^2], objectItems[^1]);
			// !!!
		}


		////////////////////////////////////////////////
		// 【降序】集合测试
		////////////////////////////////////////////////
		Array.Sort(
			testItems,
			(testItemA, testItemB) =>
			{
				//
				return testItemB.Time.CompareTo(testItemA.Time);
				//
			});
		objectBeginTime = testItems[^1].Time.AddDays(-1);
		objectEndTime = testItems[0].Time;
		objectItems = testItems.GetItemsSortByTimeInTimeSection(
			false,
			objectBeginTime,
			objectEndTime,
			(testItem, time) =>
			{
				return testItem.Time.CompareTo(time);
			});
		{
			// !!!
			Assert.AreEqual(testItems.Length - 1, objectItems!.Length);
			Assert.AreEqual(testItems[1], objectItems[0]);
			Assert.AreEqual(testItems[^1], objectItems[^1]);
			// !!!
		}
	}

	/// <summary>
	/// 【目标起始时间】小于【测试集合】最小值，
	/// 【目标结束时间】小于【测试集合】最小值。
	/// </summary>
	[TestMethod]
	public void GetItemsSortByTimeInTimeSection_06_Test()
	{
		var testItems = new TestItem[100];
		var now = DateTimeOffset.Now;
		for (int i = 0; i < testItems.Length; i++)
		{
			testItems[i] = new TestItem
			{
				Time = now.AddDays(i),
				IntValue = i
			};
		}

		////////////////////////////////////////////////
		// 【升序】集合测试
		////////////////////////////////////////////////
		Array.Sort(
			testItems,
			(testItemA, testItemB) =>
			{
				//
				return testItemA.Time.CompareTo(testItemB.Time);
				//
			});

		var objectBeginTime = testItems[0].Time.AddDays(-2);
		var objectEndTime = objectBeginTime.AddDays(1);
		var objectItems = testItems.GetItemsSortByTimeInTimeSection(
			true,
			objectBeginTime,
			objectEndTime,
			(testItem, time) =>
			{
				return testItem.Time.CompareTo(time);
			});
		{
			// !!!
			Assert.IsTrue(objectItems == null || objectItems?.Length == 0);
			// !!!
		}


		////////////////////////////////////////////////
		// 【降序】集合测试
		////////////////////////////////////////////////
		Array.Sort(
			testItems,
			(testItemA, testItemB) =>
			{
				//
				return testItemB.Time.CompareTo(testItemA.Time);
				//
			});
		objectBeginTime = testItems[^1].Time.AddDays(-2);
		objectEndTime = objectBeginTime.AddDays(1);
		objectItems = testItems.GetItemsSortByTimeInTimeSection(
			false,
			objectBeginTime,
			objectEndTime,
			(testItem, time) =>
			{
				return testItem.Time.CompareTo(time);
			});
		{
			// !!!
			Assert.IsTrue(objectItems == null || objectItems?.Length == 0);
			// !!!
		}
	}


	/// <summary>
	/// 【目标起始时间】等于【目标结束时间】。
	/// </summary>
	[TestMethod]
	public void GetItemsSortByTimeInTimeSection_07_Test()
	{
		var testItems = new TestItem[100];
		var now = DateTimeOffset.Now;
		for (int i = 0; i < testItems.Length; i++)
		{
			testItems[i] = new TestItem
			{
				Time = now.AddDays(i),
				IntValue = i
			};
		}

		////////////////////////////////////////////////
		// 【升序】集合测试
		////////////////////////////////////////////////
		Array.Sort(
			testItems,
			(testItemA, testItemB) =>
			{
				//
				return testItemA.Time.CompareTo(testItemB.Time);
				//
			});

		var objectBeginTime = testItems[0].Time;
		var objectEndTime = objectBeginTime;
		var objectItems = testItems.GetItemsSortByTimeInTimeSection(
			true,
			objectBeginTime,
			objectEndTime,
			(testItem, time) =>
			{
				return testItem.Time.CompareTo(time);
			});
		{
			// !!!
			Assert.IsTrue(objectItems == null || objectItems?.Length == 0);
			// !!!
		}


		////////////////////////////////////////////////
		// 【降序】集合测试
		////////////////////////////////////////////////
		Array.Sort(
			testItems,
			(testItemA, testItemB) =>
			{
				//
				return testItemB.Time.CompareTo(testItemA.Time);
				//
			});
		objectBeginTime = testItems[^1].Time;
		objectEndTime = objectBeginTime;
		objectItems = testItems.GetItemsSortByTimeInTimeSection(
			false,
			objectBeginTime,
			objectEndTime,
			(testItem, time) =>
			{
				return testItem.Time.CompareTo(time);
			});
		{
			// !!!
			Assert.IsTrue(objectItems == null || objectItems?.Length == 0);
			// !!!
		}
	}
}