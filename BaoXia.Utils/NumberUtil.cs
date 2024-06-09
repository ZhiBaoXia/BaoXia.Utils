namespace BaoXia.Utils;

public class NumberUtil
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法


	public static int MinOne(int number)
	{
		if (number >= 1)
		{
			return number;
		}
		return 1;
	}

	public static long MinOne(long number)
	{
		if (number >= 1)
		{
			return number;
		}
		return 1;
	}

	public static float MinOne(float number)
	{
		if (number >= 1)
		{
			return number;
		}
		return 1;
	}

	public static double MinOne(double number)
	{
		if (number >= 1)
		{
			return number;
		}
		return 1;
	}

	#endregion
}