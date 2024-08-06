using BaoXia.Utils.Extensions;
using BaoXia.Utils.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils;

public class LogFile : IDisposable
{
	////////////////////////////////////////////////
	// @静态常量
	////////////////////////////////////////////////

	/// <summary>
	/// 日志文件扩展名。
	/// </summary>
	public const string LogFileExtensionName = "log.html";

	/// <summary>
	/// 日志文件根目录。
	/// </summary>
	public static string? LogFilesDirectoryPath { get; set; }


	/// <summary>
	/// 自动重新清理日志文件“内存缓存”的时间间隔，的默认值：0.5秒。
	/// </summary>
	public const double AutoFlushLogBufferIntervalSecondsDefault = 0.5F;

	/// <summary>
	/// 获取自动重新清理日志文件“内存缓存”的时间间隔，单位为：秒，默认为：0.5秒。
	/// </summary>
	public static Func<double>? ToGetAutoFlushLogBufferIntervalSeconds { get; set; }

	/// <summary>
	/// 单个日志文件最多字节数，的默认值：1MB。
	/// </summary>
	public const long MaxBytesCountPerLogFileDefault = (1024 * 1024);

	/// <summary>
	/// 单个日志文件最多字节数。
	/// </summary>
	public static Func<long>? ToGetMaxBytesCountPerLogFile { get; set; }

	/// <summary>
	/// 日志文件写入日志记录数量的单位数量。
	/// </summary>
	public static Func<int>? ToGetLogRecordsCountPerFileWrite { get; set; } = null;

	/// <summary>
	/// 日志文件写入的最大超时时间。
	/// </summary>
	public static Func<double>? ToGetTimeoutSecondsToStorageLogRecords { get; set; } = null;

	/// <summary>
	/// 日志持久化回调节点。
	/// </summary>
	public static Func<LogFile, IEnumerable<LogRecord>, bool>? ToStorageLogRecords { get; set; } = null;

	/// <summary>
	/// 全局日志文件对象集合。
	/// </summary>
	protected static readonly List<LogFile> _logFiles = [];

	public static DateTime LastFlushLogBufferBeginTime { get; set; }

	public static DateTime LastFlushLogBufferEndTime { get; set; }

	public static DateTime LastFlushLogBufferExceptionTime { get; set; }

	public static bool IsFlushLogBufferTaskCancelled { get; set; }

	/// <summary>
	/// 定时清空日志缓存的线程任务。
	/// </summary>
	protected static readonly LoopTask _autoFlushLogBufferTask = new(
		(CancellationToken cancellationToken) =>
		{
			try
			{
				// !!!
				LogFile.IsFlushLogBufferTaskCancelled = false;
				LogFile.LastFlushLogBufferBeginTime = DateTime.Now;
				// !!!

				if (cancellationToken.IsCancellationRequested == true)
				{
					// !!!
					LogFile.IsFlushLogBufferTaskCancelled = true;
					// !!!
					return false;
				}

				LogFile[] logFiles;
				lock (_logFiles)
				{
					logFiles = [.. _logFiles];
				}

				if (logFiles?.Length > 0)
				{
					foreach (var logFile in logFiles)
					{
						if (cancellationToken.IsCancellationRequested == true)
						{
							// !!!
							LogFile.IsFlushLogBufferTaskCancelled = true;
							// !!!
							return false;
						}
						logFile.FlushLogBuffer(false);
					}
				}

				// !!!
				LogFile.LastFlushLogBufferEndTime = DateTime.Now;
				// !!!
			}
			catch
			{
				// !!!
				LogFile.LastFlushLogBufferExceptionTime = DateTime.Now;
				// !!!
			}
			finally
			{ }
			return true;
		},
		LogFile.ToGetAutoFlushLogBufferIntervalSeconds,
		false);


	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	/// <summary>
	/// 使用“日志名称”和“记录日志的时间”创建日志文件文件名。
	/// </summary>
	/// <param name="logName">日志名称。</param>
	/// <param name="logDateTime">记录日志的时间。</param>
	/// <param name="logFileIndex">日志文件索引数。</param>
	/// <returns>日志文件文件名。</returns>
	public static string CreateLogFileNameWithLogName(
		string? logName,
		DateTime logDateTime,
		int logFileIndex = 0)
	{
		var fileName
			= logName?.Length > 0
			? (logName + "_")
			: "";
		{
			fileName += logDateTime.ToString("yyyy_MM_dd");
			if (logFileIndex > 0)
			{
				fileName += "_" + (logFileIndex + 1);
			}
		}
		fileName += "." + LogFile.LogFileExtensionName;

		return fileName;
	}

