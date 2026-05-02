namespace TypeRight.CodeModel
{
	/// <summary>
	/// An array type
	/// </summary>
	public interface IArrayType : IType
	{
		/// <summary>
		/// The element type of the array
		/// </summary>
		IType ElementType { get; }

		bool IsNullable { get; }
	}
}
