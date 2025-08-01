﻿using BaoXia.Utils.Constants;
using BaoXia.Utils.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace BaoXia.Utils.Extensions;

/// <summary>
/// “String”扩展类。
/// </summary>
public static class StringExtension
{
	/// <summary>
	/// 从“指定的文件路径”处加载字符串，注意加载过程中会有同步的文件操作。
	/// </summary>
	/// <param name="filePath">指定的文件路径。</param>
	/// <param name="encoding">指定的文件编码，指定值为“null”时，默认使用“Encoding.UTF8”。</param>
	/// <returns>加载成功时，返回加载后的字符串，加载失败时，返回“空字符串”。</returns>
	public static string? LoadStringFromFilePath(
	    this string filePath,
	    Encoding? encoding = null)
	{
		string? str = null;
		if (filePath?.Length > 0)
		{
			encoding ??= Encoding.UTF8;

			str = System.IO.File.ReadAllText(filePath, encoding);
		}
		return str;
	}

	public static async Task<string?> LoadStringFromFilePathAsync(
	    this string filePath,
	    Encoding? encoding = null)
	{
		string? str = null;
		if (filePath?.Length > 0)
		{
			encoding ??= Encoding.UTF8;

			str = await System.IO.File.ReadAllTextAsync(filePath, encoding);
		}
		return str;
	}

