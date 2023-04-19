using System;

namespace BaoXia.Utils
{
        public class TextEncodingUtil
        {
                ////////////////////////////////////////////////
                // @静态常量
                ////////////////////////////////////////////////

                #region 静态常量

                public static readonly System.Text.Encoding DefaultEncoding = System.Text.Encoding.UTF8;

                #endregion

                ////////////////////////////////////////////////
                // @类方法
                ////////////////////////////////////////////////

                #region 类方法

                public static System.Text.Encoding EncodingNamed(
                        string? encodingName,
                        System.Text.Encoding? defaultEncoding = null)
                {
                        System.Text.Encoding? encoding = null;
                        if (string.IsNullOrEmpty(encodingName) == false)
                        {
                                encoding = System.Text.Encoding.GetEncoding(encodingName);
                        }
                        encoding ??= defaultEncoding ?? DefaultEncoding;
                        return encoding;
                }

                public static String NameOfEncoding(
                        System.Text.Encoding? encoding,
                        System.Text.Encoding? defaultEncoding = null)
                {
                        string? encodingName = null;
                        if (encoding != null)
                        {
                                // 使用更通用的WebName：
                                encodingName = encoding.WebName;
                        }
                        encodingName ??= (defaultEncoding ?? DefaultEncoding).WebName;
                        return encodingName;
                }

                #endregion

        }
}