using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Tasks;
using Microsoft.Build.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace TypeRight.Build
{
	/// <summary>
	/// A reference resolver that uses ResolveAssemblyReference from MSBuild to resolve references
	/// </summary>
	public class ReferenceResolver
	{
		/// <summary>
		/// The parameter provider for this reference resolver
		/// </summary>
		private ReferenceResolverParameters _paramProvider;

		/// <summary>
		/// Creates a new empty reference resolver
		/// </summary>
		public ReferenceResolver(ReferenceResolverParameters paramProvider)
		{
			_paramProvider = paramProvider;
		}

		/// <summary>
		/// Resolves the given references using the ResolveAssemblyReference task
		/// </summary>
		/// <param name="references">The references to resolve</param>
		/// <returns>The resolved references</returns>
		public IEnumerable<ResolvedReference> ResolveReferences(IEnumerable<ProjectItem> references)
		{
			if (references.Count() == 0)
			{
				return new List<ResolvedReference>();
			}

			// Create task items from references
			List<ITaskItem> taskItems = new List<ITaskItem>();
			foreach (ProjectItem item in references)
			{
				taskItems.Add(new ReferenceTaskItem(item));
			}
			taskItems.Add(new TaskItem("System.Core"));
			taskItems.Add(new TaskItem("System.Runtime"));

			// Use resolve assembly reference and add the task items
			ResolveAssemblyReference resolver = new ResolveAssemblyReference
			{
				Assemblies = taskItems.ToArray(),

				// These props come from the inputs
				TargetFrameworkDirectories = _paramProvider.TargetFrameworkDirectories,
				SearchPaths = _paramProvider.SearchPaths,
				AllowedAssemblyExtensions = _paramProvider.AllowedAssemblyExtensions,
				TargetProcessorArchitecture = _paramProvider.TargetProcessorArchitecture,
				TargetFrameworkMoniker = _paramProvider.TargetFrameworkMoniker,
				TargetedRuntimeVersion = _paramProvider.TargetedRuntimeVersion,
				TargetFrameworkVersion = _paramProvider.TargetFrameworkVersion,
				AllowedRelatedFileExtensions = _paramProvider.AllowedRelatedFileExtensions,
				FullTargetFrameworkSubsetNames = _paramProvider.FullTargetFrameworkSubsetNames,
				FullFrameworkFolders = _paramProvider.FullFrameworkFolders,

				// these are just defaulted (which i guess i could add as an input, but i don't really know what they do)
				AutoUnify = true,
				FindSatellites = true,
				BuildEngine = new DummyBuildEngine(references.First().Project)
			};

			resolver.Execute();

			return resolver.ResolvedFiles
				.Select(taskItem => new ResolvedReference(
					taskItem.ItemSpec, 
					taskItem.GetMetadata("EmbedInteropTypes").ToUpper() == "TRUE")
					)
				.Union(resolver.ResolvedDependencyFiles.Select(taskItem => new ResolvedReference(
					taskItem.ItemSpec,
					taskItem.GetMetadata("EmbedInteropTypes").ToUpper() == "TRUE")
					));
		}
	}
}
