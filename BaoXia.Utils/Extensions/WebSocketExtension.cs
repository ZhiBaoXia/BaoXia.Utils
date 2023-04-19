using BaoXia.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Extensions
{
        public static class WebSocketExtension
        {
                public static bool IsConnectionEnable(this WebSocket socket)
                {
                        switch (socket.State)
                        {
                                case WebSocketState.None:
                                case WebSocketState.CloseSent:
                                case WebSocketState.CloseReceived:
                                case WebSocketState.Closed:
                                case WebSocketState.Aborted:
                                        { }
                                        break;
                                case WebSocketState.Connecting:
                                case WebSocketState.Open:
                                        {
                                                return true;
                                        }
                        }
                        return false;
                }

                public static async Task<WebSocketReceiveDataResult> ReceiveByteArrayAsync(
                        this WebSocket webSocket,
                        BytesBuffer? receiveBuffer,
                        CancellationToken cancellationToken,
                        WebSocketReceiveDataResult? receiveDataResultSpecified = null)
                {
                        if (receiveBuffer == null)
                        {
                                receiveBuffer = new BytesBuffer();
                        }
                        else
                        {
                                receiveBuffer.Clear();
                        }

                        WebSocketReceiveResult receiveResult;
                        do
                        {
                                var requestBodyBufferBytes
                                        = receiveBuffer.GetEmptyBufferSegment();
                                receiveResult
                                        = await webSocket.ReceiveAsync(
                                                requestBodyBufferBytes,
                                                cancellationToken);
                                // !!!
                                receiveBuffer.BytesCount += receiveResult.Count;
                                // !!!
                        } while (receiveResult.EndOfMessage == false);

                        if (receiveDataResultSpecified == null)
                        {
                                receiveDataResultSpecified = new WebSocketReceiveDataResult();
                        }
                        ////////////////////////////////////////////////
                        // !!! 
                        receiveDataResultSpecified.Result = receiveResult;
                        receiveDataResultSpecified.BytesReceived = receiveBuffer.ToBytes();
                        // !!!
                        ////////////////////////////////////////////////
                        return receiveDataResultSpecified;
                }

                public static async Task<WebSocketReceiveDataResult> ReceiveStringAsync(
                        this WebSocket webSocket,
                        BytesBuffer? receiveBuffer,
                        CancellationToken cancellationToken,
                        System.Text.Encoding? textEncoding = null,
                        WebSocketReceiveDataResult? receiveDataResultSpecified = null)
                {
                        var receiveResult = await WebSocketExtension.ReceiveByteArrayAsync(
                                webSocket,
                                receiveBuffer,
                                cancellationToken,
                                receiveDataResultSpecified);
                        var bytesReceived = receiveResult.BytesReceived;
                        if (bytesReceived?.Length > 0)
                        {
                                if (textEncoding == null)
                                {
                                        textEncoding = System.Text.Encoding.UTF8;
                                }
                                // !!!
                                receiveResult.StringReceived = textEncoding.GetString(bytesReceived.ToArray());
                                // !!!
                        }
                        return receiveResult;
                }

                public static async Task<WebSocketReceiveDataResult> ReceiveJsonDocumentAsync(
                        this WebSocket webSocket,
                        BytesBuffer? receiveBuffer,
                        CancellationToken cancellationToken,
                        System.Text.Encoding? textEncoding = null,
                        JsonDocumentOptions jsonDeserializeOptions = default,
                        WebSocketReceiveDataResult? receiveDataResultSpecified = null)
                {
                        var receiveResult = await WebSocketExtension.ReceiveStringAsync(
                                webSocket,
                                receiveBuffer,
                                cancellationToken,
                                textEncoding,
                                receiveDataResultSpecified);
                        var stringReceived = receiveResult.StringReceived;
                        if (stringReceived?.Length > 0)
                        {
                                var jsonDocumentReceived = JsonDocument.Parse(stringReceived, jsonDeserializeOptions);
                                {
                                        // !!!
                                        receiveResult.JsonDocumentReceived = jsonDocumentReceived;
                                        // !!!
                                }
                        }
                        return receiveResult;
                }

                public static async Task<WebSocketReceiveObjectResult<ObjectType>> ReceiveObjectAsync<ObjectType>(
                        this WebSocket webSocket,
                        BytesBuffer? receiveBuffer,
                        CancellationToken cancellationToken,
                        System.Text.Encoding? textEncoding = null)
                {
                        var receiveObjectResult = new WebSocketReceiveObjectResult<ObjectType>();
                        var receiveResult = await WebSocketExtension.ReceiveStringAsync(
                                webSocket,
                                receiveBuffer,
                                cancellationToken,
                                textEncoding,
                                receiveObjectResult);
                        if (receiveResult != receiveObjectResult)
                        {
                                throw new ApplicationException("“receiveResult”应当为指定的“receiveObjectResult”对象。");
                        }
                        var stringReceived = receiveResult.StringReceived;
                        if (stringReceived?.Length > 0)
                        {
                                // !!!
                                receiveObjectResult.ObjectReceived = stringReceived.ToObjectByJsonDeserialize<ObjectType>();
                                // !!!
                        }
                        return receiveObjectResult;
                }

                public static async IAsyncEnumerable<WebSocketReceiveDataResult?> ReceiveByteArraysAsync(
                        this WebSocket webSocket,
                        BytesBuffer? receiveBuffer,
                        [EnumeratorCancellation] CancellationToken cancellationToken)
                {
                        if (receiveBuffer == null)
                        {
                                yield break;
                        }

                        WebSocketReceiveDataResult? receiveDataResult = null;
                        while (receiveDataResult?.Result?.CloseStatus == null)
                        {
                                receiveDataResult
                                        = await WebSocketExtension.ReceiveByteArrayAsync(
                                                webSocket,
                                                receiveBuffer,
                                                cancellationToken);
                                // !!!
                                yield return receiveDataResult;
                                // !!!
                        }
                }

                public static async IAsyncEnumerable<WebSocketReceiveDataResult?> ReceiveStringsAsync(
                        this WebSocket webSocket,
                        BytesBuffer? receiveBuffer,
                        [EnumeratorCancellation] CancellationToken cancellationToken,
                        System.Text.Encoding? textEncoding = null)
                {
                        if (receiveBuffer == null)
                        {
                                yield break;
                        }

                        WebSocketReceiveDataResult? receiveDataResult = null;
                        while (receiveDataResult?.Result?.CloseStatus == null)
                        {
                                receiveDataResult
                                        = await WebSocketExtension.ReceiveStringAsync(
                                                webSocket,
                                                receiveBuffer,
                                                cancellationToken);
                                // !!!
                                yield return receiveDataResult;
                                // !!!
                        }
                }

                public static async IAsyncEnumerable<WebSocketReceiveDataResult?> ReceiveJsonDocumentsAsync(
                        this WebSocket webSocket,
                        BytesBuffer? receiveBuffer,
                        [EnumeratorCancellation] CancellationToken cancellationToken,
                        System.Text.Encoding? textEncoding = null,
                        JsonDocumentOptions jsonDeserializeOptions = default)
                {
                        if (receiveBuffer == null)
                        {
                                yield break;
                        }

                        WebSocketReceiveDataResult? receiveDataResult = null;
                        while (receiveDataResult?.Result?.CloseStatus == null)
                        {
                                receiveDataResult
                                        = await WebSocketExtension.ReceiveJsonDocumentAsync(
                                                webSocket,
                                                receiveBuffer,
                                                cancellationToken,
                                                textEncoding,
                                                jsonDeserializeOptions);
                                // !!!
                                yield return receiveDataResult;
                                // !!!
                        }
                }

                public static async IAsyncEnumerable<WebSocketReceiveObjectResult<ObjectType>?> ReceiveObjectsAsync<ObjectType>(
                        this WebSocket webSocket,
                        BytesBuffer? receiveBuffer,
                        [EnumeratorCancellation] CancellationToken cancellationToken,
                        System.Text.Encoding? textEncoding = null)
                {
                        if (receiveBuffer == null)
                        {
                                yield break;
                        }

                        WebSocketReceiveObjectResult<ObjectType>? receiveObjectResult = null;
                        while (receiveObjectResult?.Result?.CloseStatus == null)
                        {
                                receiveObjectResult
                                        = await WebSocketExtension.ReceiveObjectAsync<ObjectType>(
                                                webSocket,
                                                receiveBuffer,
                                                cancellationToken,
                                                textEncoding);
                                // !!!
                                yield return receiveObjectResult;
                                // !!!
                        }
                }


                public static async Task<bool> TryToCloseAsync(
                        this WebSocket webSocket,
                        WebSocketCloseStatus closeStatus,
                        string? statusDescription,
                        CancellationToken cancellationToken)
                {
                        switch (webSocket.State)
                        {
                                case WebSocketState.None:
                                case WebSocketState.Closed:
                                case WebSocketState.Aborted:
                                case WebSocketState.CloseSent:
                                        {
                                                return false;
                                        }
                                case WebSocketState.CloseReceived:
                                        {
                                                await webSocket.CloseOutputAsync(
                                                        closeStatus,
                                                        statusDescription,
                                                        cancellationToken);
                                        }
                                        break;
                                default:
                                case WebSocketState.Connecting:
                                case WebSocketState.Open:
                                        {
                                                await webSocket.CloseAsync(
                                                        closeStatus,
                                                        statusDescription,
                                                        cancellationToken);
                                        }
                                        break;
                        }
                        return true;
                }
        }
}
