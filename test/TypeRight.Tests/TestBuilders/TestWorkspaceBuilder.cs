using TypeRight.Workspaces.Parsing;
using TypeRight.Tests.Testers;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using TypeRight.TypeProcessing;
using TypeRight.CodeModel;

namespace TypeRight.Tests.TestBuilders
{
	public class TestWorkspaceBuilder
	{
		public const string TestProjectDir = @"C:\FolderA\FolderB";

		public const string DefaultResultPath = @"C:\FolderA\FolderB\DefaultResult.ts";


		public AdhocWorkspace Workspace { get; }

		public TestProjectBuilder DefaultProject { get; private set; }
				

		public ProcessorSettings ProcessorSettings { get; private set; } = new ProcessorSettings()
		{
			ProjectPath = TestProjectDir,
			DefaultResultPath = DefaultResultPath
		};
			
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
			ProjectParser workspaceParser = new ProjectParser(Workspace, projectId);
			TypeVisitor visitor = new TypeVisitor(ProcessorSettings);

			workspaceParser.IterateTypes(visitor);
			return new TypeCollectionTester(visitor.TypeCollection);
		}
	}
}
