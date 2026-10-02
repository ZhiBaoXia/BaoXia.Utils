namespace BaoXia.Utils.Extensions;

public static class IntExtension
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static double ChangeRateTo(this int currentValue, int? targetValue)
	{
		var finalTargetValue = targetValue ?? 0;
		var changeValue = currentValue - finalTargetValue;
		double changeRate;
		if (finalTargetValue != 0)
		{
			changeRate = (double)changeValue / finalTargetValue;
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

	public static int TryParse(string? intString, int defaultValue = 0)
	{
		if (int.TryParse(intString, out var number))
		{
			return number;
		}
		return defaultValue;
	}


	public static string ToBytesSizeCaption(this int byteSize, int decimalDigits = 2)
	{
		var bytesCountToKB = 1024.0;
		if (byteSize < bytesCountToKB)
		{
			return byteSize + "B";
		}
		var bytesCountToMB = 1024.0 * bytesCountToKB;
		if (byteSize < bytesCountToMB)
		{
			return (byteSize / bytesCountToKB).ToString("F" + decimalDigits) + "KB";
		}
		var bytesCountToGB = 1024.0 * bytesCountToMB;
		if (byteSize < bytesCountToGB)
		{
			return (byteSize / bytesCountToMB).ToString("F" + decimalDigits) + "MB";
		}
		var bytesCountToTB = 1024.0 * bytesCountToGB;
		if (byteSize < bytesCountToTB)
		{
			return (byteSize / bytesCountToGB).ToString("F" + decimalDigits) + "GB";
		}
		return (byteSize / bytesCountToTB).ToString("F" + decimalDigits) + "TB";
	}

	#endregion
}
