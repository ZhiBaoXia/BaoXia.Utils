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
		public ConnectionIpEndPoints? ToConnectionIpEndPoints()
		{
			_ = ConnectionIpEndPoints.TryParse(clientIpInfo.ConnectionIpEndPointsString, out var connectionIpEndPoints);
			{ }
			return connectionIpEndPoints;
		}
	}

	#endregion
}