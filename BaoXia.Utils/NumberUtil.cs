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

	public static uint FirstNotZero(params uint?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue != 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static int FirstNotZero(params int?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue != 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static ulong FirstNotZero(params ulong?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue != 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static long FirstNotZero(params long?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue != 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static float FirstNotZero(params float?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue != 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static double FirstNotZero(params double?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue != 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static decimal FirstNotZero(params decimal?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue != 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}


	public static uint FirstGreaterZero(params uint?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue > 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static int FirstGreaterZero(params int?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue > 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static ulong FirstGreaterZero(params ulong?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue > 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static long FirstGreaterZero(params long?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue > 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static float FirstGreaterZero(params float?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue > 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static double FirstGreaterZero(params double?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue > 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static decimal FirstGreaterZero(params decimal?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue > 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}


	public static uint FirstLessZero(params uint?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue < 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static int FirstLessZero(params int?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue < 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static ulong FirstLessZero(params ulong?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue < 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static long FirstLessZero(params long?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue < 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static float FirstLessZero(params float?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue < 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static double FirstLessZero(params double?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue < 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	public static decimal FirstLessZero(params decimal?[] numbers)
	{
		foreach (var numberValue in numbers)
		{
			if (numberValue != null
				&& numberValue < 0)
			{
				return numberValue.Value;
			}
		}
		return 0;
	}

	#endregion
}