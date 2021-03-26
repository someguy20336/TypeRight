using System;

namespace TypeRight.Attributes
{
	/// <summary>
	/// Overrides the type of an action parameter to one or more types
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter)]
	public class ScriptParamTypesAttribute : Attribute
	{
		/// <summary>
		/// Gets the array of override types
		/// </summary>
		public Type[] Types { get; }

		/// <summary>
		/// Creates a new <see cref="ScriptParamTypesAttribute"/> with the following override types
		/// </summary>
		/// <param name="types">The types to override for the action parameter</param>
		public ScriptParamTypesAttribute(params Type[] types)
		{
			Types = types;
		}

	}
}
