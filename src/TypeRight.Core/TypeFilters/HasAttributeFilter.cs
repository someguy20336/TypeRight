using TypeRight.CodeModel;
using System.Collections.Generic;

namespace TypeRight.TypeFilters
{
	/// <summary>
	/// Parse filter for "Has specified attribute"
	/// </summary>
	internal class HasAttributeFilter : TypeFilter
	{
		/// <summary>
		/// The full name of the attribute type
		/// </summary>
		private string _attrFullName;

		/// <summary>
		/// Creates a new has attribute filter
		/// </summary>
		/// <param name="attributeFullName">The full name the attribute type</param>
		public HasAttributeFilter(string attributeFullName)
		{
			_attrFullName = attributeFullName;
		}

		/// <summary>
		/// Determines if the symbol meets the parse filter
		/// </summary>
		/// <param name="typeSymbol">The type symbol</param>
		/// <returns>True if it meets the criteria</returns>
		public override bool Evaluate(INamedType typeSymbol)
		{
			IReadOnlyList<IAttributeData> attributes = typeSymbol.Attributes;

			// Look for the matching attribute
			foreach (IAttributeData attr in attributes)
			{
				if (attr.AttributeType.FullName == _attrFullName)
				{
					return true;
				}
			}
			return false;
		}
	}
}
