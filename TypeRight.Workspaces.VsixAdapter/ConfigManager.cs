using System.ComponentModel.Composition;
using TypeRight.Configuration;
using TypeRight.VsixContract;

namespace TypeRight.Workspaces.VsixAdapter
{
	/// <summary>
	/// Configuration manager for the script generator
	/// </summary>
	[Export(typeof(IConfigManager))]
	public class ConfigManager : IConfigManager
	{

		/// <summary>
		/// Gets the configuration filepath
		/// </summary>
		/// <param name="projPath">The project path</param>
		/// <returns>The config filepath</returns>
		public string GetConfigFilepath(string projPath)
		{
			return ConfigParser.GetConfigFilepath(projPath);
		}

		/// <summary>
		/// Gets the config options for the project
		/// </summary>
		/// <param name="projPath">the project path</param>
		/// <returns>The configuration options</returns>
		public bool IsEnabled(string projPath)
		{
			var config = ConfigParser.GetForProject(projPath);
			return config?.Enabled ?? false;
		}

		/// <summary>
		/// Saves the config options
		/// </summary>
		/// <param name="toPath">The path to save to</param>
		public void CreateNew(string toPath)
		{
			ConfigOptions configOpts = new ConfigOptions()
			{
				Enabled = true
			};
			ConfigParser.Save(configOpts, toPath);
		}
	}
}
