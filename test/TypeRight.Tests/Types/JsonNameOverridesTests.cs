using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.Tests.TestBuilders;

namespace TypeRight.Tests.Types
{
	[TestClass]
	public class JsonNameOverridesTests : TypesTestBase
	{

		[TestInitialize]
		public override void TestInitialize()
		{
			base.TestInitialize();

			WorkspaceBuilder.DefaultProject.AddFakeJson();
		}

		[TestMethod]
		public void SystemTextJson_NameIsConverted()
		{
			TestClassBuilder builder = AddDefaultExtractedClass();
			
			builder.AddPropertyAndBuildAttributes("PropName", "string")
				.AddAttribute(KnownTypes.SystemTextJsonPropertyName)
					.AddConstructorArg("\"name\"")
					.Commit();

			builder.Commit();

			AssertThatTheDefaultReferenceType()
				.TestPropertyWithName("PropName")
				.OutputNameIs("name");

		}

		[TestMethod]
		public void NewtonsoftJson_NameIsConverted()
		{
			TestClassBuilder builder = AddDefaultExtractedClass();

			builder.AddPropertyAndBuildAttributes("PropName", "string")
				.AddAttribute(KnownTypes.NewtonsoftJsonPropertyName)
					.AddNamedArg("PropertyName", "\"name\"")
					.Commit();

			builder.Commit();

			AssertThatTheDefaultReferenceType()
				.TestPropertyWithName("PropName")
				.OutputNameIs("name");

		}

		[TestMethod]
		public void NewtonsoftJson_NoOverride_NameIsNotConverted()
		{
			TestClassBuilder builder = AddDefaultExtractedClass();

			builder.AddPropertyAndBuildAttributes("PropName", "string")
				.AddAttribute(KnownTypes.NewtonsoftJsonPropertyName)
					.Commit();

			builder.Commit();

			AssertThatTheDefaultReferenceType()
				.TestPropertyWithName("PropName")
				.OutputNameIs("PropName");

		}

		// TODO: test case for actual generated script
	}
}
