using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

				foreach (var queryP in action.Parameters.Where(p => p.BindingType == ActionParameterSourceType.Query))
				{
					exts.Add(new AddSimpleParameterToQueryStringScriptExtension(queryP.Name));
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
			=> action.Parameters.Any(p => p.BindingType == ActionParameterSourceType.Query);
	}
}
