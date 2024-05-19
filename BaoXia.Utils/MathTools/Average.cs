using System;
using System.Collections.Generic;

namespace BaoXia.Utils.MathTools
{
        public static class Average
        {
                public static NumberType? OfList<NumberType>(
                        Func<NumberType?, NumberType?, NumberType?> toGetSumOfItems,
                        Func<int, NumberType?, NumberType?> toGetAverageOfItems,
                        IEnumerable<NumberType> items)
                {
                        NumberType? itemsSum = default;
                        NumberType? itemsAverage = default;
                        int itemsCount = 0;
                        if (toGetSumOfItems != null
                                && toGetAverageOfItems != null
                                && items != null)
                        {
                                foreach (var item in items)
                                {
                                        itemsSum = toGetSumOfItems(itemsSum, item);
                                        itemsCount++;
                                }
                                itemsAverage
                                        = itemsCount > 0
                                        ? toGetAverageOfItems(itemsCount, itemsSum)
                                        : default;
                        }
                        return itemsAverage;
                }
                public static NumberType? Of<NumberType>(
                        Func<NumberType?, NumberType?, NumberType?> toGetSumOfItems,
                        Func<int, NumberType?, NumberType?> toGetAverageOfItems,
                        params NumberType[] items)
                {
                        return Average.OfList(
                                toGetSumOfItems,
                                toGetAverageOfItems,
                                items);
                }

                ////////////////////////////////////////////////
                ////////////////////////////////////////////////
                ////////////////////////////////////////////////

                public static nuint OfList(IEnumerable<nuint>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        nuint valuesSum = default;
                        nuint valuesCount = 0;
                        foreach (var value in values)
                        {
                                valuesSum += value;
                                valuesCount++;
                        }
                        if (valuesCount < 1)
                        {
                                return 0;
                        }
                        return valuesSum / valuesCount;
                }

                public static nuint Of(params nuint[]? values)
                {
                        return Average.OfList(values);
                }

                public static ulong OfList(IEnumerable<ulong>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        ulong valuesSum = default;
                        ulong valuesCount = 0;
                        foreach (var value in values)
                        {
                                valuesSum += value;
                                valuesCount++;
                        }
                        if (valuesCount < 1)
                        {
                                return 0;
                        }
                        return valuesSum / valuesCount;
                }

                public static ulong Of(params ulong[]? values)
                {
                        return Average.OfList(values);
                }

                public static uint OfList(IEnumerable<uint>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        uint valuesSum = default;
                        uint valuesCount = 0;
                        foreach (var value in values)
                        {
                                valuesSum += value;
                                valuesCount++;
                        }
                        if (valuesCount < 1)
                        {
                                return 0;
                        }
                        return valuesSum / valuesCount;
                }

                public static uint Of(params uint[]? values)
                {
                        return Average.OfList(values);
                }

                public static int OfList(IEnumerable<ushort>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        ushort valuesSum = default;
                        ushort valuesCount = 0;
                        foreach (var value in values)
                        {
                                valuesSum += value;
                                valuesCount++;
                        }
                        if (valuesCount < 1)
                        {
                                return 0;
                        }
                        return valuesSum / valuesCount;
                }

                public static int Of(params ushort[]? values)
                {
                        return Average.OfList(values);
                }

                public static float OfList(IEnumerable<float>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        float valuesSum = default;
                        float valuesCount = 0;
                        foreach (var value in values)
                        {
                                valuesSum += value;
                                valuesCount++;
                        }
                        if (valuesCount < 1)
                        {
                                return 0;
                        }
                        return valuesSum / valuesCount;
                }

                public static float Of(params float[]? values)
                {
                        return Average.OfList(values);
                }

                public static nint OfList(IEnumerable<nint>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        nint valuesSum = default;
                        nint valuesCount = 0;
                        foreach (var value in values)
                        {
                                valuesSum += value;
                                valuesCount++;
                        }
                        if (valuesCount < 1)
                        {
                                return 0;
                        }
                        return valuesSum / valuesCount;
                }

                public static nint Of(params nint[]? values)
                {
                        return Average.OfList(values);
                }

                public static int OfList(IEnumerable<sbyte>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        sbyte valuesSum = default;
                        sbyte valuesCount = 0;
                        foreach (var value in values)
                        {
                                valuesSum += value;
                                valuesCount++;
                        }
                        if (valuesCount < 1)
                        {
                                return 0;
                        }
                        return valuesSum / valuesCount;
                }

                public static int Of(params sbyte[]? values)
                {
                        return Average.OfList(values);
                }

                public static int OfList(IEnumerable<int>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        int valuesSum = default;
                        int valuesCount = 0;
                        foreach (var value in values)
                        {
                                valuesSum += value;
                                valuesCount++;
                        }
                        if (valuesCount < 1)
                        {
                                return 0;
                        }
                        return valuesSum / valuesCount;
                }

                public static int Of(params int[]? values)
                {
                        return Average.OfList(values);
                }

                public static int OfList(IEnumerable<short>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        short valuesSum = default;
                        short valuesCount = 0;
                        foreach (var value in values)
                        {
                                valuesSum += value;
                                valuesCount++;
                        }
                        if (valuesCount < 1)
                        {
                                return 0;
                        }
                        return valuesSum / valuesCount;
                }

                public static int Of(params short[]? values)
                {
                        return Average.OfList(values);
                }

                public static double OfList(IEnumerable<double>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        double valuesSum = default;
                        double valuesCount = 0;
                        foreach (var value in values)
                        {
                                valuesSum += value;
                                valuesCount++;
                        }
                        if (valuesCount < 1)
                        {
                                return 0;
                        }
                        return valuesSum / valuesCount;
                }

                public static double Of(params double[]? values)
                {
                        return Average.OfList(values);
                }

                public static decimal OfList(IEnumerable<decimal>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        decimal valuesSum = default;
                        decimal valuesCount = 0;
                        foreach (var value in values)
                        {
                                valuesSum += value;
                                valuesCount++;
                        }
                        if (valuesCount < 1)
                        {
                                return 0;
                        }
                        return valuesSum / valuesCount;
                }

                public static decimal Of(params decimal[]? values)
                {
                        return Average.OfList(values);
                }

                public static int OfList(IEnumerable<byte>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        byte valuesSum = default;
                        byte valuesCount = 0;
                        foreach (var value in values)
                        {
                                valuesSum += value;
                                valuesCount++;
                        }
                        if (valuesCount < 1)
                        {
                                return 0;
                        }
                        return valuesSum / valuesCount;
                }

                public static int Of(params byte[]? values)
                {
                        return Average.OfList(values);
                }

                public static long OfList(IEnumerable<long>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        long valuesSum = default;
                        long valuesCount = 0;
                        foreach (var value in values)
                        {
                                valuesSum += value;
                                valuesCount++;
                        }
                        if (valuesCount < 1)
                        {
                                return 0;
                        }
                        return valuesSum / valuesCount;
                }

                public static long Of(params long[]? values)
                {
                        return Average.OfList(values);
                }
        }
}
