using Microsoft.CodeAnalysis;
using System.IO;
using TypeRight.VsixContract.Messages;

namespace TypeRightVsix.Imports
{
	internal class NullImportedTool : ImportedToolBase
	{
		public NullImportedTool(string cachePath, string version, string assemDir) 
			: base(cachePath, version, assemDir)
		{
		}

		public override void CreateNewConfigFile(string configPath)
		{
		}

		public override GenerateScriptsResponse GenerateScripts(Workspace workspace, string projPath, bool force)
		{
			return new GenerateScriptsResponse(
				false,
				"Unable to find compatible generator - you may need to update your Nuget package"
				);
		}

		public override string GetConfigFilepath(string projPath)
		{
			// Assume this is the path if we don't have a manager - just to be safe
			FileInfo projFile = new FileInfo(projPath);
			return Path.Combine(projFile.DirectoryName, "typeRightConfig.json");
		}

		public override bool IsEnabledForProject(string projPath) => false;
	}



}
