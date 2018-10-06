using TypeRight.Packages;

namespace TypeRight.Configuration
{
	/// <summary>
	/// Represents a configuration file for a solution
	/// </summary>
	public class ConfigOptions : IPackageOptions, IConfigOptions
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
		/// Gets or sets the ajax function to use for web method calls.  If null or empty, one will
		/// be created for you for each actions file.
		/// 
		/// The parameters should be (url, data, success?, fail?)
		/// </summary>
		public string AjaxFunctionName { get; set; }

		/// <summary>
		/// Gets or sets the name of the attribute that identifies actions.
		/// If not specified, it will default to the implementation specific attribute
		/// </summary>
		public string MvcActionAttributeName { get; set; } = "";

		/// <summary>
		/// Gets or sets the template type
		/// </summary>
		public string TemplateType { get; set; }

        /// <summary>
        /// Gets the relative path to the ajax function module (for module template)
        /// </summary>
        public string AjaxFunctionModulePath { get; set; } = "";

        /// <summary>
        /// Creates a new config file
        /// </summary>
        public ConfigOptions()
		{
			Enabled = true;
			ClassNamespace = "ServerClasses";
			EnumNamespace = "ServerEnums";
			WebMethodNamespace = "WebMethods";
			ServerObjectsResultFilepath = "./Scripts/ServerObjects.ts";
			AjaxFunctionName = null;
			TemplateType = "module";
		}
	}
}
