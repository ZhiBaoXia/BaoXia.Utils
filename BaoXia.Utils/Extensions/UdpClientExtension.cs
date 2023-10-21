﻿using System;
using System.Net;
using System.Net.Sockets;

namespace BaoXia.Utils.Extensions
{
        public static class UdpClientExtension
        {

                ////////////////////////////////////////////////
                // @静态常量
                ////////////////////////////////////////////////

                #region 静态常量

                public class UdpClientListenInfo
                {
                        public UdpClient UdpClient { get; set; }

                        public IPEndPoint ListenIPEndPoint { get; set; }

                        public AsyncCallback ToReceiveMessage { get; set; }

                        public Func<UdpClientListenInfo?, bool>? ToWillContinueReceiveBytes { get; set; }

                        public UdpClientListenInfo(
                                UdpClient udpClient,
                                IPEndPoint listenIPEndPoint,
                                Action<UdpClient, IPEndPoint?, byte[]> toReceiveMessage,
                                Func<UdpClientListenInfo?, bool>? toWillContinueReceiveBytes)
                        {
                                UdpClient = udpClient;
                                ListenIPEndPoint = listenIPEndPoint;

                                ToReceiveMessage = new AsyncCallback((IAsyncResult receiveResult) =>
                                {
                                        if (receiveResult.AsyncState is not UdpClientListenInfo udpClientListenInfo)
                                        {
                                                return;
                                        }

                                        var remoteEndPoint = udpClientListenInfo.ListenIPEndPoint;
                                        var bytesReceived = udpClientListenInfo.UdpClient.EndReceive(
                                                receiveResult,
                                                ref remoteEndPoint);

                                        ////////////////////////////////////////////////
                                        // !!!
                                        toReceiveMessage(udpClientListenInfo.UdpClient, remoteEndPoint, bytesReceived);
                                        // !!!
                                        ////////////////////////////////////////////////

                                        var toWillContinueReceiveBytes = udpClientListenInfo.ToWillContinueReceiveBytes;
                                        if (toWillContinueReceiveBytes == null
                                        || toWillContinueReceiveBytes(udpClientListenInfo))
                                        {
                                                udpClientListenInfo.UdpClient.BeginReceive(
                                                        udpClientListenInfo.ToReceiveMessage,
                                                        udpClientListenInfo);
                                        }
                                });
                                ToWillContinueReceiveBytes = toWillContinueReceiveBytes;
                        }
                }

                #endregion


                ////////////////////////////////////////////////
                // @类方法
                ////////////////////////////////////////////////

                #region 类方法

                public static IAsyncResult StartReceiveBytesUtil(
                        this UdpClient udpClient,
                        Func<UdpClientListenInfo?, bool>? toWillContinueReceiveBytes,
                        IPEndPoint listenIPEndPoint,
                        Action<UdpClient, IPEndPoint?, byte[]> toReceiveMessage)
                {
                        var udpClientListenInfo = new UdpClientListenInfo(
                                udpClient,
                                listenIPEndPoint,
                                toReceiveMessage,
                                toWillContinueReceiveBytes);

                        var asyncResult = udpClient.BeginReceive(
                                udpClientListenInfo.ToReceiveMessage,
                                udpClientListenInfo);
                        {
                        }
                        return asyncResult;
                }

                #endregion
        }
}
