using System;
using System.Collections.Generic;

namespace BaoXia.Utils.Extensions;

public static class StringsExtension
{
	public static bool IsContains(
		this IEnumerable<string> strings,
		string? key,
		out int keyIndexInStrings,
		StringComparison comparisonType)
	{
		keyIndexInStrings = -1;

		if (key?.Length > 0)
		{
			var keyIndex = 0;
			foreach (var str in strings)
			{
				if (str.Equals(key, comparisonType))
				{
					// !!!
					keyIndexInStrings = keyIndex;
					// !!!
					return true;
				}
				keyIndex++;
			}
		}
		return false;
	}

	public static bool IsNotContains(
		this IEnumerable<string> strings,
		string? key,
		out int keyIndexInStrings,
		StringComparison comparisonType)
	{
		return !StringsExtension.IsContains(
			strings,
			key,
			out keyIndexInStrings,
			comparisonType);
	}

	public static bool IsContains(
		this IEnumerable<string> strings,
		string? key,
		StringComparison comparisonType)
	{
		return StringsExtension.IsContains(
			strings,
			key,
			out _,
			comparisonType);
	}

	public static bool IsNotContains(
		this IEnumerable<string> strings,
		string? key,
		StringComparison comparisonType)
	{
		return !StringsExtension.IsContains(
			strings,
			key,
			comparisonType);
	}

	public static bool IsContains(
		this IEnumerable<string> strings,
		IEnumerable<string>? keys,
		StringComparison comparisonType,
		bool isNullEqualsEmpty = true)
	{
		if (keys != null)
		{
			foreach (var key in keys)
			{
				if (strings.IsContains(
					key,
					comparisonType) != true)
				{
					return false;
				}
			}
			return true;
		}
		else if (isNullEqualsEmpty == true)
		{
			var stringsEnumerator = strings.GetEnumerator();
			if (stringsEnumerator != null)
			{
				stringsEnumerator.Reset();
				if (stringsEnumerator.MoveNext() == false)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool IsNotContains(
		this IEnumerable<string> strings,
		IEnumerable<string>? keys,
		StringComparison comparisonType,
		bool isNullEqualsEmpty = true)
	{
		return !StringsExtension.IsContains(
			strings,
			keys,
			comparisonType,
			isNullEqualsEmpty);
	}

	public static bool IsEquals(
		this IEnumerable<string> strings,
		IEnumerable<string>? keys,
		StringComparison comparisonType,
		bool isIgnoreSameItems = false,
		bool isNullEqualsEmpty = true)
	{
		if (keys != null)
		{
			if (isIgnoreSameItems)
			{
				List<int>? strIndexesMatched = null;
				var keysCount = 0;
				foreach (var key in keys)
				{
					if (strings.IsContains(
						key,
						out var strIndexMatched,
						comparisonType) != true)
					{
						return false;
					}
					// !!!
					keysCount++;
					// !!!
					strIndexesMatched ??= [];
					strIndexesMatched.Add(strIndexMatched);
				}
				foreach (var str in strings)
				{
					keysCount--;
				}
				if (keysCount < 0)
				{
					if (strIndexesMatched != null)
					{
						var strIndex = 0;
						foreach (var str in strings)
						{
							if (strIndexesMatched.Contains(strIndex) != true)
							{
								if (keys.IsContains(
									str,
									comparisonType) != true)
								{
									return false;
								}
								// !!!
								strIndex++;
								// !!!
							}
						}
						return true;
					}
					return false;
				}
			}
			else
			{
				var keysCount = 0;
				foreach (var key in keys)
				{
					if (strings.IsContains(
						key,
						comparisonType) != true)
					{
						return false;
					}
					// !!!
					keysCount++;
					// !!!
				}
				foreach (var str in strings)
				{
					keysCount--;
				}
				if (keysCount != 0)
				{
					return false;
				}
			}
			return true;
		}
		else if (isNullEqualsEmpty == true)
		{
			var stringsEnumerator = strings.GetEnumerator();
			if (stringsEnumerator != null)
			{
				stringsEnumerator.Reset();
				if (stringsEnumerator.MoveNext() == false)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool IsNotEquals(
		this IEnumerable<string> strings,
		IEnumerable<string>? keys,
		StringComparison comparisonType,
		bool isIgnoreSameItems = false,
		bool isNullEqualsEmpty = true)
	{
		return !StringsExtension.IsEquals(
			strings,
			keys,
			comparisonType,
			isIgnoreSameItems,
			isNullEqualsEmpty);
	}

	public static List<int> TryToInts(
		this IEnumerable<string> strings,
		int parseStringsCountMax = 0,
		int intsCountMax = 0)
	{
		var ints = new List<int>();
		var parseStringsCount = 0;
		foreach (var str in strings)
		{
			if (str?.Length > 0
				&& int.TryParse(str, out var intValue) == true)
			{
				ints.Add(intValue);
				if (intsCountMax > 0
					&& ints.Count >= intsCountMax)
				{
					break;
				}
			}

			parseStringsCount++;
			if (parseStringsCountMax > 0
				&& parseStringsCount >= parseStringsCountMax)
			{
				break;
			}
		}
		return ints;
	}

	public static List<float> TryToFloats(
		this IEnumerable<string> strings,
		int parseStringsCountMax = 0,
		int floatsCountMax = 0)
	{
		var floats = new List<float>();
		var parseStringsCount = 0;
		foreach (var str in strings)
		{
			if (str?.Length > 0
				&& float.TryParse(str, out var floatValue) == true)
			{
				floats.Add(floatValue);
				if (floatsCountMax > 0
					&& floats.Count >= floatsCountMax)
				{
					break;
				}
			}

			parseStringsCount++;
			if (parseStringsCountMax > 0
				&& parseStringsCount >= parseStringsCountMax)
			{
				break;
			}
		}
		return floats;
	}

	public static List<double> TryToDoubles(
		this IEnumerable<string> strings,
		int parseStringsCountMax = 0,
		int doublesCountMax = 0)
	{
		var doubles = new List<double>();
		var parseStringsCount = 0;
		foreach (var str in strings)
		{
			if (str?.Length > 0
				&& double.TryParse(str, out var doubleValue) == true)
			{
				doubles.Add(doubleValue);
				if (doublesCountMax > 0
					&& doubles.Count >= doublesCountMax)
				{
					break;
				}
			}

			parseStringsCount++;
			if (parseStringsCountMax > 0
				&& parseStringsCount >= parseStringsCountMax)
			{
				break;
			}
		}
		return doubles;
	}

	public static string? ToStringWithSeparator(
		this IEnumerable<string>? strings,
		string? separator)
	{
		return StringUtil.StringWithStrings(
			strings, 
			separator,
			true);
	}
}
