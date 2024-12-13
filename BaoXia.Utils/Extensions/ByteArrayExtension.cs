using System;
using System.IO;
using System.IO.Compression;

namespace BaoXia.Utils.Extensions;

public static class ByteArrayExtension
{

	////////////////////////////////////////////////
	// @静态常量
	////////////////////////////////////////////////

	#region 静态常量


	/// <summary>
	/// 默认读取缓冲区相比原始压缩数据的倍率，默认位：4.0。
	/// </summary>
	public const float ReadBufferSizeRatioDefault = 4.0F;


	#endregion


	////////////////////////////////////////////////
	// @”Brotli“压缩算法实现
	////////////////////////////////////////////////

	#region ”Brotli“压缩算法实现

	public static byte[]? BytesByCompressWithBrotli(
		this byte[] bytes,
		int offset,
		int length,
		CompressionLevel compressionLevel = CompressionLevel.Optimal)
	{
		if (bytes == null
				   || bytes.Length < 1
				   || offset < 0
				   || offset >= bytes.Length
				   || length <= 0)
		{
			return null;
		}
		if (offset + length > bytes.Length)
		{
			length = bytes.Length - offset;
		}

		byte[]? bytesCompressed = null;
		if (bytes?.Length > 0)
		{
			using var bytesCompressedMemoryStream = new MemoryStream();
			using var brStream = new BrotliStream(
				bytesCompressedMemoryStream,
				compressionLevel);
			{
				brStream.Write(bytes, offset, length);
				brStream.Flush();
			}
			bytesCompressed = bytesCompressedMemoryStream.ToArray();
		}
		return bytesCompressed;
	}

	public static byte[]? BytesByCompressWithBrotli(
		this byte[] bytes,
		CompressionLevel compressionLevel = CompressionLevel.Optimal)
	{
		return bytes.BytesByCompressWithBrotli(
			0,
			bytes.Length,
			compressionLevel);
	}

	public static ArraySegment<byte>? BytesByDecompressWithBrotli(
		this byte[] bytesCompressed,
		int bytesCompressedBeginIndex,
		int bytesCompressedCount,
		int readBufferSize = 0,
		float readBufferSizeRatio = ByteArrayExtension.ReadBufferSizeRatioDefault)
	{
		ArraySegment<byte>? bytesDecompressed = null;
		if (bytesCompressed.Length > 0)
		{
			using var bytesCompressedStream
				= new MemoryStream(
					bytesCompressed,
					bytesCompressedBeginIndex,
					bytesCompressedCount);
			using var brStream = new BrotliStream(
				bytesCompressedStream,
				CompressionMode.Decompress);
			if (readBufferSize <= 0)
			{
				readBufferSize
					= (int)(bytesCompressed.Length
					* readBufferSizeRatio);
			}
			if (readBufferSize <= 0)
			{
				readBufferSize
					= (int)(bytesCompressed.Length
					* ByteArrayExtension.ReadBufferSizeRatioDefault);
			}
			var bytesDecompressedBuffer = new BytesBuffer(readBufferSize);
			{
				var bytesCountRead = 0;
				do
				{
					var readBytesBuffer = bytesDecompressedBuffer.GetEmptyBufferSegment();
					bytesCountRead = brStream.Read(readBytesBuffer);
					// !!!
					bytesDecompressedBuffer.BytesCount += bytesCountRead;
					// !!!
				} while (bytesCountRead > 0);
			}
			bytesDecompressed = bytesDecompressedBuffer.Bytes;
		}
		return bytesDecompressed;
	}

	public static ArraySegment<byte>? BytesByDecompressWithBrotli(
			this byte[] bytesCompressed,
			int readBufferSize = 0,
			float readBufferSizeRatio = ByteArrayExtension.ReadBufferSizeRatioDefault)
	{
		return BytesByDecompressWithBrotli(
			bytesCompressed,
			0,
			bytesCompressed.Length,
			readBufferSize,
			readBufferSizeRatio);
	}

	#endregion


	////////////////////////////////////////////////
	// @”GZip“压缩算法实现
	////////////////////////////////////////////////

	#region ”GZip“压缩算法实现
	public static byte[]? BytesByCompressWithGZip(this byte[] bytes, int offset, int length)
	{
		if (bytes == null
				   || bytes.Length < 1
				   || offset < 0
				   || offset >= bytes.Length
				   || length <= 0)
		{
			return null;
		}
		if (offset + length > bytes.Length)
		{
			length = bytes.Length - offset;
		}

		byte[]? bytesCompressed = null;
		if (bytes.Length > 0)
		{
			using var bytesCompressedMemoryStream = new MemoryStream();
			using (var gzipStream = new GZipStream(
				bytesCompressedMemoryStream,
				CompressionMode.Compress))
			{
				gzipStream.Write(bytes, offset, length);
				gzipStream.Flush();
			}
			bytesCompressed = bytesCompressedMemoryStream.ToArray();
		}
		return bytesCompressed;
	}

