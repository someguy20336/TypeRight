using TypeRight.Configuration;
using TypeRight.Packages;
using TypeRight.TypeFilters;
using TypeRight.Workspaces.Parsing;
using TypeRightTests.HelperClasses;
using TypeRightTests.Testers;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace TypeRightTests.TestBuilders
{
	class TestWorkspaceBuilder
	{
		
		public AdhocWorkspace Workspace { get; }

		public TestProjectBuilder DefaultProject { get; private set; }

		public TypeFilter ClassParseFilter { get; set; } = new AlwaysRejectFilter();

		public TypeFilter EnumParseFilter { get; set; } = new AlwaysRejectFilter();

		public TypeFilter DisplayNameFilter { get; set; } = new AlwaysRejectFilter();

		public TypeFilter ControllerParseFilter { get; set; } = new AlwaysRejectFilter();

		public TypeFilter MvcActionFilter { get; set; } = new AlwaysRejectFilter();

		public TypeRight.Workspaces.Parsing.ParseOptions ParseOptions { get; set; } = TypeRight.Workspaces.Parsing.ParseOptions.GetDefault();

		public TestWorkspaceBuilder()
		{
			Workspace = new AdhocWorkspace();
			DefaultProject = AddNewProject("DefaultProject");			
		}

		public TestProjectBuilder AddNewProject(string name)
		{
			ProjectId projectId = ProjectId.CreateNewId();
			VersionStamp vers = VersionStamp.Create();
			ProjectInfo projectInfo = ProjectInfo.Create(projectId, vers, name, name, LanguageNames.CSharp);
			MetadataReference reference = MetadataReference.CreateFromFile(typeof(int).Assembly.Location);
			projectInfo = projectInfo.WithMetadataReferences(new List<MetadataReference>() { reference });
			return new TestProjectBuilder(Workspace, Workspace.AddProject(projectInfo).Id);
		}
		

		public PackageTester GetPackageTester()
		{
			return GetPackageTester(DefaultProject.ProjectID);
		}

		public PackageTester GetPackageTester(ProjectId projectId)
		{
			ProjectParser workspaceParser = new ProjectParser(Workspace, projectId, ParseOptions);
			TypeVisitor vistor = new TypeVisitor();
			vistor.FilterSettings.ClassFilter = ClassParseFilter;
			vistor.FilterSettings.EnumFilter = EnumParseFilter;
			vistor.FilterSettings.ControllerFilter = ControllerParseFilter;

			ScriptPackage package = ScriptPackage.BuildPackage(workspaceParser, vistor);
			return new PackageTester(package, DisplayNameFilter, MvcActionFilter);
		}
	}
}
