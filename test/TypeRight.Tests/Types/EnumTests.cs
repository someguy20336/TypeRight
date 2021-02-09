using TypeRight.Tests.HelperClasses;
using TypeRight.Tests.TestBuilders;
using TypeRight.Tests.Testers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.Attributes;

namespace TypeRight.Tests.Types
{
	[TestClass]
	public class EnumTests : TypesTestBase
	{
		[TestInitialize]
		public override void TestInitialize()
		{
			base.TestInitialize();

			AddClass("DisplayNameAttribute")
				.AddInterface(typeof(IEnumDisplayNameProvider).FullName)
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
				.Commit();
		}

		[TestMethod]
		public void Enums_HasMember()
		{
			AddDefaultExtractedEnum()
				.AddMember("One", "1").Commit()
				.Commit();

			AssertThatTheDefaultEnumType()
				.HasMemberWithName("One");
		}

		[TestMethod]
		public void Enums_TestEnumIntValue()
		{
			AddDefaultExtractedEnum()
				.AddMember("One", "100").Commit()
				.Commit();

			AssertThatTheDefaultEnumType()
				.MemberHasIntValue("One", 100);
		}

		[TestMethod]
		public void Enums_TestOneCtorDisplayName()
		{
			AddDefaultExtractedEnum()
				.AddMember("Two", "2")
					.AddAttribute("DisplayNameAttribute")
						.AddConstructorArg("\"TestTwoName\"")
						.Commit()
					.Commit()
				.Commit();

			AssertThatTheDefaultEnumType()
				.MemberDisplayNameIs("Two", "TestTwoName")
				.MemberAbbrevNameIs("Two", "TestTwoName");
		}

		[TestMethod]
		public void Enums_TestTwoCtorDisplayName()
		{
			AddDefaultExtractedEnum()
				.AddMember("Five", "5")
					.AddAttribute("DisplayNameAttribute")
						.AddConstructorArg("\"TestFiveName\"")
						.AddConstructorArg("\"TFiveAbbr\"")
						.Commit()
					.Commit()
				.Commit();

			AssertThatTheDefaultEnumType()
				.MemberDisplayNameIs("Five", "TestFiveName")
				.MemberAbbrevNameIs("Five", "TFiveAbbr");
		}

		[TestMethod]
		public void Enums_TestTwoNamedArgsDispName()
		{
			AddDefaultExtractedEnum()
				.AddMember("Six", "6")
					.AddAttribute("DisplayNameAttribute")
						.AddNamedArg("DisplayName", "\"SixDispName\"")
						.AddNamedArg("Abbreviation", "\"SixAbbr\"")
						.Commit()
					.Commit()
				.Commit();

			AssertThatTheDefaultEnumType()
				.MemberDisplayNameIs("Six", "SixDispName")
				.MemberAbbrevNameIs("Six", "SixAbbr");
		}

		[TestMethod]
		public void Enums_PropertyTypeIsEnum()
		{
			AddExtractedEnum("SimpleEnum")
				.AddMember("A", "1").Commit()
				.Commit();

			AddDefaultExtractedClass()
				.AddProperty("EnumProp", "SimpleEnum")
				.Commit();

			AssertThatTheDefaultReferenceType()
				.TestPropertyWithName("EnumProp")
				.TypescriptNameIs($"{FakeTypePrefixer.Prefix}.SimpleEnum");
		}

		//[TestMethod]
		//public void Enums_ExtendedSyntax_Setting()
		//{
		//	// TODO: will need to write a parse filter for these
		//	//Assert.AreEqual(true, _testData.GetEnumType<ExtendedSyntaxEnum>().UseExtendedSyntax);
		//	//Assert.AreEqual(false, _testData.GetEnumType<ExplicitNotExtendedSyntax>().UseExtendedSyntax);
		//	//Assert.AreEqual(false, _testData.GetEnumType<DefaultSyntaxEnum>().UseExtendedSyntax);
		//}

		//[TestMethod]
		//public void Enums_Print()
		//{
		//	_packageTester.TestScriptText();
		//}

	}
}
