using System.Collections.Generic;

namespace BaoXia.Utils.Models;

public class ClientConnectionAddressInfes
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public List<string>? ClientAddressInfoList { get; set; }


	public string? FirstClientAddressIp { get; set; }

	public int FirstClientAddressPort { get; set; }


	public string? LastClientAddressIp { get; set; }

	public int LastClientAddressPort { get; set; }


	public string? BxGatewayClientAddressIp { get; set; }

	public int BxGatewayClientAddressPort { get; set; }


	#endregion
}