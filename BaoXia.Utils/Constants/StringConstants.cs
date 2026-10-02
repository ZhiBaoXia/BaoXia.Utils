namespace BaoXia.Utils.Constants;

public class StringConstants
{
	////////////////////////////////////////////////
	// @静态常量
	////////////////////////////////////////////////

	#region 静态常量

	/// <summary>
	/// 数字的静态常量。
	/// </summary>
	public static readonly char[] ArabicNumeralChars = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];

	/// <summary>
	/// 小写和大写字母的静态常量。
	/// </summary>
	public static readonly char[] AlphabetChars = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];

	/// <summary>
	/// 小写字母的静态常量。
	/// </summary>
	public static readonly char[] AlphabetCharsInLowercase = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'];

	/// <summary>
	/// 大写字母的静态常量。
	/// </summary>
	public static readonly char[] AlphabetCharsInUppercase = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];

	/// <summary>
	/// 字母与数字的静态常量。
	/// </summary>
	public static readonly char[] ArabicNumeralAndAlphabetChars = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];

	/// <summary>
	/// 数字和小写字母的静态常量。
	/// </summary>
	public static readonly char[] ArabicNumeralAndAlphabetCharsInLowercase = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'];

	/// <summary>
	/// 数字和大写字母的静态常量。
	/// </summary>
	public static readonly char[] ArabicNumeralAndAlphabetCharsInUppercase = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];


	/// <summary>
	/// 数字和全部字符常量。
	/// </summary>
	public static readonly char[][] AllChars = [ ArabicNumeralChars, AlphabetChars, AlphabetCharsInLowercase, AlphabetCharsInUppercase,
		ArabicNumeralAndAlphabetChars,ArabicNumeralAndAlphabetCharsInLowercase, ArabicNumeralAndAlphabetCharsInUppercase ];

	#endregion
}