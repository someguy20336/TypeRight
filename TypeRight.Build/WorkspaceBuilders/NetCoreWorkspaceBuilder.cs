using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Buildalyzer;
using Buildalyzer.Workspaces;
using Microsoft.CodeAnalysis;
using MsBuild = Microsoft.Build.Evaluation;

namespace TypeRight.Build.WorkspaceBuilders
{
	/// <summary>
	/// workspace builder for .NET core projects.  I found it easiest to use Buildalyzer here, but it didn't work for .net framework
	/// </summary>
	class NetCoreWorkspaceBuilder : WorkspaceBuilder
	{
		public NetCoreWorkspaceBuilder(string projPath) : base(projPath)
		{
		}

		/// <summary>
		/// Creates the project info from the msBuild project
		/// </summary>
		/// <param name="msBuildProj">The ms Build project</param>
		/// <returns>The project info</returns>
		protected override ProjectInfo CreateProjectInfo(MsBuild.Project msBuildProj)
		{
			AnalyzerManager mgr = new AnalyzerManager();
			ProjectAnalyzer projAnalyzer = mgr.GetProject(ProjectPath);
			AdhocWorkspace workspace = projAnalyzer.GetWorkspace(true);  // This doesn't seems to work either way
			Project proj = workspace.CurrentSolution.Projects.First();

			StringBuilder builder = new StringBuilder();

			IEnumerable<MsBuild.ProjectItem> compile = msBuildProj.Items.Where(itm => itm.ItemType == "Compile");
			foreach (MsBuild.ProjectItem item in compile)
			{
				builder.Append($"{item.EvaluatedInclude} ");
			}

			// /out provides a name
			builder.Append($"/out:{new FileInfo(msBuildProj.FullPath).Name} ");

			// target: TODO: What does this do?
			builder.Append("/target:library ");

			ProjectInfo info = CommandLineProject.CreateProjectInfo(
				proj.Name,
				LanguageNames.CSharp,
				CommandLineParser.SplitCommandLineIntoArguments(builder.ToString(), true),
				msBuildProj.DirectoryPath,
				null);

			// Add the references found by buildalyzer
			info = info.WithFilePath(msBuildProj.FullPath)
				.WithMetadataReferences(proj.MetadataReferences)
				.WithAnalyzerReferences(proj.AnalyzerReferences);


			return info;
		}

	}
}
