using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypeRight.TypeProcessing;

namespace TypeRight.TypeFilters
{
	/// <summary>
	/// Filter for MVC actions
	/// </summary>
	public abstract class ActionFilter
	{
		/// <summary>
		/// Evaluates the action for whether it should be included
		/// </summary>
		/// <param name="action">The action</param>
		/// <returns>True if it should be included</returns>
		public abstract bool Evaluate(MvcActionInfo action);
	}


	/// <summary>
	/// Filter for checking if a parameter has an attribute
	/// </summary>
	public class ActionHasAttributeFilter : ActionFilter
	{
		private TypeFilter _attributeFilter;

		public ActionHasAttributeFilter(TypeFilter filter)
		{
			_attributeFilter = filter;
		}
		/// <summary>
		/// Evaluates the action for whether it should be included
		/// </summary>
		/// <param name="action">The action</param>
		/// <returns>True if it should be included</returns>
		public override bool Evaluate(MvcActionInfo action)
		{
			return action.Attributes.Any(attr => _attributeFilter.Evaluate(attr.AttributeType));
		}
	}
}
