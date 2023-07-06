﻿namespace BaoXia.Utils
{
	public class ArrayUtil
	{

		////////////////////////////////////////////////
		// @类方法
		////////////////////////////////////////////////

		#region 类方法

		public static bool IsEmpty<T>(T[]? array)
		{
			if (array?.Length > 0)
			{
				return false;
			}
			return true;
		}

		public static bool IsNotEmpty<T>(T[]? array)
		{
			return !ArrayUtil.IsEmpty(array);
		}

		#endregion
	}
}
