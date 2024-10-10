using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BaoXia.Utils.Test.ExtensionsTest.ItemsOrderByTimeExtensionTest;

namespace BaoXia.Utils.Test.ExtensionsTest;

[TestClass]
public class ItemsOrderByTimeExtensionTest
{
	public class TestItem
	{
		public DateTimeOffset Time { get; set; }
		public int IntValue { get; set; }
	}

	[TestMethod]
	public void ItemsOrderByWithObjectBeginTimeAndEndTimeTest()
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
		var objectItems = testItems.GetItemsSortByTime_Asc_InTimeSection(
			objectBeginTime,
			objectEndTime,
			(testItem, time) =>
			{
				return testItem.Time.CompareTo(time);
			});
		{
			// !!!
			Assert.IsTrue(objectItems!.Length == (testItems.Length - 2));
			Assert.IsTrue(objectItems[0] == testItems[1]);
			Assert.IsTrue(objectItems[^1] == testItems[^2]);
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
		objectEndTime = testItems[0].Time;
		objectItems = testItems.GetItemsSortByTime_Desc_InTimeSection(
			objectBeginTime,
			objectEndTime,
			(testItem, time) =>
			{
				return testItem.Time.CompareTo(time);
			});
		{
			// !!!
			Assert.IsTrue(objectItems!.Length == (testItems.Length - 2));
			Assert.IsTrue(objectItems[0] == testItems[1]);
			Assert.IsTrue(objectItems[^1] == testItems[^2]);
			// !!!
		}
	}
}