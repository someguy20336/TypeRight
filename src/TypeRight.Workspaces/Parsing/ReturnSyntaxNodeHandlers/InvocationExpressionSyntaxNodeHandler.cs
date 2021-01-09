using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TypeRight.CodeModel;

namespace TypeRight.Workspaces.Parsing.ReturnSyntaxNodeHandlers
{
	/// <summary>
	/// Handles an invocation expression
	/// Example:
	///		return MyFunction();
	/// </summary>
	internal class InvocationExpressionSyntaxNodeHandler : ReturnSyntaxNodeHandler
	{

		
		public InvocationExpressionSyntaxNodeHandler(ReturnSyntaxNodeHandlerContext handlerContext) : base(handlerContext)
		{
		}

		public override bool CanHandle(SyntaxNode node) => node is InvocationExpressionSyntax;

		public override IType Handle(SyntaxNode node)
		{
			SymbolInfo symbInfo = Model.GetSymbolInfo(node);
			if (!(symbInfo.Symbol is IMethodSymbol methodSymbol))
			{
				return null;
			}

			// Otherwise, just get from the method
			return FromMethodSymbol(methodSymbol);
		}

	}
}
