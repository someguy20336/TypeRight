using System;

namespace TypeRight.Attributes
{
	/// <summary>
	/// Directs the script generator to where the output script for this object should go
	/// </summary>
	public sealed class ScriptOutputAttribute : Attribute
	{
		/// <summary>
		/// Directs the script generator to where the output script for this object should go
		/// </summary>
		/// <param name="relativePath">The path relative to this file</param>
		public ScriptOutputAttribute(string relativePath)
		{

		}
	}
}
