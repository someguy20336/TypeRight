using TypeRight.Attributes;
using TypeRight.TypeFilters;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// Settings to use while processing found types
	/// </summary>
	public class ProcessorSettings
	{
		/// <summary>
		/// Gets the namespace used by the reference types
		/// </summary>
		public string TypeNamespace { get; set; }

		/// <summary>
		/// Gest the namespace used by enums
		/// </summary>
		public string EnumNamespace { get; set; }

		/// <summary>
		/// Gets or sets the filter to use for the display name attribute
		/// </summary>
		public TypeFilter DisplayNameFilter { get; set; }

		/// <summary>
		/// Gets or sets the filter to use for the MVC action filter
		/// </summary>
		public TypeFilter MvcActionFilter { get; set; }

		/// <summary>
		/// Creates the processor settings object with the default settings
		/// </summary>
		public ProcessorSettings()
		{
			TypeNamespace = "DefaultClass";
			EnumNamespace = "DefaultEnums";
			DisplayNameFilter = new HasInterfaceOfTypeFilter(typeof(IEnumDisplayNameProvider).FullName);
			MvcActionFilter = new IsOfTypeFilter(typeof(ScriptActionAttribute).FullName);
		}
	}
}
