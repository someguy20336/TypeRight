using TypeRight.ScriptWriting.TypeScript;
using TypeRight.Tests.TestBuilders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TypeRight.Tests.Types
{
	[TestClass]
	public class CollectionsTests : TypesTestBase
	{

		[TestMethod]
		public void Collections_StringList()
		{
			AddDefaultExtractedClass()
				.AddProperty("TestProp", "List<string>")
				.Commit();

			AssertThatTheDefaultReferenceType()
				.TestPropertyWithName("TestProp")
				.TypescriptNameIs(TypeScriptHelper.StringTypeName + "[]");
		}

		[TestMethod]
		public void Collections_StringIntDictionary()
		{
			AddDefaultExtractedClass()
				.AddProperty("TestProp", "Dictionary<string, int>")
				.Commit();

			string expectedName = $"{{ [key: {TypeScriptHelper.StringTypeName}]: {TypeScriptHelper.NumericTypeName} }}";
			AssertThatTheDefaultReferenceType()
				.TestPropertyWithName("TestProp")
				.TypescriptNameIs(expectedName);
		}

        [TestMethod]
        public void Collections_IEnumerable_1()
        {
			AddDefaultExtractedClass()
				.AddProperty("TestProp", "IEnumerable<string>")
				.Commit();

			AssertThatTheDefaultReferenceType()
				.TestPropertyWithName("TestProp")
				.TypescriptNameIs(TypeScriptHelper.StringTypeName + "[]");

        }

		[TestMethod]
		public void Collections_IEnumerable()
		{
			AddDefaultExtractedClass()
				.AddProperty("TestProp", "System.Collections.IEnumerable")
				.Commit();

			AssertThatTheDefaultReferenceType()
				.TestPropertyWithName("TestProp")
				.TypescriptNameIs(TypeScriptHelper.AnyTypeName + "[]");

		}
	}
}
