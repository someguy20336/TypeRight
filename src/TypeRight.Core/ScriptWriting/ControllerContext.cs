using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting
{
	/// <summary>
	/// Options for writing scripts
	/// </summary>
	public class ControllerContext : ScriptWriteContext
	{
						
		public FetchFunctionResolver FetchFunctionResolver { get; private set; }

		public MvcControllerInfo Controller { get; private set; }


		public ControllerContext(
			MvcControllerInfo controller,
			string outputPath,
			ExtractedTypeCollection types,
			FetchFunctionResolver fetchResolver
			)
			: base(types, outputPath)
		{
			Controller = controller;
			FetchFunctionResolver = fetchResolver;
		}

	}
}
