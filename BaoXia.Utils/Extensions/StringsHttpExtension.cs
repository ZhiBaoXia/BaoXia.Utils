using BaoXia.Utils.Constants;
using System.Collections.Generic;
using System.Text;

namespace BaoXia.Utils.Extensions;

/// <summary>
/// “String”安全扩展类。
/// </summary>
public static class StringsHttpExtension
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	extension(IEnumerable<string>? clientConnectionAddressInfes)
	{
		public string? ToClientConnectionAddressInfesString()
		{
			if (clientConnectionAddressInfes.IsEmpty())
			{
				return null;
			}
			var stringBuilder = new StringBuilder();
			foreach (var clientConnectionAddressInfo in clientConnectionAddressInfes)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(ClientConnectionAddressInfoConstants.ClientConnectionAddressInfoSparator);
				}
				stringBuilder.Append(clientConnectionAddressInfo);
			}
			return stringBuilder.ToString();
		}
	}

	#endregion
}
