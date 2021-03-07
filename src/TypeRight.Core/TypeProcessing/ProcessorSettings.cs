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
		/// Gets or sets the full path to the project file
		/// </summary>
		public string ProjectPath { get; set; }

		/// <summary>
		/// Gets or sets the default result path
		/// </summary>
		public string DefaultResultPath { get; set; }

	}
}
