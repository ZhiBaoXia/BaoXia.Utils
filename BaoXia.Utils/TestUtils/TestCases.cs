using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaoXia.Utils.TestUtils
{
        public class TestCases
        {
                ////////////////////////////////////////////////
                // @静态变量
                ////////////////////////////////////////////////

                #region 静态变量

#pragma warning disable CA2211 // 非常量字段应当不可见
                public static List<TestCase> AllTestCases = new();
#pragma warning restore CA2211 // 非常量字段应当不可见

                #endregion

                ////////////////////////////////////////////////
                // @类方法
                ////////////////////////////////////////////////

                #region 类方法

                public static async Task Start(
                        string testsCatalogName,
                        string testsName,
                        double delaySeconds = 3.0,
                        Action<string>? toOutput = null,
                        Func<string?>? toInput = null)
                {
                        const string Intent_1 = "   ";

                        Console.WriteLine("////////////////////////////////////////////////");
                        Console.WriteLine("// " + testsCatalogName + "/" + testsName);
                        Console.WriteLine("////////////////////////////////////////////////");

                        Console.WriteLine();

                        for (;
                                delaySeconds > 0.0;
                                delaySeconds -= 1.0)
                        {
                                var dotString = string.Empty;
                                for (var dotIndex = 0.0;
                                        dotIndex < delaySeconds;
                                        dotIndex++)
                                {
                                        dotString += "。";
                                }

                                Console.WriteLine($"倒计时，{delaySeconds} 秒" + dotString);
                                await Task.Delay(1000);
                        }

                        Console.WriteLine();

                        var allTestCases = TestCases.AllTestCases;
                        var testCasesTestSuccessful = new List<TestCase>();
                        var testCasesTestFailed = new List<TestCase>();
                        foreach (var testCase in allTestCases)
                        {
                                if (testCase.Test())
                                {
                                        testCasesTestSuccessful.Add(testCase);
                                }
                                else
                                {
                                        testCasesTestFailed.Add(testCase);
                                }
                        }

                        var testResult
                                = "\r\n"
                                + "\r\n"
                                + "////////////////////////////////////////////////\r\n"
                                + $"// 全部测试结束，\r\n"
                                + $"// 共 {allTestCases.Count} 个测试用例，\r\n"
                                + $"// 测试通过率 {(100.0 * testCasesTestSuccessful.Count / allTestCases.Count):F0}%。\r\n"
                                + "////////////////////////////////////////////////\r\n"
                                + "\r\n"
                                + $"测试通过 {testCasesTestSuccessful.Count} 个：\r\n";
                        foreach (var testCase in testCasesTestSuccessful)
                        {
                                testResult += Intent_1 + "√ " + testCase.Name + "\r\n";
                        }

                        testResult += $"\r\n";

                        testResult += $"测试失败 {testCasesTestFailed.Count} 个：\r\n";
                        foreach (var testCase in testCasesTestFailed)
                        {
                                testResult += Intent_1 + "× " + testCase.Name + "\r\n";
                        }

                        if (toOutput != null)
                        {
                                toOutput(testResult);
                        }
                        else
                        {
                                Console.Write(testResult);
                        }

                        Console.WriteLine("输入回车以结束测试程序。");

                        if (toInput != null)
                        {
                                toInput();
                        }
                        else
                        {
                                Console.ReadLine();
                        }
                }

                #endregion
        }
}

