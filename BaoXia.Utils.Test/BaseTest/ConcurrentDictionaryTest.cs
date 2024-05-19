using BaoXia.Utils.Extensions;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Test.BaseTest
{
        // [TestClass]
        public class ConcurrentDictionaryTest
        {
                // [TestMethod]
                public void AllKeysToArrayTest()
                {
                        const int testItemsCount = 1000000;
                        var random = new System.Random();
                        var randomNextMax = testItemsCount * 100;

                        var concurrentDictionary = new ConcurrentDictionary<string, int>();
                        for (var i = 0;
                            i < testItemsCount;
                            i++)
                        {
                                var key = random.Next(randomNextMax).ToString();
                                concurrentDictionary.AddOrUpdateWithNewValue(
                                    key,
                                    random.Next(randomNextMax));
                        }


                        Task.Run(() =>
                        {
                                while (true)
                                {
                                        var key = random.Next(randomNextMax).ToString();

                                        var beginTime = DateTime.Now;
                                        {
                                                concurrentDictionary.TryGetValue(key, out _);
                                        }
                                        var endTime = DateTime.Now;
                                        var durationMilliseconds = (endTime - beginTime).Milliseconds;

                                        //
                                        System.Diagnostics.Debug.WriteLine(
                                            DateTime.Now.MillisecondsFrom1970()
                                            + "，并发获取CurrentDictionary元素，耗时：" + durationMilliseconds + "毫秒。");
                                        //
                                }
                        });

                        int testType = 2;

                        Task.Run(() =>
                        {
                                while (testType == 1)
                                {
                                        var beginTime = DateTime.Now;
                                        {
                                                System.Diagnostics.Debug.WriteLine("获取：concurrentDictionary.Keys，Begin。");
                                                var allKeys = concurrentDictionary.Keys;
                                                System.Diagnostics.Debug.WriteLine("获取：concurrentDictionary.Keys，End。");
                                        }
                                        var endTime = DateTime.Now;
                                        var durationMilliseconds = (endTime - beginTime).Milliseconds;

                                        System.Diagnostics.Debug.WriteLine(
                                DateTime.Now.MillisecondsFrom1970()
                                + "，ConcurrentDictionaryTest，concurrentDictionary.Keys耗时："
                                + durationMilliseconds
                                + " 毫秒。");

                                        Thread.Sleep(300);
                                }
                        });


                        Task.Run(() =>
                        {
                                while (testType == 2)
                                {
                                        var beginTime = DateTime.Now;
                                        {
                                                System.Diagnostics.Debug.WriteLine("获取：concurrentDictionary.forEach，Begin。");
                                                foreach (var item in concurrentDictionary)
                                                { }
                                                System.Diagnostics.Debug.WriteLine("获取：concurrentDictionary.forEach，End。");
                                        }
                                        var endTime = DateTime.Now;
                                        var durationMilliseconds2 = (endTime - beginTime).Milliseconds;

                                        System.Diagnostics.Debug.WriteLine(
                                DateTime.Now.MillisecondsFrom1970()
                                + "，ConcurrentDictionaryTest，concurrentDictionary.forEach耗时："
                                + durationMilliseconds2
                                + " 毫秒。");

                                        Thread.Sleep(300);
                                }
                        });

                        while (true)
                        {
                                Thread.Sleep(1000);
                        }
                }
        }
}
