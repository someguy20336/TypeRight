using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRight
{
	/// <summary>
	/// The script generation result
	/// </summary>
	public interface IScriptGenerationResult
	{
		/// <summary>
		/// Gets whether the process was a success
		/// </summary>
		bool Success { get; }

		/// <summary>
		/// Gets the error message for the process
		/// </summary>
		string ErrorMessage { get; }
	}
}
