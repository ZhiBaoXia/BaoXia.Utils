using BaoXia.Utils.Constants;
using System;

namespace BaoXia.Utils.Extensions;

/// <summary>
/// “String”安全扩展类。
/// </summary>
public static class StringHttpExtension
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	extension(string stringValue)
	{
		public void GetIpAddress(out string ipAddress, out int ipPort)
		{
			var indexOfLastColon = stringValue.IndexOf(':');
			if (indexOfLastColon >= 0)
			{
				ipAddress = stringValue[..indexOfLastColon];
				var ipPortString = stringValue[(indexOfLastColon + 1)..];
				_ = int.TryParse(ipPortString, out ipPort);
			}
			else
			{
				ipAddress = stringValue;
				ipPort = 0;
			}
		}

		public string[]? ToClientConnectionAddressInfes()
		{
			if (stringValue.Length < 1)
			{
				return null;
			}

			var clientConnectionAddressInfes = stringValue.Split(
				ClientConnectionAddressInfoConstants.ClientConnectionAddressInfoSparator,
				StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
			{ }
			return clientConnectionAddressInfes;
		}
	}

	#endregion
}
