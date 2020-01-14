using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypeRight.TypeFilters;
using TypeRight.TypeProcessing.RouteGenerators;

namespace TypeRight.TypeProcessing
{
	public abstract class MvcRouteGenerator
	{
		public const string ConventionalBaseRouteTemplate = "/[controller]/[action]";
		public const string ConventionalBaseRouteTemplateWithArea = "/[area]/[controller]/[action]";

		protected MvcControllerInfo Controller { get; }

		protected MvcRouteGenerator(MvcControllerInfo controllerInfo)
		{
			Controller = controllerInfo;
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
				routeTemplate += GetActionTemplate(actionInfo);
			}

			return routeTemplate;
		}


		private string GetActionTemplate(MvcActionInfo actionInfo)
		{
			if (actionInfo.RequestMethod == RequestMethod.Get)
			{
				var getAttribute = actionInfo.Attributes.FirstOrDefault(attr => MvcTypeFilters.HttpGetTypeFilter.Evaluate(attr.AttributeType));
				if (getAttribute.ConstructorArguments.Count > 0)
				{
					return getAttribute.ConstructorArguments[0] as string;
				}
			}
			else if (actionInfo.RequestMethod == RequestMethod.Post)
			{
				var postAttr = actionInfo.Attributes.FirstOrDefault(attr => MvcTypeFilters.HttpPostTypeFilter.Evaluate(attr.AttributeType));
				if (postAttr.ConstructorArguments.Count > 0)
				{
					return postAttr.ConstructorArguments[0] as string;
				}
			}
			else if (actionInfo.RequestMethod == RequestMethod.Put)
			{
				var putAttr = actionInfo.Attributes.FirstOrDefault(attr => MvcTypeFilters.HttpPutTypeFilter.Evaluate(attr.AttributeType));
				if (putAttr.ConstructorArguments.Count > 0)
				{
					return putAttr.ConstructorArguments[0] as string;
				}
			}
			return "";
		}

		public static MvcRouteGenerator CreateGenerator(MvcControllerInfo controllerInfo)
		{
			TypeFilter aspNetCoreFilter = new IsOfTypeFilter(MvcConstants.ControllerBaseFullName_AspNetCore);

			return aspNetCoreFilter.Evaluate(controllerInfo.NamedType) 
				? new AspNetCoreRouteGenerator(controllerInfo) 
				: (MvcRouteGenerator)new AspNetRouteGenerator(controllerInfo);
		}

		private string CreateBaseRoute(string routeTemplate, string controllerName, string area)
		{
			string template = routeTemplate.Replace("[area]", area).Replace("[controller]", controllerName);

			if (!template.StartsWith("/"))
			{
				template = "/" + template;
			}
			if (!template.EndsWith("/"))
			{
				template += "/";
			}

			return template;
		}

		protected abstract string GetBaseRouteTemplate();

		protected abstract string GetArea();
	}
}
