﻿using BaoXia.Utils.Extensions;
using BaoXia.Utils.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace BaoXia.Utils.Test.ExtensionsTest;

[TestClass]
public class ObjectExtensionTest
{
	[TestMethod]
	public void CreateObjectTest()
	{
		var stringObject = ObjectExtension.CreateObject<string>();
		{
			Assert.AreEqual(string.Empty, stringObject);
		}
		var intObject = ObjectExtension.CreateObject<int>();
		{
			Assert.AreEqual(0, intObject);
		}
		var floatObject = ObjectExtension.CreateObject<float>();
		{
			Assert.AreEqual(0, floatObject);
		}
		var doubleObject = ObjectExtension.CreateObject<double>();
		{
			Assert.AreEqual(0, doubleObject);
		}
		var decimalObject = ObjectExtension.CreateObject<decimal>();
		{
			Assert.AreEqual(0, decimalObject);
		}
		var dateTimeObject = ObjectExtension.CreateObject<DateTime>();
		{
			Assert.AreEqual(DateTime.MinValue, dateTimeObject);
		}
	}

	protected class ClassA
	{
		public int IntProperty { get; set; }

		public float FloatProperty { get; set; }

		public double DoubleProperty { get; set; }

		public decimal DecimalProperty { get; set; }

		public string? StringProperty { get; set; }

		public DateTime DateTimeProperty { get; set; }

		public object? ObjectProperty { get; set; }

		public static string? ReadOnlyProperty
		{
			get
			{
				return "classA.ReadOnlyProperty";
			}
		}

		private int _intField = 1;
		public int IntFieldProperty
		{
			get
			{
				return _intField;
			}
			set
			{
				_intField = value;
			}
		}

		protected float _floatField = 1.0F;
		public float FloatFieldProperty
		{
			get
			{
				return _floatField;
			}
		}

		public string? _stringField = "classA._stringField";
		public string? StringFieldProperty
		{
			get
			{
				return _stringField;
			}
			set
			{
				_stringField = value;
			}
		}
	}

	protected class ClassB
	{
		public int IntProperty { get; set; }

		public float FloatProperty { get; set; }

		public double DoubleProperty { get; set; }

		public decimal DecimalProperty { get; set; }

		public string? StringProperty { get; set; }

		public DateTime DateTimeProperty { get; set; }

		public object? ObjectProperty { get; set; }

		public static string? ReadOnlyProperty
		{
			get
			{
				return "classB.ReadOnlyProperty";
			}
		}

		private int _intField = 2;
		public int IntFieldProperty
		{
			get
			{
				return _intField;
			}
			set
			{
				_intField = value;
			}
		}

		protected float _floatField = 2.0F;
		public float FloatFieldProperty
		{
			get
			{
				return _floatField;
			}
		}

		public string? _stringField = "classB._stringField";
		public string? StringFieldProperty
		{
			get
			{
				return _stringField;
			}
			set
			{
				_stringField = value;
			}
		}
	}

	protected class ClassC
	{
		public string? Value { get; set; }
	}


	protected class ClassD
	{
		public int Int_1 { get; set; }
		public int Int_2 { get; set; }
		public int Int_3 { get; set; }
		public int Int_4 { get; set; }
		public int Int_5 { get; set; }
		public int Int_6 { get; set; }
		public int Int_7 { get; set; }
		public int Int_8 { get; set; }
		public int Int_9 { get; set; }
		public int Int_10 { get; set; }

		public long Long_1 { get; set; }
		public long Long_2 { get; set; }
		public long Long_3 { get; set; }
		public long Long_4 { get; set; }
		public long Long_5 { get; set; }
		public long Long_6 { get; set; }
		public long Long_7 { get; set; }
		public long Long_8 { get; set; }
		public long Long_9 { get; set; }
		public long Long_10 { get; set; }

		public float Float_1 { get; set; }
		public float Float_2 { get; set; }
		public float Float_3 { get; set; }
		public float Float_4 { get; set; }
		public float Float_5 { get; set; }
		public float Float_6 { get; set; }
		public float Float_7 { get; set; }
		public float Float_8 { get; set; }
		public float Float_9 { get; set; }
		public float Float_10 { get; set; }

		public double Double_1 { get; set; }
		public double Double_2 { get; set; }
		public double Double_3 { get; set; }
		public double Double_4 { get; set; }
		public double Double_5 { get; set; }
		public double Double_6 { get; set; }
		public double Double_7 { get; set; }
		public double Double_8 { get; set; }
		public double Double_9 { get; set; }
		public double Double_10 { get; set; }

		public string? String_1 { get; set; }
		public string? String_2 { get; set; }
		public string? String_3 { get; set; }
		public string? String_4 { get; set; }
		public string? String_5 { get; set; }
		public string? String_6 { get; set; }
		public string? String_7 { get; set; }
		public string? String_8 { get; set; }
		public string? String_9 { get; set; }
		public string? String_10 { get; set; }


