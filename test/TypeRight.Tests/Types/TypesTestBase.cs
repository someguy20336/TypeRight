using TypeRight.Tests.TestBuilders;
using TypeRight.Tests.Testers;

namespace TypeRight.Tests.Types
{
	public abstract class TypesTestBase : TypeRightTestBase
	{
		protected string TestTypeName => "TestType";

		protected TestEnumBuilder AddDefaultExtractedEnum()
		{
			return AddExtractedEnum(TestTypeName);
		}

		protected TestEnumBuilder AddExtractedEnum(string name)
		{
			return AddEnum(name)
				.AddScriptEnumAttribute();
		}

		protected TestClassBuilder AddDefaultExtractedClass()
		{
			return AddClass(TestTypeName).AddScriptObjectAttribute();
		}

		protected TestClassBuilder AddExtractedClass(string name)
		{
			return AddClass(name).AddScriptObjectAttribute();
		}

		protected InterfaceBuilder AddDefaultExtractedInterface()
		{
			return AddExtractedInterface(TestTypeName);
		}

		protected InterfaceBuilder AddExtractedInterface(string name)
		{
			return AddInterface(name).AddScriptObjectAttribute();
		}

		protected ReferenceTypeTester AssertThatTheDefaultReferenceType()
		{
			return AssertThatTheReferenceTypeWithName(TestTypeName);
		}

		protected EnumTester AssertThatTheDefaultEnumType()
		{
			var packageTester = WorkspaceBuilder.GetPackageTester();
			return packageTester.TestEnumWithName(TestTypeName);
		}

		protected ReferenceTypeTester AssertThatTheReferenceTypeWithName(string name, int? typeArgCnt = null)
		{
			var packageTester = WorkspaceBuilder.GetPackageTester();
			return packageTester.TestReferenceTypeWithName(name, typeArgCnt);
		}

		protected void AssertScriptTextIs(string expected)
		{
			var packageTester = WorkspaceBuilder.GetPackageTester();
			packageTester.AssertScriptText(expected);
		}
	}
}
