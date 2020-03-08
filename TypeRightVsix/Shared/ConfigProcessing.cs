using TypeRight;
using TypeRightVsix.Imports;
using EnvDTE;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.Shell;
using TypeRight.VsixContract;

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
			ThreadHelper.ThrowIfNotOnUIThread();
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
			ThreadHelper.ThrowIfNotOnUIThread();
			return ScriptGenAssemblyCache.GetForProj(proj)?.ConfigManager.IsEnabled(proj.FullName) ?? false;
		}

		/// <summary>
		/// Gets whether a config file exists for the solution and it added to the project
		/// </summary>
		/// <returns>True if it exists and is added</returns>
		public static bool ConfigExistsForProject(Project proj)
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			string configPath = ScriptGenAssemblyCache.GetForProj(proj)?.ConfigManager.GetConfigFilepath(proj.FullName);
			if (string.IsNullOrEmpty(configPath))
			{
				return false;
			}
			bool configExists = File.Exists(configPath);

			bool existsButNotAdded = (configExists && !VsHelper.SolutionItemExists(configPath));
			return configExists && !existsButNotAdded;
		}

	}
}
