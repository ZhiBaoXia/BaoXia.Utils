using BaoXia.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoXia.Utils;

public class SearchKeyMatchProgressGetter
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static double GetMatchProgressWithSearchKey(
		string? searchKey,
		double defaultMatchProgress,
		string?[]? objectStrings,
		StringComparison comparisonType = StringComparison.OrdinalIgnoreCase,
		bool isMatchValueCharsOverlapEnable = false)
	{
		if (string.IsNullOrEmpty(searchKey))
		{
			return defaultMatchProgress;
		}
		if (objectStrings == null)
		{
			return defaultMatchProgress;
		}

		foreach (var objectString in objectStrings)
		{
			var matchProgress = objectString.GetMatchProgressValueOf(
				searchKey,
				comparisonType,
				isMatchValueCharsOverlapEnable);
			if (defaultMatchProgress < matchProgress)
			{
				defaultMatchProgress = matchProgress;
			}
		}
		return defaultMatchProgress;
	}

	public static double GetMatchProgressWithSearchKey(
		string? searchKey,
		double defaultMatchProgress,
	 	params string?[]? objectStrings)
	{
		return GetMatchProgressWithSearchKey(
			searchKey,
			defaultMatchProgress,
			objectStrings,
			StringComparison.OrdinalIgnoreCase,
			false);
	}

	public static double GetMatchProgressWithSearchKey(
		string? searchKey,
	 	params string?[]? objectStrings)
	{
		return GetMatchProgressWithSearchKey(
			searchKey,
			0.0,
			objectStrings,
			StringComparison.OrdinalIgnoreCase,
			false);
	}

	#endregion
}