using Microsoft.CodeAnalysis;

namespace TypeRight.Workspaces.CodeModel
{
	/// <summary>
	/// Helpers for types
	/// </summary>
	static class TypeHelpers
	{
		/// <summary>
		/// Gets the normalized metadata name, with type parameters
		/// </summary>
		/// <param name="symbol">The named type symbol</param>
		/// <returns>The normalized metadata name</returns>
		public static string GetNormalizedMetadataName(this INamedTypeSymbol symbol)
		{
			SymbolDisplayFormat fmt = new SymbolDisplayFormat(
			   typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces
			   );

			string name = symbol.ToDisplayString(fmt);   // Only does the class name, minus generics

			if (symbol.TypeArguments.Length > 0)
			{
				name += $"`{symbol.TypeArguments.Length}";  // for generics, add the `#.  Example: System.Collections.Generic.List`1
			}

			return name;
		}

	}
}
