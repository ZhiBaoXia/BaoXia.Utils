using System;
using System.Security.Cryptography;

namespace BaoXia.Utils;

public class BytesUtil
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static byte[] CreateRandomBytesInLength(int bytesLength)
	{
		if (bytesLength <= 0)
		{
			return [];
		}

		// 创建指定长度的字节数组并填充随机数据
		byte[] byteArray = new byte[bytesLength];
		using var andomNumberGenerator = RandomNumberGenerator.Create();
		{
			andomNumberGenerator.GetBytes(byteArray);
		}
		return byteArray;
	}

	public static string? CreateBase64StringOfBytes(
		 byte[] bytes,
		int offset = 0,
		int? length = null,
		string? dataType = null,
		Base64FormattingOptions base64FormattingOptions = Base64FormattingOptions.None)
	{
		if (bytes.Length < 1)
		{
			return null;
		}

		length ??= bytes.Length;
		if (length < 1)
		{
			return null;
		}

		if (offset < 0)
		{
			length += offset;
			offset = 0;
		}
		if (offset + length > bytes.Length)
		{
			length = bytes.Length - offset;
		}

		var base64String = System.Convert.ToBase64String(
			bytes,
			offset,
			length.Value,
			base64FormattingOptions);
		if (dataType?.Length > 0)
		{
			base64String = "data:" + dataType + ";base64,";
		}
		return base64String;
	}

	public static byte[] CreateBytesFromBase64String(
		string base64String,
		bool isAutoIgnoreContentTypeChars = true)
	{
		if (base64String.Length < 1)
		{
			return [];
		}

		if (isAutoIgnoreContentTypeChars)
		{
			var commaIndexInImageBase64Code = base64String.IndexOf(',');
			if (commaIndexInImageBase64Code >= 0)
			{
				base64String = base64String[(commaIndexInImageBase64Code + 1)..];
			}
		}
		byte[] bytes = Convert.FromBase64String(base64String);
		{ }
		return bytes;
	}

	#endregion
}