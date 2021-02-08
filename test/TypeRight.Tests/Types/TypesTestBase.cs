using TypeRight.Tests.TestBuilders;
using TypeRight.Tests.Testers;

namespace TypeRight.Tests.Types
{
	public abstract class TypesTestBase : TypeRightTestBase
	{
		protected string TestRefTypeName => "TestReferenceType";


		protected TestClassBuilder AddDefaultExtractedClass()
		{
			return AddClass(TestRefTypeName).AddScriptObjectAttribute();
		}

		protected TestClassBuilder AddExtractedClass(string name)
		{
			return AddClass(name).AddScriptObjectAttribute();
		}

		protected InterfaceBuilder AddDefaultExtractedInterface()
		{
			return AddExtractedInterface(TestRefTypeName);
		}

		protected InterfaceBuilder AddExtractedInterface(string name)
		{
			return AddInterface(name).AddScriptObjectAttribute();
		}

		protected ReferenceTypeTester AssertThatTheDefaultReferenceType()
		{
			var packageTester = WorkspaceBuilder.GetPackageTester();
			return packageTester.TestReferenceTypeWithName(TestRefTypeName);
		}
	}
}
