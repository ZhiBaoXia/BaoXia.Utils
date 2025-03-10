using BaoXia.Utils.Collections;
using BaoXia.Utils.Extensions;
using System;
using System.Collections.Generic;

namespace BaoXia.Utils.MathTools
{
	/// <summary>
	/// 用于解析数学表达式。
	/// </summary>
	public class MathExpression
	{
		////////////////////////////////////////////////
		// @静态常量
		////////////////////////////////////////////////

		#region 静态常量

		public enum OperatorId
		{
			Unknown,

			SectionBegin,
			SectionEnd,

			Number,
			Variable,
			Function,

			Plus,
			Minus,
			And,
			Or,
			Multiply,
			Divide,
			Modulo,
			Power,
			//
			LessThan,
			LessThanOrEqual,
			Equal,
			NotEqual,
			GreaterThanOrEqual,
			GreaterThan
		}

		public enum OperatorType
		{
			Section,

			CalculationItem,

			CalculationOperator
		}

		public const string FunctionParamSpliter = ",";

		public class Operator
		{
			public OperatorType Type { get; set; }

			public OperatorId Id { get; set; }

			public string Keyword { get; set; } = String.Empty;

			public double Number { get; set; }

			public string? FunctionName { get; set; }

			public string[]? FunctionParams { get; set; }

			public string? VariableName { get; set; }

			public int RankLevel { get; set; }

			public Operator(
				OperatorType type,
				OperatorId Id,
				string keyword,
				int rankLevel)
			{
				this.Type = type;
				this.Id = Id;
				this.Keyword = keyword;
				this.RankLevel = rankLevel;
			}

			public Operator(
				OperatorType type,
				OperatorId Id,
				int rankLevel)
			{
				this.Type = type;
				this.Id = Id;
				this.RankLevel = rankLevel;
			}

			public Operator(
				OperatorType type,
				OperatorId Id)
			{
				this.Type = type;
				this.Id = Id;
			}

			public Operator(
				double number)
			{
				this.Type = OperatorType.CalculationItem;
				this.Id = OperatorId.Number;
				this.Number = number;
			}
			public Operator(
				string variableName)
			{
				this.Type = OperatorType.CalculationItem;
				this.Id = OperatorId.Variable;
				this.VariableName = variableName;
			}

			public Operator(
				string functionName,
				string[]? functionParams)
			{
				this.Type = OperatorType.CalculationItem;
				this.Id = OperatorId.Function;
				this.FunctionName = functionName;
				this.FunctionParams = functionParams;
			}
		}

		public class CalculationNumber
		{
			public string? Name { get; set; }

			public double? Number { get; set; }


			public CalculationNumber(
				string name,
				double number)
			{
				this.Name = name;
				this.Number = number;
			}

			public CalculationNumber(string name)
			{
				this.Name = name;
			}

			public CalculationNumber(double number)
			{
				this.Number = number;
			}
		}

		public class CalculationUnit : LinkedItem<CalculationUnit>
		{
			public int CalculationDepth { get; set; }

			public CalculationNumber? LeftNumber { get; set; }

			public Operator? CalculationOperator { get; set; }


			public CalculationUnit()
			{ }

			public CalculationUnit(
				int calculationDepth,
				CalculationNumber leftNumber)
			{
				this.CalculationDepth = calculationDepth;

				this.LeftNumber = leftNumber;
			}

			public CalculationUnit(
				int calculationDepth,
				Operator calculationOperator)
			{
				this.CalculationDepth = calculationDepth;
				this.CalculationOperator = calculationOperator;
			}
		}

		////////////////////////////////////////////////
		// 比较操作符，优先级最低，为“0-99”：
		////////////////////////////////////////////////
		public static readonly Operator LessThan = new(OperatorType.CalculationOperator, OperatorId.LessThan, "<", 0);
		public static readonly Operator LessThanOrEqual = new(OperatorType.CalculationOperator, OperatorId.LessThanOrEqual, "<=", 0);
		public static readonly Operator GreaterThanOrEqual = new(OperatorType.CalculationOperator, OperatorId.GreaterThanOrEqual, ">=", 0);
		public static readonly Operator GreaterThan = new(OperatorType.CalculationOperator, OperatorId.GreaterThan, ">", 0);
		public static readonly Operator Equal = new(OperatorType.CalculationOperator, OperatorId.Equal, "==", 0);
		public static readonly Operator NotEqual = new(OperatorType.CalculationOperator, OperatorId.NotEqual, "!=", 0);

		////////////////////////////////////////////////
		// 数字操作符，优先级为“100-199”：
		////////////////////////////////////////////////
		public static readonly Operator Number = new(OperatorType.CalculationItem, OperatorId.Number, 100);
		public static readonly Operator Variable = new(OperatorType.CalculationItem, OperatorId.Variable, 100);
		public static readonly Operator Function = new(OperatorType.CalculationItem, OperatorId.Function, 100);

