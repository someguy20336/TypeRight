namespace TypeRight.TypeLocation
{
	/// <summary>
	/// An object that iterates types with the given visitor
	/// </summary>
	public interface ITypeIterator
	{
		/// <summary>
		/// Iterates types
		/// </summary>
		/// <param name="visitor">The visitor to use when a type is found</param>
		void IterateTypes(TypeVisitor visitor);
	}
}
