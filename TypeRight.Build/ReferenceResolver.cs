using Microsoft.Build.Evaluation;
using System.Collections.Generic;
using TypeRight.Build.ReferenceResolvers;

namespace TypeRight.Build
{
	/// <summary>
	/// A class that resolves external (non-project) references for a project
	/// </summary>
	public abstract class ReferenceResolver
	{
		
		/// <summary>
		/// Resolves the given references
		/// </summary>
		/// <param name="proj">The Project to resolve external references for</param>
		/// <returns>The resolved references</returns>
		public abstract IEnumerable<ResolvedReference> ResolveReferences(Project proj);

		public static ReferenceResolver CreateResolver(ReferenceResolverParameters parameters)
		{   
			return new NetFrameworkReferenceResolver(parameters);
		}
	}
}
