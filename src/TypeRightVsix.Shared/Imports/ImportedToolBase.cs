using Microsoft.CodeAnalysis;
using TypeRight.VsixContract.Messages;

namespace TypeRightVsix.Imports
{
	/// <summary>
	/// A generator that was imported for a specific version
	/// </summary>
	internal abstract class ImportedToolBase
	{
		public string Name => GetType().Name;

		/// <summary>
		/// Gets the assembly version for this generator
		/// </summary>
		public string Version { get; }

		/// <summary>
		/// Gets the assembly path for this version
		/// </summary>
		public string AssemblyDirectory { get; }

		/// <summary>
		/// Gets the cache Path for this generator
		/// </summary>
		public string CachePath { get; }
				
		public ImportedToolBase(string cachePath, string version, string assemDir)
		{
			Version = version;
			CachePath = cachePath;
			AssemblyDirectory = assemDir;
		}

		public abstract GenerateScriptsResponse GenerateScripts(Workspace workspace, string projPath, bool force);

		public abstract string GetConfigFilepath(string projPath);

		public abstract void CreateNewConfigFile(string configPath);

		public abstract bool IsEnabledForProject(string projPath);

		public abstract bool CanUpgradeConfig(string configPath);

		public abstract void UpgradeConfig(string configPath);
	}
}
