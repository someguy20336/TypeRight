using System.Collections.Generic;

namespace TypeRight.Configuration
{
	public class FetchConfig
	{

		/// <summary>
		/// Gets or sets the name of the function to use for fetching data
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the filepath for the fetch function
		/// </summary>
		public string FilePath { get; set; }

		/// <summary>
		/// Gets or sets additional parameters to use for the fetch function
		/// </summary>
		public List<ActionParameter> Parameters { get; set; } = new List<ActionParameter>();

		/// <summary>
		/// Gets or sets the return type of the fetch function
		/// </summary>
		public string ReturnType { get; set; }

		/// <summary>
		/// Gets or sets any addtional imports required
		/// </summary>
		public List<ImportDefinition> Imports { get; set; } = new List<ImportDefinition>();
	}
		
}
