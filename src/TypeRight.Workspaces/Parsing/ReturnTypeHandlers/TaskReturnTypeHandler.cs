using Microsoft.CodeAnalysis;
using System.Threading.Tasks;
using TypeRight.CodeModel;

namespace TypeRight.Workspaces.Parsing
{
	internal class TaskReturnTypeHandler : MethodReturnTypeHandler
	{

		private INamedTypeSymbol _targetType;

		public TaskReturnTypeHandler(ParseContext context)
		{
			_targetType = context.Compilation.GetTypeByMetadataName(typeof(Task<>).FullName);
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
			return typeArg.SpecialType != SpecialType.System_Object;    // Objects are handled by parsing the syntax
		}

		public override IType GetReturnType(ParseContext context, ITypeSymbol currentType, IMethodSymbol method)
		{
			INamedTypeSymbol returnType = currentType as INamedTypeSymbol;
			ITypeSymbol type = returnType.TypeArguments[0];
			return context.GetMethodReturnType(type, method);
		}
	}
}
