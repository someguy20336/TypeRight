using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace TypeRight.Workspaces.Parsing
{
	internal class ReturnSyntaxNodeHandlerContext
	{
		public SemanticModel Model { get; private set; }

		public ParseContext Context { get; private set; }

		public ReturnSyntaxNodeHandlerFactory Factory { get; private set; }

		public IMethodSymbol Method { get; private set; }

		public ReturnSyntaxNodeHandlerContext(SemanticModel model, ParseContext context, IMethodSymbol method, ReturnSyntaxNodeHandlerFactory factory)
		{
			Model = model;
			Context = context;
			Factory = factory;
			Method = method;
		}

	}
}
