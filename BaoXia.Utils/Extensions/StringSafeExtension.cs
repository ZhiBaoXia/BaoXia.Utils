using System;
using System.Collections.Generic;
using System.Net;

namespace BaoXia.Utils.Extensions;

/// <summary>
/// “String”安全扩展类。
/// </summary>
public static class StringSafeExtension
{
	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	private static string StringByDecodeSqlInjectionEscape(string stringValue)
	{
		var stringDecoded = stringValue;
		for (var decodeCount = 0; decodeCount < 2; decodeCount++)
		{
			var stringAfterDecode = WebUtility.HtmlDecode(
				WebUtility.UrlDecode(stringDecoded));
			if (string.Equals(
				stringAfterDecode,
				stringDecoded,
				StringComparison.Ordinal))
			{
				break;
			}
			stringDecoded = stringAfterDecode;
		}
		return stringDecoded;
	}

	private static List<string> SqlTokensFromString(string stringValue)
	{
		var sqlTokens = new List<string>();
		for (var characterIndex = 0;
			characterIndex < stringValue.Length;)
		{
			var character = stringValue[characterIndex];
			if (char.IsWhiteSpace(character)
				|| char.IsControl(character))
			{
				characterIndex++;
				continue;
			}

			if (char.IsLetterOrDigit(character)
				|| character == '_'
				|| character == '$'
				|| character == '@')
			{
				var tokenBeginIndex = characterIndex;
				characterIndex++;
				while (characterIndex < stringValue.Length)
				{
					character = stringValue[characterIndex];
					if (char.IsLetterOrDigit(character) == false
						&& character != '_'
						&& character != '$'
						&& character != '@')
					{
						break;
					}
					characterIndex++;
				}
				sqlTokens.Add(stringValue[tokenBeginIndex..characterIndex]);
				continue;
			}

			if (character == '-'
				&& characterIndex + 1 < stringValue.Length
				&& stringValue[characterIndex + 1] == '-')
			{
				sqlTokens.Add("--");
				characterIndex += 2;
				continue;
			}
			if (character == '/'
				&& characterIndex + 1 < stringValue.Length
				&& stringValue[characterIndex + 1] == '*')
			{
				sqlTokens.Add("/*");
				characterIndex += 2;
				while (characterIndex + 1 < stringValue.Length
					&& (stringValue[characterIndex] != '*'
						|| stringValue[characterIndex + 1] != '/'))
				{
					characterIndex++;
				}
				if (characterIndex + 1 < stringValue.Length)
				{
					characterIndex += 2;
				}
				continue;
			}
			if (character == '#')
			{
				sqlTokens.Add("#");
				characterIndex++;
				continue;
			}

			if (characterIndex + 1 < stringValue.Length)
			{
				var operatorWithTwoCharacters = stringValue.Substring(characterIndex, 2);
				if (operatorWithTwoCharacters is "<>" or "!=" or "<=" or ">=")
				{
					sqlTokens.Add(operatorWithTwoCharacters);
					characterIndex += 2;
					continue;
				}
			}

			sqlTokens.Add(character.ToString());
			characterIndex++;
		}
		return sqlTokens;
	}

	private static bool IsSqlCommentToken(string sqlToken)
	{
		return sqlToken is "--" or "/*" or "#";
	}

	private static int NextSqlTokenIndex(
		IReadOnlyList<string> sqlTokens,
		int tokenIndex,
		bool isIgnoreQuotesAndParentheses = false)
	{
		for (tokenIndex++;
			tokenIndex < sqlTokens.Count;
			tokenIndex++)
		{
			var sqlToken = sqlTokens[tokenIndex];
			if (IsSqlCommentToken(sqlToken))
			{
				continue;
			}
			if (isIgnoreQuotesAndParentheses
				&& sqlToken is "'" or "\"" or "`" or "(" or ")")
			{
				continue;
			}
			return tokenIndex;
		}
		return -1;
	}

