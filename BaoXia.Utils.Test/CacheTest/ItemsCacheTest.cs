using BaoXia.Utils.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Test.CacheTest
{
	[TestClass]
	public class ItemsCacheTest
	{
		////////////////////////////////////////////////
		// @测试方法
		////////////////////////////////////////////////

		[TestMethod]
		public void AddUpdateAndQueryTest_IntKey()
		{
			var itemId = 0;
			var tester = new ItemsCacheTester<int>(
				"整数Key",
				() =>
				{
					return Interlocked.Increment(ref itemId);
				});
			// !!!
			tester.AddUpdateAndQueryTest();
			// !!!
		}

		[TestMethod]
		public void GetSameKeyAtSameTimeTest_IntKey()
		{
			var itemId = 0;
			var tester = new ItemsCacheTester<int>(
				"整数Key",
				() =>
				{
					return Interlocked.Increment(ref itemId);
				});
			// !!!
			// tester.GetSameKeyAtSameTimeTest();
			// !!!
		}

		[TestMethod]
		public void TryGetTest_IntKey()
		{
			var itemId = 0;
			var tester = new ItemsCacheTester<int>(
				"整数Key",
				() =>
				{
					return Interlocked.Increment(ref itemId);
				});
			// !!!
			tester.TryGetTest();
			// !!!
		}

		[TestMethod]
		public void AddUpdateAndQueryTest_StringKey()
		{
			var itemId = 0;
			var tester = new ItemsCacheTester<string>(
				"字符串Key",
				() =>
				{
					return Interlocked.Increment(ref itemId).ToString();
				});
			// !!!
			tester.AddUpdateAndQueryTest();
			// !!!
		}

		[TestMethod]
		public void GetSameKeyAtSameTimeTest_StringKey()
		{
			var itemId = 0;
			var tester = new ItemsCacheTester<string>(
				"字符串Key",
				() =>
				{
					return Interlocked.Increment(ref itemId).ToString();
				});
			// !!!
			// tester.GetSameKeyAtSameTimeTest();
			// !!!
		}

		[TestMethod]
		public void TryGetTest_StringKey()
		{
			var itemId = 0;
			var tester = new ItemsCacheTester<string>(
				"字符串Key",
				() =>
				{
					return Interlocked.Increment(ref itemId).ToString();
				});
			// !!!
			tester.TryGetTest();
			// !!!
		}


		[TestMethod]
		public void InitializeWithNullTest()
		{
			var itemCache = new ItemsCache<string, object, object>(
				(key, _) =>
				{
					return true;
				},
				null,
				null,
				null);
			Assert.IsTrue(true);
		}

		[TestMethod]
		public async Task AutoRemoveNoneReadCacheItems_AutoRemove()
		{
			var itemsCache = new ItemsCache<int, DateTime, object>(
				(id, _) =>
				{
					return DateTime.Now;
				},
				null,
				() => 1.0);
			for (var itemId = 1;
				itemId <= 100;
				itemId++)
			{
				itemsCache.Add(itemId, DateTime.Now);
			}

			await Task.Delay(3000);

			Assert.IsTrue(itemsCache.Count == 0);
		}

		[TestMethod]
		public async Task AutoRemoveNoneReadCacheItems_AutoUpdateReadTime()
		{
			var itemsCache = new ItemsCache<int, DateTime, object>(
				(id, _) =>
				{
					return DateTime.Now;
				},
				null,
				() => 2.0);

			const int itemsCount = 100;
			for (var itemId = 1;
				itemId <= itemsCount;
				itemId++)
			{
				itemsCache.Add(itemId, DateTime.Now);
			}

			var retainTaskCancelSource = new CancellationTokenSource();
			_ = Task.Run(async () =>
			{
				while (!retainTaskCancelSource.Token.IsCancellationRequested)
				{
					await Task.Delay(1000);
					for (var itemId = 1;
						itemId <= itemsCount;
						itemId++)
					{
						_ = itemsCache.Get(itemId, null);
					}
				}
			});

			await Task.Delay(3000);
			retainTaskCancelSource.Cancel();

			Assert.IsTrue(itemsCache.Count == itemsCount);
		}
	}
}
