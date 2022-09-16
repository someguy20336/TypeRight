using System;
using System.Collections.Generic;
using System.Linq;
using TypeRight.TypeFilters;

namespace TypeRight.TypeProcessing
{

	public interface IRequestMethod
	{
		string Name { get; }

		string MethodName { get; }
		bool HasBody { get; }
		ActionFilter ActionFilter { get; }
		string GetActionTemplate(MvcAction actionInfo);
	}

	internal class DefaultRequestMethod : IRequestMethod
	{
		public string Name { get; } = "default";
		public string MethodName => "GET";		// Default GET because we don't know otherwise

		public bool HasBody { get; } = true;  // Just.... default i guess... but now weird with GET as default..

		public ActionFilter ActionFilter { get; } = new AcceptAllActionFilter();

		public string GetActionTemplate(MvcAction actionInfo) => "";
	}

	public class RequestMethod : IRequestMethod
	{
		public static IEnumerable<IRequestMethod> RequestMethods { get; } = new IRequestMethod[]
		{

			new RequestMethod("GET", MvcConstants.HttpGetAttributeName, false),
			new RequestMethod("POST", MvcConstants.HttpPostAttributeName, true),
			new RequestMethod("PUT", MvcConstants.HttpPutAttributeName, true),
			new RequestMethod("PATCH", MvcConstants.HttpPatchAttributeName, true),
			new RequestMethod("DELETE", MvcConstants.HttpDeleteAttributeName, false),
			new DefaultRequestMethod()
		};

		public static IRequestMethod Default => GetByName("default");
		public static IRequestMethod Get => GetByName("GET");
		public static IRequestMethod Post => GetByName("POST");

		public string Name { get; private set; }

		public string MethodName => Name;

		public bool HasBody { get; private set; }

		public TypeFilter TypeFilter { get; private set; }

		public ActionFilter ActionFilter { get; private set; }

		private RequestMethod(string name, string mvcAttributeName, bool hasBody)
		{
			Name = name;
			HasBody = hasBody;
			TypeFilter = new IsOfAnyTypeFilter(MvcConstants.ToAspNetCoreFullName(mvcAttributeName));
			ActionFilter = new ActionHasAttributeFilter(TypeFilter);
		}

		public string GetActionTemplate(MvcAction actionInfo)
		{
			var mvcAttr = actionInfo.Attributes.FirstOrDefault(attr => TypeFilter.Matches(attr.AttributeType));
			if (mvcAttr.ConstructorArguments.Count > 0)
			{
				return mvcAttr.ConstructorArguments[0] as string ?? "";
			}
			return "";
		}

		public static IRequestMethod GetByName(string name)
		{
			name = name.ToUpper();
			return RequestMethods.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
		}
	}
}
