using Microsoft.Build.Construction;
using Microsoft.Build.Tasks;
using Microsoft.CodeAnalysis.Host.Mef;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using MsBuild = Microsoft.Build.Evaluation;
using CodeAnalysis = Microsoft.CodeAnalysis;

namespace TypeRight.Build
{
	/// <summary>
	/// Mimics MSBuild and creates a workspace for a given project.  Should only be used
	/// if a workspace from an external source is not provided (i.e. from visual studio)
	/// </summary>
	public class WorkspaceBuilder : IDisposable
	{
		/// <summary>
		/// The ad hoc workspace
		/// </summary>
		private CodeAnalysis.AdhocWorkspace _adHocWorkspace;

		/// <summary>
		/// The object to use to resolve references
		/// </summary>
		private ReferenceResolver _refResolver;

		/// <summary>
		/// Gets the workspace used by this parser
		/// </summary>
		public CodeAnalysis.Workspace Workspace
		{
			get { return _adHocWorkspace; }
		}

		/// <summary>
		/// Creates a new solution workspace parser for the given solution
		/// </summary>
		/// <param name="projPath">The path to the project</param>
		/// <param name="refResolver">The reference resolver to use</param>
		public WorkspaceBuilder(string projPath, ReferenceResolver refResolver)
		{
			_refResolver = refResolver;
			_adHocWorkspace = new CodeAnalysis.AdhocWorkspace(DesktopMefHostServices.DefaultServices);
			CodeAnalysis.SolutionId id = CodeAnalysis.SolutionId.CreateNewId();
			CodeAnalysis.SolutionInfo solInfo = CodeAnalysis.SolutionInfo.Create(id, CodeAnalysis.VersionStamp.Create());
			_adHocWorkspace.AddSolution(solInfo);

			Dictionary<string, MsBuild.Project> buildProjIndex = ParseSolutionFile(projPath);

			IEnumerable<CodeAnalysis.ProjectInfo> caProjs = CreateProjects(buildProjIndex);

			_adHocWorkspace.AddProjects(caProjs);	
			
		}

		/// <summary>
		/// Parses the solution file for the project collection
		/// </summary>
		/// <param name="projPath">The solution file</param>
		/// <returns>An index of project GUIDs to the projects</returns>
		private Dictionary<string, MsBuild.Project> ParseSolutionFile(string projPath)
		{
			Dictionary<string, MsBuild.Project> guidToProject = new Dictionary<string, MsBuild.Project>();
			using (MsBuild.ProjectCollection projCollection = new MsBuild.ProjectCollection())
			{
				MsBuild.Project proj = projCollection.LoadProject(projPath);

				AddProjAndProjReferences(proj, projCollection, guidToProject);
			}

			return guidToProject;
		}

		/// <summary>
		/// Adds the project and all project refs  to the collection
		/// </summary>
		/// <param name="proj">The project</param>
		/// <param name="projCollection">The current project collection</param>
		/// <param name="guidToProject">The guid to project index</param>
		private void AddProjAndProjReferences(MsBuild.Project proj, MsBuild.ProjectCollection projCollection, Dictionary<string, MsBuild.Project> guidToProject)
		{
			// Add the current project GID to the dictionary
			string guid = proj.GetProperty("ProjectGuid").EvaluatedValue;
			if (guidToProject.ContainsKey(guid))
			{
				return;
			}
			guidToProject.Add(guid, proj);
			
			// Find all other project references and add them
			foreach (MsBuild.ProjectItem projRefItem in proj.Items.Where(itm => itm.ItemType == "ProjectReference"))
			{
				string projPath = Path.GetFullPath(Path.Combine(proj.DirectoryPath, projRefItem.UnevaluatedInclude));
				MsBuild.Project refProj = projCollection.LoadProject(projPath);
				AddProjAndProjReferences(refProj, projCollection, guidToProject);
			}
		}

