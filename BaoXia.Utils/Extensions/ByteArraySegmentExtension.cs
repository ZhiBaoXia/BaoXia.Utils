using System;
using System.IO.Compression;

namespace BaoXia.Utils.Extensions
{
        public static class ByteArraySegmentExtension
        {
                ////////////////////////////////////////////////
                // @”Brotli“压缩算法实现
                ////////////////////////////////////////////////

                #region ”Brotli“压缩算法实现

                public static byte[]? BytesByCompressWithBrotli(
                        this ArraySegment<byte> bytes,
                        CompressionLevel compressionLevel = CompressionLevel.Optimal)
                {
                        if (bytes.Array == null)
                        {
                                return null;
                        }
                        return ByteArrayExtension.BytesByCompressWithBrotli(
                                bytes.Array,
                                bytes.Offset,
                                bytes.Count,
                                compressionLevel);
                }

                public static ArraySegment<byte>? BytesByDecompressWithBrotli(
                        this ArraySegment<byte> bytesCompressed,
                        int readBufferSize = 0,
                        float readBufferSizeRatio = ByteArrayExtension.ReadBufferSizeRatioDefault)
                {
                        if (bytesCompressed.Array == null)
                        {
                                return null;
                        }
                        return ByteArrayExtension.BytesByDecompressWithBrotli(
                                bytesCompressed.Array,
                                bytesCompressed.Offset,
                                bytesCompressed.Count,
                                //
                                readBufferSize,
                                readBufferSizeRatio);
                }

                #endregion


                ////////////////////////////////////////////////
                // @”GZip“压缩算法实现
                ////////////////////////////////////////////////

                #region ”GZip“压缩算法实现

                public static byte[]? BytesByCompressWithGZip(this ArraySegment<byte> bytes)
                {
                        if (bytes.Array == null)
                        {
                                return null;
                        }
                        return ByteArrayExtension.BytesByCompressWithGZip(
                                bytes.Array,
                                bytes.Offset,
                                bytes.Count);
                }

                public static ArraySegment<byte>? BytesByDecompressWithGZip(
                        this ArraySegment<byte> bytesCompressed,
                        int readBufferSize = 0,
                        float readBufferSizeRatio = ByteArrayExtension.ReadBufferSizeRatioDefault)
                {
                        if (bytesCompressed.Array == null)
                        {
                                return null;
                        }
                        return ByteArrayExtension.BytesByDecompressWithGZip(
                                bytesCompressed.Array,
                                bytesCompressed.Offset,
                                bytesCompressed.Count,
                                //
                                readBufferSize,
                                readBufferSizeRatio);
                }

                #endregion


                ////////////////////////////////////////////////
                // @”Deflate“压缩算法实现
                ////////////////////////////////////////////////

                #region ”Deflate“压缩算法实现
                public static byte[]? BytesByCompressWithDeflate(
                        this ArraySegment<byte> bytes)
                {
                        if (bytes.Array == null)
                        {
                                return null;
                        }
                        return ByteArrayExtension.BytesByCompressWithDeflate(
                                bytes.Array,
                                bytes.Offset,
                                bytes.Count);
                }

                public static ArraySegment<byte>? BytesByDecompressWithDeflate(
                        this ArraySegment<byte> bytesCompressed,
                        int readBufferSize = 0,
                        float readBufferSizeRatio = ByteArrayExtension.ReadBufferSizeRatioDefault)
                {
                        if (bytesCompressed.Array == null)
                        {
                                return null;
                        }
                        return ByteArrayExtension.BytesByDecompressWithDeflate(
                                bytesCompressed.Array,
                                bytesCompressed.Offset,
                                bytesCompressed.Count,
                                //
                                readBufferSize,
                                readBufferSizeRatio);
                }

                #endregion


                public static string ToBase64(
                        this ArraySegment<byte> bytes,
                        //
                        string dataType,
                        Base64FormattingOptions base64FormattingOptions = Base64FormattingOptions.None)
                {
                        if (bytes.Array == null)
                        {
                                return string.Empty;
                        }
                        return ByteArrayExtension.ToBase64(
                                bytes.Array,
                                bytes.Offset,
                                bytes.Count,
                                //
                                dataType,
                                base64FormattingOptions);
                }

                public static string ToMD532String(
                        this ArraySegment<byte> bytes)
                {
                        if (bytes.Array == null)
                        {
                                return string.Empty;
                        }
                        return ByteArrayExtension.ToMD532String(
                                bytes.Array,
                                bytes.Offset,
                                bytes.Count);
                }

                public static string ToMD516String(
                        this ArraySegment<byte> bytes)
                {
                        if (bytes.Array == null)
                        {
                                return string.Empty;
                        }
                        return ByteArrayExtension.ToMD516String(
                                bytes.Array,
                                bytes.Offset,
                                bytes.Count);
                }


                public static string ToSHA256(
                        this ArraySegment<byte> bytes)
                {
                        if (bytes.Array == null)
                        {
                                return string.Empty;
                        }
                        return ByteArrayExtension.ToSHA256(
                                bytes.Array,
                                bytes.Offset,
                                bytes.Count);
                }

                public static string ToSHA512(
                        this ArraySegment<byte> bytes)
                {
                        if (bytes.Array == null)
                        {
                                return string.Empty;
                        }
                        return ByteArrayExtension.ToSHA512(
                                bytes.Array,
                                bytes.Offset,
                                bytes.Count);
                }

                public static byte[]? ToJavaBytes(
                        this ArraySegment<byte> bytes)
                {
                        if (bytes.Array == null)
                        {
                                return null;
                        }
                        return ByteArrayExtension.ToJavaBytes(
                                bytes.Array,
                                bytes.Offset,
                                bytes.Count);
                }
        }
}
