using System;
using System.Collections.Generic;
using TypeRight.CodeModel;
using TypeRight.CodeModel.Default;

namespace TypeRight.Tests.TestBuilders.TypeCollection
{
	public class NamedTypeBuilder : IBuilderWithPropertyList
	{
		private string _name;
		private string _namespace = TypeCollectionBuilder.DefaultNamespace;
		private string _resultPath = "";
		private INamedType _baseType;

		public TypeCollectionBuilder TypeCollectionBuilder { get; }

		public List<IProperty> Properties { get; } = new List<IProperty>();

		public NamedTypeBuilder(TypeCollectionBuilder collectionBuilder, string typeName)
		{
			TypeCollectionBuilder = collectionBuilder;
			_name = typeName;
		}

		public NamedTypeBuilder WithBaseType(string typeName, string ns = TypeCollectionBuilder.DefaultNamespace)
		{
			_baseType = TypeCollectionBuilder.GetNamedType($"{ns}.{typeName}");
			return this;
		}

		public NamedTypeBuilder WithResultPath(string path)
		{
			_resultPath = path;
			return this;
		}

		public NamedTypeBuilder AddProperty(string name, Type type)
		{
			Properties.Add(new Property(name, TypeCollectionBuilder.GetNamedType(type)));
			return this;
		}

		public NamedTypeBuilder AddProperty(string name, string typeName, string ns = TypeCollectionBuilder.DefaultNamespace)
		{
			Properties.Add(new Property(name, TypeCollectionBuilder.GetNamedType($"{ns}.{typeName}")));
			return this;
		}
						
		public TypeCollectionBuilder Build()
		{
			return TypeCollectionBuilder.RegisterType(CreateNamedType(), _resultPath);
		}


		private INamedType CreateNamedType() => new NamedType(
				_name,
				$"{_namespace}.{_name}",
				new TypeFlags(),  // TODO?
				Properties, // props
				new List<IMethod>(),   // methods,
				_baseType,  // base type,
				null,   // interfaces
				"",   // filepath
				null,   // constructed from type
				null,   // typeargs,
				null,   // fields
				new List<IAttributeData>(),   // attributes
				null    // comments
				);
	}
}
