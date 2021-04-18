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

		public IEnumerable<MvcController> Controllers { get; private set; }

		public IEnumerable<MvcAction> Actions => Controllers.SelectMany(c => c.Actions);


		public ControllerContext(
			MvcController controller,
			string outputPath,
			ExtractedTypeCollection types,
			FetchFunctionResolver fetchResolver
			)
			: this(new [] { controller }, outputPath, types, fetchResolver)
		{
		}

		public ControllerContext(
			IEnumerable<MvcController> controllers,
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
