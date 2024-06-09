using BaoXia.Utils.Extensions;
using System;
using System.Collections.Generic;
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

		/// <summary>
		/// 获取当前当前应用程序中所有的类型信息。
		/// </summary>
		/// <param name="applicationAssembly">使用”Assembly.GetExecutingAssembly()“指定的当前应用程序程序集，之所以需要指定，是为了避免在程序包管理器控制台，执行EF命令等操作时，无法扫描到当前项目中的服务。。</param>
		/// <param name="baseClassSpecified">指定要获取类型的要继承的基类，默认为“null”，不指定。</param>
		/// <param name="baseInterfaceSpecified">指定要获取类型的要实现的接口，默认为“null”，不指定。</param>
		/// <param name="customAttributeTypeSpecified">指定要获取类型的要拥有的特性，默认为“null”，不指定。</param>
		/// <param name="isIncludeTypesInEntryAssembly">是否包含“应用程序的入口点的程序集”中的类型。</param>
		/// <param name="isIncludeTypesInCurrentAssembly">是否包含“当前正在执行的代码的程序集”中的类型。</param>
		/// <param name="isIncludeTypesInCallingAssembly">是否包含“当前正在执行的代码的程序集”中的类型。</param>
		/// <returns>当前当前应用程序中所有的类型信息。</returns>
		public static List<Type> GetAllTypesInApplicationAssembly(
			Assembly? applicationAssembly,
			Type? baseClassSpecified = null,
			Type? baseInterfaceSpecified = null,
			Type? customAttributeTypeSpecified = null,
			bool isIncludeTypesInCurrentAssembly = true,
			bool isIncludeTypesInEntryAssembly = true,
			bool isIncludeTypesInCallingAssembly = true)
		{
			var allTypesInApplicationAssembly = new List<Type>();

			////////////////////////////////////////////////
			if (applicationAssembly != null)
			{
				var types = applicationAssembly.GetTypes();
				if (types.Length > 0)
				{
					allTypesInApplicationAssembly.AddRange(types);

				}
			}
			////////////////////////////////////////////////
			Assembly? entryAssembly = null;
			if (isIncludeTypesInEntryAssembly)
			{
				entryAssembly = Assembly.GetEntryAssembly();
				if (entryAssembly != null
					&& entryAssembly != applicationAssembly)
				{
					var types = entryAssembly.GetTypes();
					if (types.Length > 0)
					{
						allTypesInApplicationAssembly.AddRange(types);
					}
				}
			}
			////////////////////////////////////////////////
			Assembly? executingAssembly = null;
			if (isIncludeTypesInCurrentAssembly)
			{
				executingAssembly = Assembly.GetExecutingAssembly();
				if (executingAssembly != null
				&& executingAssembly != applicationAssembly
				&& executingAssembly != entryAssembly)
				{
					var types = executingAssembly.GetTypes();
					if (types.Length > 0)
					{
						allTypesInApplicationAssembly.AddRange(types);
					}
				}
			}
			////////////////////////////////////////////////
			if (isIncludeTypesInCallingAssembly)
			{
				var callingAssembly = Assembly.GetCallingAssembly();
				if (callingAssembly != null
				&& callingAssembly != applicationAssembly
				&& callingAssembly != entryAssembly
				&& callingAssembly != executingAssembly)
				{
					var types = callingAssembly.GetTypes();
					if (types.Length > 0)
					{
						allTypesInApplicationAssembly.AddRange(types);
					}
				}
			}
			////////////////////////////////////////////////

			// !!!
			allTypesInApplicationAssembly.RemoveTypesExceptBaseClassSpecified(
				baseClassSpecified,
				baseInterfaceSpecified,
				customAttributeTypeSpecified);
			// !!!

			return allTypesInApplicationAssembly;
		}

		#endregion
	}
}
