using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BaoXia.Utils.JsonUtil;

public class BxDateTimeJsonConverter : JsonConverter<DateTime>
{
	////////////////////////////////////////////////
	// @重载
	////////////////////////////////////////////////

	#region 重载

	public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		try
		{
			// 尝试正常解析 DateTime
			return reader.GetDateTime();
		}
		catch
		{
			return default;
		}
	}

	public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(value);
	}

	#endregion
}