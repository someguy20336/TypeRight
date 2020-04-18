using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TypeRight.CodeModel;
using TypeRight.Workspaces.CodeModel;

namespace TypeRight.Workspaces.Parsing
{
	internal class TaskReturnTypeHandler : MethodReturnTypeHandler
	{

		private INamedTypeSymbol _targetType;
		private ParseContext _context;

		public TaskReturnTypeHandler(ParseContext context)
		{
			_context = context;
			_targetType = context.Compilation.GetTypeByMetadataName(typeof(Task<>).FullName);
		}
		public override bool CanHandleMethodSymbol(IMethodSymbol method)
		{
			INamedTypeSymbol returnType = method.ReturnType as INamedTypeSymbol;
			if (returnType == null)
			{
				return false;
			}

			if (returnType.ConstructedFrom != _targetType)
			{
				return false;
			}


			var typeArg = returnType.TypeArguments[0];
			return typeArg.SpecialType != SpecialType.System_Object;    // Objects are handled by parsing the syntax
		}

		public override IType GetReturnType(ParseContext context, IMethodSymbol method)
		{
			INamedTypeSymbol returnType = method.ReturnType as INamedTypeSymbol;
			ITypeSymbol type = returnType.TypeArguments[0];
			// TODO What if action result?  Maybe just handle later..
			return RoslynType.CreateType(type, context);
		}
	}
}