		////////////////////////////////////////////////
		// 计算操作符，优先级为“200-299”：
		////////////////////////////////////////////////
		public static readonly Operator Plus = new(OperatorType.CalculationOperator, OperatorId.Plus, "+", 200);
		public static readonly Operator Minus = new(OperatorType.CalculationOperator, OperatorId.Minus, "-", 200);
		public static readonly Operator And = new(OperatorType.CalculationOperator, OperatorId.And, "&&", 200);
		public static readonly Operator Or = new(OperatorType.CalculationOperator, OperatorId.Or, "||", 200);
		public static readonly Operator Multiply = new(OperatorType.CalculationOperator, OperatorId.Multiply, "*", 210);
		public static readonly Operator Divide = new(OperatorType.CalculationOperator, OperatorId.Divide, "/", 210);
		public static readonly Operator Modulo = new(OperatorType.CalculationOperator, OperatorId.Modulo, "%", 210);
		public static readonly Operator Power = new(OperatorType.CalculationOperator, OperatorId.Power, "^", 220);

		////////////////////////////////////////////////
		// 计算块操作符，优先级最高，为“900-1000”：
		////////////////////////////////////////////////
		public static readonly Operator SectionBegin = new(OperatorType.Section, OperatorId.SectionBegin, "(", 900);
		public static readonly Operator SectionEnd = new(OperatorType.Section, OperatorId.SectionEnd, ")", 900);

		private static readonly Operator[] AllOperators = new Operator[]
		{
                        ////////////////////////////////////////////////
			// 计算块操作符：
                        ////////////////////////////////////////////////
                        SectionBegin,
			SectionEnd,
                                
                        ////////////////////////////////////////////////
                        // 数字操作符：
                        ////////////////////////////////////////////////
                        Number,
			Variable,
			Function,

                        ////////////////////////////////////////////////
                        // 计算操作符：
                        ////////////////////////////////////////////////
                        Plus,
			Minus,
			And,
			Or,
			Multiply,
			Divide,
			Modulo,
			Power,

                        ////////////////////////////////////////////////
                        // 比较操作符：
                        ////////////////////////////////////////////////
                        // 注意，由于“<，>，=”和“<=，>=，=，!=”有符号重叠，
                        // 因此，一定注意他们在“AllOperators”中的顺序。
                        LessThanOrEqual,
			GreaterThanOrEqual,
			LessThan,
			GreaterThan,
                        // 注意
                        Equal,
			NotEqual
		};

		private static readonly Func<string[]> ToGetAllOperatorKeywords = () =>
		{
			var allOperatorKeywords = new string[AllOperators.Length];
			for (var operatorIndex = 0;
			operatorIndex < AllOperators.Length;
			operatorIndex++)
			{
				var currentOperator = AllOperators[operatorIndex];
				var currentOperatorKeyword = currentOperator.Keyword;
				if (currentOperatorKeyword?.Length > 0)
				{
					// !!!
					allOperatorKeywords[operatorIndex] = currentOperatorKeyword;
					// !!!
				}
			}
			return allOperatorKeywords;
		};

		private static readonly string[] AllOperatorKeywords = ToGetAllOperatorKeywords();

		#endregion



		////////////////////////////////////////////////
		// @类方法
		////////////////////////////////////////////////

		#region 类方法

