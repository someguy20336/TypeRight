using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using TypeRight.TypeProcessing;

namespace TypeRight.Workspaces.Parsing
{
	/// <summary>
	/// Provides contextual information for the parse
	/// </summary>
	public class ParseContext
	{

		private List<MethodReturnTypeHandler> _returnTypeHandlers = new List<MethodReturnTypeHandler>();

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
		public ParseContext(Compilation comp, DocumentationProvider documentationProvider)
		{
			Compilation = comp;
			DocumentationProvider = documentationProvider;

			// ActionResult<T>
			_returnTypeHandlers.Add(new ActionResultReturnTypeHandler(this));

			// ActionResult, IActionResult, JsonResult, ActionResult<object>
			_returnTypeHandlers.Add(new ParseSyntaxForTypeMethodHandler(
				MvcConstants.ToAspNetFullName("JsonResult"),
				MvcConstants.ToAspNetCoreFullName("JsonResult"),
				MvcConstants.ToAspNetFullName("ActionResult"),
				MvcConstants.ToAspNetCoreFullName("ActionResult"),
				MvcConstants.ToAspNetCoreFullName("ActionResult") + "`1",
				MvcConstants.ToAspNetFullName("IActionResult"),
				MvcConstants.ToAspNetCoreFullName("IActionResult")
				));

			_returnTypeHandlers.Add(new DefaultMethodReturnTypeHandler());
		}

		/// <summary>
		/// Gets the return type handler for a given metho
		/// </summary>
		/// <param name="methodSymbol">The method symbol</param>
		/// <returns>The matching return type handler</returns>
		public MethodReturnTypeHandler GetMethodReturnTypeHandler(IMethodSymbol methodSymbol)
		{
			return _returnTypeHandlers.FirstOrDefault(h => h.CanHandleMethodSymbol(methodSymbol));
		}
		
	}
}
