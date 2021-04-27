using System;
using System.Collections.Specialized;
using System.Linq;
using TypeRight.Configuration;
using TypeRight.ScriptWriting.TypeScript.PartialTextTemplates;
using TypeRight.ScriptWriting.TypeScript.ScriptExtensions;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting
{

	public interface IFetchParameterResolver
	{
		string ResolveParameter(MvcAction action);
	}

	internal class UrlParameterResolver : IFetchParameterResolver
	{
		private readonly NameValueCollection _constantQueryParams;
		private readonly string _baseUrl;

		public UrlParameterResolver(NameValueCollection constantQueryParams, string baseUrl)
		{
			_constantQueryParams = constantQueryParams;
			_baseUrl = baseUrl;
		}

		public string ResolveParameter(MvcAction action)
		{
			var actionQueryParams = action.Parameters.Where(p => p.BindingType == ActionParameterSourceType.Query).ToList();

			string urlParamQueryPart = actionQueryParams.Count > 0 || _constantQueryParams.Count > 0
				? $"${{{QueryParameterHelperFunctions.GetQueryStringFuncName}({InitUrlParamsScriptExtensions.UrlParamsVarName})}}"
				: ""; ;

			// Add the route params
			string route = action.GetRouteTemplate(_baseUrl);
			var routeParamNames = action.Parameters.Where(p => p.BindingType == ActionParameterSourceType.Route).Select(p => p.Name);
			foreach (string paramName in routeParamNames)
			{
				route = route.Replace($"{{{paramName}}}", $"${{{paramName}}}");
			}

			return $"`{route}{urlParamQueryPart}`";
		}

	}

	internal class RequestMethodResolver : IFetchParameterResolver
	{
		public string ResolveParameter(MvcAction action) => $"\"{action.RequestMethod.MethodName}\"";
	}

	internal class BodyParameterResolver : IFetchParameterResolver
	{
		public string ResolveParameter(MvcAction action)
		{
			if (!action.RequestMethod.HasBody)
			{
				return "null";
			}
			var bodyParams = action.Parameters.Where(p => p.BindingType == ActionParameterSourceType.Body).ToList();

			if (bodyParams.Count == 0)
			{
				return "null";
			}
			else if (bodyParams.Count > 1)
			{
				throw new InvalidOperationException("More than one body parameter is not supported: " + string.Join(", ", bodyParams.Select(b => b.Name)));
			}
			else
			{
				// If we are only using a single parameter model binding (i.e. asp.net core), then the object itself should be the body
				return bodyParams[0].Name;
			}
		}
	}

	internal class CustomParameterResolver : IFetchParameterResolver
	{
		private readonly ActionParameter _parameter;

		public CustomParameterResolver(ActionParameter parameter)
		{
			_parameter = parameter;
		}
		public string ResolveParameter(MvcAction action) => _parameter.Name;
	}
}
