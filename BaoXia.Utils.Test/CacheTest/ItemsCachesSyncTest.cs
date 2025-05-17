using BaoXia.Utils.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Test.CacheTest;

[TestClass]
public class ItemsCachesSyncTest
{
	class TestItemWithIdAndIdentity
	{
		public int Id { get; set; }

		public string Identity { get; set; } = default!;
	}

	[TestMethod]
	public void SyncTest()
	{
		ItemsCache<int, TestItemWithIdAndIdentity, bool> itemsWithIdKey
			= new(
				(itemId, isUpdateByAnotherCache) =>
				{
					return new()
					{
						Id = itemId,
						Identity = itemId.ToString()
					};
				},
				null,
				null,
				null);
		ItemsCache<string, TestItemWithIdAndIdentity, bool> itemsWithIdentityKey
			= new(
				(itemIdentity, isUpdateByAnotherCache) =>
				{
					return new()
					{
						Id = int.Parse(itemIdentity),
						Identity = itemIdentity!
					};
				},
				null,
				null,
				null);

		itemsWithIdKey.ToDidItemCacheUpdated
			= (itemId, lastItem, currentItem, isUpdateByAnotherCache) =>
			{
				if (isUpdateByAnotherCache)
				{
					return;
				}
				if (currentItem != null)
				{
					itemsWithIdentityKey.Set(
						currentItem.Identity,
						currentItem,
						true);
				}
				else if (lastItem != null)
				{
					itemsWithIdentityKey.Remove(
						lastItem.Identity,
						true);
				}
			};
		itemsWithIdentityKey.ToDidItemCacheUpdated
			= (itemIdentity, lastItem, currentItem, isUpdateByAnotherCache) =>
			{
				if (isUpdateByAnotherCache)
				{
					return;
				}

				if (currentItem != null)
				{
					itemsWithIdKey.Set(
						currentItem.Id,
						currentItem,
						true);
				}
				else if (lastItem != null)
				{
					itemsWithIdKey.Remove(
						lastItem.Id,
						true);
				}
			};

		var testsCount = 100;
		var testTasksCount = 10;
		int currentTestNumber;
		var testTasks = new List<Task>();

		////////////////////////////////////////////////
		// 1/3，插入测试：
		////////////////////////////////////////////////
		currentTestNumber = 0;
		testTasks.Clear();
		for (var testTaskIndex = 0;
			testTaskIndex < testTasksCount;
			testTaskIndex++)
		{
			testTasks.Add(Task.Run(() =>
			{
				while (true)
				{
					var itemId = Interlocked.Increment(ref currentTestNumber);
					if (itemId > testsCount)
					{
						break;
					}
					if (itemId % 2 == 0)
					{
						itemsWithIdKey.Get(itemId, false);
					}
					else
					{
						itemsWithIdentityKey.Get(itemId.ToString(), false);
					}
				}
			}));
		}
		Task.WaitAll([.. testTasks]);
		// !!!
		Assert.AreEqual(testsCount, itemsWithIdKey.Count);
		Assert.AreEqual(testsCount, itemsWithIdentityKey.Count);
		// !!!


		////////////////////////////////////////////////
		// 2/3，更新测试：
		////////////////////////////////////////////////
		currentTestNumber = 0;
		testTasks.Clear();
		for (var testTaskIndex = 0;
			testTaskIndex < testTasksCount;
			testTaskIndex++)
		{
			testTasks.Add(Task.Run(() =>
			{
				while (true)
				{
					var itemId = Interlocked.Increment(ref currentTestNumber);
					if (itemId > testsCount)
					{
						break;
					}
					if (itemId % 2 == 0)
					{
						itemsWithIdKey.Get(itemId, false);
					}
					else
					{
						itemsWithIdentityKey.Get(itemId.ToString(), false);
					}
				}
			}));
		}
		Task.WaitAll([.. testTasks]);
		// !!!
		Assert.AreEqual(testsCount, itemsWithIdKey.Count);
		Assert.AreEqual(testsCount, itemsWithIdentityKey.Count);
		// !!!


		////////////////////////////////////////////////
		// 2/3，删除测试：
		////////////////////////////////////////////////
		currentTestNumber = 0;
		testTasks.Clear();
		for (var testTaskIndex = 0;
			testTaskIndex < testTasksCount;
			testTaskIndex++)
		{
			testTasks.Add(Task.Run(() =>
			{
				while (true)
				{
					var itemId = Interlocked.Increment(ref currentTestNumber);
					if (itemId > testsCount)
					{
						break;
					}
					if (itemId % 2 == 0)
					{
						itemsWithIdKey.Remove(itemId, false);
					}
					else
					{
						itemsWithIdentityKey.Remove(itemId.ToString(), false);
					}
				}
			}));
		}
		Task.WaitAll([.. testTasks]);
		// !!!
		Assert.AreEqual(0, itemsWithIdKey.Count);
		Assert.AreEqual(0, itemsWithIdentityKey.Count);
		// !!!
	}
}
