using BaoXia.Utils.Extensions;
using BaoXia.Utils.Models;
using System;
using System.Collections.Generic;

namespace BaoXia.Utils;

public class ConsoleUtil
{
	////////////////////////////////////////////////
	// @静态变量
	////////////////////////////////////////////////

	#region 静态变量

	public static readonly object ConsoleWriteLocker = new();

	#endregion


	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static void ClearCurrentLine()
	{
		lock (ConsoleWriteLocker)
		{
			var cursorTop = System.Console.CursorTop;
			if (System.Console.CursorLeft <= 0)
			{
				cursorTop -= 1;
			}
			System.Console.SetCursorPosition(
				0,
				cursorTop);

			System.Console.Write(string.Empty);
		}
	}

	public static void WriteCurrentLine(string? message)
	{
		if (string.IsNullOrEmpty(message))
		{
			return;
		}

		lock (ConsoleWriteLocker)
		{
			var cursorTop = System.Console.CursorTop;
			if (System.Console.CursorLeft <= 0)
			{
				cursorTop -= 1;
			}
			System.Console.SetCursorPosition(
				0,
				cursorTop);
			System.Console.WriteLine(message);
		}
	}

	public static void WriteLines(IEnumerable<string>? lines)
	{
		if (lines == null)
		{
			return;
		}

		lock (ConsoleWriteLocker)
		{
			foreach (var line in lines)
			{
				// !!!
				Console.WriteLine(line);
				// !!
			}
		}
	}

	public static void WriteLines(
	    IEnumerable<string>? lines,
	    ref ConsoleLinesInfo? lastConsoleLinesInfo,
	    bool isNeedClearLastLines = false,
	    int prefixBlankRowsCount = 0)
	{
		if (lastConsoleLinesInfo == null)
		{
			lastConsoleLinesInfo = new();
			lastConsoleLinesInfo.OriginalCursorInfo.Left = 0;// Console.CursorLeft;
			lastConsoleLinesInfo.OriginalCursorInfo.Top = Console.CursorTop;
		}

		lock (ConsoleWriteLocker)
		{
			// 如果内容一致，则不再更新：
			var lastLines = lastConsoleLinesInfo.LastLines;
			if (lines != null
				&& lastLines != null)
			{
				var linesCount = lines.GetCount();
				var lastLinesCount = lastLines.GetCount();
				if (linesCount == lastLinesCount)
				{
					var isLinesChanged = false;
					var linesEnumerator = lines.GetEnumerator();
					var lastLinesEnumerator = lastLines.GetEnumerator();
					while (linesEnumerator.MoveNext()
						&& lastLinesEnumerator.MoveNext())
					{
						var line = linesEnumerator.Current;
						var lastLine = lastLinesEnumerator.Current;
						if (line.Equals(lastLine) != true)
						{
							//
							isLinesChanged = true;
							break;
							//
						}
					}
					if (isLinesChanged == false)
					{
						return;
					}
				}
			}

			////////////////////////////////////////////////
			// 1/2，清空控制台：
			////////////////////////////////////////////////
			if (isNeedClearLastLines)
			{
				// !!!
				Console.CursorLeft = lastConsoleLinesInfo.OriginalCursorInfo.Left;
				Console.CursorTop = lastConsoleLinesInfo.OriginalCursorInfo.Top;
				// !!!
				var blankRow = string.Empty;
				while (blankRow.Length < lastConsoleLinesInfo.RowWidthMax)
				{
					blankRow += " ";
				}
				for (var consoleCursorTop = lastConsoleLinesInfo.OriginalCursorInfo.Top;
				consoleCursorTop <= lastConsoleLinesInfo.EndCursorInfo.Top;
				consoleCursorTop++)
				{
					Console.WriteLine(blankRow);
				}
			}

			////////////////////////////////////////////////
			// 2/2，重新输出最新信息：
			////////////////////////////////////////////////
			// !!!
			if (isNeedClearLastLines)
			{
				Console.CursorLeft = lastConsoleLinesInfo.OriginalCursorInfo.Left;
				Console.CursorTop = lastConsoleLinesInfo.OriginalCursorInfo.Top;
			}
			// !!!
			if (lines == null
			    || lines.GetCount() < 1)
			{
				return;
			}
			var consoleCursorRight = -1;
			var consoleCursorRightMax = lastConsoleLinesInfo.RowWidthMax;
			var consoleCursorBottom = -1;
			for (var prefixBlankRowIndex = 0;
				prefixBlankRowIndex < prefixBlankRowsCount;
				prefixBlankRowIndex++)
			{
				// !!!
				Console.WriteLine();
				// !!	
			}
			foreach (var line in lines)
			{
				// !!!
				Console.Write(line);
				// !!
				{
					consoleCursorRight = Console.CursorLeft;
					if (consoleCursorRightMax < consoleCursorRight)
					{
						consoleCursorRightMax = consoleCursorRight;
					}
					consoleCursorBottom = Console.CursorTop;
				}
				Console.WriteLine();
				// !!!
			}
			// !!!
			lastConsoleLinesInfo.LastLines = lines;
			// !!!
			lastConsoleLinesInfo.EndCursorInfo.Left = consoleCursorRight;
			lastConsoleLinesInfo.EndCursorInfo.Top = consoleCursorBottom;
			lastConsoleLinesInfo.RowWidthMax = consoleCursorRightMax;
		}
	}

	#endregion
}