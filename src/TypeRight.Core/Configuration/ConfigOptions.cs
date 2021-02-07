using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using TypeRight.Configuration.Json;

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
			= "https://raw.githubusercontent.com/someguy20336/TypeRight/v0.12/TypeRight.Core/Configuration/typeRightConfig-schema.json";

		/// <summary>
		/// Gets or sets whether script generation is enabled
		/// </summary>
		public bool Enabled { get; set; }
		
		/// <summary>
		/// Gets or sets the filepath of the generated result for the
		/// classes and enums
		/// </summary>
		public string ServerObjectsResultFilepath { get; set; }
		
		/// <summary>
		/// Gets or sets the action configurations to use
		/// </summary>
		public List<ActionConfig> ActionConfigurations { get; set; }

		/// <summary>
		/// Creates a new config file
		/// </summary>
		public ConfigOptions()
		{
			Enabled = true;
			ServerObjectsResultFilepath = "./Scripts/ServerObjects.ts";
			
			ActionConfigurations = new List<ActionConfig>();
		}
	}
}
