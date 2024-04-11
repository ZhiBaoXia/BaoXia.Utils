using BaoXia.Utils.Constants;
using BaoXia.Utils.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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

	public static ObjectType? CreateObject<ObjectType>()
	{
		var @object = Activator.CreateInstance<ObjectType>();
		{ }
		return @object;
	}

	public static object? CreateObject(Type objectType)
	{
		var @object = Activator.CreateInstance(objectType);
		{ }
		return @object;
	}

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


	public static ObjectType[]? CloneObjectsToArray<ObjectType>(this IEnumerable<ObjectType>? objects)
		where ObjectType : class, new()
	{
		if (objects == null)
		{
			return null;
		}

		var objectArray = new ObjectType[objects.GetCount()];
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

	public static List<ObjectType>? CloneObjectsToList<ObjectType>(this IEnumerable<ObjectType>? objects)
		where ObjectType : class, new()
	{
		if (objects == null)
		{
			return null;
		}

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

	/// <summary>
	/// 基本类型数据转为字节数组。
	/// </summary>
	/// <param name="baseValue">任意对象。</param>
	/// <returns>如果任意对象是基本数据类型，则将基本类型数据转为字节数组。</returns>
	public static byte[]? ConvertBaseValueToBytes(object? baseValue)
	{
		if (baseValue == null)
		{
			return null;
		}

		////////////////////////////////////////////////
		// 1/2，优先，检查并返回，常用的【值类型】：
		////////////////////////////////////////////////
		if (baseValue is string stringValue)
		{
			return Encoding.UTF8.GetBytes(stringValue);
		}
		else if (baseValue.GetType().IsValueType == false)
		{
			if (baseValue is IEnumerable items)
			{
				var itemsEnumerator = items.GetEnumerator();
				if (itemsEnumerator.MoveNext() == false)
				{
					return null;
				}
				var firstItem = itemsEnumerator.Current;
				var itemType = firstItem.GetType();
					@last
			}
			return null;
		}
		//
		if (baseValue is Boolean booleanValue)
		{
			return BitConverter.GetBytes(booleanValue);
		}
		if (baseValue is Int32 int32Value)
		{
			return BitConverter.GetBytes(int32Value);
		}
		if (baseValue is Single singleValue)
		{
			return BitConverter.GetBytes(singleValue);
		}
		if (baseValue is Double doubleValue)
		{
			return BitConverter.GetBytes(doubleValue);
		}
		if (baseValue is DateTime dateTimeValue)
		{
			return BitConverter.GetBytes(dateTimeValue.Ticks);
		}

		////////////////////////////////////////////////
		// 2/2，优先，检查并返回，不常用的【值类型】：
		////////////////////////////////////////////////

		if (baseValue is Int16 int16Value)
		{
			return BitConverter.GetBytes(int16Value);
		}
		//if (baseValue is Int32 int32Value)
		//{
		//	return BitConverter.GetBytes(int32Value);
		//}
		if (baseValue is Int64 int64Value)
		{
			return BitConverter.GetBytes(int64Value);
		}

		if (baseValue is UInt16 uInt16Value)
		{
			return BitConverter.GetBytes(uInt16Value);
		}
		if (baseValue is UInt32 uInt32Value)
		{
			return BitConverter.GetBytes(uInt32Value);
		}
		if (baseValue is UInt64 uInt64Value)
		{
			return BitConverter.GetBytes(uInt64Value);
		}
		if (baseValue is Byte byteValue)
		{
			return [byteValue];
		}

		if (baseValue is SByte sByteValue)
		{
			return [(byte)sByteValue];
		}
		//if (baseValue is Single singleValue)
		//{
		//	return BitConverter.GetBytes(singleValue);
		//}
		//if (baseValue is Double doubleValue)
		//{
		//	return BitConverter.GetBytes(doubleValue);
		//}
		if (baseValue is Decimal decimalValue)
		{
			return Decimal.GetBits(decimalValue).SelectMany(BitConverter.GetBytes).ToArray();
		}

		if (baseValue is Char charValue)
		{
			return BitConverter.GetBytes(charValue);
		}

		//if (baseValue is Boolean booleanValue)
		//{
		//	return BitConverter.GetBytes(booleanValue);
		//}

		//if (baseValue is DateTime dateTimeValue)
		//{
		//	return BitConverter.GetBytes(dateTimeValue.Ticks);
		//}

		//if (baseValue is Guid guidValue)
		//{
		//	return guidValue.ToByteArray();
		//}

		//if (baseValue is string stringValue)
		//{
		//	return Encoding.UTF8.GetBytes(stringValue);
		//}

		//if (baseValue is byte[] byteArrayValue)
		//{
		//	return byteArrayValue;
		//}

		//if (baseValue is Array arrayValue)
		//{
		//	var memoryStream = new MemoryStream();
		//	using var binaryWriter = new BinaryWriter(memoryStream);
		//	foreach (var arrayItem in arrayValue)
		//	{
		//		var arrayItemBytes = ConvertBaseValueToBytes(arrayItem);
		//		if (arrayItemBytes == null)
		//		{
		//			return null;
		//		}
		//		// !!!
		//		binaryWriter.Write(arrayItemBytes);
		//		// !!!
		//	}
		//	return memoryStream.ToArray();
		//}
		return null;
	}

	public static List<ItemPropertyGetInfo<object>>? GetPropertyGetInfesOfItem(
		object? item,
		string[]? propertyNamesExcepted = null,
		System.Reflection.BindingFlags propertiesBindingFlags = System.Reflection.BindingFlags.Default,
		Func<object, PropertyInfo, bool>? toIsPropertyInfoOfObjectValidToGenerate = null)
	{
		if (item == null)
		{
			return null;
		}
		var itemType = item.GetType();
		if (itemType.IsValueType)
		{
			return [];
		}
		// 字符串对象的特殊处理。
		if (itemType.Equals(typeof(string)))
		{
			return [];
		}

		// 容器对象的特殊处理。
		if (item is IEnumerable childItems)
		{
			var chilItemPropertyGetInfes = new List<ItemPropertyGetInfo<object>>();
			foreach (var childItem in childItems)
			{
				if (childItem.GetType().IsValueType)
				{
					return [];
				}
				else
				{
					chilItemPropertyGetInfes.Add(new ItemPropertyGetInfo<object>(
						ItemPropertyGetInfoType.ObjectItemInIEnumerable,
						item,
						null,
						childItem));
				}
			}
			return chilItemPropertyGetInfes;
		}

		// 普通的对象属性。
		if (propertiesBindingFlags == System.Reflection.BindingFlags.Default)
		{
			propertiesBindingFlags
				= System.Reflection.BindingFlags.Instance
				| System.Reflection.BindingFlags.Public
				| System.Reflection.BindingFlags.GetProperty;
		}
		var itemPropertyInfes = itemType.GetProperties(propertiesBindingFlags);
		if (itemPropertyInfes == null
		|| itemPropertyInfes.Length < 1)
		{
			return [];
		}
		var itemPropertyGetInfes = new List<ItemPropertyGetInfo<object>>();
		foreach (var hostItemPropertyInfo in itemPropertyInfes)
		{
			if (propertyNamesExcepted?.Contains(hostItemPropertyInfo.Name) == true)
			{
				continue;
			}
			if (toIsPropertyInfoOfObjectValidToGenerate?.Invoke(
				item,
				hostItemPropertyInfo) == false)
			{
				continue;
			}
			itemPropertyGetInfes.Add(new(
				ItemPropertyGetInfoType.NormalProperty,
				item,
				hostItemPropertyInfo,
				null));
		}
		return itemPropertyGetInfes;
	}

	/// <summary>
	/// 生成当前对象，所有值类型属性的字节数组。
	/// </summary>
	/// <param name="objectItem">当前对象。</param>
	/// <param name="propertyNamesExcepted">要排除生成的属性名称，大小写敏感。</param>
	/// <param name="propertiesBindingFlags">要读取的属性绑定标志，如果指定为“System.Reflection.BindingFlags.Default”，则默认使用“System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty”的组合。</param>
	/// <param name="propertyByteSeparator">属性的字节分隔符。</param>
	/// <param name="isGetPropertyInfesRecursivly">是否使用递归深度获取对象的属性信息。。</param>
	/// <param name="toIsPropertyInfoOfObjectValidToGenerate">判断指定对象，指定属性是否参与字节生成的回调。</param>
	/// <returns>返回由当前对象，所有值类型属性生成的字节数组。</returns>
	public static byte[] GenerateBytesOfValueProperties(
		this object? objectItem,
		string[]? propertyNamesExcepted = null,
		System.Reflection.BindingFlags propertiesBindingFlags = System.Reflection.BindingFlags.Default,
		byte propertyByteSeparator = 0,
		bool isGetPropertyInfesRecursivly = false,
		Func<object, PropertyInfo, bool>? toIsPropertyInfoOfObjectValidToGenerate = null)
	{
		if (objectItem == null)
		{
			return [];
		}


		var memoryStream = new MemoryStream();
		using var binaryWriter = new BinaryWriter(memoryStream);
		////////////////////////////////////////////////

		var objectItemPropertyGetInfes = GetPropertyGetInfesOfItem(
			objectItem,
			propertyNamesExcepted,
			propertiesBindingFlags,
			toIsPropertyInfoOfObjectValidToGenerate);
		if (objectItemPropertyGetInfes == null
			|| objectItemPropertyGetInfes.Count < 1)
		{

		}
		else
		{
			foreach (var objectItemPropertyGetInfo in objectItemPropertyGetInfes)
			{
				RecursionUtil.RecursionEnumerate<ItemPropertyGetInfo<object>>(
					objectItemPropertyGetInfo,
					(itemPropertyGetInfo) =>
					{
						return GetPropertyGetInfesOfItem(
							itemPropertyGetInfo.GetPropertyOfHostItem(),
							propertyNamesExcepted,
							propertiesBindingFlags,
							toIsPropertyInfoOfObjectValidToGenerate);
					},
					(parentItemPropertyGetInfo, itemPropertyGetInfo) =>
					{
						var itemPropertyValue
						= itemPropertyGetInfo.GetPropertyOfHostItem();
						var itemPropertyValueBytes = ConvertBaseValueToBytes(itemPropertyValue);
						if (itemPropertyValueBytes != null)
						{
							// !!!
							binaryWriter.Write(itemPropertyValueBytes);
							binaryWriter.Write(propertyByteSeparator);
							// !!!
						}
						return true;
					});
			}
		}
		var objectItemPropertyInfes = objectItem.GetType().GetProperties(propertiesBindingFlags);
		if (isGetPropertyInfesRecursivly)
		{
			foreach (var objectItemPropertyInfo in objectItemPropertyInfes)
			{
				if (propertyNamesExcepted?.Contains(objectItemPropertyInfo.Name) == true)
				{
					continue;
				}
				if (toIsPropertyInfoOfObjectValidToGenerate?.Invoke(
					objectItem,
					objectItemPropertyInfo) == false)
				{
					continue;
				}

				RecursionUtil.RecursionEnumerate<ItemPropertyGetInfo<object>>(
					new(ItemPropertyGetInfoType.NormalProperty,
						objectItem,
						objectItemPropertyInfo,
						null),
					(itemPropertyGetInfo) =>
					{

					},
					(parentItemPropertyGetInfo, itemPropertyGetInfo) =>
					{
						var itemPropertyValue
						= itemPropertyGetInfo.PropertyInfo.GetValue(itemPropertyGetInfo.HosttItem);

						var itemPropertyValueBytes = ConvertBaseValueToBytes(itemPropertyValue);
						if (itemPropertyValueBytes != null)
						{
							// !!!
							binaryWriter.Write(itemPropertyValueBytes);
							binaryWriter.Write(propertyByteSeparator);
							// !!!
						}
						return true;
					});
			}
		}
		else
		{
			foreach (var objectItemPropertyInfo in objectItemPropertyInfes)
			{
				if (propertyNamesExcepted?.Contains(objectItemPropertyInfo.Name) == true)
				{
					continue;
				}
				if (toIsPropertyInfoOfObjectValidToGenerate?.Invoke(
					objectItem,
					objectItemPropertyInfo) == false)
				{
					continue;
				}

				var itemPropertyValue = objectItemPropertyInfo.GetValue(objectItem);
				var itemPropertyValueBytes = ConvertBaseValueToBytes(itemPropertyValue);
				if (itemPropertyValueBytes != null)
				{
					// !!!
					binaryWriter.Write(itemPropertyValueBytes);
					binaryWriter.Write(propertyByteSeparator);
					// !!!
				}
			}
		}
		////////////////////////////////////////////////
		var objectItemBytes = memoryStream.ToArray();
		{ }
		return objectItemBytes;
	}

	#endregion
}
