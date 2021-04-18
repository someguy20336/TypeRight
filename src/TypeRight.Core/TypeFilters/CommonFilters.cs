using TypeRight.Attributes;

namespace TypeRight.TypeFilters
{
	public static class CommonFilters
	{
		public static readonly TypeFilter ScriptActionAttributeTypeFilter
			= new IsOfTypeFilter(typeof(ScriptActionAttribute).FullName);
	}
}
