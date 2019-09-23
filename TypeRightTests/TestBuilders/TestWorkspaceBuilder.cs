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

		public ParseFilterSettings FilterSettings { get; set; } = new ParseFilterSettings()
		{
			ClassFilter = new AlwaysRejectFilter(),
			ControllerFilter = new AlwaysRejectFilter(),
			EnumFilter = new AlwaysRejectFilter()
		};

		public TypeFilter ClassParseFilter
		{
			get => FilterSettings.ClassFilter;
			set => FilterSettings.ClassFilter = value;
		}

		public TypeFilter EnumParseFilter
		{
			get => FilterSettings.EnumFilter;
			set => FilterSettings.EnumFilter = value;
		}

		public ProcessorSettings ProcessorSettings { get; set; } = new ProcessorSettings()
		{
			DisplayNameFilter = new AlwaysRejectFilter(),
			ProjectPath = TestProjectDir,
			DefaultResultPath = DefaultResultPath
		};
	
		public TypeFilter DisplayNameFilter
		{
			get => ProcessorSettings.DisplayNameFilter;
			set => ProcessorSettings.DisplayNameFilter = value;
		}
		
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
			TypeVisitor visitor = new TypeVisitor(ProcessorSettings)
			{
				FilterSettings = FilterSettings
			};

			workspaceParser.IterateTypes(visitor);
			return new TypeCollectionTester(visitor.TypeCollection, DisplayNameFilter);
		}
	}
}
