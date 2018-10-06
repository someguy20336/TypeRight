using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Tasks;
using Microsoft.Build.Utilities;

namespace TypeRight.Build.ReferenceResolvers
{
	/// <summary>
	/// A reference resolver that uses ResolveAssemblyReference from MSBuild to resolve references
	/// </summary>
	class NetFrameworkReferenceResolver : ReferenceResolver
	{
		/// <summary>
		/// The parameter provider for this reference resolver
		/// </summary>
		private ReferenceResolverParameters _paramProvider;

		/// <summary>
		/// Creates a new empty reference resolver
		/// </summary>
		public NetFrameworkReferenceResolver(ReferenceResolverParameters paramProvider)
		{
			_paramProvider = paramProvider;
		}
		
		/// <summary>
		/// Resolves the given references
		/// </summary>
		/// <param name="proj">The Project to resolve external references for</param>
		/// <returns>The resolved references</returns>
		public override IEnumerable<ResolvedReference> ResolveReferences(Project proj)
		{
			IEnumerable<ProjectItem> references = proj.Items.Where(itm => itm.ItemType == "Reference");
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