	public static bool TryLoadStringFromFilePath(
	    this string filePath,
	    out string? stringRead,
	    Encoding? encoding = null)
	{
		stringRead = null;

		try
		{
			stringRead
				= LoadStringFromFilePath(
				filePath,
				encoding);
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static async Task<string?> TryLoadStringFromFilePathAsync(
	    this string filePath,
	    Encoding? encoding = null)
	{
		string? str;
		try
		{
			str = await TryLoadStringFromFilePathAsync(
				filePath,
				encoding);
		}
		catch
		{
			str = null;
		}
		return str;
	}

	public static string? TrimStart(
		this string originalString,
		string? trimString,
		StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
	{
		if (string.IsNullOrEmpty(trimString))
		{
			return originalString;
		}

		while (originalString.StartsWith(
			trimString,
			stringComparison))
		{
			originalString
				= originalString[trimString.Length..];
		}
		return originalString;
	}

	public static string? TrimStart(
		this string originalString,
		IEnumerable<string?>? trimStrings,
		StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
	{
		if (trimStrings == null)
		{
			return originalString;
		}

		var stringTrimed = originalString;
		bool isNeedRetrim;
		do
		{
			isNeedRetrim = false;
			foreach (var trimString in trimStrings)
			{
				if (stringTrimed?.Length > 0)
				{
					var lastStringTrimedLength = stringTrimed.Length;
					////////////////////////////////////////////////
					stringTrimed = TrimStart(
						stringTrimed!,
						trimString,
						stringComparison);
					////////////////////////////////////////////////
					// 替换成功后，需要重置Trim：
					if (stringTrimed?.Length != lastStringTrimedLength)
					{
						isNeedRetrim = true;
						break;
					}
				}
				else
				{
					break;
				}
			}
		} while (isNeedRetrim);
		return stringTrimed;
	}

	public static string? TrimEnd(
		this string originalString,
		string? trimString,
		StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
	{
		if (string.IsNullOrEmpty(trimString))
		{
			return originalString;
		}

		while (originalString.EndsWith(
			trimString,
			stringComparison))
		{
			originalString
				= originalString[..^trimString.Length];
		}
		return originalString;
	}

	public static string? TrimEnd(
		this string originalString,
		IEnumerable<string?>? trimStrings,
		StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
	{
		if (trimStrings == null)
		{
			return originalString;
		}

		var stringTrimed = originalString;
		bool isNeedRetrim;
		do
		{
			isNeedRetrim = false;
			foreach (var trimString in trimStrings)
			{
				if (stringTrimed?.Length > 0)
				{
					var lastStringTrimedLength = stringTrimed.Length;
					////////////////////////////////////////////////
					stringTrimed = TrimEnd(
						stringTrimed!,
						trimString,
						stringComparison);
					////////////////////////////////////////////////
					// 替换成功后，需要重置Trim：
					if (stringTrimed?.Length != lastStringTrimedLength)
					{
						isNeedRetrim = true;
						break;
					}
				}
				else
				{
					break;
				}
			}
		} while (isNeedRetrim);
		return stringTrimed;
	}

	public static string? Trim(
		this string originalString,
		string? trimString,
		StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
	{
		var stringTrimed =
			TrimStart(
			originalString,
			trimString,
			stringComparison);
		if (stringTrimed?.Length > 0)
		{
			stringTrimed =
				TrimEnd(
				stringTrimed,
				trimString,
				stringComparison);
		}
		return stringTrimed;
	}

	public static string? Trim(
		this string originalString,
		IEnumerable<string?>? trimStrings,
		StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
	{
		if (trimStrings == null)
		{
			return originalString;
		}

		var stringTrimed = TrimStart(
			originalString,
			trimStrings,
			stringComparison);
		if (stringTrimed?.Length > 0)
		{
			stringTrimed = TrimEnd(
				stringTrimed,
				trimStrings,
				stringComparison);
		}
		return stringTrimed;
	}

	/// <summary>
	/// 保存字符串到“指定的文件路径”处，注意保存过程中会有同步的文件操作。
	/// </summary>
	/// <param name="filePath">指定的文件路径。</param>
	/// <param name="encoding">指定的文件编码，指定值为“null”时，默认使用“Encoding.UTF8”。</param>
	/// <returns>保存成功时，返回：true，否则返回：false。</returns>
	public static bool SaveToFilePath(
		this string? str,
		string? filePath,
		Encoding? encoding = null)
	{
		if (filePath == null
			|| filePath.Length < 1)
		{
			return false;
		}

		encoding ??= Encoding.UTF8;

		System.IO.File.WriteAllText(
		    filePath,
		    str,
		    encoding);

		return true;
	}

	public static async Task<bool> SaveToFilePathAsync(
		this string? str,
		string? filePath,
		Encoding? encoding = null)
	{
		if (filePath == null
			|| filePath.Length < 1)
		{
			return false;
		}

		encoding ??= Encoding.UTF8;

		await System.IO.File.WriteAllTextAsync(
		    filePath,
		    str,
		    encoding);

		return true;
	}


	/// <summary>
	/// 获取指定字符串在当前字符串中出现的次数。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="key">要查找的字符串。</param>
	/// <param name="comparisonType">查找字符串时的比较参数，默认为“StringComparison.Ordinal”。</param>
	/// <param name="isKeyCharsOverlapEnable">目标关键字是否可以重叠，如：“999”是否算作包含两次的“99”，默认为“false”，不重叠。</param>
	/// <returns>指定字符串在当前字符串中出现的次数，类型为：int。</returns>
	public static int CountOfString(
	    this string? str,
	    string? key,
	    StringComparison comparisonType = StringComparison.Ordinal,
	    bool isKeyCharsOverlapEnable = false)
	{
		if (string.IsNullOrEmpty(str))
		{
			return 0;
		}
		if (string.IsNullOrEmpty(key))
		{
			return 0;
		}

		int keysCount = 0;
		var keyLength = key.Length;
		var lastKeyEndIndex = 0;
		while (true)
		{
			var indexOfKey = str.IndexOf(key, lastKeyEndIndex, comparisonType);
			if (indexOfKey >= 0)
			{
				keysCount++;
				if (isKeyCharsOverlapEnable)
				{
					lastKeyEndIndex = indexOfKey + 1;
				}
				else
				{
					lastKeyEndIndex = indexOfKey + keyLength;
				}
			}
			else
			{
				break;
			}
		}
		return keysCount;
	}

	/// <summary>
	/// 不区分大小写的获取指定字符串在当前字符串中出现的次数。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="key">要查找的字符串。</param>
	/// <returns>指定字符串在当前字符串中出现的次数，类型为：int。</returns>
	public static int CountOfStringIgnoreCase(
	    this string? str,
	    string? key)
	{
		return CountOfString(
			str,
			key,
			StringComparison.OrdinalIgnoreCase);
	}

	/// <summary>
	/// 获得指定匹配字符串，在当前字符串中的匹配进度值。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="matchValue">指定的匹配字符串。</param>
	/// <param name="comparisonType">查找字符串时的比较参数，默认为“StringComparison.OrdinalIgnoreCase”。</param>
	/// <param name="isMatchValueCharsOverlapEnable">目标字符串是否可以重叠，如：“999”是否算作包含两次的“99”，默认为“false”，不重叠。</param>
	/// <returns>指定匹配字符串，在当前字符串中的匹配进度值，范围：0.0 - 1.0 。</returns>
	public static double GetMatchProgressValueOf(
		this string? str,
		string? matchValue,
		StringComparison comparisonType = StringComparison.OrdinalIgnoreCase,
		bool isMatchValueCharsOverlapEnable = false)
	{
		if (string.IsNullOrEmpty(str)
		    || string.IsNullOrEmpty(matchValue))
		{
			return 0.0;
		}

		var searchKeysCount = str.CountOfString(
		    matchValue,
		    comparisonType,
		    isMatchValueCharsOverlapEnable);
		if (searchKeysCount < 0)
		{
			return 0;
		}
		var matchedProgressValue
		= (double)(matchValue.Length * searchKeysCount)
		/ str.Length;
		{ }
		return matchedProgressValue;
	}

	/// <summary>
	/// 返回指定字符串左侧指定长度的内容。
	/// </summary>
	/// <param name="str">指定的字符串。</param>
	/// <param name="leftSubstringLength">指定的要获取的左侧内容长度。</param>
	/// <returns>字符串中指定长度的左侧内容。</returns>
	public static string Left(
		this string? str,
		int leftSubstringLength)
	{
		if (string.IsNullOrEmpty(str)
			|| leftSubstringLength <= 0)
		{
			return String.Empty;
		}
		else if (str.Length >= leftSubstringLength)
		{
			return str[..leftSubstringLength];
		}
		return str;
	}

	/// <summary>
	/// 返回指定字符串右侧指定长度的内容。
	/// </summary>
	/// <param name="str">指定的字符串。</param>
	/// <param name="rightSubstringLength">指定的要获取的右侧内容长度。</param>
	/// <returns>字符串中指定长度的右侧内容。</returns>
	public static string Right(
		this string? str,
		int rightSubstringLength)
	{
		if (string.IsNullOrEmpty(str)
			|| rightSubstringLength <= 0)
		{
			return String.Empty;
		}
		else if (str.Length >= rightSubstringLength)
		{
			return str.Substring(
				str.Length - rightSubstringLength,
				rightSubstringLength);
		}
		return str;
	}

	/// <summary>
	/// 获取字符串中，指定关键字前的部分。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="keyword">指定的关键字。</param>
	/// <param name="startIndex">要开始查询的起始字符索引。</param>
	/// <param name="stringComparison">字符串的比较类型。</param>
	/// <returns>字符串中，指定关键字前的部分。</returns>
	public static string? SubstringBefore(
	    this string? str,
	    string? keyword,
	    int startIndex,
	    StringComparison stringComparison = StringComparison.Ordinal)
	{
		if (string.IsNullOrEmpty(str))
		{
			return str;
		}
		if (string.IsNullOrEmpty(keyword))
		{
			return null;
		}
		var indexOfKey = str.IndexOf(
			keyword,
			startIndex,
			stringComparison);
		if (indexOfKey < 0)
		{
			return null;
		}
		var substring = str[..indexOfKey];
		{
		}
		return substring;
	}

	/// <summary>
	/// 获取字符串中，指定关键字前的部分。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="keyword">指定的关键字。</param>
	/// <param name="stringComparison">字符串的比较类型。</param>
	/// <returns>字符串中，指定关键字前的部分。</returns>
	public static string? SubstringBefore(
	    this string? str,
	    string? keyword,
	    StringComparison stringComparison = StringComparison.Ordinal)
	{
		return SubstringBefore(
			str,
			keyword,
			0,
			stringComparison);
	}

	/// <summary>
	/// 获取字符串中，指定关键字后的部分。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="keyword">指定的关键字。</param>
	/// <param name="startIndex">要开始查询的起始字符索引。</param>
	/// <param name="stringComparison">字符串的比较类型。</param>
	/// <returns>字符串中，指定关键字后的部分。</returns>
	public static string? SubstringAfter(
		this string? str,
		string? keyword,
		int startIndex,
		StringComparison stringComparison = StringComparison.Ordinal)
	{
		if (string.IsNullOrEmpty(str))
		{
			return str;
		}
		if (string.IsNullOrEmpty(keyword))
		{
			return null;
		}
		var indexOfKey = str.IndexOf(
			keyword,
			startIndex,
			stringComparison);
		if (indexOfKey < 0)
		{
			return null;
		}
		var substring = str[(indexOfKey + keyword.Length)..];
		{ }
		return substring;
	}

	/// <summary>
	/// 获取字符串中，指定关键字后的部分。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="keyword">指定的关键字。</param>
	/// <param name="stringComparison">字符串的比较类型。</param>
	/// <returns>字符串中，指定关键字后的部分。</returns>
	public static string? SubstringAfter(
		this string? str,
		string? keyword,
		StringComparison stringComparison = StringComparison.Ordinal)
	{
		return SubstringAfter(
			str,
			keyword,
			0,
			stringComparison);
	}

	/// <summary>
	/// 获取指定两个字符串之间的字符串。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="beginKey">开始位置的字符串。</param>
	/// <param name="endKey">结束位置的正常。</param>
	/// <param name="comparisonType">查找字符串时的比较参数。</param>
	/// <returns>指定两个字符串之间的字符串。</returns>
	public static string? SubstringBetween(
	    this string? str,
	    string? beginKey,
	    string? endKey,
	    bool isSearchEndKeyFromRight = false,
	    StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
	{
		if (string.IsNullOrEmpty(str))
		{
			return null;
		}

		var beginIndex = 0;
		if (beginKey?.Length > 0)
		{
			var beginKeyIndex = str.IndexOf(beginKey, comparisonType);
			if (beginKeyIndex >= 0)
			{
				beginIndex = beginKeyIndex + beginKey.Length;
			}
			else
			{
				return null;
			}
		}

		var endIndex = str.Length;
		if (endKey?.Length > 0)
		{
			var endKeyIndex
			    = isSearchEndKeyFromRight
			    ? str.LastIndexOf(endKey, comparisonType)
			    : str.IndexOf(endKey, beginIndex, comparisonType);
			if (endKeyIndex >= 0)
			{
				endIndex = endKeyIndex;
			}
		}

		string? substring = null;
		if (endIndex > beginIndex)
		{
			substring = str[beginIndex..endIndex];
		}
		return substring;
	}

	/// <summary>
	/// 返回当前字符串中的“布尔值”。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <returns>当前字符中的“布尔值”。</returns>
	public static bool BoolValue(this string? str)
	{
		_ = bool.TryParse(str, out var value);
		{ }
		return value;
	}

	/// <summary>
	/// 返回当前字符串中的“整数值”。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <returns>当前字符中的“整数值”。</returns>
	public static int IntValue(this string? str)
	{
		_ = int.TryParse(str, out var value);
		{ }
		return value;
	}

	/// <summary>
	/// 返回当前字符串中的“浮点数值”。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <returns>当前字符中的“浮点数值”。</returns>
	public static float FloatValue(this string? str)
	{
		_ = float.TryParse(str, out var value);
		{ }
		return value;
	}

	/// <summary>
	/// 返回当前字符串中的“双精度值”。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <returns>当前字符中的“双精度值”。</returns>
	public static double DoubleValue(this string? str)
	{
		_ = double.TryParse(str, out var value);
		{ }
		return value;
	}

	/// <summary>
	/// 当前字符串对应的枚举值。
	/// </summary>
	/// <typeparam name="T">枚举类型。</typeparam>
	/// <param name="str">当前字符串。</param>
	/// <param name="defaultValue">转换失败时的，默认枚举值。</param>
	/// <param name="isIgnoreCase">比较字符串时是否忽略大小写，默认为：true。</param>
	/// <returns>返回当前字符串对应的枚举值。</returns>
	public static T EnumValue<T>(
		this string? str,
		T defaultValue,
		bool isIgnoreCase = true) where T : Enum
	{
		return EnumUtil.ValueOf<T>(
		    str,
		    defaultValue,
		    isIgnoreCase);
	}

	public static string? StringByAppend(
		this string? originalString,
		string? stringNeedAppend,
		string? spliter = null)
	{
		if (!string.IsNullOrEmpty(stringNeedAppend))
		{
			if (string.IsNullOrEmpty(originalString))
			{
				originalString = stringNeedAppend;
			}
			else
			{
				if (!string.IsNullOrEmpty(spliter))
				{
					originalString += spliter;
				}
				originalString += stringNeedAppend;
			}
		}
		return originalString;
	}

	public static List<string> SplitWithOptionalSeparators(
	    this string stringValue,
	    bool isIgnoreCase,
	    params char[] optionalSeparatorChars)
	{
		if (optionalSeparatorChars.Length < 1)
		{
			return [stringValue];
		}

		var substrings = new List<string>();
		char finalSeperatorChar = '\0';
		var isFinalSeperatorCharValid = false;
		var stringValueCharsCount = stringValue.Length;
		var currentSubstringBeginIndex = 0;
		var stringValueLastCharIndex = stringValueCharsCount - 1;
		for (var stringValueCharIndex = 0;
		    stringValueCharIndex < stringValueCharsCount;
		    stringValueCharIndex++)
		{
			var stringValueChar = stringValue[stringValueCharIndex];
			var isStringValueCharSeperator = false;
			if (isFinalSeperatorCharValid)
			{
				if (CharUtil.IsEquals(stringValueChar, finalSeperatorChar, isIgnoreCase))
				{
					// !!!
					isStringValueCharSeperator = true;
					// !!!
				}
			}
			else
			{
				for (var optionalSeparatorCharIndex = 0;
					optionalSeparatorCharIndex < optionalSeparatorChars.Length;
					optionalSeparatorCharIndex++)
				{
					var optionalSeparatorChar = optionalSeparatorChars[optionalSeparatorCharIndex];
					if (CharUtil.IsEquals(stringValueChar, optionalSeparatorChar, isIgnoreCase))
					{
						// !!!
						finalSeperatorChar = optionalSeparatorChar;
						isFinalSeperatorCharValid = true;
						isStringValueCharSeperator = true;
						break;
						// !!!
					}
				}
			}
			if (!isStringValueCharSeperator)
			{
				if (stringValueCharIndex == stringValueLastCharIndex)
				{
					// !!!
					stringValueCharIndex += 1;
					// isStringValueCharSeperator = true;
					// !!!
				}
				else
				{
					continue;
				}
			}

			var substring = stringValue[currentSubstringBeginIndex..stringValueCharIndex];
			{
				// !!!
				substrings.Add(substring);
				// !!!
			}
			if (stringValueCharIndex == stringValueLastCharIndex
				&& isStringValueCharSeperator)
			{
				// !!!
				substrings.Add(string.Empty);
				// !!!
			}

			currentSubstringBeginIndex = stringValueCharIndex + 1;
		}
		return substrings;
	}

	public static List<string> SplitWithOptionalSeparatorsIgnoreCase(
	    this string stringValue,
	    params char[] optionalSeparatorChars)
	{
		return SplitWithOptionalSeparators(
			stringValue,
			true,
			optionalSeparatorChars);
	}

	public static string ToStringEndWithoutFullstop(
	    this string originString)
	{
		return originString.TrimEnd(
		    '.',
		    '。');
	}

	/// <summary>
	/// 由当前字符串，拆解成字符串数组。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="spliter">字符串中的分隔符。</param>
	/// <param name="splitOptions">字符串分隔选项。</param>
	/// <returns>有拆解字符串得出的字符串数组，当字符串为空或非数字时，会转为“0”。</returns>
	public static string[] ToStringArray(
		this string? str,
		string spliter = ",",
		StringSplitOptions splitOptions = StringSplitOptions.None)
	{
		if (string.IsNullOrEmpty(str))
		{
			return [];
		}

		var sections = str.Split([spliter], splitOptions);
		var stringValues = sections;
		{ }
		return stringValues;
	}


	/// <summary>
	/// 由当前字符串，拆解成字符串数组，去掉空白元素，并且为每个元素进行Trim操作。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="spliter">字符串中的分隔符。</param>
	/// <param name="splitOptions">字符串分隔选项。</param>
	/// <returns>有拆解字符串得出的字符串数组，当字符串为空或非数字时，会转为“0”。</returns>
	public static string[] ToUnemptyStringArrayWithTrimEntries(
		this string? str,
		string spliter = ",")
	{
		return StringExtension.ToStringArray(
			str,
			spliter,
			StringSplitOptions.RemoveEmptyEntries
			| StringSplitOptions.TrimEntries);
	}

	/// <summary>
	/// 由当前字符串，拆解成整型数值数组。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="spliter">字符串中的分隔符。</param>
	/// <param name="splitOptions">字符串分隔选项。</param>
	/// <returns>有拆解字符串得出的整型数值数组，当字符串为空或非数字时，会转为“0”。</returns>
	public static int[] ToIntArray(
		this string? str,
		string spliter = ",",
		StringSplitOptions splitOptions = StringSplitOptions.None)
	{
		if (string.IsNullOrEmpty(str))
		{
			return [];
		}

		var sections = str.Split([spliter], splitOptions);
		var intValues = new int[sections.Length];
		{
			for (var sectionIndex = 0;
			    sectionIndex < sections.Length;
			    sectionIndex++)
			{
				var section = sections[sectionIndex];
				try
				{
					_ = int.TryParse(section.Trim(), out var number);
					{ }
					intValues[sectionIndex] = number;
				}
				catch
				{
					intValues[sectionIndex] = 0;
				}
			}
		}
		return intValues;
	}

	/// <summary>
	/// 由当前字符串，拆解成整型数值数组。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="spliter">字符串中的分隔符。</param>
	/// <param name="splitOptions">字符串分隔选项。</param>
	/// <returns>有拆解字符串得出的整型数值数组，当字符串为空或非数字时，会转为“0”。</returns>
	public static long[] ToLongArray(
		this string? str,
		string spliter = ",",
		StringSplitOptions splitOptions = StringSplitOptions.None)
	{
		if (string.IsNullOrEmpty(str))
		{
			return [];
		}

		var sections = str.Split([spliter], splitOptions);
		var longValues = new long[sections.Length];
		{
			for (var sectionIndex = 0;
			    sectionIndex < sections.Length;
			    sectionIndex++)
			{
				var section = sections[sectionIndex];
				try
				{
					_ = long.TryParse(section.Trim(), out var number);
					{ }
					longValues[sectionIndex] = number;
				}
				catch
				{
					longValues[sectionIndex] = 0;
				}
			}
		}
		return longValues;
	}

	/// <summary>
	/// 由当前字符串，拆解成浮点数数值数组。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="spliter">字符串中的分隔符。</param>
	/// <param name="splitOptions">字符串分隔选项。</param>
	/// <returns>有拆解字符串得出的浮点数数值数组，当字符串为空或非数字时，会转为“0.0”。</returns>
	public static float[] ToFloatArray(
		this string? str,
		string spliter = ",",
		StringSplitOptions splitOptions = StringSplitOptions.None)
	{
		if (string.IsNullOrEmpty(str))
		{
			return [];
		}

		var sections = str.Split([spliter], splitOptions);
		var floatValues = new float[sections.Length];
		{
			for (var sectionIndex = 0;
			    sectionIndex < sections.Length;
			    sectionIndex++)
			{
				var section = sections[sectionIndex];
				try
				{
					_ = float.TryParse(section.Trim(), out var number);
					{ }
					floatValues[sectionIndex] = number;
				}
				catch
				{
					floatValues[sectionIndex] = 0;
				}
			}
		}
		return floatValues;
	}

	/// <summary>
	/// 由当前字符串，拆解成浮点数数值数组。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="spliter">字符串中的分隔符。</param>
	/// <param name="splitOptions">字符串分隔选项。</param>
	/// <returns>有拆解字符串得出的浮点数数值数组，当字符串为空或非数字时，会转为“0.0”。</returns>
	public static double[] ToDoubleArray(
		this string? str,
		string spliter = ",",
		StringSplitOptions splitOptions = StringSplitOptions.None)
	{
		if (string.IsNullOrEmpty(str))
		{
			return [];
		}

		var sections = str.Split([spliter], splitOptions);
		var doubleValues = new double[sections.Length];
		{
			for (var sectionIndex = 0;
			    sectionIndex < sections.Length;
			    sectionIndex++)
			{
				var section = sections[sectionIndex];
				try
				{
					_ = double.TryParse(section.Trim(), out var number);
					{ }
					doubleValues[sectionIndex] = number;
				}
				catch
				{
					doubleValues[sectionIndex] = 0;
				}
			}
		}
		return doubleValues;
	}

	/// <summary>
	/// 由当前字符串，拆解成高精度浮点数值数组。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="spliter">字符串中的分隔符。</param>
	/// <param name="splitOptions">字符串分隔选项。</param>
	/// <returns>有拆解字符串得出的高精度浮点数数值数组，当字符串为空或非数字时，会转为“0.0”。</returns>
	public static decimal[] ToDecimalArray(
		this string? str,
		string spliter = ",",
		StringSplitOptions splitOptions = StringSplitOptions.None)
	{
		if (string.IsNullOrEmpty(str))
		{
			return [];
		}

		var sections = str.Split([spliter], splitOptions);
		var decimalValues = new decimal[sections.Length];
		{
			for (var sectionIndex = 0;
			    sectionIndex < sections.Length;
			    sectionIndex++)
			{
				var section = sections[sectionIndex];
				try
				{
					_ = decimal.TryParse(section.Trim(), out var number);
					{ }
					decimalValues[sectionIndex] = number;
				}
				catch
				{
					decimalValues[sectionIndex] = 0;
				}
			}
		}
		return decimalValues;
	}

	/// <summary>
	/// 通过在当前字符串“左侧”填充指定数量的指定字符，生成新的字符串。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="fillChar">要填充的字符。</param>
	/// <param name="fillCount">要填充的字符数量。</param>
	/// <returns>填充了指定数量，指定字符后的字符串。</returns>
	public static string StringByFillCharacterAtLeft(
	    this string? str,
	    char fillChar,
	    int fillCount)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}

		if (fillChar != '\0'
		    && fillCount > 0)
		{
			if (fillCount == 1)
			{
				str = fillChar + str;
			}
			else
			{
				var fillChars = new char[fillCount];
				{
					Array.Fill(fillChars, fillChar);
				}
				str = fillChars + str;
			}
		}
		return str;
	}

	/// <summary>
	/// 通过在当前字符串“中部”填充指定数量的指定字符，生成新的字符串。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="fillChar">要填充的字符。</param>
	/// <param name="fillCount">要填充的字符数量。</param>
	/// <returns>填充了指定数量，指定字符后的字符串。</returns>
	public static string StringByFillCharacterAtMid(
	    this string? str,
	    char fillChar,
	    int fillCount)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}

		if (fillChar != '\0'
		    && fillCount > 0)
		{
			var fillChars = new char[fillCount];
			{
				Array.Fill(fillChars, fillChar);
			}

			var leftCharsCount = str.Length / 2;
			var rightCharsCount = leftCharsCount;
			if ((str.Length % 2) != 0)
			{
				rightCharsCount += 1;
			}
			str = string.Concat(
				str.AsSpan(0, leftCharsCount),
				fillChars,
				str.AsSpan(rightCharsCount));
		}
		return str;
	}

	/// <summary>
	/// 通过在当前字符串“右侧”填充指定数量的指定字符，生成新的字符串。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="fillChar">要填充的字符。</param>
	/// <param name="fillCount">要填充的字符数量。</param>
	/// <returns>填充了指定数量，指定字符后的字符串。</returns>
	public static string StringByFillCharacterAtRight(
	    this string? str,
	    char fillChar,
	    int fillCount)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}

		if (fillChar != '\0'
		    && fillCount > 0)
		{
			if (fillCount == 1)
			{
				str += fillChar;
			}
			else
			{
				var fillChars = new char[fillCount];
				{
					Array.Fill(fillChars, fillChar);
				}
				str += fillChars;
			}
		}
		return str;
	}

	/// <summary>
	/// 通过在当前字符串“左侧”填充指定数量的指定字符，生成新的字符串。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="fillChar">要填充的字符。</param>
	/// <param name="fillCount">要填充的字符数量。</param>
	/// <returns>填充了指定数量，指定字符后的字符串。</returns>
	public static string StringByFillCharacterAtLeftToLength(
	    this string? str,
	    char fillChar,
	    int stringLengthAfterFill)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}

		if (fillChar != '\0'
		    && stringLengthAfterFill > str.Length)
		{
			var fillCount = stringLengthAfterFill - str.Length;
			{
				str = str.StringByFillCharacterAtLeft(
				    fillChar,
				    fillCount);
			}
		}
		return str;
	}


	/// <summary>
	/// 通过在当前字符串“中部”填充指定数量的指定字符，生成新的字符串。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="fillChar">要填充的字符。</param>
	/// <param name="fillCount">要填充的字符数量。</param>
	/// <returns>填充了指定数量，指定字符后的字符串。</returns>
	public static string StringByFillCharacterAtMidToLength(
	    this string? str,
	    char fillChar,
	    int stringLengthAfterFill)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}

