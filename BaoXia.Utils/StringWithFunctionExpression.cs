using BaoXia.Utils.Extensions;
using System;
using System.Collections.Generic;

namespace BaoXia.Utils
{
	/// <summary>
	/// 函数名称字符串。
	/// </summary>
	public static class StringWithFunctionExpression
	{
		public class FunctionExpressionInfo
		{
			////////////////////////////////////////////////
			// @静态常量
			////////////////////////////////////////////////

			#region 静态常量

			public const string? ParamsContainerBeginSymbolDefault = "(";

			public const string? ParamsContainerEndSymbolDefault = ")";

			public const string? ParamsSeparateDefault = ",";


			#endregion

			////////////////////////////////////////////////
			// @自身属性
			////////////////////////////////////////////////

			#region 自身属性

			public string Name { get; set; }

			public string? ParamsContainerBeginSymbol { get; set; }

			public string? ParamsContainerEndSymbol { get; set; }

			public string? ParamsSeparate { get; set; }

			public string[]? Params { get; set; }

			public int BeginIndexInOriginString { get; set; }

			public int EndIndexInOriginString { get; set; }

			#endregion


			////////////////////////////////////////////////
			// @自身实现
			////////////////////////////////////////////////

			#region 自身实现

			public FunctionExpressionInfo(
				string name,
				string? paramsContainerBeginSymbol = FunctionExpressionInfo.ParamsContainerBeginSymbolDefault,
				string? paramsContainerEndSymbol = FunctionExpressionInfo.ParamsContainerEndSymbolDefault,
				string? paramsSeparateDefault = FunctionExpressionInfo.ParamsSeparateDefault)
			{
				this.Name = name;
				this.ParamsContainerBeginSymbol = paramsContainerBeginSymbol;
				this.ParamsContainerEndSymbol = paramsContainerEndSymbol;
				this.ParamsSeparate = paramsSeparateDefault;
			}

			public FunctionExpressionInfo(
				FunctionExpressionInfo functionDefine,
				string[]? invokeParams,
				int beginCharIndexInOriginString,
				int endCharIndexInOriginString)
			{
				this.Name = functionDefine.Name;

				this.ParamsContainerBeginSymbol = functionDefine.ParamsContainerBeginSymbol;
				this.ParamsContainerEndSymbol = functionDefine.ParamsContainerEndSymbol;
				this.ParamsSeparate = functionDefine.ParamsSeparate;

				this.Params = invokeParams;

				this.BeginIndexInOriginString = beginCharIndexInOriginString;
				this.EndIndexInOriginString = endCharIndexInOriginString;
			}

			public string? GetFirstParam(bool isParamNeedTrim = true)
			{
				var functionExpressionParams = this.Params;
				if (functionExpressionParams == null
					|| functionExpressionParams.Length < 1)
				{
					return null;
				}

				var firstParam = functionExpressionParams[0];
				if (firstParam != null
					&& isParamNeedTrim)
				{
					firstParam = firstParam.Trim();
				}
				return firstParam;
			}

			public int GetFirstParamToInt()
			{
				var firstParam = this.GetFirstParam();
				_ = int.TryParse(firstParam, out var firstParamInInt);
				{ }
				return firstParamInInt;
			}

			public long GetFirstParamToLong()
			{
				var firstParam = this.GetFirstParam();
				_ = long.TryParse(firstParam, out var firstParamInLong);
				{ }
				return firstParamInLong;
			}

			public float GetFirstParamToFloat()
			{
				var firstParam = this.GetFirstParam();
				_ = float.TryParse(firstParam, out var firstParamInFloat);
				{ }
				return firstParamInFloat;
			}

			public double GetFirstParamToDouble()
			{
				var firstParam = this.GetFirstParam();
				_ = double.TryParse(firstParam, out var firstParamInDouble);
				{ }
				return firstParamInDouble;
			}

			#endregion
		}


		////////////////////////////////////////////////
		// @类方法
		////////////////////////////////////////////////

		#region 类方法

