using TypeRight.CodeModel;
using TypeRight.Workspaces.Parsing;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace TypeRight.Workspaces.CodeModel
{
	internal class RoslynNamedType : RoslynType, INamedType
	{

		protected INamedTypeSymbol NamedTypeSymbol => TypeSymbol as INamedTypeSymbol;

		private Lazy<INamedType> _constructedFrom;

		private Lazy<IReadOnlyList<IType>> _typeArguments;

		private INamedType _baseType;
		private bool _baseTypeProcessed = false;

		private Lazy<IReadOnlyList<IProperty>> _properties;

		private Lazy<IReadOnlyList<IField>> _fields;

		private Lazy<IReadOnlyList<IMethod>> _methods;

		private Lazy<IReadOnlyList<IAttributeData>> _attrs;

		private Lazy<IReadOnlyList<INamedType>> _interfaces;

		/// <summary>
		/// Gets the full name of the class, as known in code
		/// </summary>
		public string FullName { get; }

		/// <summary>
		/// Gets the type this type was constructed from (when this type is constructed from a generic)
		/// </summary>
		public INamedType ConstructedFromType => _constructedFrom.Value;

		/// <summary>
		/// Gets the base type of this type, if applicable
		/// </summary>
		public INamedType BaseType => GetBaseType();

		/// <summary>
		/// Gets the type arguments for this type
		/// </summary>
		public IReadOnlyList<IType> TypeArguments => _typeArguments.Value;

		/// <summary>
		/// Gets the comments for this type
		/// </summary>
		public string Comments { get; }

		/// <summary>
		/// Gets the properties for this type
		/// </summary>
		public IReadOnlyList<IProperty> Properties => _properties.Value;

		/// <summary>
		/// Gets the fields for this type
		/// </summary>
		public IReadOnlyList<IField> Fields => _fields.Value;

		/// <summary>
		/// Gets the methods for this type
		/// </summary>
		public IReadOnlyList<IMethod> Methods => _methods.Value;

		/// <summary>
		/// Gets the attributes for this type
		/// </summary>
		public IReadOnlyList<IAttributeData> Attributes => _attrs.Value;

		/// <summary>
		/// Gets the interfaces implemented by this type
		/// </summary>
		public IReadOnlyList<INamedType> Interfaces => _interfaces.Value;
		
		/// <summary>
		/// TODO: I DONT WANT THIS
		/// </summary>
		public string FilePath { get; }

		/// <summary>
		/// Gets the flags for this type
		/// </summary>
		public TypeFlags Flags { get; }


		public RoslynNamedType(INamedTypeSymbol namedTypeSymbol, ParseContext context)
			: base(
				  // For unbound generics, we need to use the type it is constructed from to actuall get information about it
				  namedTypeSymbol.IsUnboundGenericType ? namedTypeSymbol.ConstructedFrom : namedTypeSymbol, 
				  context
				  )
		{

			Comments = context.DocumentationProvider.GetDocumentationForSymbol(NamedTypeSymbol).Summary;
			FullName = NamedTypeSymbol.GetNormalizedMetadataName();  // TODO May need to rethink
			FilePath = namedTypeSymbol.DeclaringSyntaxReferences.FirstOrDefault()?.SyntaxTree?.FilePath;  // TODO get rid of this

			_constructedFrom = new Lazy<INamedType>(() => new RoslynNamedType(NamedTypeSymbol.ConstructedFrom, context));

			_typeArguments = new Lazy<IReadOnlyList<IType>>(() =>
			{
				List<IType> typeArgs = new List<IType>();
				foreach (ITypeSymbol typeParam in NamedTypeSymbol.TypeArguments)
				{
					typeArgs.Add(CreateType(typeParam, context));
				}
				return typeArgs;
			});


			// Properties
			_properties = new Lazy<IReadOnlyList<IProperty>>(() =>
			{
				List<IProperty> props = new List<IProperty>();
				IEnumerable<IPropertySymbol> properties = NamedTypeSymbol.GetMembers().OfType<IPropertySymbol>();

				foreach (IPropertySymbol propSymb in properties)
				{
					if (propSymb.GetMethod.DeclaredAccessibility == Accessibility.Public)
					{
						RoslynProperty clientProp = new RoslynProperty(propSymb, context);
						props.Add(clientProp);
					}
				}
				return props;
			});

			// fields
			_fields = new Lazy<IReadOnlyList<IField>>(() =>
			{
				List<IField> fields = new List<IField>();
				foreach (IFieldSymbol fieldSymb in NamedTypeSymbol.GetMembers().OfType<IFieldSymbol>())
				{
					if (fieldSymb.DeclaredAccessibility == Accessibility.Public)
					{
						RoslynField field = new RoslynField(fieldSymb, context);
						fields.Add(field);
					}
				}
				return fields;
			});

			// methods
			_methods = new Lazy<IReadOnlyList<IMethod>>(() =>
			{
				List<IMethod> methods = new List<IMethod>();
				foreach (IMethodSymbol methodSymb in NamedTypeSymbol.GetMembers().OfType<IMethodSymbol>())
				{
					if (methodSymb.DeclaredAccessibility == Accessibility.Public)
					{
						RoslynMethod clientProp = new RoslynMethod(methodSymb, context);
						methods.Add(clientProp);
					}
				}
				return methods;
			});

			// Attributes
			_attrs = new Lazy<IReadOnlyList<IAttributeData>>(() =>
			{
				return RoslynAttributeData.FromSymbol(NamedTypeSymbol, context);
			});

			// Interfaces
			_interfaces = new Lazy<IReadOnlyList<INamedType>>(() =>
			{
				List<INamedType> interfaces = new List<INamedType>();
				foreach (INamedTypeSymbol interfaceSym in NamedTypeSymbol.Interfaces)
				{
					interfaces.Add(CreateNamedType(interfaceSym, context));
				}
				return interfaces;
			});

			Flags = new TypeFlags(
				isEnum: namedTypeSymbol.TypeKind == TypeKind.Enum,
				isNullable: IsNullableType(),
				isArray: namedTypeSymbol.TypeKind == TypeKind.Array,
				isList: IsListType(),
				isDictionary: IsDictionaryType(),
				isAnonymous: TypeSymbol.IsAnonymousType,
				isInterface: TypeSymbol.TypeKind == TypeKind.Interface
				);
		}


		private bool IsNullableType()
		{
			INamedTypeSymbol nullableSymb = InCompilation.GetTypeByMetadataName(typeof(Nullable<>).FullName);
			return nullableSymb.Equals(TypeSymbol.OriginalDefinition, SymbolEqualityComparer.Default);
		}


		private bool IsListType()
		{
			if (IsDictionaryType())
			{
				return false;  // Dictionary types are IEnumerable, so we don't want to include that
			}

			// First check for list types
			List<INamedTypeSymbol> isTypes = new List<INamedTypeSymbol>()
			{
				InCompilation.GetTypeByMetadataName(typeof(IEnumerable).FullName),
				InCompilation.GetTypeByMetadataName(typeof(IEnumerable<>).FullName),
				InCompilation.GetTypeByMetadataName(typeof(IReadOnlyList<>).FullName)
			};

			if (isTypes.Contains(NamedTypeSymbol.ConstructedFrom))
			{
				return true;
			}

			// Then fall back to checking interfaces
			List<INamedTypeSymbol> listTypes = new List<INamedTypeSymbol>()
			{
				InCompilation.GetTypeByMetadataName(typeof(IList).FullName),
				InCompilation.GetTypeByMetadataName(typeof(IList<>).FullName),
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

		private INamedType GetBaseType()
		{
			if (!_baseTypeProcessed)
			{
				if (ParseHelper.ShouldParseBaseTypeOfType(NamedTypeSymbol))
				{
					INamedTypeSymbol baseTypeSymbol = NamedTypeSymbol.BaseType;
					_baseType = new RoslynNamedType(baseTypeSymbol, Context);
				}
				_baseTypeProcessed = true;
			}
			return _baseType;
		}


		public override string ToString()
		{
			return FullName;
		}
	}
}
