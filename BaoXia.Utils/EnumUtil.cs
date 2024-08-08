using BaoXia.Utils.Extensions;
using System;
using System.Collections.Generic;

namespace BaoXia.Utils;

/// <summary>
/// 枚举类型扩展类。
/// </summary>
public class EnumUtil
{
	/// <summary>
	/// 获取指定枚举类型的所有值，值名称字典。
	/// </summary>
	/// <typeparam name="Enum">指定的枚举类型。</typeparam>
	/// <returns>指定枚举类型的所有值，值名称字典。</returns>
	public static Dictionary<T, string> ValueAndNamesOf<T>() where T : Enum
	{
		var allValueNames = new Dictionary<T, string>();
		foreach (var value
		    in
		    Enum.GetValues(typeof(T)))
		{
			var valueName = value.ToString();
			if (valueName?.Length > 0)
			{
				allValueNames.Add(
				(T)value,
				valueName);
			}
		}
		return allValueNames;
	}

	/// <summary>
	/// 获取指定枚举类型的所有值，值名称字典。
	/// </summary>
	/// <typeparam name="T">指定的枚举类型。</typeparam>
	/// <returns>指定枚举类型的所有值，值名称字典。</returns>
	public static List<string> NamesOf<T>() where T : Enum
	{
		var allValueNames = new List<string>();
		foreach (var value in Enum.GetValues(typeof(T)))
		{
			var valueName = value.ToString();
			if (valueName?.Length > 0)
			{
				allValueNames.Add(valueName);
			}
		}
		return allValueNames;
	}

	/// <summary>
	/// 获取枚举值对应的枚举值名称。
	/// </summary>
	/// <typeparam name="T">指定的枚举类型。</typeparam>
	/// <param name="enumValue">指定的枚举值。</param>
	/// <returns>指定枚举值对应的枚举值名称。</returns>
	public static string NameOf<T>(T enumValue) where T : Enum
	{
		return Enum.GetName(
			enumType: enumValue.GetType(),
			enumValue)!;
	}

	/// <summary>
	/// 获取指定字符串对应的枚举值。
	/// </summary>
	/// <typeparam name="T">指定的枚举类型</typeparam>
	/// <param name="valueName">指定的枚举值名称字符串。</param>
	/// <param name="defaultValue">匹配不到时返回的默认枚举值。</param>
	/// <returns>指定字符串对应的枚举值。</returns>
	public static T ValueOf<T>(
	    string? valueName,
	    T defaultValue,
	    bool isIgnoreCase = true)
	    where T : Enum
	{
		if (valueName == null
		    || valueName.Length < 1)
		{
			return defaultValue;
		}

		if (isIgnoreCase)
		{
			foreach (var value in Enum.GetValues(typeof(T)))
			{
				var enumValueName = value.ToString();
				if (string.Equals(
				    enumValueName,
				    valueName,
				    StringComparison.OrdinalIgnoreCase))
				{
					return (T)value;
				}
			}
		}
		else
		{
			foreach (var value in Enum.GetValues(typeof(T)))
			{
				var enumValueName = value.ToString();
				if (string.Equals(
				    enumValueName,
				    valueName,
				    StringComparison.Ordinal))
				{
					return (T)value;
				}
			}
		}
		return defaultValue;
	}
}
