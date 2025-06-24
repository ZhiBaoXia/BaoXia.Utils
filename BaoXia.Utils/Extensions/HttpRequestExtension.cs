using BaoXia.Utils.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BaoXia.Utils.Extensions;

public static class HttpRequestExtension
{
	////////////////////////////////////////////////
	// @静态常量
	////////////////////////////////////////////////

	#region 静态常量

	protected class HttpHeaderKeys
	{
		public const string BxService_Gateway_ClientIp = "BxService-Gateway-ClientIp";
	}

	#endregion


	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	/// <summary>
	/// 获取当前请求的绝对地址。
	/// </summary>
	/// <param name="request">当前Http请求对象。</param>
	/// <returns>当前请求的绝对地址。</returns>
	public static string GetAbsoluteUri(this HttpRequest request)
	{
		var absoluteUri
				= new StringBuilder()
				.Append(request.Scheme)
				.Append("://")
				.Append(request.Host)
				.Append(request.PathBase)
				.Append(request.Path)
				.Append(request.QueryString.Value)
				.ToString();
		{ }
		return absoluteUri;
	}

	public static List<string> GetClientConnectionAddressList(
		this HttpRequest request)
	{
		var clientHttpProxyAddressSet = new HashSet<string>();
		var clientHttpProxyAddressList = new List<string>();

		////////////////////////////////////////////////
		// 1/，获取【Http请求头】中的客户端地址信息。
		////////////////////////////////////////////////
		if (request.Headers is IHeaderDictionary requestHeaders)
		{
			// “X-Forwarded-For”的客户端地址。
			if (requestHeaders.TryGetValue("X-Forwarded-For", out var x_Forwarded_For) == true
				&& x_Forwarded_For.Count > 0)
			{
				foreach (var forward in x_Forwarded_For)
				{
					var forwardClientAddresses = forward?.Split(",", System.StringSplitOptions.RemoveEmptyEntries);
					if (forwardClientAddresses != null)
					{
						foreach (var forwardClientAddress in forwardClientAddresses)
						{
							var forwardClientAddressTrimed = forwardClientAddress.Trim();
							if (forwardClientAddressTrimed?.Length > 0)
							{
								if (clientHttpProxyAddressSet.Contains(forwardClientAddressTrimed) == false)
								{
									// !!!
									clientHttpProxyAddressSet.Add(forwardClientAddressTrimed);
									clientHttpProxyAddressList.Add(forwardClientAddressTrimed);
									// !!!
								}
							}
						}
					}
				}
			}
			// “X-Real-IP”的客户端地址。
			if (requestHeaders.TryGetValue("X-Real-IP", out var x_Real_Ips) == true)
			{
				foreach (var x_Real_Ip in x_Real_Ips)
				{
					var x_Real_IpTrimed = x_Real_Ip?.Trim();
					if (x_Real_IpTrimed?.Length > 0)
					{
						if (clientHttpProxyAddressSet.Contains(x_Real_IpTrimed) == false)
						{
							// !!!
							clientHttpProxyAddressSet.Add(x_Real_IpTrimed);
							clientHttpProxyAddressList.Add(x_Real_IpTrimed);
							// !!!
						}
					}
				}
			}
		}


		////////////////////////////////////////////////
		// 2/，获取【Tcp连接】中的客户端地址信息。
		////////////////////////////////////////////////
		if (request.HttpContext?.Connection is { } tcpIpConnection)
		{
			var remoteIpAddress = tcpIpConnection.RemoteIpAddress?.ToString();
			if (remoteIpAddress?.Length > 0)
			{
				remoteIpAddress += ":" + tcpIpConnection.RemotePort;
				if (clientHttpProxyAddressSet.Contains(remoteIpAddress) == false)
				{
					// !!!
					clientHttpProxyAddressSet.Add(remoteIpAddress);
					clientHttpProxyAddressList.Add(remoteIpAddress);
					// !!!
				}
			}
		}


		////////////////////////////////////////////////
		// 3/，获取【宝匣网关】中的客户端地址信息。
		////////////////////////////////////////////////
		if (request.Headers?.TryGetValue(
			HttpHeaderKeys.BxService_Gateway_ClientIp,
			out var bxService_Gateway_ClientIp) == true
			&& bxService_Gateway_ClientIp.Count > 0)
		{
			foreach (var clientIp in bxService_Gateway_ClientIp)
			{
				var clientIpAddresses = clientIp?.Split(",", System.StringSplitOptions.RemoveEmptyEntries);
				if (clientIpAddresses != null)
				{
					foreach (var clientIpAddress in clientIpAddresses)
					{
						var clientIpAddressTrimed = clientIpAddress.Trim();
						if (clientIpAddressTrimed?.Length > 0)
						{
							if (clientHttpProxyAddressSet.Contains(clientIpAddressTrimed) == false)
							{
								// !!!
								clientHttpProxyAddressSet.Add(clientIpAddressTrimed);
								clientHttpProxyAddressList.Add(clientIpAddressTrimed);
								// !!!
							}
						}
					}
				}
			}
		}
		return clientHttpProxyAddressList;
	}

