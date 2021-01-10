using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRight.VsixContract
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
		/// Creates a new config options
		/// </summary>
		/// <returns></returns>
		void CreateNew(string toPath);

		/// <summary>
		/// Gets whether script generation is enabled for the given project
		/// </summary>
		/// <param name="projPath"></param>
		/// <returns></returns>
		bool IsEnabled(string projPath);

	}
}
