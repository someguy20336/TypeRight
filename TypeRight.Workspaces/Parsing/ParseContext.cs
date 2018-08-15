using Microsoft.CodeAnalysis;

namespace TypeRight.Workspaces.Parsing
{
	/// <summary>
	/// Provides contextual information for the parse
	/// </summary>
	public class ParseContext
	{
		/// <summary>
		/// The parse options
		/// </summary>
		private ParseOptions _options;

		/// <summary>
		/// Gets the compilation for this parse
		/// </summary>
		public Compilation Compilation { get; private set; }

		/// <summary>
		/// Gets the documentation provider
		/// </summary>
		public DocumentationProvider DocumentationProvider { get; private set; }
		
		/// <summary>
		/// Creates a new parse context
		/// </summary>
		/// <param name="comp">The compilation</param>
		/// <param name="documentationProvider">The doc provider</param>
		/// <param name="options">The parse options</param>
		public ParseContext(Compilation comp, DocumentationProvider documentationProvider, ParseOptions options)
		{
			Compilation = comp;
			DocumentationProvider = documentationProvider;
			_options = options;
		}

		/// <summary>
		/// Gets the return type handler for a given metho
		/// </summary>
		/// <param name="methodSymbol">The method symbol</param>
		/// <returns>The matching return type handler</returns>
		public MethodReturnTypeHandler GetMethodReturnTypeHandler(IMethodSymbol methodSymbol)
		{
			foreach (MethodReturnTypeHandler handler in _options.MethodReturnTypeHandlers)
			{
				if (handler.CanHandleMethodSymbol(methodSymbol))
				{
					return handler;
				}
			}

			return new DefaultMethodReturnTypeHandler();
		}
		
	}
}
