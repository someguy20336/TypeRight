using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using TypeRight.Workspaces.CodeModel;
using TypeRight.CodeModel;
using System.Collections.Generic;

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

		private ReturnSyntaxNodeHandlerFactory _handlerFactory;
		
		private List<IType> _candidates = new List<IType>();

		/// <summary>
		/// Creates a new MVC action method walker
		/// </summary>
		/// <param name="context">The parse context</param>
		/// <param name="forwarder">The invocation return type forwarder</param>
		public MvcActionMethodWalker(ParseContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Gets the return type for the given method
		/// </summary>
		/// <param name="method">The method symbol</param>
		/// <returns>The calculated return type, or null if one is not found</returns>
		public IType GetReturnType(IMethodSymbol method)
		{
			_model = _context.Compilation.GetSemanticModel(method.DeclaringSyntaxReferences[0].SyntaxTree);
			_handlerFactory = new ReturnSyntaxNodeHandlerFactory(_model, _context, method);

			// This only gets the first syntax, which is usually valid, but could be wrong for partial methods
			SyntaxNode node = method.DeclaringSyntaxReferences.FirstOrDefault().GetSyntax();
			Visit(node);

			IType foundType = _candidates.LastOrDefault();	// Maybe select best type? OR merge types possibly.  Probably a rare use case.

			if (foundType == null)
			{
				foundType = RoslynType.CreateType(method.ReturnType, _context);
			}

			return foundType;
		}

		/// <summary>
		/// Visits a return statement
		/// </summary>
		/// <param name="node">The return statement syntax</param>
		public override void VisitReturnStatement(ReturnStatementSyntax node)
		{
			SyntaxNode firstNode = node.ChildNodes().FirstOrDefault();
			if (firstNode != null)
			{
				var type = _handlerFactory.CreateForSyntaxNode(firstNode).Handle(firstNode);

				if (type != null)
				{
					_candidates.Add(type);
				}
			}
		}
	}
}
