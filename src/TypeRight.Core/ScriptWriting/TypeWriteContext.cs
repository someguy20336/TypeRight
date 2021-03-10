using System;
using System.Collections.Generic;
using System.Linq;
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
		public IEnumerable<ExtractedType> IncludedTypes { get; private set; }

		public TypeWriteContext(
			IEnumerable<ExtractedType> types,
			ExtractedTypeCollection extractedTypes, 
			string outputPath) 
			: base(extractedTypes, outputPath)
		{
			IncludedTypes = types?.ToList() ?? (IEnumerable<ExtractedType>)Array.Empty<ExtractedType>();
		}

	}
}
