﻿using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using TypeRight.CodeModel;

namespace TypeRight.Workspaces.Parsing
{
	/// <summary>
	/// Provides contextual information for the parse
	/// </summary>
	internal class ParseContext
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

			// Task<T>
			_returnTypeHandlers.Add(new TaskReturnTypeHandler(this));

			// ActionResult<T>
			_returnTypeHandlers.Add(new ActionResultReturnTypeHandler(this));

			// ActionResult, IActionResult, JsonResult, ActionResult<object>, object
			_returnTypeHandlers.Add(new ParseSyntaxForTypeMethodHandler(
				MvcConstants.ToAspNetCoreFullName("JsonResult"),
				MvcConstants.ToAspNetCoreFullName("ActionResult"),
				MvcConstants.ToAspNetCoreFullName("ActionResult") + "`1",
				MvcConstants.ToAspNetCoreFullName("IActionResult"),
				typeof(object).FullName
				));

			_returnTypeHandlers.Add(new DefaultMethodReturnTypeHandler());
		}

		/// <summary>
		/// Gets the return type handler for a given metho
		/// </summary>
		/// <param name="methodSymbol">The method symbol</param>
		/// <returns>The matching return type handler</returns>
		public IType GetMethodReturnType(IMethodSymbol methodSymbol)
		{
			return GetMethodReturnType(methodSymbol.ReturnType, methodSymbol);
		}

		/// <summary>
		/// Gets the return type handler for a given metho
		/// </summary>
		/// <param name="currentType">The current type being processed</param>
		/// <param name="methodSymbol">The method symbol</param>
		/// <returns>The matching return type handler</returns>
		public IType GetMethodReturnType(ITypeSymbol currentType, IMethodSymbol methodSymbol)
		{
			return _returnTypeHandlers.FirstOrDefault(h => h.CanHandleType(currentType, methodSymbol)).GetReturnType(this, currentType, methodSymbol);
		}
	}
}