	/// <summary>
	/// 开启定时任务，定时清空日志缓存。
	/// </summary>
	protected static void StartTaskToFlushLogBuffer()
	{
		// !!!
		var toGetAutoFlushLogBufferIntervalSeconds
			= LogFile.ToGetAutoFlushLogBufferIntervalSeconds;
		toGetAutoFlushLogBufferIntervalSeconds ??= () =>
			{
				return LogFile.AutoFlushLogBufferIntervalSecondsDefault;
			};
		_autoFlushLogBufferTask.ToDidGetIntervalSeconds = toGetAutoFlushLogBufferIntervalSeconds;
		_autoFlushLogBufferTask.Start();
		// !!!
	}

	/// <summary>
	/// 使用“日志文件根目录”初始化日志文件系统。
	/// </summary>
	/// <param name="logFilesDirectoryPath">指定的日志文件根目录。</param>
	/// <param name="toGetAutoFlushLogBufferIntervalSeconds">获取日志写入文件的时间间隔秒数。</param>
	/// <param name="toGetMaxBytesCountPerLogFile">单个日志文件最多字节数。</param>
	/// <param name="toGetLogRecordsCountPerFileWrite">每次写入文件的日志记录条数，默认为：不限制。</param>
	/// <param name="toGetTimeoutSecondsToStorageLogRecords">获取日志文件写入的超时，默认为：永久。</param>
	/// <param name="toStorageLogRecords ">日志文件存储方法，为空时，默认写入日志文件。</param>
	public static void InitializeWithLogFilesDirectoryPath(
		string? logFilesDirectoryPath,
		Func<double>? toGetAutoFlushLogBufferIntervalSeconds,
		Func<long>? toGetMaxBytesCountPerLogFile,
		Func<int>? toGetLogRecordsCountPerFileWrite,
		Func<double>? toGetTimeoutSecondsToStorageLogRecords,
		Func<LogFile, IEnumerable<LogRecord>, bool>? toStorageLogRecords = null)
	{
		LogFile.ToGetAutoFlushLogBufferIntervalSeconds = toGetAutoFlushLogBufferIntervalSeconds;
		/// 
		LogFile.ToGetMaxBytesCountPerLogFile = toGetMaxBytesCountPerLogFile;
		LogFile.ToGetLogRecordsCountPerFileWrite = toGetLogRecordsCountPerFileWrite;
		LogFile.ToGetTimeoutSecondsToStorageLogRecords = toGetTimeoutSecondsToStorageLogRecords;
		LogFile.ToStorageLogRecords = toStorageLogRecords;

		LogFile.LogFilesDirectoryPath = logFilesDirectoryPath?.ToFileSystemDirectoryPath();

		// !!!
		LogFile.StartTaskToFlushLogBuffer();
		// !!!
	}

	/// <summary>
	/// 获取全部日志内容缓冲长度。
	/// </summary>
	public static int GetAllLogRecordsCountInLogBuffer()
	{
		LogFile[] logFiles;
		lock (_logFiles)
		{
			logFiles = [.. _logFiles];
		}

		var allLogRecordsCountInLogBuffer = 0;
		if (logFiles?.Length > 0)
		{
			foreach (var logFile in logFiles)
			{
				allLogRecordsCountInLogBuffer += logFile.LogRecords.Count;
			}
		}
		return allLogRecordsCountInLogBuffer;
	}

	/// <summary>
	/// 清空全部日志内容缓冲。
	/// </summary>
	public static void ClearAllLogBuffer()
	{
		LogFile[] logFiles;
		lock (_logFiles)
		{
			logFiles = [.. _logFiles];
		}

		if (logFiles?.Length > 0)
		{
			foreach (var logFile in logFiles)
			{
				logFile.ClearLogBuffer();
			}
		}
	}


	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////


	/// <summary>
	/// 日志类型。
	/// </summary>
	public string? Type { get; set; }

