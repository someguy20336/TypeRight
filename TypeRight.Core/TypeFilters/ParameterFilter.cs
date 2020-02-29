using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypeRight.TypeProcessing;

namespace TypeRight.TypeFilters
{
	/// <summary>
	/// Filter for MVC action parameters
	/// </summary>
	public abstract class ParameterFilter
	{
		/// <summary>
		/// Evaluates the parameter for whether it should be included
		/// </summary>
		/// <param name="actionParameter">The parameter</param>
		/// <returns>True if it should be inclued</returns>
		public abstract bool Evaluate(MvcActionParameter actionParameter);
	}

	/// <summary>
	/// Filter for checking if a parameter has an attribute
	/// </summary>
	public class ParameterHasAttributeFilter : ParameterFilter
	{
		private TypeFilter _attributeFilter;

		public ParameterHasAttributeFilter(TypeFilter filter)
		{
			_attributeFilter = filter;
		}
		/// <summary>
		/// Evaluates the parameter for whether it should be included
		/// </summary>
		/// <param name="actionParameter">The parameter</param>
		/// <returns>True if it should be inclued</returns>
		public override bool Evaluate(MvcActionParameter actionParameter)
		{
			return actionParameter.Attributes.Any(attr => _attributeFilter.Evaluate(attr.AttributeType));
		}
	}
}
