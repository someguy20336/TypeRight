using TypeRightTests.HelperClasses;
using TypeRightTests.TestBuilders;
using TypeRightTests.Testers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TypeRightTests.Tests
{
	/// <summary>
	/// Tests cases where classes have the same name, but different type args.  Typescript doesn't allow this
	/// </summary>
	[TestClass]
	public class SameNameTests
	{

		private static PackageTester _packageTester;

		[ClassInitialize]
		public static void SetupParse(TestContext context)
		{
			TestWorkspaceBuilder wkspBuilder = new TestWorkspaceBuilder();

			wkspBuilder.DefaultProject

				// Test.TestClass
				.CreateClassBuilder("TestClass")
					.AddProperty("DontCare", "List<string>")
					.Commit()
				
				// Test.TestClass<T1>
				.CreateClassBuilder("TestClass")
					.AddGenericParameter("T1")
					.AddProperty("AlsoDontCare", "int")
					.Commit()

				// Test.TestClass<T1, T2>
				.CreateClassBuilder("TestClass")
					.AddGenericParameter("T1")
					.AddGenericParameter("T2")
					.AddProperty("TestClass_1_Prop", "TestClass<int>")
					.AddProperty("TestClass_Prop", "TestClass")
					.Commit()
				;

			wkspBuilder.ClassParseFilter = new AlwaysAcceptFilter();

			_packageTester = wkspBuilder.GetPackageTester();
		}


		[TestMethod]
		public void SameName_DoesntAppendNumberForFirst()
		{
			_packageTester.TestReferenceTypeWithName("TestClass", 0)
				.ClassNameIs("TestClass");
		}

		[TestMethod]
		public void SameName_AppendsNumber()
		{
			_packageTester.TestReferenceTypeWithName("TestClass", 1)
				.ClassNameIs("TestClass_1<T1>");

			_packageTester.TestReferenceTypeWithName("TestClass", 2)
				.ClassNameIs("TestClass_2<T1, T2>");
		}

		[TestMethod]
		public void SameName_GenericProp()
		{
			_packageTester.TestReferenceTypeWithName("TestClass", 2)
				.TestPropertyWithName("TestClass_1_Prop")
				.TypescriptNameIs($"{ReferenceTypeTester.TestNamespace}.TestClass_1<number>");
		}

		[TestMethod]
		public void SameName_NonGenericProp()
		{
			_packageTester.TestReferenceTypeWithName("TestClass", 2)
				.TestPropertyWithName("TestClass_Prop")
				.TypescriptNameIs($"{ReferenceTypeTester.TestNamespace}.TestClass");
		}

		[TestMethod]
		public void SameName_Print()
		{
			_packageTester.TestScriptText();			
		}
	}
}
