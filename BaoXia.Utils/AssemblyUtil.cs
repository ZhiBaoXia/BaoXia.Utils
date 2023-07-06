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
		/// <param name="baseClassSpecified">指定要获取类型的要继承的基类，默认为“null”，不指定。</param>
		/// <param name="baseInterfaceSpecified">指定要获取类型的要实现的接口，默认为“null”，不指定。</param>
		/// <param name="customAttributeTypeSpecified">指定要获取类型的要拥有的特性，默认为“null”，不指定。</param>
		/// <param name="isIncludeTypesInEntryAssembly">是否包含“应用程序的入口点的程序集”中的类型。</param>
		/// <param name="isIncludeTypesInCurrentAssembly">是否包含“当前正在执行的代码的程序集”中的类型。</param>
		/// <returns>当前当前应用程序中所有的类型信息。</returns>
		public static List<Type> GetAllTypesInApplicationAssembly(
			Type? baseClassSpecified = null,
			Type? baseInterfaceSpecified = null,
			Type? customAttributeTypeSpecified = null,
			bool isIncludeTypesInCurrentAssembly = true,
			bool isIncludeTypesInEntryAssembly = true)
		{
			var allTypesInApplicationAssembly = new List<Type>();
			////////////////////////////////////////////////
			if (isIncludeTypesInEntryAssembly)
			{
				var allTypesInEntryAssembly
					= BaoXia.Utils.AssemblyUtil.GetAllTypesInEntryAssembly();
				if (allTypesInEntryAssembly != null)
				{
					allTypesInApplicationAssembly.AddRange(allTypesInEntryAssembly);
				}
			}

			if (isIncludeTypesInCurrentAssembly)
			{
				var allTypesInCurrentAssembly
					= BaoXia.Utils.AssemblyUtil.GetAllTypesInCurrentAssembly();
				if (allTypesInCurrentAssembly != null)
				{
					allTypesInApplicationAssembly.AddRange(allTypesInCurrentAssembly);
				}
			}
			////////////////////////////////////////////////

			// !!!
			allTypesInApplicationAssembly.RemoveTypesWithBaseClassSpecified(
				baseClassSpecified,
				baseInterfaceSpecified,
				customAttributeTypeSpecified);
			// !!!

			return allTypesInApplicationAssembly;
		}

		#endregion


	}
}
