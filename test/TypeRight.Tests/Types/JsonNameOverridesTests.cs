using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.ScriptWriting;
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
			TestClassBuilder builder = AddExtractedClass("MyType");
			
			builder.AddPropertyAndBuildAttributes("PropName", "string")
				.AddAttribute(KnownTypes.SystemTextJsonPropertyName)
					.AddConstructorArg("\"name\"")
					.Commit();

			builder.Commit();

			AssertClassScriptTextIs(@"
/**  */
export interface MyType {
	/**  */
	name: string;
}");

		}

		[TestMethod]
		public void NewtonsoftJson_NameIsConverted()
		{
			TestClassBuilder builder = AddExtractedClass("MyType");

			builder.AddPropertyAndBuildAttributes("PropName", "string")
				.AddAttribute(KnownTypes.NewtonsoftJsonPropertyName_v12)
					.AddNamedArg("PropertyName", "\"name\"")
					.Commit();

			builder.Commit();

			AssertClassScriptTextIs(@"
/**  */
export interface MyType {
	/**  */
	name: string;
}");

		}

		[TestMethod]
		public void NewtonsoftJson_NoOverride_NameIsNotConverted()
		{
			TestClassBuilder builder = AddExtractedClass("MyType");

			builder.AddPropertyAndBuildAttributes("PropName", "string")
				.AddAttribute(KnownTypes.NewtonsoftJsonPropertyName_v12)
					.Commit();

			builder.Commit();

			AssertClassScriptTextIs(@"
/**  */
export interface MyType {
	/**  */
	PropName: string;
}");
		}

		[TestMethod]
		public void DefaultCamelCase_PropNamesAreConverted()
		{
			WorkspaceBuilder.ProcessorSettings.NamingStrategy = NamingStrategy.Create(NamingStrategyType.Camel);
			TestClassBuilder builder = AddExtractedClass("MyType");

			builder.AddPropertyAndBuildAttributes("PropName", "string")
				.AddAttribute(KnownTypes.NewtonsoftJsonPropertyName_v12)
					.Commit();

			builder.Commit();

			AssertClassScriptTextIs(@"
/**  */
export interface MyType {
	/**  */
	propName: string;
}");
		}
	}
}
