using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.CodeModel;
using TypeRight.CodeModel.Default;

namespace TypeRightTests.TestBuilders.TypeCollection
{
	class NamedTypeBuilder : IBuilderWithProperties
	{
		private TypeCollectionBuilder _collectionBuilder;
		private string _name;
		private string _namespace = TypeCollectionBuilder.DefaultNamespace;
		private string _resultPath = "";
		private INamedType _baseType;


		public List<SymbolInfo> Properties { get; } = new List<SymbolInfo>();

		public NamedTypeBuilder(TypeCollectionBuilder collectionBuilder, string typeName)
		{
			_collectionBuilder = collectionBuilder;
			_name = typeName;
		}

		public NamedTypeBuilder WithBaseType(string typeName)
		{
			_baseType = _collectionBuilder.GetNamedType(typeName);
			return this;
		}

		public NamedTypeBuilder WithResultPath(string path)
		{
			_resultPath = path;
			return this;
		}
		
		public TypeCollectionBuilder Build()
		{
			return _collectionBuilder.RegisterType(new NamedType(
				_name,
				$"{_namespace}.{_name}",
				new TypeFlags(),  // TODO?
				Properties.Select(p => new Property(p.Name, _collectionBuilder.GetNamedType(p.Type))).ToList(),	// props
				_baseType,	// base type,
				null,	// interfaces
				null,   // filepath
				null,   // constructed from type
				null,	// typeargs,
				null,	// fields
				null,	// methods,
				null,	// attributes
				null	// comments
				), _resultPath);
		}
	}
}
