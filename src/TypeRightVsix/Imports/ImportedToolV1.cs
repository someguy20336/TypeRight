using Microsoft.CodeAnalysis;
using TypeRight.VsixContract.Messages;
using TypeRight.VsixContract;

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
			var result = _scriptGenerationAdapter.GenerateScripts(workspace, projPath, force);
			return new GenerateScriptsResponse(result.Success, result.ErrorMessage);
		}

		public override string GetConfigFilepath(string projPath)
		{
			return _config.GetConfigFilepath(projPath);
		}

		public override bool IsEnabledForProject(string projPath)
		{
			return _config.IsEnabled(projPath);
		}
	}
}
