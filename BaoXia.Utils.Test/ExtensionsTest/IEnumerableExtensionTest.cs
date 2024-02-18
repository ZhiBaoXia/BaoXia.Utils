using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaoXia.Utils.Test.ExtensionsTest;

[TestClass]
public class IEnumerableExtensionTest
{
	[TestMethod]
	public void GetCountTest()
	{
		var itemArray = new int[] { 1, 2, 3, 4, 5 };
		var itemList = new List<int> { 1, 2, 3, 4, 5, 6 };

		IEnumerable<int> itemEnumerable = itemArray;
		{
			Assert.IsTrue(itemEnumerable.GetCount() == itemArray.Length);
		}

		itemEnumerable = itemList;
		{
			Assert.IsTrue(itemEnumerable.GetCount() == itemList.Count);
		}
	}

	[TestMethod]
	public void ForEachAtLeastOnceTest()
	{
		var items_Int = new int[] { 1, 2, 3, 4, 5 };
		var objectItem_Int = 0;
		items_Int.ForEachAtLeastOnce((item) =>
		{
			objectItem_Int++;
			{
				// !!!
				Assert.IsTrue(item == objectItem_Int);
				// !!!
			}
			return true;
		});
		items_Int = Array.Empty<int>();
		items_Int.ForEachAtLeastOnce((item) =>
		{
			// !!!
			Assert.IsTrue(item == 0);
			// !!!
			return true;
		});
		items_Int = null;
		items_Int.ForEachAtLeastOnce((item) =>
		{
			// !!!
			Assert.IsTrue(item == 0);
			// !!!
			return true;
		});


		var items_String = new string[] { "1", "2", "3", "4", "5" };
		var objectItem_String = 0;
		items_String.ForEachAtLeastOnce((item) =>
		{
			objectItem_String++;
			{
				// !!!
				Assert.IsTrue(item == objectItem_String.ToString());
				// !!!
			}
			return true;
		});
		items_String = Array.Empty<string>();
		items_String.ForEachAtLeastOnce((item) =>
		{
			// !!!
			Assert.IsTrue(item == null);
			// !!!
			return true;
		});
		items_String = null;
		items_String.ForEachAtLeastOnce((item) =>
		{
			// !!!
			Assert.IsTrue(item == null);
			// !!!
			return true;
		});
	}


	[TestMethod]
	public async Task ForEachAtLeastOnceAsyncAsyncTest()
	{
		var items_Int = new int[] { 1, 2, 3, 4, 5 };
		var objectItem_Int = 0;
		await items_Int.ForEachAtLeastOnceAsync(async (item) =>
		{
			objectItem_Int++;
			{
				// !!!
				Assert.IsTrue(item == objectItem_Int);
				// !!!
			}
			return await Task.FromResult(true);
		});
		items_Int = Array.Empty<int>();
		await items_Int.ForEachAtLeastOnceAsync(async (item) =>
		{
			// !!!
			Assert.IsTrue(item == 0);
			// !!!
			return await Task.FromResult(true);
		});
		items_Int = null;
		await items_Int.ForEachAtLeastOnceAsync(async (item) =>
		{
			// !!!
			Assert.IsTrue(item == 0);
			// !!!
			return await Task.FromResult(true);
		});


		var items_String = new string[] { "1", "2", "3", "4", "5" };
		var objectItem_String = 0;
		await items_String.ForEachAtLeastOnceAsync(async (item) =>
		{
			objectItem_String++;
			{
				// !!!
				Assert.IsTrue(item == objectItem_String.ToString());
				// !!!
			}
			return await Task.FromResult(true);
		});
		items_String = Array.Empty<string>();
		await items_String.ForEachAtLeastOnceAsync(async (item) =>
		{
			// !!!
			Assert.IsTrue(item == null);
			// !!!
			return await Task.FromResult(true);
		});
		items_String = null;
		await items_String.ForEachAtLeastOnceAsync(async (item) =>
		{
			// !!!
			Assert.IsTrue(item == null);
			// !!!
			return await Task.FromResult(true);
		});
	}