		public object? Object_1 { get; set; }
		public object? Object_2 { get; set; }
		public object? Object_3 { get; set; }
		public object? Object_4 { get; set; }
		public object? Object_5 { get; set; }
		public object? Object_6 { get; set; }
		public object? Object_7 { get; set; }
		public object? Object_8 { get; set; }
		public object? Object_9 { get; set; }
		public object? Object_10 { get; set; }

	}

	[TestMethod]
	public void GetPublicSetablePropertyValues()
	{
		var objectA = new ClassA()
		{
			IntProperty = 123,
			IntFieldProperty = -123,
			StringProperty = "Abc",
			StringFieldProperty = "-Abc"
		};

		var allPublisSetablePropertyNames = new string[]
		{
			"IntProperty",
			"FloatProperty",
			"DoubleProperty",
			"DecimalProperty",
			"StringProperty",
			"DateTimeProperty",
			"ObjectProperty",

			"IntFieldProperty",
			"StringFieldProperty"
		};

		var allPublicReadOnlyPropertyNames = new string[]
		{
			"FloatFieldProperty",
		};

		var objectAllPublicSetableProperties = objectA.GetPublicSettablePropertyInfes();

		Assert.IsNotNull(objectAllPublicSetableProperties);
		Assert.AreEqual(allPublisSetablePropertyNames.Length, objectAllPublicSetableProperties!.Length);
		foreach (var objectPublicSetableProperty in objectAllPublicSetableProperties)
		{
			if (objectPublicSetableProperty.Name.EqualsIgnoreCase("ReadOnlyProperty"))
			{
				Assert.Fail();
			}
			else if (objectPublicSetableProperty.Name.Contains('_', StringComparison.OrdinalIgnoreCase))
			{
				Assert.Fail();
			}
			else
			{
				Assert.IsTrue(
					Array.IndexOf(allPublisSetablePropertyNames, objectPublicSetableProperty.Name)
					>= 0);
				Assert.IsTrue(
					Array.IndexOf(allPublicReadOnlyPropertyNames, objectPublicSetableProperty.Name)
					< 0);
			}
		}
	}

	[TestMethod]
	public void GetPropertyValueWithNameTest()
	{
		var objectA = new ClassA()
		{
			IntProperty = 123,
			IntFieldProperty = -123,
			StringProperty = "Abc",
			StringFieldProperty = "-Abc"
		};

		if (objectA.GetPropertyValueWithName(nameof(ClassA.StringProperty)) is not string stringValue
			|| !stringValue.Equals(objectA.StringProperty))
		{
			Assert.Fail();
		}

		if (objectA.GetPropertyValueWithName(nameof(ClassA.IntProperty)) is not int intValue
			|| !intValue.Equals(objectA.IntProperty))
		{
			Assert.Fail();
		}

	}


	[TestMethod]
	public void GetPropertyNameWithPropertyValueTest()
	{
		var testObject = new ClassA();
		{
			testObject.StringProperty = "Abc";
		}
		var nameOfProperty = testObject.GetPropertyNameWithPropertyValue(testObject.StringProperty);
		{
			Assert.AreEqual(nameof(ClassA.StringProperty), nameOfProperty);
		}
	}

	[TestMethod]
	public void SetPropertiesWithSameNameFromTest()
	{
		var objectA = new ClassA()
		{
			IntProperty = 1,

			FloatProperty = 1.0F,

			DoubleProperty = 1.0F,

			StringProperty = "A",

			DateTimeProperty = DateTime.MinValue,

			ObjectProperty = new ClassC()
			{
				Value = "ClassA.ItemInCollection"
			}
		};
		var objectB = new ClassB()
		{
			IntProperty = 2,

			FloatProperty = 2,

			DoubleProperty = 2.0,

			StringProperty = "B",

			DateTimeProperty = DateTime.Now,

			ObjectProperty = new ClassC()
			{
				Value = "ClassB.ItemInCollection"
			}
		};

		// !!!
		objectA.SetPropertiesWithSameNameFrom(objectB);
		// !!!

		////////////////////////////////////////////////

		Assert.AreEqual(objectB.IntProperty, objectA.IntProperty);
		Assert.AreEqual(objectB.FloatProperty, objectA.FloatProperty);
		Assert.AreEqual(objectB.DoubleProperty, objectA.DoubleProperty);
		Assert.AreEqual(objectB.StringProperty, objectA.StringProperty);
		Assert.AreEqual(objectB.DateTimeProperty, objectA.DateTimeProperty);
		Assert.AreEqual(objectB.ObjectProperty, objectA.ObjectProperty);

		Assert.AreEqual(objectB.IntProperty, objectA.IntFieldProperty);
		// FloatFieldProperty 只有 get 方法。
		Assert.AreNotEqual(objectB.FloatFieldProperty, objectA.FloatFieldProperty);
		Assert.AreEqual(objectB.StringFieldProperty, objectA.StringFieldProperty);

		////////////////////////////////////////////////

	}

	[TestMethod]
	public void CloneWithSamePropertiesPropertiesTest()
	{
		var objA = new ClassA()
		{
			StringProperty = "Abc"
		};
		var objB = objA.CloneShallow();

		// !!!
		Assert.AreNotEqual(objB, objA);
		// !!!
		// !!!
		Assert.AreEqual(objB!.StringProperty, objA.StringProperty);
		Assert.IsTrue(objA.StringProperty.Equals(objB?.StringProperty));
		// !!!
	}

