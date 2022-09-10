using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;

namespace TypeRight.Configuration.Json
{
	/// <summary>
	/// Converts an enum with camel casing
	/// </summary>
	internal class CamelCaseStringEnumConverter : StringEnumConverter
	{
        private readonly object _defaultValue;

		public CamelCaseStringEnumConverter() { }

        public CamelCaseStringEnumConverter(object defaultValue)
		{
			NamingStrategy = new CamelCaseNamingStrategy();
            _defaultValue = defaultValue;
        }

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null || reader.TokenType == JsonToken.Undefined)
			{
				return _defaultValue;
			}
			return base.ReadJson(reader, objectType, existingValue, serializer);
		}
	}
}
