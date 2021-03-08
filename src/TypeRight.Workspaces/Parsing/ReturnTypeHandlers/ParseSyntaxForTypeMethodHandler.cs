using TypeRight.CodeModel;
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

		/// <inheritdoc/>
		public override bool CanHandleType(ITypeSymbol currentType, IMethodSymbol method)
		{
			if (currentType is INamedTypeSymbol namedType)
			{
				return _appliesToReturnTypes.Contains(namedType.GetNormalizedMetadataName());
			}
			return false;
		}

		/// <inheritdoc/>
		public override IType GetReturnType(ParseContext context, ITypeSymbol currentType, IMethodSymbol method)
		{
			MvcActionMethodWalker actionMethodWalker = new MvcActionMethodWalker(context);
			return actionMethodWalker.GetReturnType(method);
		}
	}
}
