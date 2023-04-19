using BaoXia.Utils.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace BaoXia.Utils
{
        public abstract class MiddlewareAsync : IMiddleware
        {
                ////////////////////////////////////////////////
                // @自身属性
                ////////////////////////////////////////////////

                #region 自身属性			

                public ILogFile? LogFile { get; set; }

                public ILogFile? ExceptionLogFile { get; set; }

                public ILogFile? WarningLogFile { get; set; }

                #endregion


                ////////////////////////////////////////////////
                // @自身实现
                ////////////////////////////////////////////////

                #region 自身实现

                public MiddlewareAsync(
                        ILogFile? logFile,
                        ILogFile? exceptionLogFile,
                        ILogFile? warningLogFile)
                {
                        LogFile = logFile;
                        ExceptionLogFile = exceptionLogFile;
                        WarningLogFile = warningLogFile;
                }

                #endregion


                ////////////////////////////////////////////////
                // @事件节点
                ////////////////////////////////////////////////

                #region 事件节点

                protected abstract Task<bool> DidIfGoToNextMiddlewareByInvokeAsync(HttpContext httpContext);

                #endregion


                ////////////////////////////////////////////////
                // @实现“Middleware”定义方法
                ////////////////////////////////////////////////

                #region 实现“Middleware”

                public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
                {
                        ////////////////////////////////////////////////
                        // 1/2，业务代码：
                        ////////////////////////////////////////////////
                        try
                        {
                                if (await this.DidIfGoToNextMiddlewareByInvokeAsync(httpContext) != true)
                                {
                                        return;
                                }
                        }
                        catch (Exception exception)
                        {
                                ExceptionLogFile?.Logs(this, "中间件（异步）处理请求失败，程序异常。", exception);
                                return;
                        }


                        ////////////////////////////////////////////////
                        // 2/2，继续响应通道：
                        ////////////////////////////////////////////////
                        // !!!
                        if (next != null)
                        {
                                await next(httpContext);
                        }
                        // !!!
                }

                #endregion
        }
}
