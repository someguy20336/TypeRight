using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using TypeRight.Configuration;
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
				return new FetchConfigFetchFunctionResolver(projUri, options.FetchConfig, options.QueryParams, options.BaseUrl);
			}

			return new ActionConfigFetchFunctionResolver(projUri, options.ActionConfigurations, options.QueryParams, options.BaseUrl);
		}

		protected string ResolveFilePath(string fetchFilePath)
		{
			return string.IsNullOrEmpty(fetchFilePath) ? null : new Uri(ProjUri, fetchFilePath).LocalPath;
		}
	}


	public class ActionConfigFetchFunctionResolver : FetchFunctionResolver
	{
		private IEnumerable<ActionConfig> _actionConfigs;
		private readonly NameValueCollection _constantQueryParams;
		private readonly string _baseUrl;

		public ActionConfigFetchFunctionResolver(Uri projUri, IEnumerable<ActionConfig> configOptions, NameValueCollection constantQueryParams, string baseUrl)
			: base(projUri)
		{
			_actionConfigs = configOptions;
			_constantQueryParams = constantQueryParams ?? new NameValueCollection();
			_baseUrl = baseUrl;
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
					new ActionParameter("success", "(result: $returnType$) => void", true),
					new ActionParameter("fail", "(result: any) => void", true )
				};
			}

			List<IFetchParameterResolver> parameterResolvers = new List<IFetchParameterResolver>()
			{
				new UrlParameterResolver(_constantQueryParams, _baseUrl),
				new BodyParameterResolver()
			};

			foreach (var addlParam in addlParameters)
			{
				parameterResolvers.Add(new CustomParameterResolver(addlParam));
			}

			return new FetchFunctionDescriptor()
			{
				AdditionalImports = actionConfig.Imports ?? new List<ImportDefinition>(),
				AdditionalParameters = addlParameters,
				FetchModulePath = ResolveFilePath(actionConfig.FetchFilePath),
				FunctionName = actionConfig.FetchFunctionName,
				ReturnType = string.IsNullOrEmpty(actionConfig.ReturnType) ? "void" : actionConfig.ReturnType,
				FetchParameterResolvers = parameterResolvers
			};
		}
	}

	public class FetchConfigFetchFunctionResolver : FetchFunctionResolver
	{
		private FetchConfig _fetchConfig;
		private readonly NameValueCollection _constantQueryParams;
		private readonly string _baseUrl;

		public FetchConfigFetchFunctionResolver(Uri projUri, FetchConfig fetchConfig, NameValueCollection constantQueryParams, string baseUrl)
			: base(projUri)
		{
			_fetchConfig = fetchConfig;
			_constantQueryParams = constantQueryParams;
			_baseUrl = baseUrl;
		}

		public override FetchFunctionDescriptor Resolve(string requestMethodName)
		{

			List<IFetchParameterResolver> parameterResolvers = _fetchConfig.Parameters.Select<ActionParameter, IFetchParameterResolver>(p =>
			{
				switch (p.Kind)
				{
					case ParameterKind.RequestMethod:
						return new RequestMethodResolver();
					case ParameterKind.Url:
						return new UrlParameterResolver(_constantQueryParams, _baseUrl);
					case ParameterKind.Body:
						return new BodyParameterResolver();
					case ParameterKind.Custom:
					default:
						return new CustomParameterResolver(p);
				}
			}).ToList();

			return new FetchFunctionDescriptor()
			{
				AdditionalImports = _fetchConfig.Imports ?? new List<ImportDefinition>(),
				AdditionalParameters = _fetchConfig.Parameters.Where(p => p.Kind == ParameterKind.Custom).ToList<ActionParameter>(),
				FetchModulePath = ResolveFilePath(_fetchConfig.FilePath),
				FunctionName = _fetchConfig.Name,
				ReturnType = string.IsNullOrEmpty(_fetchConfig.ReturnType) ? "void" : _fetchConfig.ReturnType,
				FetchParameterResolvers = parameterResolvers
			};
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

}
