using System.Linq;
using TypeRight.CodeModel;
using TypeRight.TypeProcessing;

namespace TypeRight.TypeFilters
{
	/// <summary>
	/// Filter for MVC action parameters
	/// </summary>
	internal abstract class ParameterFilter
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
	internal class ParameterHasAttributeFilter : ParameterFilter
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
		public override bool Evaluate(MvcActionParameter actionParameter) => GetAttribute(actionParameter) is not null;

		public IAttributeData GetAttribute(MvcActionParameter actionParameter) 
			=> actionParameter.Attributes.FirstOrDefault(attr => _attributeFilter.Matches(attr.AttributeType));
	}
}