	private static bool ContainsSqlTokenSequence(
		IReadOnlyList<string> sqlTokens,
		params string[] sqlTokenSequence)
	{
		for (var tokenIndex = 0;
			tokenIndex < sqlTokens.Count;
			tokenIndex++)
		{
			if (sqlTokens[tokenIndex] != sqlTokenSequence[0])
			{
				continue;
			}

			var sequenceTokenIndex = tokenIndex;
			var sequenceItemIndex = 1;
			for (;
				sequenceItemIndex < sqlTokenSequence.Length;
				sequenceItemIndex++)
			{
				sequenceTokenIndex = NextSqlTokenIndex(
					sqlTokens,
					sequenceTokenIndex);
				if (sequenceTokenIndex < 0
					|| sqlTokens[sequenceTokenIndex] != sqlTokenSequence[sequenceItemIndex])
				{
					break;
				}
			}
			if (sequenceItemIndex >= sqlTokenSequence.Length)
			{
				return true;
			}
		}
		return false;
	}

	private static bool ContainsSqlTautology(IReadOnlyList<string> sqlTokens)
	{
		for (var tokenIndex = 0;
			tokenIndex < sqlTokens.Count;
			tokenIndex++)
		{
			if (sqlTokens[tokenIndex] is not ("or" or "and"))
			{
				continue;
			}

			var leftOperandIndex = NextSqlTokenIndex(
				sqlTokens,
				tokenIndex,
				true);
			if (leftOperandIndex < 0)
			{
				continue;
			}
			if (sqlTokens[leftOperandIndex] == "true")
			{
				return true;
			}

			var comparisonOperatorIndex = NextSqlTokenIndex(
				sqlTokens,
				leftOperandIndex,
				true);
			if (comparisonOperatorIndex < 0)
			{
				continue;
			}
			var rightOperandIndex = NextSqlTokenIndex(
				sqlTokens,
				comparisonOperatorIndex,
				true);
			if (rightOperandIndex >= 0
				&& sqlTokens[comparisonOperatorIndex] is "=" or "like"
				&& sqlTokens[leftOperandIndex] == sqlTokens[rightOperandIndex])
			{
				return true;
			}
		}
		return false;
	}

	private static bool ContainsSqlStatementAfterSemicolon(IReadOnlyList<string> sqlTokens)
	{
		for (var tokenIndex = 0;
			tokenIndex < sqlTokens.Count;
			tokenIndex++)
		{
			if (sqlTokens[tokenIndex] != ";")
			{
				continue;
			}

			var commandTokenIndex = NextSqlTokenIndex(
				sqlTokens,
				tokenIndex,
				true);
			if (commandTokenIndex >= 0
				&& sqlTokens[commandTokenIndex] is
					("select" or "insert" or "update" or "delete" or "drop" or "alter" or "create" or "truncate" or "merge" or "exec" or "execute" or "grant" or "revoke" or "waitfor"))
			{
				return true;
			}
		}
		return false;
	}

	private static bool ContainsSqlCommandAtBeginning(IReadOnlyList<string> sqlTokens)
	{
		var commandTokenIndex = NextSqlTokenIndex(sqlTokens, -1);
		if (commandTokenIndex < 0)
		{
			return false;
		}

		var commandToken = sqlTokens[commandTokenIndex];
		var nextTokenIndex = NextSqlTokenIndex(
			sqlTokens,
			commandTokenIndex);
		if (nextTokenIndex < 0)
		{
			return false;
		}
		var nextToken = sqlTokens[nextTokenIndex];
		if ((commandToken == "insert" && nextToken == "into")
			|| (commandToken == "delete" && nextToken == "from")
			|| (commandToken == "merge" && nextToken == "into")
			|| (commandToken is "drop" or "alter" or "create" or "truncate"
				&& nextToken is "table" or "database" or "schema" or "user")
			|| commandToken is "exec" or "execute" or "grant" or "revoke")
		{
			return true;
		}

		if (commandToken is not ("select" or "update"))
		{
			return false;
		}
		var targetToken = commandToken == "select"
			? "from"
			: "set";
		for (var searchCount = 0;
			searchCount < 16;
			searchCount++)
		{
			nextTokenIndex = NextSqlTokenIndex(
				sqlTokens,
				nextTokenIndex);
			if (nextTokenIndex < 0)
			{
				break;
			}
			if (sqlTokens[nextTokenIndex] == targetToken)
			{
				return true;
			}
		}
		return false;
	}

