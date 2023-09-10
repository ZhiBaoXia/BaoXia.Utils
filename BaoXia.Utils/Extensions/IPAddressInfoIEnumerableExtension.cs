using BaoXia.Utils.Models;
using System.Collections.Generic;

namespace BaoXia.Utils.Extensions
{
	public static class IPAddressInfoIEnumerableExtension
	{

		////////////////////////////////////////////////
		// @类方法
		////////////////////////////////////////////////

		#region 类方法

		public static bool IsContains(
			this IEnumerable<IPAddressInfo> ipAddressInfes,
			string objectIPAddressString)
		{
			if (objectIPAddressString == null
				|| objectIPAddressString.Length < 1)
			{
				return false;
			}
			if (ipAddressInfes == null)
			{
				return false;
			}

			var objectIPAddressInfo = new IPAddressInfo(objectIPAddressString);
			foreach (var ipAddressInfo in ipAddressInfes)
			{
				if (ipAddressInfo?.Equals(objectIPAddressInfo) == true)
				{
					return true;
				}
			}
			return false;
		}

		#endregion
	}
}
