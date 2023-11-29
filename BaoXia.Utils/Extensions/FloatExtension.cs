using BaoXia.Utils.Constants;
using System;

namespace BaoXia.Utils.Extensions
{
	public static class FloatExtension
	{
		public static float TryParse(string? floatString, float defaultValue = 0.0F)
		{
			if (float.TryParse(floatString, out var number))
			{
				return number;
			}
			return defaultValue;
		}

		/// <summary>
		/// 获取当前浮点数的小数位数。
		/// </summary>
		/// <param name="floatValue">指定的浮点数。</param>
		/// <returns>指定浮点数浮点数的小数位数。</returns>
		public static int GetDecimalDigits(this Single floatValue)
		{

			// 相关资料：
			// https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/standard-numeric-format-strings
			// https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/custom-numeric-format-strings
			// 默认 ( "G" ) 格式
			// “G”或“g”	常规	结果:更紧凑的定点表示法或科学记数法。
			// 受以下类型支持：所有数值类型。
			// 精度说明符：有效位数。
			// 默认值精度说明符：具体取决于数值类型。
			// 更多信息：常规（“G”）格式说明符。
			var floatValueString = floatValue.ToString("G");
			var dotIndex = floatValueString.IndexOf(".");
			var floatValueDecimalDigits = 0;
			if (dotIndex >= 0)
			{
				floatValueDecimalDigits = (floatValueString.Length - (dotIndex + 1));
			}
			return floatValueDecimalDigits;
		}


		public static float ToFloatWithDecimalPrecision(
			this float floatA,
			int decimalPrecision = -1)
		{
			return DoubleExtension
				.ToFloatWithDecimalPrecision(
				floatA,
				decimalPrecision);
		}

		public static double ToDoubleWithDecimalPrecision(
			this float floatA,
			int decimalPrecision = -1)
		{
			return DoubleExtension
				.ToDoubleWithDecimalPrecision(
				floatA,
				decimalPrecision);
		}

		public static int CompareTo(
			this float floatA,
			float floatB,
			int decimalPrecision,
			DecimalProcessType decimalProcessType = DecimalProcessType.Round,
			MidpointRounding midpointRounding = MidpointRounding.ToEven)
		{
			return DoubleExtension.CompareTo(
				floatA,
				floatB,
				decimalPrecision,
				decimalProcessType,
				midpointRounding);
		}
	}
}
