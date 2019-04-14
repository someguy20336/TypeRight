using System;

namespace TypeRight.Attributes
{
    /// <summary>
    /// Attribute used to mark an object for extraction.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public class ScriptObjectAttribute : Attribute
	{
		/// <summary>
		/// Creates a script object that gets added to the default path
		/// </summary>
		public ScriptObjectAttribute() { }

		///// <summary>
		///// Creates a script object that gets added to the given target path
		///// </summary>
		///// <param name="targetPath">The path relative to the root of the project</param>
		//public ScriptObjectAttribute(string targetPath) { }
    }
}