		public static List<Operator>? GetOperatorsFromString(
			string? str,
			int beginCharIndex,
			bool isIgnoreSpace,
			StringComparison stringComparison,
			out int operatorsEndCharIndex)
		{
			operatorsEndCharIndex = beginCharIndex;

			if (str == null
				|| str.Length < 1)
			{
				return null;
			}
			if (beginCharIndex >= str.Length)
			{
				return null;
			}

			var operators = new List<Operator>();
			for (var charIndex = beginCharIndex;
				charIndex < str.Length;)
			{
				var character = str[charIndex];
				if (character == ' ')
				{
					// !!!
					charIndex++;
					continue;
					// !!!
				}

				// !!!
				Operator? operatorMatched = null;
				var operatorMatchedCharsCount = 0;
				// !!!

				////////////////////////////////////////////////
				// 1/2，普通的操作符匹配：
				////////////////////////////////////////////////
				foreach (var normalOperator in AllOperators)
				{
					if (CharExtension.IsCharsOfStringEqualsKey(
						normalOperator.Keyword,
						str,
						charIndex,
						out _))
					{
						// !!!
						operatorMatched = normalOperator;
						operatorMatchedCharsCount = normalOperator.Keyword.Length;
						break;
						// !!!
					}
				}

				////////////////////////////////////////////////
				// 2/2，特殊的操作符匹配：
				////////////////////////////////////////////////
				if (operatorMatched == null)
				{
					operatorMatchedCharsCount
						= CharExtension.GetFloatNumberCharsCountFromString(
							str,
							charIndex,
							isIgnoreSpace,
							out var number);
					if (operatorMatchedCharsCount > 0)
					{
						// !!!
						operatorMatched = new Operator(number);
						// !!!
					}
				}
				if (operatorMatched == null)
				{
					operatorMatchedCharsCount
						= CharExtension.GetStringCharsCountFromStringBeforeKeys(
							AllOperatorKeywords,
							true,
							str,
							charIndex,
							isIgnoreSpace,
							stringComparison,
							out var calculationItem,
							out var operatorKeywordFound);
					if (calculationItem?.Length > 0)
					{
						if (operatorKeywordFound?.EqualsIgnoreCase(SectionBegin.Keyword)
							 != true)
						{
							// !!!
							var variableName = calculationItem;
							operatorMatched = new Operator(variableName);
							// operatorMatchedCharsCount = operatorMatchedCharsCount;
							// !!!
						}
						else
						{
							var functionName = calculationItem;
							var functionParamsBeginIndex
								= charIndex
								+ operatorMatchedCharsCount
								+ SectionBegin.Keyword.Length;
							var SectionEndKeyword
								= SectionEnd.Keyword!;
							var functionParamsEndIndex
								= str.IndexOf(
									SectionEndKeyword,
									functionParamsBeginIndex,
									stringComparison);
							if (functionParamsEndIndex > functionParamsBeginIndex)
							{
								var functionParamsMatchedCharsCount = functionParamsEndIndex - functionParamsBeginIndex;
								var functionParamsString = str.Substring(functionParamsBeginIndex, functionParamsMatchedCharsCount);
								string[]? functionParams = null;
								if (functionParamsString?.Length > 0)
								{
									functionParams = functionParamsString.Split(
										MathExpression.FunctionParamSpliter,
										StringSplitOptions.RemoveEmptyEntries
										| StringSplitOptions.TrimEntries);
								}
								// !!!
								operatorMatched = new Operator(functionName, functionParams);
								operatorMatchedCharsCount
									+= SectionBegin.Keyword.Length
									+ functionParamsMatchedCharsCount
									+ SectionEndKeyword.Length;
								// !!!
							}
							else
							{
								operatorMatchedCharsCount = 0;
							}
						}
					}
				}
				if (operatorMatched != null)
				{
					// !!!
					operators.Add(operatorMatched);
					charIndex += operatorMatchedCharsCount;
					// !!!
				}
				else
				{

					// throw new ArgumentException("解析表达式失败，无效的计算表达式。");

					// !!!
					operatorsEndCharIndex = charIndex;
					// !!!
					break;
				}
			}
			return operators;
		}

