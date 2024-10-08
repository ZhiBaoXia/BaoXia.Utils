﻿using System;

namespace BaoXia.Utils;

public class NumberUtil
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static uint MinOne(uint number)
	{
		if (number >= 1)
		{
			return number;
		}
		return 1;
	}

	public static int MinOne(int number)
	{
		if (number >= 1)
		{
			return number;
		}
		return 1;
	}

	public static ulong MinOne(ulong number)
	{
		if (number >= 1)
		{
			return number;
		}
		return 1;
	}

	public static long MinOne(long number)
	{
		if (number >= 1)
		{
			return number;
		}
		return 1;
	}

	public static float MinOne(float number)
	{
		if (number >= 1)
		{
			return number;
		}
		return 1;
	}

	public static double MinOne(double number)
	{
		if (number >= 1)
		{
			return number;
		}
		return 1;
	}

	public static decimal MinOne(decimal number)
	{
		if (number >= 1)
		{
			return number;
		}
		return 1;
	}

	public static uint FirstNotZero(params uint?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue != 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static int FirstNotZero(params int?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue != 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static ulong FirstNotZero(params ulong?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue != 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static long FirstNotZero(params long?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue != 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static float FirstNotZero(params float?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue != 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static double FirstNotZero(params double?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue != 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static decimal FirstNotZero(params decimal?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue != 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}


	public static uint FirstGreaterZero(params uint?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue > 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static int FirstGreaterZero(params int?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue > 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static ulong FirstGreaterZero(params ulong?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue > 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static long FirstGreaterZero(params long?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue > 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static float FirstGreaterZero(params float?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue > 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static double FirstGreaterZero(params double?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue > 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static decimal FirstGreaterZero(params decimal?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue > 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}


	public static uint FirstLessZero(params uint?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue < 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static int FirstLessZero(params int?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue < 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static ulong FirstLessZero(params ulong?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue < 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static long FirstLessZero(params long?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue < 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static float FirstLessZero(params float?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue < 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static double FirstLessZero(params double?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue < 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static decimal FirstLessZero(params decimal?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue < 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static uint UIntFromHexString(
		string? hexString,
		uint defaultValue = 0x0)
	{
		if (string.IsNullOrWhiteSpace(hexString))
		{
			return 0;
		}

		if (hexString.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
		{
			hexString = hexString[2..];
		}

		if (hexString.Length > 8)
		{
			hexString = hexString[..8];
		}

		if (uint.TryParse(hexString,
			System.Globalization.NumberStyles.HexNumber,
			null,
			out uint hexNumber))
		{
			return hexNumber;
		}
		return defaultValue;
	}

	public static uint? MaxOf(params uint[]? numbers)
	{
		if (numbers == null
			|| numbers.Length < 1)
		{
			return null;
		}

		uint maxNumber = uint.MinValue;
		foreach (var nubmer in numbers)
		{
			if (maxNumber < nubmer)
			{
				maxNumber = nubmer;
			}
		}
		return maxNumber;
	}

	public static int? MaxOf(params int[]? numbers)
	{
		if (numbers == null
			|| numbers.Length < 1)
		{
			return null;
		}

		int maxNumber = int.MinValue;
		foreach (var nubmer in numbers)
		{
			if (maxNumber < nubmer)
			{
				maxNumber = nubmer;
			}
		}
		return maxNumber;
	}

	public static ulong? MaxOf(params ulong[]? numbers)
	{
		if (numbers == null
			|| numbers.Length < 1)
		{
			return null;
		}

		ulong maxNumber = ulong.MinValue;
		foreach (var nubmer in numbers)
		{
			if (maxNumber < nubmer)
			{
				maxNumber = nubmer;
			}
		}
		return maxNumber;
	}

	public static long? MaxOf(params long[]? numbers)
	{
		if (numbers == null
			|| numbers.Length < 1)
		{
			return null;
		}

		long maxNumber = long.MinValue;
		foreach (var nubmer in numbers)
		{
			if (maxNumber < nubmer)
			{
				maxNumber = nubmer;
			}
		}
		return maxNumber;
	}

	public static float? MaxOf(params float[]? numbers)
	{
		if (numbers == null
			|| numbers.Length < 1)
		{
			return null;
		}

		float maxNumber = float.MinValue;
		foreach (var nubmer in numbers)
		{
			if (maxNumber < nubmer)
			{
				maxNumber = nubmer;
			}
		}
		return maxNumber;
	}

	public static double? MaxOf(params double[]? numbers)
	{
		if (numbers == null
			|| numbers.Length < 1)
		{
			return null;
		}

		double maxNumber = double.MinValue;
		foreach (var nubmer in numbers)
		{
			if (maxNumber < nubmer)
			{
				maxNumber = nubmer;
			}
		}
		return maxNumber;
	}

	public static decimal? MaxOf(params decimal[]? numbers)
	{
		if (numbers == null
			|| numbers.Length < 1)
		{
			return null;
		}

		decimal maxNumber = decimal.MinValue;
		foreach (var nubmer in numbers)
		{
			if (maxNumber < nubmer)
			{
				maxNumber = nubmer;
			}
		}
		return maxNumber;
	}


	public static uint? MinOf(params uint[]? numbers)
	{
		if (numbers == null
			|| numbers.Length < 1)
		{
			return null;
		}

		uint minNumber = uint.MaxValue;
		foreach (var nubmer in numbers)
		{
			if (minNumber > nubmer)
			{
				minNumber = nubmer;
			}
		}
		return minNumber;
	}

	public static int? MinOf(params int[]? numbers)
	{
		if (numbers == null
			|| numbers.Length < 1)
		{
			return null;
		}

		int minNumber = int.MaxValue;
		foreach (var nubmer in numbers)
		{
			if (minNumber > nubmer)
			{
				minNumber = nubmer;
			}
		}
		return minNumber;
	}

	public static ulong? MinOf(params ulong[]? numbers)
	{
		if (numbers == null
			|| numbers.Length < 1)
		{
			return null;
		}

		ulong minNumber = ulong.MaxValue;
		foreach (var nubmer in numbers)
		{
			if (minNumber > nubmer)
			{
				minNumber = nubmer;
			}
		}
		return minNumber;
	}

	public static long? MinOf(params long[]? numbers)
	{
		if (numbers == null
			|| numbers.Length < 1)
		{
			return null;
		}

		long minNumber = long.MaxValue;
		foreach (var nubmer in numbers)
		{
			if (minNumber > nubmer)
			{
				minNumber = nubmer;
			}
		}
		return minNumber;
	}

	public static float? MinOf(params float[]? numbers)
	{
		if (numbers == null
			|| numbers.Length < 1)
		{
			return null;
		}

		float minNumber = float.MaxValue;
		foreach (var nubmer in numbers)
		{
			if (minNumber > nubmer)
			{
				minNumber = nubmer;
			}
		}
		return minNumber;
	}

	public static double? MinOf(params double[]? numbers)
	{
		if (numbers == null
			|| numbers.Length < 1)
		{
			return null;
		}

		double minNumber = double.MaxValue;
		foreach (var nubmer in numbers)
		{
			if (minNumber > nubmer)
			{
				minNumber = nubmer;
			}
		}
		return minNumber;
	}

	public static decimal? MinOf(params decimal[]? numbers)
	{
		if (numbers == null
			|| numbers.Length < 1)
		{
			return null;
		}

		decimal minNumber = decimal.MaxValue;
		foreach (var nubmer in numbers)
		{
			if (minNumber > nubmer)
			{
				minNumber = nubmer;
			}
		}
		return minNumber;
	}

	#endregion
}