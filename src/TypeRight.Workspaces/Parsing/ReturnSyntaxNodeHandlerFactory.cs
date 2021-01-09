using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypeRight.Workspaces.Parsing.ReturnSyntaxNodeHandlers;

namespace TypeRight.Workspaces.Parsing
{
	internal class ReturnSyntaxNodeHandlerFactory
	{
		private IEnumerable<ReturnSyntaxNodeHandler> _handlers;
		public ReturnSyntaxNodeHandlerContext Context { get; private set; }

		public ReturnSyntaxNodeHandlerFactory(SemanticModel model, ParseContext context, IMethodSymbol method)
		{
			Context = new ReturnSyntaxNodeHandlerContext(model, context, method, this);
			Initialize();
		}

		public ReturnSyntaxNodeHandler CreateForSyntaxNode(SyntaxNode node)
		{
			return _handlers.First(h => h.CanHandle(node));
		}

		private void Initialize()
		{
			// NOTE: this order matters!
			_handlers = new List<ReturnSyntaxNodeHandler>()
			{
				new JsonInvocationNodeHandler(Context),
				new IgnoreActionResultMethodNodeHandler(Context),
				new InvocationExpressionSyntaxNodeHandler(Context),
				new LiteralExpressionSyntaxNodeHandler(Context),
				new IdentifierExpressionSyntaxNodeHandler(Context),
				new NewActionResultNodeHandler(Context),
				new ObjectCreationExpressionSyntaxNodeHandler(Context),
				new AnonymousObjectCreationExpressionSyntaxNodeHandler(Context),
				new ExpressionSyntaxNodeHandler(Context),
				new DefaultReturnSyntaxNodeHandler(Context)
			};
		}
	}
}
