using TypeRight;
using TypeRightVsix.Imports;
using EnvDTE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRightVsix.Shared
{
	/// <summary>
	/// Helper methods for processing the config file
	/// </summary>
	static class ConfigProcessing
	{

		/// <summary>
		/// Gets the list of projects that are enabled for script generation
		/// </summary>
		/// <returns></returns>
		public static List<Project> GetEnabledProjectsForSolution()
		{
			List<Project> enabledProj = new List<Project>();
			foreach (Project proj in VsHelper.Current.Dte.Solution.Projects)
			{
				// Not idea why, but ran into one case with an empty project name... Solution folder maybe?
				if (!string.IsNullOrEmpty(proj.FullName) && IsGenEnabledForProject(proj))
				{
					enabledProj.Add(proj);
				}
			}
			return enabledProj;
		}
		/// <summary>
		/// Determines if script generation is enabled
		/// </summary>
		/// <param name="projPath">The path of the project</param>
		/// <returns>True if enabled</returns>
		public static bool IsGenEnabledForProject(Project proj)
		{			
			IConfigOptions opt = ScriptGenAssemblyCache.GetForProj(proj)?.ConfigManager.GetForProject(proj.FullName);
			if (opt != null && opt.Enabled)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Gets whether a config file exists for the solution and it added to the project
		/// </summary>
		/// <returns>True if it exists and is added</returns>
		public static bool ConfigExistsForProject(Project proj)
		{
			string configPath = ScriptGenAssemblyCache.GetForProj(proj)?.ConfigManager.GetConfigFilepath(proj.FullName);
			if (string.IsNullOrEmpty(configPath))
			{
				return false;
			}
			bool configExists = File.Exists(configPath);

			bool existsButNotAdded = (configExists && !VsHelper.SolutionItemExists(configPath));
			return configExists && !existsButNotAdded;
		}

		/// <summary>
		/// Creates a config file for the given solution
		/// </summary>
		public static void CreateForProject(Project proj)
		{
			string configPath = ScriptGenAssemblyCache.GetForProj(proj)?.ConfigManager.GetConfigFilepath(proj.FullName);
			if (!File.Exists(configPath))
			{
				IConfigOptions config = ScriptGenAssemblyCache.GetForProj(proj).ConfigManager.CreateNew();
				
				FileInfo projFile = new FileInfo(proj.FullName);

				Uri projUri = new Uri(projFile.Directory.FullName, UriKind.Absolute);
				Uri fileUri = new Uri(Path.Combine(projFile.Directory.FullName, "Scripts"), UriKind.Absolute);

				string relative = projUri.MakeRelativeUri(fileUri).ToString();
				relative = "." + relative.Substring(projFile.Directory.Name.Length);
				config.ServerObjectsResultFilepath = relative + "/ServerObjects.ts";

				ScriptGenAssemblyCache.GetForProj(proj).ConfigManager.Save(config, configPath);
			}
			proj.ProjectItems.AddFromFile(configPath);
		}
	}
}
