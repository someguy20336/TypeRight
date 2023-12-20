using Microsoft.CodeAnalysis;
using TypeRight.CodeModel;

namespace TypeRight.Workspaces.Parsing
{
	internal class ActionResultReturnTypeHandler : MethodReturnTypeHandler
	{

		private INamedTypeSymbol _targetType;

		public ActionResultReturnTypeHandler(ParseContext context)
		{
			_targetType = context.Compilation.GetTypeByMetadataName(MvcConstants.ActionResult_AspNetCore + "`1");
		}

		public override bool CanHandleType(ITypeSymbol currentType, IMethodSymbol method)
		{
			if (!(currentType is INamedTypeSymbol returnType))
			{
				return false;
			}

			if (!returnType.ConstructedFrom.Equals(_targetType, SymbolEqualityComparer.Default))
			{
				return false;
			}


			var typeArg = returnType.TypeArguments[0];
			return typeArg.SpecialType != SpecialType.System_Object;	// Objects are handled by parsing the syntax
		}

		public override IType GetReturnType(ParseContext context, ITypeSymbol currentType, IMethodSymbol method)
		{
			INamedTypeSymbol returnType = currentType as INamedTypeSymbol;
			return context.GetMethodReturnType(returnType.TypeArguments[0], method);
		}
	}
}