		/// <summary>
		/// Creates Code Analysis projects from the MSBuild Projects
		/// </summary>
		/// <param name="guidToBuildProjects">The index of build projects</param>
		/// <returns>Index of projects</returns>
		private IEnumerable<CodeAnalysis.ProjectInfo> CreateProjects(Dictionary<string, MsBuild.Project> guidToBuildProjects)
		{
			Dictionary<string, CodeAnalysis.ProjectInfo> guidToCAProject = new Dictionary<string, CodeAnalysis.ProjectInfo>();

			// First create project infos
			foreach (string guid in guidToBuildProjects.Keys)
			{
				MsBuild.Project buildProj = guidToBuildProjects[guid];
				try
				{
					CodeAnalysis.ProjectInfo projInfo = CodeAnalysis.CommandLineProject.CreateProjectInfo(
						new FileInfo(buildProj.FullPath).Name,
						CodeAnalysis.LanguageNames.CSharp,
						CommandLineArgsFromBuildProject(buildProj),
						buildProj.DirectoryPath,
						_adHocWorkspace);

					projInfo = projInfo.WithFilePath(buildProj.FullPath);

					// Add documentation parse option so we get XML
					projInfo = projInfo.WithParseOptions(projInfo.ParseOptions.WithDocumentationMode(CodeAnalysis.DocumentationMode.Parse));
					guidToCAProject.Add(guid, projInfo);
				}
				catch (Exception)
				{

					throw;
				}				
			}

			// Add project references as necessary
			foreach (string guid in guidToBuildProjects.Keys)
			{
				MsBuild.Project buildProj = guidToBuildProjects[guid];
				IEnumerable<MsBuild.ProjectItem> projReferences = buildProj.Items.Where(itm => itm.ItemType == "ProjectReference");

				List<CodeAnalysis.ProjectReference> projRefs = new List<CodeAnalysis.ProjectReference>();
				foreach (MsBuild.ProjectItem item in projReferences)
				{
					MsBuild.ProjectMetadata metadata = item.Metadata.Where(md => md.Name == "Project").FirstOrDefault();
					CodeAnalysis.ProjectInfo caProj = guidToCAProject[metadata.EvaluatedValue.ToUpper()];
					CodeAnalysis.ProjectReference projRef = new CodeAnalysis.ProjectReference(caProj.Id);
					projRefs.Add(projRef);
				}
				guidToCAProject[guid] = guidToCAProject[guid].WithProjectReferences(projRefs);
			}

			return guidToCAProject.Values;
		}

		/// <summary>
		/// Gets the command line args to create a code analysis project
		/// </summary>
		/// <param name="proj">The build project</param>
		/// <returns>An enumerable list of arguments</returns>
		private IEnumerable<string> CommandLineArgsFromBuildProject(MsBuild.Project proj)
		{
			StringBuilder builder = new StringBuilder();
			string projDir = proj.DirectoryPath;

			// references (non-project since project are handled later)
			IEnumerable<MsBuild.ProjectItem> references = proj.Items.Where(itm => itm.ItemType == "Reference");
			IEnumerable<ResolvedReference> resolvedRefs = _refResolver.ResolveReferences(references);
			foreach (ResolvedReference oneRef in resolvedRefs)
			{
				builder.Append(oneRef.CommandLineArg);
			}
			
			// Compile files
			IEnumerable<MsBuild.ProjectItem> compile = proj.Items.Where(itm => itm.ItemType == "Compile");
			foreach (MsBuild.ProjectItem item in compile)
			{
				builder.Append($"{item.EvaluatedInclude} ");
			}

			// /out provides a name
			builder.Append($"/out:{new FileInfo(proj.FullPath).Name} ");

			// target: TODO: What does this do?
			builder.Append("/target:library ");

			return CodeAnalysis.CommandLineParser.SplitCommandLineIntoArguments(builder.ToString(), true);
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		/// <summary>
		/// Dispose method
		/// </summary>
		/// <param name="disposing">Flag for whether we are disposing</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					_adHocWorkspace.Dispose();
				}

				_adHocWorkspace = null;

				disposedValue = true;
			}
		}

		/// <summary>
		/// Dispose method
		/// </summary>
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
		}
		#endregion
	}
}
