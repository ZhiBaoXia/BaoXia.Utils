namespace BaoXia.Utils
{
        public class StringMatchRuleKeywords
        {
                ////////////////////////////////////////////////
                // @静态常量
                ////////////////////////////////////////////////

                #region 静态常量

                /// <summary>
                /// Url中的路由关键字，模糊匹配：*。
                /// </summary>
                public static readonly string DefaultWithAnyCharsDefault = "*";

                /// <summary>
                /// Url中的匹配表达式，起始字符：[。
                /// </summary>
                public static readonly string ExpressionRuleBeginSignDefault = "[";

                /// <summary>
                /// Url中的匹配表达式，结束字符：[。
                /// </summary>
                public static readonly string ExpressionRuleEndSignDefault = "]";

                #endregion


                ////////////////////////////////////////////////
                // @自身属性
                ////////////////////////////////////////////////

                #region 自身属性

                public string DefaultWithAnyChars { get; set; } = DefaultWithAnyCharsDefault;

                public string ExpressionRuleBeginSign { get; set; } = ExpressionRuleBeginSignDefault;

                public string ExpressionRuleEndSign { get; set; } = ExpressionRuleEndSignDefault;

                #endregion
        }
}