	public static string? GetClientConnectionAddressesString(
		this HttpRequest request)
	{
		var clientHttpProxyAddressList
			= request.GetClientConnectionAddressList();
		if (clientHttpProxyAddressList.Count > 0)
		{
			return StringUtil.StringWithStrings(
				clientHttpProxyAddressList,
				",",
				true);
		}
		return null;
	}

	/// <summary>
	/// 获取客户端连接的第一个地址（使用Http代理时，会有多个连接地址）。
	/// </summary>
	/// <param name="request">当前请求对象。</param>
	/// <returns>当前请求对象的第一个连接地址。</returns>
	public static string? GetClientConnectionAddressFirst(this HttpRequest request)
	{
		var clientConnectionAddressList = request.GetClientConnectionAddressList();
		if (clientConnectionAddressList.Count > 0)
		{
			return clientConnectionAddressList[0];
		}
		return null;
	}

	/// <summary>
	/// 获取客户端连接的最后一个地址（使用Http代理时，会有多个连接地址）。
	/// </summary>
	/// <param name="request">当前请求对象。</param>
	/// <returns>当前请求对象的最后一个连接地址。</returns>
	public static string? GetClientConnectionAddressLast(this HttpRequest request)
	{
		var clientConnectionAddressList = request.GetClientConnectionAddressList();
		if (clientConnectionAddressList.Count > 0)
		{
			return clientConnectionAddressList[^1];
		}
		return null;
	}

	public static bool TryGetClientConnectionPortFromIpAddress(
		string? ipAddress,
		out int clientConnectionPort)
	{
		clientConnectionPort = 0;
		if (string.IsNullOrEmpty(ipAddress))
		{
			return false;
		}
		var ipAddresses = ipAddress?.Split(",", System.StringSplitOptions.RemoveEmptyEntries);
		if (ipAddresses == null)
		{
			return false;
		}
		var lastIpAddress = ipAddresses[^1];
		var lastIpPortString = lastIpAddress.SubstringBetween(":", null);
		if (int.TryParse(lastIpPortString, out clientConnectionPort))
		{
			return true;
		}
		return false;
	}

	/// <summary>
	/// 获取客户端连接的端口号。
	/// </summary>
	/// <param name="request">当前请求对象。</param>
	/// <returns>当前请求对象的连接端口号。</returns>
	public static int GetClientConnectionPortLast(this HttpRequest request)
	{
		var clientConnectionPortLast = request.HttpContext.Connection.RemotePort;
		// 宝匣网关的客户端地址。
		if (request.Headers?
			.TryGetValue(
				HttpHeaderKeys.BxService_Gateway_ClientIp,
				out var bxService_Gateway_ClientIp) == true
			&& bxService_Gateway_ClientIp.Count > 0)
		{
			var clientIp = bxService_Gateway_ClientIp[^1];
			if (TryGetClientConnectionPortFromIpAddress(clientIp, out var clientIpPort))
			{
				// !!!
				clientConnectionPortLast = clientIpPort;
				// !!!
			}
		}
		return clientConnectionPortLast;
	}

	/// <summary>
	/// 获取客户端的Ip信息。
	/// </summary>
	/// <param name="httpRequest">当前Http请求对象。</param>
	/// <returns>当前Http请求对象的客户端Ip信息。</returns>
	public static ClientIpInfo GetClientIpInfo(
		this HttpRequest? httpRequest)
	{
		if (httpRequest == null)
		{
			return new ClientIpInfo();
		}

		var ipPortLast
			= httpRequest.HttpContext.Connection.RemotePort;
		var ipAddressChainList
			= httpRequest.GetClientConnectionAddressList();
		if (ipPortLast == 0)
		{
			for (var ipAddressIndex = ipAddressChainList.Count - 1;
				ipAddressIndex >= 0;
				ipAddressIndex--)
			{
				var ipAddress = ipAddressChainList[ipAddressIndex];
				if (ipAddress?.LastIndexOf(':') is { } lastIndexOfColonSymbol
					&& lastIndexOfColonSymbol >= 0)
				{
					var ipPortLastString = ipAddress[(lastIndexOfColonSymbol + 1)..];
					if (int.TryParse(ipPortLastString, out var ipPortLastIntValue))
					{
						// !!!
						ipPortLast = ipPortLastIntValue;
						break;
						// !!!
					}
				}
			}
		}

		var endPointInfo = new ClientIpInfo()
		{
			IpAddressChain = StringUtil.StringWithStrings(ipAddressChainList, ",", true),
			IpPortLast = ipPortLast

		};
		return endPointInfo;
	}

	public static async Task<string?> ReadStringAsync(
		this Microsoft.AspNetCore.Http.HttpRequest httpRequest,
		System.Text.Encoding? textEncoding = null)
	{
		return await httpRequest.BodyReader.ReadStringAsync(textEncoding);
	}

	#endregion
}
