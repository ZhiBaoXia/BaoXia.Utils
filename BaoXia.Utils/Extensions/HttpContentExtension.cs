using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Extensions
{
        public static class HttpContentExtension
        {
                public static async Task<byte[]?> ReadAsBytesWithProgressActionAsync(
                        this HttpContent responseContent,
                        Action<string?, long, float, byte[]?>? toReceiveDownloadProgress,
                        CancellationToken cancellationToken = default)
                {
                        if (toReceiveDownloadProgress == null)
                        {
                                return await responseContent.ReadAsByteArrayAsync(cancellationToken);
                        }
                        return await responseContent
                                .ReadAsStream()
                                .ReadBytesAsync(
                                responseContent.Headers.ContentType?.MediaType,
                                responseContent.Headers.ContentLength,
                                toReceiveDownloadProgress,
                                cancellationToken);
                }
        }
}
