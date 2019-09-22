﻿using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.TypeProcessing;

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

		public TestClassBuilder CreateClassBuilder(string name, string @namespace = "Test")
		{
			return new TestClassBuilder(this, name, @namespace);
		}

		public InterfaceBuilder CreateInterfaceBuilder(string name)
		{
			return new InterfaceBuilder(this, name);
		}

		public TestEnumBuilder CreateEnumBuilder(string name)
		{
			return new TestEnumBuilder(this, name);
		}

		public AssemblyAttributeBuilder CreateAssemblyAttributeBuilder(string name)
		{
			return new AssemblyAttributeBuilder(this, name);
		}

		public TestProjectBuilder AddMvc()
		{
			// Basically going to just add a shim in there just to get things to work

			CreateClassBuilder(MvcControllerInfo.RouteAttributeName, MvcControllerInfo.AspNetCoreMvcNamespace)
				.AddBaseClass("System.Attribute")
				.AddConstructor()
					.AddParameter("template", "string")
					.Commit()
				.Commit();

			CreateClassBuilder(MvcControllerInfo.RouteAttributeName, MvcControllerInfo.AspNetMvcNamespace)
				.AddBaseClass("System.Attribute")
				.AddConstructor()
					.AddParameter("template", "string")
					.Commit()
				.Commit();

			return this;
		}
	}
}
