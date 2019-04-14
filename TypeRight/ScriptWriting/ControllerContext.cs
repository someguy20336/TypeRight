using System;
using TypeRight.Configuration;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting
{
	/// <summary>
	/// Options for writing scripts
	/// </summary>
	public class ControllerContext
	{
		/// <summary>
		/// Gets or sets the output path for the controller
		/// </summary>
		public string OutputPath { get; set; }

		/// <summary>
		/// Gets or sets the result filepath for the server objects file
		/// </summary>
		public Uri ServerObjectsResultFilepath { get; set; }

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

		/// <summary>
		/// Gets or sets the file path to the module containing the ajax function
		/// </summary>
		public string AjaxFunctionModulePath { get; set; }

		/// <summary>
		/// Gets whether an ajax function was defined
		/// </summary>
		public bool HasOwnAjaxFunction => !string.IsNullOrEmpty(AjaxFunctionName);

		/// <summary>
		/// Gets or sets the type of model binding to use
		/// </summary>
		public ModelBindingType ModelBinding { get; set; }

	}
}
