using TypeRight.Tests.TestBuilders;

namespace TypeRight.Tests
{
	public abstract class TypeRightTestBase
	{

		protected TestWorkspaceBuilder WorkspaceBuilder { get; private set; }

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
	}
}
