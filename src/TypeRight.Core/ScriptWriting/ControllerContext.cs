using System;
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
				
		public FetchFunctionResolver FetchFunctionResolver { get; set; }

		public MvcControllerInfo Controller { get; set; }

		public string BaseUrl { get; set; }

	}
}
