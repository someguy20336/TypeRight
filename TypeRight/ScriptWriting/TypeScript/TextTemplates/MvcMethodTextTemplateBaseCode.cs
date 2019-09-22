using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.Configuration;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting.TypeScript.TextTemplates
{
	partial class MvcMethodTextTemplateBase
	{
		/// <summary>
		/// The function named used for "sucess"
		/// </summary>
		private const string SuccessFuncName = "success";
		/// <summary>
		/// The function name used for "fail"
		/// </summary>
		private const string FailFuncName = "fail";

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

		protected void AddBaseText(string indent)
		{
			StringBuilder cache = GenerationEnvironment;
			//CurrentIndent
			GenerationEnvironment = null;
			cache.Append(TransformText());
			GenerationEnvironment = cache;
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

		/// <summary>
		/// Builds the aja
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		private string BuildWebServiceParams(ControllerActionModel action)
		{
			var bodyParams = action.Parameters.Where(p => p.ActionParameterSourceType == ActionParameterSourceType.Body).ToList();

			if (bodyParams.Count == 0)
			{
				return "{}";
			}
			else if (bodyParams.Count == 1 && Context.ModelBinding == ModelBindingType.SingleParam)	// TODO: can i not?
			{
				// If we are only using a single parameter model binding (i.e. asp.net core), then the object itself should be the body
				return bodyParams[0].Name;
			}
			else
			{
				IEnumerable<string> multiParam = bodyParams.Select(p => $"{p.Name}: {p.Name}");  // Transform to param1: param1
				return $"{{ {string.Join(", ", multiParam)} }}";
			}
		}

		/// <summary>
		/// Builds the additional parameters
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		private string BuildAddlParameters(ControllerActionModel action)
		{
			var addlParams = action.Parameters.Where(p => p.ActionParameterSourceType == ActionParameterSourceType.Fetch).ToList();
			if (addlParams.Count == 0)
			{
				return "";
			}
			return $", {string.Join(", ", addlParams.Select(p => p.Name)) }";
		}

		/// <summary>
		/// Gets the URL for an action
		/// </summary>
		/// <param name="action">The action</param>
		/// <returns>The URL</returns>
		private string GetUrl(ControllerActionModel action)
		{
			var urlParams = action.Parameters.Where(p => p.ActionParameterSourceType == ActionParameterSourceType.Url).ToList();

			string urlParamQuery = "";
			if (urlParams.Count > 0)
			{
				// TODO: need to escape?
				urlParamQuery = "?" + string.Join("&", urlParams.Select(p => $"{p.Name}=${{{ p.Name}}}"));
			}
			return $"`{action.BaseUrl}{urlParamQuery}`";
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
