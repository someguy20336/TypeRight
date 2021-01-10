using TypeRight.CodeModel;
using TypeRight.Workspaces.CodeModel;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace TypeRight.Workspaces.Parsing
{
	/// <summary>
	/// Method return handler that parses the method syntax for the return type
	/// </summary>
	public class ParseSyntaxForTypeMethodHandler : MethodReturnTypeHandler
	{
		/// <summary>
		/// The return type this applies to
		/// </summary>
		private HashSet<string> _appliesToReturnTypes;
		
		/// <summary>
		/// Creates a new handler
		/// </summary>
		/// <param name="appliesToTypes">The full name of the type this applies to</param>
		public ParseSyntaxForTypeMethodHandler(params string[] appliesToTypes)
		{
			_appliesToReturnTypes = new HashSet<string>(appliesToTypes.Distinct());
		}

		/// <summary>
		/// Returns true if the method is of the type it applies to
		/// </summary>
		/// <param name="method">The method name</param>
		/// <returns></returns>
		public override bool CanHandleMethodSymbol(IMethodSymbol method)
		{
			if (method.ReturnType is INamedTypeSymbol namedType)
			{
				return _appliesToReturnTypes.Contains(namedType.GetNormalizedMetadataName());
			}
			return false;
		}

		/// <summary>
		/// Gets the return type for this method by parsing the syntax
		/// </summary>
		/// <param name="context">The parse context</param>
		/// <param name="method">The method symbol</param>
		/// <returns>The return type</returns>
		public override IType GetReturnType(ParseContext context, IMethodSymbol method)
		{
			MvcActionMethodWalker actionMethodWalker = new MvcActionMethodWalker(context);
			return actionMethodWalker.GetReturnType(method);
		}
	}
}
