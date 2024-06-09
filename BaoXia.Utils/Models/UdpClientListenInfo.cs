using System;
using System.Net;
using System.Net.Sockets;

namespace BaoXia.Utils.Models;

public class UdpClientListenInfo(
	UdpClient udpClient,
	IPEndPoint listenIPEndPoint,
	Func<UdpClient?, IPEndPoint?, byte[]?, Exception?, bool> toReceiveMessage,
	Func<UdpClientListenInfo?, bool>? toWillContinueReceiveBytes)
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public UdpClient UdpClient { get; set; } = udpClient;

	public IPEndPoint ListenIPEndPoint { get; set; } = listenIPEndPoint;

	public AsyncCallback ToReceiveMessage { get; set; } = new AsyncCallback((IAsyncResult receiveResult) =>
	{
		UdpClientListenInfo? currentUdpClientListenInfo = null;
		Exception? exceptionOfGetCurrentUdpClientListenInfo = null;
		try
		{
			if (receiveResult.AsyncState is not UdpClientListenInfo udpClientListenInfo)
			{
				return;
			}
			// !!!
			currentUdpClientListenInfo = udpClientListenInfo;
			// !!!
		}
		catch (Exception exception)
		{
			exceptionOfGetCurrentUdpClientListenInfo = exception;
		}


		IPEndPoint? remoteEndPoint = null;
		byte[]? bytesReceived = null;
		Exception? exceptionOfReceiveBytes = null;
		try
		{
			if (currentUdpClientListenInfo != null)
			{
				remoteEndPoint = currentUdpClientListenInfo.ListenIPEndPoint;
				bytesReceived = currentUdpClientListenInfo.UdpClient.EndReceive(
					receiveResult,
					ref remoteEndPoint);
			}
		}
		catch (Exception exception)
		{
			exceptionOfReceiveBytes = exception;
		}


		var isBeginReceiveAgain = true;
		Exception? exceptionOfToReceiveMessage = null;
		try
		{
			if (exceptionOfGetCurrentUdpClientListenInfo != null)
			{
				////////////////////////////////////////////////
				// !!!
				isBeginReceiveAgain = toReceiveMessage(
					null,
					null,
					null,
					exceptionOfGetCurrentUdpClientListenInfo);
				// !!!
				////////////////////////////////////////////////
			}
			else if (currentUdpClientListenInfo != null)
			{
				////////////////////////////////////////////////
				// !!!
				isBeginReceiveAgain = toReceiveMessage(
					currentUdpClientListenInfo.UdpClient,
					remoteEndPoint,
					bytesReceived,
					exceptionOfReceiveBytes);
				// !!!
				////////////////////////////////////////////////
			}
			else
			{
				////////////////////////////////////////////////
				// !!!
				isBeginReceiveAgain = toReceiveMessage(
					null,
					remoteEndPoint,
					bytesReceived,
					exceptionOfReceiveBytes
					?? exceptionOfGetCurrentUdpClientListenInfo);
				// !!!
				////////////////////////////////////////////////
			}
		}
		catch (Exception exception)
		{
			exceptionOfToReceiveMessage = exception;
		}

		if (!isBeginReceiveAgain)
		{
			return;
		}
		if (currentUdpClientListenInfo == null)
		{
			return;
		}

		var toWillContinueReceiveBytes = currentUdpClientListenInfo.ToWillContinueReceiveBytes;
		if (toWillContinueReceiveBytes == null
		|| toWillContinueReceiveBytes(currentUdpClientListenInfo))
		{
			currentUdpClientListenInfo.UdpClient.BeginReceive(
				currentUdpClientListenInfo.ToReceiveMessage,
				currentUdpClientListenInfo);
		}
	});

	public Func<UdpClientListenInfo?, bool>? ToWillContinueReceiveBytes { get; set; } = toWillContinueReceiveBytes;

	#endregion
}