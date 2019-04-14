using System.Collections.Generic;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting
{
	/// <summary>
	/// Context for writing scripts
	/// </summary>
	public class TypeWriteContext : ScriptWriteContext
	{
		/// <summary>
		/// Gets or sets the types included in this output
		/// </summary>
		public IEnumerable<ExtractedType> IncludedTypes { get; set; }
	}
}
