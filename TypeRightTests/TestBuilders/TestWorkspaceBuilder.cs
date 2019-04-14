using TypeRight.Configuration;
using TypeRight.TypeLocation;
using TypeRight.TypeFilters;
using TypeRight.Workspaces.Parsing;
using TypeRightTests.HelperClasses;
using TypeRightTests.Testers;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using TypeRight.TypeProcessing;

namespace TypeRightTests.TestBuilders
{
	class TestWorkspaceBuilder
	{
		public const string TestProjectDir = @"C:\FolderA\FolderB";

		public const string DefaultResultPath = @"C:\FolderA\FolderB\DefaultResult.ts";


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
		

		public TypeCollectionTester GetPackageTester()
		{
			return GetPackageTester(DefaultProject.ProjectID);
		}

		public TypeCollectionTester GetPackageTester(ProjectId projectId)
		{
			ProjectParser workspaceParser = new ProjectParser(Workspace, projectId, ParseOptions);
			TypeVisitor visitor = new TypeVisitor(new ProcessorSettings()
			{
				TypeNamespace = ReferenceTypeTester.TestNamespace,
				EnumNamespace = EnumTester.TestNamespace,
				DisplayNameFilter = DisplayNameFilter,
				MvcActionFilter = MvcActionFilter,
				ProjectPath = TestProjectDir,
				DefaultResultPath = DefaultResultPath
			});
			visitor.FilterSettings.ClassFilter = ClassParseFilter;
			visitor.FilterSettings.EnumFilter = EnumParseFilter;
			visitor.FilterSettings.ControllerFilter = ControllerParseFilter;

			workspaceParser.IterateTypes(visitor);
			return new TypeCollectionTester(visitor.TypeCollection, DisplayNameFilter, MvcActionFilter);
		}
	}
}
