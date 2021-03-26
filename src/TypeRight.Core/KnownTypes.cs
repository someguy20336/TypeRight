using Newtonsoft.Json.Serialization;

namespace TypeRight
{
	public static class KnownTypes
	{
		public const string SystemTextJsonPropertyName = "System.Text.Json.Serialization.JsonPropertyNameAttribute";

		public static readonly string NewtonsoftJsonPropertyName_v12 = typeof(JsonProperty).FullName;
		public const string NewtonsoftJsonPropertyName_pre_v12 = "Newtonsoft.Json.JsonPropertyAttribute";
	}
}