		public static List<FunctionExpressionInfo>? CreateFunctionExpressionsByParseString(
			string? functionExpressionString,
			ICollection<FunctionExpressionInfo>? functionDefines,
			StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
		{
			if (functionExpressionString == null
				|| functionExpressionString.Length < 1)
			{
				return null;
			}

			var functionExpressionStringLength = functionExpressionString.Length;
			var functionInvokeInfoOriginalStrings = new List<FunctionExpressionInfo>();
			if (functionDefines?.Count > 0)
			{
				var lastFunctionExpressionEndCharIndex = -1;
				while (lastFunctionExpressionEndCharIndex <= functionExpressionStringLength)
				{
					FunctionExpressionInfo? functionExpressionMatched = null;
					foreach (var functionDefine in functionDefines)
					{
						////////////////////////////////////////////////

						var functionName = functionDefine.Name;
						string[]? functionParams = null;
						var functionNameBeginCharIndex = lastFunctionExpressionEndCharIndex + 1;
						if (CharExtension.IsCharsOfStringEqualsKey(
							functionName,
							functionExpressionString,
							functionNameBeginCharIndex,
							out var functionNameCharsCountMatched,
							stringComparison))
						{
							var functionNameEndCharIndex
								= functionNameBeginCharIndex
								+ functionNameCharsCountMatched;

							var functionParamsContainerBeginSymbol
								= functionDefine.ParamsContainerBeginSymbol;
							var functionParamsContainerEndSymbol
							= functionDefine.ParamsContainerEndSymbol;

							var functionParamsContainerBeginSymbolBeginIndex
								= functionNameEndCharIndex;
							var functionParamsContainerBeginSymbolEndIndex
								= -1;
							var functionParamsContainerEndSymbolBeginIndex
								= -1;
							var functionParamsContainerEndSymbolEndIndex
								= -1;

							if (functionParamsContainerBeginSymbol?.Length > 0)
							{
								if (CharExtension.IsCharsOfStringEqualsKey(
									functionParamsContainerBeginSymbol,
									functionExpressionString,
									functionParamsContainerBeginSymbolBeginIndex,
									out var charsCountEqualed,
									stringComparison,
									true))
								{
									functionParamsContainerBeginSymbolEndIndex
										= functionParamsContainerBeginSymbolBeginIndex
										+ charsCountEqualed;
								}
							}
							else
							{
								functionParamsContainerBeginSymbolEndIndex
									= functionParamsContainerBeginSymbolBeginIndex;
							}
							if (functionParamsContainerBeginSymbolEndIndex >= 0)
							{
								if (functionParamsContainerEndSymbol?.Length > 0)
								{
									for (var charIndex = functionParamsContainerBeginSymbolEndIndex;
										charIndex < functionExpressionStringLength;
										charIndex++)
									{
										if (CharExtension.IsCharsOfStringEqualsKey(
											functionParamsContainerEndSymbol,
											functionExpressionString,
											charIndex,
											out var charsCountEqualed,
											stringComparison,
											true))
										{
											functionParamsContainerEndSymbolBeginIndex
												= charIndex;
											functionParamsContainerEndSymbolEndIndex
												= functionParamsContainerEndSymbolBeginIndex
												+ charsCountEqualed;
											//
											break;
											//
										}
									}
								}
								else
								{
									functionParamsContainerEndSymbolBeginIndex
										= functionExpressionStringLength;
									functionParamsContainerEndSymbolEndIndex
										= functionExpressionStringLength;
								}
							}

							////////////////////////////////////////////////
							////////////////////////////////////////////////
							////////////////////////////////////////////////

							// !!!⚠ 此时已找到对应的函数字符串 ⚠!!!
							if (functionParamsContainerBeginSymbolEndIndex >= 0)
							{
								if (functionParamsContainerEndSymbolEndIndex >= 0)
								{
									var functionParamsString
										= functionExpressionString[
											functionParamsContainerBeginSymbolEndIndex..functionParamsContainerEndSymbolBeginIndex];
									var functionParamsSeparate
										= functionDefine.ParamsSeparate;
									if (functionParamsSeparate?.Length > 0)
									{
										// !!!
										functionParams = functionParamsString.Split(functionParamsSeparate);
										// !!!
									}
									else
									{
										// !!!
										functionParams =
										[
											functionParamsString
										];
										// !!!
									}

									// !!!
									functionInvokeInfoOriginalStrings.Add(new(
										functionDefine,
										functionParams,
										functionNameBeginCharIndex,
										functionParamsContainerEndSymbolEndIndex));
									// !!!
								}
								else
								{
									throw new ArgumentException("函数参数描述未闭合，没有找到参数结束符。");
								}
							}
						}
						////////////////////////////////////////////////
					}
					if (functionExpressionMatched != null)
					{
						lastFunctionExpressionEndCharIndex = functionExpressionMatched.EndIndexInOriginString;
					}
					else
					{
						lastFunctionExpressionEndCharIndex++;
					}
				}
			}
			else
			{
				functionInvokeInfoOriginalStrings.Add(new(
					functionExpressionString,
					null,
					null));
			}
			return functionInvokeInfoOriginalStrings;
		}

		public static string? CreateStringByComputeFunctionExpression(
			string? functionExpressionString,
			ICollection<FunctionExpressionInfo>? functionDefines,
			Func<FunctionExpressionInfo, string?> toInvokeFunction,
			StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
		{
			if (functionExpressionString == null
				|| functionExpressionString.Length < 1)
			{
				return null;
			}

			var functionExpressions = StringWithFunctionExpression.CreateFunctionExpressionsByParseString(
				functionExpressionString,
				functionDefines,
				stringComparison);

			var finalString = string.Empty;
			var lastSubstringEndIndex = 0;
			if (functionExpressions != null)
			{
				foreach (var functionExpression in functionExpressions)
				{
					var functionExpressionBeginIndex = functionExpression.BeginIndexInOriginString;
					var functionExpressionEndIndex = functionExpression.EndIndexInOriginString;

					var substringBeginIndex = lastSubstringEndIndex;
					var substringEndIndex = functionExpressionBeginIndex;
					if (substringEndIndex > substringBeginIndex)
					{
						var substring = functionExpressionString[substringBeginIndex..substringEndIndex];
						if (substring?.Length > 0)
						{
							// !!!
							finalString += substring;
							// !!!
						}
					}

					var functionResult = toInvokeFunction(functionExpression);
					if (functionResult?.Length > 0)
					{
						// !!!
						finalString += functionResult;
						// !!!
					}

					// !!!
					lastSubstringEndIndex = functionExpressionEndIndex;
					// !!!
				}
			}
			if (lastSubstringEndIndex < functionExpressionString.Length)
			{
				// !!!
				finalString += functionExpressionString[lastSubstringEndIndex..];
				// !!!
			}
			return finalString;
		}

		#endregion

	}
}
