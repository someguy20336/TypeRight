using System;
using System.Collections.Generic;
using System.Text;
using TypeRight.TypeProcessing;

namespace TypeRight.TypeFilters
{
	/// <summary>
	/// Common Type Filters
	/// </summary>
	public static class MvcTypeFilters
	{
		public static readonly TypeFilter HttpPostTypeFilter = new IsOfAnyTypeFilter(MvcConstants.HttpPostAttributeFullName_AspNet, MvcConstants.HttpPostAttributeFullName_AspNetCore);
		public static readonly TypeFilter HttpGetTypeFilter = new IsOfAnyTypeFilter(MvcConstants.HttpGetAttributeFullName_AspNet, MvcConstants.HttpGetAttributeFullName_AspNetCore);
		public static readonly TypeFilter HttpPutTypeFilter = new IsOfAnyTypeFilter(MvcConstants.HttpPutAttributeFullName_AspNet, MvcConstants.HttpPutAttributeFullName_AspNetCore);

	}
}
