using System.Security.Cryptography;
using System.Text;

namespace BaoXia.Utils.Security.Cryptography
{
        public static class SHA
        {
                #region 基础操作
                /// <summary>
                /// 为字符串创建字节数组。
                /// </summary>
                /// <param name="sourceString">指定的字符串。</param>
                /// <param name="textEncoding">字符串字节信息的编码方式，默认为：UTF8。</param>
                /// <returns>使用指定的编码方式编码后生成的字符串字节数组。</returns>
                public static byte[] CreateBytesFromString(
                        string sourceString,
                        System.Text.Encoding? textEncoding = null)
                {
                        if (sourceString == null
                                || sourceString.Length < 1)
                        {
                                return System.Array.Empty<byte>();
                        }

                        if (textEncoding == null)
                        {
                                textEncoding = System.Text.Encoding.UTF8;
                        }
                        byte[] textBytes = textEncoding.GetBytes(sourceString);
                        { }
                        return textBytes;
                }

                /// <summary>
                /// 将字节数组转为16进制的字符串。
                /// </summary>
                /// <param name="sourceBytes">指定的字节数组。</param>
                /// <param name="offset">指定的字节数组偏移量。</param>
                /// <param name="count">指定的字节数组长度。</param>
                /// <returns>字节数组对应的16进制字符串。</returns>
                public static string CreateHexStringFromBytes(
                        byte[] sourceBytes,
                        int offset,
                        int count)
                {
                        if (sourceBytes == null
                                || sourceBytes.Length < 1
                                || offset < 0
                                || offset >= sourceBytes.Length
                                || count <= 0)
                        {
                                return string.Empty;
                        }
                        if (offset + count > sourceBytes.Length)
                        {
                                count = sourceBytes.Length - offset;
                        }

                        StringBuilder finalStringBuilder = new();
                        for (int charIndex = offset;
                                charIndex < count;
                                charIndex++)
                        {
                                finalStringBuilder.Append(sourceBytes[charIndex].ToString("X2"));
                        }
                        var finalString = finalStringBuilder.ToString();
                        { }
                        return finalString;
                }

                /// <summary>
                /// 将字节数组转为16进制的字符串。
                /// </summary>
                /// <param name="sourceBytes">指定的字节数组。</param>
                /// <returns>字节数组对应的16进制字符串。</returns>
                public static string CreateHexStringFromBytes(
                        byte[] sourceBytes)
                {
                        return SHA.CreateHexStringFromBytes(
                                sourceBytes,
                                0,
                                sourceBytes.Length);
                }

                #endregion

                #region MD5_32

                /// <summary>
                /// 使用MD5算法生成指定字节数组的哈希值，32位字符串。
                /// </summary>
                /// <param name="plaintextBytes">指定的字节数组。</param>
                /// <param name="offset">指定的字节数组偏移量。</param>
                /// <param name="count">指定的字节数组长度。</param>
                /// <returns>字符串对应的哈希值。</returns>
                public static string CreateMD532String(
                byte[] plaintextBytes,
                int offset,
                int count)
                {
                        if (plaintextBytes == null
                                || plaintextBytes.Length < 1
                                || offset < 0
                                || offset >= plaintextBytes.Length
                                || count <= 0)
                        {
                                return string.Empty;
                        }
                        if (offset + count > plaintextBytes.Length)
                        {
                                count = plaintextBytes.Length - offset;
                        }


                        string sourceTextMd5String;
                        // 调用加密方法
                        MD5 md5 = MD5.Create();
                        {
                                byte[] md5HashCode = md5.ComputeHash(plaintextBytes, offset, count);
                                // !!!
                                sourceTextMd5String = SHA.CreateHexStringFromBytes(md5HashCode);
                                // !!!
                        }
                        md5.Clear();

                        return sourceTextMd5String;
                }

                /// <summary>
                /// 使用MD5算法生成指定字节数组的哈希值，32位字符串。
                /// </summary>
                /// <param name="plaintextBytes">指定的字节数组。</param>
                /// <returns>字符串对应的哈希值。</returns>
                public static string CreateMD532String(
                byte[] plaintextBytes)
                {
                        return SHA.CreateMD532String(
                                plaintextBytes,
                                0,
                                plaintextBytes.Length);
                }

                /// <summary>
                /// 使用MD5算法生成指定字符串的哈希值，32位的字符串。
                /// </summary>
                /// <param name="plaintext">指定的明文字符串。</param>
                /// <param name="textEncoding">指定的字符编码方式。</param>
                /// <returns>字符串对应的哈希值。</returns>
                public static string CreateMD532String(
                        string plaintext,
                        System.Text.Encoding? textEncoding = null)
                {
                        var plaintextBytes = SHA.CreateBytesFromString(plaintext, textEncoding);
                        // !!!
                        var md5HashCodeString = SHA.CreateMD532String(plaintextBytes);
                        // !!!
                        { }
                        return md5HashCodeString;
                }

                #endregion

                #region MD5_16

                /// <summary>
                /// 使用MD5算法生成指定字节数组的哈希值，16位字符串。
                /// </summary>
                /// <param name="plaintextBytes">指定的字节数组。</param>
                /// <param name="offset">指定的字节数组偏移量。</param>
                /// <param name="count">指定的字节数组长度。</param>
                /// <returns>字符串对应的哈希值。</returns>
                public static string CreateMD516String(
                        byte[] plaintextBytes,
                        int offset,
                        int count)
                {
                        var sourceTextMd532String = SHA.CreateMD532String(plaintextBytes, offset, count);
                        var sourceTextMd516String = sourceTextMd532String.Substring(8, 16);
                        { }
                        return sourceTextMd516String;
                }

