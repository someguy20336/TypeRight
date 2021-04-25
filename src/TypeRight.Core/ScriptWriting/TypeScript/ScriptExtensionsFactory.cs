using System.Collections.Generic;
using System.Linq;
using TypeRight.ScriptWriting.TypeScript.ScriptExtensions;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting.TypeScript
{
	internal static class ScriptExtensionsFactory
	{
		public static IEnumerable<IScriptExtension> CreatePostControllerScript(ControllerContext context)
		{
			List<IScriptExtension> exts = new List<IScriptExtension>();
			if (context.Controllers.Any(c => HasAnyQueryParmeter(c)))
			{
				bool hasComplex = context.Controllers.Any(c => HasAnyComplexQueryParmeter(c));
				exts.Add(new AddQueryParamHelperFunctionsScriptExtension(hasComplex));
			}

			return exts;
		}

		public static IEnumerable<IScriptExtension> CreateForActionFunctionBody(MvcAction action)
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

				foreach (var queryP in GetComplexQueryParams(action))
				{
					exts.Add(new AddComplexParameterToQueryStringScriptExtension(queryP.Name));
				}

				exts.Add(new AddStringUrlParamsScriptExtension());
			}



			return exts;
		}

		private static bool HasAnyQueryParmeter(MvcController controller)
			=> controller.Actions.Any(a => HasAnyQueryParmeter(a));

		private static bool HasAnyQueryParmeter(MvcAction action)
			=> action.Parameters.Any(p => p.BindingType == ActionParameterSourceType.Query);

		private static bool HasAnyComplexQueryParmeter(MvcController controller)
			=> controller.Actions.Any(a => HasAnyComplexQueryParmeter(a));

		private static bool HasAnyComplexQueryParmeter(MvcAction action)
			=> GetComplexQueryParams(action).Any();

		private static IEnumerable<MvcActionParameter> GetComplexQueryParams(MvcAction action)
			=> action.Parameters.Where(p => p.BindingType == ActionParameterSourceType.Query && p.Types.Any(t => t.IsComplexType()));

		private static IEnumerable<MvcActionParameter> GetSimpleQueryParams(MvcAction action)
			=> action.Parameters.Where(p => p.BindingType == ActionParameterSourceType.Query && p.Types.Any(t => !t.IsComplexType()));
	}
}
