using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace TypeRight.Configuration.Json
{
	public class ActionParameterJsonConverter : JsonConverter
	{
		private	CamelCaseNamingStrategy _camelCase = new CamelCaseNamingStrategy();
		public override bool CanConvert(Type objectType) => true;

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{

			if (reader.Value is string strVal)
			{
				return ActionParameter.FromName(strVal);
			}

			var props = serializer.Deserialize(reader, typeof(Dictionary<string, string>)) as Dictionary<string, string>;

			string name = GetValueOrDefault(props, nameof(ActionParameter.Name), "NO_NAME");
			string type = GetValueOrDefault(props, nameof(ActionParameter.Type), "NO_TYPE");
			bool optional = GetValueOrDefault(props, nameof(ActionParameter.Optional), "") == "true";

			ActionParameter parameter = new ActionParameter(name, type, optional);
			return parameter;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			ActionParameter parameter = value as ActionParameter;
			if (parameter == null)
			{
				writer.WriteNull();
				return;
			}

			if (parameter.Kind != ParameterKind.Custom)
			{
				writer.WriteValue(_camelCase.GetPropertyName(parameter.Kind.ToString(), false));
				return;
			}

			writer.WriteStartObject();

			writer.WritePropertyName(_camelCase.GetPropertyName(nameof(ActionParameter.Name), false));
			writer.WriteValue(parameter.Name);

			writer.WritePropertyName(_camelCase.GetPropertyName(nameof(ActionParameter.Type), false));
			writer.WriteValue(parameter.Type);

			writer.WritePropertyName(_camelCase.GetPropertyName(nameof(ActionParameter.Optional), false));
			writer.WriteValue(parameter.Optional);

			writer.WriteEndObject();

		}

		private string GetValueOrDefault(Dictionary<string, string> props, string key, string def)
		{
			key = _camelCase.GetPropertyName(key, false);
			if (props.ContainsKey(key))
			{
				return props[key];
			}
			return def;
		}
	}
}
