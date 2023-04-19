using System.Net;

namespace BaoXia.Utils.Extensions
{
        public static class HttpRequestHeaderExtension
        {

                ////////////////////////////////////////////////
                // @静态变量
                ////////////////////////////////////////////////

                #region 静态变量

                public static string? ToHttpHeaderParamName(
                        this HttpRequestHeader httpRequestHeader)
                {
                        return httpRequestHeader switch
                        {
                                HttpRequestHeader.Accept => "Accept",
                                HttpRequestHeader.AcceptCharset => "Accept-Charset",
                                HttpRequestHeader.AcceptEncoding => "Accept-Encoding",
                                HttpRequestHeader.AcceptLanguage => "Accept-Language",
                                HttpRequestHeader.Allow => "Allow",
                                HttpRequestHeader.Authorization => "Authorization",
                                HttpRequestHeader.CacheControl => "Cache-Control",
                                HttpRequestHeader.Connection => "Connection",
                                HttpRequestHeader.ContentEncoding => "Content-Encoding",
                                HttpRequestHeader.ContentLanguage => "Content-Language",
                                HttpRequestHeader.ContentLength => "Content-Length",
                                HttpRequestHeader.ContentLocation => "Content-Location",
                                HttpRequestHeader.ContentMd5 => "Content-MD5",
                                HttpRequestHeader.ContentRange => "Content-Range",
                                HttpRequestHeader.ContentType => "Content-Type",
                                HttpRequestHeader.Cookie => "Cookie",
                                HttpRequestHeader.Date => "Date",
                                HttpRequestHeader.Expect => "Expect",
                                HttpRequestHeader.Expires => "Expires",
                                HttpRequestHeader.From => "From",
                                HttpRequestHeader.Host => "Host",
                                HttpRequestHeader.IfMatch => "If-Match",
                                HttpRequestHeader.IfModifiedSince => "If-Modified-Since",
                                HttpRequestHeader.IfNoneMatch => "If-None-Match",
                                HttpRequestHeader.IfRange => "If-Range",
                                HttpRequestHeader.IfUnmodifiedSince => "If-Unmodified-Since",
                                HttpRequestHeader.KeepAlive => "Keep-Alive",
                                HttpRequestHeader.LastModified => "Last-Modified",
                                HttpRequestHeader.MaxForwards => "Max-Forwards",
                                HttpRequestHeader.Pragma => "Pragma",
                                HttpRequestHeader.ProxyAuthorization => "Proxy-Authorization",
                                HttpRequestHeader.Range => "Range",
                                HttpRequestHeader.Referer => "Referer",
                                HttpRequestHeader.Te => "Te",
                                HttpRequestHeader.Trailer => "Trailer",
                                HttpRequestHeader.TransferEncoding => "Transfer-Encoding",
                                HttpRequestHeader.Translate => "Translate",
                                HttpRequestHeader.Upgrade => "Upgrade",
                                HttpRequestHeader.UserAgent => "User-Agent",
                                HttpRequestHeader.Via => "Via",
                                HttpRequestHeader.Warning => "Warning",
                                _ => null,
                        };
                }

                #endregion

        }
}
