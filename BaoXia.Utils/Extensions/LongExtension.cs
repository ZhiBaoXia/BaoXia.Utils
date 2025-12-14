namespace BaoXia.Utils.Extensions
{
	public static class LongExtension
	{
		public static long TryParse(string? longString, long defaultValue = 0L)
		{
			if (long.TryParse(longString, out var number))
			{
				return number;
			}
			return defaultValue;
		}
		public static string ToBytesSizeCaption(this long byteSize, int decimalDigits = 2)
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
	}
}
