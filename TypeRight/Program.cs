using Buildalyzer;
using Buildalyzer.Workspaces;
using Microsoft.CodeAnalysis;
using System;
using System.IO;
using System.Linq;
using TypeRight.ScriptGeneration;
using TypeRight.Workspaces.Parsing;

namespace TypeRight
{
	class Program
	{

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
				string dir = new FileInfo(Path.Combine(typeof(Program).Assembly.Location)).DirectoryName;
				projectPath = Path.GetFullPath(Path.Combine(dir, projectPath));
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
				Console.WriteLine("Script generation skipped: done by extension");
				return;
			}

			AnalyzerManager mgr = new AnalyzerManager();
			ProjectAnalyzer projAnalyzer = mgr.GetProject(projectPath);

			using (Workspace workspace = projAnalyzer.GetWorkspace(true))
			{
				ProjectId mainProjId = workspace.CurrentSolution.Projects
					.Where(pr => pr.FilePath == projectPath).FirstOrDefault()?.Id;
				ProjectParser parser = new ProjectParser(workspace, mainProjId);
				ScriptGenEngine engine = new ScriptGenEngine(projectPath, parser);
				var result = engine.GenerateScripts();

				if (!result.Success)
				{
					Console.WriteLine(result.ErrorMessage);
				}
				
			}
			
		}
	}
}
