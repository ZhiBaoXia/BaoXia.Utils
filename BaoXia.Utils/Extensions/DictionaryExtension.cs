using System.Collections.Generic;

namespace BaoXia.Utils.Extensions;

public static class DictionaryExtension
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	/// <summary>
	/// 添加或设置键值对。
	/// 【⚠注意】当前扩展方法只适用于“Dictionary”类，所有操作都不是线程安全的。
	/// 线上遇到过“ConcurrentDictionary”调用本方法，并发操作时，触发了“Duplicate Keys”的异常。
	/// </summary>
	/// <typeparam name="KeyType">健字段的数据类型。</typeparam>
	/// <typeparam name="ValueType">值字段的数据类型。</typeparam>
	/// <param name="dictionary">当前字典对象。</param>
	/// <param name="key">指定的健字段。</param>
	/// <param name="value">指定的值字段。</param>
	public static void AddOrSet<KeyType, ValueType>(
		this IDictionary<KeyType, ValueType> dictionary,
		KeyType key,
		ValueType value) where KeyType : notnull
	{
		if (dictionary.TryAdd(key, value) != true)
		{
			dictionary.Remove(key);
			dictionary.Add(key, value);
		}
	}

	public static string ToUriQuery(
		this IDictionary<string, string?> queryParams)
	{
		string uriQuery = "";
		foreach (var kvp in queryParams)
		{
			var key = kvp.Key;
			if (key.Length < 1)
			{
				continue;
			}

			var queryItem
				= key.StringByEncodeInUriParam()
				+ "=";
			var value = kvp.Value;
			if (value?.Length > 0)
			{
				queryItem += value.StringByEncodeInUriParam();
			}

			if (uriQuery == null)
			{
				uriQuery = key;
			}
			else
			{
				uriQuery += "&" + queryItem;
			}
		}
		return uriQuery;
	}

	#endregion
}