	/// <summary>
	/// 日志名称。
	/// </summary>
	public string? Name { get; set; } = "";

	/// <summary>
	/// 日志内容缓冲。
	/// </summary>
	protected readonly ConcurrentQueue<LogRecord> _logRecords = new();

	public ConcurrentQueue<LogRecord> LogRecords
	{
		get
		{
			return _logRecords;
		}
	}

	/// <summary>
	/// 忽略包含在日志内容中的关键字的日志记录。
	/// </summary>
	public string[]? KeysInLogContentToIgnoreLogs { get; set; }

	/// <summary>
	/// 日志文件上一次需要写入的内容，当文件写入失败时，会进行临时保存，写入成功时，会清空该值。
	/// </summary>
	//protected readonly List<LogRecord> _logRecordsNeedWriteToFileBuffer = new();

	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////


	/// <summary>
	/// 使用“日志根目录路径”，“日志类型”和“日志名称”构造日志文件对象。
	/// </summary>
	/// <param name="type">日志类型，用于日志文件所在文件夹名称。</param>
	/// <param name="name">日志名称，用于日志文件名称前缀。</param>
	public LogFile(
		string? type,
		//
		string? name = null)
	{
		this.Type = type;

		this.Name = name;

		// !!!
		lock (_logFiles)
		{
			_logFiles.Add(this);
		}
		// !!!
	}

	/// <summary>
	/// 析构函数
	/// </summary>
	~LogFile()
	{
		this.Dispose();
	}

	/// <summary>
	/// 创建当前日志文件夹路径。
	/// </summary>
	/// <returns>当前日志文件夹路径。</returns>
	public string? CreateLogDirectoryPath()
	{
		var logDirectoryPath = LogFile.LogFilesDirectoryPath;
		if (logDirectoryPath?.Length > 0
			&& this.Type?.Length > 0)
		{
			logDirectoryPath += this.Type.ToFileSystemDirectoryPath();
		}
		return logDirectoryPath;
	}

	/// <summary>
	/// 使用指定的日志时间，创建对应的日志文件名。
	/// </summary>
	/// <param name="dateTime">指定的日志时间。</param>
	/// <param name="logFileIndex">日志文件索引数。</param>
	/// <returns>日志文件名。</returns>
	public string CreateLogFileNameForDateTime(
		DateTime dateTime,
		int logFileIndex = 0)
	{
		var logFileName
			= LogFile.CreateLogFileNameWithLogName(
			this.Name,
			dateTime,
			logFileIndex);
		{ }
		return logFileName;
	}

	/// <summary>
	/// 使用指定的日志时间，创建对应的日志文件路径。
	/// </summary>
	/// <param name="dateTime">指定的日志时间。</param>
	/// <returns>日志文件路径。</returns>
	public string? CreateLogFilePathForDateTime(
		DateTime dateTime,
		int logFileIndex = 0)
	{
		string? logFilePath = null;
		{
			var logDirectoryPath
				= this.CreateLogDirectoryPath();
			if (logDirectoryPath?.Length > 0)
			{
				var logFileName = this.CreateLogFileNameForDateTime(dateTime, logFileIndex);
				if (logFileName?.Length > 0)
				{
					logFilePath = logDirectoryPath + logFileName;
				}
			}
		}
		return logFilePath;
	}

