using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using TypeRight.Workspaces.CodeModel;
using TypeRight.CodeModel;

namespace TypeRight.Workspaces.Parsing
{
	/// <summary>
	/// Walks an MVC action to extract information about the method
	/// </summary>
	class MvcActionMethodWalker : CSharpSyntaxWalker
	{
		/// <summary>
		/// The current semantic model
		/// </summary>
		private SemanticModel _model;
		
		/// <summary>
		/// the parse context
		/// </summary>
		private ParseContext _context;

		private InvocationReturnForwardFilter _returnTypeForwarder;

		/// <summary>
		/// The return type to use
		/// </summary>
		IType _retType = null;

		/// <summary>
		/// Creates a new MVC action method walker
		/// </summary>
		/// <param name="context">The parse context</param>
		/// <param name="forwarder">The invocation return type forwarder</param>
		public MvcActionMethodWalker(ParseContext context, InvocationReturnForwardFilter forwarder)
		{
			_context = context;
			_returnTypeForwarder = forwarder;
		}

		/// <summary>
		/// Gets the return type for the given method
		/// </summary>
		/// <param name="method">The method symbol</param>
		/// <returns>The calculated return type, or null if one is not found</returns>
		public IType GetReturnType(IMethodSymbol method)
		{
			_model = _context.Compilation.GetSemanticModel(method.DeclaringSyntaxReferences[0].SyntaxTree);

			// TODO: this only gets the first syntax, which is usually valid, but could be wrong for partial methods
			SyntaxNode node = method.DeclaringSyntaxReferences.FirstOrDefault().GetSyntax();
			Visit(node);

			if (_retType == null)
			{
				_retType = RoslynType.CreateType(method.ReturnType, _context);
			}

			return _retType;
		}

		/// <summary>
		/// Visits a return statement
		/// </summary>
		/// <param name="node">The return statement syntax</param>
		public override void VisitReturnStatement(ReturnStatementSyntax node)
		{
			foreach (SyntaxNode subNode in node.ChildNodes())
			{
				_retType = FromSyntaxNode(subNode);
				// TODO: This will return the first return type, which could
				// be different than other return types.  I will not care yet
				break;  
			}
		}

		/// <summary>
		/// Gets the type from the given syntax node
		/// </summary>
		/// <param name="node">The syntax node</param>
		/// <returns>The calculated return type for the method</returns>
		private IType FromSyntaxNode(SyntaxNode node)
		{
			// This if tree is intentionally ordered in a priority list
			// Be careful when editing it
			if (node is InvocationExpressionSyntax invSyntax)
			{
				// Example: return Json(x)
				//			return Function()
				return FromInvocationExpression(invSyntax);
			}
			else if (node is LiteralExpressionSyntax literal)
			{
				// Example: return true;
				//			return "Hello World"
				return FromLiteralExpression(literal);
			}
			else if (node is IdentifierNameSyntax identifier)
			{
				// Example:	return myVariable;
				return FromIdentifier(identifier);
			}
			else if (node is ObjectCreationExpressionSyntax objSyntax)
			{
				// Example: return new Object();
				return FromSyntaxNode(objSyntax.Type);
			}
			else if (node is AnonymousObjectCreationExpressionSyntax anonSyntax)
			{
				SymbolInfo symbol = _model.GetSymbolInfo(anonSyntax);
				ITypeSymbol anonType = (symbol.Symbol as IMethodSymbol).ReceiverType;
				return FromSymbol(anonType);
			}
			else if (node is ExpressionSyntax exprSyntax)
			{
				// Example: return (1 + 2 + 3)
				return FromExpression(exprSyntax);
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Gets the type from an invocation expression.  This function
		/// as a special case of the Json method
		/// </summary>
		/// <param name="node">The expression node</param>
		/// <returns>The return type</returns>
		private IType FromInvocationExpression(InvocationExpressionSyntax node)
		{
			SymbolInfo symbInfo = _model.GetSymbolInfo(node);
			IMethodSymbol methodSymbol = symbInfo.Symbol as IMethodSymbol;
			if (methodSymbol == null)
			{
				return null;
			}

			// Special case
			if (methodSymbol.Name == _returnTypeForwarder.AppliesToMethodWithName)
			{
				SyntaxNode firstArg = node.ArgumentList.Arguments[_returnTypeForwarder.UseArgumentIndex].ChildNodes().FirstOrDefault();
				return FromSyntaxNode(firstArg);
			}

			// Otherwise, just get from the method
			return FromMethodSymbol(methodSymbol);
		}

		/// <summary>
		/// Gets the type from the liternal expression node
		/// </summary>
		/// <param name="node">the node</param>
		/// <returns>The type from the literal expression</returns>
		private IType FromLiteralExpression(LiteralExpressionSyntax node)
		{
			SyntaxKind kind = node.Kind();

			INamedTypeSymbol symb;
			switch (kind)
			{
				case SyntaxKind.TrueLiteralExpression:
				case SyntaxKind.FalseLiteralExpression:
					symb = _context.Compilation.GetTypeByMetadataName(typeof(bool).FullName);
					break;
				case SyntaxKind.StringLiteralExpression:
					symb = _context.Compilation.GetTypeByMetadataName(typeof(string).FullName);
					break;
				case SyntaxKind.NumericLiteralExpression:
					symb = _context.Compilation.GetTypeByMetadataName(typeof(int).FullName);
					break;
				default:
					return null;
			}

			return FromSymbol(symb);
		}

		/// <summary>
		/// Rerturn the type from a generic expression syntax
		/// </summary>
		/// <param name="node">The expression syntax node</param>
		/// <returns>The type</returns>
		private IType FromExpression(ExpressionSyntax node)
		{
			
			SymbolInfo info = _model.GetSymbolInfo(node);

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

		/// <summary>
		/// Return the type from an identifier (type or variable)
		/// </summary>
		/// <param name="node">The node</param>
		/// <returns>The found type</returns>
		private IType FromIdentifier(IdentifierNameSyntax node)
		{
			SymbolInfo info = _model.GetSymbolInfo(node);
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

		/// <summary>
		/// Creates a type from a method symbol
		/// </summary>
		/// <param name="methodSymb">The method symbol</param>
		/// <returns>The <see cref="IType"/> of the method</returns>
		private IType FromMethodSymbol(IMethodSymbol methodSymb) => FromSymbol(methodSymb.ReturnType);

		private IType FromSymbol(ITypeSymbol symbol) => RoslynType.CreateType(symbol, _context);
	}
}
