using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using TypeRight.TypeProcessing;

namespace TypeRight.Configuration
{
	
	/// <summary>
	/// Configuration for action methods
	/// </summary>
	public class ActionConfig
	{
		/// <summary>
		/// Gets or sets the request method this configuration applies to
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		public RequestMethod Method { get; set; }

		/// <summary>
		/// Gets or sets the name of the function to use for fetching data
		/// </summary>
		public string FetchFunctionName { get; set; }

		/// <summary>
		/// Gets or sets the filepath for the fetch function
		/// </summary>
		public string FetchFilePath { get; set; }

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

	/// <summary>
	/// An action parameter
	/// </summary>
	public class ActionParameter
	{
		public string Name { get; set; }

		public string Type { get; set; }

		public bool Optional { get; set; }
	}

	/// <summary>
	/// The import definiition
	/// </summary>
	public class ImportDefinition
	{
		public List<string> Items { get; set; } = new List<string>();

		public bool UseAlias { get; set; } = false;

		public string Path { get; set; }
	}
}
