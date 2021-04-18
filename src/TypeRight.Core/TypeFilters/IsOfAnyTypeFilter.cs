using System.Collections.Generic;
using System.Linq;
using TypeRight.CodeModel;

namespace TypeRight.TypeFilters
{
	/// <summary>
	/// A type filter that checks if the type is of any specfied type
	/// </summary>
	internal class IsOfAnyTypeFilter : TypeFilter
	{

		private IEnumerable<TypeFilter> _singleTypeFilters;

		/// <summary>
		/// Creates a new <see cref="IsOfTypeFilter"/>
		/// </summary>
		/// <param name="typeNames">The type names to include</param>
		public IsOfAnyTypeFilter(params string[] typeNames)
		{
			_singleTypeFilters = typeNames.Select(name => new IsOfTypeFilter(name));
		}

		/// <summary>
		/// Determines whether the named type meets the filter
		/// </summary>
		/// <param name="namedType">The named type to check</param>
		/// <returns>True if it meets the filter</returns>
		public override bool Evaluate(INamedType namedType)
		{
			return _singleTypeFilters.Any(filt => filt.Evaluate(namedType));
		}
	}
}
