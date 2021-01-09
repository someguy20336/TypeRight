using TypeRight.CodeModel;
using Microsoft.CodeAnalysis;

namespace TypeRight.Workspaces
{
	/// <summary>
	/// A class that can provide documentation about a symbol
	/// </summary>
	public abstract class DocumentationProvider
	{
		/// <summary>
		/// Gets documentation about a symbol
		/// </summary>
		/// <param name="sym">The symbol to get documentation for</param>
		/// <returns>The XML Documentation</returns>
		internal abstract XmlDocumentation GetDocumentationForSymbol(ISymbol sym);
	}
}
