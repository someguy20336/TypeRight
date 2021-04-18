using TypeRight.CodeModel;
using TypeRight.Workspaces.Parsing;
using Microsoft.CodeAnalysis;
using System;

namespace TypeRight.Workspaces.CodeModel
{
	internal class RoslynArrayType : RoslynType, IArrayType
	{
		private Lazy<RoslynType> _elementInitializer;

		protected IArrayTypeSymbol ArrayTypeSymbol => TypeSymbol as IArrayTypeSymbol;

		public IType ElementType => _elementInitializer.Value;

		public RoslynArrayType(IArrayTypeSymbol arrayTypeSymbol, ParseContext context)
			:base(arrayTypeSymbol, context)
		{
			_elementInitializer = new Lazy<RoslynType>(() => RoslynType.CreateType(ArrayTypeSymbol.ElementType, context));
		}

		public override string ToString()
		{
			return $"{ElementType.ToString()}[]";
		}
	}
}