	/// <summary>
	/// 测试结果：
	/// CPU：I9 12900K
	/// 单个元素的复制速度大约为： 0.01 - 0.02 毫秒。
	/// </summary>
	[TestMethod]
	public void CloneWithSamePropertiesPropertiesPerformaceTest()
	{
		var sourceObjects = new ClassD[10000];
		var objectObjects = new ClassD?[sourceObjects.Length];
		var objectIndex = 0;
		for (;
			objectIndex < sourceObjects.Length;
			objectIndex++)
		{
			sourceObjects[objectIndex] = new ClassD();
			objectObjects[objectIndex] = null;
		}

		objectIndex = 0;
		var stopwatch = new Stopwatch();
		stopwatch.Start();
		foreach (var sourceObject in sourceObjects)
		{
			objectObjects[objectIndex] = sourceObject.CloneShallow();
			objectIndex++;
		}
		stopwatch.Stop();

		var testResult
			= "\r\n"
			+ $"“CloneWithSamePropertiesProperties”了 {sourceObjects.Length} 个对象，"
			+ $"耗时： {stopwatch.ElapsedMilliseconds} 毫秒，"
			+ $"平均每次： {(double)stopwatch.ElapsedMilliseconds / sourceObjects.Length:F3} 毫秒。"
			+ "\r\n";
		{ }
		Debug.WriteLine(testResult);


		//var testResultPath
		//	= "d:\\"
		//	+ "CloneWithSamePropertiesPropertiesPerformaceTest"
		//	+ DateTime.Now.ToString("yy_MMdd_HHmm_sss")
		//	+ ".txt";
		//{
		//	System.IO.File.WriteAllText(testResultPath, testResult);
		//}
	}


	protected struct StructA
	{
		public int IntProperty { get; set; }

		public string? StringProperty { get; set; }

		public ClassForGeneratePropertyValueBytes2? ObjectProperty { get; set; }

		public int[]? IntsProperty { get; set; }
	}


	protected class ClassForGeneratePropertyValueBytes2
	{
		public int IntProperty { get; set; }
		public string? StringProperty { get; set; }

		public string[]? StringsProperty { get; set; }
	}

	protected class ClassForGeneratePropertyValueBytes
	{
		public int IntProperty { get; set; }

		public float FloatProperty { get; set; }

		public double DoubleProperty { get; set; }

		public decimal DecimalProperty { get; set; }

		public string? StringProperty { get; set; }

		public DateTime DateTimeProperty { get; set; }

		public ClassForGeneratePropertyValueBytes2 ObjectProperty { get; set; } = new();

		public StructA StructProperty { get; set; } = new();

		public Dictionary<string, int> DictionaryProperty { get; set; } = [];

	}



	[TestMethod]
	public void GeneratePropertyValueBytesTest()
	{
		var testObject = new ClassForGeneratePropertyValueBytes();
		{
			testObject.IntProperty = 1;
			testObject.FloatProperty = 2.0F;
			testObject.DoubleProperty = 3.0;
			testObject.DecimalProperty = 4.0m;
			testObject.StringProperty = "Abc";
			testObject.DateTimeProperty = DateTime.Now;

			testObject.ObjectProperty = new()
			{
				IntProperty = 11,
				StringProperty = "Def",
				StringsProperty = ["Xyz", "Xyz", "Xyz"]
			};
			testObject.DateTimeProperty = DateTime.Now;

			testObject.StructProperty = new StructA()
			{
				IntProperty = 12,
				StringProperty = "Ghi"
			};

			testObject.DictionaryProperty = new()
			{
				{ "A", 1 },
				{ "B", 2 },
				{ "C", 3 }
			};
		}
		var testObjectPropertyValueBytesA
			= testObject.ToBytes(
				null,
				true,
				System.Reflection.BindingFlags.Default);
		{
			testObject.ObjectProperty.IntProperty = 13;
		}
		var testObjectPropertyValueBytesB
			= testObject.ToBytes(
				null,
				true,
				System.Reflection.BindingFlags.Default);
		// !!!
		Assert.AreEqual(testObjectPropertyValueBytesB.Length, testObjectPropertyValueBytesA.Length
);
		// !!!
		for (var byteIndex = 0;
			byteIndex < testObjectPropertyValueBytesA.Length;
			byteIndex++)
		{
			var byteA = testObjectPropertyValueBytesA[byteIndex];
			var byteB = testObjectPropertyValueBytesB[byteIndex];
			if (byteA != byteB)
			{
				// !!!
				Assert.AreEqual(57, byteIndex);
				// !!!
			}
		}
	}

	class ClassCloneTestA
	{
		public int IntProperty { get; set; }

		public float FloatProperty { get; set; }

		public double DoubleProperty { get; set; }

		public decimal DecimalProperty { get; set; }

		public string? StringProperty { get; set; }

		public DateTime DateTimeProperty { get; set; }

		public object? ObjectProperty { get; set; }

		public int[]? IntItems { get; set; }

