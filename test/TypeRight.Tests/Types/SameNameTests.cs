using TypeRight.Tests.TestBuilders;
using TypeRight.Tests.Testers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TypeRight.Tests.Types
{
	/// <summary>
	/// Tests cases where classes have the same name, but different type args.  Typescript doesn't allow this
	/// </summary>
	[TestClass]
	public class SameNameTests : TypesTestBase
	{

		[TestMethod]
		public void SameName_DoesntAppendNumberForFirst()
		{
			// Test.TestClass
			AddExtractedClass("TestClass")
				.AddProperty("DontCare", "List<string>")
				.Commit();

			// Test.TestClass<T1>
			AddExtractedClass("TestClass")
				.AddGenericParameter("T1")
				.Commit();

			AssertThatTheReferenceTypeWithName("TestClass", 0)
				.ClassNameIs("TestClass");
		}

		[TestMethod]
		public void SameName_OneGenericParam_AppendsNumber()
		{
			// Test.TestClass
			AddExtractedClass("TestClass")
				.Commit();

			// Test.TestClass<T1>
			AddExtractedClass("TestClass")
				.AddGenericParameter("T1")
				.Commit();

			AssertThatTheReferenceTypeWithName("TestClass", 1)
				.ClassNameIs("TestClass_1<T1>");
		}

		[TestMethod]
		public void SameName_TwoGenericParam_AppendsNumber()
		{
			// Test.TestClass
			AddExtractedClass("TestClass")
				.Commit();

			// Test.TestClass<T1>
			AddExtractedClass("TestClass")
				.AddGenericParameter("T1")
				.Commit();

			// Test.TestClass<T1, T2>
			AddExtractedClass("TestClass")
				.AddGenericParameter("T1")
				.AddGenericParameter("T2")
				.Commit();
			AssertThatTheReferenceTypeWithName("TestClass", 2)
				.ClassNameIs("TestClass_2<T1, T2>");
		}

		[TestMethod]
		public void SameName_GenericProp()
		{
			// Test.TestClass
			AddExtractedClass("TestClass")
				.Commit();

			// Test.TestClass<T1>
			AddExtractedClass("TestClass")
				.AddGenericParameter("T1")
				.Commit();

			// Test.TestClass<T1, T2>
			AddExtractedClass("TestClass")
				.AddGenericParameter("T1")
				.AddGenericParameter("T2")
				.AddProperty("TestClass_1_Prop", "TestClass<int>")
				.AddProperty("TestClass_Prop", "TestClass")
				.Commit();

			AssertThatTheReferenceTypeWithName("TestClass", 2)
				.TestPropertyWithName("TestClass_1_Prop")
				.TypescriptNameIs($"{FakeTypePrefixer.Prefix}.TestClass_1<number>");
		}

		[TestMethod]
		public void SameName_NonGenericProp()
		{
			// Test.TestClass
			AddExtractedClass("TestClass")
				.Commit();

			// Test.TestClass<T1>
			AddExtractedClass("TestClass")
				.AddGenericParameter("T1")
				.Commit();

			// Test.TestClass<T1, T2>
			AddExtractedClass("TestClass")
				.AddGenericParameter("T1")
				.AddGenericParameter("T2")
				.AddProperty("TestClass_1_Prop", "TestClass<int>")
				.AddProperty("TestClass_Prop", "TestClass")
				.Commit();

			AssertThatTheReferenceTypeWithName("TestClass", 2)
				.TestPropertyWithName("TestClass_Prop")
				.TypescriptNameIs($"{FakeTypePrefixer.Prefix}.TestClass");
		}

		//[TestMethod]
		//public void SameName_Print()
		//{
		//	_packageTester.TestScriptText();			
		//}

		[TestMethod]
		public void ExtendsBaseType_WithSameName_DiffTypeParams()
		{

			AddExtractedClass("MyType")
				.AddProperty("BaseProperty", "string")
				.Commit();

			AddExtractedClass("MyType")
				.AddGenericParameter("T")
				.AddBaseClass("MyType")
				.AddProperty("GenericProp", "T")
				.Commit();

			AssertClassScriptTextIs(@"
/**  */
export interface MyType {
	/**  */
	BaseProperty: string;
}

/**  */
export interface MyType_1<T> extends MyType {
	/**  */
	GenericProp: T;
}");

		}
	}
}
