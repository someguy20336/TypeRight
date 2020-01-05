using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using TypeRight.CodeModel;
using TypeRight.TypeProcessing;
using TypeRight.Workspaces.CodeModel;

namespace TypeRight.Workspaces.Parsing
{
	internal class ActionResultReturnTypeHandler : MethodReturnTypeHandler
	{

		private INamedTypeSymbol _targetType;

		public ActionResultReturnTypeHandler(ParseContext context)
		{
			_targetType = context.Compilation.GetTypeByMetadataName(MvcConstants.ActionResult_AspNetCore + "`1");
		}

		public override bool CanHandleMethodSymbol(IMethodSymbol method)
		{
			INamedTypeSymbol returnType = method.ReturnType as INamedTypeSymbol;
			if (returnType == null)
			{
				return false;
			}

			return returnType.ConstructedFrom == _targetType;
		}

		public override IType GetReturnType(ParseContext context, IMethodSymbol method)
		{
			INamedTypeSymbol returnType = method.ReturnType as INamedTypeSymbol;
			return RoslynType.CreateType(returnType.TypeArguments[0], context);
		}
	}
}