		public float[]? FloatItems { get; set; }

		public double[]? DoubleItems { get; set; }

		public decimal[]? DecimalItems { get; set; }

		public string[]? StringItems { get; set; }

		public ClassCloneTestB[]? ObjectItems { get; set; }

		public Dictionary<string, int>? KeyValueItems { get; set; }
	}

	class ClassCloneTestB
	{
		public int BIntValue { get; set; }

		public float BFloatValue { get; set; }

		public string? BStringValue { get; set; }

		public override bool Equals(object? obj)
		{
			if (obj is not ClassCloneTestB anotherObjectB)
			{
				return false;
			}

			if (BIntValue != anotherObjectB.BIntValue
				|| BFloatValue != anotherObjectB.BFloatValue
				|| BStringValue != anotherObjectB.BStringValue)
			{
				return false;
			}

			return true;
		}

		public override int GetHashCode()
		{
			return (BIntValue
				+ BFloatValue
				+ BStringValue)
				.GetHashCode();
		}
	}

	[TestMethod]
	public void CloneTest()
	{
		var item = new ClassCloneTestA()
		{
			IntProperty = 1,
			FloatProperty = 2.0F,
			DoubleProperty = 3.0,
			DecimalProperty = 4.0M,
			StringProperty = "Abc",
			ObjectProperty = new ClassB()
			{
				IntProperty = 101,
				FloatProperty = 102.0F,
				DoubleProperty = 103.0,
				DecimalProperty = 104.0M,
				StringProperty = "10Abc"
			},
			IntItems =
			[
				1,
				2,
				3
			],
			FloatItems =
			[
				4.0F,
				5.0F,
				6.0F
			],
			DoubleItems =
			[
				7.0,
				8.0,
				9.0
			],
			DecimalItems =
			[
				10.0M,
				11.0M,
				12.0M
			],
			StringItems =
			[
				"A",
				"b",
				"c"
			],
			ObjectItems =
			[
				new()
				{
					BIntValue = 101,
					BFloatValue = 102.0F,
					BStringValue = "B_Abc"
				},
				new()
				{
					BIntValue = 201,
					BFloatValue = 202.0F,
					BStringValue = "B_Def"
				}
			],
			KeyValueItems = new()
			{
				{ "Aaa", 1 },
				{ "Bbb", 2 },
				{ "Ccc", 3 }
			}
		};

		var itemCloned = item.CloneDeep();
		var itemCloned2 = item.CloneDeep();
		////////////////////////////////////////////////
		// 1/2，克隆后，默认属性一致：
		////////////////////////////////////////////////
		{
			Assert.AreEqual(itemCloned.IntProperty, item.IntProperty);
			Assert.AreEqual(itemCloned.FloatProperty, item.FloatProperty);
			Assert.AreEqual(itemCloned.DoubleProperty, item.DoubleProperty);
			Assert.AreEqual(itemCloned.DecimalProperty, item.DecimalProperty);
			Assert.IsTrue(item.StringProperty!.Equals(itemCloned.StringProperty));

			Assert.IsTrue(ArrayExtension.IsItemsEqual(item.IntItems, itemCloned.IntItems));
			Assert.IsTrue(ArrayExtension.IsItemsEqual(item.FloatItems, itemCloned.FloatItems));
			Assert.IsTrue(ArrayExtension.IsItemsEqual(item.DoubleItems, itemCloned.DoubleItems));
			Assert.IsTrue(ArrayExtension.IsItemsEqual(item.DecimalItems, itemCloned.DecimalItems));
			Assert.IsTrue(ArrayExtension.IsItemsEqual(item.StringItems, itemCloned.StringItems));
			Assert.IsTrue(ArrayExtension.IsItemsEqual(item.ObjectItems, itemCloned.ObjectItems));

			Assert.AreEqual(itemCloned.KeyValueItems!.Count, item.KeyValueItems!.Count);
			foreach (var keyValue in item.KeyValueItems)
			{
				var value2 = itemCloned.KeyValueItems.GetValueOrDefault(keyValue.Key);
				//
				Assert.IsTrue(value2.Equals(keyValue.Value));
				//
			}
		}


		////////////////////////////////////////////////
		// 2/3，修改克隆对象，不影响原有对象：
		////////////////////////////////////////////////
		{
			itemCloned2.IntProperty++;
			itemCloned2.FloatProperty++;
			itemCloned2.DoubleProperty++;
			itemCloned2.DecimalProperty++;
			itemCloned2.StringProperty += "+1";

			for (var itemIndex = 0;
				itemIndex < itemCloned2.IntItems!.Length;
				itemIndex++)
			{
				itemCloned2.IntItems[itemIndex] = itemCloned2.IntItems[itemIndex] + 10;
			}
			for (var itemIndex = 0;
				itemIndex < itemCloned2.FloatItems!.Length;
				itemIndex++)
			{
				itemCloned2.FloatItems[itemIndex] = itemCloned2.FloatItems[itemIndex] + 10;
			}
			for (var itemIndex = 0;
				itemIndex < itemCloned2.DoubleItems!.Length;
				itemIndex++)
			{
				itemCloned2.DoubleItems[itemIndex] = itemCloned2.DoubleItems[itemIndex] + 10;
			}
			for (var itemIndex = 0;
				itemIndex < itemCloned2.DecimalItems!.Length;
				itemIndex++)
			{
				itemCloned2.DecimalItems[itemIndex] = itemCloned2.DecimalItems[itemIndex] + 10;
			}
			for (var itemIndex = 0;
				itemIndex < itemCloned2.StringItems!.Length;
				itemIndex++)
			{
				itemCloned2.StringItems[itemIndex] = itemCloned2.StringItems[itemIndex] + 10;
			}
			for (var itemIndex = 0;
				itemIndex < itemCloned2.ObjectItems!.Length;
				itemIndex++)
			{
				itemCloned2.ObjectItems[itemIndex].BIntValue
					= itemCloned2.ObjectItems[itemIndex].BIntValue + 10;
				itemCloned2.ObjectItems[itemIndex].BFloatValue
					= itemCloned2.ObjectItems[itemIndex].BFloatValue + 10;
				itemCloned2.ObjectItems[itemIndex].BStringValue
					= itemCloned2.ObjectItems[itemIndex].BStringValue + "+10";
			}

			Dictionary<string, int> newKeyValues = [];
			foreach (var keyValue in itemCloned2.KeyValueItems!)
			{
				newKeyValues.AddOrSet(
					keyValue.Key,
					keyValue.Value + 10);
			}
			foreach (var newKeyValue in newKeyValues)
			{
				itemCloned2.KeyValueItems.AddOrSet(
					newKeyValue.Key,
					newKeyValue.Value);
			}
		}

		////////////////////////////////////////////////
		// 3/3，克隆后，修改克隆对象，不影响原有对象：
		////////////////////////////////////////////////
		{
			Assert.AreEqual(itemCloned.IntProperty, item.IntProperty);
			Assert.AreEqual(itemCloned.FloatProperty, item.FloatProperty);
			Assert.AreEqual(itemCloned.DoubleProperty, item.DoubleProperty);
			Assert.AreEqual(itemCloned.DecimalProperty, item.DecimalProperty);
			Assert.IsTrue(item.StringProperty!.Equals(itemCloned.StringProperty));

			Assert.IsTrue(ArrayExtension.IsItemsEqual(item.IntItems, itemCloned.IntItems));
			Assert.IsTrue(ArrayExtension.IsItemsEqual(item.FloatItems, itemCloned.FloatItems));
			Assert.IsTrue(ArrayExtension.IsItemsEqual(item.DoubleItems, itemCloned.DoubleItems));
			Assert.IsTrue(ArrayExtension.IsItemsEqual(item.DecimalItems, itemCloned.DecimalItems));
			Assert.IsTrue(ArrayExtension.IsItemsEqual(item.StringItems, itemCloned.StringItems));
			Assert.IsTrue(ArrayExtension.IsItemsEqual(item.ObjectItems, itemCloned.ObjectItems));

			Assert.AreEqual(itemCloned.KeyValueItems!.Count, item.KeyValueItems!.Count);
			foreach (var keyValue in item.KeyValueItems)
			{
				var value2 = itemCloned.KeyValueItems.GetValueOrDefault(keyValue.Key);
				//
				Assert.IsTrue(value2.Equals(keyValue.Value));
				//
			}

			////////////////////////////////////////////////


			Assert.AreNotEqual(itemCloned2.IntProperty, item.IntProperty);
			Assert.AreNotEqual(itemCloned2.FloatProperty, item.FloatProperty);
			Assert.AreNotEqual(itemCloned2.DoubleProperty, item.DoubleProperty);
			Assert.AreNotEqual(itemCloned2.DecimalProperty, item.DecimalProperty);
			Assert.AreNotEqual(true, item.StringProperty!.Equals(itemCloned2.StringProperty));

			Assert.AreEqual(0, ArrayExtension.GetSameItemsCount(item.IntItems, itemCloned2.IntItems));
			Assert.AreEqual(0, ArrayExtension.GetSameItemsCount(item.FloatItems, itemCloned2.FloatItems));
			Assert.AreEqual(0, ArrayExtension.GetSameItemsCount(item.DoubleItems, itemCloned2.DoubleItems));
			Assert.AreEqual(0, ArrayExtension.GetSameItemsCount(item.DecimalItems, itemCloned2.DecimalItems));
			Assert.AreEqual(0, ArrayExtension.GetSameItemsCount(item.StringItems, itemCloned2.StringItems));
			Assert.AreEqual(0, ArrayExtension.GetSameItemsCount(item.ObjectItems, itemCloned2.ObjectItems));

			Assert.AreEqual(itemCloned2.KeyValueItems!.Count, item.KeyValueItems!.Count);
			foreach (var keyValue in item.KeyValueItems)
			{
				var value2 = itemCloned2.KeyValueItems.GetValueOrDefault(keyValue.Key);
				//
				Assert.AreNotEqual(true, value2.Equals(keyValue.Value));
				//
			}
		}
	}

