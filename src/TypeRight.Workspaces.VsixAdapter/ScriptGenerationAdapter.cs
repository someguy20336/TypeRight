using Microsoft.CodeAnalysis;
using System.Linq;
using TypeRight.Workspaces.Parsing;
using TypeRight.VsixContract.Messages;
using TypeRight.VsixContractV2;

namespace TypeRight.Workspaces.VsixAdapter
{

	/// <summary>
	/// Provides a workspace generation engine object
	/// </summary>
	public class ScriptGenerationAdapter
	{
		/// <summary>
		/// Creates the script generation engine
		/// </summary>
		/// <param name="workspace">The workspace</param>
		/// <param name="projPath">The project path</param>
		/// <returns>The script generation engine</returns>
		public static IResponse GenerateScripts(IRequest request)
		{
			GenerateScriptsRequest message = GenerateScriptsRequest.Read(request);
			Workspace workspace = message.Workspace;
			string projPath = message.ProjectPath;

			// Can i just use proj path in package builder?
			ProjectId mainProjId = workspace.CurrentSolution.Projects
						.Where(pr => pr.FilePath == projPath && pr.SupportsCompilation).FirstOrDefault()?.Id;
			ProjectParser parser = new ProjectParser(workspace, mainProjId);
			var result = new ScriptGenEngine().GenerateScripts(new ScriptGenerationParameters()
			{
				ProjectPath = projPath,
				TypeIterator = parser,
				Force = message.Force
			});

			return new GenerateScriptsResponse(result.Success, result.ErrorMessage);
		}
	}

}