		public static CalculationNumber? CalculateOperators(
			IList<Operator> operators,
			Func<string, CalculationNumber?>? toGetVariableValue,
			Boolean isUnknowVariableAsZero,
			Func<string, Func<string[]?, CalculationNumber?>?>? toGetFunction,
			Func<Operator, CalculationNumber?, CalculationNumber?, bool, CalculationNumber?>? toCalculateWith)
		{
			if (operators == null
				|| operators.Count < 1)
			{
				return null;
			}

			////////////////////////////////////////////////
			// 1/3，计算合并计算单元，合计后计算单元从：
			// 1+1+(2+(3+3)+2)+1+1
			// 简化为：
			// 1+ (2 + 3 + 2) + 1
			////////////////////////////////////////////////

			var calculationUnits = new LinkedItems<CalculationUnit>();
			CalculationUnit? lastCalculationUnit = null;
			var currentCalculationLevel = 0;
			var operatorsCount = operators.Count;
			for (var operatorIndex = 0;
				operatorIndex <= operatorsCount;
				operatorIndex++)
			{
				var isNeedCalculationToLeftInSameCalaculationDetp = false;

				if (operatorIndex < operatorsCount)
				{
					var currentOperator = operators[operatorIndex];
					switch (currentOperator.Type)
					{
						default:
							{
								throw new ArgumentException("计算表达式失败，未知的表达式。");
							}
						case OperatorType.Section:
							{
								if (currentOperator.Id == OperatorId.SectionBegin)
								{
									////////////////////////////////////////////////
									lastCalculationUnit = null;
									currentCalculationLevel++;
									////////////////////////////////////////////////
								}
								else if (currentOperator.Id == OperatorId.SectionEnd)
								{
									////////////////////////////////////////////////
									currentCalculationLevel--;
									//////////////////////////////////////////////// 

									// 空括号，括号之间没有表达式：
									if (lastCalculationUnit == null)
									{
										// !!!
										lastCalculationUnit = calculationUnits.Last;
										// !!!
									}
									else
									{
										//////////////////////////////////////////////// 
										isNeedCalculationToLeftInSameCalaculationDetp = true;
										//////////////////////////////////////////////// 
									}
								}
								else
								{
									throw new ArgumentException("计算表达式失败，未知的表达式。");
								}
							}
							break;
						case OperatorType.CalculationItem:
							{
								CalculationNumber? rightNumber = null;
								if (currentOperator.Id == OperatorId.Variable)
								{
									var rightNumberName
										= currentOperator.VariableName!;
									rightNumber
										= toGetVariableValue?.Invoke(rightNumberName);
									if (rightNumber != null)
									{
										rightNumber.Name = rightNumberName;
									}
									else
									{
										// !!!
										rightNumber = new CalculationNumber(rightNumberName);
										// !!!
									}
								}
								else if (currentOperator.Id == OperatorId.Function)
								{
									var function = toGetFunction?.Invoke(currentOperator.FunctionName!);
									if (function != null)
									{
										// !!!
										rightNumber = function(currentOperator.FunctionParams);
										// !!!
									}
									else if (MathExpression.CalculateWithFuncation(
										currentOperator.FunctionName!,
										currentOperator.FunctionParams,
										out var rightNumberByDefaultFunction) == true)
									{
										// !!!
										rightNumber = rightNumberByDefaultFunction;
										// !!!
									}
									else
									{
										throw new ArgumentException("计算表达式失败，无法获得函数“" + currentOperator.FunctionName + "”的操作方法。");
									}
								}
								else
								{
									rightNumber = new CalculationNumber(currentOperator.Number);
								}

								var isNeedCreateNewCalculationUnit = false;
								if (lastCalculationUnit == null)
								{
									isNeedCreateNewCalculationUnit = true;
								}
								// 前一个计算符号为“null”：
								else if (lastCalculationUnit.CalculationOperator == null)
								{
									throw new ArgumentException("计算表达式失败，遇到了错误的表达式。");
								}
								else if ((operatorIndex + 1) < operatorsCount)
								{
									var nextOperator = operators[operatorIndex + 1];
									if (nextOperator.Type == OperatorType.CalculationOperator)
									{
										// 下一个计算符号不为“null”，并且优先级高于前一个：
										if (nextOperator.RankLevel > lastCalculationUnit.CalculationOperator.RankLevel)
										{
											isNeedCreateNewCalculationUnit = true;
										}
										// 下一个计算符号不为“null”，并且优先级高低于前一个：
										else if (nextOperator.RankLevel < lastCalculationUnit.CalculationOperator.RankLevel)
										{
											// !!!
											isNeedCalculationToLeftInSameCalaculationDetp = true;
											// !!!
										}
									}
								}
								if (isNeedCreateNewCalculationUnit)
								{
									////////////////////////////////////////////////
									lastCalculationUnit = new CalculationUnit(
										currentCalculationLevel,
										rightNumber!);
									{ }
									calculationUnits.Add(lastCalculationUnit);
									////////////////////////////////////////////////
								}
								else
								{
									var leftNumber = lastCalculationUnit!.LeftNumber;
									var calculationOperator = lastCalculationUnit!.CalculationOperator!;
									// rightNumber = rightNumber;


									////////////////////////////////////////////////
									lastCalculationUnit.LeftNumber
										= toCalculateWith != null
										? toCalculateWith(
											calculationOperator,
											leftNumber,
											rightNumber,
											isUnknowVariableAsZero)
										: MathExpression.CalculateWithOperator(
											calculationOperator,
											leftNumber,
											rightNumber,
											isUnknowVariableAsZero);
									// !!! ⚠
									lastCalculationUnit.CalculationOperator = null;
									// !!! ⚠
									////////////////////////////////////////////////
								}
							}
							break;
						case OperatorType.CalculationOperator:
							{
								if (lastCalculationUnit == null
									|| lastCalculationUnit.CalculationOperator != null)
								{
									throw new ArgumentException("计算表达式失败，遇到了错误的表达式。");
								}
								else
								{
									lastCalculationUnit.CalculationOperator = currentOperator;
								}
							}
							break;
					}
				}

				////////////////////////////////////////////////
				////////////////////////////////////////////////
				////////////////////////////////////////////////
				if (operatorIndex == operatorsCount)
				{
					isNeedCalculationToLeftInSameCalaculationDetp = true;
				}
				if (isNeedCalculationToLeftInSameCalaculationDetp)
				{
					if (lastCalculationUnit == null)
					{
						throw new ArgumentException("计算表达式失败，遇到了错误的表达式。");
					}

					var lastCalculationDepth = lastCalculationUnit.CalculationDepth;
					var firstCalculationUnit = lastCalculationUnit;
					for (var prevCalculationUnit = lastCalculationUnit.Prev;
						prevCalculationUnit != null;
						prevCalculationUnit = prevCalculationUnit.Prev)
					{
						if (prevCalculationUnit.CalculationDepth == lastCalculationDepth)
						{
							firstCalculationUnit = prevCalculationUnit;
						}
						else
						{
							break;
						}
					}

					var endCalculationUnit = lastCalculationUnit.Next;
					while (firstCalculationUnit.Next != endCalculationUnit)
					{
						for (var calculationUnit = firstCalculationUnit;
							calculationUnit != null
							&& calculationUnit.Next != endCalculationUnit;)
						{
							var currentCalculationOperator = calculationUnit.CalculationOperator;
							if (currentCalculationOperator == null)
							{
								throw new ArgumentException("计算表达式失败，遇到了错误的表达式。");
							}

							var prevCalculationUnit
								= calculationUnit != firstCalculationUnit
								? calculationUnit.Prev
								: null;
							if (prevCalculationUnit != null
								&& prevCalculationUnit.CalculationOperator != null
								&& prevCalculationUnit.CalculationOperator.RankLevel == currentCalculationOperator.RankLevel)
							{
								// !!!
								break;
								// !!!
							}
							else
							{
								var nextCalculationUnit = calculationUnit.Next!;
								if (nextCalculationUnit.CalculationOperator != null
									&& nextCalculationUnit.CalculationOperator.RankLevel > currentCalculationOperator.RankLevel)
								{
									// 右侧表达式优先级较高时，本次不做计算，直接从下一步表达式开始计算。
									// !!!
									calculationUnit = calculationUnit.Next;
									// !!!
								}
								else
								{
									var leftNumber = calculationUnit.LeftNumber;
									var calculationOperator = currentCalculationOperator;
									var rightNumber = nextCalculationUnit.LeftNumber;

									////////////////////////////////////////////////
									calculationUnit.LeftNumber
										= toCalculateWith != null
										? toCalculateWith(
											calculationOperator,
											leftNumber,
											rightNumber,
											isUnknowVariableAsZero)
										: MathExpression.CalculateWithOperator(
											calculationOperator,
											leftNumber,
											rightNumber,
											isUnknowVariableAsZero);
									// !!! ⚠
									calculationUnit.CalculationOperator = nextCalculationUnit.CalculationOperator;
									// !!! ⚠
									calculationUnits.Remove(nextCalculationUnit);
									// !!! ⚠
									////////////////////////////////////////////////
								}
							}
						}
					}
					////////////////////////////////////////////////
					// !!!
					lastCalculationUnit = firstCalculationUnit;
					// 更新当前的计算深度（遇到块结束符号时会降低，否则保持不变）：
					lastCalculationUnit.CalculationDepth = currentCalculationLevel;
					// !!!
					////////////////////////////////////////////////
				}
			}

			////////////////////////////////////////////////
			// 3/3，最终一定只剩下“1”个计算单元 ：
			////////////////////////////////////////////////
			if (calculationUnits.Count != 1
				|| lastCalculationUnit != calculationUnits.First
				|| lastCalculationUnit != calculationUnits.Last)
			{
				throw new ArgumentException("计算结果错误，无法求出最终解。");
			}

			if (lastCalculationUnit != null)
			{
				return lastCalculationUnit.LeftNumber;
			}
			return null;
		}

