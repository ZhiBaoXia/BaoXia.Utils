using BaoXia.Utils.Constants;
using BaoXia.Utils.Models;
using System;
using System.Collections;
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

	public static object? CreateObject(Type objectType)
	{
		return ObjectUtil.CreateObject(objectType);
	}

	public static ObjectType? CreateObject<ObjectType>()
	{
		return ObjectUtil.CreateObject<ObjectType>();
	}

	public static object? CreateObject(object? @object)
	{
		return ObjectUtil.CreateObject(@object);
	}

	public static PropertyInfo[]? GetPublicSettablePropertyInfes(
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
		bool isGetPropertyInfesGettable
			= (propertiesBindingFlags & BindingFlags.GetProperty)
			== BindingFlags.GetProperty;
		bool isGetPropertyInfesSettable
			= (propertiesBindingFlags & BindingFlags.SetProperty)
			== BindingFlags.SetProperty;
		if (objectSetableProperties.Length > 0
			&& (isGetPropertyInfesGettable || isGetPropertyInfesSettable))
		{
			var objectSetablePropertiesCount = objectSetableProperties.Length;
			for (var propertyInfoIndex = objectSetablePropertiesCount - 1;
				propertyInfoIndex >= 0;
				propertyInfoIndex--)
			{
				var propertyInfo = objectSetableProperties[propertyInfoIndex];
				var isPropertyInfoValid = true;
				if (isGetPropertyInfesGettable
					&& propertyInfo.GetMethod == null)
				{
					isPropertyInfoValid = false;
				}
				if (isGetPropertyInfesSettable
					&& propertyInfo.SetMethod == null)
				{
					isPropertyInfoValid = false;
				}
				if (isPropertyInfoValid == false)
				{
					Array.Copy(
						objectSetableProperties,
						propertyInfoIndex + 1,
						objectSetableProperties,
						propertyInfoIndex,
						objectSetableProperties.Length - (propertyInfoIndex + 1));
					objectSetablePropertiesCount--;
				}
			}
			if (objectSetablePropertiesCount < objectSetableProperties.Length)
			{
				Array.Resize(ref objectSetableProperties, objectSetablePropertiesCount);
			}
		}
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
		object? propertyValue,
		BindingFlags propertiesBindingFlags = BindingFlags.Public | BindingFlags.Instance)
	{
		if (propertyValue == null)
		{
			return null;
		}

		var objectType = @object.GetType();
		var objectTypeProperties = objectType.GetProperties(propertiesBindingFlags);
		string? propertyName = null;
		foreach (var objectTypeProperty in objectTypeProperties)
		{
			var objectProperty = objectTypeProperty.GetValue(@object);
			if (objectProperty == propertyValue)
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
	    BindingFlags propertiesBindingFlags)
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
		    BindingFlags.Default);
	}

	public static List<ItemPropertyGetInfo<object>>? GetPropertyGetInfes(
		this object? item,
		int propertyLayerNumber = 0,
		string[]? propertyNamesExcepted = null,
		BindingFlags propertiesBindingFlags = BindingFlags.Default,
		Func<object, PropertyInfo, bool>? toIsPropertyInfoOfObjectValidToGenerate = null)
	{
		if (item == null)
		{
			return null;
		}

		var itemType = item.GetType();
		if (itemType.IsValueType)
		{
			if (itemType.IsPrimitive
				|| itemType.IsEnum)
			{
				return null;
			}
			////////////////////////////////////////////////
			// 其他均为结构体，
			// 需要特殊处理的结构体：DateTime，decimal
			////////////////////////////////////////////////
			else if (itemType.Equals(typeof(decimal))
				|| itemType.Equals(typeof(DateTime))
				|| itemType.Equals(typeof(DateTimeOffset)))
			{
				return null;
			}
			////////////////////////////////////////////////
			// !!! 其他结构体均按对象类型处理。 !!!
			////////////////////////////////////////////////
			else
			{ }
		}

		////////////////////////////////////////////////
		////////////////////////////////////////////////
		////////////////////////////////////////////////
		// !!! 默认结构体，对象的属性处理。 !!!
		////////////////////////////////////////////////
		////////////////////////////////////////////////
		////////////////////////////////////////////////

		// 字符串对象的特殊处理。
		if (itemType.Equals(typeof(string)))
		{
			return null;
		}

		// 字典对象的特殊处理。
		if (item is IDictionary childItemDictionary)
		{
			var childItemValues = childItemDictionary.Values;
			if (childItemValues.Count < 1)
			{
				return null;
			}

			var isChildItemTypeValue = false;
			foreach (var childItemValue in childItemValues)
			{
				var childItemValueType = childItemValue.GetType();
				if (childItemValueType.IsValueType
					// 字符串类型的特殊处理。
					|| childItemValueType.Equals(typeof(string)))
				{
					// !!!
					isChildItemTypeValue = true;
					// !!!
				}
				break;
			}
			if (isChildItemTypeValue)
			{
				return null;
			}

			var childItemKeys = childItemDictionary.Keys;
			var chilItemPropertyGetInfes = new List<ItemPropertyGetInfo<object>>();
			foreach (var childItemKey in childItemKeys)
			{
				var childItemValue = childItemDictionary[childItemKey];
				chilItemPropertyGetInfes.Add(new ItemPropertyGetInfo<object>(
					propertyLayerNumber,
					ItemPropertyRelation.ObjectItemInIEnumerable,
					item,
					null,
					childItemValue,
					-1,
					childItemKey));
			}
			return chilItemPropertyGetInfes;
		}
		// !!!⚠
		// !!!⚠ 注意这里一定是“容器（）”，而不能是“IEnumerable”，例子：string 类型。 ⚠!!!
		// !!!⚠
		else if (item is ICollection childItems)
		{
			if (childItems.Count < 1)
			{
				return null;
			}

			var isChildItemTypeValue = false;
			foreach (var childItem in childItems)
			{
				var childItemType = childItem.GetType();
				if (childItemType.IsValueType
					// 字符串类型的特殊处理。
					|| childItemType.Equals(typeof(string)))
				{
					isChildItemTypeValue = true;
				}
				break;
			}
			if (isChildItemTypeValue)
			{
				return null;
			}

			var chilItemPropertyGetInfes = new List<ItemPropertyGetInfo<object>>();
			var childItemIndex = 0;
			foreach (var childItem in childItems)
			{
				chilItemPropertyGetInfes.Add(new ItemPropertyGetInfo<object>(
					propertyLayerNumber,
					ItemPropertyRelation.ObjectItemInIEnumerable,
					item,
					null,
					childItem,
					childItemIndex,
					null));
				////////////////////////////////////////////////
				childItemIndex++;
				////////////////////////////////////////////////
			}
			return chilItemPropertyGetInfes;
		}

		// 普通的对象属性。
		if (propertiesBindingFlags == BindingFlags.Default)
		{
			propertiesBindingFlags
				= BindingFlags.Instance
				| BindingFlags.Public
				| BindingFlags.GetProperty;
		}
		var isGetPropertyInfesGetable
			= (propertiesBindingFlags & BindingFlags.GetProperty)
			== BindingFlags.GetProperty;
		var isGetPropertyInfesSetable
			= (propertiesBindingFlags & BindingFlags.SetProperty)
			== BindingFlags.SetProperty;
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
			if (isGetPropertyInfesGetable
				&& hostItemPropertyInfo.GetMethod == null)
			{
				continue;
			}
			if (isGetPropertyInfesSetable
				&& hostItemPropertyInfo.SetMethod == null)
			{
				continue;
			}
			itemPropertyGetInfes.Add(new(
				propertyLayerNumber,
				ItemPropertyRelation.Property,
				item,
				hostItemPropertyInfo,
				null,
				-1,
				null));
		}
		return itemPropertyGetInfes;
	}


	public static List<ObjectPropertyInfo> GetPropertyInfesFrom(
		this Type _,
		PropertyInfo[] sourceEntityPropertyInfes,
		Func<ObjectPropertyInfo, bool> isPropertyInfoValid,
		BindingFlags propertyBindingFlags = BindingFlags.Instance | BindingFlags.Public)
	{
		var objectEntityPropertyInfes = new List<ObjectPropertyInfo>();
		var objectEntityPropertyInfoId = 0;
		foreach (var entityPropertyInfo in sourceEntityPropertyInfes)
		{
			// 非值类型，
			// 并且没有可为空的标记，
			// 则该类型不可为空，需要约束，
			// 非字符串和非集合对象【需要】检查子属性：
			RecursionUtil.Enumerate(
				new ObjectPropertyInfo(0, entityPropertyInfo, []),
				(entityPropertyInfo) =>
				{
					var entityPropertyType = entityPropertyInfo.PropertyType;

					// 值类型，【不需要】检查子属性：
					if (entityPropertyType.IsValueType)
					{
						return null;
					}
					// 字符串和集合对象，【不需要】检查子属性：
					if (entityPropertyType.Equals(typeof(string))
						|| entityPropertyType.IsAssignableTo(typeof(System.Collections.ICollection)))
					{
						return null;
					}

					// 非值类型，
					// 且，非字符串和集合对象，
					// 【需要】检查子属性：
					var childEntityPropertyInfes = new List<ObjectPropertyInfo>();
					var childPropertyInfes = entityPropertyType.GetProperties(propertyBindingFlags);
					foreach (var childPropertyInfo in childPropertyInfes)
					{
						childEntityPropertyInfes.Add(
							new ObjectPropertyInfo(
								0,
								childPropertyInfo,
								[]));
					}
					return childEntityPropertyInfes;
				},
				(parentEntityPropertyInfo, entityPropertyInfo) =>
				{
					if (!isPropertyInfoValid(entityPropertyInfo))
					{
						return true;
					}


					objectEntityPropertyInfoId++;
					entityPropertyInfo.Id = objectEntityPropertyInfoId;
					if (parentEntityPropertyInfo == null)
					{
						// !!!
						objectEntityPropertyInfes.Add(entityPropertyInfo);
						// !!!
					}
					else
					{
						// !!!
						parentEntityPropertyInfo.AddChildObjectPropertyInfo(
							entityPropertyInfo);
						// !!!
					}
					return true;
				});
		}
		return objectEntityPropertyInfes;
	}

	public static List<ObjectPropertyInfo> GetObjectPropertyInfes(
		this Type entityType,
		Func<ObjectPropertyInfo, bool> isPropertyInfoValid,
		BindingFlags propertyBindingFlags = BindingFlags.Instance | BindingFlags.Public)
	{
		var objectEntityPropertyInfes = new List<ObjectPropertyInfo>();
		var objectEntityPropertyInfoId = 0;
		PropertyInfo[] sourceEntityPropertyInfes = entityType.GetProperties(propertyBindingFlags);
		foreach (var entityPropertyInfo in sourceEntityPropertyInfes)
		{
			// 非值类型，
			// 并且没有可为空的标记，
			// 则该类型不可为空，需要约束，
			// 非字符串和非集合对象【需要】检查子属性：
			RecursionUtil.Enumerate(
				new ObjectPropertyInfo(0, entityPropertyInfo, []),
				(entityPropertyInfo, currentRecursionStep, _) =>
				{
					// 值类型，【不需要】检查子属性，
					// 字符串，【不需要】检查子属性，
					// 集合对象，【不需要】检查子属性：
					if (entityPropertyInfo.IsPropertyTypeNoneChildProperties)
					{
						return null;
					}

					// 非值类型，
					// 单之前已经检查过的类型，
					// 【不需要】再次检查子属性，
					// （否则会产生死循环）：
					var entityPropertyType = entityPropertyInfo.PropertyType;
					for (var prevRecursionStep = currentRecursionStep.PrevRecursionStep;
					prevRecursionStep != null;
					prevRecursionStep = prevRecursionStep.PrevRecursionStep)
					{
						if (prevRecursionStep.CurrentItem?.PropertyType.
						Equals(entityPropertyType) == true)
						{
							return null;
						}
					}


					// 非值类型，
					// 且，之前未检查过的类型，
					// 且，非字符串和集合对象，
					// 【需要】检查子属性：
					var childEntityPropertyInfes = new List<ObjectPropertyInfo>();
					var childPropertyInfes = entityPropertyInfo.GetProperties(propertyBindingFlags);
					foreach (var childPropertyInfo in childPropertyInfes)
					{
						childEntityPropertyInfes.Add(
							new ObjectPropertyInfo(
								0,
								childPropertyInfo,
								[]));
					}
					return childEntityPropertyInfes;
				},
				(parentEntityPropertyInfo, entityPropertyInfo, _) =>
				{
					if (!isPropertyInfoValid(entityPropertyInfo))
					{
						return true;
					}

					objectEntityPropertyInfoId++;
					entityPropertyInfo.Id = objectEntityPropertyInfoId;
					if (parentEntityPropertyInfo == null)
					{
						// !!!
						objectEntityPropertyInfes.Add(entityPropertyInfo);
						// !!!
					}
					else
					{
						// !!!
						parentEntityPropertyInfo.AddChildObjectPropertyInfo(
							entityPropertyInfo);
						// !!!
					}
					return true;
				});
		}
		return objectEntityPropertyInfes;
	}

	public static ObjectCheckResultType? CheckPropertyValuesWithObjectPropertyInfes<ObjectCheckResultType>(
		this object entity,
		ObjectPropertyInfo[] entityPropertyInfesNeedCheck,
		Func<object?, ObjectPropertyInfo, object?, ObjectCheckResultType?> toCheckEntityPropertyValue)
		where ObjectCheckResultType : class
	{
		if (entityPropertyInfesNeedCheck.Length < 1)
		{
			return null;
		}

		ObjectCheckResultType? checkResult = null;
		foreach (var entityPropertyInfoNeedCheck in entityPropertyInfesNeedCheck)
		{
			RecursionUtil.EnumerateWithRecursionStepType<ObjectPropertyInfo, ObjectPropertyRecursionStep>(
				entityPropertyInfoNeedCheck,
				(entityPropertyInfo, _) =>
				{
					return entityPropertyInfo.ChildObjectPropertyInfes;
				},
				(parentEntityPropertyInfo, propertyInfo, currentRecursionStep) =>
				{
					var propertyOwner
						= currentRecursionStep.RecursionDepthIndex == 0
						? entity
						: currentRecursionStep.ParentEntity;

					var entityPropertyValue
					= propertyOwner != null
					? propertyInfo.GetValue(propertyOwner)
					: null;

					checkResult = toCheckEntityPropertyValue(
						propertyOwner,
						propertyInfo,
						entityPropertyValue);
					if (checkResult != null)
					{
						// !!!
						return false;
						// !!!
					}

					// !!! 如果当前继续检查，且当前属性值为“null”， !!!
					// !!! 则重新获取属性值（有可能被检查函数赋值）。 !!!
					entityPropertyValue
					??= propertyOwner != null
						? propertyInfo.GetValue(propertyOwner)
						: null;

					// !!!
					currentRecursionStep.CurrentEntityPropertyValue
					= entityPropertyValue;
					// !!!
					return true;
				},
				(currentRecursionStep) =>
				{
					return new()
					{
						ParentEntity = currentRecursionStep.CurrentEntityPropertyValue
					};
				});
			if (checkResult != null)
			{
				break;
			}
		}
		return checkResult;
	}


	/// <summary>
	/// 浅拷贝，通过设置同名属性，克隆产生新对象。
	/// </summary>
	/// <typeparam name="ObjectType">当前对象类型。</typeparam>
	/// <param name="currentObject">当前对象。</param>
	/// <returns>拥有相同属性的，克隆产生的新对象。</returns>
	public static ObjectType CloneShallow<ObjectType>(this ObjectType currentObject)
		where ObjectType
		: class, new()
	{
		var newObject = new ObjectType();
		{
			newObject.SetPropertiesWithSameNameFrom((object)currentObject);
		}
		return newObject;
	}

	/// <summary>
	/// 深拷贝。
	/// </summary>
	/// <typeparam name="ObjectType">当前对象类型。</typeparam>
	/// <param name="item">当前对象。</param>
	/// <param name="propertyNamesExcepted">要排除的属性名称</param>
	/// <param name="propertiesBindingFlags">要拷贝属性的绑定标志。</param>
	/// <param name="toIsPropertyInfoOfObjectValidToGenerate">用于拷贝筛选属性的回调。</param>
	/// <param name="propertyLayerNumberMax">深拷贝的层数，“-1”表示不限制层数。</param>
	/// <returns>拥有相同属性的，克隆产生的新对象。</returns>
	public static ObjectType CloneDeep<ObjectType>(
		this ObjectType item,
		string[]? propertyNamesExcepted = null,
		BindingFlags propertiesBindingFlags = BindingFlags.Default,
		Func<object, PropertyInfo, bool>? toIsPropertyInfoOfObjectValidToGenerate = null,
		int propertyLayerNumberMax = -1)
		where ObjectType : new()
	{
		var itemCloned = new ObjectType();
		if (propertiesBindingFlags == BindingFlags.Default)
		{
			propertiesBindingFlags
				= BindingFlags.Instance
				| BindingFlags.Public
				| BindingFlags.GetProperty
				| BindingFlags.SetProperty;
		}
		var itemPropertyGetInfes = item.GetPropertyGetInfes(
			1,
			propertyNamesExcepted,
			propertiesBindingFlags,
			toIsPropertyInfoOfObjectValidToGenerate);
		if (itemPropertyGetInfes == null
			|| itemPropertyGetInfes.Count < 1)
		{
			return itemCloned;
		}
		if (propertyLayerNumberMax == 0)
		{
			return itemCloned;
		}


		HashSet<object> sourcePropertyValueHadGetPropertyGetInfes = [];
		{
			sourcePropertyValueHadGetPropertyGetInfes.Add(item!);
		}
		Dictionary<object, object?> sourcePropertyValueHadCloned = [];
		{
			sourcePropertyValueHadCloned.Add(item!, itemCloned);
		}
		foreach (var objectItemPropertyGetInfo in itemPropertyGetInfes)
		{
			RecursionUtil.Enumerate<ItemPropertyGetInfo<object>>(
				objectItemPropertyGetInfo,
				(itemPropertyGetInfo) =>
				{
					var sourcePropertyValue = itemPropertyGetInfo.GetPropertyValue();
					if (sourcePropertyValue == null)
					{
						return null;
					}


					////////////////////////////////////////////////
					// 避免递归引用，重复查询检查对象：
					////////////////////////////////////////////////
					if (sourcePropertyValueHadGetPropertyGetInfes.TryGetValue(
						sourcePropertyValue,
						out _))
					{
						return null;
					}
					// !!!
					sourcePropertyValueHadGetPropertyGetInfes.Add(
						sourcePropertyValue);
					// !!!


					////////////////////////////////////////////////
					// 检查递归层级：
					////////////////////////////////////////////////
					var isSourcePropertyValueCollectionType = sourcePropertyValue
					.GetType()
					.IsAssignableTo(typeof(ICollection));
					var itemPropertyGetInfesPropertyLayerNumber
					= isSourcePropertyValueCollectionType
					? itemPropertyGetInfo.PropertyLayerNumber
					: itemPropertyGetInfo.PropertyLayerNumber + 1;
					if (propertyLayerNumberMax >= 0
					&& itemPropertyGetInfesPropertyLayerNumber > propertyLayerNumberMax)
					{
						return null;
					}


					var nextSourcePropertyGetInfes = sourcePropertyValue
					.GetPropertyGetInfes(
						itemPropertyGetInfesPropertyLayerNumber,
						propertyNamesExcepted,
						propertiesBindingFlags,
						toIsPropertyInfoOfObjectValidToGenerate);
					{ }
					return nextSourcePropertyGetInfes;
				},
				(sourcePropertyHostObjectGetInfo, sourcePropertyGetInfo) =>
				{
					////////////////////////////////////////////////
					// 1/，确定当前属性的宿主对象：
					////////////////////////////////////////////////
					var clonedPropertyValueHostObject
					= sourcePropertyHostObjectGetInfo != null
					? sourcePropertyHostObjectGetInfo.PropertyValueCloned
					: itemCloned;


					////////////////////////////////////////////////
					// 2/，克隆新值：
					////////////////////////////////////////////////
					object? clonedPropertyValue = null;
					var sourcePropertyValue = sourcePropertyGetInfo.GetPropertyValue()!;
					if (sourcePropertyValue == null)
					{
						return true;
					}
					switch (sourcePropertyGetInfo.PropertyRelation)
					{
						default:
						case ItemPropertyRelation.Unknown:
						case ItemPropertyRelation.ValueItemInIEnumerable:
							{ }
							break;
						////////////////////////////////////////////////
						// 当前属性，【与父对象的关系】为：属性关系。
						////////////////////////////////////////////////
						case ItemPropertyRelation.Property:
							{
								var sourcePropertyInfo = sourcePropertyGetInfo.PropertyInfo!;
								// 当前属性，类型为：容器。
								// !!!⚠
								// !!!⚠ 注意这里一定是“容器（）”，而不能是“IEnumerable”，例子：string 类型。 ⚠!!!
								// !!!⚠
								if (sourcePropertyValue is ICollection sourceCollectionPropertyValue)
								{
									// 当前容器属性，类型为：数组类型。
									if (sourceCollectionPropertyValue is Array sourceArrayPropertyValue)
									{
										////////////////////////////////////////////////
										// 避免递归引用，重复创建对象：
										////////////////////////////////////////////////
										if (sourcePropertyValueHadCloned.TryGetValue(
											sourcePropertyValue,
											out clonedPropertyValue) != true)
										{
											clonedPropertyValue = Activator.CreateInstance(
												sourceArrayPropertyValue.GetType(),
												sourceArrayPropertyValue.Length);
											// !!!
											sourcePropertyValueHadCloned.Add(
												sourcePropertyValue,
												clonedPropertyValue);
											// !!!
										}


										////////////////////////////////////////////////
										sourcePropertyGetInfo.PropertyValueCloned = clonedPropertyValue;
										////////////////////////////////////////////////
										// !!! 向父对象，设置新的值。 !!!
										sourcePropertyInfo.SetValue(
											clonedPropertyValueHostObject,
											clonedPropertyValue);
										////////////////////////////////////////////////
										if (sourceArrayPropertyValue.Length > 0)
										{
											var childItemType = sourceArrayPropertyValue.GetValue(0)!.GetType();
											if (
											// 值类型的数组，直接复制原始值即可：
											childItemType.IsValueType
											// 字符串元素的特殊处理：
											|| childItemType.Equals(typeof(string)))
											{
												Array.Copy(
													sourceArrayPropertyValue,
													(Array)clonedPropertyValue!,
													sourceArrayPropertyValue.Length);
											}
											// 引用类型的数组，需要递归克隆：
											else
											{
												// ...
											}
										}
									}
									// 当前容器属性，类型为：列表类型。
									else if (sourceCollectionPropertyValue is IList souceListPropertyValue)
									{
										////////////////////////////////////////////////
										// 避免递归引用，重复创建对象：
										////////////////////////////////////////////////
										if (sourcePropertyValueHadCloned.TryGetValue(
											sourcePropertyValue,
											out clonedPropertyValue) != true)
										{
											clonedPropertyValue
											= Activator.CreateInstance(
												souceListPropertyValue.GetType());
											// !!!
											sourcePropertyValueHadCloned.Add(
												sourcePropertyValue,
												clonedPropertyValue);
											// !!!
										}


										////////////////////////////////////////////////
										sourcePropertyGetInfo.PropertyValueCloned = clonedPropertyValue;
										// !!! 向父对象，设置新的值。 !!!
										sourcePropertyInfo.SetValue(
											clonedPropertyValueHostObject,
											clonedPropertyValue);
										////////////////////////////////////////////////
										var clonedListPropertyValue = (IList)clonedPropertyValue!;
										if (souceListPropertyValue.Count > 0)
										{
											var childItemType = souceListPropertyValue[0]!.GetType();
											// 值类型的数组，直接复制原始值即可：
											if (childItemType.IsValueType)
											{
												foreach (var sourceChildItem in souceListPropertyValue)
												{
													// !!!
													clonedListPropertyValue.Insert(
														clonedListPropertyValue.Count,
														sourceChildItem);
													// !!!
												}
											}
											// 引用类型的数组，需要递归克隆：
											else
											{
												// ...
											}
										}
									}
									// 当前容器属性，类型为：字典类型。
									else if (sourceCollectionPropertyValue is IDictionary sourceDictionaryPropertyValue)
									{
										////////////////////////////////////////////////
										// 避免递归引用，重复创建对象：
										////////////////////////////////////////////////
										if (sourcePropertyValueHadCloned.TryGetValue(
											sourcePropertyValue,
											out clonedPropertyValue) != true)
										{
											clonedPropertyValue
											= Activator.CreateInstance(
												sourceDictionaryPropertyValue.GetType());
											// !!!
											sourcePropertyValueHadCloned.Add(
												sourcePropertyValue,
												clonedPropertyValue);
											// !!!
										}


										////////////////////////////////////////////////
										sourcePropertyGetInfo.PropertyValueCloned = clonedPropertyValue;
										// !!! 向父对象，设置新的值。 !!!
										sourcePropertyInfo.SetValue(
											clonedPropertyValueHostObject,
											clonedPropertyValue);
										////////////////////////////////////////////////
										var clonedDictionaryPropertyValue = (IDictionary)clonedPropertyValue!;
										var values = sourceDictionaryPropertyValue.Values;
										if (values.Count > 0)
										{
											var isValueTypeValue = false;
											foreach (var value in values)
											{
												var valueType = value.GetType();
												if (valueType.IsValueType
												|| valueType.Equals(typeof(string)))
												{
													isValueTypeValue = true;
												}
												break;
											}
											if (isValueTypeValue)
											{
												var keys = sourceDictionaryPropertyValue.Keys;
												foreach (var key in keys)
												{
													// !!!
													clonedDictionaryPropertyValue.Add(
														key,
														sourceDictionaryPropertyValue[key]);
													// !!!
												}
											}
										}
									}
									// 其他类型的容器，暂不处理。
									else
									{ }
								}
								// 当前属性，类型为：非容器，
								// 即：值类型，或引用类型。
								else
								{
									// 当前属性，类型为：值类型。
									if (sourcePropertyInfo.PropertyType.IsValueType)
									{
										// !!! 直接使用原始值。 !!!
										clonedPropertyValue = sourcePropertyValue;
										// !!!
									}
									// 当前属性，类型为：引用类型。
									else
									{
										////////////////////////////////////////////////
										// 避免递归引用，重复创建对象：
										////////////////////////////////////////////////
										if (sourcePropertyValueHadCloned.TryGetValue(
											sourcePropertyValue,
											out clonedPropertyValue) != true)
										{
											// !!! 创建新的对象，并向父对象，设置新的值。 !!!
											clonedPropertyValue = ObjectUtil.CreateObject(sourcePropertyValue);
											// !!!
											sourcePropertyValueHadCloned.Add(
												sourcePropertyValue,
												clonedPropertyValue);
											// !!!
										}
									}
									////////////////////////////////////////////////
									sourcePropertyGetInfo.PropertyValueCloned = clonedPropertyValue;
									////////////////////////////////////////////////
									// !!! 向父对象，设置新的值。 !!!
									sourcePropertyInfo.SetValue(
										clonedPropertyValueHostObject,
										clonedPropertyValue);
								}
							}
							break;
						////////////////////////////////////////////////
						// 当前属性，【与父对象的关系】为：容器关系。
						////////////////////////////////////////////////
						case ItemPropertyRelation.ObjectItemInIEnumerable:
							{
								if (sourcePropertyValue is object sourceObjectPropertyValue)
								{
									////////////////////////////////////////////////
									// 避免递归引用，重复创建对象：
									////////////////////////////////////////////////
									if (sourcePropertyValueHadCloned.TryGetValue(
										sourceObjectPropertyValue,
										out clonedPropertyValue) != true)
									{
										// !!! 创建新的对象，并向父对象，设置新的值。 !!!
										clonedPropertyValue = ObjectUtil.CreateObject(sourceObjectPropertyValue);
										// !!!
										sourcePropertyValueHadCloned.Add(
											sourcePropertyValue,
											clonedPropertyValue);
										// !!!
									}


									////////////////////////////////////////////////
									sourcePropertyGetInfo.PropertyValueCloned = clonedPropertyValue;
									////////////////////////////////////////////////


									// 当前容器属性，类型为：数组类型。
									if (clonedPropertyValueHostObject is Array clonedPropertyValueArrayHostObject)
									{
										var clonedPropertyValue_Index = sourcePropertyGetInfo.ItemInCollection_Index;
										if (clonedPropertyValue_Index < clonedPropertyValueArrayHostObject.Length)
										{
											// !!!
											clonedPropertyValueArrayHostObject.SetValue(
												clonedPropertyValue,
												clonedPropertyValue_Index);
											// !!!
										}
									}
									// 当前容器属性，类型为：列表类型。
									else if (clonedPropertyValueHostObject is IList clonedPropertyValueListHostObject)
									{
										var clonedPropertyValue_Index = sourcePropertyGetInfo.ItemInCollection_Index;
										//if (clonedPropertyValue_Index < clonedPropertyValueListHostObject.Count)
										{
											// !!!
											clonedPropertyValueListHostObject.Add(clonedPropertyValue);
											// !!!
										}
									}
									// 当前容器属性，类型为：字典类型。
									else if (clonedPropertyValueHostObject is IDictionary clonedPropertyValueDictionaryHostObject)
									{
										var clonedPropertyValue_Key = sourcePropertyGetInfo.ItemInCollection_Key;
										if (clonedPropertyValue_Key != null)
										{
											// !!!
											clonedPropertyValueDictionaryHostObject.Add(
												clonedPropertyValue_Key,
												clonedPropertyValue);
											// !!!
										}
									}
								}
							}
							break;
					}
					return true;
				});
		}
		return itemCloned;
	}

	public static ObjectType Clone<ObjectType>(
		this ObjectType currentObject)
		where ObjectType
		: class, new()
	{
		return CloneShallow(currentObject);
	}

	public static ObjectType Clone<ObjectType>(
		this ObjectType currentObject,
		bool isDeepClone,
		string[]? propertyNamesExcepted = null,
		BindingFlags propertiesBindingFlags = BindingFlags.Default,
		Func<object, PropertyInfo, bool>? toIsPropertyInfoOfObjectValidToGenerate = null,
		int propertyLayerNumberMax = -1)
		where ObjectType
		: class, new()
	{
		if (isDeepClone == false)
		{
			return CloneShallow(currentObject);
		}
		return CloneDeep(
			currentObject,
			propertyNamesExcepted,
			propertiesBindingFlags,
			toIsPropertyInfoOfObjectValidToGenerate,
			propertyLayerNumberMax);
	}

	public static ObjectType[] CloneItemsToArray<ObjectType>(this IEnumerable<ObjectType> objects)
		where ObjectType : class, new()
	{
		var objectArray = new ObjectType[objects.GetCount()];
		var objectIndex = 0;
		foreach (var obj in objects)
		{
			if (objectIndex < objectArray.Length)
			{
				objectArray[objectIndex] = obj.CloneShallow();
			}
			objectIndex++;
		}
		return objectArray;
	}

	public static List<ObjectType> CloneItemsToList<ObjectType>(this IEnumerable<ObjectType> objects)
		where ObjectType : class, new()
	{
		var objectList = new List<ObjectType>();
		foreach (var obj in objects)
		{
			objectList.Add(obj.CloneShallow());
		}
		return objectList;
	}

	/// <summary>
	/// 基本类型数据转为字节数组。
	/// </summary>
	/// <param name="baseValue">任意对象。</param>
	/// <param name="itemsByteSeparator">子元素间的分隔字节。</param>
	/// <param name="keyValueByteSeparator">键值间的分隔字节。</param>
	/// <returns>如果任意对象是基本数据类型，则将基本类型数据转为字节数组。</returns>
	public static byte[]? ConvertBaseValueToBytes(
		object? baseValue,
		byte itemsByteSeparator,
		byte keyValueByteSeparator)
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
			// !!!⚠
			// !!!⚠ 字典集合的特殊处理。 ⚠!!!
			// !!!⚠
			if (baseValue is IDictionary dictionary)
			{
				var values = dictionary.Values;
				if (values.Count < 1)
				{
					return null;
				}
				foreach (var value in values)
				{
					var valueType = value.GetType();
					if (valueType.IsValueType != true
						&& valueType.Equals(typeof(string)) != true)
					{
						return null;
					}
					break;
				}

				var memoryStream = new MemoryStream();
				using var binaryWriter = new BinaryWriter(memoryStream);
				var keys = dictionary.Keys;
				foreach (var key in keys)
				{
					var keyBytes = ConvertBaseValueToBytes(
						key,
						itemsByteSeparator,
						keyValueByteSeparator);
					if (keyBytes != null)
					{
						// !!!
						binaryWriter.Write(keyBytes);
						// !!!
					}

					// !!!
					binaryWriter.Write(keyValueByteSeparator);
					// !!!

					var value = dictionary[key];
					if (value != null)
					{
						var childItemBytes = ConvertBaseValueToBytes(
							value,
							itemsByteSeparator,
							keyValueByteSeparator)
						?? throw new Exception("将基础类型值（字典）转为字节数组失败，意外的错误。");
						// !!!
						binaryWriter.Write(childItemBytes);
						binaryWriter.Write(itemsByteSeparator);
						// !!!
					}
				}
				return memoryStream.ToArray();
			}
			// !!!⚠
			// !!!⚠ 注意这里一定是“容器（）”，而不能是“IEnumerable”，例子：string 类型。 ⚠!!!
			// !!!⚠
			else if (baseValue is ICollection items)
			{
				var itemsEnumerator = items.GetEnumerator();
				if (itemsEnumerator.MoveNext() == false)
				{
					return null;
				}
				var firstItem = itemsEnumerator.Current;
				var itemType = firstItem.GetType();
				if (itemType.IsValueType != true
					&& itemType == typeof(string) != true)
				{
					return null;
				}

				var memoryStream = new MemoryStream();
				using var binaryWriter = new BinaryWriter(memoryStream);
				foreach (var item in items)
				{
					var itemBytes
						= ConvertBaseValueToBytes(
							item,
							itemsByteSeparator,
							keyValueByteSeparator)
						?? throw new Exception("将基础类型值（集合）转为字节数组失败，意外的错误。");
					// !!!
					binaryWriter.Write(itemBytes);
					binaryWriter.Write(itemsByteSeparator);
					// !!!
				}
				return memoryStream.ToArray();
			}
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
		if (baseValue is DateTimeOffset dateTimeOffsetValue)
		{
			return BitConverter.GetBytes(dateTimeOffsetValue.Ticks);
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
			return [.. Decimal.GetBits(decimalValue).SelectMany(BitConverter.GetBytes)];
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

		//if (baseValue is DateTimeOffset dateTimeOffsetValue)
		//{
		//	return BitConverter.GetBytes(dateTimeOffsetValue.Ticks);
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
		//		var arrayItemBytes = ConvertBaseValueToBytes(
		//		arrayItem,
		//		itemsByteSeparator,
		//		keyValueByteSeparator);
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
	/// <param name="isGetPropertyInfesRecursivly">是否使用递归深度获取对象的属性信息。。</param>
	/// <param name="propertiesBindingFlags">要读取的属性绑定标志，如果指定为“System.Reflection.BindingFlags.Default”，则默认使用“System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty”的组合。</param>
	/// <param name="toIsPropertyInfoOfObjectValidToGenerate">判断指定对象，指定属性是否参与字节生成的回调。</param>
	/// <param name="propertyLayerNumberMax">要获取属性层数的最大值，小于“0”时，表示不限制。</param>
	/// <param name="propertyByteSeparator">属性的字节分隔符。</param>
	/// <param name="itemsByteSeparator">子元素间的分隔字节。</param>
	/// <param name="keyValueByteSeparator">键值间的分隔字节。</param>
	/// <returns>返回由当前对象，所有值类型属性生成的字节数组。</returns>
	public static byte[] ToBytes(
		this object? objectItem,
		string[]? propertyNamesExcepted = null,
		bool isGetPropertyInfesRecursivly = false,
		BindingFlags propertiesBindingFlags = BindingFlags.Default,
		Func<object, PropertyInfo, bool>? toIsPropertyInfoOfObjectValidToGenerate = null,
		int propertyLayerNumberMax = -1,
		byte propertyByteSeparator = 0,
		byte itemsByteSeparator = 0,
		byte keyValueByteSeparator = 0)
	{
		if (objectItem == null)
		{
			return [];
		}

		var memoryStream = new MemoryStream();
		using var binaryWriter = new BinaryWriter(memoryStream);
		////////////////////////////////////////////////

		var objectItemPropertyGetInfes = objectItem.GetPropertyGetInfes(
			1,
			propertyNamesExcepted,
			propertiesBindingFlags,
			toIsPropertyInfoOfObjectValidToGenerate);
		if (objectItemPropertyGetInfes == null
			|| objectItemPropertyGetInfes.Count < 1)
		{
			return ConvertBaseValueToBytes(
				objectItem,
				itemsByteSeparator,
				keyValueByteSeparator)
				?? [];
		}
		if (propertyLayerNumberMax == 0)
		{
			return [];
		}


		byte[] objectItemBytes;
		if (isGetPropertyInfesRecursivly == false)
		{
			foreach (var objectItemPropertyGetInfo in objectItemPropertyGetInfes)
			{
				var itemPropertyValue = objectItemPropertyGetInfo.GetPropertyValue();
				var itemPropertyValueBytes = ConvertBaseValueToBytes(
					itemPropertyValue,
					itemsByteSeparator,
					keyValueByteSeparator);
				if (itemPropertyValueBytes != null)
				{
					// !!!
					binaryWriter.Write(itemPropertyValueBytes);
					binaryWriter.Write(propertyByteSeparator);
					// !!!
				}
			}
			////////////////////////////////////////////////
			objectItemBytes = memoryStream.ToArray();
			{ }
			return objectItemBytes;
			////////////////////////////////////////////////
		}

		foreach (var objectItemPropertyGetInfo in objectItemPropertyGetInfes)
		{
			RecursionUtil.Enumerate<ItemPropertyGetInfo<object>>(
				objectItemPropertyGetInfo,
				(itemPropertyGetInfo) =>
				{
					var itemPropertyValue = itemPropertyGetInfo.GetPropertyValue();
					if (itemPropertyValue == null)
					{
						return null;
					}
					var isItemPropertyValueICollection = itemPropertyValue
					.GetType()
					.IsAssignableTo(typeof(ICollection));
					var itemPropertyGetInfesPropertyLayerNumber
					= isItemPropertyValueICollection
					? itemPropertyGetInfo.PropertyLayerNumber
					: itemPropertyGetInfo.PropertyLayerNumber + 1;

					if (propertyLayerNumberMax >= 0
					&& itemPropertyGetInfesPropertyLayerNumber > propertyLayerNumberMax)
					{
						return null;
					}

					var itemPropertyGetInfes
					= itemPropertyValue
					.GetPropertyGetInfes(
						itemPropertyGetInfesPropertyLayerNumber,
						propertyNamesExcepted,
						propertiesBindingFlags,
						toIsPropertyInfoOfObjectValidToGenerate);
					{ }
					return itemPropertyGetInfes;
				},
				(parentItemPropertyGetInfo, itemPropertyGetInfo) =>
				{
					var itemPropertyValue
					= itemPropertyGetInfo.GetPropertyValue();
					// 字典元素处理：
					if (itemPropertyGetInfo.ItemInCollection_Key != null)
					{
						var itemPropertyKeyBytes = ConvertBaseValueToBytes(
							itemPropertyGetInfo.ItemInCollection_Key,
							itemsByteSeparator,
							keyValueByteSeparator);
						if (itemPropertyKeyBytes != null)
						{
							// !!!
							binaryWriter.Write(itemPropertyKeyBytes);
							// !!!
						}
					}

					// !!!
					binaryWriter.Write(keyValueByteSeparator);
					// !!!

					var itemPropertyValueBytes = ConvertBaseValueToBytes(
							itemPropertyValue,
							itemsByteSeparator,
							keyValueByteSeparator);
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
		objectItemBytes = memoryStream.ToArray();
		{ }
		return objectItemBytes;
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
		var jsonString = StringUtil.StringByJsonSerializeObject(
			item,
			jsonSerializerOptions);
		{ }
		return jsonString;
	}

	#endregion
}
