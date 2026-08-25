namespace BaoXia.Utils.Models;

public class ClientIpInfo
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	/// <summary>
	/// 连接的Ip终结点集合字符串，含端口号。
	/// </summary>
	public string? ConnectionIpEndPointsString { get; set; }

	/// <summary>
	/// 关键Ip地址，不含端口号。
	/// </summary>
	public string? KeyIpAddress { get; set; }

	#endregion


	////////////////////////////////////////////////
	// @静态变量
	////////////////////////////////////////////////

	#region 静态变量

	public static ClientIpInfo New => new();

	#endregion


	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public ClientIpInfo()
	{
	}
	public ClientIpInfo(string? connectionIpEndPointsString, string? keyIpAddress)
	{
		ConnectionIpEndPointsString = connectionIpEndPointsString;
		KeyIpAddress = keyIpAddress;
	}

	public string? GetFirsIpEndPoint(bool isPortNeedRetain = false)
	{
		_ = ConnectionIpEndPoints.TryParseToFirstIpEndPoint(ConnectionIpEndPointsString, out var firstIpEndPoint);
		{ }
		return firstIpEndPoint?.Address?.ToString();
	}

	public string? GetLastIp(bool isPortNeedRetain = false)
	{
		_ = ConnectionIpEndPoints.TryParseToLastIpEndPoint(ConnectionIpEndPointsString, out var firstIpEndPoint);
		{ }
		return firstIpEndPoint?.Address?.ToString();
	}

	#endregion
}