	[TestMethod]
	public void IsContainsItemTest()
	{
		////////////////////////////////////////////////
		// 1/3，“items”没有元素：
		////////////////////////////////////////////////
		{
			var items = Array.Empty<int>();

			// 1/2，不包含：
			Assert.IsTrue(items.IsContains(0) == false);
			Assert.IsTrue(items.IsNotContains(0) == true);

			// 1/2，包含：
			Assert.IsTrue(items.IsContains(1) == false);
			Assert.IsTrue(items.IsNotContains(1) == true);
		}

		////////////////////////////////////////////////
		// 2/3，“items”含有1个元素：
		////////////////////////////////////////////////
		{
			var items = new int[]
			{
				1
			};

			// 1/2，不包含：
			Assert.IsTrue(items.IsContains(0) == false);
			Assert.IsTrue(items.IsNotContains(0) == true);

			// 1/2，包含：
			Assert.IsTrue(items.IsContains(1) == true);
			Assert.IsTrue(items.IsNotContains(1) == false);
		}

		////////////////////////////////////////////////
		// 3/3，“items”含有多个元素：
		////////////////////////////////////////////////
		{
			var items = new int[]
			{
				1,
				2,
				3
			};

			// 1/2，不包含：
			Assert.IsTrue(items.IsContains(0) == false);
			Assert.IsTrue(items.IsNotContains(0) == true);

			// 1/2，包含：
			Assert.IsTrue(items.IsContains(1) == true);
			Assert.IsTrue(items.IsNotContains(1) == false);
		}
	}

	[TestMethod]
	public void IsContainsItemsTest()
	{
		var items_1_2 = new int[]
		{
			1,
			2
		};
		var items_3_4 = new int[]
		{
			3,
			4
		};

		////////////////////////////////////////////////
		// 0/4，“空”集合：
		////////////////////////////////////////////////
		{
			var items = Array.Empty<int>();

			Assert.IsTrue(items.IsContains(null as int[]) == false);
			Assert.IsTrue(items.IsContains(Array.Empty<int>()) == false);
		}

		////////////////////////////////////////////////
		// 1/4，“items”没有元素：
		////////////////////////////////////////////////
		{
			var items = Array.Empty<int>();

			Assert.IsTrue(items.IsContains(items_1_2) == false);
			Assert.IsTrue(items.IsNotContains(items_1_2) == true);

			Assert.IsTrue(items.IsContains(items_3_4) == false);
			Assert.IsTrue(items.IsNotContains(items_3_4) == true);
		}

		////////////////////////////////////////////////
		// 2/4，“items”含有1个元素：
		////////////////////////////////////////////////
		{
			var items = new int[]
			{
				1
			};

			Assert.IsTrue(items.IsContains(items_1_2) == false);
			Assert.IsTrue(items.IsNotContains(items_1_2) == true);

			Assert.IsTrue(items.IsContains(items_3_4) == false);
			Assert.IsTrue(items.IsNotContains(items_3_4) == true);
		}

		////////////////////////////////////////////////
		// 3/4，“items”含有同样多个元素：
		////////////////////////////////////////////////
		{
			var items = new int[]
			{
				1,
				2
			};

			Assert.IsTrue(items.IsContains(items_1_2) == true);
			Assert.IsTrue(items.IsNotContains(items_1_2) == false);

			Assert.IsTrue(items.IsContains(items_3_4) == false);
			Assert.IsTrue(items.IsNotContains(items_3_4) == true);
		}

		////////////////////////////////////////////////
		// 4/4，“items”含有多个元素：
		////////////////////////////////////////////////
		{
			var items = new int[]
			{
				1,
				2,
				3
			};

			Assert.IsTrue(items.IsContains(items_1_2) == true);
			Assert.IsTrue(items.IsNotContains(items_1_2) == false);

			Assert.IsTrue(items.IsContains(items_3_4) == false);
			Assert.IsTrue(items.IsNotContains(items_3_4) == true);

			items = new int[]
			{
				1,
				2,
				3,
				4
			};

			Assert.IsTrue(items.IsContains(items_1_2) == true);
			Assert.IsTrue(items.IsNotContains(items_1_2) == false);

			Assert.IsTrue(items.IsContains(items_3_4) == true);
			Assert.IsTrue(items.IsNotContains(items_3_4) == false);


			items = new int[]
			{
				1,
				2,
				3,
				4,
				5
			};

			Assert.IsTrue(items.IsContains(items_1_2) == true);
			Assert.IsTrue(items.IsNotContains(items_1_2) == false);

			Assert.IsTrue(items.IsContains(items_3_4) == true);
			Assert.IsTrue(items.IsNotContains(items_3_4) == false);
		}
	}

