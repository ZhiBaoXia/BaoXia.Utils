using BaoXia.Utils.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading;

namespace BaoXia.Utils.Models;

public class ConnectionIpEndPoints
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	private readonly Lock _ipEndPointsLock = new();

	private List<IPEndPoint>? _ipEndPoints = null;

	private long _ipEndPointsUpdateTicks = 0;

	private HashSet<string>? _ipAddressStrings = null;

	private long _ipAddressStringsUpdateTicks = 0;

	private string? _firstIpEndPointAddress = null;
	private long _firstIpEndPointAddressUpdateTicks = 0;

	private string? _lastIpEndPointAddress = null;
	private long _lastIpEndPointAddressUpdateTicks = 0;

	private string? _keyIpEndPointAddress = null;
	private long _keyIpEndPointAddressUpdateTicks = 0;

	public List<IPEndPoint>? IpEndPoints
	{
		get
		{
			return _ipEndPoints;
		}
		set
		{
			lock (_ipEndPointsLock)
			{
				_ipEndPoints = value;
				_ipEndPointsUpdateTicks = DateTime.Now.Ticks;

				_ipAddressStrings = null;
				_ipAddressStringsUpdateTicks = 0;

				_firstIpEndPointAddress = null;
				_firstIpEndPointAddressUpdateTicks = 0;

				_lastIpEndPointAddress = null;
				_lastIpEndPointAddressUpdateTicks = 0;

				_keyIpEndPointAddress = null;
				_keyIpEndPointAddressUpdateTicks = 0;
			}
		}
	}

	public IPEndPoint? XRealIpEndPoint { get; set; }
	public IPEndPoint? XForwardedForIpEndPoint { get; set; }
	public IPEndPoint? BxGatewayPrevIpEndPoint { get; set; }
	public IPEndPoint? TcpIpRemoteIpEndPoint { get; set; }


	public HashSet<string>? IPAddressStrings
	{
		get
		{
			if (_ipAddressStringsUpdateTicks != _ipEndPointsUpdateTicks)
			{
				lock (_ipEndPointsLock)
				{
					if (_ipAddressStringsUpdateTicks != _ipEndPointsUpdateTicks)
					{
						HashSet<string>? ipAddressStrings = null;
						if (_ipEndPoints is { } ipEndPoints)
						{
							ipAddressStrings = [];
							foreach (var ipEndPoint in ipEndPoints)
							{
								ipAddressStrings.Add(ipEndPoint.Address.ToString());
							}
						}
						//
						_ipAddressStrings = ipAddressStrings;
						_ipAddressStringsUpdateTicks = _ipEndPointsUpdateTicks;
						//
					}
				}
			}
			return _ipAddressStrings;
		}
	}

	public IPEndPoint? FirstIpEndPoint
	{
		get
		{

			if (IpEndPoints is { } connectionIpEndPoints && connectionIpEndPoints.Count > 0)
			{
				return connectionIpEndPoints[0];
			}
			return null;
		}
	}

	public string? FirstIpEndPointAddress
	{
		get
		{
			if (_firstIpEndPointAddressUpdateTicks != _ipEndPointsUpdateTicks)
			{
				lock (_ipEndPointsLock)
				{
					if (_firstIpEndPointAddressUpdateTicks != _ipEndPointsUpdateTicks)
					{
						_firstIpEndPointAddress = FirstIpEndPoint?.Address.ToString();
						_firstIpEndPointAddressUpdateTicks = _ipEndPointsUpdateTicks;
					}
				}
			}
			return _firstIpEndPointAddress;
		}
	}

	public IPEndPoint? LastIpEndPoint
	{
		get
		{
			if (IpEndPoints is { } connectionIpEndPoints && connectionIpEndPoints.Count > 0)
			{
				return connectionIpEndPoints[^1];
			}
			return null;
		}
	}

	public string? LastIpEndPointAddress
	{
		get
		{
			if (_lastIpEndPointAddressUpdateTicks != _ipEndPointsUpdateTicks)
			{
				lock (_ipEndPointsLock)
				{
					if (_lastIpEndPointAddressUpdateTicks != _ipEndPointsUpdateTicks)
					{
						_lastIpEndPointAddress = LastIpEndPoint?.Address.ToString();
						_lastIpEndPointAddressUpdateTicks = _ipEndPointsUpdateTicks;
					}
				}
			}
			return _lastIpEndPointAddress;
		}
	}

	public IPEndPoint? KeyIpEndPoint
	{
		get
		{
			if (XRealIpEndPoint != null)
			{
				return XRealIpEndPoint;
			}
			if (XForwardedForIpEndPoint != null)
			{
				return XForwardedForIpEndPoint;
			}
			if (BxGatewayPrevIpEndPoint != null)
			{
				return BxGatewayPrevIpEndPoint;
			}
			return TcpIpRemoteIpEndPoint;
		}
	}

	public string? KeyIpEndPointAddress
	{
		get
		{
			if (_keyIpEndPointAddressUpdateTicks != _ipEndPointsUpdateTicks)
			{
				lock (_ipEndPointsLock)
				{
					if (_keyIpEndPointAddressUpdateTicks != _ipEndPointsUpdateTicks)
					{
						_keyIpEndPointAddress = KeyIpEndPoint?.Address.ToString();
						_keyIpEndPointAddressUpdateTicks = _ipEndPointsUpdateTicks;
					}
				}
			}
			return _keyIpEndPointAddress;
		}
	}


	#endregion


	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static bool TryParse(string? connectionIpEndPointsString, [NotNullWhen(true)] out ConnectionIpEndPoints? connectionIpEndPoints)
	{
		//
		connectionIpEndPoints = null;
		//
		if (string.IsNullOrEmpty(connectionIpEndPointsString))
		{
			return false;
		}

		var connectionIPEndPointStrings = connectionIpEndPointsString.Split(
			ConnectionIPEndPointConstants.ConnectionIpEndPointsSparator,
			StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		if (connectionIPEndPointStrings == null || connectionIPEndPointStrings.Length < 1)
		{
			return false;
		}

		List<IPEndPoint>? ipEndPoints = null;
		foreach (var connectionIPEndPointString in connectionIPEndPointStrings)
		{
			if (!IPEndPoint.TryParse(connectionIPEndPointString, out var ipEndPoint))
			{
				continue;
			}

			ipEndPoints ??= [];
			ipEndPoints.Add(ipEndPoint);
		}
		if (ipEndPoints == null)
		{
			return false;
		}

		connectionIpEndPoints = new ConnectionIpEndPoints(ipEndPoints, null, null, null, null);
		{ }
		return connectionIpEndPoints != null;
	}

	public static bool TryParseToFirstIpEndPoint(string? connectionIpEndPointsString, [NotNullWhen(true)] out IPEndPoint? firstIpEndPoint)
	{
		//
		firstIpEndPoint = null;
		//
		if (string.IsNullOrEmpty(connectionIpEndPointsString))
		{
			return false;
		}

		var connectionIPEndPointStrings = connectionIpEndPointsString.Split(
			ConnectionIPEndPointConstants.ConnectionIpEndPointsSparator,
			StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		if (connectionIPEndPointStrings == null || connectionIPEndPointStrings.Length < 1)
		{
			return false;
		}

		var connectionIPEndPointString = connectionIPEndPointStrings[0];
		if (IPEndPoint.TryParse(connectionIPEndPointString, out firstIpEndPoint))
		{
			return true;
		}
		return false;
	}

	public static bool TryParseToLastIpEndPoint(string? connectionIpEndPointsString, [NotNullWhen(true)] out IPEndPoint? lastIpEndPoint)
	{
		//
		lastIpEndPoint = null;
		//
		if (string.IsNullOrEmpty(connectionIpEndPointsString))
		{
			return false;
		}

		var connectionIPEndPointStrings = connectionIpEndPointsString.Split(
			ConnectionIPEndPointConstants.ConnectionIpEndPointsSparator,
			StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		if (connectionIPEndPointStrings == null || connectionIPEndPointStrings.Length < 1)
		{
			return false;
		}

		var connectionIPEndPointString = connectionIPEndPointStrings[^1];
		if (IPEndPoint.TryParse(connectionIPEndPointString, out lastIpEndPoint))
		{
			return true;
		}
		return false;
	}

	#endregion


	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public ConnectionIpEndPoints(List<IPEndPoint>? ipEndPoints, IPEndPoint? xRealIpEndPoint, IPEndPoint? xForwardedForIpEndPoint,
		IPEndPoint? bxGatewayPrevIpEndPoint, IPEndPoint? tcpIpRemoteIpEndPoint)
	{
		IpEndPoints = ipEndPoints;

		XRealIpEndPoint = xRealIpEndPoint;
		XForwardedForIpEndPoint = xForwardedForIpEndPoint;
		BxGatewayPrevIpEndPoint = bxGatewayPrevIpEndPoint;
		TcpIpRemoteIpEndPoint = tcpIpRemoteIpEndPoint;
	}

	public bool Contains(string ipAddress)
	{
		if (IPAddressStrings?.Contains(ipAddress) == true)
		{
			return true;
		}
		return false;
	}

	public bool ContainsAny(IEnumerable<string> ipAddresses)
	{
		foreach (var ipAddress in ipAddresses)
		{
			if (Contains(ipAddress))
			{
				return true;
			}
		}
		return false;
	}

	#endregion
}