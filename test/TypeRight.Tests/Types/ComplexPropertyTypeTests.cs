using TypeRight.ScriptWriting.TypeScript;
using TypeRight.Tests.TestBuilders;
using TypeRight.Tests.Testers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TypeRight.Tests.Types
{
	[TestClass]
	public class ComplexPropertyTypeTests : TypesTestBase
	{

		[TestMethod]
		public void Complex_AnyForNotExtracted()
		{
			AddClass("NotExtracted").Commit();

			AddDefaultExtractedClass()
				.AddProperty("Property", "NotExtracted")
				.Commit();

			AssertThatTheDefaultReferenceType()
				.TestPropertyWithName("Property")
				.TypescriptNameIs(TypeScriptHelper.AnyTypeName);
		}

		[TestMethod]
		public void Complex_HasExtractedClassProperty()
		{
			AddExtractedClass("Extracted").Commit();

			AddDefaultExtractedClass()
				.AddProperty("Property", "Extracted")
				.Commit();

			AssertThatTheDefaultReferenceType()
				.TestPropertyWithName("Property")
				.TypescriptNameIs($"{FakeTypePrefixer.Prefix}.Extracted");

		}

		// TODO: probably have a writing test...
		//[TestMethod]
		//public void Complex_ScriptWrites()
		//{
		//	_packageTester.TestScriptText();
		//}
	}
}
