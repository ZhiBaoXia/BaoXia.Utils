using BaoXia.Utils.Constants;

namespace BaoXia.Utils.Models;

public class PrivacyInfo(
	PrivacyInfoType type,
	int beginIndex,
	int endIndex,
	string privacyContent)
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public PrivacyInfoType Type { get; set; } = type;

	public int BeginIndex { get; set; } = beginIndex;

	public int EndIndex { get; set; } = endIndex;

	public string PrivacyContent { get; set; } = privacyContent;

	#endregion
}