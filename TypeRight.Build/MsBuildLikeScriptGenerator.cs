using TypeRight.Configuration;
using TypeRight.ScriptGeneration;
using TypeRight.Workspaces.Parsing;
using Microsoft.Build.Utilities;
using Microsoft.CodeAnalysis;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TypeRight.Build
{
	/// <summary>
	/// Runs the script generation by first building a workspace
	/// </summary>
	class MsBuildLikeScriptGenerator : MarshalByRefObject
	{
		/// <summary>
		/// Gets or sets the project path to generate scripts for
		/// </summary>
		public string ProjectPath { get; set; }

		/// <summary>
		/// Gets or sets the parameters for the reference resolver
		/// </summary>
		public ReferenceResolverParameters Parameters { get; set; }

		/// <summary>
		/// Gets whether it was a success
		/// </summary>
		public bool Success { get; private set; }
				
		/// <summary>
		/// Gets or sets the logger
		/// </summary>
		public TaskLoggingHelper Log { get; set; }

		public void Execute()
		{
			ConfigOptions options = ConfigParser.GetForProject(ProjectPath);

			if (options == null || !options.Enabled)
			{
				Log.LogMessage("Script generation skipped: disabled by configuration");
				Success = true;
				return;
			}

			Log.LogMessage("Beginning script generation for project: " + ProjectPath);
			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
			try
			{
				IScriptGenerationResult result;
				// Generate the scripts for the given project
				using (WorkspaceBuilder builder = new WorkspaceBuilder(ProjectPath, new ReferenceResolver(Parameters)))
				{

					ProjectId mainProjId = builder.Workspace.CurrentSolution.Projects
						.Where(pr => pr.FilePath == ProjectPath).FirstOrDefault()?.Id;
					ProjectParser parser = new ProjectParser(builder.Workspace, mainProjId);
					ScriptGenEngine engine = new ScriptGenEngine(ProjectPath, parser);
					result = engine.GenerateScripts();
				}
				if (!result.Sucess)
				{
					Log.LogError("Script generation failed: " + result.ErrorMessage);
				}
				Success = result.Sucess;
			}
			catch (Exception e)
			{
				Log.LogError($"Script generation failed: {e.Message}");
				Success = false;
			}
			finally
			{
				AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
			}
		}


		/// <summary>
		/// Assembly resolve hack for some strange problem where my assemblies aren't being used
		/// My guess is that MSBuild is loading assemblies and this is trying to use those (incompatible)
		/// </summary>
		/// <param name="sender">the sender</param>
		/// <param name="args">The resolve event args</param>
		/// <returns>The resolved assembly</returns>
		private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			AssemblyName name = new AssemblyName(args.Name);
			string thisDirectory = new FileInfo(GetType().Assembly.Location).DirectoryName;

			string dllPath = Path.Combine(thisDirectory, name.Name + ".dll");
			if (File.Exists(dllPath))
			{
				Log.LogMessage($"Resolving {name.Name} v{name.Version.ToString()} in directory {thisDirectory}");
				return Assembly.LoadFrom(dllPath);
			}

			return null;
		}
		
	}
}
