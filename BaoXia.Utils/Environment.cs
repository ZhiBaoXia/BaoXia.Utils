﻿using BaoXia.Utils.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using System.Text.Json;

namespace BaoXia.Utils
{
        public class Environment
        {
                ////////////////////////////////////////////////
                // @静态变量
                ////////////////////////////////////////////////

                #region 静态变量

                /// <summary>
                /// 当前服务器名称。
                /// </summary>
                public static string? ServerName { get; set; }

                /// <summary>
                /// 当前环境名称，可作为配置文件后缀等环境参数使用。
                /// </summary>
                public static string? EnviromentName { get; set; }

                /// <summary>
                /// 当前应用程序名称。
                /// </summary>
                public static string? ApplicationName { get; set; }

                /// <summary>
                /// 当前应用程序完整名称。
                /// </summary>
                public static string? ApplicationFullName { get; set; }

                /// <summary>
                /// 当前应用程序所在文件夹绝对路径。
                /// </summary>
                public static string? ApplicationDirectoryPath { get; set; }

                /// <summary>
                /// Web内容所在文件夹绝对路径。
                /// </summary>
                public static string? WebRootDirectoryPath { get; set; }

                protected static string? _configFilesDirectoryPath;

                /// <summary>
                /// 配置文件夹路径，可使用相对路径，或绝对路径。
                /// </summary>
                public static string? ConfigFilesDirectoryPath
                {
                        get
                        {
                                return _configFilesDirectoryPath;
                        }
                        set
                        {
                                _configFilesDirectoryPath = value;
                                //
                                var configFilesDirectoryPath = value;
                                if (configFilesDirectoryPath?.Length > 0
                                        && Environment.ApplicationDirectoryPath is string applicationDirectoryPath)
                                {
                                        configFilesDirectoryPath
                                                = configFilesDirectoryPath.ToAbsoluteFilePathInRootPath(
                                                        applicationDirectoryPath);
                                }
                                ConfigFile.InitializeWithConfigFilesDirectoryPath(
                                        configFilesDirectoryPath);
                                //
                        }
                }

                protected static string? _logFilesDirectoryPath;

                /// <summary>
                /// 日志文件夹路径，可使用相对路径，或绝对路径。
                /// </summary>
                public static string? LogFilesDirectoryPath
                {
                        get
                        {
                                return _logFilesDirectoryPath;
                        }
                        set
                        {
                                _logFilesDirectoryPath = value;
                                //
                                var logFilesDirectoryPath = value;
                                if (logFilesDirectoryPath?.Length > 0
                                        && Environment.ApplicationDirectoryPath is string applicationDirectoryPath)
                                {
                                        logFilesDirectoryPath
                                                = logFilesDirectoryPath.ToAbsoluteFilePathInRootPath(
                                                        applicationDirectoryPath);
                                }
                                LogFile.InitializeWithLogFilesDirectoryPath(
                                        logFilesDirectoryPath,
                                        LogFile.ToGetAutoFlushLogBufferIntervalSeconds,
                                        LogFile.ToGetMaxBytesCountPerLogFile,
                                        LogFile.ToGetLogRecordsCountPerFileWrite,
                                        LogFile.ToGetTimeoutSecondsToStorageLogRecords,
                                        LogFile.ToStorageLogRecords);
                                //
                        }
                }


                /// <summary>
                /// 当前应用程序。
                /// </summary>
                public static IApplicationBuilder? ApplicationBuilder { get; set; }

                public static IHostEnvironment? HostEnvironment { get; set; }

                /// <summary>
                /// 当前应用程序默认的AES加密密钥。
                /// </summary>
                public static string AESKeyDeafult { get; set; }
                        = "f62c315e3eb34b7db9f3d55b1bfd4488"
                        + "e28565fa9a8445b585cba0febc638d88"
                        + "f5bd0e80c5064ee49575c4e37531ee80"
                        + "058bf2804b774a2c8d6bbedf9fffee68";


                /// <summary>
                /// 全局的Json序列化选项。
                /// </summary>
                public static System.Text.Json.JsonSerializerOptions? JsonSerializerOptions { get; set; }