		protected static CalculationNumber? CalculateWithOperator(
			Operator calculationOperator,
			CalculationNumber? leftNumber,
			CalculationNumber? rightNumber,
			bool isUnknowVariableAsZero)
		{
			double calculationResultValue;
			var leftNumberValue
				= leftNumber?.Number != null
				? leftNumber.Number.Value
				: 0.0;
			var rightNumberValue
				= rightNumber?.Number != null
				? rightNumber.Number.Value
				: 0.0;

			switch (calculationOperator.Id)
			{
				default:
				case OperatorId.Unknown:
					{
						throw new ArgumentException("计算表达式失败，未知的计算符号。");
					}
				case OperatorId.SectionBegin:
				case OperatorId.SectionEnd:
				case OperatorId.Number:
				case OperatorId.Variable:
				case OperatorId.Function:
					{
						throw new ArgumentException("计算表达式失败，错误的计算符号。");
					}
				case OperatorId.Plus:
					{
						if (leftNumber?.Number == null
							|| rightNumber?.Number == null)
						{
							if (isUnknowVariableAsZero == false)
							{
								if (isUnknowVariableAsZero == false)
								{
									throw new ArgumentException("计算表达式失败，缺少计算数。");
								}
							}
						}

						calculationResultValue = leftNumberValue + rightNumberValue;
					}
					break;
				case OperatorId.Minus:
					{
						if (leftNumber?.Number == null
							|| rightNumber?.Number == null)
						{
							if (isUnknowVariableAsZero == false)
							{
								throw new ArgumentException("计算表达式失败，缺少计算数。");
							}
						}

						calculationResultValue = leftNumberValue - rightNumberValue;
					}
					break;
				case OperatorId.And:
					{
						if ((leftNumber?.Number != null && leftNumber.Number != 0)
							&&
							(rightNumber?.Number != null && rightNumber.Number != 0))
						{
							calculationResultValue = 1.0;
						}
						else
						{
							calculationResultValue = 0.0;
						}
					}
					break;
				case OperatorId.Or:
					{
						if ((leftNumber?.Number != null && leftNumber.Number != 0)
							||
							(rightNumber?.Number != null && rightNumber.Number != 0))
						{
							calculationResultValue = 1.0;
						}
						else
						{
							calculationResultValue = 0.0;
						}
					}
					break;
				case OperatorId.Multiply:
					{
						if (leftNumber?.Number == null
							|| rightNumber?.Number == null)
						{
							if (isUnknowVariableAsZero == false)
							{
								throw new ArgumentException("计算表达式失败，缺少计算数。");
							}
						}

						calculationResultValue = leftNumberValue * rightNumberValue;
					}
					break;
				case OperatorId.Divide:
					{
						if (leftNumber?.Number == null
							|| rightNumber?.Number == null)
						{
							if (isUnknowVariableAsZero == false)
							{
								throw new ArgumentException("计算表达式失败，缺少计算数。");
							}
						}

						if (rightNumberValue == 0.0)
						{
							throw new ArgumentException("计算表达式失败，无法除以”0“。");
						}
						else
						{
							calculationResultValue = leftNumberValue / rightNumberValue;
						}
					}
					break;
				case OperatorId.Modulo:
					{
						if (leftNumber?.Number == null
							|| rightNumber?.Number == null)
						{
							if (isUnknowVariableAsZero == false)
							{
								throw new ArgumentException("计算表达式失败，缺少计算数。");
							}
						}

						calculationResultValue = leftNumberValue % rightNumberValue;
					}
					break;
				case OperatorId.Power:
					{
						if (leftNumber?.Number == null
							|| rightNumber?.Number == null)
						{
							if (isUnknowVariableAsZero == false)
							{
								throw new ArgumentException("计算表达式失败，缺少计算数。");
							}
						}

						calculationResultValue = System.Math.Pow(leftNumberValue, rightNumberValue);
					}
					break;
				case OperatorId.LessThan:
					{
						if (leftNumber?.Number == null
							|| rightNumber?.Number == null)
						{
							if (isUnknowVariableAsZero == false)
							{
								throw new ArgumentException("计算表达式失败，缺少计算数。");
							}
						}

						calculationResultValue = leftNumberValue < rightNumberValue ? 1.0 : 0.0;
					}
					break;
				case OperatorId.LessThanOrEqual:
					{
						if (leftNumber?.Number == null
							|| rightNumber?.Number == null)
						{
							if (isUnknowVariableAsZero == false)
							{
								throw new ArgumentException("计算表达式失败，缺少计算数。");
							}
						}

						calculationResultValue = leftNumberValue <= rightNumberValue ? 1.0 : 0.0;
					}
					break;
				case OperatorId.Equal:
					{
						if (leftNumber?.Number == null
							|| rightNumber?.Number == null)
						{
							if (isUnknowVariableAsZero == false)
							{
								throw new ArgumentException("计算表达式失败，缺少计算数。");
							}
						}

						calculationResultValue = leftNumberValue == rightNumberValue ? 1.0 : 0.0;
					}
					break;
				case OperatorId.NotEqual:
					{
						if (leftNumber?.Number == null
							|| rightNumber?.Number == null)
						{
							if (isUnknowVariableAsZero == false)
							{
								throw new ArgumentException("计算表达式失败，缺少计算数。");
							}
						}

						calculationResultValue = leftNumberValue != rightNumberValue ? 1.0 : 0.0;
					}
					break;
				case OperatorId.GreaterThanOrEqual:
					{
						if (leftNumber?.Number == null
							|| rightNumber?.Number == null)
						{
							if (isUnknowVariableAsZero == false)
							{
								throw new ArgumentException("计算表达式失败，缺少计算数。");
							}
						}

						calculationResultValue = leftNumberValue >= rightNumberValue ? 1.0 : 0.0;
					}
					break;
				case OperatorId.GreaterThan:
					{
						if (leftNumber?.Number == null
							|| rightNumber?.Number == null)
						{
							if (isUnknowVariableAsZero == false)
							{
								throw new ArgumentException("计算表达式失败，缺少计算数。");
							}
						}

						calculationResultValue = leftNumberValue > rightNumberValue ? 1.0 : 0.0;
					}
					break;
			}
			return new CalculationNumber(calculationResultValue);
		}

