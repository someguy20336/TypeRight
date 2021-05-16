using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TypeRight.CodeModel;

namespace TypeRight.Workspaces.Parsing.ReturnSyntaxNodeHandlers
{
	/// <summary>
	/// Ignores any method that produces an Action result under the current assumption it is "BadRequest" or something similar:
	/// Example:
	///		return BadRequest("Some reason");
	/// </summary>
	internal class IgnoreActionResultMethodNodeHandler : ReturnSyntaxNodeHandler
	{
		private INamedTypeSymbol _actionResult;

		public IgnoreActionResultMethodNodeHandler(ReturnSyntaxNodeHandlerContext handlerContext) : base(handlerContext)
		{
			string aspNetCore = MvcConstants.ActionResult_AspNetCore;

			Compilation compilation = handlerContext.Context.Compilation;
			_actionResult = compilation.GetTypeByMetadataName(aspNetCore);

		}

		public override bool CanHandle(SyntaxNode node)
		{
			if (_actionResult != null && node is InvocationExpressionSyntax)
			{
				SymbolInfo symbInfo = Model.GetSymbolInfo(node);
				if (!(symbInfo.Symbol is IMethodSymbol methodSymbol))
				{
					return false;
				}

				if (methodSymbol.ReturnType.HasBaseType(_actionResult))
				{
					return true;
				}
			}
			return false;
		}

		public override IType Handle(SyntaxNode node) => null;	// Could walk the methods in the future...
	}
}
