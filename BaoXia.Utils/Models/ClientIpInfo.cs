namespace BaoXia.Utils.Models;

public class ClientIpInfo
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public string? IpAddressChain { get; set; }

	public int IpPortLast { get; set; }

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
	public ClientIpInfo(
		string? ipAddressChain,
		int ipPort)
	{
		IpAddressChain = ipAddressChain;
		IpPortLast = ipPort;
	}

	#endregion
}
