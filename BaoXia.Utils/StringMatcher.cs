using BaoXia.Utils.Extensions;
using System;
using System.Collections.Generic;
using static BaoXia.Utils.MathTools.MathExpression;

namespace BaoXia.Utils
{
        /// <summary>
        /// 字符串匹配器，默认表达式规则为：
        /// 1，*，匹配任意长度的任意字符，直到遇到其他表达式对应的内容。
        /// 2，[]，根据容器内的表达式进行匹配，表达式容器默认为“[]”，支持以下运算：
        ///       a，。
        /// ，其他字符串视为字符常量进行比较。
        /// </summary>
        public class StringMatcher
        {

                ////////////////////////////////////////////////
                // @静态常量
                ////////////////////////////////////////////////

                #region 静态常量

                public const string Keyword_SubstringNeedMatched = "$Substring";

                #endregion


                ////////////////////////////////////////////////
                // @自身属性
                ////////////////////////////////////////////////

                #region 自身属性

                public StringMatchRuleKeywords? RuleKeywordsSpecified { get; set; }

                protected string? _expressionString;

                protected List<StringMatchRule>? _matchRules;

                public string? ExpressionString
                {
                        get
                        {
                                return _expressionString;
                        }
                        set
                        {
                                _expressionString = value;
                                _matchRules = new List<StringMatchRule>();

                                if (_expressionString == null
                                        || _expressionString.Length < 1)
                                {
                                        return;
                                }

                                var fuzzyMatchingKey = StringMatchRuleKeywords.DefaultWithAnyCharsDefault;
                                var expressionRuleBeginSign = StringMatchRuleKeywords.ExpressionRuleBeginSignDefault;
                                var expressionRuleEndSign = StringMatchRuleKeywords.ExpressionRuleEndSignDefault;

                                var expressionKeywordsSpecified = this.RuleKeywordsSpecified;
                                if (expressionKeywordsSpecified != null)
                                {
                                        fuzzyMatchingKey = expressionKeywordsSpecified.DefaultWithAnyChars;
                                        expressionRuleBeginSign = expressionKeywordsSpecified.ExpressionRuleBeginSign;
                                        expressionRuleEndSign = expressionKeywordsSpecified.ExpressionRuleEndSign;
                                }


                                StringMatchRuleType lastRuleType = StringMatchRuleType.Unknow;
                                int lastRuleBeginIndex = 0;
                                // int lastRuleEndIndex = 0;
                                for (var expressionCharIndex = 0;
                                        expressionCharIndex < _expressionString.Length;)
                                {
                                        // !!!🎉 【表达式】【默认】匹配成功，如果下方识别了更准确的表达式，则会覆盖当前信息。 🎉!!!
                                        StringMatchRuleType expressionType = StringMatchRuleType.Default_SameChars;
                                        int expressionBeginIndex = expressionCharIndex;
                                        int expressionEndIndex = expressionBeginIndex + 1;

                                        if (CharExtension.IsCharsOfStringEqualsKey(
                                                fuzzyMatchingKey,
                                                _expressionString,
                                                expressionBeginIndex,
                                                out _,
                                                StringComparison.OrdinalIgnoreCase) == true)
                                        {
                                                // !!!🎉 【通配符】匹配成功 🎉!!!
                                                expressionType = StringMatchRuleType.AnyChars;
                                                expressionEndIndex = expressionBeginIndex + fuzzyMatchingKey.Length;
                                                // !!!
                                        }
                                        else if (CharExtension.IsCharsOfStringEqualsKey(
                                                expressionRuleBeginSign,
                                                _expressionString,
                                                expressionBeginIndex,
                                                out _,
                                                StringComparison.OrdinalIgnoreCase) == true)
                                        {
                                                // !!!
                                                expressionType = StringMatchRuleType.Expression;
                                                // !!!
                                                var matchExpressionEndSignIndex = -1;
                                                if ((expressionBeginIndex + 1 + expressionRuleEndSign.Length)
                                                        <= _expressionString.Length)
                                                {
                                                        matchExpressionEndSignIndex
                                                                = _expressionString.IndexOf(
                                                                        expressionRuleEndSign,
                                                                        expressionBeginIndex + 1,
                                                                        System.StringComparison.OrdinalIgnoreCase);
                                                }
                                                if (matchExpressionEndSignIndex > expressionBeginIndex)
                                                {
                                                        // !!!🎉 【表达式】匹配成功 🎉!!!
                                                        expressionType = StringMatchRuleType.Expression;
                                                        expressionEndIndex = matchExpressionEndSignIndex + expressionRuleEndSign.Length;
                                                        // !!!
                                                }
                                        }

                                        ////////////////////////////////////////////////
                                        // 结束【上一个】表达式：
                                        ////////////////////////////////////////////////
                                        if (// 当前表达式类型为【默认】，
                                            lastRuleType == StringMatchRuleType.Default_SameChars)
                                        {
                                                // 并且遇到了确认的【非默认】表达式，
                                                // 结束当前【默认】表达式，并加入表达式列表：
                                                // 或到达了表达式字符串结束处时，
                                                // 结束当前【默认】表达式，并加入表达式列表：
                                                if (expressionType != StringMatchRuleType.Default_SameChars
                                                        || expressionEndIndex == _expressionString.Length)
                                                {
                                                        var lastRuleEndIndex
                                                                = expressionType != StringMatchRuleType.Default_SameChars
                                                                ? expressionBeginIndex
                                                                : _expressionString.Length;
                                                        if (lastRuleEndIndex > lastRuleBeginIndex)
                                                        {
                                                                var ruleChars = _expressionString[lastRuleBeginIndex..lastRuleEndIndex];
                                                                var rule = new StringMatchRule(
                                                                        lastRuleType,
                                                                        ruleChars,
                                                                        null,
                                                                        _matchRules.Count < 1,
                                                                        lastRuleEndIndex == _expressionString.Length);
                                                                { }
                                                                _matchRules.Add(rule);
                                                        }
                                                        // !!!
                                                        lastRuleType = StringMatchRuleType.Unknow;
                                                        // !!!
                                                }
                                        }


                                        ////////////////////////////////////////////////
                                        // 结束【当前】表达式：
                                        ////////////////////////////////////////////////
                                        if (expressionType == StringMatchRuleType.Default_SameChars)
                                        {
                                                if (lastRuleType != StringMatchRuleType.Default_SameChars)
                                                {
                                                        lastRuleType = StringMatchRuleType.Default_SameChars;
                                                        lastRuleBeginIndex = expressionBeginIndex;
                                                }
                                        }
                                        else if (expressionEndIndex > expressionBeginIndex)
                                        {
                                                var ruleChars = _expressionString[expressionBeginIndex..expressionEndIndex];
                                                string? ruleExpression = null;
                                                if (expressionType == StringMatchRuleType.Expression)
                                                {
                                                        ruleExpression = ruleChars.Substring(
                                                                expressionRuleBeginSign.Length,
                                                                ruleChars.Length
                                                                - (expressionRuleBeginSign.Length
                                                                + expressionRuleEndSign.Length));
                                                }
                                                var rule = new StringMatchRule(
                                                        expressionType,
                                                        ruleChars,
                                                        ruleExpression,
                                                        _matchRules.Count < 1,
                                                        expressionEndIndex == _expressionString.Length);
                                                { }
                                                // !!!
                                                _matchRules.Add(rule);
                                                // !!!
                                        }


                                        ////////////////////////////////////////////////
                                        // !!! ⚠ 结束本次字符检查，开始新的字符检查 ⚠!!!
                                        expressionCharIndex = expressionEndIndex;
                                        // !!! ⚠ 结束本次字符检查，开始新的字符检查 ⚠!!!
                                        ////////////////////////////////////////////////
                                }
                        }
                }

