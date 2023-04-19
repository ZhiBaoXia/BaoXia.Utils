using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BaoXia.Utils.Test.ExtensionsTest
{
        [TestClass]
        public class ObjectExtensionTest
        {
                protected class ClassA
                {
                        public int IntProperty { get; set; }

                        public float FloatProperty { get; set; }

                        public float DoubleProperty { get; set; }

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
                                else if (objectPublicSetableProperty.Name.Contains("_", StringComparison.OrdinalIgnoreCase))
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
                public void CloneWithSamePropertiesProperties()
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
        }
}
