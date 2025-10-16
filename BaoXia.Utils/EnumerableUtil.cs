using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BaoXia.Utils;

public class EnumerableUtil
{

	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static bool IsEmpty<EnumerableItemType>([NotNullWhen(false)] IEnumerable<EnumerableItemType>? enumerabler)
	{
		if (enumerabler?.Any() == true)
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