                #endregion

                ////////////////////////////////////////////////
                // @自身实现
                ////////////////////////////////////////////////

                #region 自身实现

                public StringMatcher()
                { }

                public StringMatcher(string? rulesString)
                {
                        this.ExpressionString = rulesString;
                }

                protected static StringMatchRule? FindNextRuleFromRules(
                        List<StringMatchRule> expressions,
                        int beginExpressionIndex,
                        StringMatchRuleType objectExpressionType)
                {
                        if (expressions == null)
                        {
                                return null;
                        }

                        for (var ruleIndex = beginExpressionIndex;
                                ruleIndex < expressions.Count;
                                ruleIndex++)
                        {
                                var rule = expressions[ruleIndex];
                                if (rule.Type == objectExpressionType)
                                {
                                        return rule;
                                }
                        }
                        return null;
                }

                public bool IsMatched(
                        string? stringNeedMatched,
                        StringComparison stringComparison = StringComparison.OrdinalIgnoreCase,
                        bool isUnknowVariableAsZero = true,
                        string keyword_SubstringNeedMatched = Keyword_SubstringNeedMatched)
                {
                        if (stringNeedMatched == null)
                        {
                                return false;
                        }
                        if (stringNeedMatched.Length < 1)
                        {
                                return false;
                        }

                        if (_matchRules == null
                                || _matchRules.Count < 1)
                        {
                                return false;
                        }


                        var isMatched = true;
                        var urlPathMatchCharsBeginIndex = 0;
                        var isInFuzzyMatching = false;
                        var lastRuleIndex = _matchRules.Count - 1;
                        for (var ruleIndex = 0;
                                ruleIndex < _matchRules.Count;
                                ruleIndex++)
                        {
                                var rule = _matchRules[ruleIndex];
                                switch (rule.Type)
                                {
                                        default:
                                        case StringMatchRuleType.Unknow:
                                                {
                                                        throw new InvalidOperationException("未知的匹配规则类型。");
                                                }
                                        case StringMatchRuleType.Default_SameChars:
                                                {
                                                        var ruleChars = rule.Chars;
                                                        if (ruleChars?.Length > 0)
                                                        {
                                                                var lastUrlPathMatchCharsBeginIndex
                                                                        = urlPathMatchCharsBeginIndex;
                                                                if (rule.IsLastRule)
                                                                {
                                                                        urlPathMatchCharsBeginIndex
                                                                                = stringNeedMatched.Length
                                                                                - ruleChars.Length;
                                                                }

                                                                var charsCountMatched
                                                                        = this.DidGetCharsCountInStringMatchedWithRule_Default_SameChars(
                                                                                rule,
                                                                                stringNeedMatched,
                                                                                urlPathMatchCharsBeginIndex,
                                                                                stringNeedMatched.Length,
                                                                                stringComparison);

                                                                // 字符匹配成功：
                                                                if (charsCountMatched > 0)
                                                                {
                                                                        if ((urlPathMatchCharsBeginIndex
                                                                                - lastUrlPathMatchCharsBeginIndex) <= 0
                                                                                || isInFuzzyMatching == true)
                                                                        {
                                                                                // !!!🎉 匹配成功 🎉!!!
                                                                                isInFuzzyMatching = false;
                                                                                urlPathMatchCharsBeginIndex += charsCountMatched;
                                                                                // !!!
                                                                        }
                                                                        else
                                                                        {
                                                                                // !!!
                                                                                isMatched = false;
                                                                                // !!!
                                                                        }
                                                                }
                                                                else
                                                                {
                                                                        // !!!
                                                                        isMatched = false;
                                                                        // !!!
                                                                }
                                                        }
                                                        else
                                                        {
                                                                // !!!
                                                                isMatched = false;
                                                                // !!!
                                                        }
                                                }
                                                break;
                                        case StringMatchRuleType.AnyChars:
                                                {
                                                        // !!!
                                                        isInFuzzyMatching = true;
                                                        // !!!
                                                }
                                                break;
                                        case StringMatchRuleType.Expression:
                                                {
                                                        var nextDefaultExpression
                                                                = StringMatcher.FindNextRuleFromRules(
                                                                        _matchRules,
                                                                        ruleIndex + 1,
                                                                        StringMatchRuleType.Default_SameChars);
                                                        // !!!
                                                        var urlPathMatchCharsEndIndex = stringNeedMatched.Length;
                                                        // !!!
                                                        if (nextDefaultExpression != null)
                                                        {
                                                                if (nextDefaultExpression.IsLastRule)
                                                                {
                                                                        // !!! 最后一个“默认_相同字符串”规则的特殊处理 !!!
                                                                        var nextDefaultExpressionChars = nextDefaultExpression.Chars;
                                                                        if (nextDefaultExpressionChars != null
                                                                                && stringNeedMatched.EndsWith(
                                                                                nextDefaultExpressionChars,
                                                                                stringComparison))
                                                                        {
                                                                                urlPathMatchCharsEndIndex
                                                                                        = stringNeedMatched.Length
                                                                                        - nextDefaultExpressionChars.Length;
                                                                        }
                                                                        else
                                                                        {
                                                                                urlPathMatchCharsEndIndex = -1;
                                                                        }
                                                                }
                                                                else if (nextDefaultExpression.Chars != null)
                                                                {
                                                                        urlPathMatchCharsEndIndex
                                                                                = stringNeedMatched.IndexOf(
                                                                                nextDefaultExpression.Chars,
                                                                                urlPathMatchCharsBeginIndex,
                                                                                stringComparison);
                                                                }

                                                                // !!! 没有找到后续默认表达式的对应字符，则直接返回匹配失败 !!!
                                                                if (urlPathMatchCharsEndIndex < 0)
                                                                {
                                                                        // !!!
                                                                        isMatched = false;
                                                                        // !!!
                                                                }
                                                        }
                                                        if (isMatched)
                                                        {
                                                                var charsCountMatched
                                                                        = this.DidGetCharsCountInStringMatchedWithRule_Expression(
                                                                                rule,
                                                                                stringNeedMatched,
                                                                                urlPathMatchCharsBeginIndex,
                                                                                urlPathMatchCharsEndIndex,
                                                                                stringComparison,
                                                                                isUnknowVariableAsZero,
                                                                                keyword_SubstringNeedMatched);
                                                                // 字符匹配成功：
                                                                if (charsCountMatched > 0)
                                                                {
                                                                        // !!!🎉 匹配成功 🎉!!!
                                                                        isInFuzzyMatching = false;
                                                                        urlPathMatchCharsBeginIndex += charsCountMatched;
                                                                        // !!!

                                                                        // !!!⚠ 明确指定了结尾内容的，严格执行 ⚠!!!
                                                                        // 不在此处进行【匹配完整度】的判断，交由尾端判断。
                                                                        //if (rule.IsLastRule == true
                                                                        //        && urlPathMatchBeginCharIndex < stringNeedMatched.Length)
                                                                        //{
                                                                        //        isMatched = false;
                                                                        //}
                                                                }
                                                                else
                                                                {
                                                                        // !!!
                                                                        isMatched = false;
                                                                        // !!!
                                                                }
                                                        }
                                                }
                                                break;
                                }

                                if (isMatched != true)
                                {
                                        if (rule.Type != StringMatchRuleType.AnyChars
                                                && rule.IsLastRule != true
                                                && isInFuzzyMatching == true)
                                        {
                                                // !!!🎉 匹配成功 🎉!!!
                                                isMatched = true;
                                                urlPathMatchCharsBeginIndex += 1;
                                                // !!! ⚠
                                                ruleIndex -= 1;
                                                // !!! ⚠
                                        }
                                }

                                if (isMatched != true)
                                {
                                        return false;
                                }
                        }
                        return isMatched;
                }

