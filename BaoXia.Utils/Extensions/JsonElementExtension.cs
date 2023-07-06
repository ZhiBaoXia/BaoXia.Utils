using System;
using System.Linq;
using System.Text.Json;
using static System.Text.Json.JsonElement;

namespace BaoXia.Utils.Extensions
{
	public static class JsonElementExtension
	{
		////////////////////////////////////////////////
		// @获取当前元素值
		////////////////////////////////////////////////

		#region 获取当前元素值

		public static int GetPropertiesCount(this JsonElement jsonElement)
		{
			var propertiesCount = jsonElement.EnumerateObject().Count();
			{ }
			return propertiesCount;
		}

		public static ObjectEnumerator GetProperties(this JsonElement jsonElement)
		{
			var properties = jsonElement.EnumerateObject();
			{ }
			return properties;
		}

		public static bool TryToObject(
			this JsonElement jsonElement,
			Type? objectType,
			out object? objectValue,
			JsonSerializerOptions? options = null)
		{
			objectValue = null;

			if (objectType == null)
			{
				return false;
			}

			try
			{
				objectValue = jsonElement.Deserialize(
					objectType,
					options);
				return true;
			}
			catch
			{ }
			return false;
		}

		public static bool TryToObject<ObjectType>(
			this JsonElement jsonElement,
			out ObjectType? objectValue,
			JsonSerializerOptions? options = null)
		{
			objectValue = default;
			try
			{
				objectValue = (ObjectType?)jsonElement.Deserialize(
					typeof(ObjectType),
					options);
				return true;
			}
			catch
			{ }
			return false;
		}

		public static object? ToObject(
			this JsonElement jsonElement,
			Type? objectType,
			JsonSerializerOptions? options = null)
		{
			if (TryToObject(
				jsonElement,
				objectType,
				out var objectValue,
				options))
			{
				return objectValue;
			}
			return null;
		}

		public static ObjectType? ToObject<ObjectType>(
			this JsonElement jsonElement,
			JsonSerializerOptions? options = null)
		{
			if (TryToObject<ObjectType>(
				jsonElement,
				out var objectValue,
				options))
			{
				return objectValue;
			}
			return default;
		}

		#endregion

		////////////////////////////////////////////////
		// @获取属性
		////////////////////////////////////////////////

		#region 获取属性

		public static JsonElement? TryGetProperty(
			this JsonElement jsonElement,
			string? propertyName,
			bool isIgnoreCase = true)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				return null;
			}
			if (isIgnoreCase == false)
			{
				if (!jsonElement.TryGetProperty(
					propertyName,
					out var propertyObject))
				{
					return null;
				}
				return propertyObject;
			}

