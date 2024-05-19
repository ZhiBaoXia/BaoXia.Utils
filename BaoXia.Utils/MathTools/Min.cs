using System;
using System.Collections.Generic;

namespace BaoXia.Utils.MathTools
{
        public static class Min
        {
                public static NumberType? OfList<NumberType>(
                        Func<NumberType?, NumberType?, NumberType?> toGetMinItem,
                        IEnumerable<NumberType>? items)
                {
                        NumberType? maxItem = default;
                        if (toGetMinItem != null
                                && items != null)
                        {
                                foreach (var item in items)
                                {
                                        maxItem = toGetMinItem(maxItem, item);
                                }
                        }
                        return maxItem;
                }

                public static NumberType? Of<NumberType>(
                        Func<NumberType?, NumberType?, NumberType?> toGetMaxItem,
                        params NumberType[] items)
                {
                        return Min.OfList(
                                toGetMaxItem,
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

                        var minValue = nuint.MaxValue;
                        foreach (var value in values)
                        {
                                if (minValue > value)
                                {
                                        minValue = value;
                                }
                        }
                        return minValue;
                }

                public static nuint Of(params nuint[]? values)
                {
                        return Min.OfList(values);
                }

                public static ulong OfList(IEnumerable<ulong>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        var minValue = ulong.MaxValue;
                        foreach (var value in values)
                        {
                                if (minValue > value)
                                {
                                        minValue = value;
                                }
                        }
                        return minValue;
                }

                public static ulong Of(params ulong[]? values)
                {
                        return Min.OfList(values);
                }

                public static uint OfList(IEnumerable<uint>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        var minValue = uint.MaxValue;
                        foreach (var value in values)
                        {
                                if (minValue > value)
                                {
                                        minValue = value;
                                }
                        }
                        return minValue;
                }

                public static uint Of(params uint[]? values)
                {
                        return Min.OfList(values);
                }

                public static ushort OfList(IEnumerable<ushort>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        var minValue = ushort.MaxValue;
                        foreach (var value in values)
                        {
                                if (minValue > value)
                                {
                                        minValue = value;
                                }
                        }
                        return minValue;
                }

                public static ushort Of(params ushort[]? values)
                {
                        return Min.OfList(values);
                }

                public static float OfList(IEnumerable<float>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        var minValue = float.MaxValue;
                        foreach (var value in values)
                        {
                                if (minValue > value)
                                {
                                        minValue = value;
                                }
                        }
                        return minValue;
                }

                public static float Of(params float[]? values)
                {
                        return Min.OfList(values);
                }

                public static nint OfList(IEnumerable<nint>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        var minValue = nint.MaxValue;
                        foreach (var value in values)
                        {
                                if (minValue > value)
                                {
                                        minValue = value;
                                }
                        }
                        return minValue;
                }

                public static nint Of(params nint[]? values)
                {
                        return Min.OfList(values);
                }

                public static sbyte OfList(IEnumerable<sbyte>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        var minValue = sbyte.MaxValue;
                        foreach (var value in values)
                        {
                                if (minValue > value)
                                {
                                        minValue = value;
                                }
                        }
                        return minValue;
                }

                public static sbyte Of(params sbyte[]? values)
                {
                        return Min.OfList(values);
                }

                public static int OfList(IEnumerable<int>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        var minValue = int.MaxValue;
                        foreach (var value in values)
                        {
                                if (minValue > value)
                                {
                                        minValue = value;
                                }
                        }
                        return minValue;
                }

                public static int Of(params int[]? values)
                {
                        return Min.OfList(values);
                }

                public static short OfList(IEnumerable<short>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        var minValue = short.MaxValue;
                        foreach (var value in values)
                        {
                                if (minValue > value)
                                {
                                        minValue = value;
                                }
                        }
                        return minValue;
                }

                public static short Of(params short[]? values)
                {
                        return Min.OfList(values);
                }

                public static double OfList(IEnumerable<double>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        var minValue = double.MaxValue;
                        foreach (var value in values)
                        {
                                if (minValue > value)
                                {
                                        minValue = value;
                                }
                        }
                        return minValue;
                }

                public static double Of(params double[]? values)
                {
                        return Min.OfList(values);
                }

                public static decimal OfList(IEnumerable<decimal>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        var minValue = decimal.MaxValue;
                        foreach (var value in values)
                        {
                                if (minValue > value)
                                {
                                        minValue = value;
                                }
                        }
                        return minValue;
                }

                public static decimal Of(params decimal[]? values)
                {
                        return Min.OfList(values);
                }

                public static byte OfList(IEnumerable<byte>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        var minValue = byte.MaxValue;
                        foreach (var value in values)
                        {
                                if (minValue > value)
                                {
                                        minValue = value;
                                }
                        }
                        return minValue;
                }

                public static byte Of(params byte[]? values)
                {
                        return Min.OfList(values);
                }

                public static long OfList(IEnumerable<long>? values)
                {
                        if (values == null)
                        {
                                return 0;
                        }

                        var minValue = long.MaxValue;
                        foreach (var value in values)
                        {
                                if (minValue > value)
                                {
                                        minValue = value;
                                }
                        }
                        return minValue;
                }

                public static long Of(params long[]? values)
                {
                        return Min.OfList(values);
                }
        }
}