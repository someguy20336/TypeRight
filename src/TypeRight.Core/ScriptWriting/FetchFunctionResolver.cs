using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using TypeRight.Configuration;
using TypeRight.ScriptWriting.TypeScript;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting
{
	public abstract class FetchFunctionResolver
	{
		protected Uri ProjUri { get; }
			   
		public FetchFunctionResolver(Uri projUri)
		{
			ProjUri = projUri;
		}

		public abstract FetchFunctionDescriptor Resolve(string requestMethodName);
		

		public static FetchFunctionResolver FromConfig(Uri projUri, ConfigOptions options)
		{
			if (options.FetchConfig != null)
			{
				return new FetchConfigFetchFunctionResolver(projUri, options.FetchConfig);
			}

			return new ActionConfigFetchFunctionResolver(projUri, options.ActionConfigurations, options.QueryParams);
		}
	}


	public class ActionConfigFetchFunctionResolver : FetchFunctionResolver
	{
		private IEnumerable<ActionConfig> _actionConfigs;
		private readonly NameValueCollection _constantQueryParams;

		public ActionConfigFetchFunctionResolver(Uri projUri, IEnumerable<ActionConfig> configOptions, NameValueCollection constantQueryParams = null)
			: base(projUri)
		{
			_actionConfigs = configOptions;
			_constantQueryParams = constantQueryParams ?? new NameValueCollection();
		}

		public override FetchFunctionDescriptor Resolve(string requestMethodName)
		{
			ActionConfig selected = null;
			selected = _actionConfigs.FirstOrDefault(ac => ac.Method.Equals(requestMethodName, StringComparison.OrdinalIgnoreCase));
			selected = selected ?? _actionConfigs.First(ac => ac.Method.Equals(RequestMethod.Default.Name, StringComparison.OrdinalIgnoreCase));

			return ActionConfigToDescriptor(selected);
		}

		private FetchFunctionDescriptor ActionConfigToDescriptor(ActionConfig actionConfig)
		{
			// Default Addl Params
			List<ActionParameter> addlParameters = actionConfig.Parameters;
			if (addlParameters == null)
			{
				addlParameters = new List<ActionParameter>()
				{
					new ActionParameter() {Name = "success", Type = "(result: $returnType$) => void", Optional = true},
					new ActionParameter() {Name = "fail", Type = "(result: any) => void", Optional = true }
				};
			}

			List<IFetchParameterResolver> parameterResolvers = new List<IFetchParameterResolver>()
			{
				new UrlParameterResolver(_constantQueryParams),
				new BodyParameterResolver()
			};

			foreach (var addlParam in addlParameters)
			{
				parameterResolvers.Add(new CustomParameterResolver(addlParam));
			}

			string fetchModulePath = string.IsNullOrEmpty(actionConfig.FetchFilePath) ? null : new Uri(ProjUri, actionConfig.FetchFilePath).LocalPath;

			return new FetchFunctionDescriptor()
			{
				AdditionalImports = actionConfig.Imports ?? new List<ImportDefinition>(),
				AdditionalParameters = addlParameters,
				FetchModulePath = fetchModulePath,
				FunctionName = actionConfig.FetchFunctionName,
				ReturnType = string.IsNullOrEmpty(actionConfig.ReturnType) ? "void" : actionConfig.ReturnType,
				FetchParameterResolvers = parameterResolvers
			};
		}
	}

	public class FetchConfigFetchFunctionResolver : FetchFunctionResolver
	{
		private FetchConfig _fetchConfig;

		public FetchConfigFetchFunctionResolver(Uri projUri, FetchConfig fetchConfig)
			: base(projUri)
		{
			_fetchConfig = fetchConfig;
		}

		public override FetchFunctionDescriptor Resolve(string requestMethodName)
		{
			return null;
		}

	}

	public class FetchFunctionDescriptor
	{
		public string FetchModulePath { get; internal set; }

		public string FunctionName { get; internal set; }

		public string ReturnType { get; internal set; }

		public List<ActionParameter> AdditionalParameters { get; internal set; }

		public List<ImportDefinition> AdditionalImports { get; internal set; }

		public IEnumerable<IFetchParameterResolver> FetchParameterResolvers { get; internal set; }
	}

	public interface IFetchParameterResolver
	{
		string ResolveParameter(ControllerActionModel action);
	}

	public class UrlParameterResolver : IFetchParameterResolver
	{
		private readonly NameValueCollection _constantQueryParams;

		public UrlParameterResolver(NameValueCollection constantQueryParams)
		{
			_constantQueryParams = constantQueryParams;
		}

		public string ResolveParameter(ControllerActionModel action)
		{
			var urlParams = action.Parameters.Where(p => p.ActionParameterSourceType == ActionParameterSourceType.Query).ToList();

			NameValueCollection queryParams = new NameValueCollection(_constantQueryParams);

			foreach (var p in urlParams)
			{
				queryParams.Add(p.Name, $"${{ { p.Name} ?? \"\" }}");
			}

			string urlParamQuery = queryParams.ToQueryString();

			// Add the route params
			string route = action.RouteTemplate;
			var routeParamNames = action.Parameters.Where(p => p.ActionParameterSourceType == ActionParameterSourceType.Route).Select(p => p.Name);
			foreach (string paramName in routeParamNames)
			{
				route = route.Replace($"{{{paramName}}}", $"${{{paramName}}}");
			}

			return $"`{route}{urlParamQuery}`";
		}
		
	}

	public class RequestMethodResolver : IFetchParameterResolver
	{
		public string ResolveParameter(ControllerActionModel action) => $"\"{action.RequestMethod.Name}\"";
	}

	public class BodyParameterResolver : IFetchParameterResolver
	{
		public string ResolveParameter(ControllerActionModel action)
		{
			if (!action.RequestMethod.HasBody)
			{
				return "null";  // TODO: maybe return "null" or "undefined"
			}
			var bodyParams = action.Parameters.Where(p => p.ActionParameterSourceType == ActionParameterSourceType.Body).ToList();

			if (bodyParams.Count == 0)
			{
				return "{}";
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
		public string ResolveParameter(ControllerActionModel action) => _parameter.Name;
	}
}
