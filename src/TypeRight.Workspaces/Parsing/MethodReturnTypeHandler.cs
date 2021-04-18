using TypeRight.CodeModel;
using TypeRight.Workspaces.CodeModel;
using Microsoft.CodeAnalysis;

namespace TypeRight.Workspaces.Parsing
{
	/// <summary>
	/// A handler for the return type of a method.  Allows the return type of the method
	/// to be altered for a given method
	/// </summary>
	internal abstract class MethodReturnTypeHandler
	{
		/// <summary>
		/// Determines whether this handler can handle the given method
		/// </summary>
		/// <param name="currentType">The current type to evaluate</param>
		/// <param name="method">The method to check</param>
		/// <returns>True if this method can be handled by this object</returns>
		public abstract bool CanHandleType(ITypeSymbol currentType, IMethodSymbol method);

		/// <summary>
		/// Gets the return type for the method
		/// </summary>
		/// <param name="context">The parse context</param>
		/// <param name="currentType">The current return type to evaluate</param>
		/// <param name="method">The method symbol</param>
		/// <returns>The return type for the method</returns>
		public abstract IType GetReturnType(ParseContext context, ITypeSymbol currentType, IMethodSymbol method);
	}

	/// <summary>
	/// The default return type handler
	/// </summary>
	internal class DefaultMethodReturnTypeHandler : MethodReturnTypeHandler
	{
		/// <inheritdoc/>
		public override bool CanHandleType(ITypeSymbol currentType, IMethodSymbol method)
		{
			return true;
		}

		/// <inheritdoc/>
		public override IType GetReturnType(ParseContext context, ITypeSymbol currentType, IMethodSymbol method)
		{
			return RoslynType.CreateType(currentType, context);
		}
	}
}
