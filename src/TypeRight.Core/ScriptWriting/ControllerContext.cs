using System.Collections.Generic;
using System.Linq;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting
{
	/// <summary>
	/// Options for writing scripts
	/// </summary>
	public class ControllerContext : ScriptWriteContext
	{
						
		public FetchFunctionResolver FetchFunctionResolver { get; private set; }

		public IEnumerable<MvcControllerInfo> Controllers { get; private set; }

		public IEnumerable<MvcActionInfo> Actions => Controllers.SelectMany(c => c.Actions);


		public ControllerContext(
			MvcControllerInfo controller,
			string outputPath,
			ExtractedTypeCollection types,
			FetchFunctionResolver fetchResolver
			)
			: this(new [] { controller }, outputPath, types, fetchResolver)
		{
		}

		public ControllerContext(
			IEnumerable<MvcControllerInfo> controllers,
			string outputPath,
			ExtractedTypeCollection types,
			FetchFunctionResolver fetchResolver
			)
			: base(types, outputPath)
		{
			Controllers = controllers;
			FetchFunctionResolver = fetchResolver;
		}
	}
}
