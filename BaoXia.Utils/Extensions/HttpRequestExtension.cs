using BaoXia.Utils.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
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
		public const string BxService_Gateway_ConnectionIPEndPoints = "BaoXia-Gateway-ConnectionIPEndPoints";
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

	public static ConnectionIPEndPoints GetConnectionIPEndPoints(this HttpRequest request)
	{
		var connectionIPEndPoints = new List<IPEndPoint>();
		var isConnectionIPEndPointsGetFromBxGatewayHttpHeader = false;

		////////////////////////////////////////////
		// 1/，如果存在【宝匣网关】中的客户端地址信息，则只使用【宝匣网关】中的客户端地址信息。
		////////////////////////////////////////////////
		if (request.Headers?.TryGetValue(HttpHeaderKeys.BxService_Gateway_ConnectionIPEndPoints,
			out var bxGatewayConnectionIPEndPointValues) == true
			&& bxGatewayConnectionIPEndPointValues.Count > 0)
		{
			foreach (var bxGatewayConnectionIPEndPointValue in bxGatewayConnectionIPEndPointValues)
			{
				var bxGatewayConnectionIPEndPointStrings = bxGatewayConnectionIPEndPointValue?.Split(
					',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
				if (bxGatewayConnectionIPEndPointStrings != null && bxGatewayConnectionIPEndPointStrings.Length > 0)
				{
					foreach (var bxGatewayConnectionIPEndPointString in bxGatewayConnectionIPEndPointStrings)
					{
						if (bxGatewayConnectionIPEndPointString?.Length > 0
							&& IPEndPoint.TryParse(bxGatewayConnectionIPEndPointString, out var connectionIPEndPoint))
						{
							// !!!
							connectionIPEndPoints.Add(connectionIPEndPoint);
							// !!!
						}
					}
				}
			}
			//
			isConnectionIPEndPointsGetFromBxGatewayHttpHeader = true;
			//
		}
		else
		{
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
						var forwardClientAddresses = forward?.Split(
							',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
						if (forwardClientAddresses != null)
						{
							foreach (var forwardClientAddress in forwardClientAddresses)
							{
								if (IPEndPoint.TryParse(forwardClientAddress, out var connectionIPEndPoint))
								{
									// !!!
									connectionIPEndPoints.Add(connectionIPEndPoint);
									// !!!
								}
							}
						}
					}
				}
				// “X-Real-IP”的客户端地址。
				if (requestHeaders.TryGetValue("X-Real-IP", out var xRealIps) == true)
				{
					foreach (var xRealIp in xRealIps)
					{
						if (xRealIp?.Length > 0 && IPEndPoint.TryParse(xRealIp, out var connectionIPEndPoint))
						{
							// !!!
							connectionIPEndPoints.Add(connectionIPEndPoint);
							// !!!
						}
					}
				}
			}
		}

		////////////////////////////////////////////////
		// 2/，获取【Tcp连接】中的客户端地址信息。
		////////////////////////////////////////////////
		IPEndPoint? bxGatewayPrevIPEndPoint = null;
		if (request.HttpContext?.Connection is { } httpConnection
			&& httpConnection.RemoteIpAddress is { } remoteIpAddress)
		{
			if (isConnectionIPEndPointsGetFromBxGatewayHttpHeader && connectionIPEndPoints.Count > 0)
			{
				bxGatewayPrevIPEndPoint = connectionIPEndPoints[^1];
			}

			var connectionEndPoint = new IPEndPoint(remoteIpAddress, httpConnection.RemotePort);
			// !!!
			connectionIPEndPoints.Add(connectionEndPoint);
			// !!!
		}

		return new()
		{
			IPEndPoints = connectionIPEndPoints,

			BxGatewayPrevIPEndPoint = bxGatewayPrevIPEndPoint
		};
	}

	public static string? GetClientConnectionAddressInfesString(this HttpRequest request)
	{
		var clientConnectionAddressInfesString = request.GetConnectionIPEndPoints().IPEndPoints.ToClientConnectionIPEndPointsString();
		{ }
		return clientConnectionAddressInfesString;
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

		var clientConnectionAddressInfes = httpRequest.GetConnectionIPEndPoints();


		var endPointInfo = new ClientIpInfo()
		{
			IpAddressChain = clientConnectionAddressInfes.IPEndPoints.ToClientConnectionIPEndPointsString(),
			IpPortLast = clientConnectionAddressInfes.LastIPEndPoint?.Port ?? 0

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
