using TypeRight.CodeModel;
using TypeRight.TypeFilters;
using System.Collections.Generic;
using System.Linq;

namespace TypeRightTests.HelperClasses
{

	class ExcludeWithAnyName : TypeFilter
	{

		private IEnumerable<string> _excludeNames;

		public ExcludeWithAnyName(params string[] names)
		{
			_excludeNames = names;
		}

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

		public override bool Evaluate(INamedType typeSymbol)
		{
			return _includeNames.Any(name => name == typeSymbol.Name);
		}
	}

	class AlwaysAcceptFilter : TypeFilter
	{
		public override bool Evaluate(INamedType typeSymbol)
		{
			return true;
		}
	}

	class AlwaysRejectFilter : TypeFilter
	{
		public override bool Evaluate(INamedType typeSymbol)
		{
			return false;
		}
	}
}