	private static bool ContainsSqlCommentAfterBreakout(IReadOnlyList<string> sqlTokens)
	{
		var singleQuoteCount = 0;
		var doubleQuoteCount = 0;
		var backtickCount = 0;
		foreach (var sqlToken in sqlTokens)
		{
			switch (sqlToken)
			{
				case "'":
					{
						singleQuoteCount++;
					}
					break;
				case "\"":
					{
						doubleQuoteCount++;
					}
					break;
				case "`":
					{
						backtickCount++;
					}
					break;
			}
		}

		var isBreakoutTokenFound = false;
		var isSqlExpressionFound = false;
		foreach (var sqlToken in sqlTokens)
		{
			if (sqlToken == ";"
				|| (sqlToken == "'" && singleQuoteCount % 2 != 0)
				|| (sqlToken == "\"" && doubleQuoteCount % 2 != 0)
				|| (sqlToken == "`" && backtickCount % 2 != 0))
			{
				isBreakoutTokenFound = true;
			}
			else if (sqlToken is "or" or "and" or "union" or "select"
				or "insert" or "update" or "delete" or "drop" or "exec" or "execute")
			{
				isSqlExpressionFound = true;
			}
			else if (IsSqlCommentToken(sqlToken)
				&& (isBreakoutTokenFound || isSqlExpressionFound))
			{
				return true;
			}
		}
		return false;
	}

	#endregion

	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	extension(string? stringValue)
	{
		/// <summary>
		/// 判断当前字符串是否包含常见的 SQL 注入攻击特征。
		/// </summary>
		/// <returns>包含常见的 SQL 注入攻击特征时返回“true”，否则返回“false”。</returns>
		/// <remarks>此方法只能作为辅助检测，数据库操作仍必须使用参数化查询。</remarks>
		public bool IsHackingStringBySqlInjection()
		{
			if (string.IsNullOrWhiteSpace(stringValue))
			{
				return false;
			}

			var stringDecoded = StringByDecodeSqlInjectionEscape(stringValue)
				.ToLowerInvariant();
			var sqlTokens = SqlTokensFromString(stringDecoded);
			if (sqlTokens.Count < 1)
			{
				return false;
			}

			if (ContainsSqlTautology(sqlTokens)
				|| ContainsSqlStatementAfterSemicolon(sqlTokens)
				|| ContainsSqlCommandAtBeginning(sqlTokens)
				|| ContainsSqlCommentAfterBreakout(sqlTokens)
				|| ContainsSqlTokenSequence(sqlTokens, "union", "select")
				|| ContainsSqlTokenSequence(sqlTokens, "union", "all", "select")
				|| ContainsSqlTokenSequence(sqlTokens, "waitfor", "delay")
				|| ContainsSqlTokenSequence(sqlTokens, "into", "outfile")
				|| ContainsSqlTokenSequence(sqlTokens, "into", "dumpfile")
				|| ContainsSqlTokenSequence(sqlTokens, "drop", "table")
				|| ContainsSqlTokenSequence(sqlTokens, "drop", "database")
				|| ContainsSqlTokenSequence(sqlTokens, "truncate", "table")
				|| ContainsSqlTokenSequence(sqlTokens, "alter", "table")
				|| ContainsSqlTokenSequence(sqlTokens, "xp_cmdshell")
				|| ContainsSqlTokenSequence(sqlTokens, "information_schema")
				|| ContainsSqlTokenSequence(sqlTokens, "load_file")
				|| ContainsSqlTokenSequence(sqlTokens, "sleep", "(")
				|| ContainsSqlTokenSequence(sqlTokens, "pg_sleep", "(")
				|| ContainsSqlTokenSequence(sqlTokens, "benchmark", "("))
			{
				return true;
			}

			return false;
		}
	}

	#endregion
}
