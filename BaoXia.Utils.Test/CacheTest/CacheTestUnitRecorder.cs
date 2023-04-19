using System;

namespace BaoXia.Utils.Test.CacheTest
{
        public class CacheTestUnitRecorder
        {
                protected static int CacheTestItemId;
                public static readonly object CacheTestItemIdLocker = new object();


                public int Id { get; set; }

                public string? Name { get; set; }

                public string? TestDataId { get; set; }

                public DateTime TestBeginTime { get; set; }

                public DateTime TestEndTime { get; set; }

                public double TestMillisecondsNow
                {
                        get
                        {
                                return (DateTime.Now - this.TestBeginTime).TotalMilliseconds;
                        }
                }

                public double TestMilliseconds
                {
                        get
                        {
                                return (this.TestEndTime - this.TestBeginTime).TotalMilliseconds;
                        }
                }

                public CacheTestUnitRecorder(string name)
                {
                        int id = 0;
                        lock (CacheTestItemIdLocker)
                        {
                                id = ++CacheTestItemId;
                        }
                        this.Id = id;

                        this.Name = name;
                }


                protected void LogTestInfo(String stepName, String testDataId)
                {
                        var now = DateTime.Now;
                        var taskName = this.Name;
                        var taskId = this.Id;
                        var threadId = System.Environment.CurrentManagedThreadId;
                        var durationMilliseconds = this.TestMillisecondsNow;

                        string logContent
                                = now.ToString("HH-mm-ss-ffffff")
                                + "\t"
                                + "任务-" + taskName + "-" + taskId
                                + "\t"
                                + "线程-" + threadId
                                + "\t"
                                + "操作-" + stepName
                                + "\t"
                                + "内容-" + testDataId
                                + "\t"
                                + "耗时-" + durationMilliseconds + "ms";
                        // !!!
                        // System.Diagnostics.Debug.WriteLine(logContent);
                        // !!!
                }

                public void BeginTest(String testDataId)
                {
                        this.TestBeginTime = DateTime.Now;
                        this.TestDataId = testDataId;
                        this.LogTestInfo("测试开始", testDataId);
                }

                public void EndTest()
                {
                        this.TestEndTime = DateTime.Now;
                        this.LogTestInfo("测试结束", this.TestDataId!);
                }
        }
}
