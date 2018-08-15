using TypeRight.ScriptWriting;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// Represents and unknown type, generally because it isn't extracted
	/// </summary>
	public class UnknownTypeDescriptor : TypeDescriptor
	{
		/// <summary>
		/// Creates a new unknown type
		/// </summary>
		internal UnknownTypeDescriptor() : base(null)
		{
		}

		/// <summary>
		/// Formats the type
		/// </summary>
		/// <param name="formatter">The type formatter</param>
		/// <returns>The formatter type</returns>
		public override string FormatType(TypeFormatter formatter)
		{
			return formatter.FormatUnknownType(this);
		}

		/// <summary>
		/// Pretty print
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return "(Unknown Type)";
		}
	}
}
