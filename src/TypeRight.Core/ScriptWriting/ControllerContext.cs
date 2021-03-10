using System;
using System.Collections.Specialized;
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
		public Uri ServerObjectsResultFilepath { get; private set; }
				
		public FetchFunctionResolver FetchFunctionResolver { get; private set; }

		public MvcControllerInfo Controller { get; private set; }

		public string BaseUrl { get; private set; }

		public NameValueCollection QueryParameters { get; private set; }

		public ControllerContext(
			MvcControllerInfo controller,
			string outputPath,
			ExtractedTypeCollection types,
			Uri serverObjectsPath,
			FetchFunctionResolver fetchResolver,
			string baseUrl = "",
			NameValueCollection queryParams = null
			)
			: base(types, outputPath)
		{
			Controller = controller;
			BaseUrl = baseUrl;
			ServerObjectsResultFilepath = serverObjectsPath;
			FetchFunctionResolver = fetchResolver;
			QueryParameters = queryParams ?? new NameValueCollection();
		}

	}
}
