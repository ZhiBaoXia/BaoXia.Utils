using Microsoft.AspNetCore.Http;
using System.Text;

namespace BaoXia.Utils.Extensions;

public static class IHeaderDictionaryExtension
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static string ToHttpHeadersString(this IHeaderDictionary headers)
	{
		var headersStringBuilder = new StringBuilder();

		foreach (var header in headers)
		{
			var headerKey = header.Key;
			var headerValue = header.Value.ToString();
			headersStringBuilder.Append($"{headerKey}: {headerValue}\r\n");
		}
		var headersString = headersStringBuilder.ToString();
		{ }
		return headersString;
	}

	#endregion
}