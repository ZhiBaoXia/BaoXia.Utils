using BaoXia.Utils.Extensions;
using System;
using System.Text;

namespace BaoXia.Utils
{
	public class BytesOperator
	{

		////////////////////////////////////////////////
		// @自身属性
		////////////////////////////////////////////////

		#region 自身属性

		public BytesBuffer BytesBuffer { get; set; }

		public ArraySegment<byte> Bytes
		{
			get
			{
				return BytesBuffer.Bytes;
			}
			set
			{
				byte[] bytes;
				if (value.Offset == 0
					&& value.Array != null
					&& value.Count == value.Array.Length)
				{
					bytes = value.Array;
				}
				else
				{
					bytes = [.. value];
				}
				// !!!
				BytesBuffer.SetBuffer(bytes, bytes.Length);
				// !!! 设置完字节数组后，要重置当前游标 !!!
				CursorPosition = 0;
				// !!!
			}
		}


		public int Count
		{
			get
			{
				return BytesBuffer.BytesCount;
			}
			set
			{
				BytesBuffer.BytesCount = value;
			}
		}

		public int CursorPosition { get; set; }

		#endregion


		////////////////////////////////////////////////
		// @自身实现
		////////////////////////////////////////////////

		#region 自身实现

		public BytesOperator(
			int bufferCapacity = BytesBuffer.BytesBufferCapacityDefault)
		{
			BytesBuffer = new BytesBuffer(bufferCapacity);
		}

		public BytesOperator(
			BytesBuffer? bytesBuffer,
			int bufferCapacity = BytesBuffer.BytesBufferCapacityDefault)
		{
			BytesBuffer = bytesBuffer ?? new BytesBuffer(bufferCapacity);
		}

		public BytesOperator(
			byte[]? bytes,
			int bufferCapacity = BytesBuffer.BytesBufferCapacityDefault)
		{
			BytesBuffer = new BytesBuffer(
				bytes,
				bufferCapacity);
			if (bytes != null)
			{
				this.Count = bytes.Length;
			}
		}

		public void Clear()
		{
			this.Count = 0;
			CursorPosition = 0;
		}

		public byte[] ToBytes()
		{
			return BytesBuffer.ToBytes();
		}

		public int Write(
			byte[]? sourceBytes,
			int sourceBytesBeginIndex = 0,
			int sourceBytesWriteCount = -1)
		{
			if (sourceBytes == null
				|| sourceBytes.Length < 1)
			{
				return 0;
			}
			var sourceBytesCount = sourceBytes.Length;
			if (sourceBytesBeginIndex >= sourceBytesCount)
			{
				return 0;
			}
			if (sourceBytesWriteCount < 0)
			{
				sourceBytesWriteCount = sourceBytesCount - sourceBytesBeginIndex;
			}

			if (sourceBytesBeginIndex < 0)
			{
				sourceBytesWriteCount += sourceBytesBeginIndex;
				sourceBytesBeginIndex = 0;
			}
			if ((sourceBytesBeginIndex + sourceBytesWriteCount) > sourceBytesCount)
			{
				sourceBytesWriteCount = sourceBytesCount - sourceBytesBeginIndex;
			}
			if (sourceBytesWriteCount <= 0)
			{
				return 0;
			}

			var cursorPositionBeforeWrite = CursorPosition;
			var cursorPosition = cursorPositionBeforeWrite;
			var bytesBuffer = BytesBuffer.GetBufferWithBufferLength(cursorPosition + sourceBytesWriteCount);
			{
				Array.Copy(
					sourceBytes,
					sourceBytesBeginIndex,
					//
					bytesBuffer,
					//
					cursorPosition,
					sourceBytesWriteCount);
				cursorPosition += sourceBytesWriteCount;
				if (BytesBuffer.BytesCount < cursorPosition)
				{
					BytesBuffer.BytesCount = cursorPosition;
				}
				// !!!
				CursorPosition = cursorPosition;
				// !!!
			}
			return (cursorPosition - cursorPositionBeforeWrite);
		}

		public int Write(ArraySegment<byte> bytesSegment)
		{
			return Write(
				bytesSegment.Array,
				bytesSegment.Offset,
				bytesSegment.Count);
		}

		public int Write(Span<byte> byteSpan)
		{
			return Write(byteSpan.ToArray());
		}

		public int Write(Memory<byte> byteMemory)
		{
			return Write(byteMemory.ToArray());
		}

		public int Write(byte byteValue)
		{
			return Write(new byte[] { byteValue });
		}

		public int Write(bool boolValue)
		{
			return Write(BitConverter.GetBytes(boolValue));
		}