	public static byte[]? BytesByCompressWithGZip(this byte[] bytes)
	{
		return bytes.BytesByCompressWithGZip(0, bytes.Length);
	}

	public static ArraySegment<byte>? BytesByDecompressWithGZip(
		this byte[] bytesCompressed,
		int bytesCompressedBeginIndex,
		int bytesCompressedCount,
		int readBufferSize = 0,
		float readBufferSizeRatio = ByteArrayExtension.ReadBufferSizeRatioDefault)
	{
		ArraySegment<byte>? bytesDecompressed = null;
		if (bytesCompressed.Length > 0)
		{
			using var bytesCompressedStream
				= new MemoryStream(
					bytesCompressed,
					bytesCompressedBeginIndex,
					bytesCompressedCount);
			using var brStream = new GZipStream(
				bytesCompressedStream,
				CompressionMode.Decompress);
			if (readBufferSize <= 0)
			{
				readBufferSize
					= (int)(bytesCompressed.Length
					* readBufferSizeRatio);
			}
			if (readBufferSize <= 0)
			{
				readBufferSize
					= (int)(bytesCompressed.Length
					* ByteArrayExtension.ReadBufferSizeRatioDefault);
			}
			var bytesDecompressedBuffer = new BytesBuffer(readBufferSize);
			{
				var bytesCountRead = 0;
				do
				{
					var readBytesBuffer = bytesDecompressedBuffer.GetEmptyBufferSegment();
					bytesCountRead = brStream.Read(readBytesBuffer);
					// !!!
					bytesDecompressedBuffer.BytesCount += bytesCountRead;
					// !!!
				} while (bytesCountRead > 0);
			}
			bytesDecompressed = bytesDecompressedBuffer.Bytes;
		}
		return bytesDecompressed;
	}

	public static ArraySegment<byte>? BytesByDecompressWithGZip(
		this byte[] bytesCompressed,
		int readBufferSize = 0,
		float readBufferSizeRatio = ByteArrayExtension.ReadBufferSizeRatioDefault)
	{
		return BytesByDecompressWithGZip(
			bytesCompressed,
			0,
			bytesCompressed.Length,
			readBufferSize,
			readBufferSizeRatio);
	}

	#endregion


	////////////////////////////////////////////////
	// @”Deflate“压缩算法实现
	////////////////////////////////////////////////

	#region ”Deflate“压缩算法实现
	public static byte[]? BytesByCompressWithDeflate(this byte[] bytes, int offset, int length)
	{
		if (bytes == null
				   || bytes.Length < 1
				   || offset < 0
				   || offset >= bytes.Length
				   || length <= 0)
		{
			return null;
		}
		if (offset + length > bytes.Length)
		{
			length = bytes.Length - offset;
		}

		byte[]? bytesCompressed = null;
		if (bytes?.Length > 0)
		{
			using var bytesCompressedMemoryStream = new MemoryStream();
			using var deflateStream = new DeflateStream(
				bytesCompressedMemoryStream,
				CompressionMode.Compress);
			{
				deflateStream.Write(bytes, offset, length);
				deflateStream.Flush();
			}
			bytesCompressed = bytesCompressedMemoryStream.ToArray();
		}
		return bytesCompressed;
	}

	public static byte[]? BytesByCompressWithDeflate(this byte[] bytes)
	{
		return bytes.BytesByCompressWithDeflate(0, bytes.Length);
	}

	public static ArraySegment<byte>? BytesByDecompressWithDeflate(
		this byte[] bytesCompressed,
		int bytesCompressedBeginIndex,
		int bytesCompressedCount,
		int readBufferSize = 0,
		float readBufferSizeRatio = ByteArrayExtension.ReadBufferSizeRatioDefault)
	{
		ArraySegment<byte>? bytesDecompressed = null;
		if (bytesCompressed.Length > 0)
		{
			using var bytesCompressedStream
				= new MemoryStream(
					bytesCompressed,
					bytesCompressedBeginIndex,
					bytesCompressedCount);
			using var brStream = new DeflateStream(
				bytesCompressedStream,
				CompressionMode.Decompress);
			if (readBufferSize <= 0)
			{
				readBufferSize
					= (int)(bytesCompressed.Length
					* readBufferSizeRatio);
			}
			if (readBufferSize <= 0)
			{
				readBufferSize
					= (int)(bytesCompressed.Length
					* ByteArrayExtension.ReadBufferSizeRatioDefault);
			}
			var bytesDecompressedBuffer = new BytesBuffer(readBufferSize);
			{
				var bytesCountRead = 0;
				do
				{
					var readBytesBuffer = bytesDecompressedBuffer.GetEmptyBufferSegment();
					bytesCountRead = brStream.Read(readBytesBuffer);
					// !!!
					bytesDecompressedBuffer.BytesCount += bytesCountRead;
					// !!!
				} while (bytesCountRead > 0);
			}
			bytesDecompressed = bytesDecompressedBuffer.Bytes;
		}
		return bytesDecompressed;
	}

