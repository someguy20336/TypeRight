using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TypeRight.CodeModel;

namespace TypeRight.Workspaces.Parsing.ReturnSyntaxNodeHandlers
{
	/// <summary>
	/// Handles general expressions
	/// Example:
	///		return (1 + 2 + 3);
	/// </summary>
	internal class ExpressionSyntaxNodeHandler : ReturnSyntaxNodeHandler
	{
		public ExpressionSyntaxNodeHandler(ReturnSyntaxNodeHandlerContext parameters) : base(parameters)
		{
		}

		public override bool CanHandle(SyntaxNode node) => node is ExpressionSyntax;

		public override IType Handle(SyntaxNode node)
		{
			SymbolInfo info = Model.GetSymbolInfo(node);

			// If the symbol is a method, use that (i.e. return GetThing(); )
			if (info.Symbol is IMethodSymbol methodSymb)
			{
				return FromMethodSymbol(methodSymb); ;
			}

			// If the symbol is a property, use that (i.e. return object.Property; )
			if (info.Symbol is IPropertySymbol propSymb)
			{
				return FromSymbol(propSymb.Type);
			}

			// Else, i can't find it for now. 
			return null;
		}
	}
}
