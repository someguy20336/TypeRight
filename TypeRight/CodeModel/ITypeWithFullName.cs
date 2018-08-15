namespace TypeRight.CodeModel
{
	/// <summary>
	/// An object that is named
	/// </summary>
	public interface ITypeWithFullName
	{

		/// <summary>
		/// Gets the full name of the class, as known in code
		/// </summary>
		string FullName { get; }
	}
}
