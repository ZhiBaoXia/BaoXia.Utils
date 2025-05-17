using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Text.Json;

namespace BaoXia.Utils.Test.ExtensionsTest
{
	[TestClass]
	public class JsonElementExtensionTest
	{
		public const string JsonText = "{\"boolValue\":true, \"intValue\":1,\"floatValue\":2.0,\"doubleValue\":3.0,\"stringValue\":\"4.0\",\"objectValue\":{\"propertyName\":\"属性名称\", \"propertyValue\":\"属性值\"}}";

		public const string JsonTextWithUppercasePropetyName = "{\"boolValue\":true, \"intValue\":1,\"floatValue\":2.0,\"doubleValue\":3.0,\"stringValue\":\"4.0\",\"objectValue\":{\"propertyName\":\"属性名称\", \"propertyValue\":\"属性值\"}}";

		public const string JsonTextWihtInvalidValue = "{\"boolValue\":\"true\", \"intValue\":\"1\",\"floatValue\":\"2.0\",\"doubleValue\":\"3.0\",\"stringValue\":4.0,\"objectValue\":true}";

		class ObjectValue
		{
			public string? PropertyName { get; set; }
			public string? PropertyValue { get; set; }

			public override bool Equals(object? obj)
			{
				if (obj is ObjectValue @objectValue)
				{
					if (StringExtension.Equals(PropertyName, @objectValue.PropertyName)
							&& StringExtension.Equals(PropertyValue, @objectValue.PropertyValue))
					{
						return true;
					}
				}
				return false;
			}

			public override int GetHashCode()
			{
				return ((PropertyName ?? string.Empty) + (PropertyValue ?? string.Empty))
					.GetHashCode();
			}
		}

		[TestMethod]
		public void TryGetPropertyTest()
		{
			var jsonDocument = JsonDocument.Parse(
				JsonText,
				new JsonDocumentOptions()
				{
					AllowTrailingCommas = true,
					CommentHandling = JsonCommentHandling.Skip,

				});
			var jsonElement = jsonDocument.RootElement;

			var boolValue = jsonElement.TryGetBoolProperty("boolValue");
			Assert.IsTrue(boolValue);
			var intValue = jsonElement.TryGetIntProperty("intValue");
			Assert.AreEqual(1, intValue);
			var floatValue = jsonElement.TryGetFloatProperty("floatValue");
			Assert.AreEqual(2.0F, floatValue);
			var doubleValue = jsonElement.TryGetDoubleProperty("doubleValue");
			Assert.AreEqual(3.0, doubleValue);
			var stringValue = jsonElement.TryGetStringProperty("stringValue");
			Assert.AreEqual("4.0", stringValue);
			var objectValue = jsonElement.TryGetObjectProperty<ObjectValue>(
				"objectValue",
				null,
				true,
				new JsonSerializerOptions()
				{
					PropertyNameCaseInsensitive = true
				});
			Assert.IsTrue(objectValue?.Equals(new ObjectValue()
			{
				PropertyName = "属性名称",
				PropertyValue = "属性值"
			}));
		}

		[TestMethod]
		public void TryGetPropertyWithIgnoreCaseTest()
		{
			var jsonDocument = JsonDocument.Parse(
				JsonTextWithUppercasePropetyName,
				new JsonDocumentOptions()
				{
					AllowTrailingCommas = true,
					CommentHandling = JsonCommentHandling.Skip,

				});
			var jsonElement = jsonDocument.RootElement;

			var boolValue = jsonElement.TryGetBoolProperty("boolValue");
			Assert.IsTrue(boolValue);
			var intValue = jsonElement.TryGetIntProperty("intValue");
			Assert.AreEqual(1, intValue);
			var floatValue = jsonElement.TryGetFloatProperty("floatValue");
			Assert.AreEqual(2.0F, floatValue);
			var doubleValue = jsonElement.TryGetDoubleProperty("doubleValue");
			Assert.AreEqual(3.0, doubleValue);
			var stringValue = jsonElement.TryGetStringProperty("stringValue");
			Assert.AreEqual("4.0", stringValue);
			var objectValue = jsonElement.TryGetObjectProperty<ObjectValue>(
				"objectValue",
				null,
				true,
				new JsonSerializerOptions()
				{
					PropertyNameCaseInsensitive = true
				});
			Assert.IsTrue(objectValue?.Equals(new ObjectValue()
			{
				PropertyName = "属性名称",
				PropertyValue = "属性值"
			}));
		}

