using TypeRight.CodeModel;
using TypeRight.Workspaces.CodeModel;
using Microsoft.CodeAnalysis;

namespace TypeRight.Workspaces.Parsing
{
	/// <summary>
	/// A handler for the return type of a method.  Allows the return type of the method
	/// to be altered for a given method
	/// </summary>
	public abstract class MethodReturnTypeHandler
	{
		/// <summary>
		/// Determines whether this handler can handle the given method
		/// </summary>
		/// <param name="method">The method to check</param>
		/// <returns>True if this method can be handled by this object</returns>
		public abstract bool CanHandleMethodSymbol(IMethodSymbol method);

		/// <summary>
		/// Gets the return type for the method
		/// </summary>
		/// <param name="context">The parse context</param>
		/// <param name="method">The method symbol</param>
		/// <returns>The return type for the method</returns>
		public abstract IType GetReturnType(ParseContext context, IMethodSymbol method);
	}

	/// <summary>
	/// The default return type handler
	/// </summary>
	public class DefaultMethodReturnTypeHandler : MethodReturnTypeHandler
	{
		/// <summary>
		/// Determines whether this handler can handle the given method
		/// </summary>
		/// <param name="method">The method to check</param>
		/// <returns>True if this method can be handled by this object</returns>
		public override bool CanHandleMethodSymbol(IMethodSymbol method)
		{
			return true;
		}

		/// <summary>
		/// Gets the return type for the method
		/// </summary>
		/// <param name="context">The parse context</param>
		/// <param name="method">The method symbol</param>
		/// <returns>The return type for the method</returns>
		public override IType GetReturnType(ParseContext context, IMethodSymbol method)
		{
			return RoslynType.CreateType(method.ReturnType, context);
		}
	}
}
