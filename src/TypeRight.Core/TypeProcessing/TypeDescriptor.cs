using TypeRight.CodeModel;
using TypeRight.ScriptWriting;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// Describes a given type
	/// </summary>
	public abstract class TypeDescriptor
	{
		/// <summary>
		/// Gets the type being described
		/// </summary>
		public IType Type { get; }

		/// <summary>
		/// Creates a new type descriptor
		/// </summary>
		/// <param name="type">The type</param>
		protected TypeDescriptor(IType type)
		{
			Type = type;
		}

		/// <summary>
		/// Formats the given type
		/// </summary>
		/// <param name="formatter">The formatter</param>
		/// <returns>The formatted type name</returns>
		public abstract string FormatType(TypeFormatter formatter);

		/// <summary>
		/// Pretty print
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Type.ToString();
		}
	}
}