		[TestMethod]
		public void TryGetPropertyWithDefaultValue()
		{
			var jsonDocument = JsonDocument.Parse(
				JsonTextWihtInvalidValue,
				new JsonDocumentOptions()
				{
					AllowTrailingCommas = true,
					CommentHandling = JsonCommentHandling.Skip,

				});
			var jsonElement = jsonDocument.RootElement;

			var boolValue = jsonElement.TryGetBoolProperty("boolValue", true);
			Assert.IsTrue(boolValue);
			var intValue = jsonElement.TryGetIntProperty("intValue", 1);
			Assert.AreEqual(1, intValue);
			var floatValue = jsonElement.TryGetFloatProperty("floatValue", 2.0F);
			Assert.AreEqual(2.0F, floatValue);
			var doubleValue = jsonElement.TryGetDoubleProperty("doubleValue", 3.0);
			Assert.AreEqual(3.0, doubleValue);
			var stringValue = jsonElement.TryGetStringProperty("stringValue", "4.0");
			Assert.AreEqual("4.0", stringValue);
			var objectValue = jsonElement.TryGetObjectProperty<ObjectValue>(
				"objectValue",
				new ObjectValue()
				{
					PropertyName = "属性名称",
					PropertyValue = "属性值"
				},
				true,
				new JsonSerializerOptions()
				{
					PropertyNameCaseInsensitive = true
				});
			Assert.IsTrue(objectValue?.Equals(new ObjectValue()
			{
				PropertyName = "属性名称",
				PropertyValue = "属性值"
			}));
		}

		[TestMethod]
		public void JsonElementEnumerateObjectPerformance()
		{
			var testsCount = 1000000;
			var jsonDocument = JsonDocument.Parse(
				JsonText,
				new JsonDocumentOptions()
				{
					AllowTrailingCommas = true,
					CommentHandling = JsonCommentHandling.Skip,

				});
			var jsonElement = jsonDocument.RootElement;


			////////////////////////////////////////////////
			// 1/2，创建一次，持续使用：
			////////////////////////////////////////////////
			var stopwatch_CreateOnce = new Stopwatch();
			stopwatch_CreateOnce.Start();
			{
				var enumerateObject = jsonElement.EnumerateObject();
				for (var testIndex = 0;
					testIndex < testsCount;
					testIndex++)
				{
					foreach (var property in enumerateObject)
					{
						if (property.Equals("doubleValue"))
						{
							break;
						}
					}
				}
			}
			stopwatch_CreateOnce.Stop();

			////////////////////////////////////////////////
			// 2/2，每次都创建使用：
			////////////////////////////////////////////////
			var stopwatch_CreateEveryTime = new Stopwatch();
			stopwatch_CreateEveryTime.Start();
			{
				for (var testIndex = 0;
					testIndex < testsCount;
					testIndex++)
				{
					var enumerateObject = jsonElement.EnumerateObject();
					foreach (var property in enumerateObject)
					{
						if (property.Equals("doubleValue"))
						{
							break;
						}
					}
				}
			}
			stopwatch_CreateEveryTime.Stop();

			System.Diagnostics.Debug.WriteLine(
				"“JsonElement.EnumerateObject”性能测试完成：");
			System.Diagnostics.Debug.WriteLine(
				$"“共测试 {testsCount} 次");
			System.Diagnostics.Debug.WriteLine(
				$"“只创建一次时，合计耗时： {stopwatch_CreateOnce.Elapsed.TotalSeconds} 秒。");
			System.Diagnostics.Debug.WriteLine(
				$"“每次都创建时，合计耗时： {stopwatch_CreateEveryTime.Elapsed.TotalSeconds} 秒。");
			var elapsedMillisecondsOfCreate
				= (stopwatch_CreateEveryTime.Elapsed.TotalMilliseconds - stopwatch_CreateOnce.Elapsed.TotalMilliseconds)
				/ (testsCount - 1);
			System.Diagnostics.Debug.WriteLine(
				$"“平均每次创建，耗时： {elapsedMillisecondsOfCreate} 毫秒。");
		}
	}
}
