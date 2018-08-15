using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRight
{
	/// <summary>
	/// A script generation engine
	/// </summary>
    public interface IScriptGenEngine
    {
		/// <summary>
		/// Generates scripts
		/// </summary>
		/// <returns>The result of the generation</returns>
		IScriptGenerationResult GenerateScripts();
	}
}
