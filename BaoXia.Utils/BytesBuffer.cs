using System;

namespace BaoXia.Utils
{
        public class BytesBuffer
        {
                ////////////////////////////////////////////////
                // @静态常量
                ////////////////////////////////////////////////

                #region 静态常量

                public const int BytesBufferCapacityDefault = 32 * 1024;

                #endregion


                ////////////////////////////////////////////////
                // @自身属性
                ////////////////////////////////////////////////

                #region 自身属性

                byte[]? _bytesBuffer = null;
                readonly int _bytesBufferCapacity = BytesBufferCapacityDefault;

                int _bytesCount = 0;

                public int BytesCount
                {
                        get
                        {
                                return _bytesCount;
                        }

                        set
                        {
                                if (_bytesCount < value)
                                {
                                        // !!!
                                        this.GetBufferWithBufferLength(value);
                                        // !!!
                                }
                                // !!!
                                _bytesCount = value;
                                // !!!
                        }
                }

                public byte[] Buffer
                {
                        get
                        {
                                return this.GetBufferWithBufferLength();
                        }
                }

                public int BufferLength
                {
                        get
                        {
                                // !!!
                                return this.GetBufferWithBufferLength().Length;
                                // !!!
                        }
                }


                public int BufferLengthMax => this.BufferLength;

                public int BufferLengthWrote => this.BytesCount;

                public int BufferLengthCanAppendWrite => (this.BufferLengthMax - this.BytesCount);

                public ArraySegment<byte> Bytes
                {
                        get
                        {
                                return new ArraySegment<byte>(
                                        this.GetBufferWithBufferLength(_bytesCount),
                                        0,
                                        _bytesCount);
                        }
                }

                public byte this[int index]
                {
                        get
                        {
                                if (index < 0
                                        || index > _bytesBuffer?.Length)
                                {
                                        throw new ArgumentOutOfRangeException("index");
                                }
                                return _bytesBuffer![index];
                        }
                        set
                        {
                                if (index < 0
                                        || index > _bytesBuffer?.Length)
                                {
                                        throw new ArgumentOutOfRangeException("index");
                                }
                                _bytesBuffer![index] = value;
                        }
                }

                #endregion


                ////////////////////////////////////////////////
                // @自身实现
                ////////////////////////////////////////////////

                #region 自身实现

                public BytesBuffer(int bufferCapacity = BytesBuffer.BytesBufferCapacityDefault)
                {
                        if (bufferCapacity <= 0)
                        {
                                bufferCapacity = BytesBuffer.BytesBufferCapacityDefault;
                        }

                        _bytesBuffer = new byte[bufferCapacity];
                        _bytesBufferCapacity = bufferCapacity;

                        _bytesCount = 0;
                }

                public BytesBuffer(
                        byte[]? bytesBuffer,
                        int bufferCapacity = BytesBuffer.BytesBufferCapacityDefault)
                {
                        if (bufferCapacity <= 0)
                        {
                                bufferCapacity = BytesBuffer.BytesBufferCapacityDefault;
                        }

                        _bytesBuffer = bytesBuffer ?? new byte[bufferCapacity];
                        _bytesBufferCapacity = bufferCapacity;

                        _bytesCount = 0;
                }

                public byte[] GetBufferWithBufferLength(int bytesBufferLength = 0)
                {
                        if (bytesBufferLength <= 0)
                        {
                                bytesBufferLength = _bytesBufferCapacity;
                        }
                        if (_bytesBuffer == null)
                        {
                                _bytesBuffer = new byte[bytesBufferLength];
                        }
                        else if (bytesBufferLength > _bytesBuffer.Length)
                        {
                                var newBytesBuffer = new byte[bytesBufferLength];
                                {
                                        Array.Copy(
                                                _bytesBuffer,
                                                newBytesBuffer,
                                                _bytesBuffer.Length);
                                }
                                _bytesBuffer = newBytesBuffer;
                        }
                        return _bytesBuffer;
                }

                public ArraySegment<byte> GetEmptyBufferSegment(int emptyBufferLength = 0)
                {
                        if (emptyBufferLength <= 0)
                        {
                                emptyBufferLength = _bytesBufferCapacity;
                        }
                        // !!!
                        var bytesBuffer = this.GetBufferWithBufferLength(_bytesCount + emptyBufferLength);
                        // !!!
                        return new ArraySegment<byte>(
                                bytesBuffer,
                                _bytesCount,
                                bytesBuffer.Length - _bytesCount);
                }

                public Span<byte> GetEmptyBufferSpan(int emptyBufferLength = 0)
                {
                        if (emptyBufferLength <= 0)
                        {
                                emptyBufferLength = _bytesBufferCapacity;
                        }
                        // !!!
                        var bytesBuffer = this.GetBufferWithBufferLength(_bytesCount + emptyBufferLength);
                        // !!!
                        return new Span<byte>(
                                bytesBuffer,
                                _bytesCount,
                                bytesBuffer.Length - _bytesCount);
                }

                public Memory<byte> GetEmptyBufferMemory(int emptyBufferLength = 0)
                {
                        if (emptyBufferLength <= 0)
                        {
                                emptyBufferLength = _bytesBufferCapacity;
                        }
                        // !!!
                        var bytesBuffer = this.GetBufferWithBufferLength(_bytesCount + emptyBufferLength);
                        // !!!
                        return new Memory<byte>(
                                bytesBuffer,
                                _bytesCount,
                                bytesBuffer.Length - _bytesCount);
                }


                public void SetBuffer(
                        byte[] buffer,
                        int bytesCount)
                {
                        _bytesBuffer = buffer;
                        _bytesCount = bytesCount;
                }

                ////////////////////////////////////////////////
                ////////////////////////////////////////////////


                public int AppendBytes(byte[] bytes, int bytesCount)
                {
                        var bytesBuffer = this.GetEmptyBufferSegment(bytesCount);
                        Array.Copy(
                                bytes, 0,
                                bytesBuffer.Array!,
                                bytesBuffer.Offset,
                                bytesCount);
                        // !!!
                        _bytesCount += bytesCount;
                        // !!!
                        return _bytesCount;
                }

                public int AppendBytes(byte[] bytes)
                {
                        return this.AppendBytes(bytes, bytes.Length);
                }

                public void Clear()
                {
                        _bytesCount = 0;
                }

                public byte[] ToBytes()
                {
                        return this.Bytes.ToArray();
                }

                #endregion

        }
}
