using Buildalyzer;
using Buildalyzer.Workspaces;
using Microsoft.CodeAnalysis;
using System;
using System.IO;
using System.Linq;
using TypeRight.Workspaces.Parsing;

namespace TypeRight
{
	class Program
	{
		public const string ForceSwitch = "force";

		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				// TODO output help?
				return;
			}


			// TODO 
			//  - More Logging?  Arg parameter
			//  - Document how to test msbuild in case I forget (publishing)


			string projectPath = args[0];
			if (!Path.IsPathRooted(projectPath))
			{
				projectPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, projectPath));
			}

			if (!File.Exists(projectPath))
			{
				Console.Error.WriteLine($"Provided file path does not exist: {projectPath}");
				return;
			}

			if (!projectPath.EndsWith("csproj"))
			{
				Console.Error.WriteLine("Only csproj files are supported");
				return;
			}

			if (!BuildHelper.NeedsScriptGeneration(projectPath))
			{
				Console.WriteLine("Script generation skipped: generation in progress");
				return;
			}

			try
			{
				Console.WriteLine($"Generating scripts for project path: {projectPath}");
				BuildHelper.StartBuild(projectPath);
				RunGeneration(args, projectPath);
			}
			catch (Exception e)
			{

				Console.WriteLine("Script generation failed: " + e.Message);
				Console.WriteLine(e.StackTrace);
			}
			finally
			{
				BuildHelper.EndBuild(projectPath);
			}

		}

		private static void RunGeneration(string[] args, string projectPath)
		{
			AnalyzerManager mgr = new AnalyzerManager();
			IProjectAnalyzer projAnalyzer = mgr.GetProject(projectPath);

			using (Workspace workspace = projAnalyzer.GetWorkspace(true))
			{
				ProjectId mainProjId = workspace.CurrentSolution.Projects
					.Where(pr => pr.FilePath == projectPath).FirstOrDefault()?.Id;
				ProjectParser parser = new ProjectParser(workspace, mainProjId);
				ScriptGenEngine engine = new ScriptGenEngine();
				var result = engine.GenerateScripts(new ScriptGenerationParameters()
				{
					ProjectPath = projectPath,
					TypeIterator = parser,
					Force = args.HasSwitch(ForceSwitch)
				});

				if (!result.Success)
				{
					Console.WriteLine(result.ErrorMessage);
				}

			}
		}

	}
}
