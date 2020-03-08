using System.Collections.Generic;
using TypeRight.TypeLocation;

namespace TypeRight.Configuration
{
	/// <summary>
	/// Represents a configuration file for a solution
	/// </summary>
	public class ConfigOptions
	{

		/// <summary>
		/// Gets or sets whether script generation is enabled
		/// </summary>
		public bool Enabled { get; set; }

		/// <summary>
		/// Gets or sets the class namespace for a namespaced template
		/// </summary>
		public string ClassNamespace { get; set; }

		/// <summary>
		/// Gets or sets the enum namespace for the namespaced template
		/// </summary>
		public string EnumNamespace { get; set; }

		/// <summary>
		/// Gets or sets the Web Method namespace for the namespaced template
		/// </summary>
		public string WebMethodNamespace { get; set; }

		/// <summary>
		/// Gets or sets the filepath of the generated result for the
		/// classes and enums
		/// </summary>
		public string ServerObjectsResultFilepath { get; set; }
		

		/// <summary>
		/// Gets or sets the template type
		/// </summary>
		public string TemplateType { get; set; }

		/// <summary>
		/// Gets the model binding style to use.  This affects how MVC actions are invoked. The options can be singleParam or multiParam
		/// "singleParam" should generally be used for ASP.NET Core applications since there is only one FromBody parameter allowed.  
		/// "multiParam" will create an object for the parameters.
		/// </summary>
		public ModelBindingType ModelBindingType { get; set; } = ModelBindingType.MultiParam;

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
			
			TemplateType = "module";
			ActionConfigurations = new List<ActionConfig>();

			ClassNamespace = "";
			EnumNamespace = "";
			WebMethodNamespace = "";
		}
	}
}
