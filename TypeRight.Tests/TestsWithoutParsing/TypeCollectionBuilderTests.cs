using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.TypeProcessing;
using TypeRight.Tests.TestBuilders;
using TypeRight.Tests.TestBuilders.TypeCollection;

namespace TypeRight.Tests.TestsWithoutParsing
{
	/// <summary>
	/// Basically just tests that my type builder works as I would expect
	/// </summary>
	[TestClass]
	public class TypeCollectionBuilderTests
	{
		[TestMethod]
		public void BuildsSimpleNamedType()
		{
			ExtractedTypeCollection collection = TypeCollectionBuilder.Create()
				.AddNamedType("TestClass")
					.Build()
				.Build();

			Assert.AreEqual(1, collection.GetReferenceTypes().Count());
		}

		[TestMethod]
		public void BuildsSimpleNamedType_WithBaseType()
		{
			ExtractedTypeCollection collection = TypeCollectionBuilder.Create()
				.AddNamedType("BaseType")
					.Build()
				.AddNamedType("TestClass")
					.WithBaseType("BaseType")
					.Build()
				.Build();

			Assert.AreEqual(2, collection.GetReferenceTypes().Count());

			Assert.AreEqual("BaseType", collection.GetTypeByName("TestClass").BaseType.Type.Name);
		}

		[TestMethod]
		public void Property_IsAdded()
		{
			var collection = TypeCollectionBuilder.Create()
				.AddNamedType("PropType").Build()
				.AddNamedType("TestClass")
					.AddProperty("TestProp", "PropType")
					.Build()
				.Build();

			Assert.AreEqual("PropType", collection.GetTypeByName("TestClass").Properties.FirstOrDefault(p => p.Name == "TestProp").Type.Type.Name);
			AssertPropertyTypeName(collection, "PropType", "TestClass", "TestProp");
		}

		[TestMethod]
		public void PropertyWithKnownType_TypeIsResolved()
		{
			var collection = TypeCollectionBuilder.Create()
				.AddNamedType("TestClass")
					.AddProperty("TestProp", typeof(int))
					.Build()
				.Build();

			AssertPropertyTypeName(collection, typeof(int).Name, "TestClass", "TestProp");
		}

		private void AssertPropertyTypeName(ExtractedTypeCollection collection, string expectedName, string className, string propName)
			=> Assert.AreEqual(expectedName, collection.GetTypeByName(className).Properties.FirstOrDefault(p => p.Name == propName).Type.Type.Name);
	}
}
