using TypeRight.CodeModel;
using TypeRight.Workspaces.Parsing;
using Microsoft.CodeAnalysis;

namespace TypeRight.Workspaces.CodeModel
{
	internal class RoslynTypeParameter : RoslynType, ITypeParameter
	{
		public bool IsNullable { get; }
		public RoslynTypeParameter(ITypeSymbol sym, ParseContext context)
			: base(sym, context)
		{
			IsNullable = sym.NullableAnnotation == NullableAnnotation.Annotated;
		}

		public IType AsNonNullable() => new NonNullTypeParameter(this);
	}
}