	/// <summary>
	/// 清空日志内容缓冲到日志文件。
	/// </summary>
	/// <param name="isClearBufferOnly">是否只是清空缓存。</param>
	public void FlushLogBuffer(bool isClearBufferOnly = false)
	{
		var logRecordsCountNeedWriteToFile = _logRecords.Count;
		if (logRecordsCountNeedWriteToFile < 1)
		{
			return;
		}

		var newLogRecordsNeedWriteToFile = new List<LogRecord>();
		while (_logRecords.TryDequeue(out var logRecord)
			&& logRecordsCountNeedWriteToFile > 0)
		{
			newLogRecordsNeedWriteToFile.Add(logRecord);
			//
			logRecordsCountNeedWriteToFile--;
			//
		}

		if (isClearBufferOnly)
		{
			return;
		}

		if (newLogRecordsNeedWriteToFile.Count > 0)
		{
			var timeoutSecondsToStorageLogRecords = 0.0;
			var toGetTimeoutSecondsToStorageLogRecords
				= LogFile.ToGetTimeoutSecondsToStorageLogRecords;
			if (toGetTimeoutSecondsToStorageLogRecords != null)
			{
				timeoutSecondsToStorageLogRecords
					= toGetTimeoutSecondsToStorageLogRecords();
			}

			try
			{
				using var timeoutCancellationTokenSource
					= timeoutSecondsToStorageLogRecords > 0.0
					? new CancellationTokenSource()
					: null;
				// !!!
				timeoutCancellationTokenSource?.CancelAfter(
					(int)(1000.0 * timeoutSecondsToStorageLogRecords));
				// !!!

				////////////////////////////////////////////////
				// !!!
				this.DidTryStorageLogRecords(newLogRecordsNeedWriteToFile);
				// !!!
				////////////////////////////////////////////////
			}
			catch (TaskCanceledException)
			{
				// !!! 避免内存泄露 !!!
				newLogRecordsNeedWriteToFile.Clear();
				// !!!
			}
		}
	}

	/// <summary>
	/// 清空日志内容缓冲。
	/// </summary>
	public void ClearLogBuffer()
	{
		this.FlushLogBuffer(true);
	}

	/// <summary>
	/// 记录日志信息。
	/// </summary>
	/// <param name="invoker">调用者</param>
	/// <param name="logFileContent">日志内容</param>
	/// <param name="logParamObject">日志内容，对象类型参数。</param>
	/// <param name="invokerFullName">调用者名称，适用于静态方法，由开发者手动输入调用者名称。</param>
	public void Logs(
		object? invoker,
		string logContent,
		object? logContentParamObject = null,
		string? invokerFullName = null)
	{
		if ((logContent == null
			|| logContent.Length < 1)
		&& logContentParamObject == null)
		{
			return;
		}

		logContent ??= string.Empty;
		if (logContentParamObject != null)
		{
			logContent
				+= "\r\n"
				+ logContentParamObject.ToString();
		}

		if (logContent?.Length > 0)
		{
			var keysInLogContentToIgnoreLogs
				= this.KeysInLogContentToIgnoreLogs;
			if (keysInLogContentToIgnoreLogs?.Length > 0)
			{
				for (var keyToIgnoreLogIndex = keysInLogContentToIgnoreLogs.Length - 1;
					keyToIgnoreLogIndex >= 0;
					keyToIgnoreLogIndex--)
				{
					var keyToIgnoreLog = keysInLogContentToIgnoreLogs[keyToIgnoreLogIndex];
					if (keyToIgnoreLog?.Length > 0)
					{
						if (logContent.IndexOfIgnoreCase(keyToIgnoreLog) >= 0)
						{
							return;
						}
					}
				}
			}
		}

		invokerFullName ??= invoker?.GetType()?.FullName;


#if DEBUG
		var logName = this.Name ?? string.Empty;
		var now = DateTime.Now;
		var timestamp = now.MillisecondsFrom1970();
		var timestampCaption = now.ToString("yyyy_MM_dd hh:mm:ss ms");
		System.Diagnostics.Debug.WriteLine(
			"\r\n"
			+ logName
			+ " "
			+ invokerFullName
			+ " "
			+ timestampCaption
			+ "\r\n"
			+ logContent);
#endif


		var newLogRecord = new LogRecord()
		{
			LogTime = DateTime.Now,
			Invoker = invokerFullName,
			Content = logContent
		};
		_logRecords.Enqueue(newLogRecord);
	}

	////////////////////////////////////////////////
	// @事件节点
	////////////////////////////////////////////////

