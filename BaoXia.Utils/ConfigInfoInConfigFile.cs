using System;
using System.Reflection;

namespace BaoXia.Utils;

public class ConfigInfoInConfigFile<ConfigType> where ConfigType : class
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	private readonly ConfigType _defaultConfig;

	private readonly ConfigFile? _configFile;

	private readonly string? _configPropertyName;

	private readonly PropertyInfo? _configPropertyInfoInConfigFile;

	public ConfigType Config
	{
		get
		{
			ConfigType? serviceConfig = null;
			if (_configFile != null
				&& _configPropertyInfoInConfigFile != null)
			{
				serviceConfig
					= _configPropertyInfoInConfigFile.GetValue(_configFile)
					as ConfigType;
			}
			serviceConfig ??= _defaultConfig;
			return serviceConfig;
		}
	}

	#endregion

	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public ConfigInfoInConfigFile(
		ConfigFile? configFile,
		string? configPropertyName,
		//
		ConfigType defaultConfig,
		//
		Action<ConfigFile>? toReceiveConfigFileChanged = null)
	{
		_configFile = configFile;
		_configPropertyName = configPropertyName;
		if (_configFile != null
			&& !string.IsNullOrEmpty(_configPropertyName))
		{
			var configPropertyInfoInConfigFile
				= _configFile
				.GetType()
				.GetProperty(_configPropertyName);
			if (configPropertyInfoInConfigFile == null)
			{
				var configPropertyNotFoundException
					= new ArgumentException(
						$"无法在配置文件中，使用名称“{configPropertyName}”找到“{typeof(ConfigType).Name}”类型的配置信息。",
						nameof(configPropertyName));
				{
					configPropertyNotFoundException.Data.Add(
						nameof(configFile),
						configFile);
					configPropertyNotFoundException.Data.Add(
						nameof(configPropertyName),
						configPropertyName);
				}
				throw configPropertyNotFoundException;
			}
			_configPropertyInfoInConfigFile = configPropertyInfoInConfigFile;

			if (toReceiveConfigFileChanged != null)
			{
				_configFile.AddConfigFileChangedEvent(toReceiveConfigFileChanged);
			}
		}
		// !!!
		_defaultConfig = defaultConfig;
		// !!!
	}

	#endregion
}