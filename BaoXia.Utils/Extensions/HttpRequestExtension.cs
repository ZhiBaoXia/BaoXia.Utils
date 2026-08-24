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
		public const string BxService_Gateway_ClientIp = "BaoXia-Gateway-ClientIp";
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

	public static ClientConnectionAddressInfes GetClientConnectionAddressInfes(this HttpRequest request)
	{
		var clientAddressInfoList = new List<string>();
		string? bxGatewayClientAddressInfo = null;

		////////////////////////////////////////////////
		// 1/，获取【Http请求头】中的客户端地址信息。
		////////////////////////////////////////////////
		if (request.Headers is IHeaderDictionary requestHeaders)
		{
			// “X-Forwarded-For”的客户端地址。
			if (requestHeaders.TryGetValue("X-Forwarded-For", out var x_Forwarded_For) == true && x_Forwarded_For.Count > 0)
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
								// !!!
								clientAddressInfoList.Add(forwardClientAddressTrimed);
								// !!!
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
						// !!!
						clientAddressInfoList.Add(x_Real_IpTrimed);
						// !!!
					}
				}
			}
		}


		////////////////////////////////////////////////
		// 3/，获取【宝匣网关】中的客户端地址信息。
		////////////////////////////////////////////////
		if (request.Headers?.TryGetValue(HttpHeaderKeys.BxService_Gateway_ClientIp, out var bxServiceGatewayClientIp) == true
		    && bxServiceGatewayClientIp.Count > 0)
		{
			foreach (var clientIp in bxServiceGatewayClientIp)
			{
				var bxGatewayClientIpAddresses = clientIp?.Split(",", System.StringSplitOptions.RemoveEmptyEntries);
				if (bxGatewayClientIpAddresses != null && bxGatewayClientIpAddresses.Length > 0)
				{
					foreach (var clientIpAddress in bxGatewayClientIpAddresses)
					{
						var clientIpAddressTrimed = clientIpAddress.Trim();
						if (clientIpAddressTrimed?.Length > 0)
						{
							// !!!
							bxGatewayClientAddressInfo = clientIpAddressTrimed;
							clientAddressInfoList.Add(clientIpAddressTrimed);
							// !!!
						}
					}
				}
			}
		}

		////////////////////////////////////////////////
		// 2/，获取【Tcp连接】中的客户端地址信息。
		////////////////////////////////////////////////
		string? firstClientAddressIp = null;
		int firstClientAddressPort = 0;
		string? lastClientAddressIp = null;
		int lastClientAddressPort = 0;
		if (request.HttpContext?.Connection is { } tcpIpConnection)
		{
			var remoteIpAddress = tcpIpConnection.RemoteIpAddress?.ToString();
			if (remoteIpAddress?.Length > 0)
			{
				var remotePort = tcpIpConnection.RemotePort;
				if (clientAddressInfoList.Count < 1)
				{
					firstClientAddressIp = remoteIpAddress;
					firstClientAddressPort = remotePort;
				}
				lastClientAddressIp = remoteIpAddress;
				lastClientAddressPort = remotePort;
				//
				remoteIpAddress += ":" + remotePort;
				// !!!
				clientAddressInfoList.Add(remoteIpAddress);
				// !!!
			}
		}

		if (clientAddressInfoList.Count > 0)
		{
			if (firstClientAddressIp == null)
			{
				var firstClientAddressInfo = clientAddressInfoList[0];
				//
				firstClientAddressInfo.GetIpAddress(out firstClientAddressIp, out firstClientAddressPort);
				//
			}
			if (lastClientAddressIp == null)
			{
				var lastClientAddressInfo = clientAddressInfoList[^1];
				//
				lastClientAddressInfo.GetIpAddress(out lastClientAddressIp, out lastClientAddressPort);
				//
			}
		}
		string? bxGatewayClientAddressIp = null;
		int bxGatewayClientAddressPort = 0;
		if (bxGatewayClientAddressInfo?.Length > 0)
		{
			bxGatewayClientAddressInfo.GetIpAddress(out bxGatewayClientAddressIp, out bxGatewayClientAddressPort);
		}

		return new()
		{
			ClientAddressInfoList = clientAddressInfoList,

			FirstClientAddressIp = firstClientAddressIp,
			FirstClientAddressPort = firstClientAddressPort,

			LastClientAddressIp = lastClientAddressIp,
			LastClientAddressPort = lastClientAddressPort,

			BxGatewayClientAddressIp = bxGatewayClientAddressIp,
			BxGatewayClientAddressPort = bxGatewayClientAddressPort
		};
	}

	public static string? GetClientConnectionAddressInfesString(this HttpRequest request)
	{
		var clientAddressInfoList = request.GetClientConnectionAddressInfes()?.ClientAddressInfoList;
		if (clientAddressInfoList?.Count > 0)
		{
			return clientAddressInfoList.ToClientConnectionAddressInfesString();
		}
		return null;
	}

	/// <summary>
	/// 获取客户端连接的第一个地址（使用Http代理时，会有多个连接地址）。
	/// </summary>
	/// <param name="request">当前请求对象。</param>
	/// <returns>当前请求对象的第一个连接地址。</returns>
	public static string? GetFirstClientConnectionAddressInfoString(this HttpRequest request)
	{
		var clientAddressInfoList = request.GetClientConnectionAddressInfes()?.ClientAddressInfoList;
		if (clientAddressInfoList?.Count > 0)
		{
			return clientAddressInfoList[0];
		}
		return null;
	}

	/// <summary>
	/// 获取客户端连接的最后一个地址（使用Http代理时，会有多个连接地址）。
	/// </summary>
	/// <param name="request">当前请求对象。</param>
	/// <returns>当前请求对象的最后一个连接地址。</returns>
	public static string? GetLastClientConnectionAddressInfoString(this HttpRequest request)
	{
		var clientAddressInfoList = request.GetClientConnectionAddressInfes()?.ClientAddressInfoList;
		if (clientAddressInfoList?.Count > 0)
		{
			return clientAddressInfoList[^1];
		}
		return null;
	}

	/// <summary>
	/// 获取客户端的Ip信息。
	/// </summary>
	/// <param name="httpRequest">当前Http请求对象。</param>
	/// <returns>当前Http请求对象的客户端Ip信息。</returns>
	public static ClientIpInfo GetClientIpInfo(this HttpRequest? httpRequest)
	{
		if (httpRequest == null)
		{
			return new ClientIpInfo();
		}

		var clientConnectionAddressInfes = httpRequest.GetClientConnectionAddressInfes();


		var endPointInfo = new ClientIpInfo()
		{
			IpAddressChain = clientConnectionAddressInfes.ClientAddressInfoList.ToClientConnectionAddressInfesString(),
			IpPortLast = clientConnectionAddressInfes.LastClientAddressPort

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
