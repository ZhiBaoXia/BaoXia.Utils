using BaoXia.Utils.Extensions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BaoXia.Utils;

public class EnumerableUtil
{

	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static bool IsEmpty<EnumerableItemType>([NotNullWhen(false)] IEnumerable<EnumerableItemType>? enumerabler)
	{
		if (enumerabler?.IsNotEmpty() == true)
		{
			return false;
		}
		return true;
	}

	public static bool IsNotEmpty<EnumerableItemType>([NotNullWhen(true)] IEnumerable<EnumerableItemType>? enumerabler)
	{
		return !EnumerableUtil.IsEmpty(enumerabler);
	}

	#endregion
}
