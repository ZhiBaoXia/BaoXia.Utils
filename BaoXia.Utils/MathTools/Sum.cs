using System;
using System.Collections.Generic;

namespace BaoXia.Utils.MathTools
{
        public static class Sum
        {
                public static NumberType? OfList<NumberType>(
                        Func<NumberType?, NumberType?, NumberType?> toGetSumOfItems,
                        IEnumerable<NumberType> items)
                {
                        if (toGetSumOfItems == null)
                        {
                                return default;
                        }
                        if (items == null)
                        {
                                return default;
                        }

                        NumberType? itemsSum = default;
                        foreach (var item in items)
                        {
                                itemsSum = toGetSumOfItems(itemsSum, item);
                        }
                        return itemsSum;
                }

                public static NumberType? Of<NumberType>(
                        Func<NumberType?, NumberType?, NumberType?> toGetSumOfItems,
                        params NumberType[] items)
                {
                        return Sum.OfList(
                                toGetSumOfItems,
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

                        foreach (var value in values)
                        {
                                valuesSum += value;

                        }
                        return valuesSum;
                }

                public static nuint Of(params nuint[]? values)
                {
                        return Sum.OfList(values);
                }

                public static ulong OfList(IEnumerable<ulong>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        ulong valuesSum = default;

                        foreach (var value in values)
                        {
                                valuesSum += value;

                        }
                        return valuesSum;
                }

                public static ulong Of(params ulong[]? values)
                {
                        return Sum.OfList(values);
                }

                public static uint OfList(IEnumerable<uint>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        uint valuesSum = default;

                        foreach (var value in values)
                        {
                                valuesSum += value;

                        }
                        return valuesSum;
                }

                public static uint Of(params uint[]? values)
                {
                        return Sum.OfList(values);
                }

                public static int OfList(IEnumerable<ushort>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        ushort valuesSum = default;

                        foreach (var value in values)
                        {
                                valuesSum += value;

                        }
                        return valuesSum;
                }

                public static int Of(params ushort[]? values)
                {
                        return Sum.OfList(values);
                }

                public static float OfList(IEnumerable<float>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        float valuesSum = default;

                        foreach (var value in values)
                        {
                                valuesSum += value;

                        }
                        return valuesSum;
                }

                public static float Of(params float[]? values)
                {
                        return Sum.OfList(values);
                }

                public static nint OfList(IEnumerable<nint>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        nint valuesSum = default;

                        foreach (var value in values)
                        {
                                valuesSum += value;

                        }
                        return valuesSum;
                }

                public static nint Of(params nint[]? values)
                {
                        return Sum.OfList(values);
                }

                public static int OfList(IEnumerable<sbyte>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        sbyte valuesSum = default;

                        foreach (var value in values)
                        {
                                valuesSum += value;

                        }
                        return valuesSum;
                }

                public static int Of(params sbyte[]? values)
                {
                        return Sum.OfList(values);
                }

                public static int OfList(IEnumerable<int>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        int valuesSum = default;

                        foreach (var value in values)
                        {
                                valuesSum += value;

                        }
                        return valuesSum;
                }

                public static int Of(params int[]? values)
                {
                        return Sum.OfList(values);
                }

                public static int OfList(IEnumerable<short>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        short valuesSum = default;

                        foreach (var value in values)
                        {
                                valuesSum += value;

                        }
                        return valuesSum;
                }

                public static int Of(params short[]? values)
                {
                        return Sum.OfList(values);
                }

                public static double OfList(IEnumerable<double>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        double valuesSum = default;

                        foreach (var value in values)
                        {
                                valuesSum += value;

                        }
                        return valuesSum;
                }

                public static double Of(params double[]? values)
                {
                        return Sum.OfList(values);
                }

                public static decimal OfList(IEnumerable<decimal>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        decimal valuesSum = default;

                        foreach (var value in values)
                        {
                                valuesSum += value;

                        }
                        return valuesSum;
                }

                public static decimal Of(params decimal[]? values)
                {
                        return Sum.OfList(values);
                }

                public static int OfList(IEnumerable<byte>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        byte valuesSum = default;
                        foreach (var value in values)
                        {
                                valuesSum += value;

                        }
                        return valuesSum;
                }

                public static int Of(params byte[]? values)
                {
                        return Sum.OfList(values);
                }

                public static long OfList(IEnumerable<long>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        long valuesSum = default;
                        foreach (var value in values)
                        {
                                valuesSum += value;

                        }
                        return valuesSum;
                }

                public static long Of(params long[]? values)
                {
                        return Sum.OfList(values);
                }
        }
}
