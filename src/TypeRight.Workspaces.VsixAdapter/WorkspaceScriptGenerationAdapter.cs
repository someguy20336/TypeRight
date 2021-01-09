using Microsoft.CodeAnalysis;
using System.ComponentModel.Composition;
using System.Linq;
using TypeRight.Workspaces.Parsing;
using TypeRight.VsixContract;

namespace TypeRight.Workspaces.VsixAdapter
{

	/// <summary>
	/// Provides a workspace generation engine object
	/// </summary>
	[Export(typeof(IScriptGenerationAdapter))]
	public class WorkspaceGenerationAdapter : IScriptGenerationAdapter
	{
		/// <summary>
		/// Creates the script generation engine
		/// </summary>
		/// <param name="workspace">The workspace</param>
		/// <param name="projPath">The project path</param>
		/// <returns>The script generation engine</returns>
		public IScriptGenerationResult GenerateScripts(Workspace workspace, string projPath, bool force)
		{
			// Can i just use proj path in package builder?
			ProjectId mainProjId = workspace.CurrentSolution.Projects
						.Where(pr => pr.FilePath == projPath && pr.SupportsCompilation).FirstOrDefault()?.Id;
			ProjectParser parser = new ProjectParser(workspace, mainProjId);
			var result = new ScriptGenEngine().GenerateScripts(new ScriptGenerationParameters()
			{
				ProjectPath = projPath,
				TypeIterator = parser
			});

			return new ScriptGenerationResultAdapter()
			{
				ErrorMessage = result.ErrorMessage,
				Success = result.Success
			};
		}
	}

}
