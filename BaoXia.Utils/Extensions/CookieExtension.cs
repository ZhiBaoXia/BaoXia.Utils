using System;
using System.Collections.Generic;
using System.Net;

namespace BaoXia.Utils.Extensions
{
	/// <summary>
	/// Cookie信息，用于保存解析后的Cookie信息。
	/// </summary>
	public class CookieInfo
	{
		/// <summary>
		/// Cookie所属的域名。
		/// </summary>
		public string? Domain { get; set; }

		/// <summary>
		/// Cookie所属的路径。
		/// </summary>
		public string? Path { get; set; }

		/// <summary>
		/// Cookie的寿命字段，秒数，优先级最高。
		/// </summary>
		public string? Max_Age { get; set; }

		/// <summary>
		/// Cookie的寿命字段，时间字符串，优先级较低。
		/// </summary>
		public string? Expires { get; set; }

		/// <summary>
		/// 是否为安全的Cookie，标记为 Secure 的Cookie只应通过被HttpS协议加密过的请求发送给服务端
		/// </summary>
		public bool IsSecure { get; set; }

		/// <summary>
		/// 是否为“只有服务端”可以使用的Cookie， HttpOnly 标记的Cookie，它们只应该发送给服务端。
		/// </summary>
		public bool IsHttpOnly { get; set; }

		/// <summary>
		/// Cookie中保存的参数数组，字符串类型的键值对。
		/// </summary>
		public Dictionary<string, string?>? Params { get; set; }


		/// <summary>
		/// 由当前Cookie信息生成Cookie对象。
		/// </summary>
		/// <returns>返回由当前Cookie信息创建的Cookie对象。</returns>
		public Cookie ToCookie()
		{
			var cookie = new Cookie();
			{
				cookie.Domain = this.Domain != null ? this.Domain : ".";
				cookie.Path = this.Path != null ? this.Path : "/";
				cookie.Expires = this.Expires != null ? DateTime.Parse(this.Expires) : DateTime.MaxValue;
				cookie.Secure = this.IsSecure;
				cookie.HttpOnly = this.IsHttpOnly;

				if (this.Params?.Count > 0)
				{
					foreach (var cookieParam in this.Params)
					{
						if (cookieParam.Key.Length > 0)
						{
							cookie.Name = cookieParam.Key;
							if (cookieParam.Value?.Length > 0)
							{
								cookie.Value = cookieParam.Value;
							}
							//
							break;
						}
					}
				}
			}
			return cookie;
		}

		/// <summary>
		/// 由当前Cookie信息生成Cookie容器对象。
		/// </summary>
		/// <returns>返回由当前Cookie信息创建的Cookie容器对象。</returns>
		public CookieContainer ToCookieContainer()
		{
			var cookieContainer = new CookieContainer();
			{
				var cookie = this.ToCookie();
				if (cookie != null)
				{
					cookieContainer.Add(cookie);
				}
			}
			return cookieContainer;
		}
	}


	/// <summary>
	/// Cookie操作扩展。
	/// </summary>
	public static class CookieExtension
	{
		/// <summary>
		/// 通过解析Cookie字符串，创建“CookieInfo”对象。
		/// </summary>
		/// <param name="cookieString"></param>
		/// <returns>解析成功时，返回解析得出的“CookieInfo”对象，解析失败时，返回“null”。</returns>
		public static CookieInfo? CreateCookieInfoWithCookieString(string cookieString)
		{
			CookieInfo? cookieInfo = null;
			if (cookieString.Length > 0)
			{
				// !!!
				cookieInfo = new CookieInfo();
				// !!!
				var cookieSections = cookieString.Split(';');
				if (cookieSections?.Length > 0)
				{
					foreach (var cookieSection in cookieSections)
					{
						var sectionName = "";
						var sectionValue = "";
						var indexOfEqualMark = cookieSection.IndexOf('=');
						if (indexOfEqualMark >= 0)
						{
							sectionName = cookieSection.Substring(0, indexOfEqualMark).Trim();
							sectionValue = cookieSection[(indexOfEqualMark + 1)..].Trim();
						}
						else
						{
							sectionName = cookieSection.Trim();
						}

						if ("Domain".Equals(sectionName, StringComparison.OrdinalIgnoreCase))
						{
							cookieInfo.Domain = sectionValue;
						}
						else if ("Path".Equals(sectionName, StringComparison.OrdinalIgnoreCase))
						{
							cookieInfo.Path = sectionValue;
						}
						else if ("Max-Age".Equals(sectionName, StringComparison.OrdinalIgnoreCase))
						{
							cookieInfo.Max_Age = sectionValue;
						}
						else if ("Expires".Equals(sectionName, StringComparison.OrdinalIgnoreCase))
						{
							cookieInfo.Expires = sectionValue;
						}
						else if ("Secure".Equals(sectionName, StringComparison.OrdinalIgnoreCase))
						{
							cookieInfo.IsSecure = true;
						}
						else if ("HttpOnly".Equals(sectionName, StringComparison.OrdinalIgnoreCase))
						{
							cookieInfo.IsHttpOnly = true;
						}
						else
						{
							if (cookieInfo.Params == null)
							{
								cookieInfo.Params = [];
							}
							//
							cookieInfo.Params.Add(sectionName, sectionValue);
							//
						}
					}
				}
			}
			return cookieInfo;
		}
	}
}
