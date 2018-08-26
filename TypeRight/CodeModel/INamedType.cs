using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRight.CodeModel
{
	/// <summary>
	/// Represents a type that will be extracted to a script
	/// </summary>
	public interface INamedType : IType, ITypeWithFullName
	{

		/// <summary>
		/// Gets the type this type was constructed from (when this type is constructed from a generic)
		/// </summary>
		INamedType ConstructedFromType { get; }

		/// <summary>
		/// Gets the base type of this type, if applicable
		/// </summary>
		INamedType BaseType { get; }

		/// <summary>
		/// Gets the interfaces implemented by this type
		/// </summary>
		IReadOnlyList<INamedType> Interfaces { get; }

		/// <summary>
		/// Gets the type arguments for this type
		/// </summary>
		IReadOnlyList<IType> TypeArguments { get; }

		/// <summary>
		/// Gets the comments for this type
		/// </summary>
		string Comments { get; }

		/// <summary>
		/// Gets the properties for this type
		/// </summary>
		IReadOnlyList<IProperty> Properties { get; }

		/// <summary>
		/// Gets the fields for this type
		/// </summary>
		IReadOnlyList<IField> Fields { get; }

		/// <summary>
		/// Gets the methods for this type
		/// </summary>
		IReadOnlyList<IMethod> Methods { get; }

		/// <summary>
		/// Gets the attributes for this type
		/// </summary>
		IReadOnlyList<IAttributeData> Attributes { get; }

		/// <summary>
		/// Gets the flags for this type
		/// </summary>
		TypeFlags Flags { get; }


		/// <summary>
		/// TODO: I DONT WANT THIS
		/// </summary>
		string FilePath { get; }
	}
}