	protected bool DidTryStorageLogRecords(List<LogRecord> logRecordsNeedStorage)
	{
		if (logRecordsNeedStorage.Count < 1)
		{
			return true;
		}

		var didStorageLogRecords = LogFile.ToStorageLogRecords;
		if (didStorageLogRecords != null)
		{
			return didStorageLogRecords(this, logRecordsNeedStorage);
		}


		// 将日志内容写入文件：
		var logDirectoryPath = this.CreateLogDirectoryPath();
		if (string.IsNullOrEmpty(logDirectoryPath))
		{
			return false;
		}
		System.IO.Directory.CreateDirectory(logDirectoryPath);

		DateTime firstLogRecordLogTime = logRecordsNeedStorage[0].LogTime;
		var logFilePath = this.CreateLogFilePathForDateTime(firstLogRecordLogTime);
		if (string.IsNullOrEmpty(logFilePath))
		{
			return false;
		}

		//var logFileInfo = new FileInfo(logFilePath);
		//var isLogFileExisted
		//    = logFileInfo.Exists == true
		//    && logFileInfo.Length > 0;
		//if (isLogFileExisted
		//    && logFileInfo.Length > LogFile.MaxBytesCountPerLogFile)
		var serverName = Environment.ServerName;
		while (logRecordsNeedStorage.Count > 0)
		{
			var logFileContent = string.Empty;
			var logRecordsCountPerFileWrite = 0;
			var toGetLogRecordsCountPerFileWrite = LogFile.ToGetLogRecordsCountPerFileWrite;
			if (toGetLogRecordsCountPerFileWrite != null)
			{
				logRecordsCountPerFileWrite = toGetLogRecordsCountPerFileWrite();
			}
			if (logRecordsCountPerFileWrite <= 0)
			{
				logRecordsCountPerFileWrite = logRecordsNeedStorage.Count;
			}
			for (var logRecordIndex = 0;
				logRecordIndex < logRecordsCountPerFileWrite;
				logRecordIndex++)
			{
				if (logRecordsNeedStorage.Count <= 0)
				{
					break;
				}
				var logRecord = logRecordsNeedStorage[0];
				{
					logRecordsNeedStorage.RemoveAt(0);
				}

				var logTime = logRecord.LogTime;
				var logTimestamp = logTime.MillisecondsFrom1970();
				var logTimestampCaption = logTime.ToString("yyyy_MM_dd HH:mm:ss fff");

				if (logFileContent.Length > 0)
				{
					logFileContent += "\r\n";
				}
				logFileContent
				+= "<log type=\"log\" serverName=\"" + serverName + "\">"
				+ "\r\n\t<time logTimestamp=\"" + logTimestamp + "\">" + logTimestampCaption + "</time>"
				+ "\r\n\t<invoker>" + logRecord.Invoker + "</invoker>"
				+ "\r\n\t<content>" + (logRecord.Content.StringByDecodeInHtmlEscape() ?? string.Empty) + "</content>"
				+ "\r\n</log>";
			}


			var logFileIndex = 0;
			FileInfo logFileInfo;
			var maxBytesCountPerLogFile = LogFile.MaxBytesCountPerLogFileDefault;
			var toGetMaxBytesCountPerLogFile = LogFile.ToGetMaxBytesCountPerLogFile;
			if (toGetMaxBytesCountPerLogFile != null)
			{
				maxBytesCountPerLogFile = toGetMaxBytesCountPerLogFile();
			}
			if (maxBytesCountPerLogFile <= 0)
			{
				maxBytesCountPerLogFile = LogFile.MaxBytesCountPerLogFileDefault;
			}
			do
			{
				// !!!
				logFilePath = this.CreateLogFilePathForDateTime(
					firstLogRecordLogTime,
					logFileIndex);
				if (string.IsNullOrEmpty(logFilePath))
				{
					return false;
				}
				logFileInfo = new FileInfo(logFilePath);
				// !!!
				logFileIndex++;
				// !!!
			} while (logFileInfo.Exists
			&& logFileInfo.Length > maxBytesCountPerLogFile);

			if (logFileInfo.Exists)
			{
				File.AppendAllText(
					logFilePath,
					logFileContent);
			}
			else
			{
				var fileName = logFilePath.ToFileName();
				logFileContent = LogFileTemplateInHtml.GetHtmlWithServerName(
					Environment.ServerName,
					this.Type,
					fileName)
					+ "\r\n"
					+ "\r\n"
					+ logFileContent;

				File.WriteAllText(
					logFilePath,
					logFileContent);
			}
		}
		return true;
	}

	////////////////////////////////////////////////
	// @实现“IDisposable”
	////////////////////////////////////////////////

	public void Dispose()
	{
		lock (_logFiles)
		{
			_logFiles.Remove(this);
		}

		////////////////////////////////////////////////
		GC.SuppressFinalize(this);
		////////////////////////////////////////////////
	}
}