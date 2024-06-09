using System;

namespace BaoXia.Utils.Extensions;

public static class EnumExtension
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	/// <summary>
	/// 获取枚举值对应的枚举值名称。
	/// </summary>
	/// <typeparam name="T">指定的枚举类型。</typeparam>
	/// <param name="enumValue">指定的枚举值。</param>
	/// <returns>指定枚举值对应的枚举值名称。</returns>
	public static string? Name(this Enum enumValue)
	{
		var enumValueType
		    = Enum.Parse(
			enumValue.GetType(),
			enumValue.ToString());
		{ }
		return enumValueType.ToString();
	}

	#endregion
}
