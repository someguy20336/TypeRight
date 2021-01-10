using System.Collections.Generic;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting
{
	/// <summary>
	/// A text template for generating type scripts
	/// </summary>
	public interface ITypeTextTemplate
	{
		/// <summary>
		/// Gets the script for the extracted types
		/// </summary>
		/// <param name="context">The context for writing the script</param>
		/// <returns>The script text</returns>
		string GetText(TypeWriteContext context);  
	}
}
