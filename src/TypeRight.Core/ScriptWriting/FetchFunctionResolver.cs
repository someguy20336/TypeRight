using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using TypeRight.Configuration;

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
			return new FetchConfigFetchFunctionResolver(projUri, options.FetchConfig, options.QueryParams, options.BaseUrl);
		}

		protected string ResolveFilePath(string fetchFilePath)
		{
			return string.IsNullOrEmpty(fetchFilePath) ? null : new Uri(ProjUri, fetchFilePath).LocalPath;
		}
	}

	internal class FetchConfigFetchFunctionResolver : FetchFunctionResolver
	{
		private FetchConfig _fetchConfig;
		private readonly NameValueCollection _constantQueryParams;
		private readonly string _baseUrl;

		public FetchConfigFetchFunctionResolver(Uri projUri, FetchConfig fetchConfig, NameValueCollection constantQueryParams, string baseUrl)
			: base(projUri)
		{
			_fetchConfig = fetchConfig;
			_constantQueryParams = constantQueryParams ?? new NameValueCollection();
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
				AdditionalParameters = _fetchConfig.Parameters.Where(p => p.Kind == ParameterKind.Custom).ToList(),
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
