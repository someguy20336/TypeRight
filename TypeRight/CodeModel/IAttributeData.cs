using System.Collections.Generic;

namespace TypeRight.CodeModel
{
	/// <summary>
	/// Represents data for a given attribute
	/// </summary>
	public interface IAttributeData
	{
		/// <summary>
		/// Gets the attribute type
		/// </summary>
		INamedType AttributeType { get; }

		/// <summary>
		/// Gets the dictionary of named arguments
		/// </summary>
		IReadOnlyDictionary<string, object> NamedArguments { get; }

		/// <summary>
		/// Gets the constructor arguments
		/// </summary>
		IReadOnlyList<object> ConstructorArguments { get; }
	}
}
