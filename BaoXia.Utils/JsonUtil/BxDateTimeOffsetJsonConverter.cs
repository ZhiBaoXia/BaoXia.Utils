using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BaoXia.Utils.JsonUtil;

public class BxDateTimeOffsetJsonConverter : JsonConverter<DateTimeOffset>
{
	////////////////////////////////////////////////
	// @重载
	////////////////////////////////////////////////

	#region 重载

	public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		try
		{
			// 尝试正常解析 DateTime
			var dateTimeOffset = reader.GetDateTimeOffset();
			return dateTimeOffset;
		}
		catch
		{
			return DateTime.MinValue;
		}
	}

	public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(value);
	}

	#endregion
}