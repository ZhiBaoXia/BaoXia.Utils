using System;
using System.Collections.Generic;
using System.Reflection;

namespace BaoXia.Utils.Extensions;

public static class TypeListExtension
{
	////////////////////////////////////////////////
	// @类方法，删除非目标类型。
	////////////////////////////////////////////////

	#region 类方法，删除非目标类型。

	public static List<Type> RemoveTypesExceptAnyBaseClassSpecified(this List<Type> typeList,
		Type? orBaseClassSpecified, Type? orBaseInterfaceSpecified = null, Type? orCustomAttributeTypeSpecified = null)
	{
		if (orBaseClassSpecified == null && orBaseInterfaceSpecified == null && orCustomAttributeTypeSpecified == null)
		{
			return typeList;
		}
		typeList.NotRemoveIf((type) =>
		{
			if (orBaseClassSpecified != null && type.IsSubclassOf(orBaseClassSpecified))
			{
				return true;
			}
			if (orBaseInterfaceSpecified != null && type.IsAssignableTo(orBaseInterfaceSpecified))
			{
				return true;
			}
			if (orCustomAttributeTypeSpecified != null)
			{
				var customAttributeSpecifed = type.GetCustomAttribute(orCustomAttributeTypeSpecified);
				if (customAttributeSpecifed != null)
				{
					return true;
				}
			}
			return false;
		});
		return typeList;
	}

	public static List<Type> RemoveTypesExceptBaseClassSpecified(this List<Type> typeList, Type baseClassSpecified)
	{
		typeList.NotRemoveIf((type) =>
		{
			if (type.IsSubclassOf(baseClassSpecified))
			{
				return true;
			}
			return false;
		});
		return typeList;
	}

	public static List<Type> RemoveTypesExceptBaseInterfaceSpecified(this List<Type> typeList, Type baseInterfaceSpecified)
	{
		typeList.NotRemoveIf((type) =>
		{
			if (type.IsAssignableTo(baseInterfaceSpecified))
			{
				return true;
			}
			return false;
		});
		return typeList;
	}

	public static List<Type> RemoveTypesExceptCustomAttributeTypeSpecified(this List<Type> typeList, Type customAttributeTypeSpecified)
	{
		typeList.NotRemoveIf((type) =>
		{
			var customAttributeSpecifed = type.GetCustomAttribute(customAttributeTypeSpecified);
			if (customAttributeSpecifed != null)
			{
				return true;
			}
			return false;
		});
		return typeList;
	}

	#endregion


	////////////////////////////////////////////////
	// @类方法，获得目标类型。
	////////////////////////////////////////////////

	#region 类方法，获得目标类型。

	public static List<Type> ToTypesWithAnyBaseClassSpecified(this List<Type> typeList,
		Type? orBaseClassSpecified, Type? orBaseInterfaceSpecified = null, Type? orCustomAttributeTypeSpecified = null)
	{
		var targetTypes = new List<Type>();
		if (orBaseClassSpecified == null && orBaseInterfaceSpecified == null && orCustomAttributeTypeSpecified == null)
		{
			targetTypes.AddRange(typeList);
		}
		else
		{
			foreach (var type in typeList)
			{
				if (orBaseClassSpecified != null && type.IsSubclassOf(orBaseClassSpecified))
				{
					targetTypes.Add(type);
				}
				else if (orBaseInterfaceSpecified != null && type.IsAssignableTo(orBaseInterfaceSpecified))
				{
					targetTypes.Add(type);
				}
				else if (orCustomAttributeTypeSpecified != null)
				{
					var customAttributeSpecifed = type.GetCustomAttribute(orCustomAttributeTypeSpecified);
					if (customAttributeSpecifed != null)
					{
						targetTypes.Add(type);
					}
				}
			}
		}
		return targetTypes;
	}

	public static List<Type> ToTypesWithBaseClassSpecified(this List<Type> typeList, Type baseClassSpecified)
	{
		var targetTypes = new List<Type>();
		foreach (var type in typeList)
		{
			if (type.IsSubclassOf(baseClassSpecified))
			{
				targetTypes.Add(type);
			}
		}
		return targetTypes;
	}

	public static List<Type> ToTypesWithBaseInterfaceSpecified(this List<Type> typeList, Type baseInterfaceSpecified)
	{
		var targetTypes = new List<Type>();
		foreach (var type in typeList)
		{
			if (type.IsAssignableTo(baseInterfaceSpecified))
			{
				targetTypes.Add(type);
			}
		}
		return targetTypes;
	}

	public static List<Type> ToTypesWithCustomAttributeTypeSpecified(this List<Type> typeList, Type customAttributeTypeSpecified)
	{
		var targetTypes = new List<Type>();
		foreach (var type in typeList)
		{
			var customAttributeSpecifed = type.GetCustomAttribute(customAttributeTypeSpecified);
			if (customAttributeSpecifed != null)
			{
				targetTypes.Add(type);
			}
		}
		return targetTypes;
	}

	#endregion
}
