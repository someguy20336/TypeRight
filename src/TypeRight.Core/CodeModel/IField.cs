using System.Collections.Generic;

namespace TypeRight.CodeModel
{
	/// <summary>
	/// Represents a type field
	/// </summary>
	public interface IField
	{
		/// <summary>
		/// Gets the name of the fielf
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets the comments for the field
		/// </summary>
		string Comments { get; }

		/// <summary>
		/// Gets the value of the field
		/// </summary>
		object Value { get; }

		/// <summary>
		/// Gets the attributes for the field
		/// </summary>
		IReadOnlyList<IAttributeData> Attributes { get; }
	}
}
