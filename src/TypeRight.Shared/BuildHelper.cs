using System;
using System.IO;

namespace TypeRight
{
	/// <summary>
	/// Helper used for builds
	/// </summary>
	internal class BuildHelper
	{

		/// <summary>
		/// Signifies the start of a build for a project
		/// </summary>
		/// <param name="projPath">The project path</param>
		public static void StartBuild(string projPath)
		{
			try
			{
				File.WriteAllText(GetTempFilePath(projPath), "lock");
			}
			catch (Exception)
			{
				// who cares
			}
		}

		/// <summary>
		/// Signifies the end of a build
		/// </summary>
		/// <param name="projPath">The project path</param>
		public static void EndBuild(string projPath)
		{
			string buildPath = GetTempFilePath(projPath);
			if (File.Exists(buildPath))
			{
				try
				{
					File.Delete(buildPath);
				}
				catch (Exception)
				{
					// also who cares
				}
			}

		}

		/// <summary>
		/// Gets whether the project path needs the scripts generated
		/// </summary>
		/// <param name="projPath">The project path</param>
		/// <returns>True if it needs generation</returns>
		public static bool NeedsScriptGeneration(string projPath)
		{
			return !File.Exists(GetTempFilePath(projPath));
		}

		private static string GetTempFilePath(string projFile)
		{
			FileInfo fileInfo = new FileInfo(projFile);
			string dir = fileInfo.DirectoryName;
			return Path.Combine(dir, "obj", ".scriptGenLock");
		}
	}
}