                public bool IsNotMatched(
                        string stringNeedMatched,
                        StringComparison stringComparison = StringComparison.OrdinalIgnoreCase,
                        bool isUnknowVariableAsZero = true,
                        string keyword_SubstringNeedMatched = Keyword_SubstringNeedMatched)
                {
                        return !this.IsMatched(
                                stringNeedMatched,
                                stringComparison,
                                isUnknowVariableAsZero,
                                keyword_SubstringNeedMatched);
                }

                #endregion


                ////////////////////////////////////////////////
                // @事件节点
                ////////////////////////////////////////////////

                #region 事件节点

                protected virtual int DidGetCharsCountInStringMatchedWithRule_Default_SameChars(
                        StringMatchRule rule,
                        string objectString,
                        int objectCharsBeginIndex,
                        int objectCharsEndIndex,
                        StringComparison stringComparison)
                {
                        if (rule == null)
                        {
                                return 0;
                        }

                        var ruleChars = rule.Chars;
                        if (ruleChars == null
                                || ruleChars.Length < 1)
                        {
                                throw new AggregateException("无法匹配没有内容的“默认_相同字符串”规则。");
                        }

                        if (CharExtension.IsCharsOfStringEqualsKey(
                                 ruleChars,
                                 objectString,
                                 objectCharsBeginIndex,
                                 out _,
                                 stringComparison))
                        {

                                return ruleChars.Length;
                        }
                        return 0;
                }

