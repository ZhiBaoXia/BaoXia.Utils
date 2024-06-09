using System;
using System.Collections.Generic;

namespace BaoXia.Utils.MathTools
{
	public static class Max
	{
		public static NumberType? OfList<NumberType>(
			Func<NumberType?, NumberType?, NumberType?> toGetMaxItem,
			IEnumerable<NumberType>? items)
		{
			NumberType? maxItem = default;
			if (toGetMaxItem != null
				&& items != null)
			{
				foreach (var item in items)
				{
					maxItem = toGetMaxItem(maxItem, item);
				}
			}
			return maxItem;
		}

		public static NumberType? Of<NumberType>(
			Func<NumberType?, NumberType?, NumberType?> toGetMaxItem,
			params NumberType[] items)
		{
			return Max.OfList(
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

			var maxValue = nuint.MinValue;
			foreach (var value in values)
			{
				if (maxValue < value)
				{
					maxValue = value;
				}
			}
			return maxValue;
		}

		public static nuint Of(params nuint[]? values)
		{
			return Max.OfList(values);
		}

		public static ulong OfList(IEnumerable<ulong>? values)
		{
			if (values == null)
			{
				return 0;
			}

			var maxValue = ulong.MinValue;
			foreach (var value in values)
			{
				if (maxValue < value)
				{
					maxValue = value;
				}
			}
			return maxValue;
		}

		public static ulong Of(params ulong[]? values)
		{
			return Max.OfList(values);
		}

		public static uint OfList(IEnumerable<uint>? values)
		{
			if (values == null)
			{
				return 0;
			}

			var maxValue = uint.MinValue;
			foreach (var value in values)
			{
				if (maxValue < value)
				{
					maxValue = value;
				}
			}
			return maxValue;
		}

		public static uint Of(params uint[]? values)
		{
			return Max.OfList(values);
		}

		public static ushort OfList(IEnumerable<ushort>? values)
		{
			if (values == null)
			{
				return 0;
			}

			var maxValue = ushort.MinValue;
			foreach (var value in values)
			{
				if (maxValue < value)
				{
					maxValue = value;
				}
			}
			return maxValue;
		}

		public static ushort Of(params ushort[]? values)
		{
			return Max.OfList(values);
		}

		public static float OfList(IEnumerable<float>? values)
		{
			if (values == null)
			{
				return 0;
			}

			var maxValue = float.MinValue;
			foreach (var value in values)
			{
				if (maxValue < value)
				{
					maxValue = value;
				}
			}
			return maxValue;
		}

		public static float Of(params float[]? values)
		{
			return Max.OfList(values);
		}

		public static nint OfList(IEnumerable<nint>? values)
		{
			if (values == null)
			{
				return 0;
			}

			var maxValue = nint.MinValue;
			foreach (var value in values)
			{
				if (maxValue < value)
				{
					maxValue = value;
				}
			}
			return maxValue;
		}

		public static nint Of(params nint[]? values)
		{
			return Max.OfList(values);
		}

		public static sbyte OfList(IEnumerable<sbyte>? values)
		{
			if (values == null)
			{
				return 0;
			}

			var maxValue = sbyte.MinValue;
			foreach (var value in values)
			{
				if (maxValue < value)
				{
					maxValue = value;
				}
			}
			return maxValue;
		}

		public static sbyte Of(params sbyte[]? values)
		{
			return Max.OfList(values);
		}

		public static int OfList(IEnumerable<int>? values)
		{
			if (values == null)
			{
				return 0;
			}

			var maxValue = int.MinValue;
			foreach (var value in values)
			{
				if (maxValue < value)
				{
					maxValue = value;
				}
			}
			return maxValue;
		}

		public static int Of(params int[]? values)
		{
			return Max.OfList(values);
		}

		public static short OfList(IEnumerable<short>? values)
		{
			if (values == null)
			{
				return 0;
			}

			var maxValue = short.MinValue;
			foreach (var value in values)
			{
				if (maxValue < value)
				{
					maxValue = value;
				}
			}
			return maxValue;
		}

		public static short Of(params short[]? values)
		{
			return Max.OfList(values);
		}

		public static double OfList(IEnumerable<double>? values)
		{
			if (values == null)
			{
				return 0;
			}

			var maxValue = double.MinValue;
			foreach (var value in values)
			{
				if (maxValue < value)
				{
					maxValue = value;
				}
			}
			return maxValue;
		}

		public static double Of(params double[]? values)
		{
			return Max.OfList(values);
		}

		public static decimal OfList(IEnumerable<decimal>? values)
		{
			if (values == null)
			{
				return 0;
			}

			var maxValue = decimal.MinValue;
			foreach (var value in values)
			{
				if (maxValue < value)
				{
					maxValue = value;
				}
			}
			return maxValue;
		}

		public static decimal Of(params decimal[]? values)
		{
			return Max.OfList(values);
		}

		public static byte OfList(IEnumerable<byte>? values)
		{
			if (values == null)
			{
				return 0;
			}

			var maxValue = byte.MinValue;
			foreach (var value in values)
			{
				if (maxValue < value)
				{
					maxValue = value;
				}
			}
			return maxValue;
		}

		public static byte Of(params byte[]? values)
		{
			return Max.OfList(values);
		}

		public static long OfList(IEnumerable<long>? values)
		{
			if (values == null)
			{
				return 0;
			}

			var maxValue = long.MinValue;
			foreach (var value in values)
			{
				if (maxValue < value)
				{
					maxValue = value;
				}
			}
			return maxValue;
		}

		public static long Of(params long[]? values)
		{
			return Max.OfList(values);
		}
	}
}