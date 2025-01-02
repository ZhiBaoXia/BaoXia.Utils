using BaoXia.Utils.Extensions;
using System;
using System.Reflection;

namespace BaoXia.Utils.Models;

public class ObjectPropertyInfo
    (int id,
    PropertyInfo propertyInfo,
    ObjectPropertyInfo[] childEntityPropertyInfes)
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public int Id { get; set; } = id;

	public PropertyInfo PropertyInfo { get; set; } = propertyInfo;

	public Type PropertyType => PropertyInfo.PropertyType;

	public bool IsPropertyTypeValue => PropertyType.IsValueType;

	public bool IsPropertyTypeString => PropertyType.Equals(typeof(string));

	public bool IsPropertyTypeCollection => PropertyType.IsCollectible; //.IsAssignableTo(typeof(System.Collections.ICollection));

	public bool IsPropertyTypeNoneChildProperties =>
		IsPropertyTypeValue
		|| IsPropertyTypeString
		|| IsPropertyTypeCollection;

	public string Name => PropertyInfo.Name;

	public string PropertyLinkName
	{
		get
		{
			var propertyLinkName = Name;
			for (var parentObjectPropertyInfo = ParentObjectPropertyInfo;
				parentObjectPropertyInfo != null;
				parentObjectPropertyInfo = parentObjectPropertyInfo.ParentObjectPropertyInfo)
			{
				propertyLinkName = parentObjectPropertyInfo.Name + "." + propertyLinkName;
			}
			return propertyLinkName;
		}
	}

	public ObjectPropertyInfo? ParentObjectPropertyInfo { get; set; }

	public ObjectPropertyInfo[] ChildObjectPropertyInfes { get; protected set; } = childEntityPropertyInfes;

	#endregion


	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public bool IsPropertyTypeNullable(NullabilityInfoContext? nullabilityInfoContext)
	{
		nullabilityInfoContext ??= new();
		var propertyNullableInfo = nullabilityInfoContext.Create(PropertyInfo);
		if (propertyNullableInfo.WriteState == NullabilityState.Nullable)
		{
			return true;
		}
		return false;
	}

	public object? GetValue(object? entity)
	{
		return PropertyInfo.GetValue(entity);
	}

	public PropertyInfo[] GetProperties(BindingFlags propertyBindingFlags = BindingFlags.Public | BindingFlags.Instance)
	{
		return PropertyInfo.PropertyType.GetProperties(propertyBindingFlags);
	}

	public ObjectPropertyInfo[] AddChildObjectPropertyInfo(ObjectPropertyInfo? childObjectPropertyInfo)
	{
		if (childObjectPropertyInfo != null)
		{
			childObjectPropertyInfo.ParentObjectPropertyInfo?.RemoveChildObjectPropertyInfo(childObjectPropertyInfo);
			//
			childObjectPropertyInfo.ParentObjectPropertyInfo = this;
			ChildObjectPropertyInfes = ChildObjectPropertyInfes.ArrayByAdd(childObjectPropertyInfo);
			//
		}
		return ChildObjectPropertyInfes;
	}

	public ObjectPropertyInfo[] RemoveChildObjectPropertyInfo(ObjectPropertyInfo? childObjectPropertyInfo)
	{
		if (childObjectPropertyInfo != null)
		{
			//
			childObjectPropertyInfo.ParentObjectPropertyInfo = null;
			ChildObjectPropertyInfes = ChildObjectPropertyInfes.ArrayByRemove(childObjectPropertyInfo);
			//
		}
		return ChildObjectPropertyInfes;
	}

	#endregion
}