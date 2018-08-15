using System;

namespace TypeRight.Attributes
{
	/// <summary>
	/// Marks an MVC controller action for extraction
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class MvcActionAttribute : Attribute
	{
	}
}
