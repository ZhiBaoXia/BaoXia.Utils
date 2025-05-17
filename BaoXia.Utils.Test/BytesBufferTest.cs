using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BaoXia.Utils.Test
{
	[TestClass]
	public class BytesBufferTest
	{
		[TestMethod]
		public void AppendBytesTest()
		{
			var sourceBytesBuffer = new byte[1024];
			for (int i = 0; i < sourceBytesBuffer.Length; i++)
			{
				sourceBytesBuffer[i] = (byte)i;
			}

			const int copyPageSize = 12;
			var destinationBytesBuffer = new BytesBuffer(copyPageSize);

			for (var allBytesCountCopied = 0;
				allBytesCountCopied < sourceBytesBuffer.Length;
				allBytesCountCopied += copyPageSize)
			{
				var bytesBuffer
					= destinationBytesBuffer.GetEmptyBufferSegment(
						copyPageSize);
				var bytesCountCopied = copyPageSize;
				{
					if (allBytesCountCopied + bytesCountCopied > sourceBytesBuffer.Length)
					{
						bytesCountCopied
							= sourceBytesBuffer.Length
							- allBytesCountCopied;
					}
					var bytesBufferArray = bytesBuffer.Array;
					if (bytesBufferArray != null)
					{
						Array.Copy(
							sourceBytesBuffer, allBytesCountCopied,
							bytesBufferArray, bytesBuffer.Offset,
							bytesCountCopied);
					}
				}
				destinationBytesBuffer.BytesCount += bytesCountCopied;
			}

			Assert.AreEqual(destinationBytesBuffer.BytesCount, sourceBytesBuffer.Length);
			for (int i = 0; i < sourceBytesBuffer.Length; i++)
			{
				var sourceByte = sourceBytesBuffer[i];
				var destinationByte = destinationBytesBuffer[i];
				// !!!
				Assert.AreEqual(destinationByte, sourceByte);
				// !!!
			}
		}
	}
}
