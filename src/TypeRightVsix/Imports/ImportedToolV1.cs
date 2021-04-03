using Microsoft.CodeAnalysis;
using TypeRight.VsixContract.Messages;
using TypeRight.VsixContract;
using System;
using System.Linq;

namespace TypeRightVsix.Imports
{
	internal class ImportedToolV1 : ImportedToolBase
	{
		private readonly IScriptGenerationAdapter _scriptGenerationAdapter;
		private readonly IConfigManager _config;

		public ImportedToolV1(IScriptGenerationAdapter scriptGenerationAdapter, IConfigManager config, string cachePath, string version, string assemDir) 
			: base(cachePath, version, assemDir)
		{
			_scriptGenerationAdapter = scriptGenerationAdapter;
			_config = config;
		}


		public override void CreateNewConfigFile(string configPath)
		{
			_config.CreateNew(configPath);
		}

		public override GenerateScriptsResponse GenerateScripts(Workspace workspace, string projPath, bool force)
		{
			try
			{
				IScriptGenerationResult result = _scriptGenerationAdapter.GenerateScripts(workspace, projPath, force);
				return new GenerateScriptsResponse(result.Success, result.ErrorMessage);
			}
			catch (Exception e)
			{
				string[] topStackTrace = e.StackTrace.Split(new[] { "\r\n" }, StringSplitOptions.None).Take(8).ToArray();

				string message = e.Message + Environment.NewLine + Environment.NewLine
					+ string.Join(Environment.NewLine, topStackTrace) + Environment.NewLine + "...";
				return new GenerateScriptsResponse(false, message);
			}
		}

		public override string GetConfigFilepath(string projPath)
		{
			return _config.GetConfigFilepath(projPath);
		}

		public override bool IsEnabledForProject(string projPath)
		{
			return _config.IsEnabled(projPath);
		}

		public override bool CanUpgradeConfig(string configPath) => false;
		public override void UpgradeConfig(string configPath)
		{
			throw new System.NotImplementedException();
		}
	}
}
