using System.Collections.Generic;
using System.Linq;
using TypeRight.TypeFilters;

namespace TypeRight.TypeProcessing
{

	public interface IRequestMethod
	{
		string Name { get; }
		bool HasBody { get; }
		ActionFilter ActionFilter { get; }
		string GetActionTemplate(MvcActionInfo actionInfo);
	}

	internal class DefaultRequestMethod : IRequestMethod
	{
		public string Name { get; } = "DEFAULT";

		public bool HasBody { get; } = true;  // Just.... default i guess.

		public ActionFilter ActionFilter { get; } = new AcceptAllActionFilter();

		public string GetActionTemplate(MvcActionInfo actionInfo) => "";
	}

	public class RequestMethod : IRequestMethod
	{
		public static IEnumerable<IRequestMethod> RequestMethods { get; } = new IRequestMethod[]
		{

			new RequestMethod("GET", MvcConstants.HttpGetAttributeName, false),
			new RequestMethod("POST", MvcConstants.HttpPostAttributeName, true),
			new RequestMethod("PUT", MvcConstants.HttpPutAttributeName, true),
			new RequestMethod("DELETE", MvcConstants.HttpDeleteAttributeName, false),
			new DefaultRequestMethod()
		};

		public static IRequestMethod Default => GetByName("DEFAULT");
		public static IRequestMethod Get => GetByName("GET");
		public static IRequestMethod Post => GetByName("POST");

		public string Name { get; private set; }

		public bool HasBody { get; private set; }

		public TypeFilter TypeFilter { get; private set; }

		public ActionFilter ActionFilter { get; private set; }

		private RequestMethod(string name, string mvcAttributeName, bool hasBody)
		{
			Name = name;
			HasBody = hasBody;
			TypeFilter = new IsOfAnyTypeFilter(MvcConstants.ToAspNetFullName(mvcAttributeName), MvcConstants.ToAspNetCoreFullName(mvcAttributeName));
			ActionFilter = new ActionHasAttributeFilter(TypeFilter);
		}

		public string GetActionTemplate(MvcActionInfo actionInfo)
		{
			var mvcAttr = actionInfo.Attributes.FirstOrDefault(attr => TypeFilter.Evaluate(attr.AttributeType));
			if (mvcAttr.ConstructorArguments.Count > 0)
			{
				return mvcAttr.ConstructorArguments[0] as string;
			}
			return "";
		}

		public static IRequestMethod GetByName(string name)
		{
			name = name.ToUpper();
			return RequestMethods.FirstOrDefault(r => r.Name == name);
		}
	}
}
