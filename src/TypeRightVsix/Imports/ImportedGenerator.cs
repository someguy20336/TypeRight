using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using TypeRight.VsixContract;
using TypeRightVsix.Shared;
using System.Reflection;
using TypeRight.VsixContractV2;
using Microsoft.CodeAnalysis;
using TypeRight.VsixContract.Messages;

namespace TypeRightVsix.Imports
{
	/// <summary>
	/// A generator that was imported for a specific version
	/// </summary>
	class ImportedGenerator
	{
		[Import(typeof(IMessageRouter), AllowDefault = true)]
		private IMessageRouter _messageRouter = null;

		[Import(typeof(IScriptGenerationAdapter), AllowDefault = true)]
		private IScriptGenerationAdapter _scriptGenerator;

		[Import(typeof(IConfigManager), AllowDefault = true)]
		private IConfigManager _configManager;

		/// <summary>
		/// Gets the assembly version for this generator
		/// </summary>
		public string AssemblyVersion { get; }

		/// <summary>
		/// Gets the assembly path for this version
		/// </summary>
		public string AssemblyDirectory { get; }

		/// <summary>
		/// Gets the cache Path for this generator
		/// </summary>
		public string CachePath { get; }
				
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
			string relativeBuildDir = @"..\..\..\src\TypeRight.Workspaces.VsixAdapter\bin\Debug\net472";
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
				ShowFailedToLoadMessage();
				return;
			}
					   
			// If we don't have this version locally cached, do that now
			CachePath = Path.Combine(cacheBasePath, AssemblyVersion);

#if DEBUG && !NUGET
			if (Directory.Exists(CachePath))
			{
				Directory.Delete(CachePath, true);
			}
#endif
			if (!Directory.Exists(CachePath))
			{
				Directory.CreateDirectory(CachePath);
				FileUtils.DirectoryCopy(AssemblyDirectory, CachePath, true);
			}

			// Import the files
			DirectoryCatalog catalog = new DirectoryCatalog(CachePath, "TypeRight*.dll");
			using (CompositionContainer container = new CompositionContainer(catalog, true))
			{
				try
				{
					container.SatisfyImportsOnce(this);
				}
				catch (Exception)
				{
					TryGetLegacy();
				}
			}

			TrySetNullImporters();
		}

		private void TrySetNullImporters()
		{
			_scriptGenerator = _scriptGenerator ?? new NullScriptGenerationAdapter();
			_configManager = _configManager ?? new NullConfigManager();
		}

		private void TryGetLegacy()
		{
			try
			{
				string genPath = Path.Combine(CachePath, "TypeRight.Workspaces.dll");
				Assembly assembly = Assembly.LoadFrom(genPath);
				Type type = assembly.GetType("TypeRight.Workspaces.Parsing.WorkspaceGenEngineProvider");
				dynamic legacyGenerator = Activator.CreateInstance(type);
				_scriptGenerator = new LegacyScriptGenerationAdapter(legacyGenerator);

				string configPath = Path.Combine(CachePath, "TypeRight.dll");
				assembly = Assembly.LoadFrom(configPath);
				type = assembly.GetType("TypeRight.Configuration.ConfigManager");
				dynamic legacyConfig = Activator.CreateInstance(type);
				_configManager = new LegacyConfigManagerAdapter(legacyConfig);
			}
			catch (Exception)
			{
				Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
				ShowFailedToLoadMessage();
			}
		}

		private void ShowFailedToLoadMessage()
		{
			Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
			VsHelper.SetStatusBar("Failed to load compatible version of TypeRight");
		}

		public GenerateScriptsResponse GenerateScripts(Workspace workspace, string projPath, bool force)
		{
			GenerateScriptsRequest message = new GenerateScriptsRequest(workspace, projPath, force);
			var result = _messageRouter.Send(message);
			return GenerateScriptsResponse.Read(result);
		}

		public string GetConfigFilepath(string projPath)
		{
			GetConfigFilePathRequest message = new GetConfigFilePathRequest(projPath);
			var result = _messageRouter.Send(message);
			return GetConfigFilePathResponse.Read(result).FilePath;
		}

		public void CreateNewConfigFile(string configPath)
		{
			AddNewConfigFileRequest message = new AddNewConfigFileRequest(configPath);
			_messageRouter.Send(message);
			// return AddNewConfigFileResponse.Read(result);
		}

		public bool IsEnabledForProject(string projPath)
		{
			IsEnabledForProjectRequest message = new IsEnabledForProjectRequest(projPath);
			var result = _messageRouter.Send(message);
			return IsEnabledForProjectResponse.Read(result).IsEnabled;
		}
	}
}