		if (fillChar != '\0'
		    && stringLengthAfterFill > str.Length)
		{
			var fillCount = stringLengthAfterFill - str.Length;
			{
				str = str.StringByFillCharacterAtMid(
				    fillChar,
				    fillCount);
			}
		}
		return str;
	}

	/// <summary>
	/// 通过在当前字符串“右侧”填充指定数量的指定字符，生成新的字符串。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="fillChar">要填充的字符。</param>
	/// <param name="fillCount">要填充的字符数量。</param>
	/// <returns>填充了指定数量，指定字符后的字符串。</returns>
	public static string StringByFillCharacterAtRightToLength(
	    this string? str,
	    char fillChar,
	    int stringLengthAfterFill)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}

		if (fillChar != '\0'
		    && stringLengthAfterFill > str.Length)
		{
			var fillCount = stringLengthAfterFill - str.Length;
			{
				str = str.StringByFillCharacterAtRight(
				    fillChar,
				    fillCount);
			}
		}
		return str;
	}

	/// <summary>
	/// 通过移除“左侧”字符，产生指定长度的字符串。
	/// </summary>
	/// <param name="str">当前字符串</param>
	/// <param name="stringLengthAfterRemove">新字符串的目标长度。</param>
	/// <returns>小于等于指定长度的新字符串。</returns>
	public static string StringByRemoveLeftCharsToLength(
		this string? str,
		int stringLengthAfterRemove)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}

		if (str.Length > stringLengthAfterRemove)
		{
			str = str[(str.Length - stringLengthAfterRemove)..];
		}
		return str;
	}

	/// <summary>
	/// 通过移除“右侧”字符，产生指定长度的字符串。
	/// </summary>
	/// <param name="str">当前字符串</param>
	/// <param name="stringLengthAfterRemove">新字符串的目标长度。</param>
	/// <returns>小于等于指定长度的新字符串。</returns>
	public static string StringByRemoveRightCharsToLength(
		this string? str,
		int stringLengthAfterRemove)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}

		if (str.Length > stringLengthAfterRemove)
		{
			str = str[..stringLengthAfterRemove];
		}
		return str;
	}

	/// <summary>
	/// 通过移除“中部”字符，产生指定长度的字符串。
	/// </summary>
	/// <param name="str">当前字符串</param>
	/// <param name="stringLengthAfterRemove">新字符串的目标长度。</param>
	/// <returns>小于等于指定长度的新字符串。</returns>
	public static string StringByRemoveMidCharsToLength(
		this string? str,
		int stringLengthAfterRemove)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}

		if (str.Length > stringLengthAfterRemove)
		{
			var leftCharsCount = stringLengthAfterRemove / 2;
			var rightCharsCount = leftCharsCount;
			if ((stringLengthAfterRemove % 2) != 0)
			{
				rightCharsCount++;
			}
			str = string.Concat(
				str.AsSpan(0, leftCharsCount),
				str.AsSpan(str.Length - rightCharsCount));
		}
		return str;
	}

	/// <summary>
	/// 通过添加，或移除“左侧”字符，产生指定长度的字符串。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="fillChar">要填充的字符。</param>
	/// <param name="stringLengthAfterOperation">新字符串的目标长度。</param>
	/// <returns>等于指定长度的新字符串。</returns>
	public static string StringByRetainRightCharsToLength(
		this string? str,
		char fillChar,
		int stringLengthAfterOperation)
	{
		return str.StringByFillCharacterAtLeftToLength(
			fillChar,
			stringLengthAfterOperation)
			.StringByRemoveLeftCharsToLength(
			stringLengthAfterOperation);
	}

	/// <summary>
	/// 通过添加，或移除“中部”字符，产生指定长度的字符串。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="fillChar">要填充的字符。</param>
	/// <param name="stringLengthAfterOperation">新字符串的目标长度。</param>
	/// <returns>等于指定长度的新字符串。</returns>
	public static string StringByRetainLeftAndRightCharsToLength(
		this string? str,
		char fillChar,
		int stringLengthAfterOperation)
	{
		return str.StringByFillCharacterAtMidToLength(
			fillChar,
			stringLengthAfterOperation)
			.StringByRemoveMidCharsToLength(
			stringLengthAfterOperation);
	}

	/// <summary>
	/// 通过添加，或移除“右侧”字符，产生指定长度的字符串。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="fillChar">要填充的字符。</param>
	/// <param name="stringLengthAfterOperation">新字符串的目标长度。</param>
	/// <returns>等于指定长度的新字符串。</returns>
	public static string StringByRetainLeftCharsToLength(
		this string? str,
		char fillChar,
		int stringLengthAfterOperation)
	{
		return str.StringByFillCharacterAtRightToLength(
			fillChar,
			stringLengthAfterOperation)
			.StringByRemoveRightCharsToLength(
			stringLengthAfterOperation);
	}

	/// <summary>
	/// 使用Uri路径编码创建新的字符串。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <returns>Uri路径编码的新字符串。</returns>
	public static string StringByEncodeInUriPath(this string? str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}

		var strEncoded = Uri.EscapeDataString(str);
		{ }
		return strEncoded;
	}

	/// <summary>
	/// 使用Uri路径解码创建新的字符串。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <returns>Uri路径解码的新字符串。</returns>
	public static string StringByDecodeInUriPath(this string? str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}

		var strEncoded = Uri.UnescapeDataString(str);
		{ }
		return strEncoded;
	}

	/// <summary>
	/// 使用Uri数据编码创建新的字符串。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <returns>Uri数据编码的新字符串。</returns>
	public static string StringByEncodeInUriParam(this string? str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}

		var strEncoded = Uri.EscapeDataString(str);
		{ }
		return strEncoded;
	}

	/// <summary>
	/// 使用Uri数据解码创建新的字符串。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <returns>Uri数据解码的新字符串。</returns>
	public static string StringByDecodeInUriParam(this string? str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}

		var strEncoded = Uri.UnescapeDataString(str);
		{ }
		return strEncoded;
	}

	/// <summary>
	/// 使用Uri参数编码方式，编码原字符串中非Ascii码字符，以生成纯Ascii码字符串。
	/// </summary>
	/// <param name="stringWithUnasciiChars">当前字符串。</param>
	/// <returns>纯Ascii码构成掉字符串。</returns>
	public static string StringByEncodeUnasciiCharsToUriParam(this string? stringWithUnasciiChars)
	{
		if (string.IsNullOrEmpty(stringWithUnasciiChars))
		{
			return string.Empty;
		}

		for (var charIndex = stringWithUnasciiChars.Length - 1;
			charIndex >= 0;
			charIndex--)
		{
			var character = stringWithUnasciiChars[charIndex];
			if (!Char.IsAscii(character))
			{
				var lastUnasciiCharIndex = charIndex;
				for (charIndex--;
				charIndex >= 0;
				charIndex--)
				{
					character = stringWithUnasciiChars[charIndex];
					if (Char.IsAscii(character))
					{
						break;
					}
				}
				var firstUnasciiCharIndex = charIndex + 1;
				var unasciiCharStringLength = lastUnasciiCharIndex - firstUnasciiCharIndex + 1;
				if (unasciiCharStringLength > 0)
				{
					var unasciiCharString
						= stringWithUnasciiChars.Substring(
							firstUnasciiCharIndex,
							unasciiCharStringLength);
					var asciiCharString
						= System.Web.HttpUtility.UrlEncode(
							unasciiCharString,
							System.Text.Encoding.UTF8);
					// !!!
					stringWithUnasciiChars
						= stringWithUnasciiChars[..firstUnasciiCharIndex]
						+ asciiCharString
						+ stringWithUnasciiChars[(lastUnasciiCharIndex + 1)..];
					// !!!
				}
			}
		}
		return stringWithUnasciiChars;
	}

	/// <summary>
	/// 链接指定的Uri和Uri相对路径。
	/// </summary>
	/// <param name="uri">指定的Uri。</param>
	/// <param name="uriRelativePath">指定的Uri相对路径。</param>
	/// <param name="isCurrentStringFileUri">当前字符串是否为文件URI，如果是，则会取当前文件所在的文件夹路径。</param>
	/// <returns>连接了指定Uri相对路径后的Uri路径。</returns>
	public static string StringByUriAppendRelativePath(
		this string uri,
		string? uriRelativePath,
		bool isCurrentStringFileUri = false)
	{
		if (string.IsNullOrEmpty(uriRelativePath))
		{
			return uri;
		}

		string? queryParams = null;
		var indexOfQueryDelimiter = uri.IndexOf('?');
		if (indexOfQueryDelimiter > 0)
		{
			queryParams = uri[indexOfQueryDelimiter..];
			uri = uri[..indexOfQueryDelimiter];
		}
		string? fragments = null;
		var indexOfFragmentDelimiter = uri.IndexOf('#');
		if (indexOfFragmentDelimiter > 0)
		{
			fragments = uri[indexOfFragmentDelimiter..];
			uri = uri[..indexOfFragmentDelimiter];
		}

		uri = uri.ToUriSystemDirectoryPath(isCurrentStringFileUri)
			+ uriRelativePath.ToUriSystemRelativePath();
		if (fragments?.Length > 0)
		{
			uri += fragments;
		}
		if (queryParams?.Length > 0)
		{
			uri += queryParams;
		}
		return uri;
	}

	/// <summary>
	/// 连接指定的Uri和Uri查询参数字符串。
	/// </summary>
	/// <param name="uri">当前Uri字符串。</param>
	/// <param name="uriQueryParams">要连接的Url查询参数字符串。</param>
	/// <returns>连接了Url查询参数的Uri字符串。</returns>
	public static string StringByUriAppendQueryParams(
		this string uri,
		string? uriQueryParams)
	{
		if (uriQueryParams == null
			|| uriQueryParams.Length < 1)
		{
			return uri;
		}

		string? uriSuffix = null;
		var indexOfSharpSymbolInUri = uri.IndexOf('#');
		if (indexOfSharpSymbolInUri >= 0)
		{
			uriSuffix = uri[indexOfSharpSymbolInUri..];
			uri = uri[..indexOfSharpSymbolInUri];
		}

		uriQueryParams = uriQueryParams.Trim('?', '&');
		string? uriQueryParamsSuffix = null;
		var indexOfSharpSymbolInUriQueryParams = uriQueryParams.IndexOf('#');
		if (indexOfSharpSymbolInUriQueryParams >= 0)
		{
			uriQueryParamsSuffix = uriQueryParams[indexOfSharpSymbolInUriQueryParams..];
			uriQueryParams = uriQueryParams[..indexOfSharpSymbolInUriQueryParams];
		}

		var indexOfQuestionMark = uri.IndexOf('?');
		if (indexOfQuestionMark < 1)
		{
			uri += '?';
		}
		else
		{
			if (indexOfQuestionMark < (uri.Length - 1)
			&& uri.EndsWith('&') != true)
			{
				uri += '&';
			}
		}
		uri += uriQueryParams;

		if (uriSuffix?.Length > 0
			&& uriQueryParamsSuffix?.Length > 0)
		{
			uri += uriSuffix + '&' + uriQueryParamsSuffix.Trim('#');
		}
		else if (uriSuffix?.Length > 0)
		{
			uri += uriSuffix;
		}
		else if (uriQueryParamsSuffix?.Length > 0)
		{
			uri += uriQueryParamsSuffix;
		}

		return uri;
	}

	/// <summary>
	/// 连接指定的Uri和Uri查询参数字符串。
	/// </summary>
	/// <param name="uri">当前Uri字符串。</param>
	/// <param name="uriQueryParams">要连接的Url查询参数字符串。</param>
	/// <returns>连接了Url查询参数的Uri字符串。</returns>
	public static string StringByUriAppendQueryParams(
		this string uri,
		Dictionary<string, string?>? uriQueryParams)
	{
		var uriQueryString = uriQueryParams?.ToUriQuery();
		{ }
		return StringByUriAppendQueryParams(
			uri,
			uriQueryString);
	}

	/// <summary>
	/// 使用Html转义，编码当前字符串。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <returns>返回经过Html转义编码的字符串。</returns>
	public static string? StringByEncodeInHtmlEscape(this string? str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return null;
		}

		var strEncoded = System.Net.WebUtility.HtmlEncode(str);
		{ }
		return strEncoded;
	}

	/// <summary>
	/// 使用Html转义，解码当前字符串。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <returns>返回经过Html转义解码的字符串。</returns>
	public static string? StringByDecodeInHtmlEscape(this string? str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return null;
		}

		var strEncoded = System.Net.WebUtility.HtmlDecode(str);
		{ }
		return strEncoded;
	}

	/// <summary>
	/// 去除标准代码格式中的备注信息，如：预先移除含有备注内容的Json字符串。
	/// </summary>
	/// <param name="str">含有标注代码备注的字符串。</param>
	/// <returns>不含有标注代码备注的字符串。</returns>
	public static string? StringByRemoveCodeComments(this string? str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}

		var codeStrings = new List<string>();
		var codeStringWithComments = str;
		if (codeStringWithComments?.Length > 0)
		{
			codeStringWithComments = codeStringWithComments.Trim();

			var codeStringBeginIndex = 0;
			var codeStringEndIndex = 0;
			var lastCharIndex = codeStringWithComments.Length - 1;
			for (var charIndex = 0;
			    charIndex <= lastCharIndex;
			    charIndex++)
			{
				var newCodeStringBeginIndex = codeStringBeginIndex;
				var newCodeStringEndIndex = codeStringEndIndex;
				{
					var currentChar = codeStringWithComments[charIndex];
					if (charIndex == lastCharIndex)
					{
						// !!!
						newCodeStringEndIndex = lastCharIndex + 1;
						// !!!
					}
					else if (currentChar == '\''
					    || currentChar == '"')
					{
						for (charIndex++;
						    charIndex <= lastCharIndex;
						    charIndex++)
						{
							var nextChar = codeStringWithComments[charIndex];
							if (nextChar == currentChar)
							{
								break;
							}
						}
						if (charIndex == lastCharIndex)
						{
							// !!!
							newCodeStringEndIndex = lastCharIndex + 1;
							// !!!
						}
					}
					else if (currentChar == '/')
					{
						var nextCharIndex
						    = charIndex + 1;
						var nextChar
						    = nextCharIndex <= lastCharIndex
						    ? codeStringWithComments[nextCharIndex]
						    : 0;
						if (nextChar == '/')
						{
							// !!!
							newCodeStringEndIndex = charIndex;
							// !!!

							for (charIndex = nextCharIndex + 1;
							    charIndex <= lastCharIndex;
							    charIndex++)
							{
								currentChar = codeStringWithComments[charIndex];
								if (currentChar == '\r')
								{
									nextCharIndex = charIndex + 1;
									if (nextCharIndex <= lastCharIndex)
									{
										nextChar = codeStringWithComments[nextCharIndex];
										if (nextChar == '\n')
										{
											charIndex = nextCharIndex;
											break;
										}
										else
										{
											break;
										}
									}
								}
								else if (currentChar == '\n')
								{
									break;
								}
							}
							// !!!
							newCodeStringBeginIndex = charIndex + 1;
							// !!!
						}
						else if (nextChar == '*')
						{
							// !!!
							newCodeStringEndIndex = charIndex;
							// !!!

							for (charIndex = nextCharIndex + 1;
							    charIndex <= lastCharIndex;
							    charIndex++)
							{
								currentChar = codeStringWithComments[charIndex];
								if (currentChar == '*'
								    && charIndex < lastCharIndex)
								{
									nextCharIndex = charIndex + 1;
									nextChar = codeStringWithComments[nextCharIndex];
									if (nextChar == '/')
									{
										charIndex = nextCharIndex;
										break;
									}
								}
							}
							// !!!
							newCodeStringBeginIndex = charIndex + 1;
							// !!!
						}
					}
				}
				if (codeStringEndIndex != newCodeStringEndIndex)
				{
					codeStringEndIndex = newCodeStringEndIndex;
					if (codeStringEndIndex > codeStringBeginIndex)
					{
						var codStringLength
						    = codeStringEndIndex - codeStringBeginIndex;
						var codeString
						    = codeStringWithComments.Substring(codeStringBeginIndex, codStringLength);
						// !!!
						codeStrings.Add(codeString);
						// !!!
					}
				}
				codeStringBeginIndex = newCodeStringBeginIndex;
			}
		}

		string? codeStringWithoutComments = null;
		if (codeStrings.Count > 0)
		{
			foreach (var codeString in codeStrings)
			{
				codeStringWithoutComments += codeString;
			}
		}
		return codeStringWithoutComments;
	}

	/// <summary>
	/// 使用“Environment.AESKey_Default”作为加密Key，通过“AES”算法对当前字符串进行加密。
	/// </summary>
	/// <param name="plaintext">当前“明文”字符串。</param>
	/// <param name="key">指定的加密Key，为空时，默认使用“Environment.AESKey_Default”。</param>
	/// <returns>返回加密后的字符串。</returns>

	[Obsolete("当前函数，使用“Aes/Ecb算法”，存在安全隐患（相同明文、密钥时，密文永远相同，因此可通过重复明文的方式进行破解），推荐使用“ToNewCiphertext”方法替代。")]
	public static string? StringByEncrypted(
		this string plaintext,
		string? key = null)
	{
		key ??= Environment.AESKeyDeafult;

		var plaintextBytes = System.Text.Encoding.UTF8.GetBytes(plaintext);
		{ }
#pragma warning disable CS0618 // 类型或成员已过时
		var cipherBytes = AES.EncryptToBytesWithECB(plaintextBytes, key);
#pragma warning restore CS0618 // 类型或成员已过时
		if (cipherBytes.Length < 1)
		{
			return null;
		}
		var ciphertext = Convert.ToBase64String(cipherBytes);
		{ }
		return ciphertext;
	}

	/// <summary>
	/// 使用“Environment.AESKey_Default”作为加密Key，通过“AES”算法对当前字符串进行解密。
	/// </summary>
	/// <param name="ciphertext">当前“密文”字符串。，</param>
	/// <param name="key">指定的解密Key，为空时，默认使用“Environment.AESKey_Default”。</param>
	/// <returns>返回解密后的字符串。</returns>
	[Obsolete("当前函数，使用“Aes/Ecb算法”，存在安全隐患（相同明文、密钥时，密文永远相同，因此可通过重复明文的方式进行破解），推荐使用“ToPlaintext”方法替代。")]
	public static string StringByDecrypted(
		this string ciphertext,
		string? key = null)
	{
		return ToPlaintext(ciphertext, key);
	}

	/// <summary>
	/// 使用Utf8编码，将当前字符串转为字节数组。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <returns>使用Utf8编码转化成的字节数组。</returns>
	public static byte[] ToUtf8Bytes(this string? str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return [];
		}

		var strUtf8Bytes = UTF8Encoding.UTF8.GetBytes(str);
		{ }
		return strUtf8Bytes;
	}

	/// <summary>
	/// 使用字符的大小写，生成当前字符串的哈希码。
	/// </summary>
	/// <param name="plaintext">
	/// 当前字符串对象。
	/// </param>
	/// <returns>
	/// 当前字符串对象对应的字符大小写哈希码，如：“Abc”的哈希码为“100”，“aBc”的哈希码为“010”。 
	/// </returns>
	public static string ToHashCodeByCharCase(
	    this string? plaintext)
	{
		var hashCodeBuilder = new StringBuilder();
		if (!string.IsNullOrEmpty(plaintext))
		{
			foreach (var character in plaintext)
			{
				if (char.IsUpper(character))
				{
					hashCodeBuilder.Append('1');
				}
				else
				{
					hashCodeBuilder.Append('0');
				}
			}
		}
		var hashCode = hashCodeBuilder.ToString();
		{ }
		return hashCode;
	}

	/// <summary>
	/// 使用SHA256算法，将当前字符串转为哈希码。
	/// </summary>
	/// <param name="plaintext">要获取哈希值的明文字符串。</param>
	/// <param name="textEncoding">哈希值的默认编码方式，默认为：null，Utf8编码。</param>
	/// <param name="hashtextLength">哈希值的字符串长度，默认为：64。</param>
	/// <returns>指定明文的SHA256哈希值字符串。</returns>
	public static string ToHashCodeBySHA256(
	    this string plaintext,
	    System.Text.Encoding? textEncoding = null)
	{
		textEncoding ??= System.Text.Encoding.UTF8;

		var hashCode = Security.Cryptography.SHA.CreateSHA256String(
			     plaintext,
			     textEncoding);
		{ }
		return hashCode;
	}

	/// <summary>
	/// 使用SHA512算法，将当前字符串转为哈希码。
	/// </summary>
	/// <param name="plaintext">要获取哈希值的明文字符串。</param>
	/// <param name="textEncoding">哈希值的默认编码方式，默认为：null，Utf8编码。</param>
	/// <param name="hashtextLength">哈希值的字符串长度，默认为：256。</param>
	/// <returns>指定明文的SHA512哈希值字符串。</returns>
	public static string ToHashCodeBySHA512(
	    this string plaintext,
	    System.Text.Encoding? textEncoding = null)
	{
		var hashCode = Security.Cryptography.SHA.CreateSHA512String(
			    plaintext,
			    textEncoding);
		{ }
		return hashCode;
	}

	/// <summary>
	/// 创建当前字符串的MD5值，32位长度。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="textEncoding">指定的字符编码方式。</param>
	/// <returns>当前字符串的MD5值。</returns>
	public static string ToHashCodeByMD532(
	    this string? str,
	    System.Text.Encoding? textEncoding = null)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}

		var md5String = Security.Cryptography.SHA.CreateMD532String(
			    str,
			    textEncoding);
		{ }
		return md5String;
	}

	/// <summary>
	/// 创建当前字符串的MD5值，16位长度。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="textEncoding">指定的字符编码方式。</param>
	/// <returns>当前字符串的MD5值。</returns>
	public static string ToHashCodeByMD516(
	    this string? str,
	    System.Text.Encoding? textEncoding = null)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}

		var md5String = Security.Cryptography.SHA.CreateMD516String(
			    str,
			    textEncoding);
		{ }
		return md5String;
	}

	/// <summary>
	/// 使用“Environment.AESKey_Default”作为加密Key，通过“Aes/Ctr”算法对当前字符串进行加密，产生新的密文字符串。
	/// 【注意】：每次提供的密钥、或噪音字符串不同时，产生的密文字符串也会不同。
	/// </summary>
	/// <param name="plaintext">当前“明文”字符串。</param>
	/// <param name="key">指定的加密Key，为空时，默认使用“Environment.AESKey_Default”。</param>
	/// <returns>返回加密后的字符串。</returns>
	public static string? ToNewCiphertext(
		this string plaintext,
		string? key = null,
		string? nonceInBase64 = null)
	{
		key ??= Environment.AESKeyDeafult;
		var ciphertext = AesCtr.EncryptString(
			plaintext,
			key,
			nonceInBase64,
			out var finalNonceInBase64);
		if (ciphertext.Length < 1)
		{
			return null;
		}

		var ciphertextParamName = BxAesValueEncryptionParamNames.Ciphertext.StringByEncodeInUriParam();
		var ciphertextParamValue = ciphertext.StringByEncodeInUriParam();

		var nonceParamName = BxAesValueEncryptionParamNames.Nonce.StringByEncodeInUriParam();
		var nonceParamValue = finalNonceInBase64.StringByEncodeInUriParam();

		var uriQuery
			= $"{ciphertextParamName}={ciphertextParamValue}&{nonceParamName}={nonceParamValue}";

		var uriBuilder = new UriBuilder
		{
			Scheme = BxAesValueEncryptionMethodNames.Aes_Ctr,
			Host = string.Empty,
			Query = uriQuery
		};
		ciphertext = uriBuilder.ToString();
		{ }
		return ciphertext;
	}

	/// <summary>
	/// 使用“Environment.AESKey_Default”作为加密Key，通过“AES”算法对当前字符串进行解密。
	/// </summary>
	/// <param name="ciphertext">当前“密文”字符串。，</param>
	/// <param name="key">指定的解密Key，为空时，默认使用“Environment.AESKey_Default”。</param>
	/// <returns>返回解密后的字符串。</returns>
	public static string ToPlaintext(
		this string ciphertext,
		out string? nonceInBase64,
		string? key = null)
	{
		key ??= Environment.AESKeyDeafult;
		nonceInBase64 = null;

		////////////////////////////////////////////////
		// 1/2，新版本使用“AES/CTR”加密方法：
		////////////////////////////////////////////////

		if (ciphertext.StartsWith(BxAesValueEncryptionMethodNames.Aes_Ctr + ":"))
		{
			try
			{
				if (Uri.TryCreate(
					ciphertext,
					new UriCreationOptions()
					{
						DangerousDisablePathAndQueryCanonicalization = true
					},
					out var ciphertextUri))
				{
					var encryptionParams = HttpUtility.ParseQueryString(ciphertextUri.Query);
					var ciphertextContent = encryptionParams[BxAesValueEncryptionParamNames.Ciphertext];
					if (!string.IsNullOrEmpty(ciphertextContent))
					{
						nonceInBase64 = encryptionParams[BxAesValueEncryptionParamNames.Nonce];
						if (!string.IsNullOrEmpty(nonceInBase64))
						{
							return AesCtr.DecryptString(
								ciphertextContent,
								key,
								nonceInBase64);
						}
					}
				}
			}
			catch
			{ }
			return string.Empty;
		}


		////////////////////////////////////////////////
		// 2/2，旧版本使用不安全的“AES/ECB”加密方法：
		////////////////////////////////////////////////

#pragma warning disable CS0618 // 类型或成员已过时
		var plaintextBytes = AES.DecryptToBytesWithECB(ciphertext, key);
#pragma warning restore CS0618 // 类型或成员已过时
		if (plaintextBytes == null
		    || plaintextBytes.Length < 1)
		{
			return String.Empty;
		}
		var plaintext = System.Text.Encoding.UTF8.GetString(plaintextBytes);
		{ }
		return plaintext;
	}

	public static string ToPlaintext(
		this string ciphertext,
		string? key = null)
	{
		return ToPlaintext(
			ciphertext,
			out _
			, key);
	}

	/// <summary>
	/// 生成格式合法的以“\”结尾的文件系统路径字符串。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="isCurrentStringFilePath">当前字符串是否为文件路径。</param>
	/// <returns>格式合法的以“\”结尾的文件系统路径字符串。</returns>
	public static string ToFileSystemDirectoryPath(
	    this string? str,
	    bool isCurrentStringFilePath = false)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}

		var fileDirectoryPath = str.Trim();
		if (fileDirectoryPath.EndsWith(System.IO.Path.DirectorySeparatorChar) != true)
		{
			if (isCurrentStringFilePath)
			{
				var lastIndexOfDirectorySeparatorChar
				    = fileDirectoryPath
				    .LastIndexOf(System.IO.Path.DirectorySeparatorChar);
				if (lastIndexOfDirectorySeparatorChar >= 0)
				{
					fileDirectoryPath
					    = fileDirectoryPath[..(lastIndexOfDirectorySeparatorChar + 1)];
				}
			}
			else
			{
				fileDirectoryPath += System.IO.Path.DirectorySeparatorChar;
			}
		}
		return fileDirectoryPath;
	}

	/// <summary>
	/// 通过去除字符串起始处的文件夹分隔符号，以生成相对文件路径。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <returns>去除字符串起始处的“\”和“/”符号后的相对路径字符串。</returns>
	public static string ToFileSystemRelativePath(
	    this string? str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}

		var relativeFilePath = str.Trim();
		if (relativeFilePath?.Length > 0)
		{
			var newRelativeFilePathLength = relativeFilePath.Length;
			var firstUnslashCharIndex = 0;
			for (;
			    firstUnslashCharIndex < newRelativeFilePathLength;
			    firstUnslashCharIndex++)
			{
				var character = relativeFilePath[firstUnslashCharIndex];
				if (character != System.IO.Path.DirectorySeparatorChar)
				{
					break;
				}
			}
			if (firstUnslashCharIndex < newRelativeFilePathLength)
			{
				relativeFilePath = relativeFilePath[firstUnslashCharIndex..];
			}
			else
			{
				relativeFilePath = null;
			}
		}
		if (relativeFilePath == null)
		{
			return string.Empty;
		}
		return relativeFilePath;
	}

	/// <summary>
	/// 当前字符串格式为相对路径时，返回在指定根路径下的绝对路径；当前字符串格式为绝对路径时，返回当前字符串作为绝对路径。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="rootPath">指定的根路径。</param>
	/// <returns>当前字符串最终确认的绝对路径，当当前字符串为”null“，或长度无效时，返回”null“。</returns>
	public static string ToAbsoluteFilePathInRootPath(
		this string? str,
		string? rootPath)
	{
		string absoluteFilePath;
		if (System.IO.Path.IsPathRooted(str) == true)
		{
			absoluteFilePath = str;
		}
		else if (rootPath?.Length > 0
			&& System.IO.Path.IsPathRooted(rootPath))
		{
			absoluteFilePath = rootPath.ToFileSystemDirectoryPath() + str;
		}
		else if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}
		else
		{
			absoluteFilePath = System.IO.Path.DirectorySeparatorChar + str;
		}
		return absoluteFilePath;
	}

	/// <summary>
	/// 生成格式合法的以“/”结尾的URI系统路径字符串。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="isCurrentStringFileUri">当前字符串是否为文件URI，如果是，则会取当前文件所在的文件夹路径。</param>
	/// <returns>格式合法的以“/”结尾的URI系统路径字符串。</returns>
	public static string ToUriSystemDirectoryPath(
	    this string? str,
	    bool isCurrentStringFileUri = false)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}

		var uriDirectoryPath = str.Trim();
		var directorySeparatorChar = System.IO.Path.AltDirectorySeparatorChar;
		if (uriDirectoryPath.EndsWith(directorySeparatorChar) != true)
		{
			if (isCurrentStringFileUri)
			{
				var lastIndexOfBackslash = uriDirectoryPath.LastIndexOf(directorySeparatorChar);
				if (lastIndexOfBackslash >= 0)
				{
					uriDirectoryPath
					    = uriDirectoryPath[..(lastIndexOfBackslash + 1)];
				}
			}
			else
			{
				uriDirectoryPath += directorySeparatorChar;
			}
		}
		return uriDirectoryPath;
	}

	/// <summary>
	/// 通过去除字符串起始处的“\”和“/”符号，以生成相对Uri路径。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <returns>去除字符串起始处的“\”和“/”符号后的相对路径字符串。</returns>
	public static string? ToUriSystemRelativePath(
	    this string? str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}

		var relativeFilePath = str.Trim();
		var directorySeparatorCharA = '/';
		var directorySeparatorCharB = '\\';
		{
			var newRelativeFilePathLength = relativeFilePath.Length;
			var firstUnslashCharIndex = 0;
			for (;
			    firstUnslashCharIndex < newRelativeFilePathLength;
			    firstUnslashCharIndex++)
			{
				var character = relativeFilePath[firstUnslashCharIndex];
				if (character != directorySeparatorCharA
					&& character != directorySeparatorCharB)
				{
					break;
				}
			}
			if (firstUnslashCharIndex < newRelativeFilePathLength)
			{
				relativeFilePath = relativeFilePath[firstUnslashCharIndex..];
			}
			else
			{
				relativeFilePath = null;
			}
		}
		return relativeFilePath;
	}

	/// <summary>
	/// 提取当前“Uri字符串”中的协议部分。
	/// </summary>
	/// <param name="uriString">当前“Uri字符串”。</param>
	/// <returns>返回提取出的协议部分字符串。</returns>
	public static string? GetSchemeInUri(
	       this string? uriString)
	{
		if (string.IsNullOrEmpty(uriString))
		{
			return null;
		}

		var indexOfSchemeDelimiter = uriString.IndexOf(System.Uri.SchemeDelimiter);
		if (indexOfSchemeDelimiter < 0)
		{
			return null;
		}

		var uriScheme = uriString[..indexOfSchemeDelimiter];
		{ }
		return uriScheme;
	}

	/// <summary>
	/// 提取当前“Uri字符串”中的主机部分。
	/// </summary>
	/// <param name="uriString">当前“Uri字符串”。</param>
	/// <param name="isIncludePort">获取的主机部分，是否包含访问端口。</param>
	/// <returns>返回提取出的主机部分字符串。</returns>
	public static string? GetHostInUri(
	       this string? uriString,
	       bool isIncludePort = false)
	{
		if (string.IsNullOrEmpty(uriString))
		{
			return null;
		}

		var schemeDelimiter = System.Uri.SchemeDelimiter;
		var indexOfSchemeDelimiter = uriString.IndexOf(schemeDelimiter);

		var hostBeginCharIndex
			= indexOfSchemeDelimiter >= 0
			? indexOfSchemeDelimiter + schemeDelimiter.Length
			: 0;
		var hostEndCharIndex = uriString.Length;
		var directorySeparatorChar = System.IO.Path.AltDirectorySeparatorChar;
		var indexOfDirectorySeparatorChar = uriString.IndexOf(directorySeparatorChar, hostBeginCharIndex);
		if (indexOfDirectorySeparatorChar >= 0)
		{
			hostEndCharIndex = indexOfDirectorySeparatorChar;
		}

		var indexOfQueryDelimiter = uriString.IndexOf('?', hostBeginCharIndex);
		if (indexOfQueryDelimiter >= 0
			&& hostEndCharIndex > indexOfQueryDelimiter)
		{
			hostEndCharIndex = indexOfQueryDelimiter;
		}

		if (!isIncludePort)
		{
			var hostPortDelimiter = ':';
			var indexOfHostPortDelimiter = uriString.IndexOf(hostPortDelimiter, hostBeginCharIndex);
			if (indexOfHostPortDelimiter >= 0
				&& hostEndCharIndex > indexOfHostPortDelimiter)
			{
				hostEndCharIndex = indexOfHostPortDelimiter;
			}
		}
		if (hostEndCharIndex <= hostBeginCharIndex)
		{
			return null;
		}

		var uriHost = uriString[hostBeginCharIndex..hostEndCharIndex];
		{ }
		return uriHost;
	}

	/// <summary>
	/// 提取当前“Uri字符串”中的路径部分，【注意】路径会以“/”开头。
	/// </summary>
	/// <param name="uriString">当前“Uri字符串”。</param>
	/// <param name="isIncludeQueryParams">提取出的路径字符串，是否要包含查询参数。</param>
	/// <returns>返回提取出的路径部分字符串。</returns>
	public static string? GetPathInUri(
	       this string? uriString,
	       bool isIncludeQueryParams = false)
	{
		if (string.IsNullOrEmpty(uriString))
		{
			return null;
		}

		var hostEndCharIndex = 0;
		var uriSchemeDelimiter = System.Uri.SchemeDelimiter;
		var indexOfSchemeDelimiter = uriString.IndexOf(uriSchemeDelimiter);
		if (indexOfSchemeDelimiter > 0)
		{
			hostEndCharIndex = indexOfSchemeDelimiter + uriSchemeDelimiter.Length;
		}

		var pathSeparator = System.IO.Path.AltDirectorySeparatorChar;
		var indexOfRootPathSeparator = uriString.IndexOf(pathSeparator, hostEndCharIndex);
		if (indexOfRootPathSeparator < 0)
		{
			return null;
		}

		var pathBeginCharIndex = indexOfRootPathSeparator;
		var pathEndCharIndex = uriString.Length;
		if (!isIncludeQueryParams)
		{
			var indexOfQuestionMark = uriString.IndexOf('?', pathBeginCharIndex);
			if (indexOfQuestionMark >= 0)
			{
				pathEndCharIndex = indexOfQuestionMark;
			}
		}

		var uriRelativePath = uriString[pathBeginCharIndex..pathEndCharIndex];
		{ }
		return uriRelativePath;
	}

	/// <summary>
	/// 提取当前“Uri字符串”中的查询参数部分。
	/// </summary>
	/// <param name="uriString">当前“Uri字符串”。</param>
	/// <param name="isIncludeFragment">提取出的查询参数部分字符串，是否包含锚点。</param>
	/// <returns>返回提取出的查询参数部分字符串。</returns>
	public static string? GetQueryParamsInUri(
	       this string? uriString,
	       bool isIncludeFragment = false)
	{
		if (string.IsNullOrEmpty(uriString))
		{
			return null;
		}

		var indexOfQueryDelimiter = uriString.IndexOf('?');
		if (indexOfQueryDelimiter < 0)
		{
			return null;
		}

		var uriQueryBeginCharIndex = indexOfQueryDelimiter + 1;
		var uriQueryEndCharIndex = uriString.Length;
		if (isIncludeFragment == false)
		{
			var indexOfFragmentDelimiter = uriString.IndexOf('#', uriQueryBeginCharIndex);
			if (indexOfFragmentDelimiter >= 0)
			{
				uriQueryEndCharIndex = indexOfFragmentDelimiter;
			}
		}
		var uriQuery = uriString[uriQueryBeginCharIndex..uriQueryEndCharIndex];
		{ }
		return uriQuery;
	}

	/// <summary>
	/// 提取当前“Uri字符串”中的查询参数字典。
	/// </summary>
	/// <param name="uriString">当前“Uri字符串”。</param>
	/// <param name="isIncludeFragment">提取出的查询参数部分字符串，是否包含锚点。</param>
	/// <returns>返回提取出的查询参数字典。</returns>
	public static Dictionary<string, string?>? GetQueryParamDictionaryInUri(
	       this string? uriString,
	       bool isIncludeFragment = false)
	{
		if (string.IsNullOrEmpty(uriString))
		{
			return null;
		}

		if (isIncludeFragment)
		{
			uriString = uriString.Replace('#', '&');
		}

		var queryParamsString = GetQueryParamsInUri(
			uriString,
			isIncludeFragment);
		if (string.IsNullOrEmpty(queryParamsString))
		{
			return null;
		}

		var queryParamDictionary = new Dictionary<string, string?>();
		var queryParamStrings = queryParamsString.Split('&');
		if (queryParamStrings != null)
		{
			foreach (var queryParamString in queryParamStrings)
			{
				if (string.IsNullOrEmpty(queryParamString))
				{
					continue;
				}

				var queryParamKeyValue = queryParamString.Split('=');
				if (queryParamKeyValue == null
					|| queryParamKeyValue.Length < 1)
				{
					continue;
				}

				var queryParamKey
					= queryParamKeyValue[0].StringByDecodeInUriParam();
				var queryParamValue
					= queryParamKeyValue.Length > 1
					? queryParamKeyValue[1].StringByDecodeInUriParam()
					: null;
				// !!!
				queryParamDictionary.Add(queryParamKey, queryParamValue);
				// !!!
			}
		}
		return queryParamDictionary;
	}

	/// <summary>
	/// 提取当前“Uri字符串”中的锚点部分。
	/// </summary>
	/// <param name="uriString">当前“Uri字符串”。</param>
	/// <returns>返回提取出的锚点部分字符串。</returns>
	public static string? GetFragmentInUri(
	       this string? uriString)
	{
		if (string.IsNullOrEmpty(uriString))
		{
			return null;
		}

		var indexOfFragmentDelimiter = uriString.IndexOf('#');
		if (indexOfFragmentDelimiter < 0)
		{
			return null;
		}
		var uriFragment = uriString[(indexOfFragmentDelimiter + 1)..];
		{ }
		return uriFragment;
	}

	/// <summary>
	/// 生成格式合法的文件名，或资源名，通常用于截取文件路径中的文件名部分。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="isContainsFileExtensionName">是否包含扩展名，默认为：true。</param>
	/// <returns>格式合法的文件名，或资源名。</returns>
	public static string? ToFileName(
	    this string? str,
	    bool isContainsFileExtensionName = true,
	    char? directorySeparatorChar = null)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}

		directorySeparatorChar ??= System.IO.Path.DirectorySeparatorChar;

		var lastIndexOfDirectorySeparatorChar
		    = str.LastIndexOf(directorySeparatorChar.Value);
		if (lastIndexOfDirectorySeparatorChar < 0)
		{
			return str;
		}

		var fileName = str[(lastIndexOfDirectorySeparatorChar + 1)..];
		if (isContainsFileExtensionName != true)
		{
			var lastDotIndex = fileName.LastIndexOf('.');
			if (lastDotIndex >= 0)
			{
				fileName = fileName[..lastDotIndex];
			}
		}
		if (fileName.Length > 0)
		{
			var questionSymbolIndex = fileName.IndexOf('?');
			if (questionSymbolIndex >= 0)
			{
				fileName = fileName[..questionSymbolIndex];
			}
			var sharpSymbolIndex = fileName.IndexOf('#');
			if (sharpSymbolIndex >= 0)
			{
				fileName = fileName[..sharpSymbolIndex];
			}
		}
		return fileName;
	}

	/// <summary>
	/// 截取获取当前字符串中的文件扩展名，即：最后一个“.”之后的字符串。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="isGetFullExtensionName">是否包含完整的扩展名，即：第一个“.”之后的字符串，默认为：false。</param>
	/// <returns>格式合法的文件名，或资源名。</returns>
	public static string ToFileExtensionName(
	    this string? str,
	    bool isGetFullExtensionName = false,
	    char? directorySeparatorChar = null)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}

		directorySeparatorChar ??= System.IO.Path.DirectorySeparatorChar;

		var fileName = str.Trim();
		var lastIndexOfDirectorySeparatorChar
		    = fileName.LastIndexOf(directorySeparatorChar.Value);
		if ((lastIndexOfDirectorySeparatorChar + 1) == str.Length)
		{
			return string.Empty;
		}
		else if (lastIndexOfDirectorySeparatorChar >= 0)
		{
			fileName = fileName[(lastIndexOfDirectorySeparatorChar + 1)..];
		}

		var dotIndex = isGetFullExtensionName
		    ? fileName.IndexOf('.')
		    : fileName.LastIndexOf('.');
		if (dotIndex < 0)
		{
			return string.Empty;
		}

		string fileExtensionName = fileName[(dotIndex + 1)..];
		{
			var questionMarkIndex = fileExtensionName.IndexOf('?');
			if (questionMarkIndex >= 0)
			{
				fileExtensionName = fileExtensionName[..questionMarkIndex];
			}
		}
		return fileExtensionName;
	}


	/// <summary>
	/// 使用Json反序列化规则，反序列化当前字符串，生成指定类型的对象。
	/// </summary>
	/// <typeparam name="T">指定的对象类型。</typeparam>
	/// <param name="str">当前字符串。</param>
	/// <returns>反序列化当前字符串生成的指定类型的对象。</returns>
	public static object? ToObjectByJsonDeserialize(
	    this string? str,
	    Type objectType,
	    JsonSerializerOptions? jsonSerializerOptions = null)
	{
		if (string.IsNullOrEmpty(str))
		{
			return null;
		}

		object? obj = null;
		if (str?.Length > 0)
		{
			obj = System.Text.Json.JsonSerializer.Deserialize(
			    str,
			    objectType,
			    jsonSerializerOptions ?? Environment.JsonSerializerOptions);
		}
		return obj;
	}

	/// <summary>
	/// 使用Json反序列化规则，反序列化当前字符串，生成指定类型的对象。
	/// </summary>
	/// <typeparam name="T">指定的对象类型。</typeparam>
	/// <param name="str">当前字符串。</param>
	/// <returns>反序列化当前字符串生成的指定类型的对象。</returns>
	public static T? ToObjectByJsonDeserialize<T>(
		this string? str,
		JsonSerializerOptions? jsonSerializerOptions = null)
	{
		if (string.IsNullOrEmpty(str))
		{
			return default;
		}

		T? tObject = default;
		if (str?.Length > 0)
		{
			tObject = System.Text.Json.JsonSerializer.Deserialize<T>(
			    str,
			    jsonSerializerOptions ?? Environment.JsonSerializerOptions);
		}
		return tObject;
	}

	public static bool TryToObjectByJsonDeserialize(
		this string? str,
		Type objectType,
		out object? objectDeserialized,
		JsonSerializerOptions? jsonSerializerOptions = null)
	{
		objectDeserialized = null;
		try
		{
			if (string.IsNullOrEmpty(str))
			{
				return false;
			}

			objectDeserialized = str.ToObjectByJsonDeserialize(
				objectType,
				jsonSerializerOptions);
			return true;
		}
		catch
		{ }
		return false;
	}

	public static bool TryToObjectByJsonDeserialize<ObjectType>(
		this string? str,
		out ObjectType? objectDeserialized,
		JsonSerializerOptions? jsonSerializerOptions = null)
	{
		objectDeserialized = default;
		try
		{
			if (string.IsNullOrEmpty(str))
			{
				return false;
			}

			objectDeserialized = str.ToObjectByJsonDeserialize<ObjectType>(
				jsonSerializerOptions);
			return true;
		}
		catch
		{ }
		return false;
	}

	public static string WithDefault(
		this string? str,
		string defaultString = "[无]",
		bool isStringNullToDefault = true,
		bool isStringEmptyToDefault = true)
	{
		if (str == null)
		{
			if (isStringNullToDefault)
			{
				return defaultString;
			}
		}
		else if (str.Length < 1)
		{
			if (isStringEmptyToDefault)
			{
				return defaultString;
			}
		}
		return str ?? string.Empty;
	}

	/// <summary>
	/// 将当前字符串转为对应的RGBA颜色像素值。
	/// </summary>
	/// <param name="colorString">当前颜色描述字符串。</param>
	/// <param name="red">当前颜色描述字符串对应的“红色”像素值，取值范围：0-255。</param>
	/// <param name="green">当前颜色描述字符串对应的“绿色”像素值，取值范围：0-255。</param>
	/// <param name="blue">当前颜色描述字符串对应的“蓝色”像素值，取值范围：0-255。</param>
	/// <param name="alpha">当前颜色描述字符串对应的“透明度”像素值，取值范围：0.0-1.0。</param>
	/// <returns></returns>
	public static bool ToRGBA(
		this string colorString,
		out byte red,
		out byte green,
		out byte blue,
		out float alpha)
	{
		// !!!
		red = 0;
		green = 0;
		blue = 0;
		alpha = 0.0F;
		// !!!

		if (colorString.Length < 1)
		{
			return false;
		}

		colorString = colorString.Trim();

		if (colorString.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
			|| colorString.StartsWith("&h", StringComparison.OrdinalIgnoreCase))
		{
			var colorValuesString = colorString[2..];
			if (!int.TryParse(
				colorValuesString,
				System.Globalization.NumberStyles.HexNumber,
				null,
				out var colorValue))
			{
				return false;
			}

			red = (byte)(colorValue & 0xFF >> 24);
			green = (byte)(colorValue & 0x00FF >> 16);
			blue = (byte)(colorValue & 0x0000FF >> 8);
			alpha = (colorValue & 0xFF) / 255.0F;
		}
		else if (colorString.StartsWith("rgb(", StringComparison.OrdinalIgnoreCase)
			|| colorString.StartsWith("rgba(", StringComparison.OrdinalIgnoreCase))
		{
			var colorValuesString = colorString.SubstringBetween(
				"(",
				")");
			if (colorValuesString != null)
			{
				var colorValueStrings = colorValuesString.Split(',');
				if (colorValueStrings == null)
				{
					return false;
				}
				if (colorValueStrings.Length > 0)
				{
					if (!int.TryParse(colorValueStrings[0], out var redValue))
					{
						return false;
					}
					// !!!
					red = (byte)redValue;
					// !!!
					////////////////////////////////////////////////
					if (colorValueStrings.Length > 1)
					{
						if (!int.TryParse(colorValueStrings[1], out var greenValue))
						{
							return false;
						}
						// !!!
						green = (byte)greenValue;
						// !!!
						////////////////////////////////////////////////
						if (colorValueStrings.Length > 2)
						{
							if (!int.TryParse(colorValueStrings[2], out var blueValue))
							{
								return false;
							}
							// !!!
							blue = (byte)blueValue;
							// !!!
							////////////////////////////////////////////////
							if (colorValueStrings.Length > 3)
							{
								if (!float.TryParse(colorValueStrings[3], out var alphaValue))
								{
									return false;
								}
								// !!!
								alpha = alphaValue;
								// !!!
							}
						}
					}
				}
			}
		}
		return true;
	}


	/// <summary>
	/// 将当前字符串转为隐私字符串。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="privacyCharsCount">要隐私处理的字符数量。</param>
	/// <param name="privacyStringPart">要隐私处理的字符位置，默认位：StringPartType.Center。</param>
	/// <param name="privacytext">隐私字符文本，默认为：“*”。</param>
	/// <returns>返回经过隐私处理的字符串。</returns>
	public static string ToPrivacyString(
		this string str,
		int privacyCharsCount,
		StringPartType privacyStringPart = StringPartType.Center,
		string? privacytext = "*")
	{
		if (string.IsNullOrEmpty(str))
		{
			return str ?? string.Empty;
		}

		var strLength = str.Length;
		if (privacyCharsCount <= 0)
		{
			privacyCharsCount = strLength;
		}

		int privacyCharsBeginIndex;
		int privacyCharsEndIndex;
		switch (privacyStringPart)
		{
			default:
			case StringPartType.Unknown:
				{
					return str;
				}
			case StringPartType.Left:
				{
					privacyCharsBeginIndex = 0;
				}
				break;
			case StringPartType.Center:
				{
					privacyCharsBeginIndex = (strLength - privacyCharsCount) / 2;
					//if (((strLength - privacyCharsCount) % 2) > 0)
					//{
					//	privacyCharsBeginIndex += 1;
					//}
				}
				break;
			case StringPartType.Right:
				{
					privacyCharsBeginIndex = strLength - privacyCharsCount;
				}
				break;
		}
		if (privacyCharsBeginIndex < 0)
		{
			privacyCharsBeginIndex = 0;
		}
		privacyCharsEndIndex = privacyCharsBeginIndex + privacyCharsCount;
		if (privacyCharsEndIndex > strLength)
		{
			privacyCharsEndIndex = strLength;
			if (strLength - privacyCharsCount >= 0)
			{
				privacyCharsBeginIndex = privacyCharsEndIndex - privacyCharsCount;
			}
			else
			{
				privacyCharsBeginIndex = 0;
			}
		}

		//
		var leftPlaintext = str[..privacyCharsBeginIndex];
		//
		var centerPrivacytext = string.Empty;
		var centerPrivacytextLength = privacyCharsEndIndex - privacyCharsBeginIndex;
		if (centerPrivacytextLength > 0
			&& privacytext?.Length > 0)
		{
			var privacytextLength = privacytext.Length;
			while (centerPrivacytext.Length < centerPrivacytextLength)
			{
				if ((centerPrivacytext.Length + privacytextLength) > centerPrivacytextLength)
				{
					centerPrivacytext += privacytext[..(centerPrivacytextLength - centerPrivacytext.Length)];
				}
				else
				{
					centerPrivacytext += privacytext;
				}
			}
		}
		//
		var rightPlaintext = str[privacyCharsEndIndex..];
		//

		var privacyText = leftPlaintext + centerPrivacytext + rightPlaintext;
		{ }
		return privacyText;
	}

	/// <summary>
	/// 将当前“电话号码”字符串（11位）转为隐私字符串。
	/// </summary>
	/// <param name="email">当前“电话毫秒”字符串。</param>
	/// <param name="privacyCharsCount">要隐私处理的字符数量，默认位“5”。</param>
	/// <param name="privacyStringPart">要隐私处理的字符位置，默认位：StringPartType.Center。</param>
	/// <param name="privacytext">隐私字符文本，默认为：“*”。</param>
	/// <returns>返回经过隐私处理的字符串。</returns>
	[Obsolete("“plaintextCharsCount”将改为“int?”类型。")]
	public static string ToPrivacyStringForPhoneNumber(
		this string phoneNumber,
		int plaintextCharsCount = 7,
		StringPartType privacyStringPart = StringPartType.Center,
		string? privacytext = "*")
	{
		if (plaintextCharsCount == 7
			&& phoneNumber.Length < 11)
		{
			plaintextCharsCount = 4;
		}
		return ToPrivacyString(
			phoneNumber,
			phoneNumber.Length - plaintextCharsCount,
			privacyStringPart,
			privacytext);
	}

	/// <summary>
	/// 将当前“电话号码”字符串（11位）转为隐私字符串。
	/// </summary>
	/// <param name="email">当前“电话毫秒”字符串。</param>
	/// <param name="privacyCharsCount">要隐私处理的字符数量，默认位“5”。</param>
	/// <param name="privacyStringPart">要隐私处理的字符位置，默认位：StringPartType.Center。</param>
	/// <param name="privacytext">隐私字符文本，默认为：“*”。</param>
	/// <returns>返回经过隐私处理的字符串。</returns>
	public static string ToPrivacyStringForPhoneNumber(
		this string phoneNumber,
		int? plaintextCharsCount,
		StringPartType privacyStringPart = StringPartType.Center,
		string? privacytext = "*")
	{
		if (plaintextCharsCount == null)
		{
			plaintextCharsCount = 7;
			if (phoneNumber.Length < 11)
			{
				plaintextCharsCount = 4;
			}
		}
		return ToPrivacyString(
			phoneNumber,
			phoneNumber.Length - plaintextCharsCount.Value,
			privacyStringPart,
			privacytext);
	}

	/// <summary>
	/// 将当前“电子邮箱”字符串（11位）转为隐私字符串。
	/// </summary>
	/// <param name="email">当前“电话毫秒”字符串。</param>
	/// <param name="privacyCharsCount">要隐私处理的字符数量，默认位“5”。</param>
	/// <param name="privacyStringPart">要隐私处理的字符位置，默认位：StringPartType.Center。</param>
	/// <param name="privacytext">隐私字符文本，默认为：“*”。</param>
	/// <returns>返回经过隐私处理的字符串。</returns>
	[Obsolete("“plaintextCharsCount”将改为“int?”类型。")]
	public static string ToPrivacyStringForEMail(
		this string email,
		int plaintextCharsCount = 2,
		StringPartType privacyStringPart = StringPartType.Center,
		string? privacytext = "*")
	{
		return ToPrivacyString(
			email,
			email.Length - plaintextCharsCount,
			privacyStringPart,
			privacytext);
	}

	/// <summary>
	/// 将当前“电子邮箱”字符串（11位）转为隐私字符串。
	/// </summary>
	/// <param name="email">当前“电话毫秒”字符串。</param>
	/// <param name="privacyCharsCount">要隐私处理的字符数量，默认位“5”。</param>
	/// <param name="privacyStringPart">要隐私处理的字符位置，默认位：StringPartType.Center。</param>
	/// <param name="privacytext">隐私字符文本，默认为：“*”。</param>
	/// <returns>返回经过隐私处理的字符串。</returns>
	public static string ToPrivacyStringForEMail(
		this string email,
		int? plaintextCharsCount,
		StringPartType privacyStringPart = StringPartType.Center,
		string? privacytext = "*")
	{
		plaintextCharsCount ??= 2;
		return ToPrivacyString(
			email,
			email.Length - plaintextCharsCount.Value,
			privacyStringPart,
			privacytext);
	}

	/// <summary>
	/// 将当前“电话号码”字符串（11位）转为隐私字符串。
	/// </summary>
	/// <param name="account">当前“电话毫秒”字符串。</param>
	/// <param name="plaintextCharsCount">【注意】不要隐私处理的字符数量，默认位“2”。</param>
	/// <param name="privacyStringPart">要隐私处理的字符位置，默认位：StringPartType.Center。</param>
	/// <param name="privacytext">隐私字符文本，默认为：“*”。</param>
	/// <returns>返回经过隐私处理的字符串。</returns>
	[Obsolete("“plaintextCharsCount”将改为“int?”类型。")]
	public static string ToPrivacyStringForAccount(
		this string account,
		int plaintextCharsCount = 2,
		StringPartType privacyStringPart = StringPartType.Center,
		string? privacytext = "*")
	{
		return account.ToPrivacyString(
			account.Length - plaintextCharsCount,
			privacyStringPart,
			privacytext);
	}

	/// <summary>
	/// 将当前“电话号码”字符串（11位）转为隐私字符串。
	/// </summary>
	/// <param name="account">当前“电话毫秒”字符串。</param>
	/// <param name="plaintextCharsCount">【注意】不要隐私处理的字符数量，默认位“2”。</param>
	/// <param name="privacyStringPart">要隐私处理的字符位置，默认位：StringPartType.Center。</param>
	/// <param name="privacytext">隐私字符文本，默认为：“*”。</param>
	/// <returns>返回经过隐私处理的字符串。</returns
	public static string ToPrivacyStringForAccount(
		this string account,
		int? plaintextCharsCount,
		StringPartType privacyStringPart = StringPartType.Center,
		string? privacytext = "*")
	{
		plaintextCharsCount ??= 2;
		return account.ToPrivacyString(
			account.Length - plaintextCharsCount.Value,
			privacyStringPart,
			privacytext);
	}

	/// <summary>
	/// 将当前“身份证”字符串（18位）转为隐私字符串。
	/// </summary>
	/// <param name="idCardNumber">当前“电话毫秒”字符串。</param>
	/// <param name="privacyCharsCount">要隐私处理的字符数量，默认位“10”。</param>
	/// <param name="privacyStringPart">要隐私处理的字符位置，默认位：StringPartType.Center。</param>
	/// <param name="privacytext">隐私字符文本，默认为：“*”。</param>
	/// <returns>返回经过隐私处理的字符串。</returns>
	[Obsolete("“plaintextCharsCount”将改为“int?”类型。")]
	public static string ToPrivacyStringForCNIdCardNumber(
		this string idCardNumber,
		int plaintextCharsCount = 7,
		StringPartType privacyStringPart = StringPartType.Center,
		string? privacytext = "*")
	{
		return ToPrivacyString(
			idCardNumber,
			idCardNumber.Length - plaintextCharsCount,
			privacyStringPart,
			privacytext);
	}

	/// <summary>
	/// 将当前“身份证”字符串（18位）转为隐私字符串。
	/// </summary>
	/// <param name="idCardNumber">当前“电话毫秒”字符串。</param>
	/// <param name="privacyCharsCount">要隐私处理的字符数量，默认位“10”。</param>
	/// <param name="privacyStringPart">要隐私处理的字符位置，默认位：StringPartType.Center。</param>
	/// <param name="privacytext">隐私字符文本，默认为：“*”。</param>
	/// <returns>返回经过隐私处理的字符串。</returns>
	public static string ToPrivacyStringForCNIdCardNumber(
		this string idCardNumber,
		int? plaintextCharsCount,
		StringPartType privacyStringPart = StringPartType.Center,
		string? privacytext = "*")
	{
		plaintextCharsCount ??= 7;
		return ToPrivacyString(
			idCardNumber,
			idCardNumber.Length - plaintextCharsCount.Value,
			privacyStringPart,
			privacytext);
	}

	/// <summary>
	/// 不区分大小写的比较两个字符串是否相当。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="anotherStr">要比较的另一个字符串。</param>
	/// <param name="isNullEqualsEmpty">是否空字符串与长度为零的字符串视为相同。</param>
	/// <returns>两个字符串相等时，返回：true，否则返回：false。</returns>
	public static bool EqualsIgnoreCase(
		this string? str,
		string? anotherStr,
		bool isNullEqualsEmpty = true)
	{
		if (str == anotherStr)
		{
			return true;
		}
		else if (str != null)
		{
			if (str.Length < 1
				&& anotherStr == null
				&& isNullEqualsEmpty)
			{
				return true;
			}
			else if (str.Equals(anotherStr, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		else if (str == null
			&& anotherStr?.Length < 1
			&& isNullEqualsEmpty)
		{
			return true;
		}
		return false;
	}

	/// <summary>
	/// 不区分大小写的从前向后查找指定字符串的位置。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="keywords">要查找的字符串。</param>
	/// <param name="startIndex">指定起始的查找位置。</param>
	/// <param name="comparisonType">字符串的比较类型，默认未：StringComparison.OrdinalIgnoreCase</param>
	/// <returns>指定字符串在当前字符串中的位置，未找到时返回：-1 。</returns>
	public static int IndexOfIgnoreCase(
		this string? str,
		string? keywords,
		int startIndex,
		StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
	{
		if (string.IsNullOrEmpty(str))
		{
			return -1;
		}
		if (keywords == null
			|| keywords.Length < 1)
		{
			return -1;
		}
		if (startIndex >= str.Length)
		{
			return -1;
		}
		if (startIndex < 0)
		{
			startIndex = 0;
		}

		return str.IndexOf(
			keywords,
			startIndex,
			comparisonType);
	}

	/// <summary>
	/// 不区分大小写的从前向后查找指定字符串的位置。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="keywords">要查找的字符串。</param>
	/// <param name="comparisonType">字符串的比较类型，默认未：StringComparison.OrdinalIgnoreCase</param>
	/// <returns>指定字符串在当前字符串中的位置，未找到时返回：-1 。</returns>
	public static int IndexOfIgnoreCase(
		this string? str,
		string? keywords,
		StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
	{
		return StringExtension.IndexOfIgnoreCase(
			str,
			keywords,
			0,
			comparisonType);
	}

	/// <summary>
	/// 不区分大小写的从后向前查找指定字符串的位置。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="keywords">要查找的字符串。</param>
	/// <param name="startIndex">指定起始的查找位置。</param>
	/// <param name="comparisonType">字符串的比较类型，默认未：StringComparison.OrdinalIgnoreCase</param>
	/// <returns>指定字符串在当前字符串中的位置，未找到时返回：-1 。</returns>
	public static int LastIndexOfIgnoreCase(
		this string? str,
		string? keywords,
		int startIndex,
		StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
	{
		if (string.IsNullOrEmpty(str))
		{
			return -1;
		}
		if (keywords == null
			|| keywords.Length < 1)
		{
			return -1;
		}
		if (startIndex >= str.Length)
		{
			return -1;
		}
		if (startIndex < 0)
		{
			startIndex = 0;
		}
		return str.LastIndexOf(
			keywords,
			startIndex,
			comparisonType);
	}

	/// <summary>
	/// 不区分大小写的从后向前查找指定字符串的位置。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="keywords">要查找的字符串。</param>
	/// <param name="comparisonType">字符串的比较类型，默认未：StringComparison.OrdinalIgnoreCase</param>
	/// <returns>指定字符串在当前字符串中的位置，未找到时返回：-1 。</returns>
	public static int LastIndexOfIgnoreCase(
		this string? str,
		string? keywords,
		StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
	{
		return StringExtension.LastIndexOfIgnoreCase(
			str,
			keywords,
			0,
			comparisonType);
	}

	/// <summary>
	/// 当前字符串是否为纯数字字符串。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <param name="isIntNumber">是否只是整形数字。</param>
	/// <returns>字符串是否为纯数字字符串时，返回：true，否则返回：false。</returns>
	public static bool IsNumberString(
	    this string? str,
	    bool isIntNumber = true)
	{
		if (str == null
		    || str.Length < 1)
		{
			return false;
		}
		else if (isIntNumber)
		{
			for (var charIndex = str.Length - 1;
			    charIndex >= 0;
			    charIndex--)
			{
				var character = str[charIndex];
				if (character == '-')
				{
					if (charIndex != 0)
					{
						return false;
					}
				}
				else if (character < '0' || character > '9')
				{
					return false;
				}
			}
		}
		else
		{
			var dotsCount = 0;
			for (var charIndex = str.Length - 1;
			    charIndex >= 0;
			    charIndex--)
			{
				var character = str[charIndex];
				if (character == '-')
				{
					if (charIndex != 0)
					{
						return false;
					}
				}
				else if (character == '.')
				{
					dotsCount++;
					if (dotsCount > 1)
					{
						return false;
					}
				}
				else if (character < '0' || character > '9')
				{
					return false;
				}
			}
		}
		return true;
	}

	public static bool IsStartWithNumber(this string? stringValue)
	{
		return StringUtil.IsStartWithNumber(stringValue);
	}

	public static bool IsEndWithNumber(this string? stringValue)
	{
		return StringUtil.IsEndWithNumber(stringValue);
	}

	/// <summary>
	/// 当前字符串是否为纯字母字符串。
	/// </summary>
	/// <param name="str">当前字符串。</param>
	/// <returns>字符串是否为纯字母字符串时，返回：true，否则返回：false。</returns>
	public static bool IsAlphabetString(
	    this string? str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return false;
		}

		foreach (var character in str)
		{
			if ((character < 'a' || character > 'z')
			    && (character < 'A' || character > 'Z'))
			{
				return false;
			}
		}
		return true;
	}

	/// <summary>
	/// 当前字符串是否为有效的Uri主机名称。
	/// </summary>
	/// <returns></returns>
	public static bool IsValidUriHostName(this string? str)
	{
		if (str?.Length > 0)
		{
			var dotIndex = str.IndexOf('.');
			if (dotIndex > 1
			    && dotIndex < (str.Length - 1))
			{
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// 当前字符串是否为有效的Uri路径。
	/// </summary>
	/// <returns></returns>
	public static bool IsValidUri(
	    this string? str,
	    string? schemeSpecified = "https")
	{
		if (str?.Length > 0)
		{
			try
			{
				var uri = new Uri(str);
				if (uri != null
				    && uri.Host?.Length > 0)
				{
					var uriScheme = uri.Scheme;
					if (uriScheme?.Length > 0)
					{
						if (schemeSpecified == null
						    || schemeSpecified.Length < 1
						    || schemeSpecified.EqualsIgnoreCase(uriScheme))
						{
							return true;
						}
					}
				}
			}
			catch
			{ }
		}
		return false;
	}

	public static bool IsIn(
		this string key,
		ICollection<string>? strings,
		StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
	{
		if (strings?.IsContains(key, comparisonType) == true)
		{
			return true;
		}
		return false;
	}

	public static bool IsNotIn(
		this string key,
		ICollection<string>? strings,
		StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
	{
		if (strings?.IsNotContains(key, comparisonType) == true)
		{
			return true;
		}
		return false;
	}

	public static async Task<List<ItemMatchResultType>?> GetItemsContainedAsync<MatchItemType, ItemMatchResultType>(
		this string? str,
		IEnumerable<MatchItemType>? matchItems,
		Func<string, MatchItemType, ItemMatchResultType?> toIsStringContainedItem,
		int itemsCountMinToMatchWithTasks = 5,
		int tasksCountToMatch = 10)
	{
		if (string.IsNullOrEmpty(str))
		{
			return null;
		}
		if (matchItems == null)
		{
			return null;
		}
		var matchItemsCount = matchItems.GetCount();
		if (matchItemsCount < 1)
		{
			return null;
		}

		var itemsContained = new List<ItemMatchResultType>();
		if (matchItemsCount < itemsCountMinToMatchWithTasks)
		{
			foreach (var matchItem in matchItems)
			{
				var itemMatchResult = toIsStringContainedItem(
					str,
					matchItem);
				if (itemMatchResult != null)
				{
					// !!!
					itemsContained.Add(itemMatchResult);
					// !!!
				}
			}
		}
		else
		{
			if (tasksCountToMatch > matchItemsCount)
			{
				tasksCountToMatch = matchItemsCount;
			}
			var tasksToMatch = new List<Task>();
			var matchItemsEnumerator = matchItems.GetEnumerator();
			for (var taskIndex = 0;
				taskIndex < tasksCountToMatch;
				taskIndex++)
			{
				tasksToMatch.Add(Task.Run(() =>
				{
					MatchItemType? matchItem = default;
					lock (matchItemsEnumerator)
					{
						if (matchItemsEnumerator.MoveNext())
						{
							matchItem = matchItemsEnumerator.Current;
						}
					}
					if (matchItem != null)
					{

						var itemMatchResult = toIsStringContainedItem(
							str,
							matchItem);
						if (itemMatchResult != null)
						{
							lock (itemsContained)
							{
								// !!!
								itemsContained.Add(itemMatchResult);
								// !!!
							}
						}
					}
				}));
			}
			await Task.WhenAll(tasksToMatch);
		}
		return itemsContained;
	}
}