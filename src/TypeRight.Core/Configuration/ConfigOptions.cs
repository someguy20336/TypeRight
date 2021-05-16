using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Specialized;
using TypeRight.Configuration.Json;
using TypeRight.ScriptWriting;

namespace TypeRight.Configuration
{
	/// <summary>
	/// Represents a configuration file for a solution
	/// </summary>
	[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
	public class ConfigOptions
	{
		[JsonProperty("$schema")]
		public string Schema { get; set; }
			= "https://raw.githubusercontent.com/someguy20336/TypeRight/tool/v1.4.0/src/TypeRight.Core/Configuration/typeRightConfig-schema.json";

		/// <summary>
		/// Gets or sets whether script generation is enabled
		/// </summary>
		public bool Enabled { get; set; }

		public string BaseUrl { get; set; }

		[JsonConverter(typeof(CamelCaseStringEnumConverter))]
		public NamingStrategyType NameCasingConverter { get; set; }

		[JsonConverter(typeof(QueryParamJsonConverter))]
		public NameValueCollection QueryParams { get; set; }

		/// <summary>
		/// Gets or sets the filepath of the generated result for the
		/// classes and enums
		/// </summary>
		public string ServerObjectsResultFilepath { get; set; }

		public FetchConfig FetchConfig { get; set; }

		/// <summary>
		/// Creates a new config file
		/// </summary>
		public ConfigOptions()
		{
			Enabled = true;
			ServerObjectsResultFilepath = "./Scripts/ServerObjects.ts";

			BaseUrl = "";
		}
	}
}
