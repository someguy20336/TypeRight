using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using TypeRight.ScriptWriting.TypeScript.ScriptExtensions;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting.TypeScript
{
	internal class ScriptExtensionsFactory
	{
		private readonly NameValueCollection _constantQueryParams;

		public ScriptExtensionsFactory(NameValueCollection constantQueryParams)
		{
			_constantQueryParams = constantQueryParams ?? new NameValueCollection();
		}

		public IEnumerable<IScriptExtension> CreatePostControllerScript(ControllerContext context)
		{
			List<IScriptExtension> exts = new List<IScriptExtension>();
			if (context.Controllers.Any(c => HasAnyQueryParmeter(c)))
			{
				bool hasComplex = context.Controllers.Any(c => HasAnyComplexQueryParmeter(c));
				exts.Add(new AddQueryParamHelperFunctionsScriptExtension(hasComplex));
			}

			return exts;
		}

		public IEnumerable<IScriptExtension> CreateForActionFunctionBody(MvcAction action)
		{
			List<IScriptExtension> exts = new List<IScriptExtension>();

			bool hasQueryParam = HasAnyQueryParmeter(action);
			if (hasQueryParam)
			{
				exts.Add(new InitUrlParamsScriptExtensions());

				foreach (var queryP in GetSimpleQueryParams(action))
				{
					exts.Add(new AddSimpleParameterToQueryStringScriptExtension(queryP.Name));
				}

				foreach (string key in _constantQueryParams.Keys)
				{
					foreach (var val in _constantQueryParams.GetValues(key))
					{
						exts.Add(new AddKeyValueToQueryStringScriptExtension(key, val));
					}
				}

				foreach (var queryP in GetComplexQueryParams(action))
				{
					exts.Add(new AddComplexParameterToQueryStringScriptExtension(queryP.Name));
				}

			}

			return exts;
		}

		private bool HasAnyQueryParmeter(MvcController controller)
			=> controller.Actions.Any(a => HasAnyQueryParmeter(a));

		private bool HasAnyQueryParmeter(MvcAction action)
			=> _constantQueryParams.Count > 0 ||  action.ActionParameters.Any(p => p.BindingType == ActionParameterSourceType.Query);

		private bool HasAnyComplexQueryParmeter(MvcController controller)
			=> controller.Actions.Any(a => HasAnyComplexQueryParmeter(a));

		private bool HasAnyComplexQueryParmeter(MvcAction action)
			=> GetComplexQueryParams(action).Any();

		private IEnumerable<MvcActionParameter> GetComplexQueryParams(MvcAction action)
			=> action.ActionParameters.Where(p => p.BindingType == ActionParameterSourceType.Query && p.Types.Any(t => t.IsComplexType()));

		private IEnumerable<MvcActionParameter> GetSimpleQueryParams(MvcAction action)
			=> action.ActionParameters.Where(p => p.BindingType == ActionParameterSourceType.Query && p.Types.Any(t => !t.IsComplexType()));
	}
}
