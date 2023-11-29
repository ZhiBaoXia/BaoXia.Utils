using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BaoXia.Utils
{
	public class ShellUtil
	{

		////////////////////////////////////////////////
		// @类方法
		////////////////////////////////////////////////

		#region 类方法

		public static List<string> RunCommand(string cmd)
		{
			if (cmd == null
				|| cmd.Length < 1)
			{
				throw new ApplicationException("无法创建Shell进程。");
			}

			cmd = cmd.Trim();

			var applicationFileName = cmd;
			string? applicationRunParams = null;
			var firstBlankIndexInCmd = cmd.IndexOf(" ");
			if (firstBlankIndexInCmd >= 0)
			{
				applicationFileName = cmd[0..firstBlankIndexInCmd];
				applicationRunParams = cmd[(firstBlankIndexInCmd + 1)..];
			}

			var processStartInfo
				= applicationRunParams?.Length > 0
				? new ProcessStartInfo(applicationFileName, applicationRunParams)
				: new ProcessStartInfo(applicationFileName);
			{
				processStartInfo.UseShellExecute = false;
				processStartInfo.CreateNoWindow = false;

				processStartInfo.RedirectStandardInput = true;
				processStartInfo.RedirectStandardOutput = true;
			};
			var process = Process.Start(processStartInfo) ?? throw new ApplicationException("无法创建Shell进程。");
			using var standardOutput = process.StandardOutput;
			var stringsProcessOutput = new List<string>();
			while (!standardOutput.EndOfStream)
			{
				var stringRead = standardOutput.ReadLine();
				if (stringRead?.Length > 0)
				{
					// !!!
					stringsProcessOutput.Add(stringRead);
					// !!!
				}
			}
			if (!process.HasExited)
			{
				process.Kill();
			}
			return stringsProcessOutput;
		}

		#endregion
	}
}
