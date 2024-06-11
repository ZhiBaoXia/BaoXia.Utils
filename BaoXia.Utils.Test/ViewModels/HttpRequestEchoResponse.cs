using BaoXia.Constants.Models;
using System.Collections.Generic;

namespace BaoXia.Utils.Test.ViewModels
{
	public class HttpRequestEchoResponse : Response
	{

		////////////////////////////////////////////////
		// @静态常量
		////////////////////////////////////////////////

		#region 静态常量

		public class HttpRequestInfo
		{
			public string HttpMethod { get; set; }

			public Dictionary<string, string> HttpHeaders { get; set; }

			public string? HttpBodyString { get; set; }

			public long HttpBodyBytesCount { get; set; }


			public HttpRequestInfo(
				string httpMethod,
				Dictionary<string, string> httpHeaders,
				string? httpBodyString,
				long httpBodyBytesCount)
			{
				HttpMethod = httpMethod;
				HttpHeaders = httpHeaders;
				HttpBodyString = httpBodyString;
				HttpBodyBytesCount = httpBodyBytesCount;
			}
		}

		#endregion



		////////////////////////////////////////////////
		// @自身属性
		////////////////////////////////////////////////

		#region 自身属性

		public HttpRequestInfo? RequestInfo { get; set; }

		#endregion


	}
}
