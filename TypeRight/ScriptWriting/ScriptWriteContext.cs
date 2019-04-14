using System.Collections.Generic;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting
{
	/// <summary>
	/// Context for writing scripts
	/// </summary>
	public class ScriptWriteContext
	{
		/// <summary>
		/// Gets or sets the output path
		/// </summary>
		public string OutputPath { get; set; }

		/// <summary>
		/// Gets or sets the type collection
		/// </summary>
		public ExtractedTypeCollection TypeCollection { get; set; }

		/// <summary>
		/// Gets or sets the types included in this output
		/// </summary>
		public IEnumerable<ExtractedType> IncludedTypes { get; set; }
	}
}
