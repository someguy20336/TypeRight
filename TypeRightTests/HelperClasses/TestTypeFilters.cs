using TypeRight.CodeModel;
using TypeRight.TypeFilters;
using System.Collections.Generic;
using System.Linq;
using TypeRight.TypeProcessing;

namespace TypeRightTests.HelperClasses
{

	class ExcludeWithAnyName : TypeFilter
	{

		private IEnumerable<string> _excludeNames;

		public ExcludeWithAnyName(params string[] names)
		{
			_excludeNames = names;
		}
		/// <summary>
		/// Determines whether the named type meets the filter
		/// </summary>
		/// <param name="namedType">The named type to check</param>
		/// <returns>True if it meets the filter</returns>
		public override bool Evaluate(INamedType typeSymbol)
		{
			return !_excludeNames.Any(name => name == typeSymbol.Name);
		}
	}

	class AcceptWithName : TypeFilter
	{

		private List<string> _includeNames;

		public AcceptWithName(string name)
			: this(new List<string>() { name })
		{
		}

		public AcceptWithName(List<string> names)
		{
			_includeNames = names;
		}
		/// <summary>
		/// Determines whether the named type meets the filter
		/// </summary>
		/// <param name="namedType">The named type to check</param>
		/// <returns>True if it meets the filter</returns>
		public override bool Evaluate(INamedType typeSymbol)
		{
			return _includeNames.Any(name => name == typeSymbol.Name);
		}
	}

	class AlwaysAcceptFilter : TypeFilter
	{
		/// <summary>
		/// Determines whether the named type meets the filter
		/// </summary>
		/// <param name="namedType">The named type to check</param>
		/// <returns>True if it meets the filter</returns>
		public override bool Evaluate(INamedType typeSymbol)
		{
			return true;
		}
	}

	class AlwaysRejectFilter : TypeFilter
	{
		/// <summary>
		/// Determines whether the named type meets the filter
		/// </summary>
		/// <param name="namedType">The named type to check</param>
		/// <returns>True if it meets the filter</returns>
		public override bool Evaluate(INamedType typeSymbol)
		{
			return false;
		}
	}

	class AcceptAllParameters : ParameterFilter
	{
		/// <summary>
		/// Evaluates the parameter for whether it should be included
		/// </summary>
		/// <param name="actionParameter">The parameter</param>
		/// <returns>True if it should be inclued</returns>
		public override bool Evaluate(MvcActionParameter actionParameter)
		{
			return true;
		}
	}
}
