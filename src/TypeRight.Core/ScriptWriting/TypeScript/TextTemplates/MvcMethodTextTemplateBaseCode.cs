using System.Collections.Generic;
using System.Linq;

namespace TypeRight.ScriptWriting.TypeScript.TextTemplates
{
	partial class MvcMethodTextTemplateBase
	{
		protected ControllerModel ControllerInfo { get; private set; }

		protected ControllerContext Context { get; private set; }

		/// <summary>
		/// Gets the name of the controller, without the "Controller" part
		/// </summary>
		public string ControllerName => ControllerInfo.Name;

		public void Initialize(ControllerModel model, ControllerContext context)
		{
			ControllerInfo = model;
			Context = context;
		}

		/// <summary>
		/// Gets a list of all actions ordered by name
		/// </summary>
		/// <returns>An enumerable list of actions</returns>
		private IEnumerable<ControllerActionModel> GetActions() => ControllerInfo.Actions.OrderBy(act => act.Name);

		/// <summary>
		/// Builds the fetch function name, including the return keyword if necessary
		/// </summary>
		/// <param name="actionInfo"></param>
		/// <returns></returns>
		private string BuildFetchFunctionName(ControllerActionModel actionInfo)
		{
			if (actionInfo.ReturnType == "void")
			{
				return actionInfo.FetchFunctionName;
			}
			else
			{
				return "return " + actionInfo.FetchFunctionName;
			}
		}

		/// <summary>
		/// Builds the action signature
		/// </summary>
		/// <param name="action">The action to build the signature for</param>
		/// <returns>The string signature</returns>
		private string BuildActionSignature(ControllerActionModel action)
		{
			List<string> actionParams = new List<string>();

			// Build parameters
			foreach (ActionParameterModel oneParam in action.Parameters.Where(p => p.ActionParameterSourceType != ActionParameterSourceType.Ignored))
			{
				string paramText = $"{oneParam.Name}{ (oneParam.IsOptional ? "?" : "") }: {oneParam.ParameterType}";
				actionParams.Add(paramText);
			}

			return $"{action.Name}({string.Join(", ", actionParams)}): { action.ReturnType }";
		}

		private string BuildFetchParameters(ControllerActionModel action)
		{
			FetchFunctionDescriptor fetchFunc = Context.FetchFunctionResolver.Resolve(action.RequestMethod.Name);
			var allParams = fetchFunc.FetchParameterResolvers.Select(pr => pr.ResolveParameter(action));
			return string.Join(", ", allParams);
		}

		/// <summary>
		/// Gets the key value pairs of parameters and comments for this action
		/// </summary>
		/// <param name="action">The action</param>
		/// <returns></returns>
		private IEnumerable<KeyValuePair<string, string>> GetParameterComments(ControllerActionModel action)
		{
			// Get the params that should actually be written
			HashSet<string> allParams = new HashSet<string>(action.Parameters
				.Where(p => p.ActionParameterSourceType != ActionParameterSourceType.Ignored)
				.Select(p => p.Name)
				);

			return action.ParameterComments.Where(kv => allParams.Contains(kv.Key));
		}
	}
}
