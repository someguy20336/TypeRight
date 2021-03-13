using Microsoft.CodeAnalysis;
using TypeRight.VsixContract.Messages;
using TypeRight.VsixContractV2;

namespace TypeRightVsix.Imports
{
	internal class ImportedToolV2 : ImportedToolBase
	{
		private readonly IMessageRouter _messageRouter;

		public ImportedToolV2(IMessageRouter router, string cachePath, string version, string assemDir) 
			: base(cachePath, version, assemDir)
		{
			_messageRouter = router;
		}

		public override GenerateScriptsResponse GenerateScripts(Workspace workspace, string projPath, bool force)
		{
			GenerateScriptsRequest message = new GenerateScriptsRequest(workspace, projPath, force);
			var result = _messageRouter.Send(message);
			return GenerateScriptsResponse.Read(result);
		}

		public override string GetConfigFilepath(string projPath)
		{
			GetConfigFilePathRequest message = new GetConfigFilePathRequest(projPath);
			var result = _messageRouter.Send(message);
			return GetConfigFilePathResponse.Read(result).FilePath;
		}

		public override void CreateNewConfigFile(string configPath)
		{
			AddNewConfigFileRequest message = new AddNewConfigFileRequest(configPath);
			_messageRouter.Send(message);
			// return AddNewConfigFileResponse.Read(result);
		}

		public override bool IsEnabledForProject(string projPath)
		{
			IsEnabledForProjectRequest message = new IsEnabledForProjectRequest(projPath);
			var result = _messageRouter.Send(message);
			return IsEnabledForProjectResponse.Read(result).IsEnabled;
		}

		public override bool CanUpgradeConfig(string configPath)
		{
			if (!_messageRouter.SupportsMessageType(UpgradeConfigRequest.MessageTypeValue))
			{
				return false;
			}

			CanUpgradeConfigRequest message = new CanUpgradeConfigRequest(configPath);
			var result = _messageRouter.Send(message);
			return CanUpgradeConfigResponse.Read(result).CanUpgrade;
		}

		public override void UpgradeConfig(string configPath)
		{
			UpgradeConfigRequest message = new UpgradeConfigRequest(configPath);
			_messageRouter.Send(message);
		}
	}
}