			if (jsonElement.ValueKind != JsonValueKind.Object)
			{
				return null;
			}
			var jsonElementEnumerateObject = jsonElement.EnumerateObject();
			foreach (var jsonElementProperty in jsonElementEnumerateObject)
			{
				if (jsonElementProperty.Name.EqualsIgnoreCase(propertyName))
				{
					return jsonElementProperty.Value;
				}
			}
			return null;
		}

		public static byte TryGetByteProperty(
			this JsonElement jsonElement,
			string? propertyName,
			byte defaultObject = 0,
			bool isIgnoreCase = true)
		{
			var propertyObject
				= TryGetProperty(
				jsonElement,
				propertyName,
				isIgnoreCase);
			if (propertyObject == null)
			{
				return defaultObject;
			}

			if (!propertyObject.Value.TryGetByte(out var intObject))
			{
				return defaultObject;
			}
			return intObject;
		}

		public static bool TryGetBoolProperty(
			this JsonElement jsonElement,
			string? propertyName,
			bool defaultObject = false,
			bool isIgnoreCase = true)
		{
			var propertyObject
				= TryGetProperty(
				jsonElement,
				propertyName,
				isIgnoreCase);
			if (propertyObject == null)
			{
				return defaultObject;
			}

			if (propertyObject.Value.ValueKind == JsonValueKind.True)
			{
				return true;
			}
			else if (propertyObject.Value.ValueKind == JsonValueKind.False)
			{
				return false;
			}
			return defaultObject;
		}

		public static int TryGetIntProperty(
			this JsonElement jsonElement,
			string? propertyName,
			int defaultObject = 0,
			bool isIgnoreCase = true)
		{
			var propertyObject
				= TryGetProperty(
				jsonElement,
				propertyName,
				isIgnoreCase);
			if (propertyObject == null
				|| propertyObject.Value.ValueKind != JsonValueKind.Number)
			{
				return defaultObject;
			}

			if (!propertyObject.Value.TryGetInt32(out var intObject))
			{
				return defaultObject;
			}
			return intObject;
		}

		public static long TryGetLongProperty(
			this JsonElement jsonElement,
			string? propertyName,
			long defaultObject = 0,
			bool isIgnoreCase = true)
		{
			var propertyObject
				= TryGetProperty(
				jsonElement,
				propertyName,
				isIgnoreCase);
			if (propertyObject == null
				|| propertyObject.Value.ValueKind != JsonValueKind.Number)
			{
				return defaultObject;
			}

			if (!propertyObject.Value.TryGetInt64(out var longObject))
			{
				return defaultObject;
			}
			return longObject;
		}

		public static float TryGetFloatProperty(
			this JsonElement jsonElement,
			string? propertyName,
			float defaultObject = 0.0F,
			bool isIgnoreCase = true)
		{
			var propertyObject
				= TryGetProperty(
				jsonElement,
				propertyName,
				isIgnoreCase);
			if (propertyObject == null
				|| propertyObject.Value.ValueKind != JsonValueKind.Number)
			{
				return defaultObject;
			}

			if (!propertyObject.Value.TryGetSingle(out var floatObject))
			{
				return defaultObject;
			}
			return floatObject;
		}

		public static double TryGetDoubleProperty(
			this JsonElement jsonElement,
			string? propertyName,
			double defaultObject = 0.0,
			bool isIgnoreCase = true)
		{
			var propertyObject
				= TryGetProperty(
				jsonElement,
				propertyName,
				isIgnoreCase);
			if (propertyObject == null
				|| propertyObject.Value.ValueKind != JsonValueKind.Number)
			{
				return defaultObject;
			}

			if (!propertyObject.Value.TryGetDouble(out var doubleObject))
			{
				return defaultObject;
			}
			return doubleObject;
		}

		public static decimal TryGetDecimalProperty(
			this JsonElement jsonElement,
			string? propertyName,
			decimal defaultObject = 0,
			bool isIgnoreCase = true)
		{
			var propertyObject
				= TryGetProperty(
				jsonElement,
				propertyName,
				isIgnoreCase);
			if (propertyObject == null
				|| propertyObject.Value.ValueKind != JsonValueKind.Number)
			{
				return defaultObject;
			}

			if (!propertyObject.Value.TryGetDecimal(out var doubleObject))
			{
				return defaultObject;
			}
			return doubleObject;
		}

		public static string? TryGetStringProperty(
			this JsonElement jsonElement,
			string? propertyName,
			string? defaultObject = null,
			bool isIgnoreCase = true)
		{
			var propertyObject
				= TryGetProperty(
				jsonElement,
				propertyName,
				isIgnoreCase);
			if (propertyObject == null
				|| propertyObject.Value.ValueKind != JsonValueKind.String)
			{
				return defaultObject;
			}

			var stringValue = propertyObject.Value.GetString();
			{ }
			return stringValue;
		}

		public static ObjectType? TryGetObjectProperty<ObjectType>(
			this JsonElement jsonElement,
			string? propertyName,
			ObjectType? defaultObject = default,
			bool isIgnoreCase = true,
			JsonSerializerOptions? options = null)
		{
			var propertyObject
				= TryGetProperty(
				jsonElement,
				propertyName,
				isIgnoreCase);
			if (propertyObject == null
				|| propertyObject.Value.ValueKind != JsonValueKind.Object)
			{
				return defaultObject;
			}

			var @object = propertyObject.Value.Deserialize<ObjectType>(
				options ?? Environment.JsonSerializerOptions);
			{ }
			return @object;
		}

		#endregion
	}
}
