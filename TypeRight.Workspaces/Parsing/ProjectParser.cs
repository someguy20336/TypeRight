using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System;
using System.Linq;
using TypeRight.TypeLocation;

namespace TypeRight.Workspaces.Parsing
{
	/// <summary>
	/// Parses a solution for script classes and enums
	/// </summary>
	public class ProjectParser : ITypeIterator
	{
		private ITypeVisitor _visitor;

		/// <summary>
		/// The current workspace
		/// </summary>
		private Workspace _workspace;
		
		/// <summary>
		/// The path of the project currently being processed
		/// </summary>
		private readonly ProjectId _projectId;
				
		/// <summary>
		/// Creates a new solution parser
		/// </summary>
		/// <param name="workspace">The solution workspace</param>
		/// <param name="projId">The project ID we are parsing</param>
		public ProjectParser(Workspace workspace, ProjectId projId)
		{
			_workspace = workspace;
			_projectId = projId;
		}

		/// <summary>
		/// Iterates the types in the project
		/// </summary>
		/// <param name="visitor">The visitor for the iterator</param>
		public void IterateTypes(ITypeVisitor visitor)
		{
			_visitor = visitor;
			Solution sol = _workspace.CurrentSolution;
			ProjectDependencyGraph graph = sol.GetProjectDependencyGraph();

			// Typescript projects show up with the same project path, so make sure it supports compilation
			Project mainProj = sol.Projects.Where(proj => proj.Id.Equals(_projectId) && proj.SupportsCompilation).FirstOrDefault();
			if (mainProj == null)
			{
				throw new InvalidOperationException($"Project with path {_projectId} does not exist.");
			}

			// Get a list of all classes being extracted
			Dictionary<string, Compilation> compilations = new Dictionary<string, Compilation>();
			AddCompiledProjects(sol, new List<ProjectId> { mainProj.Id }, compilations);
			AddCompiledProjects(sol, graph.GetProjectsThatThisProjectDirectlyDependsOn(mainProj.Id), compilations);
			AddCompiledProjects(sol, graph.GetProjectsThatThisProjectTransitivelyDependsOn(mainProj.Id), compilations);

			foreach (Compilation comp in compilations.Values)
			{
				CompilationParser parser = new CompilationParser(comp);
				parser.IterateTypes(_visitor);
			}
		}
				
		/// <summary>
		/// Adds the list of project IDs to the compilation index
		/// </summary>
		/// <param name="sol">The solution</param>
		/// <param name="projIds">The project IDs to add</param>
		/// <param name="compIndex">The list of compiled projects</param>
		private void AddCompiledProjects(Solution sol, IEnumerable<ProjectId> projIds, Dictionary<string, Compilation> compIndex)
		{
			foreach (ProjectId projId in projIds)
			{
				if (compIndex.ContainsKey(projId.Id.ToString()))
				{
					continue;
				}

				Project proj = sol.GetProject(projId);

				if (!proj.SupportsCompilation)
				{
					continue;
				}

				compIndex.Add(projId.Id.ToString(), proj.GetCompilationAsync().Result);
			}
		}
	}
}
