using System;

namespace BaoXia.Utils.Models;

public class TempTokenInfo
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public string TokenValue { get; init; } = default!;

	public ClientIpInfo ClientIpInfo { get; init; } = default!;

	public double LiveSecondsMaxSpecified { get; set; }

	public DateTimeOffset CreateTime { get; init; } = default!;

	#endregion


	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public TempTokenInfo()
	{ }

	public TempTokenInfo(
		string tokenValue,
		ClientIpInfo clientIpInfo,
		double? liveSecondsMaxSpecified,
		DateTimeOffset createTime)
	{
		TokenValue = tokenValue;
		ClientIpInfo = clientIpInfo;
		if (liveSecondsMaxSpecified != null)
		{
			LiveSecondsMaxSpecified = liveSecondsMaxSpecified.Value;
		}
		CreateTime = createTime;
	}

	#endregion
}