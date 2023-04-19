using BaoXia.Utils.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Test.CacheTest
{
        [TestClass]
        public class AsyncItemsCacheTest
        {
                ////////////////////////////////////////////////
                // @测试方法
                ////////////////////////////////////////////////

                [TestMethod]
                public void AddUpdateAndQueryTest_IntKey()
                {
                        var itemId = 0;
                        var tester = new AsyncItemsCacheTester<int>(
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
                        var tester = new AsyncItemsCacheTester<int>(
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
                public async Task TryGetTest_IntKey()
                {
                        var itemId = 0;
                        var tester = new AsyncItemsCacheTester<int>(
                                "整数Key",
                                () =>
                                {
                                        return Interlocked.Increment(ref itemId);
                                });
                        // !!!
                        await tester.TryGetTest();
                        // !!!
                }

                [TestMethod]
                public void AddUpdateAndQueryTest_StringKey()
                {
                        var itemId = 0;
                        var tester = new AsyncItemsCacheTester<string>(
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
                        var tester = new AsyncItemsCacheTester<string>(
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
                public async Task TryGetTest_StringKey()
                {
                        var itemId = 0;
                        var tester = new AsyncItemsCacheTester<string>(
                                "字符串Key",
                                () =>
                                {
                                        return Interlocked.Increment(ref itemId).ToString();
                                });
                        // !!!
                        await tester.TryGetTest();
                        // !!!
                }

                [TestMethod]
                public void InitializeWithNullTest()
                {
                        var itemCache = new AsyncItemsCache<string, object, object>(
                                async (key, _) =>
                                {
                                        await Task.Delay(1);

                                        return null;
                                },
                                null,
                                null,
                                null);
                        Assert.IsTrue(true);
                }

                class TestItem
                {
                        public int Id { get; set; }
                        public DateTime CreateTime { get; set; }
                }

                [TestMethod]
                public async Task AutoRemoveNoneReadCacheItems_AutoRemove()
                {
                        var itemsCache = new AsyncItemsCache<int, TestItem, object>(
                                async (id, _) =>
                                {
                                        await Task.Delay(0);
                                        return new TestItem
                                        {
                                                Id = id,
                                                CreateTime = DateTime.Now
                                        };
                                },
                                null,
                                () => 1.0);
                        for (var itemId = 1;
                                itemId <= 100;
                                itemId++)
                        {
                                await itemsCache.GetAsync(itemId, null);
                        }

                        await Task.Delay(3000);

                        Assert.IsTrue(itemsCache.Count == 0);
                }

                [TestMethod]
                public async Task AutoRemoveNoneReadCacheItems_AutoUpdateReadTime()
                {
                        var itemsCache = new AsyncItemsCache<int, TestItem, object>(
                                async (id, _) =>
                                {
                                        await Task.Delay(0);
                                        return new TestItem
                                        {
                                                Id = id,
                                                CreateTime = DateTime.Now
                                        };
                                },
                                null,
                                () => 2.0);

                        const int itemsCount = 100;
                        for (var itemId = 1;
                                itemId <= itemsCount;
                                itemId++)
                        {
                                await itemsCache.GetAsync(itemId, null);
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
                                                _ = await itemsCache.GetAsync(itemId, null);
                                        }
                                }
                        });

                        await Task.Delay(3000);
                        retainTaskCancelSource.Cancel();

                        Assert.IsTrue(itemsCache.Count == itemsCount);
                }
        }
}
