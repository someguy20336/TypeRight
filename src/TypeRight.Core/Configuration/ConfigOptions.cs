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
		/// Gets the model binding style to use.  This affects how MVC actions are invoked. The options can be singleParam or multiParam
		/// "singleParam" should generally be used for ASP.NET Core applications since there is only one FromBody parameter allowed.  
		/// "multiParam" will create an object for the parameters.
		/// </summary>
		[JsonConverter(typeof(CamelCaseStringEnumConverter))]
		public ModelBindingType ModelBindingType { get; set; } = ModelBindingType.SingleParam;

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
