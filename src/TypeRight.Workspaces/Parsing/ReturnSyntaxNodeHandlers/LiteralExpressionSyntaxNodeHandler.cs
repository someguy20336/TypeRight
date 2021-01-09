using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TypeRight.CodeModel;

namespace TypeRight.Workspaces.Parsing.ReturnSyntaxNodeHandlers
{

	/// <summary>
	/// Handles a literal expression
	/// Example:
	///		return true;
	///		return "Hello!";
	/// </summary>
	internal class LiteralExpressionSyntaxNodeHandler : ReturnSyntaxNodeHandler
	{
		public LiteralExpressionSyntaxNodeHandler(ReturnSyntaxNodeHandlerContext parameters) : base(parameters)
		{
		}

		public override bool CanHandle(SyntaxNode node) => node is LiteralExpressionSyntax literal;

		public override IType Handle(SyntaxNode node)
		{

			SyntaxKind kind = node.Kind();

			INamedTypeSymbol symb;
			switch (kind)
			{
				case SyntaxKind.TrueLiteralExpression:
				case SyntaxKind.FalseLiteralExpression:
					symb = Context.Compilation.GetTypeByMetadataName(typeof(bool).FullName);
					break;
				case SyntaxKind.StringLiteralExpression:
					symb = Context.Compilation.GetTypeByMetadataName(typeof(string).FullName);
					break;
				case SyntaxKind.NumericLiteralExpression:
					symb = Context.Compilation.GetTypeByMetadataName(typeof(int).FullName);
					break;
				default:
					return null;
			}

			return FromSymbol(symb);
		}
	}
}
