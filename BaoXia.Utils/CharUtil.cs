namespace BaoXia.Utils;

public class CharUtil
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

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