		public int Write(uint uintValue)
		{
			return Write(BitConverter.GetBytes(uintValue));
		}

		public int Write(int intValue)
		{
			return Write(BitConverter.GetBytes(intValue));
		}

		public int Write(ulong ulongValue)
		{
			return Write(BitConverter.GetBytes(ulongValue));
		}

		public int Write(long longValue)
		{
			return Write(BitConverter.GetBytes(longValue));
		}

		public int Write(float floatValue)
		{
			return Write(BitConverter.GetBytes(floatValue));
		}

		public int Write(double doubleValue)
		{
			return Write(BitConverter.GetBytes(doubleValue));
		}

		public int Write(
			string? textValue,
			System.Text.Encoding? textEncoding = null)
		{
			int textValueBytesCount = 0;
			byte[]? textValueBytes = null;
			if (!string.IsNullOrEmpty(textValue))
			{
				//
				textEncoding ??= Encoding.UTF8;
				//
				textValueBytes = textEncoding.GetBytes(textValue);
				textValueBytesCount = textValueBytes.Length;
			}

			// !!! 无论字符串长度，都要写入 !!!
			this.Write(textValueBytesCount);
			// !!!
			return Write(textValueBytes);
		}

		public int Write(
			DateTime dateTime)
		{
			return this.Write(dateTime.MillisecondsFrom1970(
				Constants.TimeZoneNumber.Utc0,
				true));
		}

		public int Write(
			DateTimeOffset dateTimeOffset)
		{
			return this.Write(dateTimeOffset.MillisecondsFrom1970(
				Constants.TimeZoneNumber.Utc0,
				true));
		}

		public Span<byte> ReadBytes(
			int readBytesCount)
		{
			var cursorPosition = CursorPosition;
			if (cursorPosition < 0)
			{
				throw new IndexOutOfRangeException("读取字节信息失败，游标位置无效，小于“0”。");
			}
			if ((cursorPosition + readBytesCount) > BytesBuffer.BytesCount)
			{
				throw new IndexOutOfRangeException("读取字节信息失败，读取长度超过了当前字节数组长度。");
			}

			var bytesBuffer = BytesBuffer.Buffer;
			var bytes = new Span<byte>(
				bytesBuffer,
				cursorPosition,
				readBytesCount);
			{
				// !!!
				CursorPosition = cursorPosition + readBytesCount;
				// !!!
			}
			return bytes;
		}

		public byte ReadByte()
		{
			var bytesSpan = this.ReadBytes(sizeof(byte));
			var value = bytesSpan[0];
			{ }
			return value;
		}

		public bool ReadBool()
		{
			var bytesSpan = this.ReadBytes(sizeof(bool));
			var value = BitConverter.ToBoolean(bytesSpan);
			{ }
			return value;
		}

		public uint ReadUInt()
		{
			var bytesSpan = this.ReadBytes(sizeof(uint));
			var value = BitConverter.ToUInt32(bytesSpan);
			{ }
			return value;
		}

		public int ReadInt()
		{
			var bytesSpan = this.ReadBytes(sizeof(int));
			var value = BitConverter.ToInt32(bytesSpan);
			{ }
			return value;
		}

		public ulong ReadULong()
		{
			var bytesSpan = this.ReadBytes(sizeof(ulong));
			var value = BitConverter.ToUInt64(bytesSpan);
			{ }
			return value;
		}

		public long ReadLong()
		{
			var bytesSpan = this.ReadBytes(sizeof(long));
			var value = BitConverter.ToInt64(bytesSpan);
			{ }
			return value;
		}

		public float ReadFloat()
		{
			var bytesSpan = this.ReadBytes(sizeof(float));
			var value = BitConverter.ToSingle(bytesSpan);
			{ }
			return value;
		}

		public double ReadDouble()
		{
			var bytesSpan = this.ReadBytes(sizeof(double));
			var value = BitConverter.ToDouble(bytesSpan);
			{ }
			return value;
		}

		public string? ReadString(
			System.Text.Encoding? textEncoding = null)
		{
			var stringBytesCount = ReadInt();
			if (stringBytesCount < 0)
			{
				throw new DataMisalignedException("读取字符串失败，字符串长度无效，小于“0”。");
			}
			if (stringBytesCount == 0)
			{
				return null;
			}

			var textBytes = ReadBytes(stringBytesCount);
			textEncoding ??= Encoding.UTF8;
			var value = textEncoding.GetString(textBytes);
			{ }
			return value;
		}

		public DateTime ReadDateTime()
		{
			var dateTimestamp = ReadLong();
			var dateTime = DateTimeUtil.DateTimeWithMillisecondsAfter1970(dateTimestamp);
			{ }
			return dateTime;
		}

