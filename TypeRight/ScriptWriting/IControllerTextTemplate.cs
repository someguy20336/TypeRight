using System;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting
{
	/// <summary>
	/// A template for writing a single controller script
	/// </summary>
	public interface IControllerTextTemplate
	{
		/// <summary>
		/// Gets the controller template text
		/// </summary>
		/// <param name="controllerInfo">The controller info</param>
		/// <param name="context">The script write context</param>
		/// <param name="outputPath">The output path for the controller</param>
		/// <returns>the script text</returns>
		string GetText(MvcControllerInfo controllerInfo, ScriptWriteContext context, Uri outputPath);
	}
}
