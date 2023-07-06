using System.Collections.Generic;

namespace BaoXia.Utils
{
	public class ListUtil
	{
		////////////////////////////////////////////////
		// @类方法
		////////////////////////////////////////////////

		#region 类方法

		public static bool IsEmpty<T>(List<T>? list)
		{
			if (list?.Count > 0)
			{
				return false;
			}
			return true;
		}

		public static bool IsNotEmpty<T>(List<T>? list)
		{
			return !ListUtil.IsEmpty(list);
		}

		#endregion
	}
}
