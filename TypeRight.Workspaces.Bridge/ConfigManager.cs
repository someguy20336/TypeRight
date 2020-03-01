using System.ComponentModel.Composition;
using TypeRight.Configuration;

namespace TypeRight.Workspaces.Bridge
{
	/// <summary>
	/// Configuration manager for the script generator
	/// </summary>
	[Export(typeof(IConfigManager))]
	public class ConfigManager : IConfigManager
	{
		/// <summary>
		/// Creates a new Config options object
		/// </summary>
		/// <returns>Configuration options</returns>
		public IConfigOptions CreateNew()
		{
			return new ConfigOptions();
		}

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
		public IConfigOptions GetForProject(string projPath)
		{
			return ConfigParser.GetForProject(projPath);
		}

		/// <summary>
		/// Saves the config options
		/// </summary>
		/// <param name="options">The config options to save</param>
		/// <param name="toPath">The path to save to</param>
		public void Save(IConfigOptions options, string toPath)
		{
			ConfigOptions configOpts = options as ConfigOptions;
			ConfigParser.Save(configOpts, toPath);
		}
	}
}
