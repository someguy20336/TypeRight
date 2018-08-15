using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRight
{
	/// <summary>
	/// The script generation engine provider
	/// </summary>
	/// <typeparam name="TSolutionProvider">The object type that is used to provide solution details</typeparam>
	public interface IScriptGenEngineProvider<TSolutionProvider>
	{
		/// <summary>
		/// Gets the script generation engine
		/// </summary>
		/// <param name="solutionProvider">The object that will be providing details about the solution</param>
		/// <param name="projPath">The project path</param>
		/// <returns>The script generation engine</returns>
		IScriptGenEngine GetEngine(TSolutionProvider solutionProvider, string projPath);
	}
}
