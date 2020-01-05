using Microsoft.CodeAnalysis;
using System.Collections.Generic;
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
		/// <param name="options">The parse options</param>
		public ParseContext(Compilation comp, DocumentationProvider documentationProvider)
		{
			Compilation = comp;
			DocumentationProvider = documentationProvider;

			_returnTypeHandlers.Add(new ParseSyntaxForTypeMethodHandler(
				MvcConstants.ToAspNetFullName("JsonResult"),
				new InvocationReturnForwardFilter("Json", 0)
				));
			_returnTypeHandlers.Add(new ParseSyntaxForTypeMethodHandler(
				MvcConstants.ToAspNetFullName("ActionResult"),
				new InvocationReturnForwardFilter("Json", 0)
				));
			_returnTypeHandlers.Add(new ParseSyntaxForTypeMethodHandler(
				MvcConstants.ToAspNetCoreFullName("JsonResult"),
				new InvocationReturnForwardFilter("Json", 0)
				));
			_returnTypeHandlers.Add(new ParseSyntaxForTypeMethodHandler(
				MvcConstants.ToAspNetCoreFullName("ActionResult"),
				new InvocationReturnForwardFilter("Json", 0)
				));
			_returnTypeHandlers.Add(new ParseSyntaxForTypeMethodHandler(
				MvcConstants.ToAspNetCoreFullName("IActionResult"),
				new InvocationReturnForwardFilter("Json", 0)
				));
			_returnTypeHandlers.Add(new ParseSyntaxForTypeMethodHandler(
				MvcConstants.ToAspNetCoreFullName("IActionResult"),
				new InvocationReturnForwardFilter("Json", 0)
				));
			_returnTypeHandlers.Add(new ActionResultReturnTypeHandler(this));
		}

		/// <summary>
		/// Gets the return type handler for a given metho
		/// </summary>
		/// <param name="methodSymbol">The method symbol</param>
		/// <returns>The matching return type handler</returns>
		public MethodReturnTypeHandler GetMethodReturnTypeHandler(IMethodSymbol methodSymbol)
		{
			foreach (MethodReturnTypeHandler handler in _returnTypeHandlers)
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
