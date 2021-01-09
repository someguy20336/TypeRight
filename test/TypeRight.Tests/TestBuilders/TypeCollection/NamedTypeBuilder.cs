using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.CodeModel;
using TypeRight.CodeModel.Default;

namespace TypeRight.Tests.TestBuilders.TypeCollection
{
	internal class NamedTypeBuilder : IBuilderWithPropertyList, IAttributable
	{
		private string _name;
		private string _namespace = TypeCollectionBuilder.DefaultNamespace;
		private string _resultPath = "";
		private string _filePath = @"C:\FolderA\FolderB\class.cs";
		private INamedType _baseType;

		public TypeCollectionBuilder TypeCollectionBuilder { get; }

		public List<IProperty> Properties { get; } = new List<IProperty>();
		public List<IMethod> Methods { get; } = new List<IMethod>();
		public List<IAttributeData> Attributes { get; } = new List<IAttributeData>();

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

		public NamedTypeBuilder WithFilePath(string path)
		{
			_filePath = path;
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

		public MethodBuilder AddMethod(string name, Type returnType)
		{
			return new MethodBuilder(this, name, TypeCollectionBuilder.GetNamedType(returnType));
		}

		public MethodBuilder AddMethod(string name, IType returnType)
		{
			return new MethodBuilder(this, name, returnType);
		}
				
		public TypeCollectionBuilder Build()
		{
			return TypeCollectionBuilder.RegisterType(CreateNamedType(), _resultPath);
		}

		public TypeCollectionBuilder BuildAsController()
		{
			return TypeCollectionBuilder.RegisterController(CreateNamedType());
		}

		private INamedType CreateNamedType() => new NamedType(
				_name,
				$"{_namespace}.{_name}",
				new TypeFlags(),  // TODO?
				Properties, // props
				Methods,   // methods,
				_baseType,  // base type,
				null,   // interfaces
				_filePath,   // filepath
				null,   // constructed from type
				null,   // typeargs,
				null,   // fields
				Attributes,   // attributes
				null    // comments
				);
	}
}
