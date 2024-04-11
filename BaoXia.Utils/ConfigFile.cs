using BaoXia.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BaoXia.Utils
{
	public class ConfigFile : IDisposable
	{
		////////////////////////////////////////////////
		/// @静态常量
		////////////////////////////////////////////////

		public const string ConfigFileExtensionName = "json";

		public const int TryLoadsCountAtConfigFileContentChanged = 3;

		public string? ConfigFileSecondFileExtensionNameSpecifed { get; set; }


		static string? _configFilesDirectoryPath;

		static readonly List<ConfigFile> _configFiles = new();

		static readonly object _configFileConstructLocker = new();
		static bool _isConfigFileConstructWithAutoLoadEnable = true;
		protected class ConfigFileConstructWithAutoLoadDisabler : IDisposable
		{
			public ConfigFileConstructWithAutoLoadDisabler()
			{
				_isConfigFileConstructWithAutoLoadEnable = false;
			}

			public void Dispose()
			{
				_isConfigFileConstructWithAutoLoadEnable = true;
			}
		}

		/// <summary>
		/// 配置文件根目录路径。
		/// </summary>
		public static string? ConfigFilesDirectoryPath
		{
			get
			{
				return _configFilesDirectoryPath;
			}
		}

		////////////////////////////////////////////////
		/// @类方法
		////////////////////////////////////////////////

		/// <summary>
		/// 初始化配置文件系统。
		/// </summary>
		public static bool InitializeWithConfigFilesDirectoryPath(
			string? configFilesDirectoryPath)
		{
			if (configFilesDirectoryPath == null
				|| configFilesDirectoryPath.Length < 1)
			{
				return false;
			}

			_configFilesDirectoryPath
				= configFilesDirectoryPath.ToFileSystemDirectoryPath();
			if (!System.IO.Directory.Exists(_configFilesDirectoryPath))
			{
				System.IO.Directory.CreateDirectory(_configFilesDirectoryPath);
			}

			lock (_configFiles)
			{
				foreach (var configFile in _configFiles)
				{
					var configFilePath = configFile.FilePath;
					if (configFilePath == null
						|| configFilePath.Length < 1)
					{
						configFilePath
							= _configFilesDirectoryPath
							+ configFile.GetType().Name
							+ "."
							+ ConfigFileExtensionName;
						// !!!
						configFile.FilePath = configFilePath;
						// !!!
					}
				}
			}

			return true;
		}

		////////////////////////////////////////////////
		/// @自身属性
		////////////////////////////////////////////////

		protected System.IO.FileSystemWatcher? _fileWatcher;

		protected string? _filePath;

		/// <summary>
		/// 配置文件的绝对路径。
		/// </summary>
		[JsonIgnore]
		public string? FilePath
		{
			get
			{
				return _filePath;
			}

			set
			{
				var filePath = value?.Trim();

				if (StringUtil.EqualsStrings(
					_filePath,
					filePath,
					StringComparison.CurrentCultureIgnoreCase))
				{
					return;
				}

				if (_fileWatcher != null)
				{
					_fileWatcher.Dispose();
					_fileWatcher = null;
				}

				_filePath = value;

				var finalConfigFilePath = this.FinalFilePath;
				if (string.IsNullOrEmpty(finalConfigFilePath) == false)
				{
					var fileDirectoryPath = finalConfigFilePath.ToFileSystemDirectoryPath(true);
					var fileName = finalConfigFilePath.ToFileName();
					if (fileName?.Length > 0)
					{
						//
						_fileWatcher = new System.IO.FileSystemWatcher();
						{
							_fileWatcher.Path = fileDirectoryPath;
							_fileWatcher.Filter = fileName;
							_fileWatcher.NotifyFilter = NotifyFilters.LastWrite;

							// !!!
							_fileWatcher.Changed += this.DidConfigFileChanged;
							// !!!
						}
						_fileWatcher.EnableRaisingEvents = true;
					}
				}

				// !!!
				this.Load();
				// !!!
			}
		}


		/// <summary>
		/// 实际读写的文件绝对路径。
		/// </summary>
		public string? FinalFilePath
		{
			get
			{
				return GetFinalConfigFilePath();
			}
		}

		/// <summary>
		/// 配置文件发生变化时的要进行通知的事件链。
		/// </summary>
		protected List<Action<ConfigFile>>? _configFileChangedEvents;

		////////////////////////////////////////////////
		// @自身实现
		////////////////////////////////////////////////

		/// <summary>
		/// 默认构造函数。
		/// !!!⚠ 因为加载配置文件时，反序列化会自动调用默认的构造函数， ⚠!!!
		/// !!!⚠ 所以配置文件的默认构造函数不应执行一些重要的关联操作。 ⚠!!!
		/// </summary>
		public ConfigFile()
		{
			lock (_configFileConstructLocker)
			{
				if (_isConfigFileConstructWithAutoLoadEnable)
				{
					lock (_configFiles)
					{
						_configFiles.Add(this);
					}

					if (_configFilesDirectoryPath?.Length > 0)
					{
						var configFilePath
							= _configFilesDirectoryPath
							+ this.GetType().Name
							+ "."
							+ ConfigFileExtensionName;
						// !!!
						this.FilePath = configFilePath;
						// !!!
					}
				}
			}
		}

		/// <summary>
		/// 析构函数
		/// </summary>
		~ConfigFile()
		{
			if (_fileWatcher != null)
			{
				_fileWatcher.Dispose();
				_fileWatcher = null;
			}
			lock (_configFiles)
			{
				_configFiles.Remove(this);
			}
		}

		public string? GetFinalConfigFilePath()
		{
			var originalConfigFilePath = _filePath;
			string? configFileSecondFileExtensionNameSpecifed = null;
			if (string.IsNullOrEmpty(originalConfigFilePath)
				== false)
			{
				configFileSecondFileExtensionNameSpecifed
					= ConfigFileSecondFileExtensionNameSpecifed;
				if (string.IsNullOrEmpty(configFileSecondFileExtensionNameSpecifed))
				{
					var enviromentName = Environment.EnviromentName;
					if (string.IsNullOrEmpty(enviromentName)
						== false)
					{
						configFileSecondFileExtensionNameSpecifed
							= enviromentName;
					}
				}
			}
			return this.DidGetFinalConfigFilePath(
				originalConfigFilePath,
				configFileSecondFileExtensionNameSpecifed);
		}

		public bool Load()
		{
			var finalConfigFilePath = this.FinalFilePath;
			if (!string.IsNullOrEmpty(finalConfigFilePath)
				&& System.IO.File.Exists(finalConfigFilePath))
			{
				var newConfigJson = System.IO.File.ReadAllText(finalConfigFilePath);
				if (newConfigJson?.Length > 0)
				{
					lock (_configFileConstructLocker)
					{
						using var autoLoadDisabler = new ConfigFileConstructWithAutoLoadDisabler();
						using var newConfigFile
							= (ConfigFile?)newConfigJson.ToObjectByJsonDeserialize(this.GetType());
						if (newConfigFile != null)
						{
							this.SetPropertiesWithSameNameFrom(
								newConfigFile,
								"FilePath");
							// !!! 异步通知配置文件发生变化，避免在静态类初始化时触发相关事件， !!!
							// !!! 从而逻辑递归 !!!
							Task.Run(() =>
							{
								this.DidLoadConfigFileCompletedFromFilePath(
									 finalConfigFilePath,
									 this);
							});
							//
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool Save(
			bool isBackupLastFile,
			out string? configBackupFilePath)
		{
			//
			configBackupFilePath = null;
			//

			var finalConfigFilePath = this.FinalFilePath;
			if (string.IsNullOrEmpty(finalConfigFilePath))
			{
				return false;
			}

			// 1/2，备份旧的配置信息：
			if (isBackupLastFile
				&& System.IO.File.Exists(finalConfigFilePath))
			{
				var configDictionaryPath = finalConfigFilePath.ToFileSystemDirectoryPath(true);

				var configFileName = finalConfigFilePath.ToFileName(false);
				{
					var configBackupTimeStamp = DateTime.Now.ToString("Backup_yyyy_MM_dd hh_mm_ss");
					if (configFileName?.Length > 0)
					{
						configFileName += "_";
					}
					configFileName += configBackupTimeStamp;
				}
				var configFileExtensionName = finalConfigFilePath.ToFileExtensionName(true);

				// !!!⚠
				configBackupFilePath
					= FileExtension.CreateFilePathNotExistedBySameFileNameIndexWithDirectoryPath(
						configDictionaryPath,
						configFileName,
						configFileExtensionName);
				// !!!⚠

				// !!!
				if (configBackupFilePath != null)
				{
					var configJson
						= System.IO.File.ReadAllText(finalConfigFilePath);
					System.IO.File.WriteAllText(
						configBackupFilePath,
						configJson);
				}
				// !!!
			}

			// 2/2，写入新的配置信息：
			{
				var newConfigJson
					= StringExtension.StringByJsonSerialize(this);
				System.IO.File.WriteAllText(
					finalConfigFilePath,
					newConfigJson,
					Encoding.UTF8);

				// !!!
				this.DidSaveConfigFileCompletedToFilePath(
					finalConfigFilePath,
					this);
				// !!!
			}
			return true;
		}

		public bool Save()
		{
			return this.Save(false, out _);
		}

		public void AddConfigFileChangedEvent(Action<ConfigFile> configFileChangedEvent)
		{
			lock (this)
			{
				_configFileChangedEvents ??= new();
				if (!_configFileChangedEvents.Contains(configFileChangedEvent))
				{
					_configFileChangedEvents.Add(configFileChangedEvent);
				}
			}
		}

		public void RemoveConfigFileChangedEvent(Action<ConfigFile> configFileChangedEvent)
		{
			_configFileChangedEvents?.Remove(configFileChangedEvent);
		}

		////////////////////////////////////////////////
		/// @事件节点
		////////////////////////////////////////////////

		protected virtual string? DidGetFinalConfigFilePath(
			string? originalConfigFilePath,
			string? configFileSecondFileExtensionNameSpecifed)
		{
			var finalConfigFilePath = originalConfigFilePath;
			if (string.IsNullOrEmpty(finalConfigFilePath)
				== false
				&& string.IsNullOrEmpty(configFileSecondFileExtensionNameSpecifed)
				== false)
			{
				var fileDirectoryPath = finalConfigFilePath.ToFileSystemDirectoryPath(true);
				var fileName = finalConfigFilePath.ToFileName(false);
				var fileExtensionName = finalConfigFilePath.ToFileExtensionName();
				var filePathWithSecondFileExtensionNameSpecifed
					= fileDirectoryPath
					+ fileName
					+ "." + configFileSecondFileExtensionNameSpecifed
					+ "." + fileExtensionName;
				if (System.IO.File.Exists(filePathWithSecondFileExtensionNameSpecifed))
				{
					// !!!
					finalConfigFilePath = filePathWithSecondFileExtensionNameSpecifed;
					// !!!
				}
			}
			return finalConfigFilePath;
		}

		/// <summary>
		/// 配置文件内容发生变化时的事件节点。
		/// </summary>
		/// <param name="sender">事件发送者。</param>
		/// <param name="configFile">文件系统事件参数。</param>
		protected virtual void DidConfigFileChanged(
			object sender,
			FileSystemEventArgs fileSystemEventArgs)
		{
#if DEBUG
			var myType = this.GetType();
			var myName = myType.Namespace + "." + myType.Name;
			System.Diagnostics.Trace.WriteLine(myName + "，检测到配置文件内容发生变化：" + _filePath);
#endif
			for (var tryLoadIndex = 0;
				tryLoadIndex < ConfigFile.TryLoadsCountAtConfigFileContentChanged;
				tryLoadIndex++)
			{
				try
				{
					if (this.Load() == true)
					{
						break;
					}
				}
				catch
				{ }
			}
		}

		/// <summary>
		/// 成功加载配置文件的事件节点。
		/// </summary>
		/// <param name="filePath">加载配置文件所在的文件路径。</param>
		/// <param name="configFile">成功加载生成的配置文件对象。</param>
		protected virtual void DidLoadConfigFileCompletedFromFilePath(
			string filePath,
			ConfigFile configFile)
		{
			Action<ConfigFile>[]? configFileChangedEvents;
			lock (this)
			{
				configFileChangedEvents = _configFileChangedEvents?.ToArray();
			}
			if (configFileChangedEvents?.Length > 0)
			{
				foreach (var configFileChangedEvent in configFileChangedEvents)
				{
					try
					{
						configFileChangedEvent?.Invoke(this);
					}
					catch
					{ }
				}
			}
		}

		/// <summary>
		/// 成功保存配置文件的事件节点。
		/// </summary>
		/// <param name="filePath">保存配置文件所在的文件路径。</param>
		/// <param name="configFile">当前保存的配置文件对象。</param>
		protected virtual void DidSaveConfigFileCompletedToFilePath(
			string filePath,
			ConfigFile configFile)
		{ }

		////////////////////////////////////////////////
		/// @实现“IDisposable”
		////////////////////////////////////////////////

		public void Dispose()
		{
			if (_fileWatcher != null)
			{
				_fileWatcher.Dispose();
				_fileWatcher = null;
			}
			lock (_configFiles)
			{
				_configFiles.Remove(this);
			}

			////////////////////////////////////////////////
			GC.SuppressFinalize(this);
			////////////////////////////////////////////////
		}
	}
}