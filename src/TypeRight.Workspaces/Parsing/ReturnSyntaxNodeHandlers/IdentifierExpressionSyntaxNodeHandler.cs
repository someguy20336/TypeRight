using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TypeRight.CodeModel;

namespace TypeRight.Workspaces.Parsing.ReturnSyntaxNodeHandlers
{
	/// <summary>
	/// Handles an identifier
	/// Example:
	///		return myVariable;
	/// </summary>
	internal class IdentifierExpressionSyntaxNodeHandler : ReturnSyntaxNodeHandler
	{
		public IdentifierExpressionSyntaxNodeHandler(ReturnSyntaxNodeHandlerContext parameters) : base(parameters)
		{
		}

		public override bool CanHandle(SyntaxNode node) => node is IdentifierNameSyntax;

		public override IType Handle(SyntaxNode node)
		{
			SymbolInfo info = Model.GetSymbolInfo(node);
			if (info.Symbol is ILocalSymbol)
			{
				ILocalSymbol localVarSymb = info.Symbol as ILocalSymbol;
				return FromSymbol(localVarSymb.Type);
			}
			else if (info.Symbol is INamedTypeSymbol)
			{
				INamedTypeSymbol typeSymb = info.Symbol as INamedTypeSymbol;
				return FromSymbol(typeSymb);
			}
			else if (info.Symbol is IParameterSymbol)
			{
				IParameterSymbol typeSymb = info.Symbol as IParameterSymbol;
				return FromSymbol(typeSymb.Type);
			}
			else
			{
				return null;
			}
		}
	}
}