	[TestMethod]
	public void IsEqualsTest()
	{
		var items_1_2 = new int[]
		{
			1,
			2
		};
		var items_3_4 = new int[]
		{
			3,
			4
		};
		var items_1_1_2_2 = new int[]
		{
			1,
			1,
			2,
			2
		};
		var items_3_3_4_4_4 = new int[]
		{
			3,
			3,
			4,
			4
		};
		var items_1_2_2 = new int[]
		{
			1,
			2,
			2
		};



		////////////////////////////////////////////////
		// 0/4，“空”集合：
		////////////////////////////////////////////////
		{
			var items = Array.Empty<int>();

			Assert.IsTrue(items.IsEquals(null, true) == true);
			Assert.IsTrue(items.IsEquals(Array.Empty<int>(), true) == true);
		}

		////////////////////////////////////////////////
		// 1/4，“items”没有元素：
		////////////////////////////////////////////////
		{
			var items = Array.Empty<int>();

			Assert.IsTrue(items.IsEquals(items_1_2) == false);
			Assert.IsTrue(items.IsNotEquals(items_1_2) == true);

			Assert.IsTrue(items.IsEquals(items_3_4) == false);
			Assert.IsTrue(items.IsNotEquals(items_3_4) == true);
		}

		////////////////////////////////////////////////
		// 2/4，“items”含有1个元素：
		////////////////////////////////////////////////
		{
			var items = new int[]
			{
				1
			};

			Assert.IsTrue(items.IsEquals(items_1_2) == false);
			Assert.IsTrue(items.IsNotEquals(items_1_2) == true);

			Assert.IsTrue(items.IsEquals(items_3_4) == false);
			Assert.IsTrue(items.IsNotEquals(items_3_4) == true);
		}

		////////////////////////////////////////////////
		// 3/4，“items”含有同样多个元素：
		////////////////////////////////////////////////
		{
			var items = new int[]
			{
				1,
				2
			};

			Assert.IsTrue(items.IsEquals(items_1_2) == true);
			Assert.IsTrue(items.IsNotEquals(items_1_2) == false);

			Assert.IsTrue(items.IsEquals(items_3_4) == false);
			Assert.IsTrue(items.IsNotEquals(items_3_4) == true);
		}

		////////////////////////////////////////////////
		// 4/4，“items”含有多个元素：
		////////////////////////////////////////////////
		{
			var items = new int[]
			{
				1,
				2,
				3
			};

			Assert.IsTrue(items.IsEquals(items_1_2) == false);
			Assert.IsTrue(items.IsNotEquals(items_1_2) == true);

			Assert.IsTrue(items.IsEquals(items_3_4) == false);
			Assert.IsTrue(items.IsNotEquals(items_3_4) == true);

			items = new int[]
			{
				1,
				2,
				3,
				4
			};

			Assert.IsTrue(items.IsEquals(items_1_2) == false);
			Assert.IsTrue(items.IsNotEquals(items_1_2) == true);

			Assert.IsTrue(items.IsEquals(items_3_4) == false);
			Assert.IsTrue(items.IsNotEquals(items_3_4) == true);


			items = new int[]
			{
				1,
				2,
				3,
				4,
				5
			};

			Assert.IsTrue(items.IsEquals(items_1_2) == false);
			Assert.IsTrue(items.IsNotEquals(items_1_2) == true);

			Assert.IsTrue(items.IsEquals(items_3_4) == false);
			Assert.IsTrue(items.IsNotEquals(items_3_4) == true);


			////////////////////////////////////////////////
			////////////////////////////////////////////////
			////////////////////////////////////////////////


			items = new int[]
			{
				1,
				2,
				1,
				2
			};

			Assert.IsTrue(items.IsEquals(items_1_2, true) == true);
			Assert.IsTrue(items.IsNotEquals(items_1_2, true) == false);
			Assert.IsTrue(items.IsEquals(items_1_2, false) == false);
			Assert.IsTrue(items.IsNotEquals(items_1_2, false) == true);

			Assert.IsTrue(items.IsEquals(items_3_4, true) == false);
			Assert.IsTrue(items.IsNotEquals(items_3_4, true) == true);
			Assert.IsTrue(items.IsEquals(items_3_4, false) == false);
			Assert.IsTrue(items.IsNotEquals(items_3_4, false) == true);


			items = new int[]
			{
				3,
				3,
				4,
				4,
				4
			};

			Assert.IsTrue(items.IsEquals(items_1_2, true) == false);
			Assert.IsTrue(items.IsNotEquals(items_1_2, true) == true);
			Assert.IsTrue(items.IsEquals(items_1_2, false) == false);
			Assert.IsTrue(items.IsNotEquals(items_1_2, false) == true);

			Assert.IsTrue(items.IsEquals(items_3_4, true) == true);
			Assert.IsTrue(items.IsNotEquals(items_3_4, true) == false);
			Assert.IsTrue(items.IsEquals(items_3_4, false) == false);
			Assert.IsTrue(items.IsNotEquals(items_3_4, false) == true);


			////////////////////////////////////////////////
			////////////////////////////////////////////////
			////////////////////////////////////////////////


			items = new int[]
			{
				1,
				2
			};

			Assert.IsTrue(items.IsEquals(items_1_1_2_2, true) == true);
			Assert.IsTrue(items.IsNotEquals(items_1_1_2_2, true) == false);
			Assert.IsTrue(items.IsEquals(items_1_1_2_2, false) == false);
			Assert.IsTrue(items.IsNotEquals(items_1_1_2_2, false) == true);

			Assert.IsTrue(items.IsEquals(items_3_3_4_4_4, true) == false);
			Assert.IsTrue(items.IsNotEquals(items_3_3_4_4_4, true) == true);
			Assert.IsTrue(items.IsEquals(items_3_3_4_4_4, false) == false);
			Assert.IsTrue(items.IsNotEquals(items_3_3_4_4_4, false) == true);


			items = new int[]
			{
				3,
				4
			};

			Assert.IsTrue(items.IsEquals(items_1_1_2_2, true) == false);
			Assert.IsTrue(items.IsNotEquals(items_1_1_2_2, true) == true);
			Assert.IsTrue(items.IsEquals(items_1_1_2_2, false) == false);
			Assert.IsTrue(items.IsNotEquals(items_1_1_2_2, false) == true);

			Assert.IsTrue(items.IsEquals(items_3_3_4_4_4, true) == true);
			Assert.IsTrue(items.IsNotEquals(items_3_3_4_4_4, true) == false);
			Assert.IsTrue(items.IsEquals(items_3_3_4_4_4, false) == false);
			Assert.IsTrue(items.IsNotEquals(items_3_3_4_4_4, false) == true);


			////////////////////////////////////////////////
			////////////////////////////////////////////////
			////////////////////////////////////////////////


			items = new int[]
			{
				1,
				2
			};

			Assert.IsTrue(items.IsEquals(items_1_1_2_2, true) == true);
			Assert.IsTrue(items.IsNotEquals(items_1_1_2_2, true) == false);
			Assert.IsTrue(items.IsEquals(items_1_1_2_2, false) == false);
			Assert.IsTrue(items.IsNotEquals(items_1_1_2_2, false) == true);

			Assert.IsTrue(items.IsEquals(items_3_3_4_4_4, true) == false);
			Assert.IsTrue(items.IsNotEquals(items_3_3_4_4_4, true) == true);
			Assert.IsTrue(items.IsEquals(items_3_3_4_4_4, false) == false);
			Assert.IsTrue(items.IsNotEquals(items_3_3_4_4_4, false) == true);


			items = new int[]
			{
				3,
				4
			};

			Assert.IsTrue(items.IsEquals(items_1_1_2_2, true) == false);
			Assert.IsTrue(items.IsNotEquals(items_1_1_2_2, true) == true);
			Assert.IsTrue(items.IsEquals(items_1_1_2_2, false) == false);
			Assert.IsTrue(items.IsNotEquals(items_1_1_2_2, false) == true);

			Assert.IsTrue(items.IsEquals(items_3_3_4_4_4, true) == true);
			Assert.IsTrue(items.IsNotEquals(items_3_3_4_4_4, true) == false);
			Assert.IsTrue(items.IsEquals(items_3_3_4_4_4, false) == false);
			Assert.IsTrue(items.IsNotEquals(items_3_3_4_4_4, false) == true);


			////////////////////////////////////////////////
			////////////////////////////////////////////////
			////////////////////////////////////////////////


			items = new int[]
			{
				1,
				2
			};

			Assert.IsTrue(items.IsEquals(items_1_2_2, true) == true);
			Assert.IsTrue(items.IsNotEquals(items_1_2_2, true) == false);
			Assert.IsTrue(items.IsEquals(items_1_2_2, false) == false);
			Assert.IsTrue(items.IsNotEquals(items_1_2_2, false) == true);


			items = new int[]
			{
				3,
				4
			};

			Assert.IsTrue(items.IsEquals(items_1_1_2_2, true) == false);
			Assert.IsTrue(items.IsNotEquals(items_1_1_2_2, true) == true);
			Assert.IsTrue(items.IsEquals(items_1_1_2_2, false) == false);
			Assert.IsTrue(items.IsNotEquals(items_1_1_2_2, false) == true);

			Assert.IsTrue(items.IsEquals(items_3_3_4_4_4, true) == true);
			Assert.IsTrue(items.IsNotEquals(items_3_3_4_4_4, true) == false);
			Assert.IsTrue(items.IsEquals(items_3_3_4_4_4, false) == false);
			Assert.IsTrue(items.IsNotEquals(items_3_3_4_4_4, false) == true);
		}
	}

