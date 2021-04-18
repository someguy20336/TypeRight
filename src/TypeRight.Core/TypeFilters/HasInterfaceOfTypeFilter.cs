using TypeRight.CodeModel;

namespace TypeRight.TypeFilters
{
	/// <summary>
	/// A parse filter for whether the type has an interface of a given type
	/// </summary>
	internal class HasInterfaceOfTypeFilter : TypeFilter
	{
		/// <summary>
		/// The type to check
		/// </summary>
		private string _typeToCheck;

		/// <summary>
		/// Creates a new "Is Of Type" filter
		/// </summary>
		/// <param name="typeToCheck">The type to check</param>
		public HasInterfaceOfTypeFilter(string typeToCheck)
		{
			_typeToCheck = typeToCheck;
		}

		/// <summary>
		/// Determines whether the type meets the filter
		/// </summary>
		/// <param name="namedType">The type to check</param>
		/// <returns>True if it meets the criteria</returns>
		public override bool Evaluate(INamedType namedType)
		{
			foreach (INamedType interfaceType in namedType.Interfaces)
			{
				if (interfaceType.FullName == _typeToCheck)
				{
					return true;
				}
			}
			return false;
		}
	}
}
