namespace BaoXia.Utils
{
	public class ConsoleUtil
	{
		////////////////////////////////////////////////
		// @类方法
		////////////////////////////////////////////////

		#region 类方法

		public static void ClearCurrentLine()
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

		public static void WriteCurrentLine(string? message)
		{
			if (string.IsNullOrEmpty(message))
			{
				return;
			}

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

		#endregion
	}
}