                /// <summary>
                /// 使用MD5算法生成指定字节数组的哈希值，16位字符串。
                /// </summary>
                /// <param name="plaintextBytes">指定的字节数组。</param>
                /// <returns>字符串对应的哈希值。</returns>
                public static string CreateMD516String(
                        byte[] plaintextBytes)
                {
                        return SHA.CreateMD516String(
                                plaintextBytes,
                                0,
                                plaintextBytes.Length);
                }

                /// <summary>
                /// 使用MD5算法生成指定字符串的哈希值，16位的字符串。
                /// </summary>
                /// <param name="plaintext">指定的明文字符串。</param>
                /// <param name="textEncoding">指定的字符编码方式。</param>
                /// <returns>字符串对应的哈希值。</returns>
                public static string CreateMD516String(
                        string plaintext,
                        System.Text.Encoding? textEncoding = null)
                {
                        var plaintextBytes = SHA.CreateBytesFromString(plaintext, textEncoding);
                        // !!!
                        var md5HashCodeString = SHA.CreateMD516String(plaintextBytes);
                        // !!!
                        { }
                        return md5HashCodeString;
                }

                #endregion

                #region SHA256

                /// <summary>
                /// 使用SHA256算法生成指定字节数组的哈希值。
                /// </summary>
                /// <param name="plaintextBytes">指定的字节数组。</param>
                /// <param name="offset">指定的字节数组偏移量。</param>
                /// <param name="count">指定的字节数组长度。</param>
                /// <returns>字符串对应的哈希值。</returns>
                public static string CreateSHA256String(
                        byte[] plaintextBytes,
                        int offset,
                        int count)
                {
                        if (plaintextBytes == null
                                || plaintextBytes.Length < 1
                                || offset < 0
                                || offset >= plaintextBytes.Length
                                || count <= 0)
                        {
                                return string.Empty;
                        }
                        if (offset + count > plaintextBytes.Length)
                        {
                                count = plaintextBytes.Length - offset;
                        }

                        using var sha256 = SHA256.Create();
                        var sha256HashCode = sha256.ComputeHash(plaintextBytes, offset, count);
                        // !!!
                        var sha256HashCodeString = SHA.CreateHexStringFromBytes(sha256HashCode);
                        // !!!
                        { }
                        return sha256HashCodeString;
                }

                /// <summary>
                /// 使用SHA256算法生成指定字节数组的哈希值。
                /// </summary>
                /// <param name="plaintextBytes">指定的字节数组。</param>
                /// <returns>字符串对应的哈希值。</returns>
                public static string CreateSHA256String(
                        byte[] plaintextBytes)
                {
                        return SHA.CreateSHA256String(
                                plaintextBytes,
                                0,
                                plaintextBytes.Length);
                }

                /// <summary>
                /// 使用SHA256算法生成指定字符串的哈希值。
                /// </summary>
                /// <param name="plaintext">指定的字符串。</param>
                /// <param name="textEncoding">指定的字符编码方式。</param>
                /// <returns>字符串对应的哈希值。</returns>
                public static string CreateSHA256String(
                        string plaintext,
                        System.Text.Encoding? textEncoding = null)
                {
                        var plaintextBytes = SHA.CreateBytesFromString(plaintext, textEncoding);
                        // !!!
                        var sha256HashCodeString = SHA.CreateSHA256String(plaintextBytes);
                        // !!!
                        { }
                        return sha256HashCodeString;
                }

                #endregion

                #region SHA512

                /// <summary>
                /// 使用SHA512算法生成指定字节数组的哈希值。
                /// </summary>
                /// <param name="plaintextBytes">指定的字节数组。</param>
                /// <param name="offset">指定的字节数组偏移量。</param>
                /// <param name="count">指定的字节数组长度。</param>
                /// <returns>字符串对应的哈希值。</returns>
                public static string CreateSHA512String(
                        byte[] plaintextBytes,
                        int offset,
                        int count)
                {
                        if (plaintextBytes == null
                                || plaintextBytes.Length < 1
                                || offset < 0
                                || offset >= plaintextBytes.Length
                                || count <= 0)
                        {
                                return string.Empty;
                        }
                        if (offset + count > plaintextBytes.Length)
                        {
                                count = plaintextBytes.Length - offset;
                        }

                        using var sha512 = SHA512.Create();
                        var sha512HashCode = sha512.ComputeHash(plaintextBytes, offset, count);
                        // !!!
                        var sha512HashCodeString = SHA.CreateHexStringFromBytes(sha512HashCode);
                        // !!!
                        sha512.Clear();

                        return sha512HashCodeString;
                }

                /// <summary>
                /// 使用SHA512算法生成指定字节数组的哈希值。
                /// </summary>
                /// <param name="plaintextBytes">指定的字节数组。</param>
                /// <returns>字符串对应的哈希值。</returns>
                public static string CreateSHA512String(
                        byte[] plaintextBytes)
                {
                        return SHA.CreateSHA512String(
                                plaintextBytes,
                                0,
                                plaintextBytes.Length);
                }

                /// <summary>
                /// 使用SHA512算法生成指定字符串的哈希值。
                /// </summary>
                /// <param name="plaintext">指定的字符串。</param>
                /// <param name="textEncoding">指定的字符编码方式。</param>
                /// <returns>字符串对应的哈希值。</returns>
                public static string CreateSHA512String(
                        string plaintext,
                        System.Text.Encoding? textEncoding)
                {
                        var plaintextBytes = SHA.CreateBytesFromString(plaintext, textEncoding);
                        // !!!
                        var sha512HashCodeString = SHA.CreateSHA512String(plaintextBytes);
                        // !!!
                        { }
                        return sha512HashCodeString;
                }

                #endregion
        }
}
