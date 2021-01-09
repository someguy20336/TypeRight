using Newtonsoft.Json;
using System.IO;
using System.Xml.Serialization;

namespace TypeRight.Configuration
{
	/// <summary>
	/// Parser used for config files
	/// </summary>
	public static class ConfigParser
	{
		/// <summary>
		/// The name of the config file
		/// </summary>
		public const string ConfigFileName = "typeRightConfig.json";

		/// <summary>
		/// Gets the filepath for a given project
		/// </summary>
		/// <param name="projPath">The project path</param>
		/// <returns>Returns the config file for that project</returns>
		public static ConfigOptions GetForProject(string projPath)
		{
			ConfigOptions config = ParseFromFile(GetConfigFilepath(projPath));
			return config;
		}

		/// <summary>
		/// Parses a config file from the given filepath
		/// </summary>
		/// <param name="filePath">The config filepath</param>
		/// <returns>The config file object, or null if it doesn't exist</returns>
		public static ConfigOptions ParseFromFile(string filePath)
		{
			FileInfo file = new FileInfo(filePath);

			if (!file.Exists)
			{
				return null;
			}
			else
			{
				return JsonConvert.DeserializeObject<ConfigOptions>(File.ReadAllText(filePath));
			}
		}

		/// <summary>
		/// Saves the config file
		/// </summary>
		/// <param name="config">The config file to save</param>
		/// <param name="configPath">The path of the config file</param>
		public static void Save(ConfigOptions config, string configPath)
		{
			File.WriteAllText(configPath, JsonConvert.SerializeObject(config, Formatting.Indented));
		}

		/// <summary>
		/// Gets the path of the config file for the project at the given path
		/// </summary>
		/// <param name="projPath">The project path</param>
		/// <returns>The config file path</returns>
		public static string GetConfigFilepath(string projPath)
		{
			FileInfo projFile = new FileInfo(projPath);
			return Path.Combine(projFile.DirectoryName, ConfigFileName);
		}
	}
}
