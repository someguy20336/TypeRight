using System;
using System.Collections.Generic;
using System.Linq;
using TypeRight.CodeModel;
using TypeRight.CodeModel.Default;
using TypeRight.TypeProcessing;

namespace TypeRight.Tests.TestBuilders.TypeCollection
{
	/// <summary>
	/// Builds a type collection
	/// </summary>
	public class TypeCollectionBuilder
	{

		public const string DefaultNamespace = "Test.TypeCollection";

		private ExtractedTypeCollection _extractedTypes = new ExtractedTypeCollection(new ProcessorSettings()
		{
			DefaultResultPath = @"C:\FolderA\FolderB\Results.ts",
			ProjectPath = @"C:\FolderA\Test.csproj"
		});

		private Dictionary<string, INamedType> _externalTypes = new Dictionary<string, INamedType>();

		public static TypeCollectionBuilder Create() => new TypeCollectionBuilder();

		private TypeCollectionBuilder()
		{
			RegisterExternalType(typeof(string));
			RegisterExternalType(typeof(int));
		}

		public NamedTypeBuilder AddNamedType(string typeName)
		{
			return new NamedTypeBuilder(this, typeName);
		}
		
		public TypeCollectionBuilder RegisterType(INamedType type, string targetPath)
		{
			_extractedTypes.RegisterType(type, targetPath);
			return this;
		}

		public TypeCollectionBuilder RegisterController(INamedType type)
		{
			_extractedTypes.RegisterController(type);
			return this;
		}

		public INamedType GetNamedType(Type type)
		{
			return GetNamedType(type.FullName);
		}

		public INamedType GetNamedType(string fullName)
		{
			return _externalTypes.ContainsKey(fullName)
				? _externalTypes[fullName]
				: _extractedTypes.GetReferenceTypes().Select(t => t.NamedType).First(t => t.FullName == fullName);
		}

		public ExtractedTypeCollection Build() => _extractedTypes;

		public TypeCollectionBuilder RegisterExternalType(Type type)
		{
			return RegisterExternalType(type.Name, type.Namespace);
		}

		public TypeCollectionBuilder RegisterExternalType(string name, string ns)
		{
			string fullName = $"{ns}.{name}";
			_externalTypes.Add(fullName, new NamedType(name, fullName));
			return this;
		}
	}
}
