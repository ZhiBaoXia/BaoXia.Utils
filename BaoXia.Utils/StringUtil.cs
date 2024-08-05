using BaoXia.Utils.Constants;
using BaoXia.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;

namespace BaoXia.Utils;

public class StringUtil
{
	////////////////////////////////////////////////
	// @类方法，字符串自身判断。
	////////////////////////////////////////////////

	#region 字符串自身判断

	public static bool IsEmpty([NotNullWhen(false)] string? str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return true;
		}
		return false;
	}

	public static bool IsNotEmpty([NotNullWhen(true)] string? str)
	{
		return !StringUtil.IsEmpty(str);
	}

	public static bool IsBlank([NotNullWhen(false)] string? str)
	{
		if (string.IsNullOrWhiteSpace(str))
		{
			return true;
		}
		return false;
	}

	public static bool IsNotBlank([NotNullWhen(true)] string? str)
	{
		return !StringUtil.IsBlank(str);
	}

	public static bool IsIntegralNumber([NotNullWhen(true)] string? str)
	{
		return str.IsNumberString(true);
	}

	public static bool IsNotIntegralNumber([NotNullWhen(false)] string? str)
	{
		return !StringUtil.IsIntegralNumber(str);
	}

	public static bool IsDecimalNumber([NotNullWhen(true)] string? str)
	{
		return str.IsNumberString(false);
	}

	public static bool IsNotDecimalNumber([NotNullWhen(false)] string? str)
	{
		return !StringUtil.IsDecimalNumber(str);
	}

	public static bool IsAlphabet([NotNullWhen(true)] string? str)
	{
		return str.IsAlphabetString();
	}

	public static bool IsNotAlphabet([NotNullWhen(false)] string? str)
	{
		return !StringUtil.IsAlphabet(str);
	}

	#endregion


	////////////////////////////////////////////////
	// @类方法，字符串比较。
	////////////////////////////////////////////////

	#region 类方法，字符串比较。

	/// <summary>
	/// 比较两个字符串是否相同。
	/// </summary>
	/// <param name="strA">要进行比较的第一个字符串。</param>
	/// <param name="strB">要进行比较的第二个字符串。</param>
	/// <param name="stringComparison">字符串的比较类型。</param>
	/// <param name="isNullEqualsEmpty">是否空字符串与长度为零的字符串视为相同。</param>
	/// <returns>两个字符串相同时，返回：true，否则返回：false。</returns>
	public static bool EqualsStrings(
	    string? strA,
	    string? strB,
	    StringComparison stringComparison = StringComparison.Ordinal,
	    bool isNullEqualsEmpty = true)
	{
		if (strA == strB)
		{
			return true;
		}
		else if (strA != null
		    && strB == null)
		{
			if (strA.Length == 0)
			{
				if (isNullEqualsEmpty)
				{
					return true;
				}
			}
		}
		else if (strA == null
		    && strB != null)
		{
			if (strB.Length == 0)
			{
				if (isNullEqualsEmpty)
				{
					return true;
				}
			}
		}
		else if (strA?.Equals(strB, stringComparison) == true)
		{
			return true;
		}
		return false;
	}


	#endregion


	////////////////////////////////////////////////
	// @类方法，其他类型转为字符串。
	////////////////////////////////////////////////

	#region 类方法，其他类型转为字符串。

	/// <summary>
	/// 由整型数值数组，创建字符串，如：由“[1, 2, 3]”，创建字符串“1,2,3”。
	/// </summary>
	/// <param name="intArray">原始的整数数值数组。</param>
	/// <param name="spliter">创建字符串时使用的数值分隔符，默认为“,”。</param>
	/// <param name="numberFormat">指定的数字字符串格式字符串。</param>
	/// <returns>整型数值数组有效时，返回对应的字符串，否则返回“空字符串”。</returns>
	public static string StringWithInts(
		IEnumerable<int>? intArray,
		string? spliter = ",",
		string? numberFormat = null)
	{
		if (EnumerableUtil.IsEmpty(intArray))
		{
			return string.Empty;
		}

		var stringBuilder = new StringBuilder();
		if (spliter?.Length > 0)
		{
			foreach (var intValue in intArray)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(spliter);
				}
				stringBuilder.Append(intValue.ToString(numberFormat));
			}
		}
		else
		{
			foreach (var intValue in intArray)
			{
				stringBuilder.Append(intValue.ToString(numberFormat));
			}
		}
		var nubmersString = stringBuilder.ToString();
		{ }
		return nubmersString;
	}

	/// <summary>
	/// 由整型数值数组，创建字符串，如：由“[1, 2, 3]”，创建字符串“1,2,3”。
	/// </summary>
	/// <param name="longArray">原始的整数数值数组。</param>
	/// <param name="spliter">创建字符串时使用的数值分隔符，默认为“,”。</param>
	/// <param name="numberFormat">指定的数字字符串格式字符串。</param>
	/// <returns>整型数值数组有效时，返回对应的字符串，否则返回“空字符串”。</returns>
	public static string StringWithLongs(
		IEnumerable<long>? longArray,
		string? spliter = ",",
		string? numberFormat = null)
	{
		if (EnumerableUtil.IsEmpty(longArray))
		{
			return string.Empty;
		}

		var stringBuilder = new StringBuilder();
		if (spliter?.Length > 0)
		{
			foreach (var longValue in longArray)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(spliter);
				}
				stringBuilder.Append(longValue.ToString(numberFormat));
			}
		}
		else
		{
			foreach (var longValue in longArray)
			{
				stringBuilder.Append(longValue.ToString(numberFormat));
			}
		}
		var nubmersString = stringBuilder.ToString();
		{ }
		return nubmersString;
	}


	/// <summary>
	/// 由浮点型数值数组，创建字符串，如：由“[1.0, 2.0, 3.0]”，创建字符串“1.0,2.0,3.0”。
	/// </summary>
	/// <param name="floatArray">原始的整数数值数组。</param>
	/// <param name="spliter">创建字符串时使用的数值分隔符，默认为“,”。</param>
	/// <param name="numberFormat">指定的数字字符串格式字符串。</param>
	/// <returns>浮点型数值数组有效时，返回对应的字符串，否则返回“空字符串”。</returns>
	public static string StringWithFloats(
		IEnumerable<float> floatArray,
		string? spliter = ",",
		string? numberFormat = null)
	{
		if (EnumerableUtil.IsEmpty(floatArray))
		{
			return string.Empty;
		}

		var stringBuilder = new StringBuilder();
		if (spliter?.Length > 0)
		{
			foreach (var floatValue in floatArray)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(spliter);
				}
				stringBuilder.Append(floatValue.ToString(numberFormat));
			}
		}
		else
		{
			foreach (var floatValue in floatArray)
			{
				stringBuilder.Append(floatValue.ToString(numberFormat));
			}
		}
		var nubmersString = stringBuilder.ToString();
		{ }
		return nubmersString;
	}

	/// <summary>
	/// 由浮点型数值数组，创建字符串，如：由“[1.0, 2.0, 3.0]”，创建字符串“1.0,2.0,3.0”。
	/// </summary>
	/// <param name="doubleArray">原始的整数数值数组。</param>
	/// <param name="spliter">创建字符串时使用的数值分隔符，默认为“,”。</param>
	/// <param name="numberFormat">指定的数字字符串格式字符串。</param>
	/// <returns>浮点型数值数组有效时，返回对应的字符串，否则返回“空字符串”。</returns>
	public static string StringWithDoubles(
		IEnumerable<double>? doubleArray,
		string? spliter = ",",
		string? numberFormat = null)
	{
		if (EnumerableUtil.IsEmpty(doubleArray))
		{
			return string.Empty;
		}

		var stringBuilder = new StringBuilder();
		if (spliter?.Length > 0)
		{
			foreach (var doubleValue in doubleArray)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(spliter);
				}
				stringBuilder.Append(doubleValue.ToString(numberFormat));
			}
		}
		else
		{
			foreach (var doubleValue in doubleArray)
			{
				stringBuilder.Append(doubleValue.ToString(numberFormat));
			}
		}
		var nubmersString = stringBuilder.ToString();
		{ }
		return nubmersString;
	}

	/// <summary>
	/// 由高精度数值数组，创建字符串，如：由“[1.0, 2.0, 3.0]”，创建字符串“1.0,2.0,3.0”。
	/// </summary>
	/// <param name="decimalArray">原始的整数数值数组。</param>
	/// <param name="spliter">创建字符串时使用的数值分隔符，默认为“,”。</param>
	/// <param name="numberFormat">指定的数字字符串格式字符串。</param>
	/// <returns>浮点型数值数组有效时，返回对应的字符串，否则返回“空字符串”。</returns>
	public static string StringWithDicemals(
		IEnumerable<decimal>? decimalArray,
		string? spliter = ",",
		string? numberFormat = null)
	{
		if (EnumerableUtil.IsEmpty(decimalArray))
		{
			return string.Empty;
		}

		var stringBuilder = new StringBuilder();
		if (spliter?.Length > 0)
		{
			foreach (var decimalValue in decimalArray)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(spliter);
				}
				stringBuilder.Append(decimalValue.ToString(numberFormat));
			}
		}
		else
		{
			foreach (var doubleValue in decimalArray)
			{
				stringBuilder.Append(doubleValue.ToString(numberFormat));
			}
		}
		var nubmersString = stringBuilder.ToString();
		{ }
		return nubmersString;
	}

	/// <summary>
	/// 生成指定长度的随机内容字符串，随机内容只包含英文字母和阿拉伯数字。
	/// </summary>
	/// <param name="randomStringLength">指定的随机字符串长度。</param>
	/// <param name="randomSeek">随机种子值，设置为“0”时，使用系统当前时间戳。</param>
	/// <param name="isOnlyUppercase">是否只使用大写字符串。</param>
	/// <returns>返回指定长度随机内容字符串，随机内容只包含英文字母和阿拉伯数字。</returns>
	public static string StringByFillRandomCharsToLength(
	    int randomStringLength,
	    int randomSeek = 0,
	    bool isOnlyUppercase = true)
	{
		string randomString = string.Empty;
		if (randomSeek == 0)
		{
			randomSeek = DateTime.Now.Millisecond;
		}
		Random random = new(randomSeek);
		if (isOnlyUppercase)
		{
			for (int charIndex = 0;
			    charIndex < randomStringLength;
			    charIndex++)
			{
				var randomCharIndex = random.Next(StringConstants.kArabicNumeralAndAlphabetCharsInUppercase.Length);
				{ }
				randomString += StringConstants.kArabicNumeralAndAlphabetCharsInUppercase[randomCharIndex].ToString();
			}
		}
		else
		{
			for (int charIndex = 0;
			    charIndex < randomStringLength;
			    charIndex++)
			{
				var chars = StringConstants.kArabicNumeralAndAlphabetCharsInUppercase;
				if (random.Next(2) == 1)
				{
					chars = StringConstants.kArabicNumeralAndAlphabetCharsInLowercase;
				}
				var randomCharIndex = random.Next(chars.Length);
				{ }
				randomString += chars[randomCharIndex].ToString();
			}
		}
		return randomString;
	}

	/// <summary>
	/// 生成指定长度的随机内容字符串，随机内容只包含英文字母和阿拉伯数字。
	/// </summary>
	/// <param name="randomStringLength">指定的随机字符串长度。</param>
	/// <param name="randomSeek">随机种子值，设置为“0”时，使用系统当前时间戳。</param>
	/// <param name="isOnlyUppercase">是否只使用大写字符串。</param>
	/// <returns>返回指定长度随机内容字符串，随机内容只包含英文字母和阿拉伯数字。</returns>
	public static string RandomStringInLength(
	    int randomStringLength,
	    int randomSeek = 0,
	    bool isOnlyUppercase = true)
	{
		return StringByFillRandomCharsToLength(
			randomStringLength,
			randomSeek,
			isOnlyUppercase);
	}

	/// <summary>
	/// 由字符串数组，创建字符串，如：由“["a", "b", "c"]”，创建字符串“a,b,c”。
	/// </summary>
	/// <param name="stringArray">原始的整数数值数组。</param>
	/// <param name="separator">创建字符串时使用的数值分隔符，默认为“,”。</param>
	/// <returns>字符串数组有效时，返回对应的字符串，否则返回“空字符串”。</returns>
	public static string StringWithStrings(IEnumerable<string>? stringArray, string? separator = ",")
	{
		var stringBuilder = new StringBuilder();
		if (stringArray != null)
		{
			if (separator?.Length > 0)
			{
				foreach (var stringValue in stringArray)
				{
					if (stringValue.Length > 0)
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.Append(separator);
						}
						stringBuilder.Append(stringValue);
					}
				}
			}
			else
			{
				foreach (var stringValue in stringArray)
				{
					if (stringValue.Length > 0)
					{
						stringBuilder.Append(stringValue);
					}
				}

			}
		}
		var stringAfterJoin = stringBuilder.ToString();
		{ }
		return stringAfterJoin;
	}

	public static string StringWithStringsJoinSeparator(
		string? separator,
		params string[]? strings)
	{
		return StringWithStrings(strings, separator);
	}

	/// <summary>
	/// 使用Utf8编码的字节数组，创建对应的字符串。
	/// </summary>
	/// <param name="utf8Bytes">指定的Utf8编码的字节数组。</param>
	/// <returns>Utf8编码的字节数组，对应的字符串。</returns>
	public static string StringWithUtf8Bytes(ArraySegment<byte> utf8Bytes)
	{
		if (utf8Bytes.Array == null
			|| utf8Bytes.Count < 1)
		{
			return string.Empty;
		}

		var str = System.Text.UTF8Encoding.UTF8.GetString(
			utf8Bytes.Array,
			utf8Bytes.Offset,
			utf8Bytes.Count);
		{ }
		return str;
	}

	/// <summary>
	/// 使用Utf8编码的字节数组，创建对应的字符串。
	/// </summary>
	/// <param name="utf8Bytes">指定的Utf8编码的字节数组。</param>
	/// <returns>Utf8编码的字节数组，对应的字符串。</returns>
	public static string StringWithUtf8Bytes(
		byte[]? utf8Bytes,
		int offset,
		int count)
	{
		if (utf8Bytes == null
			|| count < 1)
		{
			return string.Empty;
		}

		var str = System.Text.UTF8Encoding.UTF8.GetString(
			utf8Bytes,
			offset,
			count);
		{ }
		return str;
	}

	/// <summary>
	/// 使用Utf8编码的字节数组，创建对应的字符串。
	/// </summary>
	/// <param name="utf8Bytes">指定的Utf8编码的字节数组。</param>
	/// <returns>Utf8编码的字节数组，对应的字符串。</returns>
	public static string StringWithUtf8Bytes(byte[]? utf8Bytes)
	{
		if (utf8Bytes == null
			|| utf8Bytes.Length < 1)
		{
			return string.Empty;
		}

		var str = System.Text.UTF8Encoding.UTF8.GetString(utf8Bytes);
		{ }
		return str;
	}

	/// <summary>
	/// 通过序列化指定的对象，生成Json字符串。
	/// </summary>
	/// <param name="obj">要被序列化的对象。</param>
	/// <returns>对象序列化后的Json字符串。</returns>
	public static string StringByJsonSerializeObject(
		object? obj,
		JsonSerializerOptions? jsonSerializerOptions = null)
	{
		if (obj == null)
		{
			return String.Empty;
		}
		var str = System.Text.Json.JsonSerializer.Serialize(
			    obj,
			    jsonSerializerOptions ?? Environment.JsonSerializerOptions);
		{ }
		return str;
	}

	#endregion


	////////////////////////////////////////////////
	// @类方法，其他方法。
	////////////////////////////////////////////////

	#region 类方法

	public static string? FirstNotEmpty(params string?[] strings)
	{
		foreach (var stringValue in strings)
		{
			if (!string.IsNullOrEmpty(stringValue))
			{
				return stringValue;
			}
		}
		return null;
	}

	#endregion
}