                #endregion


                ////////////////////////////////////////////////
                // @类方法
                ////////////////////////////////////////////////

                #region 类方法

                /// <summary>
                /// 在当前应用程序中初始化环境信息。
                /// </summary>
                /// <param name="serverName">当前服务器名称。</param>
                /// <param name="enviromentName">当前环境名称。</param>
                /// <param name="applicationBuilder">当前应用构建器。</param>
                /// <param name="hostEnvironment">当前主机环境信息。</param>
                /// <param name="webRootDirectoryPath">Web内容所在文件夹路径。</param>
                /// <param name="aesKeyDeafult">默认的AES加密密钥。</param>
                /// <param name="configFilesDirectoryPath">配置我呢见所在文件夹路径。</param>
                /// <param name="logFilesDirectoryPath">日志文件所在文件夹路径。</param>
                /// <param name="environmentInitializeParam">环境初始化的参数，详情可参见“EnvironmentInitializeParam”的定义。</param>
                public static void InitializeWithServerName(
                                string? serverName,
                                string? enviromentName,
                                //
                                IApplicationBuilder? applicationBuilder,
                                IHostEnvironment? hostEnvironment,
                                //
                                string? webRootDirectoryPath,
                                //
                                string aesKeyDeafult,
                                string configFilesDirectoryPath,
                                string logFilesDirectoryPath,
                                //
                                EnvironmentInitializeParam? environmentInitializeParam = null)
                {
                        Environment.ServerName = serverName;
                        Environment.EnviromentName = enviromentName;

                        Environment.ApplicationName = Assembly.GetExecutingAssembly().GetName().Name;
                        Environment.ApplicationFullName = Assembly.GetExecutingAssembly().GetName().ToString();

                        Environment.ApplicationDirectoryPath
                                = AppContext.BaseDirectory.ToFileSystemDirectoryPath();
                        Environment.WebRootDirectoryPath
                                = webRootDirectoryPath?.ToFileSystemDirectoryPath();

                        Environment.ApplicationBuilder = applicationBuilder;
                        Environment.HostEnvironment = hostEnvironment;

                        Environment.AESKeyDeafult = aesKeyDeafult;

                        if (environmentInitializeParam == null)
                        {
                                environmentInitializeParam = new EnvironmentInitializeParam();
                        }

                        ////////////////////////////////////////////////////////////////
                        /// !!! 优先配置Json序列化选项 !!!
                        var jsonSerializerOptions = environmentInitializeParam.JsonSerializerOptions;
                        if (jsonSerializerOptions == null)
                        {
                                jsonSerializerOptions = new System.Text.Json.JsonSerializerOptions()
                                {
                                        AllowTrailingCommas = true,
                                        WriteIndented = false,

                                        ReadCommentHandling = JsonCommentHandling.Skip,
                                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                                        PropertyNameCaseInsensitive = true,
                                };
                        }
                        Environment.JsonSerializerOptions = jsonSerializerOptions;

                        ////////////////////////////////////////////////////////////////

                        Environment.ConfigFilesDirectoryPath = configFilesDirectoryPath;

                        LogFile.InitializeWithLogFilesDirectoryPath(
                                logFilesDirectoryPath,
                                environmentInitializeParam.ToGetAutoFlushLogBufferIntervalSeconds,
                                environmentInitializeParam.ToGetMaxBytesCountPerLogFile,
                                environmentInitializeParam.ToGetLogRecordsCountPerFileWrite,
                                environmentInitializeParam.ToGetTimeoutSecondsToStorageLogRecords,
                                environmentInitializeParam.ToStorageLogRecords);
                        // !!!
                        Environment.LogFilesDirectoryPath = logFilesDirectoryPath;
                        // !!!

                        ////////////////////////////////////////////////////////////////
                }


