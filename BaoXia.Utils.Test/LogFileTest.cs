using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace BaoXia.Utils.Test
{
	[TestClass]
	public class LogFileTest
	{
		[TestMethod]
		public void InitializeAndRecordTest()
		{
			var debugLogDirectoryName = "测试_调试日志";
			var debugLogFile = new LogFile(debugLogDirectoryName, "debug");
			var tmpFileDirectoryPath
				= System.IO.Path.GetTempPath().ToFileSystemDirectoryPath();
			var logFilesDirectoryPath = tmpFileDirectoryPath + "测试日志文件夹";
			{
				LogFile.InitializeWithLogFilesDirectoryPath(
					  logFilesDirectoryPath,
					  () => 0.6,
					  () => 2 * 1024 * 1024,
					  () => 100,
					  () => 1);
			}
			logFilesDirectoryPath = LogFile.LogFilesDirectoryPath;
			if (System.IO.Directory.Exists(logFilesDirectoryPath))
			{
				System.IO.Directory.Delete(
					logFilesDirectoryPath,
					true);
			}

			// !!!
			string logContentKeywords = "测试【调试】日志记录功能";
			for (var logNumber = 1;
				logNumber <= 100;
				logNumber++)
			{
				var logContent = logContentKeywords + "，第【" + logNumber + "】条日志。";
				{ }
				debugLogFile.Logs(this, logContent);
			}
			// !!!

			// !!!⚠ 等待1秒钟后，检查日志文件 ⚠!!!
			var logFileDirectoryPath = logFilesDirectoryPath + debugLogDirectoryName;
			DirectoryInfo logFileDirectoryInfo;
			do
			{
				System.Threading.Thread.Sleep(2000);

				// !!!
				logFileDirectoryInfo = new DirectoryInfo(logFileDirectoryPath);
				// !!!
			} while (logFileDirectoryInfo.Exists == false);

			var logFilePathes = System.IO.Directory.GetFiles(
				logFileDirectoryPath
				+ System.IO.Path.DirectorySeparatorChar,
				"*." + LogFile.LogFileExtensionName);
			//
			Assert.IsTrue(logFilePathes?.Length > 0);
			//

			var logFilePath = logFilePathes[0];
			var logFileContent = System.IO.File.ReadAllText(logFilePath);

			//
			Assert.IsTrue(logFileContent.Contains(logContentKeywords));
			//

			//
			debugLogFile.FlushLogBuffer();
			Assert.IsTrue(debugLogFile.LogRecords.IsEmpty);
			//
		}


		[TestMethod]
		public void KeysInLogContentToIgnoreLogsTest()
		{
			var debugLogDirectoryName = "测试_忽略关键字日志";
			var debugLogFile = new LogFile(debugLogDirectoryName, "debug");
			var tmpFileDirectoryPath
				= System.IO.Path.GetTempPath().ToFileSystemDirectoryPath();
			var logFilesDirectoryPath = tmpFileDirectoryPath + "测试日志文件夹";
			{
				LogFile.InitializeWithLogFilesDirectoryPath(
					  logFilesDirectoryPath,
					  () => 0.6,
					  () => 2 * 1024 * 1024,
					  () => 100,
					  () => 1);
			}
			logFilesDirectoryPath = LogFile.LogFilesDirectoryPath;
			if (System.IO.Directory.Exists(logFilesDirectoryPath))
			{
				System.IO.Directory.Delete(
					logFilesDirectoryPath,
					true);
			}


			////////////////////////////////////////////////
			// !!!
			string keyInLogContentToIgnoreLog = "应该被忽略的日志";
			debugLogFile.KeysInLogContentToIgnoreLogs = new string[] { keyInLogContentToIgnoreLog };
			// !!!
			////////////////////////////////////////////////



			// !!!
			string logContentKeywords = "测试【调试】日志记录功能";
			for (var logNumber = 1;
				logNumber <= 100;
				logNumber++)
			{
				var logContent = logContentKeywords + "，第【" + logNumber + "】条日志。";
				{
					if (logNumber % 2 != 0)
					{
						logContent = keyInLogContentToIgnoreLog + "：" + logContent;
					}
				}
				debugLogFile.Logs(this, logContent);
			}
			// !!!

			// !!!⚠ 等待1秒钟后，检查日志文件 ⚠!!!
			var logFileDirectoryPath = logFilesDirectoryPath + debugLogDirectoryName;
			DirectoryInfo logFileDirectoryInfo;
			do
			{
				System.Threading.Thread.Sleep(2000);

				// !!!
				logFileDirectoryInfo = new DirectoryInfo(logFileDirectoryPath);
				// !!!
			} while (logFileDirectoryInfo.Exists == false);

			var logFilePathes = System.IO.Directory.GetFiles(
				logFileDirectoryPath
				+ System.IO.Path.DirectorySeparatorChar,
				"*." + LogFile.LogFileExtensionName);
			//
			Assert.IsTrue(logFilePathes?.Length > 0);
			//

			var logFilePath = logFilePathes[0];
			var logFileContent = System.IO.File.ReadAllText(logFilePath);

			//
			Assert.IsTrue(logFileContent.Contains(logContentKeywords));
			//
			// !!!
			Assert.IsTrue(logFileContent.Contains(keyInLogContentToIgnoreLog) != true);
			// !!!

			//
			debugLogFile.FlushLogBuffer();
			Assert.IsTrue(debugLogFile.LogRecords.IsEmpty);
			//
		}

		[TestMethod]
		public void StresstTest()
		{
			var testLogRecordsCount = 1 * 10000;
			var debugLogDirectoryName = "测试_压力日志";
			var debugLogFile = new LogFile(debugLogDirectoryName, "debug");
			var tmpFileDirectoryPath
				= System.IO.Path.GetTempPath().ToFileSystemDirectoryPath();
			var logFilesDirectoryPath = tmpFileDirectoryPath + "测试日志文件夹";
			{
				LogFile.InitializeWithLogFilesDirectoryPath(
					  logFilesDirectoryPath,
					  () => 0.6,
					  () => 2 * 1024 * 1024,
					  () => 100,
					  () => 1);
			}
			logFilesDirectoryPath = LogFile.LogFilesDirectoryPath;
			if (System.IO.Directory.Exists(logFilesDirectoryPath))
			{
				System.IO.Directory.Delete(
					logFilesDirectoryPath,
					true);
			}

			System.Diagnostics.Debug.WriteLine(
				"#LogFileTest#，日志组件【压力测试】：开始 ，共测试 " + testLogRecordsCount + " 条日志，"
				+ "\r\n测试日志文件夹为：" + logFilesDirectoryPath);
			var testBeginTime = DateTime.Now;

			// !!!
			string logContentKeywords = "测试【调试】日志记录功能";
			for (var logNumber = 1;
				logNumber <= testLogRecordsCount;
				logNumber++)
			{
				var logContent = logContentKeywords + "，第【" + logNumber + "】条日志。";
				{ }
				debugLogFile.Logs(this, logContent);
			}
			// !!!

			// !!!⚠ 等待1秒钟后，检查日志文件 ⚠!!!
			var logFileDirectoryPath = logFilesDirectoryPath + debugLogDirectoryName;
			DirectoryInfo logFileDirectoryInfo;
			do
			{
				System.Threading.Thread.Sleep(2000);

				// !!!
				logFileDirectoryInfo = new DirectoryInfo(logFileDirectoryPath);
				// !!!
			} while (logFileDirectoryInfo.Exists == false);

			var logFilePathes = System.IO.Directory.GetFiles(
				logFileDirectoryPath
				+ System.IO.Path.DirectorySeparatorChar,
				"*." + LogFile.LogFileExtensionName);
			//
			Assert.IsTrue(logFilePathes?.Length > 0);
			//
			while (debugLogFile.LogRecords.IsEmpty != true)
			{
				System.Diagnostics.Debug.WriteLine(
					"#LogFileTest#，日志组件【压力测试】： 等待结束 ，已记录 " + debugLogFile.LogRecords.Count + " / " + testLogRecordsCount + " 条日志，"
					+ "\r\n已等待 " + (DateTime.Now - testBeginTime).TotalSeconds.ToString("F2") + " 秒，"
					+ "\r\n测试日志文件夹为：" + logFilesDirectoryPath);
			}
			System.Diagnostics.Debug.WriteLine(
				"#LogFileTest#，日志组件【压力测试】： 结束 ，共测试 " + testLogRecordsCount + " 条日志，"
				+ "\r\n已等待 " + (DateTime.Now - testBeginTime).TotalSeconds.ToString("F2") + " 秒，"
				+ "\r\n测试日志文件夹为：" + logFilesDirectoryPath);
		}
	}
}
