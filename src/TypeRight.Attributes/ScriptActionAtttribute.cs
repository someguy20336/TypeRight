using System;

namespace TypeRight.Attributes
{
	/// <summary>
	/// Marks an MVC controller action for extraction
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class ScriptActionAttribute : Attribute
	{
		/// <summary>
		/// Gets or sets the name to use for this action
		/// </summary>
		public string Name { get; set; }
	}
}
