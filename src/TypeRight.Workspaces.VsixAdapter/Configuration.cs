using TypeRight.Configuration;
using TypeRight.VsixContract.Messages;
using TypeRight.VsixContractV2;

namespace TypeRight.Workspaces.VsixAdapter
{
	/// <summary>
	/// Configuration manager for the script generator
	/// </summary>
	public static class Configuration
	{
		public static IResponse GetConfigFilepath(IRequest request)
		{
			GetConfigFilePathRequest message = GetConfigFilePathRequest.Read(request);
			string path = ConfigParser.GetConfigFilepath(message.ProjectPath);
			return new GetConfigFilePathResponse(path);
		}

		public static IResponse IsEnabled(IRequest request)
		{
			IsEnabledForProjectRequest message = IsEnabledForProjectRequest.Read(request);
			var config = ConfigParser.GetForProject(message.ProjectPath);
			return new IsEnabledForProjectResponse(config?.Enabled ?? false);
		}

		/// <summary>
		/// Saves the config options
		/// </summary>
		/// <param name="toPath">The path to save to</param>
		public static IResponse CreateNew(IRequest request)
		{
			AddNewConfigFileRequest message = AddNewConfigFileRequest.Read(request);
			ConfigOptions configOpts = new ConfigOptions()
			{
				Enabled = true
			};
			ConfigParser.Save(configOpts, message.ConfigPath);
			return new AddNewConfigFileResponse();
		}

		public static IResponse CanUpgradeConfig(IRequest request)
		{
			CanUpgradeConfigRequest message = CanUpgradeConfigRequest.Read(request);
			var config = ConfigParser.ParseFromFile(message.ConfigPath);
			var currentVers = new ConfigOptions();

			return new CanUpgradeConfigResponse(config.Schema != currentVers.Schema);
		}

		public static IResponse UpgradeConfig(IRequest request)
		{
			UpgradeConfigRequest message = UpgradeConfigRequest.Read(request);
			var config = ConfigParser.ParseFromFile(message.ConfigPath);
			var currentVers = new ConfigOptions();
			config.Schema = currentVers.Schema;
			ConfigParser.Save(config, message.ConfigPath);

			return new UpgradeConfigResponse();
		}
	}
}
