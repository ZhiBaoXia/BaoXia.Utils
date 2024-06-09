using System.Net;

namespace BaoXia.Utils.Extensions
{
	/// <summary>
	/// 服务端响应的字节信息。
	/// </summary>

	public class HttpResponseBytesInfo
	{
		/// <summary>
		/// 响应对象。
		/// </summary>
		public HttpWebResponse? Response { get; set; }

		/// <summary>
		/// 服务端响应的内容类型。
		/// </summary>
		public string? ContentType { get; set; }

		/// <summary>
		/// 服务端响应的字符集。
		/// </summary>
		public string? CharacterSet { get; set; }

		/// <summary>
		/// 服务端响应的字节数组。
		/// </summary>
		public byte[]? Bytes { get; set; }

		public override string ToString()
		{
			var responseString = "";
			if (this.Bytes?.Length > 0)
			{
				var responseCharacterSet = this.CharacterSet;
				if ("ascii".EqualsIgnoreCase(responseCharacterSet))
				{
					responseString = System.Text.Encoding.ASCII.GetString(this.Bytes);
				}
				else if ("unicode".EqualsIgnoreCase(responseCharacterSet))
				{
					responseString = System.Text.Encoding.Unicode.GetString(this.Bytes);
				}
				else // if ("utf-8".EqualsIgnoreCase(responseCharacterSet))
				{
					responseString = System.Text.Encoding.UTF8.GetString(this.Bytes);
				}
			}
			return responseString;
		}

		public T? ToObject<T>()
		{
			T? tObject = default;
			var responseStringInJson = this.ToString();
			if (responseStringInJson?.Length > 0)
			{
				tObject = responseStringInJson.ToObjectByJsonDeserialize<T>();
			}
			return tObject;
		}
	}
}
