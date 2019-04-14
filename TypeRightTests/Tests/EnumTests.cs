using TypeRightTests.HelperClasses;
using TypeRightTests.TestBuilders;
using TypeRightTests.Testers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TypeRightTests.Tests
{
	[TestClass]
	public class EnumTests
	{
		private static TypeCollectionTester _packageTester;

		/// <summary>
		/// Sets up a parse of this solution
		/// </summary>
		[ClassInitialize]
		public static void SetupParse(TestContext context)
		{
			TestWorkspaceBuilder wkspBuilder = new TestWorkspaceBuilder();

			wkspBuilder.DefaultProject
				// Display name attribute
				.CreateClassBuilder("DisplayNameAttribute")
					.AddBaseClass("Attribute")
					.AddProperty("DisplayName", "string")
					.AddProperty("Abbreviation", "string")
					.AddConstructor()
						.Commit()
					.AddConstructor()
						.AddParameter("dispName", "string")
						.Commit()
					.AddConstructor()
						.AddParameter("dispName", "string")
						.AddParameter("abbrev", "string")
						.Commit()
					.Commit()

				// Simple enum
				.CreateEnumBuilder("SimpleEnum")
					.AddMember("One", "1").Commit()
					.AddMember("Two", "2")
						.AddAttribute("DisplayNameAttribute")
							.AddConstructorArg("\"TestTwoName\"")
							.Commit()
						.Commit()
					.AddMember("Three", "3").Commit()
					.AddMember("Four", "44").Commit()
					.AddMember("Five", "5")
						.AddAttribute("DisplayNameAttribute")
							.AddConstructorArg("\"TestFiveName\"")
							.AddConstructorArg("\"TFiveAbbr\"")
							.Commit()
						.Commit()
					.AddMember("Six", "6")
						.AddAttribute("DisplayNameAttribute")
							.AddNamedArg("DisplayName", "\"SixDispName\"")
							.AddNamedArg("Abbreviation", "\"SixAbbr\"")
							.Commit()
						.Commit()
					.Commit()
				.CreateEnumBuilder("SecondEnumType")
					.AddMember("A", "1").Commit()
					.AddMember("B", "2").Commit()
					.AddMember("C", "3").Commit()
					.Commit()

				// Simple class
				.CreateClassBuilder("SimpleClass")
					.AddProperty("EnumProp", "SimpleEnum")
					.Commit()
				
				;
					

			wkspBuilder.EnumParseFilter = new AlwaysAcceptFilter();
			wkspBuilder.DisplayNameFilter = new AcceptWithName("DisplayNameAttribute");

			_packageTester = wkspBuilder.GetPackageTester();
		}

		[TestMethod]
		public void Enums_HasMember()
		{
			_packageTester.TestEnumsWithName("SimpleEnum")
				.HasMemberWithName("Three");
		}

		[TestMethod]
		public void Enums_TestEnumIntValue()
		{
			_packageTester.TestEnumsWithName("SimpleEnum")
				.MemberHasIntValue("Four", 44);
		}

		[TestMethod]
		public void Enums_TestOneCtorDisplayName()
		{
			_packageTester.TestEnumsWithName("SimpleEnum")
				.MemberDisplayNameIs("Two", "TestTwoName")
				.MemberAbbrevNameIs("Two", "TestTwoName");
		}

		[TestMethod]
		public void Enums_TestTwoCtorDisplayName()
		{
			_packageTester.TestEnumsWithName("SimpleEnum")
				.MemberDisplayNameIs("Five", "TestFiveName")
				.MemberAbbrevNameIs("Five", "TFiveAbbr");
		}

		[TestMethod]
		public void Enums_TestTwoNamedArgsDispName()
		{			
			_packageTester.TestEnumsWithName("SimpleEnum")
				.MemberDisplayNameIs("Six", "SixDispName")
				.MemberAbbrevNameIs("Six", "SixAbbr");
		}

		[TestMethod]
		public void Enums_PropertyTypeIsEnum()
		{
			_packageTester.TestReferenceTypeWithName("SimpleClass")
				.TestPropertyWithName("EnumProp")
				.TypescriptNameIs($"{EnumTester.TestNamespace}.SimpleEnum");
		}

		//[TestMethod]
		//public void Enums_ExtendedSyntax_Setting()
		//{
		//	// TODO: will need to write a parse filter for these
		//	//Assert.AreEqual(true, _testData.GetEnumType<ExtendedSyntaxEnum>().UseExtendedSyntax);
		//	//Assert.AreEqual(false, _testData.GetEnumType<ExplicitNotExtendedSyntax>().UseExtendedSyntax);
		//	//Assert.AreEqual(false, _testData.GetEnumType<DefaultSyntaxEnum>().UseExtendedSyntax);
		//}

		[TestMethod]
		public void Enums_Print()
		{
			_packageTester.TestScriptText();
		}

	}
}
