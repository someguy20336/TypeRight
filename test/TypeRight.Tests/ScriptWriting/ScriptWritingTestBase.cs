using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.Tests.TestBuilders;

namespace TypeRight.Tests.ScriptWriting
{
	public abstract class ScriptWritingTestBase
	{

		protected TestWorkspaceBuilder WorkspaceBuilder { get; private set; }

		protected virtual void InitializeDefaultBuilder()
		{
			WorkspaceBuilder = new TestWorkspaceBuilder();

			WorkspaceBuilder.DefaultProject
				.AddFakeTypeRight()
			;

		}
	}
}
