using TypeRight.CodeModel;

namespace TypeRight.TypeFilters
{
	/// <summary>
	/// Parse filter for whether the symbol is of a specific type in its heirarchy
	/// </summary>
	internal class IsOfTypeFilter : TypeFilter
	{
		/// <summary>
		/// The type to check
		/// </summary>
		private readonly string _typeToCheck;

		/// <summary>
		/// Creates a new "Is Of Type" filter
		/// </summary>
		/// <param name="typeToCheck">The type to check</param>
		public IsOfTypeFilter(string typeToCheck)
		{
			_typeToCheck = typeToCheck;
		}

		/// <summary>
		/// Determines whether the type meets the filter
		/// </summary>
		/// <param name="namedType">The type to check</param>
		/// <returns>True if it meets the criteria</returns>
		public override bool Matches(INamedType namedType)
		{
			INamedType baseType = namedType;
			while (baseType != null)
			{
				if (baseType.FullName == _typeToCheck)
				{
					return true;
				}
				baseType = baseType.BaseType;
			}
			return false;
		}
	}
}
