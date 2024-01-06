using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;

namespace BaoXia.Utils.Extensions;

/// <summary>
/// Object 扩展类。
/// </summary>
public static class ObjectExtension
{

	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static PropertyInfo[]? GetPublicSetablePropertyInfes(
	    this object @object,
	    BindingFlags propertiesBindingFlags = BindingFlags.Default)
	{
		var objectType = @object.GetType();

		if (propertiesBindingFlags == BindingFlags.Default)
		{
			propertiesBindingFlags
				= BindingFlags.Instance
				| BindingFlags.Public
				| BindingFlags.SetProperty;
		}
		var objectSetableProperties = objectType.GetProperties(propertiesBindingFlags);
		{ }
		return objectSetableProperties;
	}

	public static object? GetPropertyValueWithName(
	    this object @object,
	    string? propertyName)
	{
		if (string.IsNullOrEmpty(propertyName))
		{
			return null;
		}

		var propertyTypeInfo = @object.GetType().GetProperty(propertyName);
		var propertyValue = propertyTypeInfo?.GetValue(@object);
		{ }
		return propertyValue;
	}


	public static string? GetPropertyNameWithPropertyValue(
		this object @object,
		object? propertyObject)
	{
		if (propertyObject == null)
		{
			return null;
		}

		var objectType = @object.GetType();
		var objectTypeProperties = objectType.GetProperties();
		string? propertyName = null;
		foreach (var objectTypeProperty in objectTypeProperties)
		{
			var objectProperty = objectTypeProperty.GetValue(@object);
			if (objectProperty == propertyObject)
			{
				// !!!
				propertyName = objectTypeProperty.Name;
				break;
				// !!!
			}
		}
		return propertyName;
	}

	/// <summary>
	/// 设置当前对象（objectA）中与指定对象（objectB）同名的属性值。
	/// </summary>
	/// <param name="objectA">当前对象。</param>
	/// <param name="objectB">指定的对象。</param>
	/// <param name="propertyNamesExcepted">要排除设置的属性名称，大小写敏感。</param>
	/// <param name="propertiesBindingFlags">要设置属性绑定标志，默认为：
	/// System.Reflection.BindingFlags.Public
	/// | System.Reflection.BindingFlags.Instance</param>
	public static void SetPropertiesWithSameNameFrom(
	    this object objectA,
	    object objectB,
	    string[] propertyNamesExcepted,
	    System.Reflection.BindingFlags propertiesBindingFlags)
	{
		if (objectA == null
		    || objectB == null)
		{
			return;
		}

		if (propertiesBindingFlags == BindingFlags.Default)
		{
			propertiesBindingFlags
			    = BindingFlags.Instance
			    | BindingFlags.Public
			    | BindingFlags.SetProperty;
		}


		var objectAProperties = objectA.GetType().GetProperties(
		    propertiesBindingFlags);
		if (objectAProperties == null
		    || objectAProperties.Length < 1)
		{
			return;
		}

		var objectBProperties = objectB.GetType().GetProperties(
		    propertiesBindingFlags);
		if (objectBProperties == null
		    || objectBProperties.Length < 1)
		{
			return;
		}

		if (propertyNamesExcepted?.Length > 0)
		{
			foreach (var propertyA in objectAProperties)
			{
				if (propertyA.CanWrite)
				{
					var propertyAName = propertyA.Name;
					if (Array.IndexOf(propertyNamesExcepted, propertyAName) < 0)
					{
						var propertyAType = propertyA.PropertyType;
						PropertyInfo? propertyAInObjectB = null;
						foreach (var propertyB in objectBProperties)
						{
							if (propertyAName.Equals(propertyB.Name))
							{
								if (propertyAType.Equals(propertyB.PropertyType))
								{
									// !!!
									propertyAInObjectB = propertyB;
									// !!!
								}
								break;
							}
						}

						if (propertyAInObjectB != null)
						{
							propertyA.SetValue(objectA, propertyAInObjectB.GetValue(objectB));
						}
					}
				}
			}
		}
		else
		{
			foreach (var propertyA in objectAProperties)
			{
				if (propertyA.CanWrite)
				{
					var propertyAName = propertyA.Name;
					var propertyAType = propertyA.PropertyType;
					PropertyInfo? propertyAInObjectB = null;
					foreach (var propertyB in objectBProperties)
					{
						if (propertyAName.Equals(propertyB.Name))
						{
							if (propertyAType.Equals(propertyB.PropertyType))
							{
								// !!!
								propertyAInObjectB = propertyB;
								// !!!
							}
							break;
						}
					}

					if (propertyAInObjectB != null)
					{
						propertyA.SetValue(objectA, propertyAInObjectB.GetValue(objectB));
					}
				}
			}
		}
	}

	/// <summary>
	/// 设置当前对象（objectA）中与指定对象（objectB）同名的属性值。
	/// </summary>
	/// <param name="objectA">当前对象。</param>
	/// <param name="objectB">指定的对象。</param>
	/// <param name="propertyNamesExcepted">要排除设置的属性名称，大小写敏感。</param>
	public static void SetPropertiesWithSameNameFrom(
	    this object objectA,
	    object objectB,
	    params string[] propertyNamesExcepted)
	{
		ObjectExtension.SetPropertiesWithSameNameFrom(
		    objectA,
		    objectB,
		    propertyNamesExcepted,
		    System.Reflection.BindingFlags.Default);
	}


	/// <summary>
	/// 通过设置同名属性，克隆产生新对象。
	/// </summary>
	/// <typeparam name="ObjectType">当前对象类型。</typeparam>
	/// <param name="currentObject">当前对象。</param>
	/// <returns>拥有相同属性的，克隆产生的新对象。</returns>
	public static ObjectType CloneWithSameProperties<ObjectType>(this ObjectType currentObject)
		where ObjectType
		: class, new()
	{
		var newObject = new ObjectType();
		{
			newObject.SetPropertiesWithSameNameFrom((object)currentObject);
		}
		return newObject;
	}


	public static ObjectType[] CloneObjectsToArray<ObjectType>(this ICollection<ObjectType> objects)
		where ObjectType : class, new()
	{
		var objectArray = new ObjectType[objects.Count];
		var objectIndex = 0;
		foreach (var obj in objects)
		{
			if (objectIndex < objectArray.Length)
			{
				objectArray[objectIndex] = obj.CloneWithSameProperties();
			}
			objectIndex++;
		}
		return objectArray;
	}

	public static List<ObjectType> CloneObjectsToList<ObjectType>(this ICollection<ObjectType> objects)
		where ObjectType : class, new()
	{
		var objectList = new List<ObjectType>();
		foreach (var obj in objects)
		{
			objectList.Add(obj.CloneWithSameProperties());
		}
		return objectList;
	}

	/// <summary>
	/// 将当前对象序列化为Json字符串。
	/// </summary>
	/// <typeparam name="ObjectType">当前对象类型。</typeparam>
	/// <param name="item">当前对象。</param>
	/// <returns>对象序列化产生的Json字符串。</returns>
	public static string ToJsonString<ObjectType>(
		this ObjectType item,
		JsonSerializerOptions? jsonSerializerOptions = null)
	{
		var jsonString = StringExtension.StringByJsonSerialize(
			item,
			jsonSerializerOptions);
		{ }
		return jsonString;
	}

	#endregion
}