                /// <summary>
                /// 在当前应用程序中初始化环境信息。
                /// </summary>
                /// <param name="serverName">当前服务器名称。</param>
                /// <param name="enviromentName">当前环境名称。</param>
                /// <param name="aesKeyDeafult">默认的AES加密密钥。</param>
                /// <param name="configFilesDirectoryPath">配置文件的文件夹路径。</param>
                /// <param name="logFilesDirectoryPath">日志文件的文件夹路径。</param>
                /// <param name="environmentInitializeParam">环境初始化的参数，详情可参见“EnvironmentInitializeParam”的定义。</param>
                public static void InitializeWithServerNameAtProgramRun(
                        string? serverName,
                        string? enviromentName,
                        string aesKeyDeafult,
                        string configFilesDirectoryPath,
                        string logFilesDirectoryPath,
                        //
                        EnvironmentInitializeParam? environmentInitializeParam = null)
                {
                        Environment.InitializeWithServerName(
                                   serverName,
                                   enviromentName,
                                   //
                                   null,
                                   null,
                                   //
                                   null,
                                   aesKeyDeafult,
                                   configFilesDirectoryPath,
                                   logFilesDirectoryPath,
                                   environmentInitializeParam);
                }

                /// <summary>
                /// 在当前应用程序中初始化环境信息。
                /// </summary>
                /// <param name="serverName">当前服务器名称。</param>
                /// <param name="enviromentName">当前环境名称。</param>
                /// <param name="applicationBuilder">当前应用构建器。</param>
                /// <param name="hostEnvironment">当前主机环境信息。</param>
                /// <param name="webRootDirectoryPath"></param>
                /// <param name="aesKeyDeafult">默认的AES加密密钥。</param>
                /// <param name="isDevelopment">是否是开发环境。</param>
                /// <param name="configFilesDirectoryPath"></param>
                /// <param name="logFilesDirectoryPath"></param>
                /// <param name="environmentInitializeParam">环境初始化的参数，详情可参见“EnvironmentInitializeParam”的定义。</param>
                public static void InitializeWithServerNameAtStartup(
                        string? serverName,
                        string? enviromentName,
                        //
                        IApplicationBuilder applicationBuilder,
                        IHostEnvironment? hostEnvironment,
                        //
                        string webRootDirectoryPath,
                        string aesKeyDeafult,
                        string configFilesDirectoryPath,
                        string logFilesDirectoryPath,
                        //
                        EnvironmentInitializeParam? environmentInitializeParam = null)
                {
                        Environment.InitializeWithServerName(
                                   serverName,
                                   enviromentName,
                                   //
                                   applicationBuilder,
                                   hostEnvironment,
                                   //
                                   webRootDirectoryPath,
                                   aesKeyDeafult,
                                   configFilesDirectoryPath,
                                   logFilesDirectoryPath,
                                   //
                                   environmentInitializeParam);
                }


                /// <summary>
                /// 在配置服务前初始化环境信息。
                /// </summary>
                /// <param name="serverName">当前服务器名称。</param>
                /// <param name="enviromentName">当前环境名称。</param>
                /// <param name="aesKeyDeafult">默认的AES加密密钥。</param>
                /// <param name="configFilesDirectoryPath">配置文件的文件夹路径。</param>
                /// <param name="logFilesDirectoryPath">日志文件的文件夹路径。</param>
                /// <param name="environmentInitializeParam">环境初始化的参数，详情可参见“EnvironmentInitializeParam”的定义。</param>
                public static void InitializeBeforeConfigureServicesWithServerName(
                        string? serverName,
                        string? enviromentName,
                        //
                        string aesKeyDeafult,
                        //
                        string configFilesDirectoryPath,
                        string logFilesDirectoryPath,
                        //
                        EnvironmentInitializeParam? environmentInitializeParam = null)
                {
                        Environment.InitializeWithServerName(
                                   serverName,
                                   enviromentName,
                                   null,
                                   null,
                                   null,
                                   aesKeyDeafult,
                                   configFilesDirectoryPath,
                                   logFilesDirectoryPath,
                                   //
                                   environmentInitializeParam);
                }

