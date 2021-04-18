using TypeRight.CodeModel;
using TypeRight.Workspaces.Parsing;
using Microsoft.CodeAnalysis;

namespace TypeRight.Workspaces.CodeModel
{
	internal class RoslynTypeParameter : RoslynType, ITypeParameter
	{
		public RoslynTypeParameter(ITypeSymbol sym, ParseContext context)
			: base(sym, context)
		{

		}
	}
}
