using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text;

namespace BaoXia.Utils.Extensions
{
	public static class HttpRequestExtension
	{
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

		/// <summary>
		/// 根据“Http_X_FORWARDED_FOR”字段，获取Http代理链，
		/// 最终代理地址一定为“请求客户端地址”，
		/// 用户没有代理时，代理链等于“请求客户端地址”。
		/// </summary>
		/// <param name="request">当前请求对象。</param>
		/// <returns>代理链字符串。</returns>
		public static List<string> GetClientConnectionAddressList(this HttpRequest request)
		{
			var clientHttpProxyAddressList = new List<string>();
			if (request.Headers?.TryGetValue("X-Forwarded-For", out var x_Forwarded_For) == true
				&& x_Forwarded_For.Count > 0)
			{
				foreach (var forward in x_Forwarded_For)
				{
					var forwardClientAddresses = forward.Split(",", System.StringSplitOptions.RemoveEmptyEntries);
					foreach (var forwardClientAddress in forwardClientAddresses)
					{
						var forwardClientAddressTrimed = forwardClientAddress.Trim();
						if (forwardClientAddressTrimed?.Length > 0)
						{
							// !!!
							clientHttpProxyAddressList.Add(forwardClientAddressTrimed);
							// !!!
						}
					}
				}
			}

			if (request.Headers?.TryGetValue("X-Real-IP", out var x_Real_Ips) == true)
			{
				foreach (var x_Real_Ip in x_Real_Ips)
				{
					var x_Real_IpTrimed = x_Real_Ip.Trim();
					if (x_Real_IpTrimed?.Length > 0)
					{
						// !!!
						clientHttpProxyAddressList.Add(x_Real_IpTrimed);
						// !!!
					}
				}
			}

			if (request?.HttpContext?.Connection != null)
			{
				var remoteIpAddress = request.HttpContext.Connection.RemoteIpAddress?.ToString();
				if (remoteIpAddress?.Length > 0)
				{
					if (clientHttpProxyAddressList.Count < 1
						|| clientHttpProxyAddressList[^1].EqualsIgnoreCase(remoteIpAddress) != true)
					{
						// !!!
						clientHttpProxyAddressList.Add(remoteIpAddress);
						// !!!
					}
				}
			}
			return clientHttpProxyAddressList;
		}

		/// <summary>
		/// 根据“Http_X_FORWARDED_FOR”字段，获取Http代理链，
		/// 最终代理地址一定为“请求客户端地址”，
		/// 用户没有代理时，代理链等于“请求客户端地址”。
		/// </summary>
		/// <param name="request">当前请求对象。</param>
		/// <returns>代理链字符串。</returns>
		public static string? GetClientConnectionAddressesString(this HttpRequest request)
		{
			var clientHttpProxyAddressList = request.GetClientConnectionAddressList();
			if (clientHttpProxyAddressList.Count > 0)
			{
				return StringExtension.StringWithStrings(clientHttpProxyAddressList);
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

		/// <summary>
		/// 获取客户端连接的端口号。
		/// </summary>
		/// <param name="request">当前请求对象。</param>
		/// <returns>当前请求对象的连接端口号。</returns>
		public static int GetClientConnectionPortLast(this HttpRequest request)
		{
			if (request.HttpContext?.Connection != null)
			{
				return request.HttpContext.Connection.RemotePort;
			}
			return 0;
		}
	}
}
