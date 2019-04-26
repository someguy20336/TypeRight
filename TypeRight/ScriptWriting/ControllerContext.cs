using System;
using System.Collections.Generic;
using TypeRight.Configuration;
using TypeRight.ScriptWriting.TypeScript;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting
{
	/// <summary>
	/// Options for writing scripts
	/// </summary>
	public class ControllerContext : ScriptWriteContext
	{

		/// <summary>
		/// Gets or sets the result filepath for the server objects file
		/// </summary>
		public Uri ServerObjectsResultFilepath { get; set; }
		
		/// <summary>
		/// Gets the default web method namespace
		/// </summary>
		public string WebMethodNamespace { get; set; }
		

		/// <summary>
		/// Gets whether an ajax function was defined
		/// </summary>
		public bool HasOwnAjaxFunction => !string.IsNullOrEmpty(FetchFunctionName);

		/// <summary>
		/// Gets or sets the type of model binding to use
		/// </summary>
		public ModelBindingType ModelBinding { get; set; }

		/// <summary>
		/// Gets or sets the file path to the module containing the ajax function
		/// </summary>
		public string FetchFunctionModulePath { get; set; }

		/// <summary>
		/// Gets or sets the function name to use for ajax calls (optional)
		/// </summary>
		public string FetchFunctionName { get; set; }

		/// <summary>
		/// Gets or sets the additional action parameters
		/// </summary>
		public List<ActionParameter> AdditionalParameters { get; set; }

		/// <summary>
		/// Gets or sets any additional imports
		/// </summary>
		public List<ImportDefinition> AdditionalImports { get; set; }

		/// <summary>
		/// Gets or sets the fetch function return type
		/// </summary>
		public string FetchReturnType { get; set; }

	}
}
