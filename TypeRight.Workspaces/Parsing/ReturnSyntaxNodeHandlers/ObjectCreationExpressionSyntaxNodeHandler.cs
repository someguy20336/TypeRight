using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TypeRight.CodeModel;

namespace TypeRight.Workspaces.Parsing.ReturnSyntaxNodeHandlers
{
	/// <summary>
	/// Handles the new object syntax
	/// Example:
	///		return new MyResult();
	/// </summary>
	internal class ObjectCreationExpressionSyntaxNodeHandler : ReturnSyntaxNodeHandler
	{
		public ObjectCreationExpressionSyntaxNodeHandler(ReturnSyntaxNodeHandlerContext parameters) : base(parameters)
		{
		}

		public override bool CanHandle(SyntaxNode node) => node is ObjectCreationExpressionSyntax;

		public override IType Handle(SyntaxNode node)
		{
			var objSyntax = node as ObjectCreationExpressionSyntax;
			return FromSyntaxNode(objSyntax.Type);
		}
	}
}
