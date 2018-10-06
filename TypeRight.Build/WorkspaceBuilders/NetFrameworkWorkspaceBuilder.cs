using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsBuild = Microsoft.Build.Evaluation;
using CodeAnalysis = Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host.Mef;
using System.IO;

namespace TypeRight.Build.WorkspaceBuilders
{
	/// <summary>
	/// Workspace builder for the .NET framework projects
	/// </summary>
	class NetFrameworkWorkspaceBuilder : WorkspaceBuilder
	{
		/// <summary>
		/// The object to use to resolve references
		/// </summary>
		private ReferenceResolver _refResolver;


		/// <summary>
		/// Creates a new solution workspace parser for the given solution
		/// </summary>
		/// <param name="projPath">The path to the project</param>
		/// <param name="refResolver">The reference resolver to use</param>
		public NetFrameworkWorkspaceBuilder(string projPath, ReferenceResolver refResolver) : base(projPath)
		{
			_refResolver = refResolver;
		}

		/// <summary>
		/// Creates the project info from the msBuild project
		/// </summary>
		/// <param name="msBuildProj">The ms Build project</param>
		/// <returns>The project info</returns>
		protected override CodeAnalysis.ProjectInfo CreateProjectInfo(MsBuild.Project msBuildProj)
		{
			return CodeAnalysis.CommandLineProject.CreateProjectInfo(
						new FileInfo(msBuildProj.FullPath).Name,
						CodeAnalysis.LanguageNames.CSharp,
						CommandLineArgsFromBuildProject(msBuildProj),
						msBuildProj.DirectoryPath,
						null);
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
			IEnumerable<ResolvedReference> resolvedRefs = _refResolver.ResolveReferences(proj);
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
	}
}
