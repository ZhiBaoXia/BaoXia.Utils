namespace BaoXia.Utils.Extensions;

public static class NumberExtension
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static int IfNullOrLessThanReturn(
		this int? number,
		int defaultValue,
		int compareValue = 0)
	{
		if (number == null
			|| number < compareValue)
		{
			return defaultValue;
		}
		return number.Value;
	}

	public static float IfNullOrLessThanReturn(
		this float? number,
		float defaultValue,
		float compareValue = 0.0F)
	{
		if (number == null
			|| number < compareValue)
		{
			return defaultValue;
		}
		return number.Value;
	}

	public static double IfNullOrLessThanReturn(
		this double? number,
		double defaultValue,
		double compareValue = 0.0)
	{
		if (number == null
			|| number < compareValue)
		{
			return defaultValue;
		}
		return number.Value;
	}

	public static int IfNullOrLessEqualThanReturn(
		this int? number,
		int defaultValue,
		int compareValue = 0)
	{
		if (number == null
			|| number <= compareValue)
		{
			return defaultValue;
		}
		return number.Value;
	}

	public static float IfNullOrLessEqualThanReturn(
		this float? number,
		float defaultValue,
		float compareValue = 0.0F)
	{
		if (number == null
			|| number <= compareValue)
		{
			return defaultValue;
		}
		return number.Value;
	}

	public static double IfNullOrLessEqualThanReturn(
		this double? number,
		double defaultValue,
		double compareValue = 0.0)
	{
		if (number == null
			|| number <= compareValue)
		{
			return defaultValue;
		}
		return number.Value;
	}

	public static int IfNullOrGreaterThanReturn(
		this int? number,
		int defaultValue,
		int compareValue = 0)
	{
		if (number == null
			|| number > compareValue)
		{
			return defaultValue;
		}
		return number.Value;
	}

	public static float IfNullOrGreaterThanReturn(
		this float? number,
		float defaultValue,
		float compareValue = 0.0F)
	{
		if (number == null
			|| number > compareValue)
		{
			return defaultValue;
		}
		return number.Value;
	}

	public static double IfNullOrGreaterThanReturn(
		this double? number,
		double defaultValue,
		double compareValue = 0.0)
	{
		if (number == null
			|| number > compareValue)
		{
			return defaultValue;
		}
		return number.Value;
	}

	public static int IfNullOrGreaterEqualThanReturn(
		this int? number,
		int defaultValue,
		int compareValue = 0)
	{
		if (number == null
			|| number >= compareValue)
		{
			return defaultValue;
		}
		return number.Value;
	}

	public static float IfNullOrGreaterEqualThanReturn(
		this float? number,
		float defaultValue,
		float compareValue = 0.0F)
	{
		if (number == null
			|| number >= compareValue)
		{
			return defaultValue;
		}
		return number.Value;
	}

	public static double IfNullOrGreaterEqualThanReturn(
		this double? number,
		double defaultValue,
		double compareValue = 0.0)
	{
		if (number == null
			|| number >= compareValue)
		{
			return defaultValue;
		}
		return number.Value;
	}


	public static uint GreaterZeroOr(
		this uint? currentValue,
		uint defaultValue)
	{
		if (currentValue > 0)
		{
			return currentValue.Value;
		}
		return defaultValue;
	}

	public static uint GreaterZeroOr(
		this uint currentValue,
		uint defaultValue)
	{
		if (currentValue > 0)
		{
			return currentValue;
		}
		return defaultValue;
	}

	public static int GreaterZeroOr(
		this int? currentValue,
		int defaultValue)
	{
		if (currentValue > 0)
		{
			return currentValue.Value;
		}
		return defaultValue;
	}

	public static int GreaterZeroOr(
		this int currentValue,
		int defaultValue)
	{
		if (currentValue > 0)
		{
			return currentValue;
		}
		return defaultValue;
	}

	public static ulong GreaterZeroOr(
		this ulong? currentValue,
		ulong defaultValue)
	{
		if (currentValue > 0)
		{
			return currentValue.Value;
		}
		return defaultValue;
	}

	public static ulong GreaterZeroOr(
		this ulong currentValue,
		ulong defaultValue)
	{
		if (currentValue > 0)
		{
			return currentValue;
		}
		return defaultValue;
	}

	public static long GreaterZeroOr(
		this long? currentValue,
		long defaultValue)
	{
		if (currentValue > 0)
		{
			return currentValue.Value;
		}
		return defaultValue;
	}

	public static long GreaterZeroOr(
		this long currentValue,
		long defaultValue)
	{
		if (currentValue > 0)
		{
			return currentValue;
		}
		return defaultValue;
	}

	public static float GreaterZeroOr(
		this float? currentValue,
		float defaultValue)
	{
		if (currentValue > 0)
		{
			return currentValue.Value;
		}
		return defaultValue;
	}

	public static float GreaterZeroOr(
		this float currentValue,
		float defaultValue)
	{
		if (currentValue > 0)
		{
			return currentValue;
		}
		return defaultValue;
	}

	public static double GreaterZeroOr(
		this double? currentValue,
		double defaultValue)
	{
		if (currentValue > 0)
		{
			return currentValue.Value;
		}
		return defaultValue;
	}

	public static double GreaterZeroOr(
		this double currentValue,
		double defaultValue)
	{
		if (currentValue > 0)
		{
			return currentValue;
		}
		return defaultValue;
	}

	public static decimal GreaterZeroOr(
		this decimal? currentValue,
		decimal defaultValue)
	{
		if (currentValue > 0)
		{
			return currentValue.Value;
		}
		return defaultValue;
	}

	public static decimal GreaterZeroOr(
		this decimal currentValue,
		decimal defaultValue)
	{
		if (currentValue > 0)
		{
			return currentValue;
		}
		return defaultValue;
	}




	public static uint GreaterEqualZeroOr(
		this uint? currentValue,
		uint defaultValue)
	{
		if (currentValue >= 0)
		{
			return currentValue.Value;
		}
		return defaultValue;
	}

	//public static uint GreaterEqualZeroOr(
	//	this uint currentValue,
	//	uint defaultValue)
	//{
	//	if (currentValue >= 0)
	//	{
	//		return currentValue;
	//	}
	//	return defaultValue;
	//}

	public static int GreaterEqualZeroOr(
		this int? currentValue,
		int defaultValue)
	{
		if (currentValue >= 0)
		{
			return currentValue.Value;
		}
		return defaultValue;
	}

	public static int GreaterEqualZeroOr(
		this int currentValue,
		int defaultValue)
	{
		if (currentValue >= 0)
		{
			return currentValue;
		}
		return defaultValue;
	}

	public static ulong GreaterEqualZeroOr(
		this ulong? currentValue,
		ulong defaultValue)
	{
		if (currentValue >= 0)
		{
			return currentValue.Value;
		}
		return defaultValue;
	}

	//public static ulong GreaterEqualZeroOr(
	//	this ulong currentValue,
	//	ulong defaultValue)
	//{
	//	if (currentValue >= 0)
	//	{
	//		return currentValue;
	//	}
	//	return defaultValue;
	//}

	public static long GreaterEqualZeroOr(
		this long? currentValue,
		long defaultValue)
	{
		if (currentValue >= 0)
		{
			return currentValue.Value;
		}
		return defaultValue;
	}

	public static long GreaterEqualZeroOr(
		this long currentValue,
		long defaultValue)
	{
		if (currentValue >= 0)
		{
			return currentValue;
		}
		return defaultValue;
	}

	public static float GreaterEqualZeroOr(
		this float? currentValue,
		float defaultValue)
	{
		if (currentValue >= 0)
		{
			return currentValue.Value;
		}
		return defaultValue;
	}

	public static float GreaterEqualZeroOr(
		this float currentValue,
		float defaultValue)
	{
		if (currentValue >= 0)
		{
			return currentValue;
		}
		return defaultValue;
	}

	public static double GreaterEqualZeroOr(
		this double? currentValue,
		double defaultValue)
	{
		if (currentValue >= 0)
		{
			return currentValue.Value;
		}
		return defaultValue;
	}

	public static double GreaterEqualZeroOr(
		this double currentValue,
		double defaultValue)
	{
		if (currentValue >= 0)
		{
			return currentValue;
		}
		return defaultValue;
	}

	public static decimal GreaterEqualZeroOr(
		this decimal? currentValue,
		decimal defaultValue)
	{
		if (currentValue >= 0)
		{
			return currentValue.Value;
		}
		return defaultValue;
	}

	public static decimal GreaterEqualZeroOr(
		this decimal currentValue,
		decimal defaultValue)
	{
		if (currentValue >= 0)
		{
			return currentValue;
		}
		return defaultValue;
	}


	#endregion
}