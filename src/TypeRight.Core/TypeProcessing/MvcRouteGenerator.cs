using System.Collections.Generic;
using TypeRight.ScriptWriting;
using TypeRight.TypeFilters;
using TypeRight.TypeProcessing.MvcRouting;

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

			// Append HttpGet, Post, etc route name
			string actionTemplate = GetActionTemplate(actionInfo);
			if (!string.IsNullOrEmpty(actionTemplate))
			{
				routeTemplate = AppendSlashIfNecessary(routeTemplate);
				routeTemplate += actionTemplate;
			}

			routeTemplate = PrependBasePath(routeTemplate);

			routeTemplate = routeTemplate.Replace("[area]", area);	// Not a great option for this yet...

			foreach (var resolver in GetParameterResolvers())
			{
				routeTemplate = resolver.TryResolve(routeTemplate, Controller, actionInfo);
			}
			

			return routeTemplate;

		}

		private string PrependBasePath(string url)
		{
			string baseUrl = !string.IsNullOrEmpty(_context.BaseUrl) ? _context.BaseUrl : "/";
			baseUrl = AppendSlashIfNecessary(baseUrl);

			url = baseUrl + url.TrimStart('/');
			if (!url.StartsWith("/"))
			{
				url = "/" + url;
			}
			return url;
		}

		private string AppendSlashIfNecessary(string path)
		{
			if (!path.EndsWith("/"))
			{
				path += "/";
			}
			return path;
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

		protected virtual List<RouteParameterResolver> GetParameterResolvers()
		{
			return new List<RouteParameterResolver>()
			{
				new DelegateRouteParameterResolver((r, c, a) => r.Replace("[controller]", c.ControllerName)),
				new DelegateRouteParameterResolver((r, c, a) => r.Replace("[action]", a.Name))
			};
		}

		protected abstract string GetBaseRouteTemplate();

		protected abstract string GetArea();
	}
}
