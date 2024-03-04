using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoXia.Utils.Extensions;

public static class StringExtension_DefaultValue
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static string WithDefault(
		this string? str,
		string defaultString = "[无]",
		bool isStringNullToDefault = true,
		bool isStringEmptyToDefault = true)
	{
		if (str == null)
		{
			if (isStringNullToDefault)
			{
				return defaultString;
			}
		}
		else if (str.Length < 1)
		{
			if (isStringEmptyToDefault)
			{
				return defaultString;
			}
		}
		return str ?? string.Empty;
	}

	#endregion
}