using BaoXia.Utils.Constants;
using BaoXia.Utils.Extensions;
using BaoXia.Utils.Models;

namespace BaoXia.Utils;

public class StringSafeUtil
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法


	public static StringsSafeCheckResult? CheckStrings(params (string stringName, string? stringValue)[] stringValues)
	{
		if (stringValues.IsEmpty())
		{
			return null;
		}

		foreach (var (stringName, stringValue) in stringValues)
		{
			if (stringValue.IsHackingStringBySqlInjection())
			{
				return new()
				{
					UnsafeStringName = stringName,
					UnsafeType = StringUnsafeType.HackingBySqlInjection,
					UnsafeDescription = $"“{stringName}”字段含有危险的“Sql注入代码”。"
				};
			}
		}
		return null;
	}

	#endregion
}