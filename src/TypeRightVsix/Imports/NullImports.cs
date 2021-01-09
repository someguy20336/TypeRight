using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.VsixContract;

namespace TypeRightVsix.Imports
{
	public class NullScriptGenerationAdapter : IScriptGenerationAdapter
	{
		public IScriptGenerationResult GenerateScripts(Workspace workspace, string projPath, bool force)
		{
			return new ScriptGenerationResultAdapter()
			{
				ErrorMessage = "Unable to find compatible generator - you may need to update your Nuget package"
			};
		}
	}

	public class ScriptGenerationResultAdapter : IScriptGenerationResult
	{
		public bool Success { get; set; }

		public string ErrorMessage { get; set; }

	}

	public class NullConfigManager : IConfigManager
	{
		public void CreateNew(string toPath)
		{
		}

		public string GetConfigFilepath(string projPath)
		{
			// Assume this is the path if we don't have a manager - just to be safe
			FileInfo projFile = new FileInfo(projPath);
			return Path.Combine(projFile.DirectoryName, "typeRightConfig.json");
		}

		public bool IsEnabled(string projPath) => false;
	}
}
