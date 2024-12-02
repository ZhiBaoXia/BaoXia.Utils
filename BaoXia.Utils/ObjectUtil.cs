using System;

namespace BaoXia.Utils;

public static class ObjectUtil
{

	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static object? CreateObject(Type objectType)
	{
		if (objectType.Equals(typeof(string)))
		{
			return string.Empty;
		}
		var @object = Activator.CreateInstance(objectType);
		{ }
		return @object;
	}

	public static ObjectType? CreateObject<ObjectType>()
	{
		return (ObjectType?)CreateObject(typeof(ObjectType));
	}

	public static object? CreateObject(object? @object)
	{
		if (@object == null)
		{
			return @object;
		}
		if (@object is string stringObject)
		{
			return new string(stringObject);
		}
		{
			@object = Activator.CreateInstance(@object.GetType());
		}
		return @object;
	}


	#endregion
}
