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
	}
}
