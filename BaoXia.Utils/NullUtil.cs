using System;
using System.Collections.Generic;

namespace BaoXia.Utils;

public class NullUtil
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static string EmptyOr(string? stringValue)
	{
		return stringValue ?? string.Empty;
	}

	public static DateTime EmptyOr(DateTime? dateTime)
	{
		return dateTime ?? DateTime.MinValue;
	}

	public static ItemType[] EmptyOr<ItemType>(ItemType[]? arrayValue)
	{
		return arrayValue ?? [];
	}

	public static List<ItemType> EmptyOr<ItemType>(List<ItemType>? listValue)
	{
		return listValue ?? [];
	}

	public static uint ZeroOr(uint? numberValue)
	{
		return numberValue ?? 0;
	}

	public static int ZeroOr(int? numberValue)
	{
		return numberValue ?? 0;
	}

	public static ulong ZeroOr(ulong? numberValue)
	{
		return numberValue ?? 0;
	}

	public static long ZeroOr(long? numberValue)
	{
		return numberValue ?? 0;
	}

	public static float ZeroOr(float? numberValue)
	{
		return numberValue ?? 0;
	}

	public static double ZeroOr(double? numberValue)
	{
		return numberValue ?? 0;
	}

	public static decimal ZeroOr(decimal? numberValue)
	{
		return numberValue ?? 0;
	}

	#endregion
}