	class ClassCloneTestC
	{
		public int Id { get; set; }

		public ClassCloneTestD? ObjectD { get; set; }
	}

	class ClassCloneTestD
	{
		public int Id { get; set; }

		public ClassCloneTestC? ObjectC { get; set; }
	}

	[TestMethod]
	public void CloneForRecursiveReferenceTest()
	{
		var objectC = new ClassCloneTestC()
		{
			Id = 101
		};
		var objectD = new ClassCloneTestD()
		{
			Id = 201
		};

		objectC.ObjectD = objectD;
		objectD.ObjectC = objectC;

		var objectCCloned = objectC.CloneDeep();
		var objectDCloned = objectCCloned.ObjectD!;
		{
			Assert.AreEqual(objectC.Id, objectCCloned.Id);
			Assert.AreEqual(objectD.Id, objectDCloned.Id);
		}
		{
			objectCCloned.Id += 1;
			objectDCloned.Id += 1;
		}
		{
			Assert.AreEqual(objectC.Id + 1, objectCCloned.Id);
			Assert.AreEqual(objectD.Id + 1, objectDCloned.Id);
		}
	}

	[TestMethod]
	public void CloneWithPropertyLayerIndexMaxTest()
	{
		var item = new ClassCloneTestA()
		{
			IntProperty = 1,
			FloatProperty = 2.0F,
			DoubleProperty = 3.0,
			DecimalProperty = 4.0M,
			StringProperty = "Abc",
			ObjectProperty = new ClassB()
			{
				IntProperty = 101,
				FloatProperty = 102.0F,
				DoubleProperty = 103.0,
				DecimalProperty = 104.0M,
				StringProperty = "10Abc"
			},
			IntItems =
			[
				1,
				2,
				3
			],
			FloatItems =
			[
				4.0F,
				5.0F,
				6.0F
			],
			DoubleItems =
			[
				7.0,
				8.0,
				9.0
			],
			DecimalItems =
			[
				10.0M,
				11.0M,
				12.0M
			],
			StringItems =
			[
				"A",
				"b",
				"c"
			],
			ObjectItems =
			[
				new()
				{
					BIntValue = 101,
					BFloatValue = 102.0F,
					BStringValue = "B_Abc"
				},
				new()
				{
					BIntValue = 201,
					BFloatValue = 202.0F,
					BStringValue = "B_Def"
				}
			],
			KeyValueItems = new()
			{
				{ "Aaa", 1 },
				{ "Bbb", 2 },
				{ "Ccc", 3 }
			}
		};

		var itemCloned = item.CloneDeep(null, BindingFlags.Default, null, 1);
		{
			Assert.AreEqual(itemCloned.IntProperty, item.IntProperty);
			Assert.AreEqual(itemCloned.FloatProperty, item.FloatProperty);
			Assert.AreEqual(itemCloned.DoubleProperty, item.DoubleProperty);
			Assert.AreEqual(itemCloned.DecimalProperty, item.DecimalProperty);
			Assert.IsTrue(item.StringProperty!.Equals(itemCloned.StringProperty));

			// 基础类型的容器，设为值类型。
			Assert.IsTrue(ArrayExtension.IsItemsEqual(item.IntItems, itemCloned.IntItems));
			Assert.IsTrue(ArrayExtension.IsItemsEqual(item.FloatItems, itemCloned.FloatItems));
			Assert.IsTrue(ArrayExtension.IsItemsEqual(item.DoubleItems, itemCloned.DoubleItems));
			Assert.IsTrue(ArrayExtension.IsItemsEqual(item.DecimalItems, itemCloned.DecimalItems));
			Assert.IsTrue(ArrayExtension.IsItemsEqual(item.StringItems, itemCloned.StringItems));

			Assert.AreEqual(itemCloned.ObjectItems!.Length, item.ObjectItems!.Length);
			Assert.AreNotEqual(true
, ArrayExtension.IsItemsEqual(item.ObjectItems, itemCloned.ObjectItems)
				// !!!
				);
			foreach (var objectB in itemCloned.ObjectItems)
			{
				Assert.AreEqual(0, objectB.BIntValue);
				Assert.AreEqual(0, objectB.BFloatValue);
				Assert.AreEqual(0, objectB.BFloatValue);
				Assert.IsNull(objectB.BStringValue);
			}

			Assert.AreEqual(itemCloned.KeyValueItems!.Count, item.KeyValueItems!.Count);
			foreach (var keyValue in item.KeyValueItems)
			{
				var valueCloned = itemCloned.KeyValueItems.GetValueOrDefault(keyValue.Key);
				//
				Assert.IsTrue(valueCloned.Equals(keyValue.Value));
				//
			}
		}
	}


