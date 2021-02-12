using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.Tests.TestBuilders;

namespace TypeRight.Tests
{
	public abstract class TypeRightTestBase
	{

		protected TestWorkspaceBuilder WorkspaceBuilder { get; private set; }

		[TestInitialize]
		public virtual void TestInitialize()
		{
			WorkspaceBuilder = new TestWorkspaceBuilder();

			WorkspaceBuilder.DefaultProject
				.AddFakeTypeRight()
			;

		}

		protected TestClassBuilder AddClass(string name, string ns = "Test")
		{
			return WorkspaceBuilder.DefaultProject
				.CreateClassBuilder(name, ns);
		}

		protected InterfaceBuilder AddInterface(string name, string ns = "Test")
		{
			return WorkspaceBuilder.DefaultProject
				.CreateInterfaceBuilder(name, ns);
		}

		protected TestEnumBuilder AddEnum(string name)
		{
			return WorkspaceBuilder.DefaultProject
				.CreateEnumBuilder(name);
		}
	}
}
