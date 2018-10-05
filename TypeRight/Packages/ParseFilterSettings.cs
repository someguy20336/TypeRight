using TypeRight.Attributes;
using TypeRight.TypeFilters;

namespace TypeRight.Packages
{
	/// <summary>
	/// Settings for the different parse filters
	/// </summary>
	public class ParseFilterSettings
	{
		/// <summary>
		/// Gets or sets the parse filter to use for class types
		/// </summary>
		public TypeFilter ClassFilter { get; set; }

		/// <summary>
		/// Gets or sets the parse filter to use for Enum types
		/// </summary>
		public TypeFilter EnumFilter { get; set; }

		/// <summary>
		/// Gets or sets the parse filter to use for controller types
		/// </summary>
		public TypeFilter ControllerFilter { get; set; }

		/// <summary>
		/// Creates the parse filter settings object with the default settings
		/// </summary>
		/// <returns>The parse filter to use for finding classes to extract </returns>
		public ParseFilterSettings()
		{
			ClassFilter = new HasAttributeFilter(typeof(ScriptObjectAttribute).FullName);
			EnumFilter = new HasAttributeFilter(typeof(ScriptEnumAttribute).FullName);
			ControllerFilter = new IsOfAnyTypeFilter("System.Web.Mvc.Controller", "Microsoft.AspNetCore.Mvc.Controller");
		}
	}
}