	public class TestEntity
	{
		public int Id { get; set; }

		public int? Number { get; set; }

		public string Name { get; set; } = default!;

		public string? Description { get; set; }

		public IpInfo IpInfoNotNull { get; set; } = default!;

		public IpInfo? IpInfoNullable { get; set; }
	}

	public class IpInfo
	{
		public string IpAddress { get; set; } = default!;

		public string? ProxyInfo { get; set; }
	}

	[TestMethod]
	public void GetPropertyInfesFromTest()
	{
		var nullabilityInfoContext = new NullabilityInfoContext();
		var testEntityPropertyInfesNotNull
			= typeof(TestEntity).GetObjectPropertyInfes(
			(objectPropertyInfo) =>
			{
				// 值类型，【不需要】非空约束：
				if (objectPropertyInfo.IsPropertyTypeValue)
				{
					return false;
				}
				// 拥有【可为空特性】，
				// 则该类型可为空，不需要约束： 
				if (objectPropertyInfo.IsPropertyTypeNullable(nullabilityInfoContext))
				{
					return false;
				}

				////////////////////////////////////////////////
				// 非值类型，
				// 且，没有【可为空特性】，
				// 【需要】非空约束：
				//////////////////////////////////////////////// 
				return true;
			});

		// “Name”，“IpInfoNotNull”
		Assert.AreEqual(2, testEntityPropertyInfesNotNull.Count);
		Assert.IsTrue(testEntityPropertyInfesNotNull[0].Name.Equals(nameof(TestEntity.Name)));
		Assert.IsTrue(testEntityPropertyInfesNotNull[1].Name.Equals(nameof(TestEntity.IpInfoNotNull)));
		Assert.AreEqual(1, testEntityPropertyInfesNotNull[1].ChildObjectPropertyInfes.Length);
		Assert.IsTrue(testEntityPropertyInfesNotNull[1].ChildObjectPropertyInfes[0].Name.Equals(nameof(IpInfo.IpAddress)));
	}

