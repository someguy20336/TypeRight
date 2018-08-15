using TypeRight.Attributes;
using TypeRight.CodeModel;
using TypeRight.Workspaces.Parsing;
using Microsoft.CodeAnalysis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TypeRight.Workspaces.CodeModel
{
	class RoslynType : IType
	{
		protected ParseContext Context { get; private set; }
		protected ITypeSymbol TypeSymbol { get; set; }
		protected DocumentationProvider DocProvider => Context.DocumentationProvider;
		protected Compilation InCompilation => Context.Compilation;

		public string Name { get; }

		public TypeFlags Flags { get; }

		public RoslynType(ITypeSymbol typeSymbol, ParseContext context)
		{
			Context = context;
			TypeSymbol = GetFunctionalType(typeSymbol);
			Name = TypeSymbol.Name;

			Flags = new TypeFlags(
				isEnum: typeSymbol.TypeKind == TypeKind.Enum,
				isNullable: IsNullableType(),
				isArray: typeSymbol.TypeKind == TypeKind.Array,
				isList: IsListType(),
				isDictionary: IsDictionaryType(),
				isAnonymous: TypeSymbol.IsAnonymousType,
				isInterface: TypeSymbol.TypeKind == TypeKind.Interface
				);

		}

		/// <summary>
		/// Gets the functional type of this class.  Generally, this is just the same as the provided symbol.  If the 
		/// class does have an override class, that will be returned instead
		/// </summary>
		/// <param name="symb">The type symbol</param>
		/// <returns>The functional type</returns>
		private ITypeSymbol GetFunctionalType(ITypeSymbol symb)
		{
			INamedTypeSymbol funcTypeAttr = InCompilation.GetTypeByMetadataName(typeof(FunctionalTypeAttribute).FullName);
			ITypeSymbol functionalType = symb;
			foreach (AttributeData attr in symb.GetAttributes())
			{
				if (attr.AttributeClass.Equals(funcTypeAttr))
				{
					functionalType = attr.ConstructorArguments[0].Value as INamedTypeSymbol;
					break;
				}
			}

			return functionalType;
		}

		private bool IsNullableType()
		{
			INamedTypeSymbol nullableSymb = InCompilation.GetTypeByMetadataName(typeof(Nullable<>).FullName);
			return nullableSymb.Equals(TypeSymbol.OriginalDefinition);
		}

		private bool IsListType()
		{
			List<INamedTypeSymbol> listTypes = new List<INamedTypeSymbol>()
				{
					InCompilation.GetTypeByMetadataName(typeof(IList).FullName),
					InCompilation.GetTypeByMetadataName(typeof(IList<>).FullName)
				};
			return TypeSymbol.Interfaces.Any(nt => listTypes.Contains(nt));
		}

		private bool IsDictionaryType()
		{
			List<INamedTypeSymbol> dictTypes = new List<INamedTypeSymbol>()
				{
					InCompilation.GetTypeByMetadataName(typeof(IDictionary).FullName),
					InCompilation.GetTypeByMetadataName(typeof(IDictionary<,>).FullName)
				};
			return TypeSymbol.Interfaces.Any(nt => dictTypes.Contains(nt));
		}

		public static RoslynType CreateType(ITypeSymbol typeSymb, ParseContext context)
		{
			if (typeSymb is INamedTypeSymbol namedType)
			{
				return CreateNamedType(namedType, context);
			}
			else if (typeSymb is IArrayTypeSymbol arraySymb)
			{
				return new RoslynArrayType(arraySymb, context);
			}
			else if (typeSymb is ITypeParameterSymbol typeParam)
			{
				return new RoslynTypeParameter(typeParam, context);
			}
			else
			{
				return new RoslynType(typeSymb, context);
			}
		}

		public static RoslynNamedType CreateNamedType(INamedTypeSymbol namedType, ParseContext context)
		{
			return new RoslynNamedType(namedType, context);
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
