using System;

namespace BaoXia.Utils;

public class PathUtil
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static string? ToAbsoluteFilePathFromCurerntAppBaseDirectory(string? anyPath)
	{
		if (string.IsNullOrEmpty(anyPath))
		{
			return anyPath;
		}

		if (System.IO.Path.IsPathRooted(anyPath))
		{
			return anyPath;
		}

		var rootPath = System.IO.Path.Combine(
			AppDomain.CurrentDomain.BaseDirectory,
			anyPath);
		{ }
		return rootPath;
	}

	#endregion
}