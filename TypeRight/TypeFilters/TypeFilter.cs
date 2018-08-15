using TypeRight.CodeModel;

namespace TypeRight.TypeFilters
{
	/// <summary>
	/// A filter for selecting symbols to extract
	/// </summary>
	public abstract class TypeFilter
	{
		/// <summary>
		/// Determines whether the named type meets the filter
		/// </summary>
		/// <param name="namedType">The named type to check</param>
		/// <returns>True if it meets the filter</returns>
		public abstract bool Evaluate(INamedType namedType);
	}
}
