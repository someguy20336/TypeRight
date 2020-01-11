using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.CodeModel;
using TypeRight.TypeProcessing;

namespace TypeRightTests.TestBuilders.TypeCollection
{
	/// <summary>
	/// Builds a type collection
	/// </summary>
	class TypeCollectionBuilder
	{

		public const string DefaultNamespace = "Test.TypeCollection";

		private ExtractedTypeCollection _extractedTypes = new ExtractedTypeCollection(new ProcessorSettings()
		{
			DefaultResultPath = @"C:\FolderA\FolderB\Results.ts",
			ProjectPath = @"C:\FolderA\Test.csproj"
		});

		public static TypeCollectionBuilder Create() => new TypeCollectionBuilder();

		public NamedTypeBuilder AddNamedType(string typeName)
		{
			return new NamedTypeBuilder(this, typeName);
		}

		public TypeCollectionBuilder RegisterType(INamedType type, string targetPath)
		{
			_extractedTypes.RegisterType(type, targetPath);
			return this;
		}

		public INamedType GetNamedType(string name)
		{
			return KnownTypes.TryResolveKnownType(name) ?? _extractedTypes.GetReferenceTypes()
				.Select(t => t.NamedType)
				.First(t => t.Name == name);
		}

		public ExtractedTypeCollection Build() => _extractedTypes;
	}
}
