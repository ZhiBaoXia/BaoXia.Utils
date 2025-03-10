using BaoXia.Utils.MathTools;

namespace BaoXia.Utils
{
	public enum StringMatchRuleType
	{
		Unknown = 0,

		Default_SameChars = 1,

		AnyChars = 2,

		Expression = 3
	}

	public class StringMatchRule
	{
		public StringMatchRuleType Type { get; set; } = StringMatchRuleType.Unknown;

		public string? Chars { get; set; }

		public MathExpression? Expression { get; set; }

		public bool IsFirstRule { get; set; }

		public bool IsLastRule { get; set; }

		public StringMatchRule(
			StringMatchRuleType type,
			string? chars,
			string? expression,
			bool isFirstRule,
			bool isLastRule)
		{
			this.Type = type;
			this.Chars = chars;

			this.Expression
				= expression != null
				? new MathExpression(expression)
				: null;

			this.IsFirstRule = isFirstRule;
			this.IsLastRule = isLastRule;
		}
	}
}
