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
	public ClientIpInfo(string? ipAddressChain, int ipPort)
	{
		IpAddressChain = ipAddressChain;
		IpPortLast = ipPort;
	}

	public string GetFirstIp()
	{
		var openOrderClientIpAddressLast = string.Empty;
		if (IpAddressChain is { } ipAddressChain)
		{
			var indexOfLastComma = ipAddressChain.IndexOf(',');
			if (indexOfLastComma >= 0)
			{
				openOrderClientIpAddressLast = ipAddressChain[..indexOfLastComma];
			}
			else
			{
				openOrderClientIpAddressLast = ipAddressChain;
			}
		}
		return openOrderClientIpAddressLast;
	}

	public string? GetLastIp(bool isPortNeedRetain = false)
	{
		if (IpAddressChain is not { } ipAddressChain)
		{
			return null;
		}
		var openOrderClientIpAddressLast = string.Empty;
		var indexOfLastComma = ipAddressChain.LastIndexOf(',');
		if (indexOfLastComma >= 0)
		{
			openOrderClientIpAddressLast = ipAddressChain[(indexOfLastComma + 1)..];
		}
		else
		{
			openOrderClientIpAddressLast = ipAddressChain;
		}

		if (openOrderClientIpAddressLast != null && !isPortNeedRetain)
		{
			var indexOfColon = openOrderClientIpAddressLast.LastIndexOf(':');
			if (indexOfColon >= 0)
			{
				openOrderClientIpAddressLast = openOrderClientIpAddressLast[..indexOfColon];
			}
		}
		return openOrderClientIpAddressLast;
	}

	#endregion
}
