using System.Collections.Generic;
using System.Net;

namespace BaoXia.Utils.Extensions
{
        /// <summary>
        /// “WebHeaderCollection”扩展类。
        /// </summary>
        public static class WebHeaderCollectionExtension
        {
                /// <summary>
                /// 将字符串字典中的值，填入Http请求头。，
                /// </summary>
                /// <param name="httpRequestHeaders">要被填充的Http请求头对象。</param>
                /// <param name="headers">要填充的Http请求头键值对。</param>
                public static void AddRange(
                        this WebHeaderCollection webHeaderCollection,
                        Dictionary<string, string> headers)
                {
                        if (webHeaderCollection != null
                                && headers?.Count > 0)
                        {
                                foreach (var header in headers)
                                {
                                        webHeaderCollection.Add(header.Key, header.Value);
                                }
                        }
                }
        }
}