		protected static bool CalculateWithFuncation(
			string funcationName,
			string[]? functionParams,
			out CalculationNumber? resultNumber)
		{

			// !!!
			resultNumber = null;
			// !!!

			if (funcationName == null
				|| funcationName.Length < 1)
			{
				return false;
			}


			////////////////////////////////////////////////
			// 常量：
			////////////////////////////////////////////////
			if ("E".EqualsIgnoreCase(funcationName))
			{
				// !!!
				resultNumber = new CalculationNumber(Math.E);
				// !!!
				return true;
			}
			else if ("PI".EqualsIgnoreCase(funcationName))
			{
				// !!!
				resultNumber = new CalculationNumber(Math.PI);
				// !!!
				return true;
			}
			else if ("Tau".EqualsIgnoreCase(funcationName))
			{
				// !!!
				resultNumber = new CalculationNumber(Math.Tau);
				// !!!
				return true;
			}
			////////////////////////////////////////////////
			// 数值操作：
			////////////////////////////////////////////////
			else if ("ToNumber".EqualsIgnoreCase(funcationName))
			{
				if (functionParams == null
					|| functionParams.Length < 0)
				{
					throw new ArgumentException("没有足够的计算参数。");
				}

				var numberString = functionParams[0];

				// !!!
				resultNumber = new CalculationNumber(numberString.DoubleValue());
				// !!!

				return true;
			}
			else if ("ToHashCodeInInteger".EqualsIgnoreCase(funcationName))
			{
				if (functionParams == null
					|| functionParams.Length < 0)
				{
					throw new ArgumentException("没有足够的计算参数。");
				}

				var numberString = functionParams[0];

				// !!!
				resultNumber = new CalculationNumber(numberString.GetHashCode());
				// !!!

				return true;
			}
			else if ("Abs".EqualsIgnoreCase(funcationName))
			{
				var numbers = functionParams?.TryToDoubles();
				if (numbers == null
					|| numbers.Count < 0)
				{
					throw new ArgumentException("没有足够的计算参数。");
				}

				double number = numbers[0];

				// !!!
				resultNumber = new CalculationNumber(Math.Abs(number));
				// !!!
				return true;
			}
			else if ("Floor".EqualsIgnoreCase(funcationName))
			{
				var numbers = functionParams?.TryToDoubles();
				if (numbers == null
					|| numbers.Count < 0)
				{
					throw new ArgumentException("没有足够的计算参数。");
				}

				double number = numbers[0];

				// !!!
				resultNumber = new CalculationNumber(Math.Floor(number));
				// !!!
				return true;
			}
			else if ("Round".EqualsIgnoreCase(funcationName))
			{
				var numbers = functionParams?.TryToDoubles();
				if (numbers == null
					|| numbers.Count < 0)
				{
					throw new ArgumentException("没有足够的计算参数。");
				}

				double number = numbers[0];
				int decimalsCount = numbers.Count > 1
					? (int)numbers[1]
					: 0;

				// !!!
				resultNumber = new CalculationNumber(Math.Round(number, decimalsCount));
				// !!!
				return true;
			}
			else if ("Ceiling".EqualsIgnoreCase(funcationName))
			{
				var numbers = functionParams?.TryToDoubles();
				if (numbers == null
					|| numbers.Count < 0)
				{
					throw new ArgumentException("没有足够的计算参数。");
				}

				double number = numbers[0];

				// !!!
				resultNumber = new CalculationNumber(Math.Ceiling(number));
				// !!!
				return true;
			}
			////////////////////////////////////////////////
			// 代数计算：
			////////////////////////////////////////////////
			else if ("Max".EqualsIgnoreCase(funcationName))
			{
				var numbers = functionParams?.TryToDoubles();
				if (numbers == null
					|| numbers.Count < 0)
				{
					throw new ArgumentException("没有足够的计算参数。");
				}

				double maxValue = double.MinValue;
				foreach (var number in numbers)
				{
					if (maxValue < number)
					{
						maxValue = number;
					}
				}
				// !!!
				resultNumber = new CalculationNumber(maxValue);
				// !!!

				return true;
			}
			else if ("Min".EqualsIgnoreCase(funcationName))
			{
				var numbers = functionParams?.TryToDoubles();
				if (numbers == null
					|| numbers.Count < 0)
				{
					throw new ArgumentException("没有足够的计算参数。");
				}

				double minValue = double.MaxValue;
				foreach (var number in numbers)
				{
					if (minValue > number)
					{
						minValue = number;
					}
				}
				// !!!
				resultNumber = new CalculationNumber(minValue);
				// !!!

				return true;
			}
			else if ("Avg".EqualsIgnoreCase(funcationName))
			{
				var numbers = functionParams?.TryToDoubles();
				if (numbers == null
					|| numbers.Count < 0)
				{
					throw new ArgumentException("没有足够的计算参数。");
				}

				double numbersValue = 0.0;
				foreach (var number in numbers)
				{
					numbersValue += number;
				}
				// !!!
				resultNumber = new CalculationNumber(numbersValue / numbers.Count);
				// !!!

				return true;
			}
			else if ("Pow".EqualsIgnoreCase(funcationName))
			{
				var numbers = functionParams?.TryToDoubles();
				if (numbers == null
					|| numbers.Count < 0)
				{
					throw new ArgumentException("没有足够的计算参数。");
				}

				var number = numbers[0];
				var powValue
					= numbers.Count > 1
					? numbers[0]
					: 2.0F;

				// !!!
				resultNumber = new CalculationNumber(Math.Pow(number, powValue));
				// !!!

				return true;
			}
			else if ("Sqrt".EqualsIgnoreCase(funcationName))
			{
				var numbers = functionParams?.TryToDoubles();
				if (numbers == null
					|| numbers.Count < 0)
				{
					throw new ArgumentException("没有足够的计算参数。");
				}

				var number = numbers[0];

				// !!!
				resultNumber = new CalculationNumber(Math.Sqrt(number));
				// !!!

				return true;
			}
			////////////////////////////////////////////////
			// 几何，三角函数：
			////////////////////////////////////////////////
			else if ("Sin".EqualsIgnoreCase(funcationName))
			{
				var numbers = functionParams?.TryToDoubles();
				if (numbers == null
					|| numbers.Count < 0)
				{
					throw new ArgumentException("没有足够的计算参数。");
				}

				var angle = numbers[0];

				// !!!
				resultNumber = new CalculationNumber(Math.Sin(angle / (180.0 / Math.PI)));
				// !!!

				return true;
			}
			else if ("Cos".EqualsIgnoreCase(funcationName))
			{
				var numbers = functionParams?.TryToDoubles();
				if (numbers == null
					|| numbers.Count < 0)
				{
					throw new ArgumentException("没有足够的计算参数。");
				}

				var angle = numbers[0];

				// !!!
				resultNumber = new CalculationNumber(Math.Cos(angle / (180.0 / Math.PI)));
				// !!!

				return true;
			}
			else if ("Tan".EqualsIgnoreCase(funcationName))
			{
				var numbers = functionParams?.TryToDoubles();
				if (numbers == null
					|| numbers.Count < 0)
				{
					throw new ArgumentException("没有足够的计算参数。");
				}

				var angle = numbers[0];

				// !!!
				resultNumber = new CalculationNumber(Math.Tan(angle / (180.0 / Math.PI)));
				// !!!

				return true;
			}
			else if ("Cot".EqualsIgnoreCase(funcationName))
			{
				var numbers = functionParams?.TryToDoubles();
				if (numbers == null
					|| numbers.Count < 0)
				{
					throw new ArgumentException("没有足够的计算参数。");
				}

				var angle = numbers[0];

				// !!!
				resultNumber = new CalculationNumber(1.0 / Math.Tan(angle / (180.0 / Math.PI)));
				// !!!

				return true;
			}
			return false;
		}

