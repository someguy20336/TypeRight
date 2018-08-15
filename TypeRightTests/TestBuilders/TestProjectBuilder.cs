using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRightTests.TestBuilders
{
	class TestProjectBuilder
	{
		public AdhocWorkspace Workspace { get; set; }

		public ProjectId ProjectID { get; private set; }

		public TestProjectBuilder(AdhocWorkspace adhocWorkspace, ProjectId projId)
		{
			Workspace = adhocWorkspace;
			ProjectID = projId;
		}

		public TestClassBuilder CreateClassBuilder(string name)
		{
			return new TestClassBuilder(this, name);
		}

		public InterfaceBuilder CreateInterfaceBuilder(string name)
		{
			return new InterfaceBuilder(this, name);
		}

		public TestEnumBuilder CreateEnumBuilder(string name)
		{
			return new TestEnumBuilder(this, name);
		}
	}
}
