using TypeRight.CodeModel;
using TypeRight.Workspaces.CodeModel;
using Microsoft.CodeAnalysis;

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
		private string _appliesToReturnType;

		/// <summary>
		/// The method invocation forwarder
		/// </summary>
		private InvocationReturnForwardFilter _returnForwarder;

		/// <summary>
		/// Creates a new handler
		/// </summary>
		/// <param name="appliesToType">The full name of the type this applies to</param>
		/// <param name="returnTypeForwarder">The invocation forwarder</param>
		public ParseSyntaxForTypeMethodHandler(string appliesToType, InvocationReturnForwardFilter returnTypeForwarder)
		{
			_appliesToReturnType = appliesToType;
			_returnForwarder = returnTypeForwarder;
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
				return namedType.GetNormalizedMetadataName() == _appliesToReturnType;
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
			MvcActionMethodWalker actionMethodWalker = new MvcActionMethodWalker(context, _returnForwarder);
			return actionMethodWalker.GetReturnType(method);
		}
	}
}
