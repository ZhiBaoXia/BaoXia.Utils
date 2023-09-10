using BaoXia.Utils.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace BaoXia.Utils
{
	public class StringUtil
	{
		////////////////////////////////////////////////
		// @类方法
		////////////////////////////////////////////////

		#region 类方法

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

		public static string ToStringWithSeparator(
			string? separator,
			params string[]? strings)
		{
			return strings.ToStringWithSeparator(separator);
		}

		#endregion
	}
}
