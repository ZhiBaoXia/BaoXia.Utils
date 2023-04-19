using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace BaoXia.Utils.Test.CacheTest
{
        [TestClass]
        public class AsyncListsCacheTest
        {
                ////////////////////////////////////////////////
                // @测试方法
                ////////////////////////////////////////////////

                [TestMethod]
                public void AddUpdateAndQueryTest_IntKey()
                {
                        var listId = 0;
                        var cacheListTestAsync = new AsyncListsCacheTester<int>(
                                "整数Key",
                                () =>
                                {
                                        return Interlocked.Increment(ref listId);
                                });
                        // !!!
                        cacheListTestAsync.AddUpdateAndQueryTest();
                        // !!!
                }

                [TestMethod]
                public void AddUpdateAndQueryTest_StringKey()
                {
                        var listId = 0;
                        var cacheListTestAsync = new AsyncListsCacheTester<string>(
                                "字符串Key",
                                () =>
                                {
                                        return Interlocked.Increment(ref listId).ToString();
                                });
                        // !!!
                        cacheListTestAsync.AddUpdateAndQueryTest();
                        // !!!
                }
        }
}
