namespace BaoXia.Utils;

public class ValueIn8421Util
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static bool IsContains(int originalValue, int objectValue)
	{
		return (originalValue & objectValue) == objectValue;
	}

	public static bool IsNotContains(int originalValue, int objectValue)
	{
		return !IsContains(originalValue, objectValue);
	}

	public static int SetValue(int originalValue, int objectValue)
	{
		originalValue |= objectValue;
		//
		return originalValue;
	}

	public static int RemoveValue(int originalValue, int objectValue)
	{
		originalValue &= ~objectValue;
		//
		return originalValue;
	}

	#endregion
}