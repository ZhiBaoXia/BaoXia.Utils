using BaoXia.Utils.Models;
using System;
using System.Collections.Generic;

namespace BaoXia.Utils
{
        public class EnvironmentInitializeParam
        {
                ////////////////////////////////////////////////
                // @静态变量
                ////////////////////////////////////////////////

                #region 静态变量

                /// <summary>
                /// 日志记录，写入文件的时间间隔，默认为： 0.5 秒。
                /// </summary>
                public Func<double>? ToGetAutoFlushLogBufferIntervalSeconds { get; set; } = () => 0.5;

                /// <summary>
                /// 每个日志文件的最大尺寸，默认为： 2 MB。
                /// </summary>
                public Func<int>? ToGetMaxBytesCountPerLogFile { get; set; } = () => 1024 * 1024 * 2;

                /// <summary>
                /// 每次写入日志文件的最多日志条目数，默认为： 200 条。
                /// </summary>
                public Func<int>? ToGetLogRecordsCountPerFileWrite { get; set; } = () => 1000;

                /// <summary>
                /// 每次日志持久化的超时时间，默认为： 1 秒。
                /// </summary>
                public Func<double>? ToGetTimeoutSecondsToStorageLogRecords { get; set; } = () => 1.0;

                /// <summary>
                /// 自定义持久化的实现，当该值不为空时，日志组件不会写入记录文件。
                /// </summary>
                public Func<LogFile, IEnumerable<LogRecord>, bool>? ToStorageLogRecords = null;

                /// <summary>
                /// 全局默认的Json序列化选项，默认规则：
                /// 不保留缩进；
                /// 允许“//”开始的注释信息；
                /// 解析时，跳过注释信息；
                /// 属性名称，驼峰；
                /// 属性名称，不区分大小写。
                /// </summary>
                public System.Text.Json.JsonSerializerOptions? JsonSerializerOptions = null;

                #endregion


                ////////////////////////////////////////////////
                // @自身实现
                ////////////////////////////////////////////////

                #region 自身实现

                public EnvironmentInitializeParam()
                { }

                /// <summary>
                /// 环境初始化信息的默认参数。
                /// </summary>
                /// <param name="toGetAutoFlushLogBufferIntervalSeconds">日志记录，写入文件的时间间隔，默认为： 0.5 秒。</param>
                /// <param name="toGetMaxBytesCountPerLogFile">每个日志文件的最大尺寸，默认为： 2 MB。</param>
                /// <param name="toGetLogRecordsCountPerFileWrite">每次写入日志文件的最多日志条目数，默认为： 200 条。</param>
                /// <param name="toGetTimeoutSecondsToStorageLogRecords">每次日志持久化的超时时间，默认为： 1 秒。</param>
                /// <param name="toStorageLogRecords">自定义持久化的实现，当该值不为空时，日志组件不会写入记录文件。</param>
                /// <param name="jsonSerializerOptions"> 全局默认的Json序列化选项，默认规则：
                /// 不保留缩进；
                /// 允许“//”开始的注释信息；
                /// 解析时，跳过注释信息；
                /// 属性名称，驼峰；
                /// 属性名称，不区分大小写。
                /// </param>
                public EnvironmentInitializeParam(
                        Func<double>? toGetAutoFlushLogBufferIntervalSeconds = null,
                        Func<int>? toGetMaxBytesCountPerLogFile = null,
                        Func<int>? toGetLogRecordsCountPerFileWrite = null,
                        Func<double>? toGetTimeoutSecondsToStorageLogRecords = null,
                        Func<LogFile, IEnumerable<LogRecord>, bool>? toStorageLogRecords = null,
                        System.Text.Json.JsonSerializerOptions? jsonSerializerOptions = null)
                {
                        if (toGetAutoFlushLogBufferIntervalSeconds != null)
                        {
                                ToGetAutoFlushLogBufferIntervalSeconds = toGetAutoFlushLogBufferIntervalSeconds;
                        }

                        if (toGetMaxBytesCountPerLogFile != null)
                        {
                                ToGetMaxBytesCountPerLogFile = toGetMaxBytesCountPerLogFile;
                        }

                        if (toGetLogRecordsCountPerFileWrite != null)
                        {
                                ToGetMaxBytesCountPerLogFile = toGetLogRecordsCountPerFileWrite;
                        }

                        if (toGetTimeoutSecondsToStorageLogRecords != null)
                        {
                                ToGetTimeoutSecondsToStorageLogRecords = toGetTimeoutSecondsToStorageLogRecords;
                        }

                        if (toStorageLogRecords != null)
                        {
                                ToStorageLogRecords = toStorageLogRecords;
                        }

                        if (jsonSerializerOptions != null)
                        {
                                JsonSerializerOptions = jsonSerializerOptions;
                        }
                }

                #endregion
        }
}