	class TestItem
	{
		public int GroupId { get; set; }

		public string Value { get; set; }

		public TestItem(
			int groupId,
			string value)
		{
			GroupId = groupId;
			Value = value;
		}
	}

	[TestMethod]
	public void ToGroupsGroupByTest()
	{
		// 生成测试数据：
		var items = new List<TestItem>();
		{
			var maxItemGroupId = 100;
			for (var groupIndex = 0;
				groupIndex < 100;
				groupIndex++)
			{
				var itemsCountInGroup = new Random().Next(100);
				for (var itemIndex = 0;
					itemIndex < itemsCountInGroup;
					itemIndex++)
				{
					var itemGroupId = new Random().Next(maxItemGroupId);
					var item = new TestItem(
						itemGroupId,
						itemGroupId.ToString());
					{ }
					items.Add(item);
				}
			}
		}

		// 开始测试：
		var itemGroups = items.ToGroupsBy((item) => item.GroupId);
		var itemsCount = 0;
		{
			Assert.IsTrue(itemGroups?.Length > 0);
			foreach (var itemGroup in itemGroups)
			{
				itemsCount += itemGroup.Count;
				if (itemGroup.Count > 0)
				{
					var itemGroupId = itemGroup[0].GroupId;
					foreach (var item in itemGroup)
					{
						// !!!
						Assert.IsTrue(item.GroupId == itemGroupId);
						// !!!
					}
				}
			}
		}
		// !!!
		Assert.IsTrue(itemsCount == items.Count);
		// !!!
	}
}
