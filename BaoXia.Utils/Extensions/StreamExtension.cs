using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Extensions
{
	public static class StreamExtension
	{
		public static async Task<byte[]?> ReadBytesAsync(
			this Stream stream,
			string? bytesContentType,
			long? bytesContentCount,
			Action<string?, long, float, byte[]?>? toReceiveDownloadProgress,
			CancellationToken cancellationToken = default)
		{
			if (stream == null
				|| stream.Length < 1)
			{
				toReceiveDownloadProgress?.Invoke(null, 0, 1.0F, null);
				return null;
			}

			if (bytesContentCount == null
				|| bytesContentCount < 1)
			{
				toReceiveDownloadProgress?.Invoke(null, 0, 1.0F, null);
				return null;
			}

			var responseBufferBytes = new BytesBuffer();
			while (true)
			{
				var bytesCountRead = await stream.ReadAsync(
					responseBufferBytes.GetEmptyBufferMemory(),
					cancellationToken);
				var downloadProgress
					= 1.0F
					* responseBufferBytes.BytesCount
					/ bytesContentCount.Value;

				if (bytesCountRead < 1)
				{
					var responseBytes = responseBufferBytes.ToBytes();
					{
						toReceiveDownloadProgress?.Invoke(
							bytesContentType,
							bytesContentCount.Value,
							downloadProgress,
							responseBytes);
					}
					// !!!
					return responseBytes;
					// !!!
				}
				else
				{
					toReceiveDownloadProgress?.Invoke(
						bytesContentType,
						bytesContentCount.Value,
						downloadProgress,
						null);
				}
			}
		}
	}
}
