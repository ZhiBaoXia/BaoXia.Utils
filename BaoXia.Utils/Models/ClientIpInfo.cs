namespace BaoXia.Utils.Models;

public class ClientIpInfo
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public string? IpAddressChain { get; set; }

	public string? ConnectionIPEndPointsString { get; set; }

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
	public ClientIpInfo(string? ipAddressChain, int ipPort)
	{
		IpAddressChain = ipAddressChain;
		IpPortLast = ipPort;
	}

	public string GetFirstIp(bool isPortNeedRetain = false)
	{
		var clientIpAddressFirst = string.Empty;
		if (IpAddressChain is { } ipAddressChain)
		{
			var indexOfLastComma = ipAddressChain.IndexOf(',');
			if (indexOfLastComma >= 0)
			{
				clientIpAddressFirst = ipAddressChain[..indexOfLastComma];
			}
			else
			{
				clientIpAddressFirst = ipAddressChain;
				if (!isPortNeedRetain)
				{
					var indexOfColon = clientIpAddressFirst.IndexOf(':');
					if (indexOfColon >= 0)
					{
						clientIpAddressFirst = clientIpAddressFirst[..indexOfColon];
					}
				}
			}
		}
		return clientIpAddressFirst;
	}

	public string? GetLastIp(bool isPortNeedRetain = false)
	{
		if (IpAddressChain is not { } ipAddressChain)
		{
			return null;
		}
		string clientIpAddressLast;
		var indexOfLastComma = ipAddressChain.LastIndexOf(',');
		if (indexOfLastComma >= 0)
		{
			clientIpAddressLast = ipAddressChain[(indexOfLastComma + 1)..];
		}
		else
		{
			clientIpAddressLast = ipAddressChain;
		}

		if (clientIpAddressLast != null && !isPortNeedRetain)
		{
			var indexOfColon = clientIpAddressLast.IndexOf(':');
			if (indexOfColon >= 0)
			{
				clientIpAddressLast = clientIpAddressLast[..indexOfColon];
			}
		}
		return clientIpAddressLast;
	}

	#endregion
}
