using TypeRight.ScriptWriting;
using TypeRight.TypeFilters;
using TypeRight.TypeProcessing.RouteGenerators;

namespace TypeRight.TypeProcessing
{
	public abstract class MvcRouteGenerator
	{
		public const string ConventionalBaseRouteTemplate = "/[controller]/[action]";
		public const string ConventionalBaseRouteTemplateWithArea = "/[area]/[controller]/[action]";
		private readonly ControllerContext _context;

		protected MvcControllerInfo Controller => _context.Controller;

		protected MvcRouteGenerator(ControllerContext context)
		{
			_context = context;
		}

		public string GenerateRouteTemplate(MvcActionInfo actionInfo)
		{
			string area = GetArea();
			string routeTemplate = GetBaseRouteTemplate();

			if (string.IsNullOrEmpty(routeTemplate))
			{
				routeTemplate = string.IsNullOrEmpty(area) ? ConventionalBaseRouteTemplate : ConventionalBaseRouteTemplateWithArea;
			}

			routeTemplate = routeTemplate.Replace("[area]", area)
				.Replace("[controller]", Controller.ControllerName)
				.Replace("[action]", actionInfo.Name);

			if (!routeTemplate.StartsWith("/"))
			{
				routeTemplate = "/" + routeTemplate;
			}

			// Append HttpGet, Post, etc route name
			string actionTemplate = GetActionTemplate(actionInfo);
			if (!string.IsNullOrEmpty(actionTemplate))
			{
				if (!routeTemplate.EndsWith("/"))
				{
					routeTemplate += "/";
				}
				routeTemplate += actionTemplate;
			}

			return routeTemplate;
		}


		private string GetActionTemplate(MvcActionInfo actionInfo)
		{
			return actionInfo.RequestMethod.GetActionTemplate(actionInfo);
		}

		public static MvcRouteGenerator CreateGenerator(ControllerContext context)
		{
			TypeFilter aspNetCoreFilter = new IsOfTypeFilter(MvcConstants.ControllerBaseFullName_AspNetCore);

			var controllerInfo = context.Controller;
			return aspNetCoreFilter.Evaluate(controllerInfo.NamedType) 
				? new AspNetCoreRouteGenerator(context) 
				: (MvcRouteGenerator)new AspNetRouteGenerator(context);
		}

		protected abstract string GetBaseRouteTemplate();

		protected abstract string GetArea();
	}
}