	public static ArraySegment<byte>? BytesByDecompressWithDeflate(
		this byte[] bytesCompressed,
		int readBufferSize = 0,
		float readBufferSizeRatio = ByteArrayExtension.ReadBufferSizeRatioDefault)
	{
		return BytesByDecompressWithDeflate(
			bytesCompressed,
			0,
			bytesCompressed.Length,
			readBufferSize,
			readBufferSizeRatio);
	}

	#endregion


	/// <summary>
	/// 讲Base64码格式的字符串转为对应的字节数组。
	/// </summary>
	/// <param name="base64String">Base64码格式的字符串。</param>
	/// <param name="isAutoIgnoreContentTypeChars">是否自动处理内容类型字符，如：“data:image/jpeg;base64,”。</param>
	/// <returns>Base64码格式的字符串，对应的字节数组。</returns>
	public static byte[]? BytesByConvertFromBase64(
		string base64String,
		bool isAutoIgnoreContentTypeChars = true)
	{
		if (base64String.Length < 1)
		{
			return null;
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

	/// <summary>
	/// 将字节数组转为对应的Base64码格式的字符串。
	/// </summary>
	/// <param name="bytes">指定的字节数组。</param>
	/// <param name="offset">要转换的字节偏移量。</param>
	/// <param name="length">要转换的字节长度。</param>
	/// <param name="dataType">字节数据类型，如果指定了类型，则转换后的Base64码包含以下前缀： “data:#指定的数据类型#;base64,”，常见的格式如：“image/jpeg”。</param>
	/// <param name="base64FormattingOptions">Base64码的格式参数。</param>
	/// <returns>字节数组转为对应的Base64码格式的字符串。</returns>
	public static string ToBase64(
		this byte[] bytes,
		int offset,
		int length,
		string dataType,
		Base64FormattingOptions base64FormattingOptions = Base64FormattingOptions.None)
	{
		if (bytes == null
			|| bytes.Length < 1)
		{
			return string.Empty;
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
		if (length < 0)
		{
			return string.Empty;
		}

		var base64String = System.Convert.ToBase64String(
			bytes,
			offset,
			length,
			base64FormattingOptions);
		if (dataType?.Length > 0)
		{
			base64String
				= "data:" + dataType + ";base64,";
		}
		return base64String;
	}

	public static string ToMD532String(this byte[] bytes, int offset, int length)
	{
		return Security.Cryptography.SHA.CreateMD532String(
			bytes,
			offset,
			length);
	}
	public static string ToMD532String(this byte[] bytes)
	{
		return Security.Cryptography.SHA.CreateMD532String(
			bytes);
	}

	public static string ToMD516String(this byte[] bytes, int offset, int length)
	{
		return Security.Cryptography.SHA.CreateMD516String(
			bytes,
			offset,
			length);
	}

	public static string ToMD516String(this byte[] bytes)
	{
		return Security.Cryptography.SHA.CreateMD516String(
			bytes);
	}

	public static string ToSHA256(this byte[] bytes, int offset, int length)
	{
		return Security.Cryptography.SHA.CreateSHA256String(
			bytes,
			offset,
			length);
	}

	public static string ToSHA256(this byte[] bytes)
	{
		return Security.Cryptography.SHA.CreateSHA256String(
			bytes);
	}

	public static string ToSHA512(this byte[] bytes, int offset, int length)
	{
		return Security.Cryptography.SHA.CreateSHA512String(
			bytes,
			offset,
			length);
	}

	public static string ToSHA512(this byte[] bytes)
	{
		return Security.Cryptography.SHA.CreateSHA512String(
			bytes);
	}

	/// <summary>
	/// C#的“byte”为“无符号整数‘，Java的”byte“为”有符号整数“，因此需要值超过”127“的字节，减去”256“，以获取对应的Java字节值。
	/// </summary>
	/// <param name="bytes">C#的字节数组。</param>
	/// <returns>符合Java值定义的字节数组。</returns>
	public static byte[] ToJavaBytes(
		this byte[] bytes,
		int offset,
		int count)
	{
		var javaBytes = new sbyte[bytes.Length];
		{
			var beginByteIndex = offset;
			if (beginByteIndex < 0)
			{
				beginByteIndex = 0;
			}
			var endByteIndex = offset + count;
			if (endByteIndex > bytes.Length)
			{
				endByteIndex = bytes.Length;
			}

			for (int byteIndex = beginByteIndex;
				byteIndex < endByteIndex;
				byteIndex++)
			{
				var @byte = bytes[byteIndex];
				javaBytes[byteIndex]
					= @byte <= 127
					? (sbyte)@byte
					: (sbyte)(@byte - 256);
			}
		}
		return (byte[])(Array)javaBytes;
	}

	public static byte[] ToJavaBytes(this byte[] bytes)
	{
		return ToJavaBytes(bytes, 0, bytes.Length);
	}
}
