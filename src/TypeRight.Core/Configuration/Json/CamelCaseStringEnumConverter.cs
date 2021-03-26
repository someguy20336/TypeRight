using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;

namespace TypeRight.Configuration.Json
{
	/// <summary>
	/// Converts an enum with camel casing
	/// </summary>
	class CamelCaseStringEnumConverter : StringEnumConverter
	{

		public CamelCaseStringEnumConverter()
		{
			NamingStrategy = new CamelCaseNamingStrategy();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null || reader.TokenType == JsonToken.Undefined)
			{
				return existingValue;
			}
			return base.ReadJson(reader, objectType, existingValue, serializer);
		}
	}
}