		public static CalculationNumber? Parse(
			string? mathExpression,
			out int operatorsEndCharIndex,
			Func<string, CalculationNumber?>? toGetVariableValue,
			Boolean isUnknowVariableAsZero,
			Func<string, Func<string[]?, CalculationNumber>>? toGetFunction,
			int beginCharIndex = 0,
			bool isIgnoreSpace = true,
			StringComparison stringComparison = StringComparison.OrdinalIgnoreCase,
			Func<Operator, CalculationNumber?, CalculationNumber?, bool, CalculationNumber?>? toCalculateWith = null)
		{
			var operators = MathExpression.GetOperatorsFromString(
					mathExpression,
					beginCharIndex,
					isIgnoreSpace,
					stringComparison,
					out operatorsEndCharIndex);
			CalculationNumber? calculationResult = null;
			if (operators?.Count > 0)
			{
				calculationResult
					= MathExpression.CalculateOperators(
						operators,
						toGetVariableValue,
						isUnknowVariableAsZero,
						toGetFunction,
						toCalculateWith);
			}
			return calculationResult;
		}

		#endregion



		////////////////////////////////////////////////
		// @自身属性
		////////////////////////////////////////////////

		#region 自身属性

		protected List<Operator>? Operators { get; set; }


		public bool IsValid
		{
			get
			{
				if (this.Operators?.Count > 0)
				{
					return true;
				}
				return false;
			}
		}

