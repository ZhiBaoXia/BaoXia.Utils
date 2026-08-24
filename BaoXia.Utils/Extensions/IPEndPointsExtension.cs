using BaoXia.Utils.Constants;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BaoXia.Utils.Extensions;

/// <summary>
/// “String”安全扩展类。
/// </summary>
public static class IPEndPointsExtension
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	extension(IEnumerable<IPEndPoint>? ipEndPoints)
	{
		public string? ToClientConnectionIPEndPointsString()
		{
			if (ipEndPoints.IsEmpty())
			{
				return null;
			}
			var stringBuilder = new StringBuilder();
			foreach (var clientConnectionAddressInfo in ipEndPoints)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(ConnectionIPEndPointConstants.ConnectionIPEndPointsSparator);
				}
				//
				stringBuilder.Append(clientConnectionAddressInfo.ToString());
				//
			}
			return stringBuilder.ToString();
		}
	}

	#endregion
}
