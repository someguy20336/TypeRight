using TypeRight.CodeModel;
using TypeRight.ScriptWriting;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// Descriptor for a type paramteer
	/// </summary>
	public class TypeParameterDescriptor : TypeDescriptor
	{
		/// <summary>
		/// Creates a type parameter descriptor
		/// </summary>
		/// <param name="type">The type</param>
		internal TypeParameterDescriptor(IType type) : base(type)
		{
		}

		/// <summary>
		/// Formats the type
		/// </summary>
		/// <param name="formatter">The type formatter</param>
		/// <returns>The formatter type</returns>
		public override string FormatType(TypeFormatter formatter)
		{
			return formatter.FormatTypeParameter(this);
		}
	}
}
