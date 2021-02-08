using TypeRight.ScriptWriting.TypeScript;
using TypeRight.Tests.TestBuilders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TypeRight.Tests.Types
{
	[TestClass]
	public class PrimitiveTypeTests : TypesTestBase
	{
		
		[TestMethod]
		public void PrimitiveTypes_TestInt()
		{
			AddDefaultExtractedClass()
				.AddProperty("IntProp", "int")
				.Commit();

			AssertThatTheDefaultReferenceType()
				.TestPropertyWithName("IntProp")
				.TypeMetadataIs(typeof(int));
		}

		/// <summary>
		/// Tests int typescript name
		/// </summary>
		[TestMethod]
		public void PrimitiveTypes_IntTypescriptName()
		{
			AddDefaultExtractedClass()
				.AddProperty("IntProp", "int")
				.Commit();

			AssertThatTheDefaultReferenceType()
				.TestPropertyWithName("IntProp")
				.TypescriptNameIs(TypeScriptHelper.NumericTypeName);
		}



		/// <summary>
		/// Test string typescript name
		/// </summary>
		[TestMethod]
		public void PrimitiveTypes_StringTypescriptName()
		{
			AddDefaultExtractedClass()
				.AddProperty("TestProp", "string")
				.Commit();

			AssertThatTheDefaultReferenceType()
				.TestPropertyWithName("TestProp")
				.TypescriptNameIs(TypeScriptHelper.StringTypeName);
		}


		/// <summary>
		/// Tests a nullable typescript name (i.e. int? == number)
		/// </summary>
		[TestMethod]
		public void PrimitiveTypes_NullableIntTypescriptName()
		{
			AddDefaultExtractedClass()
				.AddProperty("TestProp", "int?")
				.Commit();

			AssertThatTheDefaultReferenceType()
				.TestPropertyWithName("TestProp")
				.TypescriptNameIs(TypeScriptHelper.NumericTypeName);
		}

		/// <summary>
		/// Tests a nullable typescript name (i.e. int? == number)
		/// </summary>
		[TestMethod]
		public void PrimitiveTypes_IntArray()
		{
			AddDefaultExtractedClass()
				.AddProperty("TestProp", "int[]")
				.Commit();

			AssertThatTheDefaultReferenceType()
				.TestPropertyWithName("TestProp")
				.TypescriptNameIs(TypeScriptHelper.NumericTypeName + "[]");
		}
	}
}
