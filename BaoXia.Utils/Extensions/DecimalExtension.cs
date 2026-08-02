namespace BaoXia.Utils.Extensions;

public static class DecimalExtension
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static double ChangeRateTo(this decimal currentValue, decimal targetValue)
	{
		var changeValue = currentValue - targetValue;
		double changeRate;
		if (targetValue != 0)
		{
			changeRate = (double)(changeValue / targetValue);
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