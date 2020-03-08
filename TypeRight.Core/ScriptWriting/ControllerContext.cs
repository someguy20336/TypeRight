using System;
using System.Collections.Generic;
using TypeRight.Configuration;
using TypeRight.TypeFilters;

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
		
		public FetchFunctionResolver FetchFunctionResolver { get; set; }

		public ModelBindingType ModelBinding { get; set; }

	}
}
