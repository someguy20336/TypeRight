using Microsoft.CodeAnalysis;
using TypeRight.VsixContract;

namespace TypeRightVsix.Imports
{
	internal class LegacyScriptGenerationAdapter : IScriptGenerationAdapter
	{
		private dynamic _legacyGenerator;

		public LegacyScriptGenerationAdapter(dynamic legacyGenerator)
		{
			_legacyGenerator = legacyGenerator;
		}

		public IScriptGenerationResult GenerateScripts(Workspace workspace, string projPath, bool force)
		{
			var engine = _legacyGenerator.GetEngine(workspace, projPath);
			var result = engine.GenerateScripts();
			return new ScriptGenerationResultAdapter()
			{
				Success = result.Sucess,		// Typo... whoops
				ErrorMessage = result.ErrorMessage
			};
		}
	}

	internal class LegacyConfigManagerAdapter : IConfigManager
	{
		private dynamic _legacyConfig;

		public LegacyConfigManagerAdapter(dynamic legacyConfig)
		{
			_legacyConfig = legacyConfig;
		}

		public void CreateNew(string toPath)
		{
			var newConfig = _legacyConfig.CreateNew();
			_legacyConfig.Save(newConfig, toPath);
		}

		public string GetConfigFilepath(string projPath)
		{
			return _legacyConfig.GetConfigFilepath(projPath);
		}

		public bool IsEnabled(string projPath)
		{
			var options = _legacyConfig.GetForProject(projPath);
			return options?.Enabled ?? false;
		}
	}
}
