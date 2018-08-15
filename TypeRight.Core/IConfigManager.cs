using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRight
{
	/// <summary>
	/// A class that manages the configuration file
	/// </summary>
	public interface IConfigManager
	{
		/// <summary>
		/// Gets the config filepath for the given project path
		/// </summary>
		/// <param name="projPath">The project path</param>
		/// <returns>The path to the config file</returns>
		string GetConfigFilepath(string projPath);

		/// <summary>
		/// Gets the config options for the project
		/// </summary>
		/// <param name="projPath">The project path</param>
		/// <returns>The config options</returns>
		IConfigOptions GetForProject(string projPath);

		/// <summary>
		/// Creates a new config options
		/// </summary>
		/// <returns></returns>
		IConfigOptions CreateNew();

		/// <summary>
		/// Saves the config options
		/// </summary>
		/// <param name="options">The options to save</param>
		/// <param name="toPath">The path to save them to</param>
		void Save(IConfigOptions options, string toPath);

	}
}
