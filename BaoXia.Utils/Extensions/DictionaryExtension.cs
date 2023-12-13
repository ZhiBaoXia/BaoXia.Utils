using System.Collections.Generic;

namespace BaoXia.Utils.Extensions;

public static class DictionaryExtension
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法


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
