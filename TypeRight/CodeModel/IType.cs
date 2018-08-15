namespace TypeRight.CodeModel
{
	/// <summary>
	/// Represents a simple type that will be extracted to a script
	/// </summary>
	public interface IType
	{
		/// <summary>
		/// Gets the name of the type
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets the type flags for this type
		/// </summary>
		TypeFlags Flags { get; }
	}
}
