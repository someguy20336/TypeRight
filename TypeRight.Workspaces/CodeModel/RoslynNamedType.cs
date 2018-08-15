using TypeRight.CodeModel;
using TypeRight.Workspaces.Parsing;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TypeRight.Workspaces.CodeModel
{
	class RoslynNamedType : RoslynType, INamedType
	{
		protected INamedTypeSymbol NamedTypeSymbol => TypeSymbol as INamedTypeSymbol;

		private Lazy<INamedType> _constructedFrom;

		private Lazy<IReadOnlyList<IType>> _typeArguments;

		private Lazy<INamedType> _baseType;
		
		private Lazy<IReadOnlyList<IProperty>> _properties;

		private Lazy<IReadOnlyList<IField>> _fields;

		private Lazy<IReadOnlyList<IMethod>> _methods;

		private Lazy<IReadOnlyList<IAttributeData>> _attrs;

		private Lazy<IReadOnlyList<INamedType>> _interfaces;

		public string FullName { get; }

		public INamedType ConstructedFromType => _constructedFrom.Value;

		public INamedType BaseType => _baseType.Value;
		
		public IReadOnlyList<IType> TypeArguments => _typeArguments.Value;

		public string Comments { get; }

		public IReadOnlyList<IProperty> Properties => _properties.Value;

		public IReadOnlyList<IField> Fields => _fields.Value;

		public IReadOnlyList<IMethod> Methods => _methods.Value;

		public IReadOnlyList<IAttributeData> Attributes => _attrs.Value;

		public IReadOnlyList<INamedType> Interfaces => _interfaces.Value;

		public string FilePath { get; }


		public RoslynNamedType(INamedTypeSymbol namedTypeSymbol, ParseContext context)
			: base(namedTypeSymbol, context)
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

			_baseType = new Lazy<INamedType>(() =>
			{
				if (ParseHelper.ShouldParseBaseTypeOfType(NamedTypeSymbol))
				{
					INamedTypeSymbol baseTypeSymbol = NamedTypeSymbol.BaseType;
					return new RoslynNamedType(baseTypeSymbol, context);
				}
				else
				{
					return null;
				}
			});
			
			// Properties
			_properties = new Lazy<IReadOnlyList<IProperty>>(() =>
			{
				List<IProperty> props = new List<IProperty>();
				foreach (IPropertySymbol propSymb in NamedTypeSymbol.GetMembers().OfType<IPropertySymbol>())
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
				return RoslynAttributeData.FromSymbol(namedTypeSymbol, context);
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
		}
		

		public override string ToString()
		{
			return FullName;
		}
	}
}
