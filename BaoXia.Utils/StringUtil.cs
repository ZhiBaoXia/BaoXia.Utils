using BaoXia.Utils.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;

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

	public static string ToStringWithSeparator(
		string? separator,
		params string[]? strings)
	{
		return strings.ToStringWithSeparator(separator);
	}

	#endregion
}
