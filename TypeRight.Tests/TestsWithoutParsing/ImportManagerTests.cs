using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.ScriptWriting.TypeScript;
using TypeRight.TypeProcessing;
using TypeRight.Tests.TestBuilders;
using TypeRight.Tests.TestBuilders.TypeCollection;

namespace TypeRight.Tests.TestsWithoutParsing
{
	[TestClass]
	public class ImportManagerTests
	{

		[TestMethod]
		public void BaseType_AddedToImport()
		{
			string importFromPath = @"C:\FolderA\FolderB\BaseTypes.ts";
			string output = @"C:\FolderA\DerivedTypes.ts";
			var collection = TypeCollectionBuilder.Create()
				.AddNamedType("BaseType")
					.WithResultPath(importFromPath)
					.Build()
				.AddNamedType("DerivedType")
					.WithBaseType("BaseType")
					.WithResultPath(output)
					.Build()
				.Build();

			ImportManager importManager = ImportManager.FromTypes(collection, output);
			Assert.IsTrue(importManager.GetImportAtPath(importFromPath).Items.Contains("BaseType"));
		}

		[TestMethod]
		public void PropertyType_AddedToImport()
		{
			string importFromPath = @"C:\FolderA\FolderB\BaseTypes.ts";
			string output = @"C:\FolderA\DerivedTypes.ts";
			var collection = TypeCollectionBuilder.Create()
				.AddNamedType("SomeOtherType")
					.WithResultPath(importFromPath)
					.Build()
				.AddNamedType("DerivedType")
					.WithResultPath(output)
					.AddProperty("TestProp", "SomeOtherType")
					.Build()
				.Build();

			ImportManager importManager = ImportManager.FromTypes(collection, output);
			Assert.IsTrue(importManager.GetImportAtPath(importFromPath).Items.Contains("SomeOtherType"));
		}

		[TestMethod]
		public void PropertyType_SameFile_NotAddedToImport()
		{
			string output = @"C:\FolderA\DerivedTypes.ts";
			var collection = TypeCollectionBuilder.Create()
				.AddNamedType("SomeOtherType")
					.WithResultPath(output)
					.Build()
				.AddNamedType("DerivedType")
					.WithResultPath(output)
					.AddProperty("TestProp", "SomeOtherType")
					.Build()
				.Build();

			ImportManager importManager = ImportManager.FromTypes(collection, output);
			Assert.AreEqual(0, importManager.GetImports().Count());
		}
	}
}
