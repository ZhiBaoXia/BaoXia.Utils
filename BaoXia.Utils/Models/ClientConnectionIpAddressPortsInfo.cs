using System.Collections.Generic;

namespace BaoXia.Utils.Models;

public class ClientConnectionIpAddressPortsInfo
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public List<string>? IpAddressPortList { get; set; }


	public string? FirstClientIpAddress { get; set; }

	public int FirstClientIpPort { get; set; }


	public string? LastClientIpAddress { get; set; }

	public int LastClientIpPort { get; set; }


	public string? BxGatewayClientIpAddress { get; set; }

	public int BxGatewayClientIpPort { get; set; }


	#endregion
}