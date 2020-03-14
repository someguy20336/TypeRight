﻿using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using TypeRight.VsixContract;
using TypeRightVsix.Shared;
using System.Reflection;

namespace TypeRightVsix.Imports
{
	/// <summary>
	/// A generator that was imported for a specific version
	/// </summary>
	class ImportedGenerator
	{

		/// <summary>
		/// Gets the assembly version for this generator
		/// </summary>
		public string AssemblyVersion { get; }

		/// <summary>
		/// Gets the assembly path for this version
		/// </summary>
		public string AssemblyDirectory { get; }

		/// <summary>
		/// Gets or sets the engine provider
		/// </summary>
		[Import(typeof(IScriptGenerationAdapter))]
		public IScriptGenerationAdapter ScriptGenerator { get; set; }		
		
		/// <summary>
		/// Gets or sets the config manager
		/// </summary>
		[Import(typeof(IConfigManager))]
		public IConfigManager ConfigManager { get; set; }

		/// <summary>
		/// Creates a new imported generator
		/// </summary>
		/// <param name="cacheBasePath">The base path of the cache</param>
		/// <param name="version">the version to create the generator for</param>
		/// <param name="nugetPath">The path to the nuget install</param>
		public ImportedGenerator(string cacheBasePath, string version, string nugetPath)
		{
			Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
			AssemblyVersion = version;
			string importDir = Path.Combine(nugetPath, "tools", "adapter");
			string legacyDir = Path.Combine(nugetPath, "tools");


#if DEBUG && !NUGET
			// Use the test build
			string solnDir = new FileInfo(VsHelper.Current.Dte.Solution.FullName).Directory.FullName;
			string relativeBuildDir = @"..\..\TypeRight.Workspaces.VsixAdapter\bin\Debug\";
			importDir = Path.GetFullPath(Path.Combine(solnDir, relativeBuildDir));
#endif

			if (Directory.Exists(importDir))
			{
				AssemblyDirectory = importDir;
			}
			else if (Directory.Exists(legacyDir))
			{
				AssemblyDirectory = legacyDir;
			}
			else
			{
				AssemblyDirectory = "";
				TrySetNullImporters();
				return;
			}
					   
			// If we don't have this version locally cached, do that now
			string cachePath = Path.Combine(cacheBasePath, AssemblyVersion);

#if DEBUG && !NUGET
			if (Directory.Exists(cachePath))
			{
				Directory.Delete(cachePath);
			}
#endif
			if (!Directory.Exists(cachePath))
			{
				Directory.CreateDirectory(cachePath);
				DirectoryCopy(AssemblyDirectory, cachePath, true);
			}

			// Import the files
			DirectoryCatalog catalog = new DirectoryCatalog(cachePath, "TypeRight*.dll");
			using (CompositionContainer container = new CompositionContainer(catalog, true))
			{
				try
				{
					container.SatisfyImportsOnce(this);
				}
				catch (Exception)
				{
					TryGetLegacy(cachePath);
				}
			}

			TrySetNullImporters();
		}

		private void TrySetNullImporters()
		{
			Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
			VsHelper.SetStatusBar("Failed to load compatible version of TypeRight - you may need to update the Nuget package");
			ScriptGenerator = ScriptGenerator ?? new NullScriptGenerationAdapter();
			ConfigManager = ConfigManager ?? new NullConfigManager();
		}

		private void TryGetLegacy(string cachePath)
		{
			try
			{
				string genPath = Path.Combine(cachePath, "TypeRight.Workspaces.dll");
				Assembly assembly = Assembly.LoadFrom(genPath);
				Type type = assembly.GetType("TypeRight.Workspaces.Parsing.WorkspaceGenEngineProvider");
				dynamic legacyGenerator = Activator.CreateInstance(type);
				ScriptGenerator = new LegacyScriptGenerationAdapter(legacyGenerator);

				string configPath = Path.Combine(cachePath, "TypeRight.dll");
				assembly = Assembly.LoadFrom(configPath);
				type = assembly.GetType("TypeRight.Configuration.ConfigManager");
				dynamic legacyConfig = Activator.CreateInstance(type);
				ConfigManager = new LegacyConfigManagerAdapter(legacyConfig);
			}
			catch (Exception)
			{
				// Will be set to null
			}
		}

		private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
		{
			// Get the subdirectories for the specified directory.
			DirectoryInfo dir = new DirectoryInfo(sourceDirName);

			if (!dir.Exists)
			{
				throw new DirectoryNotFoundException(
					"Source directory does not exist or could not be found: "
					+ sourceDirName);
			}

			DirectoryInfo[] dirs = dir.GetDirectories();
			// If the destination directory doesn't exist, create it.
			if (!Directory.Exists(destDirName))
			{
				Directory.CreateDirectory(destDirName);
			}

			// Get the files in the directory and copy them to the new location.
			FileInfo[] files = dir.GetFiles();
			foreach (FileInfo file in files)
			{
				string temppath = Path.Combine(destDirName, file.Name);
				file.CopyTo(temppath, false);
			}

			// If copying subdirectories, copy them and their contents to new location.
			if (copySubDirs)
			{
				foreach (DirectoryInfo subdir in dirs)
				{
					string temppath = Path.Combine(destDirName, subdir.Name);
					DirectoryCopy(subdir.FullName, temppath, copySubDirs);
				}
			}
		}
	}
}
