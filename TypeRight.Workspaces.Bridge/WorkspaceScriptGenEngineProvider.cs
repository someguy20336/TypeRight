using TypeRight.ScriptGeneration;
using Microsoft.CodeAnalysis;
using System.ComponentModel.Composition;
using System.Linq;
using TypeRight.Workspaces.Parsing;

namespace TypeRight.Workspaces.Bridge
{

	/// <summary>
	/// Provides a workspace generation engine object
	/// </summary>
	[Export(typeof(IScriptGenEngineProvider<Workspace>))]
	public class WorkspaceGenEngineProvider : IScriptGenEngineProvider<Workspace>
	{
		/// <summary>
		/// Creates the script generation engine
		/// </summary>
		/// <param name="workspace">The workspace</param>
		/// <param name="projPath">The project path</param>
		/// <returns>The script generation engine</returns>
		public IScriptGenEngine GetEngine(Workspace workspace, string projPath)
		{
			// Can i just use proj path in package builder?
			ProjectId mainProjId = workspace.CurrentSolution.Projects
						.Where(pr => pr.FilePath == projPath && pr.SupportsCompilation).FirstOrDefault()?.Id;
			ProjectParser parser = new ProjectParser(workspace, mainProjId);
			return new ScriptGenEngine(projPath, parser);
		}
	}

}
