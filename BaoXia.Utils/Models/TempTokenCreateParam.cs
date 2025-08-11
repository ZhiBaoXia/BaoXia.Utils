namespace BaoXia.Utils.Models;

public class TempTokenCreateParam(
	ClientIpInfo clientIpInfo,
	double? liveSecondsMaxSpecified = null)
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public ClientIpInfo ClientIpInfo { get; init; } = clientIpInfo;

	public double LiveSecondsMaxSpecified { get; set; } = liveSecondsMaxSpecified != null
		? liveSecondsMaxSpecified.Value
		: 0;

	#endregion
}
