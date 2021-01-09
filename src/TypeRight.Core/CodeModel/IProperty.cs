namespace TypeRight.CodeModel
{
	/// <summary>
	/// Represents a property that will extracted to a script
	/// </summary>
	public interface IProperty
	{
		/// <summary>
		/// Gets the name of the property
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets the type of this property
		/// </summary>
		IType PropertyType { get; }

		/// <summary>
		/// Gets the comments for this property
		/// </summary>
		string Comments { get; }
	}
}
