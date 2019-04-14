using TypeRight.CodeModel;
using TypeRight.ScriptWriting;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// A type descriptor for an enum that is extracted
	/// </summary>
	public class ExtractedEnumTypeDescriptor : ExtractedTypeDescriptor
	{
		/// <summary>
		/// Gets whether this enum is using the extended syntax
		/// </summary>
		public bool UseExtendedSyntax { get; }

		/// <summary>
		/// Creates a new extracted enum type descriptor
		/// </summary>
		/// <param name="type"></param>
		/// <param name="typeNamespace"></param>
		/// <param name="usingExtendedSyntax"></param>
		/// <param name="targetPath">The target path of the type</param>
		internal ExtractedEnumTypeDescriptor(INamedType type, string typeNamespace, bool usingExtendedSyntax, string targetPath) : base(type, typeNamespace, targetPath)
		{
			UseExtendedSyntax = usingExtendedSyntax;
		}

		/// <summary>
		/// Formats the type
		/// </summary>
		/// <param name="formatter">The type formatter</param>
		/// <returns>The formatter type</returns>
		public override string FormatType(TypeFormatter formatter)
		{
			return formatter.FormatExtractedEnumType(this);
		}
	}
}
