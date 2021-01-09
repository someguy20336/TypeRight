using System;

namespace TypeRight.Attributes
{
	/// <summary>
	/// Use this attribute to extract a type or list of types without adding the <see cref="ScriptObjectAttribute"/> to the type.
	/// This can be used to extract types that aren't in the current assembly
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public class ScriptObjectsAttribute : Attribute
	{

		/// <summary>
		/// Marks a list of types for extraction
		/// </summary>
		/// <param name="types">The types to extract</param>
		public ScriptObjectsAttribute(params Type[] types)
		{
		}

		/// <summary>
		/// Marks a list of types for extraction to the given relatively path
		/// </summary>
		/// <param name="path">The path, relative to the root of the project, to save the result file to</param>
		/// <param name="types">The types to extract</param>
		public ScriptObjectsAttribute(string path, params Type[] types)
		{
		}
	}
}
