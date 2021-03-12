using EnvDTE;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace TypeRightVsix.Imports
{
	/// <summary>
	/// A cache of all the script generators
	/// </summary>
	class ScriptGenAssemblyCache
	{
		/// <summary>
		/// Stores the base path for version caches
		/// </summary>
		public static readonly string CacheBasePath = Path.Combine(new FileInfo(typeof(ScriptGenAssemblyCache).Assembly.Location).DirectoryName, "VersCache");
		
		/// <summary>
		/// A dictionary of the package versions to the associated generator for that version
		/// </summary>
		private static ConcurrentDictionary<string, ImportedToolBase> s_imports = new ConcurrentDictionary<string, ImportedToolBase>();

		/// <summary>
		/// Gets the generator for the given project
		/// </summary>
		/// <param name="proj">The project</param>
		/// <returns>the associated import generator</returns>
		public static ImportedToolBase GetForProj(Project proj)
		{
			var importer = GetImporter(proj);
			if (!importer.ShouldTryImport())
			{
				return null;		// Or null importer?
			}
			return s_imports.GetOrAdd(importer.Version, (dontcare) =>
			{
				return importer.TryImport();
			});
		}

		public static ImportedToolBase LoadFromDirectory(string vers, string dir)
		{
			var importer = new SpecifiedDirectoryImporter(vers, dir, CacheBasePath);
			if (!importer.ShouldTryImport())
			{
				return null;        // Or null importer?
			}
			return s_imports.GetOrAdd(importer.Version, (dontcare) =>
			{
				return importer.TryImport();
			});
		}

		public static IEnumerable<ImportedToolBase> GetAllLoaded() => s_imports.Values;
		public static ImportedToolBase GetVersion(string version)
		{
			s_imports.TryGetValue(version, out var val);
			return val;
		}

		private static ToolImporter GetImporter(Project proj)
		{
#if DEBUG && !NUGET
			return new DebugDirectoryImporter("Debug", CacheBasePath);
#else
			return new NugetPackageToolImporter(proj, CacheBasePath);
#endif
		}

		/// <summary>
		/// Marks the cache for clearning next time VS restarts
		/// </summary>
		public static void MarkForClear()
		{
			string path = Path.Combine(CacheBasePath, "ClearAll.txt");
			if (!File.Exists(path))
			{
				try
				{
					File.Create(path).Dispose();  // just create the file
				}
				finally
				{
					// Who cares, maybe
				}
			}
		}

		/// <summary>
		/// Tries to clear the cache if marked for clearing.
		/// </summary>
		/// <returns>True if successful</returns>
		public static bool TryClearCache()
		{
			string path = Path.Combine(CacheBasePath, "ClearAll.txt");
			if (File.Exists(path))
			{
				return ClearCache();
			}
			else
			{
				return true;
			}

		}

		/// <summary>
		/// Clears the cache.
		/// </summary>
		/// <returns>True if successful</returns>
		public static bool ClearCache()
		{
			if (!Directory.Exists(CacheBasePath))
			{
				return true;
			}
			try
			{
				Directory.Delete(CacheBasePath, true);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
