using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host.Mef;
using TypeRight.Build.WorkspaceBuilders;
using MsBuild = Microsoft.Build.Evaluation;
using CodeAnalysis = Microsoft.CodeAnalysis;

namespace TypeRight.Build
{
	/// <summary>
	/// Mimics MSBuild and creates a workspace for a given project.  Should only be used
	/// if a workspace from an external source is not provided (i.e. from visual studio)
	/// </summary>
	public abstract class WorkspaceBuilder
	{
		
		/// <summary>
		/// Gets the project path for this builder
		/// </summary>
		public string ProjectPath { get; private set; }

		/// <summary>
		/// Creates a new solution workspace parser for the given solution
		/// </summary>
		/// <param name="projPath">The path to the project</param>
		public WorkspaceBuilder(string projPath)
		{
			ProjectPath = projPath;
		}

		/// <summary>
		/// Builds the workspace
		/// </summary>
		/// <returns></returns>
		public Workspace BuildWorkspace()
		{
			AdhocWorkspace adhocWorkspace;

			using (ProjectCollection collection = CreateProjectCollection())
			{
				adhocWorkspace = new AdhocWorkspace(DesktopMefHostServices.DefaultServices);
				SolutionId id = SolutionId.CreateNewId();
				SolutionInfo solInfo = SolutionInfo.Create(id, VersionStamp.Create());
				adhocWorkspace.AddSolution(solInfo);

				IEnumerable<ProjectInfo> projInfos = GetProjectInfos(collection);
				adhocWorkspace.AddProjects(projInfos);
			}

			return adhocWorkspace;
		}

		/// <summary>
		/// Parses the project file for the project collection
		/// </summary>
		private ProjectCollection CreateProjectCollection()
		{
			HashSet<string> processedPaths = new HashSet<string>();
			ProjectCollection collection = new ProjectCollection();
			MsBuild.Project proj = collection.LoadProject(ProjectPath);
			AddProjReferences(proj, collection, processedPaths);
			return collection;
		}

		/// <summary>
		/// Adds the project and all project refs  to the collection
		/// </summary>
		/// <param name="proj">The project</param>
		/// <param name="collection">The project collection</param>
		/// <param name="processedProjects">The set of processed paths</param>
		private void AddProjReferences(MsBuild.Project proj, ProjectCollection collection,  HashSet<string> processedProjects)
		{
			// Add the current project path to the dictionary
			string filePath = proj.FullPath;
			if (processedProjects.Contains(filePath))
			{
				return;
			}
			processedProjects.Add(filePath);

			// Find all other project references and add them
			foreach (ProjectItem projRefItem in proj.Items.Where(itm => itm.ItemType == "ProjectReference"))
			{
				string projPath = Path.GetFullPath(Path.Combine(proj.DirectoryPath, projRefItem.UnevaluatedInclude));
				MsBuild.Project refProj = collection.LoadProject(projPath);
				AddProjReferences(refProj, collection, processedProjects);
			}
		}

		/// <summary>
		/// Gets the project info for all attached projects
		/// </summary>
		/// <returns>The project infos</returns>
		private IEnumerable<ProjectInfo> GetProjectInfos(ProjectCollection collection)
		{
			Dictionary<string, ProjectInfo> pathToCAProject = new Dictionary<string, ProjectInfo>();

			foreach (MsBuild.Project msBuildProj in collection.LoadedProjects)
			{
				ProjectInfo projectInfo = CreateProjectInfo(msBuildProj);
				projectInfo = projectInfo.WithFilePath(msBuildProj.FullPath);

				// Add documentation parse option so we get XML
				projectInfo = projectInfo.WithParseOptions(projectInfo.ParseOptions.WithDocumentationMode(DocumentationMode.Parse));

				pathToCAProject.Add(msBuildProj.FullPath, projectInfo);

			}

			// Add project references as necessary
			foreach (MsBuild.Project msBuildProj in collection.LoadedProjects)
			{
				IEnumerable<ProjectItem> projReferences = msBuildProj.Items.Where(itm => itm.ItemType == "ProjectReference");

				List<ProjectReference> projRefs = new List<ProjectReference>();
				foreach (ProjectItem item in projReferences)
				{
					//MsBuild.ProjectMetadata metadata = item.Metadata.Where(md => md.Name == "Project").FirstOrDefault();
					string projPath = item.EvaluatedInclude;
					projPath = Path.GetFullPath(Path.Combine(msBuildProj.DirectoryPath, projPath));
					ProjectInfo caProj = pathToCAProject[projPath];
					ProjectReference projRef = new ProjectReference(caProj.Id);
					projRefs.Add(projRef);
				}
				pathToCAProject[msBuildProj.FullPath] = pathToCAProject[msBuildProj.FullPath].WithProjectReferences(projRefs);
			}

			return pathToCAProject.Values;
		}

		/// <summary>
		/// Creates the project info from the msBuild project
		/// </summary>
		/// <param name="msBuildProj">The ms Build project</param>
		/// <returns>The project info</returns>
		protected abstract ProjectInfo CreateProjectInfo(MsBuild.Project msBuildProj);

		/// <summary>
		/// Creates a builder for the given project
		/// </summary>
		/// <param name="projectPath">The path to the project</param>
		/// <param name="parameters">The ref resovler params</param>
		/// <returns>The workspace builder</returns>
		public static WorkspaceBuilder CreateBuilder(string projectPath, ReferenceResolverParameters parameters)
		{
			if (parameters.TargetFrameworkMoniker.ToUpper().StartsWith(".NETCOREAPP"))
			{
				return new NetCoreWorkspaceBuilder(projectPath);
			}
			return new NetFrameworkWorkspaceBuilder(projectPath, ReferenceResolver.CreateResolver(parameters));
		}
	}
}
