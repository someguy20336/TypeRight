using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace TypeRight.Configuration.Json
{
	public class QueryParamJsonConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return true;
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return new NameValueCollection();
			}

			if (reader.Value is string strVal)
			{
				return System.Web.HttpUtility.ParseQueryString(strVal);
			}

			NameValueCollection queryParams = new NameValueCollection();
			while (reader.Read())
			{
				if (reader.TokenType == JsonToken.EndObject)
				{
					break;
				}

				string propName = reader.Value as string;

				reader.Read();
				string val;

				switch (reader.TokenType)
				{
					case JsonToken.Integer:
					case JsonToken.Float:
					case JsonToken.String:
						val = reader.Value.ToString();
						break;
					case JsonToken.Boolean:
						val = reader.Value.ToString().ToLower();
						break;
					case JsonToken.Null:
					case JsonToken.Undefined:
						val = "";
						break;
					default:
						throw new NotSupportedException("Encountered invalid configuration.  Query Parameters objects must be a name/value object.");
				}
				queryParams.Add(propName, val);
			}

			return queryParams;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var col = value as NameValueCollection;
			if (col == null || col.Count == 0)
			{
				writer.WriteNull();
			}

			Dictionary<string, string> queryParams = new Dictionary<string, string>();
			foreach (string key in col.Keys)
			{
				queryParams.Add(key, col.Get(key));
			}

			serializer.Serialize(writer, queryParams);
		}
	}
}
