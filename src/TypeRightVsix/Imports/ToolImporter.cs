using EnvDTE;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using NuGet.VisualStudio;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using TypeRight.VsixContract;
using TypeRight.VsixContractV2;
using TypeRightVsix.Shared;

namespace TypeRightVsix.Imports
{
	internal abstract class ToolImporter
	{

		protected string CacheBasePath { get; }
		protected string CacheDirectory => Path.Combine(CacheBasePath, Version);
		public abstract string Version { get; }


		public ToolImporter(string cacheBasePath)
		{
			CacheBasePath = cacheBasePath;
		}

		public abstract bool ShouldTryImport();


		public ImportedToolBase TryImport()
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			if (!ShouldTryImport())
			{
				return NotApplicable();
			}

			string directory = GetImportFromDirectory();
			if (!Directory.Exists(directory))
			{
				return ImportFailed(directory);
			}

			if (!Directory.Exists(CacheDirectory))
			{
				Directory.CreateDirectory(CacheDirectory);
				FileUtils.DirectoryCopy(directory, CacheDirectory, true);
			}

			// Import the files
			ImportedComponents components = new ImportedComponents();
			DirectoryCatalog catalog = new DirectoryCatalog(CacheDirectory, "TypeRight*.dll");
			using (CompositionContainer container = new CompositionContainer(catalog, true))
			{
				try
				{
					container.SatisfyImportsOnce(components);
				}
				catch (Exception)
				{
					return ImportFailed(directory);
				}
			}

			if (components.ScriptGenerator != null)
			{
				return new ImportedToolV1(
					components.ScriptGenerator,
					components.ConfigManager,
					CacheDirectory,
					Version,
					directory
					);
			}
			else if (components.MessageRouter != null)
			{
				return new ImportedToolV2(components.MessageRouter, CacheDirectory, Version, directory);
			}

			return ImportFailed(directory);

		}

		protected virtual void BeforeImport() { }

		protected abstract string GetImportFromDirectory();

		private NullImportedTool NotApplicable()
		{
			return new NullImportedTool(CacheDirectory, Version, "");
		}

		private NullImportedTool ImportFailed(string fromDir)
		{
			ShowFailedToLoadMessage();
			return new NullImportedTool(CacheDirectory, Version, fromDir);
		}

		private static void ShowFailedToLoadMessage()
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			VsHelper.SetStatusBar("Failed to load compatible version of TypeRight");
		}

		protected class ImportedComponents
		{
			[Import(typeof(IMessageRouter), AllowDefault = true)]
			public IMessageRouter MessageRouter { get; set; }

			[Import(typeof(IScriptGenerationAdapter), AllowDefault = true)]
			public IScriptGenerationAdapter ScriptGenerator { get; set; }

			[Import(typeof(IConfigManager), AllowDefault = true)]
			public IConfigManager ConfigManager { get; set; }
		}
	}

	internal class NugetPackageToolImporter : ToolImporter
	{

		private IVsPackageMetadata _pkg;

		public override string Version => _pkg?.VersionString ?? "None";

		public NugetPackageToolImporter(Project proj, string cacheBasePath)
			: base(cacheBasePath)
		{
			_pkg = GetInstalledPackageMetadata(proj);
		}


		protected override string GetImportFromDirectory() => Path.Combine(_pkg.InstallPath, "tools", "adapter");

		public override bool ShouldTryImport() => _pkg != null;

		/// <summary>
		/// Gets the package metatdata installed for the given project
		/// </summary>
		/// <param name="proj"></param>
		/// <returns></returns>
		private static IVsPackageMetadata GetInstalledPackageMetadata(Project proj)
		{
			IComponentModel componentModel = (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));
			IVsPackageInstallerServices installerServices = componentModel.GetService<IVsPackageInstallerServices>();
			IEnumerable<IVsPackageMetadata> packages = installerServices.GetInstalledPackages(proj);
			return packages.Where(pkg => pkg.Id == TypeRightPackage.NugetID).FirstOrDefault();
		}
	}

	internal class SpecifiedDirectoryImporter : ToolImporter
	{
		private readonly string _dir;

		public override string Version { get; }

		public SpecifiedDirectoryImporter(string vers, string dir, string cacheBasePath)
			: base(cacheBasePath)
		{
			Version = vers;
			_dir = dir;
		}
		protected override string GetImportFromDirectory() => _dir;

		public override bool ShouldTryImport() => true;
	}

	internal class DebugDirectoryImporter : SpecifiedDirectoryImporter
	{
		public DebugDirectoryImporter(string vers, string cacheBasePath) : base(vers, GetDebugDir(), cacheBasePath)
		{			
		}

		protected override void BeforeImport()
		{
			// Clear the cache directory
			if (Directory.Exists(CacheDirectory))
			{
				Directory.Delete(CacheDirectory, true);
			}
		}

		public static string GetDebugDir()
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			// Use the test build
			string solnDir = new FileInfo(VsHelper.Current.Dte.Solution.FullName).Directory.FullName;
			string relativeBuildDir = @"..\..\..\src\TypeRight.Workspaces.VsixAdapter\bin\Debug\net472";
			return Path.GetFullPath(Path.Combine(solnDir, relativeBuildDir));
		}
	}
}
