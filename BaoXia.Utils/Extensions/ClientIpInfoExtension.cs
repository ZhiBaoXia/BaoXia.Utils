using BaoXia.Utils.Models;

namespace BaoXia.Utils.Extensions;

public static class ClientIpInfoExtension
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	extension(ClientIpInfo clientIpInfo)
	{
		public ConnectionIPEndPoints? ToConnectionIPEndPoints()
		{
			_ = ConnectionIPEndPoints.TryParse(clientIpInfo.ConnectionIPEndPointsString, out var connectionIPEndPoints);
			{ }
			return connectionIPEndPoints;
		}
	}

	#endregion
}