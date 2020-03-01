using TypeRight;
using Microsoft.CodeAnalysis;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;

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
		public string AssemblyPath { get; }

		/// <summary>
		/// Gets or sets the engine provider
		/// </summary>
		[Import(typeof(IScriptGenEngineProvider<Workspace>))]
		public IScriptGenEngineProvider<Workspace> EngineProvider { get; set; }

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
			AssemblyVersion = version;
			AssemblyPath = Path.Combine(nugetPath, "tools\bridge");
			string cachePath = Path.Combine(cacheBasePath, AssemblyVersion);

			// If we don't have this version locally cached, do that now
#if DEBUG
			if (Directory.Exists(cachePath))
			{
				Directory.Delete(cachePath);
			}
			// Use the test build
			Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
			string solnDir = new FileInfo(Shared.VsHelper.Current.Dte.Solution.FullName).Directory.FullName;
			string relativeBuildDir = @"..\..\TypeRight.Workspaces.Bridge\bin\Debug\";
			AssemblyPath = Path.GetFullPath(Path.Combine(solnDir, relativeBuildDir));
#endif
			if (!Directory.Exists(cachePath))
			{
				Directory.CreateDirectory(cachePath);
				DirectoryCopy(AssemblyPath, cachePath, true);

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
					// TODO - useful error message here?
					throw;
				}
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
