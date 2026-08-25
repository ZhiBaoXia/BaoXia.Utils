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

	#endregion
}