                /// <summary>
                /// 在配置服务后初始化环境信息。
                /// </summary>
                /// <param name="applicationBuilder">当前应用构建器。</param>
                /// <param name="hostEnvironment">当前主机环境信息。</param>
                /// <param name="webRootDirectoryPath"></param>
                public static void InitializeAfterConfigureServices(
                        IApplicationBuilder applicationBuilder,
                        IHostEnvironment? hostEnvironment,
                        string webRootDirectoryPath)
                {
                        Environment.InitializeWithServerName(
                                   Environment.ServerName,
                                   Environment.EnviromentName,
                                   applicationBuilder,
                                   hostEnvironment,
                                   webRootDirectoryPath,
                                   Environment.AESKeyDeafult,
                                   Environment.ConfigFilesDirectoryPath!,
                                   Environment.LogFilesDirectoryPath!,
                                   //
                                   new EnvironmentInitializeParam(
                                           LogFile.ToGetAutoFlushLogBufferIntervalSeconds,
                                           LogFile.ToGetMaxBytesCountPerLogFile,
                                           LogFile.ToGetLogRecordsCountPerFileWrite,
                                           LogFile.ToGetTimeoutSecondsToStorageLogRecords,
                                           LogFile.ToStorageLogRecords,
                                           Environment.JsonSerializerOptions));
                }


                /// <summary>
                /// 在非“Asp.Net Core”的环境中初始化工具集环境信息。
                /// </summary>
                /// <param name="enviromentName">当前环境名称。</param>
                /// <param name="aesKeyDeafult">默认的AES加密密钥。</param>
                /// <param name="configFilesDirectoryPath">配置文件的文件夹路径。</param>
                /// <param name="logFilesDirectoryPath">日志文件的文件夹路径。</param>
                /// <param name="environmentInitializeParam">环境初始化的参数，详情可参见“EnvironmentInitializeParam”的定义。</param>
                public static void InitializeWithEnviromentName(
                        string? enviromentName,
                        //
                        string aesKeyDeafult,
                        //
                        string configFilesDirectoryPath,
                        string logFilesDirectoryPath,
                        //
                        EnvironmentInitializeParam? environmentInitializeParam = null)
                {
                        Environment.InitializeBeforeConfigureServicesWithServerName(
                                null,
                                enviromentName,
                                aesKeyDeafult,
                                configFilesDirectoryPath,
                                logFilesDirectoryPath,
                                environmentInitializeParam);
                }


                /// <summary>
                /// 使用“ASP.Net Core”的环境参数“ASPNETCORE_ENVIRONMENT”，获取默认的环境名称。
                /// </summary>
                /// <returns>“ASP.Net Core”的环境参数“ASPNETCORE_ENVIRONMENT”，获取对应的环境名称。</returns>
                public static string? GetEnvironmentNameWith_ASPNETCORE_ENVIRONMENT()
                {
                        var aspNetCoreEnvironment = System.Environment.GetEnvironmentVariable(
                                "ASPNETCORE_ENVIRONMENT",
                                EnvironmentVariableTarget.Process);
                        if ("Development".EqualsIgnoreCase(aspNetCoreEnvironment))
                        {
                                return EnvironmentNameDefault.Development;
                        }
                        return EnvironmentNameDefault.Product;
                }

                /// <summary>
                /// 使用“ApplicationBuilder.ApplicationServices.GetService(ServiceType)”获取指定类型的服务实例对象。
                /// </summary>
                /// <param name="serviceType">指定的服务类型。</param>
                /// <returns>指定服务类型的服务实例对象。</returns>
                public static object? GetService(Type serviceType)
                {
                        if (ApplicationBuilder is not IApplicationBuilder app)
                        {
                                return null;
                        }

                        var service = app.ApplicationServices.GetService(serviceType);
                        { }
                        return service;
                }

                /// <summary>
                /// 使用“ApplicationBuilder.ApplicationServices.GetService<ServiceType>()”获取指定类型的服务实例对象。
                /// </summary>
                /// <typeparam name="ServiceType">指定的服务类型。</typeparam>
                /// <returns>指定服务类型的服务实例对象。</returns>
                public static ServiceType? GetService<ServiceType>()
                {
                        if (ApplicationBuilder is not IApplicationBuilder app)
                        {
                                return default;
                        }

                        var service = app.ApplicationServices.GetService<ServiceType>();
                        { }
                        return service;
                }

                #endregion
        }
}