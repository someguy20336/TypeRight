using Microsoft.CodeAnalysis;
using System;
using System.Xml;

namespace TypeRight.Workspaces.CodeModel
{
	/// <summary>
	/// Helpful function for parsing
	/// </summary>
	internal static class ParseHelper
    {

		/// <summary>
		/// Gets whether the base type should be parsed.  
		/// </summary>
		/// <param name="symb">The Symbol</param>
		/// <returns>True if the base type should be parsed, otherwise false if not</returns>
		internal static bool ShouldParseBaseTypeOfType(ITypeSymbol symb)
        {
            SymbolDisplayFormat fmt = new SymbolDisplayFormat(
               typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
               genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters
               );

            if (symb.BaseType == null)
            {
                return false;
            }
            if (symb.TypeKind == TypeKind.Array || symb.TypeKind == TypeKind.Enum)
            {
                return false;
            }
            string typeName = symb.BaseType.ToDisplayString(fmt);

            if (typeName == typeof(object).FullName 
                || typeName == typeof(ValueType).FullName)
            {
                return false;
            }

            return true;
        }
	}
}
