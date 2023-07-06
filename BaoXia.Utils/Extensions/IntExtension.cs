namespace BaoXia.Utils.Extensions
{
	public static class IntExtension
	{
		public static int TryParse(string? intString, int defaultValue = 0)
		{
			if (int.TryParse(intString, out var number))
			{
				return number;
			}
			return defaultValue;
		}
	}
}
