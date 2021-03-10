using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

		/// <summary>
		/// Builds the aja
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		private string BuildWebServiceParams(ControllerActionModel action)
		{
			if (!action.RequestMethod.HasBody)
			{
				return "";
			}
			var bodyParams = action.Parameters.Where(p => p.ActionParameterSourceType == ActionParameterSourceType.Body).ToList();

			if (bodyParams.Count == 0)
			{
				return ", {}";
			}
			else if (bodyParams.Count > 1)
			{
				throw new InvalidOperationException("More than one body parameter is not supported: " + string.Join(", ", bodyParams.Select(b => b.Name)));
			}
			else
			{
				// If we are only using a single parameter model binding (i.e. asp.net core), then the object itself should be the body
				return ", " + bodyParams[0].Name;
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
			var urlParams = action.Parameters.Where(p => p.ActionParameterSourceType == ActionParameterSourceType.Query).ToList();
			
			NameValueCollection queryParams = new NameValueCollection(Context.QueryParameters);

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
