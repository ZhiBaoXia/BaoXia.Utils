using System;
using System.Collections.Generic;
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
		////////////////////////////////////////////////
		// 优先，检查并返回，常用的数据类型：
		////////////////////////////////////////////////
		if (baseValue == null
			|| baseValue.GetType().IsValueType != true)
		{
			return null;
		}

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
		if (baseValue is string stringValue)
		{
			return Encoding.UTF8.GetBytes(stringValue);
		}

		////////////////////////////////////////////////
		// 其次，检查并返回，不常用的数据类型：
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
			return new byte[] { byteValue };
		}

		if (baseValue is SByte sByteValue)
		{
			return new byte[] { (byte)sByteValue };
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

	/// <summary>
	/// 生成当前对象，所有值类型属性的字节数组。
	/// </summary>
	/// <param name="objectItem">当前对象。</param>
	/// <param name="propertyNamesExcepted">要排除生成的属性名称，大小写敏感。</param>
	/// <param name="propertiesBindingFlags">要读取的属性绑定标志，如果指定为“System.Reflection.BindingFlags.Default”，则默认使用“System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty”的组合。</param>
	/// <param name="propertyByteSeparator">属性的字节分隔符。</param>
	/// <returns>返回由当前对象，所有值类型属性生成的字节数组。</returns>
	public static byte[] GenerateBytesOfValueProperties(
		this object? objectItem,
		string[]? propertyNamesExcepted = null,
		System.Reflection.BindingFlags propertiesBindingFlags = System.Reflection.BindingFlags.Default,
		byte propertyByteSeparator = 0)
	{
		if (objectItem == null)
		{
			return Array.Empty<byte>();
		}
		if (propertiesBindingFlags == System.Reflection.BindingFlags.Default)
		{
			propertiesBindingFlags
				= System.Reflection.BindingFlags.Instance
				| System.Reflection.BindingFlags.Public
				| System.Reflection.BindingFlags.GetProperty;
		}

		var propertyInfes = objectItem.GetType().GetProperties(propertiesBindingFlags);
		if (propertyInfes.Length == 0)
		{
			return Array.Empty<byte>();
		}


		var memoryStream = new MemoryStream();
		using var binaryWriter = new BinaryWriter(memoryStream);
		{
			//memoryStream.SetLength(0);
			//binaryWriter.Seek(0, SeekOrigin.Begin);
		}
		foreach (var propertyInfo in propertyInfes)
		{
			if (propertyNamesExcepted?.Contains(propertyInfo.Name) == true)
			{
				continue;
			}

			var propertyValue = propertyInfo.GetValue(objectItem);
			var propertyValueBytes = ConvertBaseValueToBytes(propertyValue);
			if (propertyValueBytes == null)
			{
				continue;
			}

			// !!!
			binaryWriter.Write(propertyValueBytes);
			binaryWriter.Write(propertyByteSeparator);
			// !!!
		}
		var objectItemBytes = memoryStream.ToArray();
		{ }
		return objectItemBytes;
	}

	#endregion
}
