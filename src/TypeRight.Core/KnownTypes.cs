using Newtonsoft.Json.Serialization;

namespace TypeRight
{
	public static class KnownTypes
	{
		public const string SystemTextJsonPropertyName = "System.Text.Json.Serialization.JsonPropertyNameAttribute";

		public static readonly string NewtonsoftJsonPropertyName = typeof(JsonProperty).FullName;
	}
}
