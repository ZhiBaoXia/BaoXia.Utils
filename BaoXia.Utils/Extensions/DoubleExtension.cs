using BaoXia.Utils.Constants;
using System;

namespace BaoXia.Utils.Extensions
{
        public static class DoubleExtension
        {
                ////////////////////////////////////////////////
                // @类方法
                ////////////////////////////////////////////////

                #region 类方法


                public static double TryParse(string? doubleString, double defaultValue = 0.0)
                {
                        if (double.TryParse(doubleString, out var number))
                        {
                                return number;
                        }
                        return defaultValue;
                }

                /// <summary>
                /// 获取当前双精度数的小数位数。
                /// </summary>
                /// <param name="doubleValue">指定的双精度数。</param>
                /// <returns>指定双精度数双精度数的小数位数。</returns>
                public static int GetDecimalDigits(this Double doubleValue)
                {

                        // 相关资料：
                        // https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/standard-numeric-format-strings
                        // https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/custom-numeric-format-strings
                        // 默认 ( "G" ) 格式
                        // “G”或“g”	常规	结果:更紧凑的定点表示法或科学记数法。
                        // 受以下类型支持：所有数值类型。
                        // 精度说明符：有效位数。
                        // 默认值精度说明符：具体取决于数值类型。
                        // 更多信息：常规（“G”）格式说明符。
                        var doubleValueString = doubleValue.ToString("G");
                        var dotIndex = doubleValueString.IndexOf(".");
                        var doubleValueDecimalDigits = 0;
                        if (dotIndex >= 0)
                        {
                                doubleValueDecimalDigits = (doubleValueString.Length - (dotIndex + 1));
                        }
                        return doubleValueDecimalDigits;
                }

                public static float ToFloatWithDecimalPrecision(
                        this double doubleA,
                        int decimalPrecision = -1)
                {
                        if (decimalPrecision >= 0)
                        {
                                doubleA = Math.Round(doubleA, decimalPrecision);
                        }
                        return Convert.ToSingle(doubleA);
                }

                public static double ToDoubleWithDecimalPrecision(
                        this double doubleA,
                        int decimalPrecision = -1)
                {
                        if (decimalPrecision >= 0)
                        {
                                doubleA = Math.Round(doubleA, decimalPrecision);
                        }
                        return doubleA;
                }

                public static int CompareTo(
                        this double doubleA,
                        double doubleB,
                        int decimalPrecision,
                        DecimalProcessType decimalProcessType = DecimalProcessType.Round,
                        MidpointRounding midpointRounding = MidpointRounding.ToEven)
                {
                        if (decimalPrecision >= 0)
                        {
                                switch (decimalProcessType)
                                {
                                        case DecimalProcessType.Round:
                                                {
                                                        doubleA = Math.Round(doubleA, decimalPrecision, midpointRounding);
                                                        doubleB = Math.Round(doubleB, decimalPrecision, midpointRounding);
                                                }
                                                break;
                                        case DecimalProcessType.Floor:
                                                {
                                                        doubleA = Math.Floor(doubleA);
                                                        doubleB = Math.Floor(doubleB);
                                                }
                                                break;
                                        case DecimalProcessType.Ceiling:
                                                {
                                                        doubleA = Math.Ceiling(doubleA);
                                                        doubleB = Math.Ceiling(doubleB);
                                                }
                                                break;
                                }
                        }
                        return doubleA.CompareTo(doubleB);
                }

                #endregion
        }
}
