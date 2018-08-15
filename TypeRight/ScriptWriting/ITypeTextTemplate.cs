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
		/// <param name="extractedTypeCollection">The collection of extracted types</param>
		/// <returns>The script text</returns>
		string GetText(ExtractedTypeCollection extractedTypeCollection);  
	}
}
