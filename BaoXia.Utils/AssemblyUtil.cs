using System;
using System.Reflection;

namespace BaoXia.Utils
{
        public class AssemblyUtil
        {

                ////////////////////////////////////////////////
                // @类方法
                ////////////////////////////////////////////////

                #region 类方法

                /// <summary>
                /// 获取当前应用程序（入口）模块中所有的类型信息。
                /// </summary>
                /// <returns>当前应用程序（入口）模块中所有的类型信息。</returns>
                public static Type[]? GetAllTypesInEntryAssembly()
                {
                        var currentAssemblyTypes
                                = Assembly.GetEntryAssembly()?.GetTypes();
                        { }
                        return currentAssemblyTypes;
                }

                /// <summary>
                /// 获取当前正在运行的代码模块中所有的类型信息。
                /// </summary>
                /// <returns>当前正在运行的代码模块中所有的类型信息。</returns>
                public static Type[] GetAllTypesInCurrentAssembly()
                {
                        Type[] currentAssemblyTypes
                = Assembly.GetExecutingAssembly().GetTypes();
                        { }
                        return currentAssemblyTypes;
                }

                /// <summary>
                /// 获取当前正在运行的代码，被调用的模块中所有的类型信息。
                /// </summary>
                /// <returns>当前正在运行的代码，被调用的模块中所有的类型信息。</returns>
                public static Type[] GetAllTypesInCallingAssembly()
                {
                        Type[] currentAssemblyTypes
                                = Assembly.GetCallingAssembly().GetTypes();
                        { }
                        return currentAssemblyTypes;
                }

                #endregion


        }
}