		#endregion


		////////////////////////////////////////////////
		// @类方法
		////////////////////////////////////////////////

		////////////////////////////////////////////////
		// @自身实现
		////////////////////////////////////////////////

		#region 自身实现

		public MathExpression(
			string mathExpression,
			int beginCharIndex = 0,
			bool isIgnoreSpace = true,
			StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
		{
			this.Operators = MathExpression.GetOperatorsFromString(
				mathExpression,
				beginCharIndex,
				isIgnoreSpace,
				stringComparison,
				out _);
		}

		public CalculationNumber? Calcuate(
			Func<string, CalculationNumber?>? toGetVariableValue,
			Boolean isUnknowVariableAsZero,
			Func<string, Func<string[]?, CalculationNumber?>?>? toGetFunction)
		{
			CalculationNumber? calculateResult = null;
			var operators = this.Operators;
			if (operators?.Count > 0)
			{
				calculateResult
					= MathExpression.CalculateOperators(
						operators,
						toGetVariableValue,
						isUnknowVariableAsZero,
						toGetFunction,
						this.DidCalculateWithOperator);
			}
			return calculateResult;
		}

		#endregion


		////////////////////////////////////////////////
		// @事件节点
		////////////////////////////////////////////////

		#region 事件节点

		protected virtual CalculationNumber? DidCalculateWithOperator(
			Operator calculationOperator,
			CalculationNumber? leftNumber,
			CalculationNumber? rightNumber,
			bool isUnknowVariableAsZero)
		{
			return MathExpression.CalculateWithOperator(
				calculationOperator,
				leftNumber,
				rightNumber,
				isUnknowVariableAsZero);
		}

		#endregion


	}
}
