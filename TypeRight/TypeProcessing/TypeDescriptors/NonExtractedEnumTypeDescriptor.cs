using TypeRight.CodeModel;
using TypeRight.ScriptWriting;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// Descriptor for an enum that isn't extracted
	/// </summary>
	public class NonExtractedEnumTypeDescriptor : TypeDescriptor
	{
		/// <summary>
		/// Creates a non extracted enum type
		/// </summary>
		/// <param name="type"></param>
		internal NonExtractedEnumTypeDescriptor(INamedType type) : base(type)
		{
		}

		/// <summary>
		/// Formats the type
		/// </summary>
		/// <param name="formatter">The type formatter</param>
		/// <returns>The formatter type</returns>
		public override string FormatType(TypeFormatter formatter)
		{
			return formatter.FormatNonExtractedEnum(this);
		}
	}
}
