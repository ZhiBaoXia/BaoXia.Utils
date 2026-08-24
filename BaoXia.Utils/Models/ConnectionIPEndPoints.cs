using BaoXia.Utils.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace BaoXia.Utils.Models;

public class ConnectionIPEndPoints
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	private List<IPEndPoint>? _ipEndPoints = null;

	private long _ipEndPointsUpdateTicks = 0;

	private HashSet<string>? _ipAddressStrings = null;

	private long _ipAddressStringsUpdateTicks = 0;

	private string? _firstIPEndPointAddress = null;
	private long _firstIPEndPointAddressUpdateTicks = 0;

	private string? _lastIPEndPointAddress = null;
	private long _lastIPEndPointAddressUpdateTicks = 0;


	public List<IPEndPoint>? IPEndPoints
	{
		get
		{
			return _ipEndPoints;
		}
		set
		{
			lock (this)
			{
				_ipEndPoints = value;
				_ipEndPointsUpdateTicks = DateTime.Now.Ticks;

				_ipAddressStrings = null;
				_ipAddressStringsUpdateTicks = 0;

				_firstIPEndPointAddress = null;
				_firstIPEndPointAddressUpdateTicks = 0;

				_lastIPEndPointAddress = null;
				_lastIPEndPointAddressUpdateTicks = 0;
			}
		}
	}


	public HashSet<string>? IPAddressStrings
	{
		get
		{
			if (_ipAddressStringsUpdateTicks != _ipEndPointsUpdateTicks)
			{
				lock (this)
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

	public IPEndPoint? FirstIPEndPoint
	{
		get
		{

			if (IPEndPoints is { } connectionIPEndPoints && connectionIPEndPoints.Count > 0)
			{
				return connectionIPEndPoints[0];
			}
			return null;
		}
	}

	public string? FirstIPEndPointAddress
	{
		get
		{
			if (_firstIPEndPointAddressUpdateTicks != _ipEndPointsUpdateTicks)
			{
				lock (this)
				{
					if (_firstIPEndPointAddressUpdateTicks != _ipEndPointsUpdateTicks)
					{
						_firstIPEndPointAddress = FirstIPEndPoint?.Address.ToString();
						_firstIPEndPointAddressUpdateTicks = _ipEndPointsUpdateTicks;
					}
				}
			}
			return _firstIPEndPointAddress;
		}
	}

	public IPEndPoint? LastIPEndPoint
	{
		get
		{
			if (IPEndPoints is { } connectionIPEndPoints && connectionIPEndPoints.Count > 0)
			{
				return connectionIPEndPoints[^1];
			}
			return null;
		}
	}

	public string? LastIPEndPointAddress
	{
		get
		{
			if (_lastIPEndPointAddressUpdateTicks != _ipEndPointsUpdateTicks)
			{
				lock (this)
				{
					if (_lastIPEndPointAddressUpdateTicks != _ipEndPointsUpdateTicks)
					{
						_lastIPEndPointAddress = LastIPEndPoint?.Address.ToString();
						_lastIPEndPointAddressUpdateTicks = _ipEndPointsUpdateTicks;
					}
				}
			}
			return _lastIPEndPointAddress;
		}
	}

	public IPEndPoint? BxGatewayPrevIPEndPoint { get; set; }

	#endregion


	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static bool TryParse(string? connectionIPEndPointsString, [NotNullWhen(true)] out ConnectionIPEndPoints? connectionIPEndPoints)
	{
		//
		connectionIPEndPoints = null;
		//
		if (string.IsNullOrEmpty(connectionIPEndPointsString))
		{
			return false;
		}

		var connectionIPEndPointStrings = connectionIPEndPointsString.Split(
			ConnectionIPEndPointConstants.ConnectionIPEndPointsSparator,
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

		connectionIPEndPoints = new ConnectionIPEndPoints(ipEndPoints);
		{ }
		return connectionIPEndPoints != null;
	}

	#endregion



	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public ConnectionIPEndPoints()
	{ }

	public ConnectionIPEndPoints(List<IPEndPoint>? ipEndPoints)
	{
		IPEndPoints = ipEndPoints;
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