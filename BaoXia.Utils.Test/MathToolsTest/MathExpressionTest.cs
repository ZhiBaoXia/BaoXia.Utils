using BaoXia.Utils.MathTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BaoXia.Utils.Test.MathToolsTest
{
        [TestClass]
        public class MathExpressionTest
        {
                class TestItem
                {
                        public string? MathExpression { get; set; }

                        public int OperatorsCount
                        {
                                get
                                {
                                        if (this.MathExpression?.Length > 0)
                                        {
                                                return this.MathExpression.Replace(" ", null).Length;
                                        }
                                        return 0;
                                }
                        }

                        public double? CalculationResult { get; set; }
                }
                static readonly TestItem[] TestItems_ForOperatorsCount = new TestItem[]
                   {
                        new TestItem
                        {
                                MathExpression = "1 +1",
                                CalculationResult = 2
                        },
                        new TestItem
                        {
                                MathExpression = " 1 + 2 + 3",
                                CalculationResult = 6
                        },
                        new TestItem
                        {
                                MathExpression = " 1 * 2 + 3",
                                CalculationResult = 5
                        },
                        new TestItem
                        {
                                MathExpression = " 1 + 2 * 3",
                                CalculationResult = 7
                        },
                        new TestItem
                        {
                                MathExpression = " 1 + 2 * 3+4",
                                CalculationResult = 11
                        },
                        new TestItem
                        {
                                MathExpression = "(1 +  2) + 3",
                                CalculationResult = 6
                        },
                        new TestItem
                        {
                                MathExpression = "1 + ( 2 + 3)",
                                CalculationResult = 6
                        },
                        new TestItem
                        {
                                MathExpression = "1 + ( 2 + 3) +  4",
                                CalculationResult = 10
                        },
                        new TestItem
                        {
                                MathExpression = "(1 +  2) * 3 +  4",
                                CalculationResult = 13
                        },
                        new TestItem
                        {
                                MathExpression = "1 + ( 2 * 3) +  4",
                                CalculationResult = 11
                        },
                        new TestItem
                        {
                                MathExpression = "1 +2 + (3 *  4)",
                                CalculationResult = 15
                        },
                        new TestItem
                        {
                                MathExpression = "(1 +  2) * 3 *  4",
                                CalculationResult = 36
                        },
                        new TestItem
                        {
                                MathExpression = "1 * ( 2 * 3) *  4",
                                CalculationResult = 24
                        },
                        new TestItem
                        {
                                MathExpression = "1 *2 * (3 *  4)",
                                CalculationResult = 24
                        },
                        new TestItem
                        {
                                MathExpression = "1 + (2 + (3 *  4)) % 2",
                                CalculationResult = 1
                        },
                        new TestItem
                        {
                                MathExpression = "1 + (2 + (3 *  5)) % 2",
                                CalculationResult = 2
                        }
                   };

                static readonly TestItem[] TestItems_WithCalculation = new TestItem[]
                {
                        new TestItem
                        {
                                MathExpression = "0==1",
                                CalculationResult = 0
                        },
                        new TestItem
                        {
                                MathExpression = "0!=1",
                                CalculationResult = 1
                        },
                        new TestItem
                        {
                                MathExpression = "2==2",
                                CalculationResult = 1
                        },
                        new TestItem
                        {
                                MathExpression = "1 +1",
                                CalculationResult = 2
                        },
                        new TestItem
                        {
                                MathExpression = " 1 + 2 + 3",
                                CalculationResult = 6
                        },
                        new TestItem
                        {
                                MathExpression = " 1 * 2 + 3",
                                CalculationResult = 5
                        },
                        new TestItem
                        {
                                MathExpression = " 1 + 2 * 3",
                                CalculationResult = 7
                        },
                        new TestItem
                        {
                                MathExpression = " 1 + 2 * 3+4",
                                CalculationResult = 11
                        },
                        new TestItem
                        {
                                MathExpression = "(1 +  2) + 3",
                                CalculationResult = 6
                        },
                        new TestItem
                        {
                                MathExpression = "1 + ( 2 + 3)",
                                CalculationResult = 6
                        },
                        new TestItem
                        {
                                MathExpression = "1 + ( 2 + 3) +  4",
                                CalculationResult = 10
                        },
                        new TestItem
                        {
                                MathExpression = "(1 +  2) * 3 +  4",
                                CalculationResult = 13
                        },
                        new TestItem
                        {
                                MathExpression = "1 + ( 2 * 3) +  4",
                                CalculationResult = 11
                        },
                        new TestItem
                        {
                                MathExpression = "1 +2 + (3 *  4)",
                                CalculationResult = 15
                        },
                        new TestItem
                        {
                                MathExpression = "(1 +  2) * 3 *  4",
                                CalculationResult = 36
                        },
                        new TestItem
                        {
                                MathExpression = "1 * ( 2 * 3) *  4",
                                CalculationResult = 24
                        },
                        new TestItem
                        {
                                MathExpression = "1 *2 * (3 *  4)",
                                CalculationResult = 24
                        },
                        new TestItem
                        {
                                MathExpression = "1 + (2 + (3 *  4)) % 2",
                                CalculationResult = 1
                        },
                        new TestItem
                        {
                                MathExpression = "1 + (2 + (3 *  5)) % 2",
                                CalculationResult = 2
                        },
                        new TestItem
                        {
                                MathExpression = "1 + (2 + (3 *  5)) % 2 * 3 - 4 / 2 + 15",
                                CalculationResult = 17
                        },
                        new TestItem
                        {
                                MathExpression = "1 + (2 + (3 *  5)) % (2 * 3) - 4 / 2 * 3 + 15",
                                CalculationResult = 15
                        },
                        new TestItem
                        {
                                MathExpression = "1 + (2 + (3 *  4)) % (1+5 * 6+7)*(8*9 + 10 - 11) - 12 / 13 * 14  + 15",
                                CalculationResult = 997.076923077
                        },
                        new TestItem
                        {
                                MathExpression = "1 < 2",
                                CalculationResult = 1
                        },
                        new TestItem
                        {
                                MathExpression = "1 + 2 < 3 == 0",
                                CalculationResult = 1
                        },
                        new TestItem
                        {
                                MathExpression = "1 + 2 <= 3 == 1",
                                CalculationResult = 1
                        },
                        new TestItem
                        {
                                MathExpression = "1 + 2 + 3 == 3 + 3",
                                CalculationResult = 1
                        },
                        new TestItem
                        {
                                MathExpression = "1 + 2 + 3 == 3 * 2",
                                CalculationResult = 1
                        }
                };

                static readonly TestItem[] TestItems_WithCompareOperator = new TestItem[]
                {
                        // 测试 “<”：
                        //new TestItem
                        //{
                        //        MathExpression = "0.0 < 1.0",
                        //        CalculationResult = 1.0
                        //},
                        //new TestItem
                        //{
                        //        MathExpression = "1.0 < 0.0",
                        //        CalculationResult = 0.0
                        //},
                        
                        // 测试 “<=”：
                        new TestItem
                        {
                                MathExpression = "0.0 <= 1.0",
                                CalculationResult = 1.0
                        },
                        new TestItem
                        {
                                MathExpression = "1.0 <= 1.0",
                                CalculationResult = 1.0
                        },
                        new TestItem
                        {
                                MathExpression = "1.0 <= 0.0",
                                CalculationResult = 0.0
                        },

                        // 测试 “==”：
                        new TestItem
                        {
                                MathExpression = "1.0 == 1.0",
                                CalculationResult = 1.0
                        },
                        new TestItem
                        {
                                MathExpression = "0.0 == 1.0",
                                CalculationResult = 0.0
                        },

                        // 测试 “!=”：
                        new TestItem
                        {
                                MathExpression = "0.0 != 1.0",
                                CalculationResult = 1.0
                        },
                        new TestItem
                        {
                                MathExpression = "1.0 != 1.0",
                                CalculationResult = 0.0
                        },

                        // 测试 “>=”：
                        new TestItem
                        {
                                MathExpression = "0.0 >= 1.0",
                                CalculationResult = 0.0
                        },
                        new TestItem
                        {
                                MathExpression = "1.0 >= 1.0",
                                CalculationResult = 1.0
                        },
                        new TestItem
                        {
                                MathExpression = "1.0 >= 0.0",
                                CalculationResult = 1.0
                        },

                        // 测试 “>”：
                        new TestItem
                        {
                                MathExpression = "0.0 > 1.0",
                                CalculationResult = 0.0
                        },
                        new TestItem
                        {
                                MathExpression = "1.0 > 0.0",
                                CalculationResult = 1.0
                        },
                };

                [TestMethod]
                public void GetOperatorsFromStringTest()
                {
                        foreach (var testItem in TestItems_ForOperatorsCount)
                        {
                                var operators
                                        = MathExpression.GetOperatorsFromString(
                                                testItem.MathExpression,
                                                0,
                                                true,
                                                StringComparison.OrdinalIgnoreCase,
                                                out _);
                                {
                                        Assert.IsTrue(operators?.Count == testItem?.OperatorsCount);
                                }
                        }
                }

                [TestMethod]
                public void ParseTest_WithCalculation()
                {
                        foreach (var testItem in TestItems_WithCalculation)
                        {
                                var calculationResult
                                        = MathExpression.Parse(
                                                testItem.MathExpression,
                                                out _,
                                                null,
                                                true,
                                                null);
                                {
                                        Assert.IsTrue(System.Math.Abs(calculationResult!.Number!.Value - testItem!.CalculationResult!.Value) <= 0.001);
                                }
                        }
                }


                [TestMethod]
                public void ParseTest_WithCompareOperator()
                {
                        foreach (var testItem in TestItems_WithCompareOperator)
                        {
                                var calculationResult
                                        = MathExpression.Parse(
                                                testItem.MathExpression,
                                                out _,
                                                null,
                                                true,
                                                null);
                                if (calculationResult?.Number != null
                                        && testItem.CalculationResult != null)
                                {
                                        Assert.IsTrue(System.Math.Abs(calculationResult.Number.Value - testItem.CalculationResult.Value) <= 0.001);
                                }
                                else
                                {
                                        Assert.Fail();
                                }
                        }
                }
        }
}
