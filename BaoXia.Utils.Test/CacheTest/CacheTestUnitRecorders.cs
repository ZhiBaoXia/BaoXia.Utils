using System;

namespace BaoXia.Utils.Test.CacheTest
{
        public class CacheTestUnitRecorders
        {
                public string? TestName { get; set; }

                public int TestsCount { get; set; }

                public DateTime TestsBeginTime { get; set; } = DateTime.MaxValue;

                public DateTime TestsEndTime { get; set; } = DateTime.MinValue;

                public double TestsMilliseconds { get; set; }

                public double TestMilliseconds_Avg
                {
                        get
                        {
                                if (this.TestsCount > 0)
                                {
                                        return this.TestsMilliseconds / this.TestsCount;
                                }
                                return 0;
                        }
                }

                public double TestMilliseconds_Min { get; set; } = double.MaxValue;

                public double TestMilliseconds_Max { get; set; } = double.MinValue;


                public void AddRecorder(CacheTestUnitRecorder recorder)
                {
                        lock (this)
                        {

                                this.TestName = recorder.Name;

                                this.TestsCount++;

                                if (this.TestsBeginTime > recorder.TestBeginTime)
                                {
                                        this.TestsBeginTime = recorder.TestBeginTime;
                                }
                                if (this.TestsEndTime < recorder.TestEndTime)
                                {
                                        this.TestsEndTime = recorder.TestEndTime;
                                }

                                var testMilliseconds = recorder.TestMilliseconds;
                                this.TestsMilliseconds += testMilliseconds;


                                if (this.TestMilliseconds_Min > testMilliseconds)
                                {
                                        this.TestMilliseconds_Min = testMilliseconds;
                                }
                                if (this.TestMilliseconds_Max < testMilliseconds)
                                {
                                        this.TestMilliseconds_Max = testMilliseconds;
                                }
                        }
                }

                public void LogTestsInfo()
                {
                        var testQPS = 0.0;
                        var testsDurationSecondsw = (this.TestsEndTime - this.TestsBeginTime).TotalSeconds;
                        if (testsDurationSecondsw != 0)
                        {
                                testQPS = (double)this.TestsCount / testsDurationSecondsw;
                        }

                        var testReport
                                = "\r\n" + "////////////////////////////////////////////////"
                                + "\r\n" + "// " + this.TestName
                                + "\r\n" + "////////////////////////////////////////////////"
                                + "\r\n" + "QPS" + "\t" + testQPS.ToString("F0")
                                + "\r\n" + "测试次数" + "\t" + this.TestsCount
                                + "\r\n" + "合计耗时（s）" + "\t" + (this.TestsMilliseconds / 1000.0).ToString("F2")
                                + "\r\n" + "平均耗时（ms）" + "\t" + (this.TestMilliseconds_Avg).ToString("F4")
                                + "\r\n" + "最少耗时（ms）" + "\t" + (this.TestMilliseconds_Min).ToString("F4")
                                + "\r\n" + "最多耗时（ms）" + "\t" + (this.TestMilliseconds_Max).ToString("F4")
                                + "\r\n";
                        // !!!
                        System.Diagnostics.Debug.WriteLine(testReport);
                        // !!!
                }


        }
}
