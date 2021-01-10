using Microsoft.CodeAnalysis;
using System;

namespace TypeRight.VsixContract
{
	/// <summary>
	/// Adapter for running script generation
	/// </summary>
	public interface IScriptGenerationAdapter
	{
		IScriptGenerationResult GenerateScripts(Workspace workspace, string projPath, bool force);
	}
}
