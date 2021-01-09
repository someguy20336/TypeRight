using System;

namespace TypeRight.Attributes
{
	/// <summary>
	/// Defines a "functional" type for the given class.  In most cases, this isn't necessary.  However,
	/// you may have a class that is functionally a string in javascript, but is converted to something
	/// else on the server.
	/// 
	/// For classes with this attribute, you generally do not want to mark it with <see cref="ScriptObjectAttribute"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class FunctionalTypeAttribute : Attribute
	{
		/// <summary>
		/// Gets the functional type of the class
		/// </summary>
		public Type FunctionalType { get; private set; }

		/// <summary>
		/// Creates a new functional type attribute
		/// </summary>
		/// <param name="functionalType">The functional type to use</param>
		public FunctionalTypeAttribute(Type functionalType)
		{
			FunctionalType = functionalType;
		}
	}
}
