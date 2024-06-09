using BaoXia.Utils.Models;
using System;
using System.Net;
using System.Net.Sockets;

namespace BaoXia.Utils.Extensions;

public static class UdpClientExtension
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static IAsyncResult StartReceiveBytesUtil(
		this UdpClient udpClient,
		Func<UdpClientListenInfo?, bool>? toWillContinueReceiveBytes,
		IPEndPoint listenIPEndPoint,
		Func<UdpClient?, IPEndPoint?, byte[]?, Exception?, bool> toReceiveMessage)
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
