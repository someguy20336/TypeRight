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
		/// Gets or sets the filter to use for the display name attribute
		/// </summary>
		public TypeFilter DisplayNameFilter { get; set; }

		/// <summary>
		/// Gets or sets the filter to use for the MVC action filter
		/// </summary>
		public TypeFilter MvcActionFilter { get; set; }

		/// <summary>
		/// Gets or sets the full path to the project file
		/// </summary>
		public string ProjectPath { get; set; }

		/// <summary>
		/// Gets or sets the default result path
		/// </summary>
		public string DefaultResultPath { get; set; }

		/// <summary>
		/// Creates the processor settings object with the default settings
		/// </summary>
		public ProcessorSettings()
		{
			DisplayNameFilter = new HasInterfaceOfTypeFilter(typeof(IEnumDisplayNameProvider).FullName);
			MvcActionFilter = new IsOfTypeFilter(typeof(ScriptActionAttribute).FullName);
		}
	}
}
