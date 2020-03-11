using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using TypeRight.CodeModel;

namespace TypeRight.Workspaces.Parsing.ReturnSyntaxNodeHandlers
{
	/// <summary>
	/// Handles a node where we are creating a new action result
	/// Example:
	///		return new ActionResult("some value");
	///		return new ActionResult&lt;object&gt;(new { prop: 1 })
	/// </summary>
	internal class NewActionResultNodeHandler : ReturnSyntaxNodeHandler
	{
		public NewActionResultNodeHandler(ReturnSyntaxNodeHandlerContext handlerContext) : base(handlerContext)
		{
		}

		public override bool CanHandle(SyntaxNode node)
		{
			if (node is ObjectCreationExpressionSyntax objCreate)
			{
				var symb = Model.GetSymbolInfo(objCreate.Type).Symbol as INamedTypeSymbol;

				string name = symb.GetNormalizedMetadataName();

				if (name == (MvcConstants.ActionResult_AspNetCore + "`1")
					&& symb.TypeArguments[0].SpecialType == SpecialType.System_Object)
				{
					return true;
				}

			}

			return false;
		}

		public override IType Handle(SyntaxNode node)
		{
			ObjectCreationExpressionSyntax objCreate = node as ObjectCreationExpressionSyntax;
			return FromSyntaxNode(objCreate.ArgumentList.Arguments[0].ChildNodes().First());
		}
	}
}
