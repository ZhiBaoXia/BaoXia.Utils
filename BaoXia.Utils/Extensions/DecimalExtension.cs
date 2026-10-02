namespace BaoXia.Utils.Extensions;

public static class DecimalExtension
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static double ChangeRateTo(this decimal currentValue, decimal? targetValue)
	{
		var finalTargetValue = targetValue ?? 0.0M;
		var changeValue = currentValue - finalTargetValue;
		double changeRate;
		if (finalTargetValue != 0)
		{
			changeRate = (double)(changeValue / finalTargetValue);
		}
		else if (changeValue > 0)
		{
			changeRate = 1;
		}
		else if (changeValue < 0)
		{
			changeRate = -1;
		}
		else
		{
			changeRate = 0;
		}
		return changeRate;
	}

	#endregion
}