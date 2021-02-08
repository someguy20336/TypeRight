using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.Tests.TestBuilders;
using TypeRight.Tests.Testers;

namespace TypeRight.Tests.Types
{
	public abstract class TypesTestBase : TypeRightTestBase
	{
		protected string TestClassName => "TestClass";


		protected TestClassBuilder AddDefaultExtractedClass()
		{
			return AddClass(TestClassName).AddScriptObjectAttribute();
		}

		protected TestClassBuilder AddExtractedClass(string name)
		{
			return AddClass(name).AddScriptObjectAttribute();
		}

		protected ReferenceTypeTester AssertThatTheDefaultClass()
		{
			var packageTester = WorkspaceBuilder.GetPackageTester();
			return packageTester.TestReferenceTypeWithName(TestClassName);
		}
	}
}