	public class TestEntityWithRecursionProperty
	{
		public int Id { get; set; }

		public int? Number { get; set; }

		public string Name { get; set; } = default!;

		public string? Description { get; set; }

		public IpInfo IpInfoNotNull { get; set; } = default!;

		public IpInfo? IpInfoNullable { get; set; }

		public TestEntityWithRecursionProperty ChildTestEntity { get; set; } = default!;
	}

	[TestMethod]
	public void GetPropertyInfesFromWithRecursionPropertyTest()
	{
		var nullabilityInfoContext = new NullabilityInfoContext();
		var testEntityPropertyInfesNotNull
			= typeof(TestEntityWithRecursionProperty).GetObjectPropertyInfes(
			(objectPropertyInfo) =>
			{
				// 值类型，【不需要】非空约束：
				if (objectPropertyInfo.IsPropertyTypeValue)
				{
					return false;
				}
				// 拥有【可为空特性】，
				// 则该类型可为空，不需要约束： 
				if (objectPropertyInfo.IsPropertyTypeNullable(nullabilityInfoContext))
				{
					return false;
				}

				////////////////////////////////////////////////
				// 非值类型，
				// 且，没有【可为空特性】，
				// 【需要】非空约束：
				//////////////////////////////////////////////// 
				return true;
			});


		////////////////////////////////////////////////
		////////////////////////////////////////////////
		////////////////////////////////////////////////
		// !!! "如果没有合理含有递归子属性的类型， !!!
		// !!! 则测试程序不会运行至此处。               !!!
		////////////////////////////////////////////////
		////////////////////////////////////////////////
		////////////////////////////////////////////////


		// “Name”，“IpInfoNotNull”
		Assert.AreEqual(3, testEntityPropertyInfesNotNull.Count);
		Assert.IsTrue(testEntityPropertyInfesNotNull[0].Name.Equals(nameof(TestEntityWithRecursionProperty.Name)));
		Assert.IsTrue(testEntityPropertyInfesNotNull[1].Name.Equals(nameof(TestEntityWithRecursionProperty.IpInfoNotNull)));
		Assert.AreEqual(1, testEntityPropertyInfesNotNull[1].ChildObjectPropertyInfes.Length);
		Assert.IsTrue(testEntityPropertyInfesNotNull[1].ChildObjectPropertyInfes[0].Name.Equals(nameof(IpInfo.IpAddress)));
		Assert.IsTrue(testEntityPropertyInfesNotNull[2].Name.Equals(nameof(TestEntityWithRecursionProperty.ChildTestEntity)));
	}

