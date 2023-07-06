using BaoXia.Utils.Extensions;

namespace BaoXia.Utils
{
	public class StringUtil
	{
		////////////////////////////////////////////////
		// @类方法
		////////////////////////////////////////////////

		#region 类方法

		public static bool IsEmpty(string? str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return true;
			}
			return false;
		}

		public static bool IsNotEmpty(string? str)
		{
			return !StringUtil.IsEmpty(str);
		}

		public static bool IsBlank(string? str)
		{
			if (string.IsNullOrWhiteSpace(str))
			{
				return true;
			}
			return false;
		}

		public static bool IsNotBlank(string? str)
		{
			return !StringUtil.IsBlank(str);
		}

		public static bool IsIntegralNumber(string? str)
		{
			return str.IsNumberString(true);
		}

		public static bool IsNotIntegralNumber(string? str)
		{
			return !StringUtil.IsIntegralNumber(str);
		}

		public static bool IsDecimalNumber(string? str)
		{
			return str.IsNumberString(false);
		}

		public static bool IsNotDecimalNumber(string? str)
		{
			return !StringUtil.IsDecimalNumber(str);
		}

		public static bool IsAlphabet(string? str)
		{
			return str.IsAlphabetString();
		}

		public static bool IsNotAlphabet(string? str)
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
