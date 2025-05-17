namespace BaoXia.Utils;

public static class CharUtil
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static bool IsAlphabetChar(
		char character)
	{
		if (character >= 'a'
			&& character <= 'z')
		{
			return true;
		}
		if (character >= 'A'
			&& character <= 'Z')
		{
			return true;
		}
		return false;
	}

	public static bool IsNumberChar(
		char character,
		bool isDotValid = false,
		bool isSymbolValid = false)
	{
		if (character >= '0'
			&& character <= '9')
		{
			return true;
		}
		if (character == '.')
		{
			return isDotValid;
		}
		if (isSymbolValid)
		{
			if (character == '-'
				|| character == '+'
				|| character == '*'
				|| character == '/'
				|| character == '^')
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsInvisibleChar(
		char character,
		bool isBlankInvisible = true)
	{
		if (char.IsControl(character))
		{
			return true;
		}
		if (character == ' '
			&& isBlankInvisible)
		{
			return true;
		}
		return false;
	}

	public static bool IsVisibleChar(
		char character,
		bool isBlankInvisible = true)
	{
		return !IsInvisibleChar(
			character,
			isBlankInvisible);
	}

	public static bool IsPhoneNumberChar(char chacter)
	{
		if (CharUtil.IsNumberChar(chacter)
			|| chacter == '-'
			|| chacter == '+')
		{
			return true;
		}
		return false;
	}

	public static bool IsEMailChar(char chacter)
	{
		if (IsNumberChar(chacter)
			|| IsAlphabetChar(chacter)
			|| chacter == '.'
			|| chacter == '-'
			|| chacter == '_')
		{
			return true;
		}
		return false;
	}

	public static bool IsEnglishAccount(
		char chacter,
		bool isFirstChar = false)
	{
		if (IsAlphabetChar(chacter))
		{
			return true;
		}
		if (isFirstChar)
		{
			return false;
		}
		if (IsNumberChar(chacter)
			|| chacter == '_'
			|| chacter == '-')
		{
			return true;
		}
		return false;
	}

	public static bool IsEquals(
		char currentChar,
		char anotherChar,
		bool isIgnoreCase = false)
	{
		if (currentChar == anotherChar)
		{
			return true;
		}
		if (isIgnoreCase == false)
		{
			return false;
		}
		else if (currentChar >= 'a'
			&& currentChar <= 'z')
		{
			if (anotherChar >= 'A'
				&& anotherChar <= 'Z')
			{
				return true;
			}
		}
		else if (currentChar >= 'A'
			&& currentChar <= 'Z')
		{
			if (anotherChar >= 'a'
				&& anotherChar <= 'z')
			{
				return true;
			}
		}
		return false;
	}

	#endregion
}