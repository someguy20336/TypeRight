using System.Collections.Generic;

namespace TypeRight.CodeModel
{
	/// <summary>
	/// Represents a parameter for a method
	/// </summary>
	public interface IMethodParameter
	{
		/// <summary>
		/// Gets the name of the parameter
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets the comments for this parameter
		/// </summary>
		string Comments { get; }

		/// <summary>
		/// Gets the type of the parameter
		/// </summary>
		IType ParameterType { get; }

		/// <summary>
		/// Gets the attribues for this parameter
		/// </summary>
		IEnumerable<IAttributeData> Attributes { get; }

		/// <summary>
		/// Gets whether this parameter is an optional parameter
		/// </summary>
		bool IsOptional { get; }
	}
}
