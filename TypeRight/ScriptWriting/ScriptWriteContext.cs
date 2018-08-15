using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting
{
	/// <summary>
	/// Options for writing scripts
	/// </summary>
	public class ScriptWriteContext
	{
		/// <summary>
		/// Gets or sets the result filepath for the server objects file
		/// </summary>
		public string ServerObjectsResultFilepath { get; set; }

		/// <summary>
		/// Gets or sets the function name to use for ajax calls (optional)
		/// </summary>
		public string AjaxFunctionName { get; set; }
		
		/// <summary>
		/// Gets the default web method namespace
		/// </summary>
		public string WebMethodNamespace { get; set; }

		/// <summary>
		/// Gets the current collection of extracted types
		/// </summary>
		public ExtractedTypeCollection ExtractedTypes { get; set; }

	}
}