                protected virtual int DidGetCharsCountInStringMatchedWithRule_Expression(
                                StringMatchRule rule,
                                string objectString,
                                int objectCharsBeginIndex,
                                int objectCharsEndIndex,
                                StringComparison stringComparison,
                                bool isUnknowVariableAsZero,
                                string keyword_SubstringNeedMatched)
                {
                        if (rule == null)
                        {
                                return 0;
                        }
                        var ruleExpression = rule.Expression;
                        if (ruleExpression == null
                                || ruleExpression.IsValid != true)
                        {
                                //

                                throw new AggregateException("无法匹配没有内容的“表达式”规则。");
                                //
                        }
                        if (objectString == null
                                || objectString.Length < 1)
                        {
                                return 0;
                        }
                        if (objectCharsBeginIndex < 0)
                        {
                                objectCharsBeginIndex = 0;
                        }
                        if (objectCharsEndIndex > objectString.Length)
                        {
                                objectCharsEndIndex = objectString.Length;
                        }
                        if (objectCharsBeginIndex >= objectCharsEndIndex)
                        {
                                return 0;
                        }

                        var expressionCalcuateResult
                                = ruleExpression.Calcuate(
                                        (variableName) =>
                                        {
                                                return this.DidGetExpressionVariableValueWithVariableName(
                                                        variableName,
                                                        objectString,
                                                        objectCharsBeginIndex,
                                                        objectCharsEndIndex,
                                                        stringComparison,
                                                        keyword_SubstringNeedMatched);
                                        },
                                        isUnknowVariableAsZero,
                                        (functionName) =>
                                        {
                                                return this.DidGetExpressionFunctionWithFunctionName(
                                                        functionName,
                                                        objectString,
                                                        objectCharsBeginIndex,
                                                        objectCharsEndIndex);
                                        });
                        if (expressionCalcuateResult != null)
                        {
                                if (expressionCalcuateResult.Number != null
                                        && expressionCalcuateResult.Number != 0)
                                {
                                        return (objectCharsEndIndex - objectCharsBeginIndex);
                                }
                        }
                        return 0;
                }

