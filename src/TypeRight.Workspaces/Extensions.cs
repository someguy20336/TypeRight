using Microsoft.CodeAnalysis;
using System;

namespace TypeRight.Workspaces
{
	internal static class Extensions
	{
		/// <summary>
		/// Gets whether the given type symbol has the given base type
		/// </summary>
		/// <param name="typeSymbol"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool HasBaseType(this ITypeSymbol typeSymbol, INamedTypeSymbol type)
		{
			type = type ?? throw new ArgumentNullException(nameof(type));

			var baseType = typeSymbol.BaseType;
			while (baseType != null)
			{
				if (type.Equals(baseType))
				{
					return true;
				}
				baseType = baseType.BaseType;
			}

			return false;
		}

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
