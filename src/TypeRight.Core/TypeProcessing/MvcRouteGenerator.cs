﻿using System.Collections.Generic;
using TypeRight.TypeFilters;
using TypeRight.TypeProcessing.MvcRouting;

namespace TypeRight.TypeProcessing
{
	public abstract class MvcRouteGenerator
	{
		public const string ConventionalBaseRouteTemplate = "/[controller]/[action]";
		public const string ConventionalBaseRouteTemplateWithArea = "/[area]/[controller]/[action]";
		private readonly string _baseUrl;

		protected MvcController Controller { get; }

		protected MvcRouteGenerator(MvcController controller, string baseUrl)
		{
			_baseUrl = baseUrl;
			Controller = controller;
		}

		public string GenerateRouteTemplate(MvcAction actionInfo)
		{
			string routeTemplate = GetBaseRouteTemplate();

			// Start with HttpGet, Post, etc route name
			string actionTemplate = GetActionTemplate(actionInfo);
            if (actionTemplate.StartsWith("/"))		// If rooted, then don't use Route
            {
				routeTemplate = actionTemplate;
            }
			else if (!string.IsNullOrEmpty(actionTemplate))
			{
				routeTemplate = AppendSlashIfNecessary(routeTemplate);
				routeTemplate += actionTemplate;
			}

			// If we still have nothing, then assume conventional routing
			string area = GetArea();
			if (string.IsNullOrEmpty(routeTemplate))
			{
				routeTemplate = string.IsNullOrEmpty(area) ? ConventionalBaseRouteTemplate : ConventionalBaseRouteTemplateWithArea;
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
			string baseUrl = !string.IsNullOrEmpty(_baseUrl) ? _baseUrl : "/";
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

		private string GetActionTemplate(MvcAction actionInfo)
		{
			return actionInfo.RequestMethod.GetActionTemplate(actionInfo);
		}

		public static MvcRouteGenerator CreateGenerator(MvcController controller, string baseUrl)
		{
			return new AspNetCoreRouteGenerator(controller, baseUrl);
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
