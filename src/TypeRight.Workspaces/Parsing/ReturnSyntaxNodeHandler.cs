using Microsoft.CodeAnalysis;
using TypeRight.CodeModel;
using TypeRight.Workspaces.CodeModel;

namespace TypeRight.Workspaces.Parsing
{
	internal abstract class ReturnSyntaxNodeHandler
	{
		protected ReturnSyntaxNodeHandlerContext HandlerContext { get; }

		public SemanticModel Model => HandlerContext.Model;

		public ParseContext Context => HandlerContext.Context;

		public ReturnSyntaxNodeHandler(ReturnSyntaxNodeHandlerContext handlerContext)
		{
			HandlerContext = handlerContext;
		}

		public abstract bool CanHandle(SyntaxNode node);

		public abstract IType Handle(SyntaxNode node);


		protected IType FromSyntaxNode(SyntaxNode node)
		{
			return HandlerContext.Factory.CreateForSyntaxNode(node).Handle(node);
		}

		protected IType FromMethodSymbol(IMethodSymbol methodSymb) => FromSymbol(methodSymb.ReturnType);

		protected IType FromSymbol(ITypeSymbol symbol) => RoslynType.CreateType(symbol, HandlerContext.Context);
	}

	   	

	internal class DefaultReturnSyntaxNodeHandler : ReturnSyntaxNodeHandler
	{
		public DefaultReturnSyntaxNodeHandler(ReturnSyntaxNodeHandlerContext parameters) : base(parameters)
		{
		}

		public override bool CanHandle(SyntaxNode node) => true;

		public override IType Handle(SyntaxNode node) => null;
	}
}
