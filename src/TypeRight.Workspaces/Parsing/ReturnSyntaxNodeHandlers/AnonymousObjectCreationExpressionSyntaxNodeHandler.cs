using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TypeRight.CodeModel;

namespace TypeRight.Workspaces.Parsing.ReturnSyntaxNodeHandlers
{
	/// <summary>
	/// Handles anonymous objects
	/// Example:
	///		return new { propA: 1, propB: 2 }
	/// </summary>
	internal class AnonymousObjectCreationExpressionSyntaxNodeHandler : ReturnSyntaxNodeHandler
	{
		public AnonymousObjectCreationExpressionSyntaxNodeHandler(ReturnSyntaxNodeHandlerContext parameters) : base(parameters)
		{
		}

		public override bool CanHandle(SyntaxNode node) => node is AnonymousObjectCreationExpressionSyntax;

		public override IType Handle(SyntaxNode node)
		{
			SymbolInfo symbol = Model.GetSymbolInfo(node);
			ITypeSymbol anonType = (symbol.Symbol as IMethodSymbol).ReceiverType;
			return FromSymbol(anonType);
		}
	}
}
