using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using TypeRight.CodeModel;

namespace TypeRight.Workspaces.Parsing.ReturnSyntaxNodeHandlers
{
	/// <summary>
	/// Handles an invocation to the common Json method
	/// Example:
	///		return Json(someData);
	/// </summary>
	internal class JsonInvocationNodeHandler : ReturnSyntaxNodeHandler
	{
		public JsonInvocationNodeHandler(ReturnSyntaxNodeHandlerContext handlerContext) : base(handlerContext)
		{
		}

		public override bool CanHandle(SyntaxNode node)
		{
			if (node is InvocationExpressionSyntax)
			{
				SymbolInfo symbInfo = Model.GetSymbolInfo(node);
				if (!(symbInfo.Symbol is IMethodSymbol methodSymbol))
				{
					return false;
				}

				// Special case
				if (methodSymbol.Name == "Json")
				{
					return true;
				}
			}
			return false;
		}

		public override IType Handle(SyntaxNode node)
		{
			InvocationExpressionSyntax invocation = node as InvocationExpressionSyntax;
			SyntaxNode firstArg = invocation.ArgumentList.Arguments[0].ChildNodes().FirstOrDefault();
			return FromSyntaxNode(firstArg);
		}
	}
}
