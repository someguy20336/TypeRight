using System;
using System.Collections.Specialized;
using System.Linq;
using TypeRight.Configuration;
using TypeRight.ScriptWriting.TypeScript;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting
{

	public interface IFetchParameterResolver
	{
		string ResolveParameter(MvcActionInfo action);
	}

	public class UrlParameterResolver : IFetchParameterResolver
	{
		private readonly NameValueCollection _constantQueryParams;
		private readonly string _baseUrl;

		public UrlParameterResolver(NameValueCollection constantQueryParams, string baseUrl)
		{
			_constantQueryParams = constantQueryParams;
			_baseUrl = baseUrl;
		}

		public string ResolveParameter(MvcActionInfo action)
		{
			var urlParams = action.Parameters.Where(p => p.BindingType == ActionParameterSourceType.Query).ToList();

			NameValueCollection queryParams = new NameValueCollection(_constantQueryParams);

			foreach (var p in urlParams)
			{
				queryParams.Add(p.Name, $"${{ { p.Name} ?? \"\" }}");
			}

			string urlParamQuery = queryParams.ToQueryString();

			// Add the route params
			string route = action.GetRouteTemplate(_baseUrl);
			var routeParamNames = action.Parameters.Where(p => p.BindingType == ActionParameterSourceType.Route).Select(p => p.Name);
			foreach (string paramName in routeParamNames)
			{
				route = route.Replace($"{{{paramName}}}", $"${{{paramName}}}");
			}

			return $"`{route}{urlParamQuery}`";
		}

	}

	public class RequestMethodResolver : IFetchParameterResolver
	{
		public string ResolveParameter(MvcActionInfo action) => $"\"{action.RequestMethod.MethodName}\"";
	}

	public class BodyParameterResolver : IFetchParameterResolver
	{
		public string ResolveParameter(MvcActionInfo action)
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

	public class CustomParameterResolver : IFetchParameterResolver
	{
		private readonly ActionParameter _parameter;

		public CustomParameterResolver(ActionParameter parameter)
		{
			_parameter = parameter;
		}
		public string ResolveParameter(MvcActionInfo action) => _parameter.Name;
	}
}