		public DateTimeOffset ReadDateTimeOffset()
		{
			var dateTimestamp = ReadLong();
			var dateTime = DateTimeUtil.DateTimeWithMillisecondsAfter1970(dateTimestamp);
			{ }
			return dateTime;
		}

		public bool TryToReadBytes(
			int readBytesCount,
			out Span<byte> bytes)
		{
			bytes = default;

			var cursorPosition = CursorPosition;
			if (cursorPosition < 0)
			{
				return false;
			}
			if ((cursorPosition + readBytesCount) > BytesBuffer.BytesCount)
			{
				return false;
			}

			var bytesBuffer = BytesBuffer.Buffer;
			bytes = new Span<byte>(
				bytesBuffer,
				cursorPosition,
				readBytesCount);
			{
				// !!!
				CursorPosition = cursorPosition + readBytesCount;
				// !!!
			}
			return true;
		}

		public bool TryToReadByte(out byte byteValue)
		{
			byteValue = default;

			if (!this.TryToReadBytes(1, out var bytesRead))
			{
				return false;
			}
			// !!!
			byteValue = bytesRead[0];
			// !!!
			return true;
		}

		public bool TryToReadBool(out bool boolValue)
		{
			boolValue = default;

			if (!this.TryToReadBytes(sizeof(bool), out var bytesRead))
			{
				return false;
			}
			// !!!
			boolValue = BitConverter.ToBoolean(bytesRead);
			// !!!
			return true;
		}

		public bool TryToReadUInt(out uint uintValue)
		{
			uintValue = default;

			if (!this.TryToReadBytes(sizeof(uint), out var bytesRead))
			{
				return false;
			}
			// !!!
			uintValue = BitConverter.ToUInt32(bytesRead);
			// !!!
			return true;
		}

		public bool TryToReadInt(out int intValue)
		{
			intValue = default;

			if (!this.TryToReadBytes(sizeof(int), out var bytesRead))
			{
				return false;
			}
			// !!!
			intValue = BitConverter.ToInt32(bytesRead);
			// !!!
			return true;
		}

		public bool TryToReadULong(out ulong ulongValue)
		{
			ulongValue = default;

			if (!this.TryToReadBytes(sizeof(ulong), out var bytesRead))
			{
				return false;
			}
			// !!!
			ulongValue = BitConverter.ToUInt64(bytesRead);
			// !!!
			return true;
		}

		public bool TryToReadLong(out long longValue)
		{
			longValue = default;

			if (!this.TryToReadBytes(sizeof(long), out var bytesRead))
			{
				return false;
			}
			// !!!
			longValue = BitConverter.ToInt64(bytesRead);
			// !!!
			return true;
		}

		public bool TryToReadFloat(out float floatValue)
		{
			floatValue = default;

			if (!this.TryToReadBytes(sizeof(float), out var bytesRead))
			{
				return false;
			}
			// !!!
			floatValue = BitConverter.ToSingle(bytesRead);
			// !!!
			return true;
		}

		public bool TryToReadDouble(out double doubleValue)
		{
			doubleValue = default;

			if (!this.TryToReadBytes(sizeof(double), out var bytesRead))
			{
				return false;
			}
			// !!!
			doubleValue = BitConverter.ToDouble(bytesRead);
			// !!!
			return true;
		}

		public bool TryToReadString(
			out string? textValue,
			System.Text.Encoding? textEncoding = null)
		{
			textValue = null;

			var lastCursorPosition = CursorPosition;
			if (!TryToReadInt(out var stringBytesLength))
			{
				return false;
			}
			if (stringBytesLength < 0)
			{
				// !!!
				CursorPosition = lastCursorPosition;
				// !!!
				return false;
			}
			if (stringBytesLength == 0)
			{
				return true;
			}

			if (!TryToReadBytes(stringBytesLength, out var textBytes))
			{
				// !!!
				CursorPosition = lastCursorPosition;
				// !!!
				return false;
			}

			textEncoding ??= Encoding.UTF8;
			// !!!
			textValue = textEncoding.GetString(textBytes);
			// !!!

			return true;
		}

		public bool TryToReadDateTime(out DateTime dateTimeValue)
		{
			dateTimeValue = default;

			var lastCursorPosition = CursorPosition;
			if (!TryToReadLong(out var dateTimestamp))
			{
				return false;
			}
			if (dateTimestamp < 0)
			{
				// !!!
				CursorPosition = lastCursorPosition;
				// !!!
				return false;
			}
			// !!!
			dateTimeValue = DateTimeUtil.DateTimeWithMillisecondsAfter1970(dateTimestamp);
			// !!!
			return true;
		}

		#endregion
	}
}
