using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.Attributes;
using TypeRight.TypeProcessing;

namespace TypeRight.Tests.TestBuilders
{
	class TestProjectBuilder
	{
		public const string DefaultNamespace = "Test";

		public AdhocWorkspace Workspace { get; set; }

		public ProjectId ProjectID { get; private set; }

		public TestProjectBuilder(AdhocWorkspace adhocWorkspace, ProjectId projId)
		{
			Workspace = adhocWorkspace;
			ProjectID = projId;
		}

		public TestClassBuilder CreateClassBuilder(string name, string @namespace = DefaultNamespace)
		{
			return new TestClassBuilder(this, name, @namespace);
		}

		public InterfaceBuilder CreateInterfaceBuilder(string name, string @namespace = DefaultNamespace)
		{
			return new InterfaceBuilder(this, name, DefaultNamespace);
		}

		public TestEnumBuilder CreateEnumBuilder(string name)
		{
			return new TestEnumBuilder(this, name);
		}

		public AssemblyAttributeBuilder CreateAssemblyAttributeBuilder(string name)
		{
			return new AssemblyAttributeBuilder(this, name);
		}

		public TestProjectBuilder AddFakeTypeRight()
		{
			string typeRightNamespace = "TypeRight.Attributes";
			CreateClassBuilder(typeof(ScriptActionAttribute).Name, typeRightNamespace)
				.AddBaseClass("System.Attribute")
				.Commit();

			CreateClassBuilder(typeof(ScriptObjectAttribute).Name, typeRightNamespace)
				.AddBaseClass("System.Attribute")
				.Commit();

			CreateClassBuilder(typeof(ScriptEnumAttribute).Name, typeRightNamespace)
				.AddBaseClass("System.Attribute")
				.Commit();

			CreateInterfaceBuilder(typeof(IEnumDisplayNameProvider).Name, typeRightNamespace)
				.AddProperty(nameof(IEnumDisplayNameProvider.DisplayName), "string")
				.AddProperty(nameof(IEnumDisplayNameProvider.Abbreviation), "string")
				.Commit();

			return this;
		}

		public TestProjectBuilder AddFakeMvc()
		{
			
			// Route attribute
			CreateClassBuilder(MvcConstants.RouteAttributeName, MvcConstants.AspNetCoreNamespace)
				.AddBaseClass("System.Attribute")
				.AddConstructor()
					.AddParameter("template", "string")
					.Commit()
				.Commit();

			CreateClassBuilder(MvcConstants.RouteAttributeName, MvcConstants.AspNetNamespace)
				.AddBaseClass("System.Attribute")
				.AddConstructor()
					.AddParameter("template", "string")
					.Commit()
				.Commit();

			// HTTP GET
			CreateClassBuilder(MvcConstants.HttpGetAttributeName, MvcConstants.AspNetCoreNamespace)
				.AddBaseClass("System.Attribute")
				.AddConstructor()
					.AddParameter("template", "string")
					.Commit()
				.Commit();

			CreateClassBuilder(MvcConstants.HttpGetAttributeName, MvcConstants.AspNetNamespace)
				.AddBaseClass("System.Attribute")
				.AddConstructor()
					.AddParameter("template", "string")
					.Commit()
				.Commit();

			// HTTP Post
			CreateClassBuilder(MvcConstants.HttpPostAttributeName, MvcConstants.AspNetCoreNamespace)
			.AddBaseClass("System.Attribute")
			.AddConstructor()
					.AddParameter("template", "string")
					.Commit()
			.Commit();

			CreateClassBuilder(MvcConstants.HttpPostAttributeName, MvcConstants.AspNetNamespace)
				.AddBaseClass("System.Attribute")
				.AddConstructor()
					.AddParameter("template", "string")
					.Commit()
				.Commit();

			// HTTP Put
			CreateClassBuilder(MvcConstants.HttpPutAttributeName, MvcConstants.AspNetCoreNamespace)
			.AddBaseClass("System.Attribute")
			.AddConstructor()
					.AddParameter("template", "string")
					.Commit()
			.Commit();

			CreateClassBuilder(MvcConstants.HttpPutAttributeName, MvcConstants.AspNetNamespace)
				.AddBaseClass("System.Attribute")
				.AddConstructor()
					.AddParameter("template", "string")
					.Commit()
				.Commit();

			// From body
			CreateClassBuilder(MvcConstants.FromBodyAttributeName, MvcConstants.AspNetCoreNamespace)
				.AddBaseClass("System.Attribute")
				.Commit();

			// From services
			CreateClassBuilder(MvcConstants.FromServicesAttributeName, MvcConstants.AspNetCoreNamespace)
				.AddBaseClass("System.Attribute")
				.Commit();

			// From query
			CreateClassBuilder(MvcConstants.FromQueryAttributeName, MvcConstants.AspNetCoreNamespace)
				.AddBaseClass("System.Attribute")
				.Commit();

			CreateClassBuilder(MvcConstants.JsonResultName, MvcConstants.AspNetCoreNamespace).Commit();
			CreateClassBuilder(MvcConstants.JsonResultName, MvcConstants.AspNetNamespace).Commit();

			CreateClassBuilder(MvcConstants.ActionResultName, MvcConstants.AspNetCoreNamespace)
				.AddGenericParameter("TValue")
				.Commit();

			return this;
		}
	}
}
