using System;

namespace BaoXia.Utils.Interfaces
{
        public class ILogFile
        {
                ////////////////////////////////////////////////
                // @自身属性
                ////////////////////////////////////////////////

                #region 自身属性

                public Utils.LogFile? LogFile { get; set; }

                /// <summary>
                /// 接收事件日志的方法，方法参数为：
                /// object? invoker，当前调用者对象；
                /// stirng? description，事件描述信息；
                /// object? object，事件信息参数对象；
                /// string？invokerName，当前调用者的名称。
                /// </summary>
                public Action<object?, string, object?, string?>? ToReceiveLogInfo { get; set; }

                #endregion


                ////////////////////////////////////////////////
                // @自身实现
                ////////////////////////////////////////////////

                #region 自身实现

                public ILogFile(
                        Utils.LogFile? logFile,
                        Action<object?, string, object?, string?>? toReceiveLogInfo = null)
                {
                        LogFile = logFile;
                        ToReceiveLogInfo = toReceiveLogInfo;
                }

                public ILogFile(Action<object?, string, object?, string?>? toReceiveLogInfo)
                {
                        ToReceiveLogInfo = toReceiveLogInfo;
                }

                public virtual void Logs(
                        object? invoker,
                        string description,
                        object? infoObject,
                        string? invokerFullName = null)
                {
                        this.LogFile?.Logs(invoker, description, infoObject, invokerFullName);
                        //
                        this.ToReceiveLogInfo?.Invoke(invoker, description, infoObject, invokerFullName);
                }

                #endregion
        }
}