	[TestMethod]
	public void NullabilityInfoContextTest()
	{
		var nullabilityInfoContext = new NullabilityInfoContext();
		var testEntityPropertyInfesNotNull
			= typeof(TestEntity).GetObjectPropertyInfes(
			(objectPropertyInfo) =>
			{
				// 值类型，【不需要】非空约束：
				if (objectPropertyInfo.IsPropertyTypeValue)
				{
					return false;
				}
				// 拥有【可为空特性】，
				// 则该类型可为空，不需要约束： 
				if (objectPropertyInfo.IsPropertyTypeNullable(nullabilityInfoContext))
				{
					return false;
				}

				////////////////////////////////////////////////
				// 非值类型，
				// 且，没有【可为空特性】，
				// 【需要】非空约束：
				//////////////////////////////////////////////// 
				return true;
			});
		ObjectPropertyInfo[] testEntityPropertyInfoArrayNotNull = [.. testEntityPropertyInfesNotNull];


		////////////////////////////////////////////////


		var entityValid = new TestEntity()
		{
			Name = "TestEntity",
			IpInfoNotNull = new IpInfo()
			{
				IpAddress = "TestIpAddress"
			}
		};
		var checkError = entityValid.CheckPropertyValuesWithObjectPropertyInfes<string>(
			testEntityPropertyInfoArrayNotNull,
			(propertyOwner, propertyInfo, propertyValue) =>
			{
				if (propertyValue == null)
				{
					return $"数据无效，属性“{propertyInfo.Name}”为“null”。";
				}
				return null;
			});
		Assert.IsTrue(string.IsNullOrEmpty(checkError));


		////////////////////////////////////////////////


		var entityInvalid = new TestEntity()
		{
#pragma warning disable CS8625 // 无法将 null 字面量转换为非 null 的引用类型。
			Name = null,
#pragma warning restore CS8625 // 无法将 null 字面量转换为非 null 的引用类型。
			IpInfoNotNull = new IpInfo()
			{
				IpAddress = "TestIpAddress"
			}
		};
		checkError = entityInvalid.CheckPropertyValuesWithObjectPropertyInfes<string>(
			testEntityPropertyInfoArrayNotNull,
			(propertyOwner, propertyInfo, propertyValue) =>
			{
				if (propertyValue == null)
				{
					return $"数据无效，属性“{propertyInfo.Name}”为“null”。";
				}
				return null;
			});
		Assert.IsTrue(checkError!.Equals($"数据无效，属性“{nameof(TestEntity.Name)}”为“null”。"));


		////////////////////////////////////////////////


		entityInvalid = new TestEntity()
		{
			Name = "TestEntity",
#pragma warning disable CS8625 // 无法将 null 字面量转换为非 null 的引用类型。
			IpInfoNotNull = null
#pragma warning restore CS8625 // 无法将 null 字面量转换为非 null 的引用类型。
		};
		checkError = entityInvalid.CheckPropertyValuesWithObjectPropertyInfes<string>(
			testEntityPropertyInfoArrayNotNull,
			(propertyOwner, propertyInfo, propertyValue) =>
			{
				if (propertyValue == null)
				{
					return $"数据无效，属性“{propertyInfo.Name}”为“null”。";
				}
				return null;
			});
		Assert.IsTrue(checkError!.Equals($"数据无效，属性“{nameof(TestEntity.IpInfoNotNull)}”为“null”。"));


		////////////////////////////////////////////////


		entityInvalid = new TestEntity()
		{
			Name = "TestEntity",
			IpInfoNotNull = new IpInfo()
			{
#pragma warning disable CS8625 // 无法将 null 字面量转换为非 null 的引用类型。
				IpAddress = null
#pragma warning restore CS8625 // 无法将 null 字面量转换为非 null 的引用类型。
			}
		};
		checkError = entityInvalid.CheckPropertyValuesWithObjectPropertyInfes<string>(
			testEntityPropertyInfoArrayNotNull,
			(propertyOwner, propertyInfo, propertyValue) =>
			{
				if (propertyValue == null)
				{
					return $"数据无效，属性“{propertyInfo.PropertyLinkName}”为“null”。";
				}
				return null;
			});
		Assert.IsTrue(checkError!.Equals($"数据无效，属性“{nameof(TestEntity.IpInfoNotNull)}.{nameof(IpInfo.IpAddress)}”为“null”。"));
	}

	class ToBytesObject
	{
		public byte ByteValue0 { get; set; } = 0;
		public byte ByteValue1 { get; set; } = 1;
		public byte ByteValue2 { get; set; } = 2;
		public byte[] ByteValue { get; set; } = [3, 4, 5];
		public string StringValue { get; set; } = "abc";
	}

	[TestMethod]
	public void ToBytesTest()
	{
		var toBytesObject = new ToBytesObject();
		var objectBytes = toBytesObject.ToBytes();
		{
			Assert.IsTrue(objectBytes.Length > 0);
		}
	}

	[TestMethod]
	public void ToBytes_Bytes_Test()
	{
		byte[] bytes = [0, 1, 2, 3, 4, 5];
		var bytesSegment = new ArraySegment<byte>(bytes, 0, 3);

		var objectBytes = bytesSegment.ToBytes();
		{
			Assert.IsTrue(objectBytes.Length > 0);
		}
	}

	class IndexProperty
	{
		public int[] _numbers = { 1, 2, 3 };

		public int this[int index]
		{
			get => _numbers[index];
			set => _numbers[index] = value;
		}
	}

	class ToBytesObjectWithIndexProperty
	{
		public byte ByteValue0 { get; set; } = 0;
		public byte ByteValue1 { get; set; } = 1;
		public byte ByteValue2 { get; set; } = 2;
		public byte[] ByteValue { get; set; } = [3, 4, 5];
		public string StringValue { get; set; } = "abc";
		public IndexProperty IndexProperty { get; set; } = new();
	}

	[TestMethod]
	public void ToBytes_ToBytesObjectWithIndexProperty_Test()
	{
		var toBytesObject = new ToBytesObjectWithIndexProperty();
		var objectBytes = toBytesObject.ToBytes();
		{
			Assert.IsTrue(objectBytes.Length > 0);
		}
	}
}