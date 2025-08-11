namespace BaoXia.Utils.Models;

public class TempTokenCreateParam(
	ClientIpInfo clientIpInfo)
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public ClientIpInfo ClientIpInfo { get; init; } = clientIpInfo;

	#endregion
}
