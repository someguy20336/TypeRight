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

	}
}
