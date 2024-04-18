using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Reflection;

namespace BaoXia.Utils.Test.ExtensionsTest;

[TestClass]
public class ObjectExtensionTest
{
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

		public int FloatProperty { get; set; }

		public double DoubleProperty { get; set; }

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
			"StringProperty",
			"DateTimeProperty",
			"ObjectProperty",

			"IntFieldProperty",
			"FloatFieldProperty",
			"StringFieldProperty"
		};

		var objectAllPublicSetableProperties = objectA.GetPublicSetablePropertyInfes();

		Assert.IsTrue(objectAllPublicSetableProperties?.Length == allPublisSetablePropertyNames.Length);
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
			Assert.IsTrue(nameof(ClassA.StringProperty) == nameOfProperty);
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
				Value = "ClassA.ObjectProperty"
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
				Value = "ClassB.ObjectProperty"
			}
		};


		objectA.SetPropertiesWithSameNameFrom(objectB);


		////////////////////////////////////////////////

		Assert.IsTrue(objectA.IntProperty == objectB.IntProperty);
		Assert.IsTrue(objectA.FloatProperty != objectB.FloatProperty);
		Assert.IsTrue(objectA.DoubleProperty != objectB.DoubleProperty);
		Assert.IsTrue(objectA.StringProperty == objectB.StringProperty);
		Assert.IsTrue(objectA.DateTimeProperty == objectB.DateTimeProperty);
		Assert.IsTrue(objectA.ObjectProperty == objectB.ObjectProperty);

		Assert.IsTrue(objectA.IntFieldProperty == objectB.IntFieldProperty);
		Assert.IsTrue(objectA.FloatFieldProperty != objectB.FloatFieldProperty);
		Assert.IsTrue(objectA.StringFieldProperty == objectB.StringFieldProperty);

		////////////////////////////////////////////////

	}

	[TestMethod]
	public void CloneWithSamePropertiesPropertiesTest()
	{
		var objA = new ClassA()
		{
			StringProperty = "Abc"
		};
		var objB = objA.CloneWithSameProperties();

		// !!!
		Assert.IsTrue(objA != objB);
		// !!!
		// !!!
		Assert.IsTrue(objA.StringProperty == objB?.StringProperty);
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
			objectObjects[objectIndex] = sourceObject.CloneWithSameProperties();
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



	[TestMethod]
	public void CloneWithSamePropertiesPropertiesRecursivlyTest()
	{
		var item = new ClassA()
		{
			IntFieldProperty = 1,
			FloatProperty = 2.0F,
			DoubleProperty = 3.0,
			DecimalProperty = 4.0M,
			StringProperty = "Abc",
			ObjectProperty = new ClassA()
			{
				IntFieldProperty = 101,
				FloatProperty = 2.0F,
				DoubleProperty = 3.0,
				DecimalProperty = 4.0M,
				StringProperty = "Abc", @last
			}
		};
		var itemCloned = item.CloneWithSamePropertiesRecursivly();
		{
			Assert.IsNotNull(item.IntFieldProperty == itemCloned.IntFieldProperty);
			Assert.IsNotNull(item.FloatProperty == itemCloned.FloatProperty);
			Assert.IsNotNull(item.DoubleProperty == itemCloned.DoubleProperty);
			Assert.IsNotNull(item.DecimalProperty == itemCloned.DecimalProperty);
			Assert.IsNotNull(item.StringProperty!.Equals(itemCloned.StringProperty));
		}
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
		}
		var testObjectPropertyValueBytesA
			= testObject.GeneratePropertyValueBytes(
				null,
				System.Reflection.BindingFlags.Default,
				true);
		{
			testObject.ObjectProperty.IntProperty = 13;
		}
		var testObjectPropertyValueBytesB
			= testObject.GeneratePropertyValueBytes(
				null,
				System.Reflection.BindingFlags.Default,
				true);
		// !!!
		Assert.IsTrue(testObjectPropertyValueBytesA.Length
			== testObjectPropertyValueBytesB.Length);
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
				Assert.IsTrue(byteIndex == 51);
				// !!!
			}
		}
	}
}
