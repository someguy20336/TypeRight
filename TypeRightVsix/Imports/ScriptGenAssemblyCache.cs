using TypeRightVsix.Shared;
using EnvDTE;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using NuGet.VisualStudio;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		private static ConcurrentDictionary<string, ImportedGenerator> s_imports = new ConcurrentDictionary<string, ImportedGenerator>();

		/// <summary>
		/// Gets the generator for the given project
		/// </summary>
		/// <param name="proj">The project</param>
		/// <returns>the associated import generator</returns>
		public static ImportedGenerator GetForProj(Project proj)
		{
#if DEBUG
			return s_imports.GetOrAdd("Debug", (dontcare) =>
			{
				return new ImportedGenerator(CacheBasePath, "Debug", "");
			});
#else
			IVsPackageMetadata pkg = GetInstalledPackageMetadata(proj);
			if (pkg == null)
			{
				return null;
			}
			return s_imports.GetOrAdd(pkg.VersionString, (dontcare) =>
			{
				return new ImportedGenerator(CacheBasePath, pkg.VersionString, pkg.InstallPath);
			});
#endif
		}

		/// <summary>
		/// Gets the package metatdata installed for the given project
		/// </summary>
		/// <param name="proj"></param>
		/// <returns></returns>
		private static IVsPackageMetadata GetInstalledPackageMetadata(Project proj)
		{
			IComponentModel componentModel = (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));
			IVsPackageInstallerServices installerServices = componentModel.GetService<IVsPackageInstallerServices>();
			IEnumerable<IVsPackageMetadata> packages = installerServices.GetInstalledPackages(proj);
			return packages.Where(pkg => pkg.Id == TypeRightPackage.NugetID).FirstOrDefault();
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