                protected virtual CalculationNumber? DidGetExpressionVariableValueWithVariableName(
                        string variableName,
                        string objectString,
                        int objectCharsBeginIndex,
                        int objectCharsEndIndex,
                        StringComparison stringComparison,
                        string keyword_SubstringNeedMatched)
                {
                        if (variableName == null
                                || variableName.Length < 1)
                        {
                                return null;
                        }

                        var doubleValue = 0.0;
                        if (variableName.EqualsIgnoreCase(keyword_SubstringNeedMatched) == true)
                        {
                                var substring = objectString[objectCharsBeginIndex..objectCharsEndIndex];
                                if (substring != null)
                                {
                                        _ = double.TryParse(substring, out doubleValue);
                                }
                        }
                        else if (CharExtension.IsCharsOfStringEqualsKey(
                               variableName,
                               objectString,
                               objectCharsBeginIndex,
                               out _,
                               stringComparison) == true
                               && (objectCharsBeginIndex + variableName.Length) == objectCharsEndIndex)
                        {
                                doubleValue = 1.0;
                        }
                        // !!!
                        var variableValue = new CalculationNumber(doubleValue);
                        { }
                        return variableValue;
                }

                protected virtual Func<string[]?, CalculationNumber?>? DidGetExpressionFunctionWithFunctionName(
                        string functionName,
                        string objectString,
                        int objectCharsBeginIndex,
                        int objectCharsEndIndex)
                {
                        return null;
                }

                #endregion
        }
}
