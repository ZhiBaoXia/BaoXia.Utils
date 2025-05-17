using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BaoXia.Utils.Test.ExtensionsTest
{
	[TestClass]
	public class StringsExtensionTest
	{
		[TestMethod]
		public void IsContainsItemTest()
		{
			////////////////////////////////////////////////
			// 1/3，“items”没有元素：
			////////////////////////////////////////////////
			{
				var items = Array.Empty<string>();

				// 1/2，不包含：
				Assert.IsFalse(items.IsContains("0"));
				Assert.IsTrue(items.IsNotContains("0"));

				// 1/2，包含：
				Assert.IsFalse(items.IsContains("1"));
				Assert.IsTrue(items.IsNotContains("1"));
			}

			////////////////////////////////////////////////
			// 2/3，“items”含有"1"个元素：
			////////////////////////////////////////////////
			{
				var items = new string[]
				{
					"1"
				};

				// 1/2，不包含：
				Assert.IsFalse(items.IsContains("0"));
				Assert.IsTrue(items.IsNotContains("0"));

				// 1/2，包含：
				Assert.IsTrue(items.IsContains("1"));
				Assert.IsFalse(items.IsNotContains("1"));
			}

			////////////////////////////////////////////////
			// 3/3，“items”含有多个元素：
			////////////////////////////////////////////////
			{
				var items = new string[]
				{
					"1",
					"2",
					"3"
				};

				// 1/2，不包含：
				Assert.IsFalse(items.IsContains("0"));
				Assert.IsTrue(items.IsNotContains("0"));

				// 1/2，包含：
				Assert.IsTrue(items.IsContains("1"));
				Assert.IsFalse(items.IsNotContains("1"));
			}
		}

		[TestMethod]
		public void IsContainsItemsTest()
		{
			var items_1_2 = new string[]
			{
				"1",
				"2"
			};
			var items_3_4 = new string[]
			{
				"3",
				"4"
			};

			////////////////////////////////////////////////
			// 1/4，“items”没有元素：
			////////////////////////////////////////////////
			{
				var items = Array.Empty<string>();

				Assert.IsFalse(items.IsContains(items_1_2));
				Assert.IsTrue(items.IsNotContains(items_1_2));

				Assert.IsFalse(items.IsContains(items_3_4));
				Assert.IsTrue(items.IsNotContains(items_3_4));
			}

			////////////////////////////////////////////////
			// 2/4，“items”含有"1"个元素：
			////////////////////////////////////////////////
			{
				var items = new string[]
				{
					"1"
				};

				Assert.IsFalse(items.IsContains(items_1_2));
				Assert.IsTrue(items.IsNotContains(items_1_2));

				Assert.IsFalse(items.IsContains(items_3_4));
				Assert.IsTrue(items.IsNotContains(items_3_4));
			}

			////////////////////////////////////////////////
			// 3/4，“items”含有同样多个元素：
			////////////////////////////////////////////////
			{
				var items = new string[]
				{
					"1",
					"2"
				};

				Assert.IsTrue(items.IsContains(items_1_2));
				Assert.IsFalse(items.IsNotContains(items_1_2));

				Assert.IsFalse(items.IsContains(items_3_4));
				Assert.IsTrue(items.IsNotContains(items_3_4));
			}

			////////////////////////////////////////////////
			// 4/4，“items”含有多个元素：
			////////////////////////////////////////////////
			{
				var items = new string[]
				{
					"1",
					"2",
					"3"
				};

				Assert.IsTrue(items.IsContains(items_1_2));
				Assert.IsFalse(items.IsNotContains(items_1_2));

				Assert.IsFalse(items.IsContains(items_3_4));
				Assert.IsTrue(items.IsNotContains(items_3_4));

				items = new string[]
				{
					"1",
					"2",
					"3",
					"4"
				};

				Assert.IsTrue(items.IsContains(items_1_2));
				Assert.IsFalse(items.IsNotContains(items_1_2));

				Assert.IsTrue(items.IsContains(items_3_4));
				Assert.IsFalse(items.IsNotContains(items_3_4));


				items = new string[]
				{
					"1",
					"2",
					"3",
					"4",
					"5"
				};

				Assert.IsTrue(items.IsContains(items_1_2));
				Assert.IsFalse(items.IsNotContains(items_1_2));

				Assert.IsTrue(items.IsContains(items_3_4));
				Assert.IsFalse(items.IsNotContains(items_3_4));
			}
		}

		[TestMethod]
		public void IsEqualsTest()
		{
			var items_1_2 = new string[]
			{
				"1",
				"2"
			};
			var items_3_4 = new string[]
			{
				"3",
				"4"
			};
			var items_1_1_2_2 = new string[]
			{
				"1",
				"1",
				"2",
				"2"
			};
			var items_3_3_4_4_4 = new string[]
			{
				"3",
				"3",
				"4",
				"4"
			};
			var items_1_2_2 = new string[]
			{
				"1",
				"2",
				"2"
			};

			////////////////////////////////////////////////
			// 1/4，“items”没有元素：
			////////////////////////////////////////////////
			{
				var items = Array.Empty<string>();

				Assert.IsFalse(items.IsEquals(items_1_2));
				Assert.IsTrue(items.IsNotEquals(items_1_2));

				Assert.IsFalse(items.IsEquals(items_3_4));
				Assert.IsTrue(items.IsNotEquals(items_3_4));
			}

			////////////////////////////////////////////////
			// 2/4，“items”含有"1"个元素：
			////////////////////////////////////////////////
			{
				var items = new string[]
				{
					"1"
				};

				Assert.IsFalse(items.IsEquals(items_1_2));
				Assert.IsTrue(items.IsNotEquals(items_1_2));

				Assert.IsFalse(items.IsEquals(items_3_4));
				Assert.IsTrue(items.IsNotEquals(items_3_4));
			}

			////////////////////////////////////////////////
			// 3/4，“items”含有同样多个元素：
			////////////////////////////////////////////////
			{
				var items = new string[]
				{
					"1",
					"2"
				};

				Assert.IsTrue(items.IsEquals(items_1_2));
				Assert.IsFalse(items.IsNotEquals(items_1_2));

				Assert.IsFalse(items.IsEquals(items_3_4));
				Assert.IsTrue(items.IsNotEquals(items_3_4));
			}

			////////////////////////////////////////////////
			// 4/4，“items”含有多个元素：
			////////////////////////////////////////////////
			{
				var items = new string[]
				{
					"1",
					"2",
					"3"
				};

				Assert.IsFalse(items.IsEquals(items_1_2));
				Assert.IsTrue(items.IsNotEquals(items_1_2));

				Assert.IsFalse(items.IsEquals(items_3_4));
				Assert.IsTrue(items.IsNotEquals(items_3_4));

				items = new string[]
				{
					"1",
					"2",
					"3",
					"4"
				};

				Assert.IsFalse(items.IsEquals(items_1_2));
				Assert.IsTrue(items.IsNotEquals(items_1_2));

				Assert.IsFalse(items.IsEquals(items_3_4));
				Assert.IsTrue(items.IsNotEquals(items_3_4));


				items = new string[]
				{
					"1",
					"2",
					"3",
					"4",
					"5"
				};

				Assert.IsFalse(items.IsEquals(items_1_2));
				Assert.IsTrue(items.IsNotEquals(items_1_2));

				Assert.IsFalse(items.IsEquals(items_3_4));
				Assert.IsTrue(items.IsNotEquals(items_3_4));


				////////////////////////////////////////////////
				////////////////////////////////////////////////
				////////////////////////////////////////////////


				items = new string[]
				{
					"1",
					"2",
					"1",
					"2"
				};

				Assert.IsTrue(items.IsEquals(items_1_2, true));
				Assert.IsFalse(items.IsNotEquals(items_1_2, true));
				Assert.IsFalse(items.IsEquals(items_1_2, false));
				Assert.IsTrue(items.IsNotEquals(items_1_2, false));

				Assert.IsFalse(items.IsEquals(items_3_4, true));
				Assert.IsTrue(items.IsNotEquals(items_3_4, true));
				Assert.IsFalse(items.IsEquals(items_3_4, false));
				Assert.IsTrue(items.IsNotEquals(items_3_4, false));


				items = new string[]
				{
					"3",
					"3",
					"4",
					"4",
					"4"
				};

				Assert.IsFalse(items.IsEquals(items_1_2, true));
				Assert.IsTrue(items.IsNotEquals(items_1_2, true));
				Assert.IsFalse(items.IsEquals(items_1_2, false));
				Assert.IsTrue(items.IsNotEquals(items_1_2, false));

				Assert.IsTrue(items.IsEquals(items_3_4, true));
				Assert.IsFalse(items.IsNotEquals(items_3_4, true));
				Assert.IsFalse(items.IsEquals(items_3_4, false));
				Assert.IsTrue(items.IsNotEquals(items_3_4, false));


				////////////////////////////////////////////////
				////////////////////////////////////////////////
				////////////////////////////////////////////////


				items = new string[]
				{
					"1",
					"2"
				};

				Assert.IsTrue(items.IsEquals(items_1_1_2_2, true));
				Assert.IsFalse(items.IsNotEquals(items_1_1_2_2, true));
				Assert.IsFalse(items.IsEquals(items_1_1_2_2, false));
				Assert.IsTrue(items.IsNotEquals(items_1_1_2_2, false));

				Assert.IsFalse(items.IsEquals(items_3_3_4_4_4, true));
				Assert.IsTrue(items.IsNotEquals(items_3_3_4_4_4, true));
				Assert.IsFalse(items.IsEquals(items_3_3_4_4_4, false));
				Assert.IsTrue(items.IsNotEquals(items_3_3_4_4_4, false));


				items = new string[]
				{
					"3",
					"4"
				};

				Assert.IsFalse(items.IsEquals(items_1_1_2_2, true));
				Assert.IsTrue(items.IsNotEquals(items_1_1_2_2, true));
				Assert.IsFalse(items.IsEquals(items_1_1_2_2, false));
				Assert.IsTrue(items.IsNotEquals(items_1_1_2_2, false));

				Assert.IsTrue(items.IsEquals(items_3_3_4_4_4, true));
				Assert.IsFalse(items.IsNotEquals(items_3_3_4_4_4, true));
				Assert.IsFalse(items.IsEquals(items_3_3_4_4_4, false));
				Assert.IsTrue(items.IsNotEquals(items_3_3_4_4_4, false));


				////////////////////////////////////////////////
				////////////////////////////////////////////////
				////////////////////////////////////////////////


				items = new string[]
				{
					"1",
					"2"
				};

				Assert.IsTrue(items.IsEquals(items_1_1_2_2, true));
				Assert.IsFalse(items.IsNotEquals(items_1_1_2_2, true));
				Assert.IsFalse(items.IsEquals(items_1_1_2_2, false));
				Assert.IsTrue(items.IsNotEquals(items_1_1_2_2, false));

				Assert.IsFalse(items.IsEquals(items_3_3_4_4_4, true));
				Assert.IsTrue(items.IsNotEquals(items_3_3_4_4_4, true));
				Assert.IsFalse(items.IsEquals(items_3_3_4_4_4, false));
				Assert.IsTrue(items.IsNotEquals(items_3_3_4_4_4, false));


				items = new string[]
				{
					"3",
					"4"
				};

				Assert.IsFalse(items.IsEquals(items_1_1_2_2, true));
				Assert.IsTrue(items.IsNotEquals(items_1_1_2_2, true));
				Assert.IsFalse(items.IsEquals(items_1_1_2_2, false));
				Assert.IsTrue(items.IsNotEquals(items_1_1_2_2, false));

				Assert.IsTrue(items.IsEquals(items_3_3_4_4_4, true));
				Assert.IsFalse(items.IsNotEquals(items_3_3_4_4_4, true));
				Assert.IsFalse(items.IsEquals(items_3_3_4_4_4, false));
				Assert.IsTrue(items.IsNotEquals(items_3_3_4_4_4, false));


				////////////////////////////////////////////////
				////////////////////////////////////////////////
				////////////////////////////////////////////////


				items = new string[]
				{
					"1",
					"2"
				};

				Assert.IsTrue(items.IsEquals(items_1_2_2, true));
				Assert.IsFalse(items.IsNotEquals(items_1_2_2, true));
				Assert.IsFalse(items.IsEquals(items_1_2_2, false));
				Assert.IsTrue(items.IsNotEquals(items_1_2_2, false));


				items = new string[]
				{
					"3",
					"4"
				};

				Assert.IsFalse(items.IsEquals(items_1_1_2_2, true));
				Assert.IsTrue(items.IsNotEquals(items_1_1_2_2, true));
				Assert.IsFalse(items.IsEquals(items_1_1_2_2, false));
				Assert.IsTrue(items.IsNotEquals(items_1_1_2_2, false));

				Assert.IsTrue(items.IsEquals(items_3_3_4_4_4, true));
				Assert.IsFalse(items.IsNotEquals(items_3_3_4_4_4, true));
				Assert.IsFalse(items.IsEquals(items_3_3_4_4_4, false));
				Assert.IsTrue(items.IsNotEquals(items_3_3_4_4_4, false));
			}
		}

		[TestMethod]
		public void ToStringWithSeparator_StringArray_Test()
		{
			////////////////////////////////////////////////
			// 1/，正常组合：
			////////////////////////////////////////////////
			var strings = new string[] { "1", "2", "3" };
			{
				Assert.IsTrue(strings
					.ToStringWithSeparator(",")
					.Equals(
					"1,2,3"));
			}

			////////////////////////////////////////////////
			// 2/，没有分隔符：
			////////////////////////////////////////////////
			{
				Assert.IsTrue(strings
					.ToStringWithSeparator(null)
					.Equals(
					"123"));
			}

			////////////////////////////////////////////////
			// 3/，没有元素：
			////////////////////////////////////////////////
			{
				Assert.IsTrue(
					(new string[0])
					.ToStringWithSeparator(",")
					.Equals(
					""));
				Assert.IsTrue(
					(new string[0])
					.ToStringWithSeparator(null)
					.Equals(
					""));
			}
		}

		[TestMethod]
		public void ToStringWithSeparator_StringParams_Test()
		{
			////////////////////////////////////////////////
			// 1/，正常组合：
			////////////////////////////////////////////////
			var finalString = StringUtil.StringWithStringsJoinSeparator(
					",",
					"1",
					"2",
					"3");
			{
				Assert.IsTrue(
					finalString
					.Equals(
					"1,2,3"));
			}

			////////////////////////////////////////////////
			// 2/，没有分隔符：
			////////////////////////////////////////////////
			finalString = StringUtil.StringWithStringsJoinSeparator(
					null,
					"1",
					"2",
					"3");
			{
				Assert.IsTrue(
					finalString
					.Equals(
					"123"));
			}

			////////////////////////////////////////////////
			// 3/，没有元素：
			////////////////////////////////////////////////
			finalString = StringUtil.StringWithStringsJoinSeparator(
					",");
			{
				Assert.IsTrue(
					finalString
					.Equals(
					""));
			}
			finalString = StringUtil.StringWithStringsJoinSeparator(
					null);
			{
				Assert.IsTrue(
					finalString
					.Equals(
					""));
			}
		}
	}
}
