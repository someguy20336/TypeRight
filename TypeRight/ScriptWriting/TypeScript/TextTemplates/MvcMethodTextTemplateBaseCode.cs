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

		protected MvcControllerInfo ControllerInfo { get; private set; }

		protected ControllerContext Context { get; private set; }

		/// <summary>
		/// Gets the name of the controller, without the "Controller" part
		/// </summary>
		public string ControllerName { get; private set; }

		/// <summary>
		/// Gets whether this package has its own Ajax function
		/// </summary>
		public bool HasOwnAjaxFunction { get; private set; }

		/// <summary>
		/// Gets the name of the ajax function to use
		/// </summary>
		public string FetchFunctionName { get; private set; }

		/// <summary>
		/// The base URL for all actions
		/// </summary>
		public string BaseActionUrl { get; private set; }

		public TypeFormatter TypeFormatter { get; private set; }

		public void Initialize(MvcControllerInfo controllerInfo, ControllerContext context, TypeFormatter formatter)
		{
			ControllerInfo = controllerInfo;
			TypeFormatter = formatter;
			Context = context;

			if (context.HasOwnAjaxFunction)
			{
				HasOwnAjaxFunction = true;
				FetchFunctionName = context.FetchFunctionName;
			}
			else
			{
				FetchFunctionName = "callService";
			}

			ControllerName = ControllerInfo.Name.Substring(0, ControllerInfo.Name.Length - "Controller".Length);

			FileInfo fileInfo = new FileInfo(ControllerInfo.FilePath);
			DirectoryInfo controllerDir = fileInfo.Directory;

			// Check if this controller is in an "Area"
			bool foundAreas = false;
			DirectoryInfo dir = controllerDir;
			DirectoryInfo areaDir = null;  // if areas is found, this is the specific area directory (like "Admin", or "Shared", etc)
			while (dir != null)
			{
				if (dir.Name == "Areas")
				{
					foundAreas = true;
					break;
				}
				areaDir = dir;
				dir = dir.Parent;
			}

			if (foundAreas)
			{
				// Area/ControllerName/Action
				BaseActionUrl = $"/{areaDir.Name}/{ControllerName}/";
			}
			else
			{
				// ControllerName/Action
				BaseActionUrl = $"/{ControllerName}/";
			}

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
		private IEnumerable<MvcActionInfo> GetActions() => ControllerInfo.Actions.OrderBy(act => act.Name);

		/// <summary>
		/// Builds the fetch function name, including the return keyword if necessary
		/// </summary>
		/// <param name="actionInfo"></param>
		/// <returns></returns>
		private string BuildFetchFunctionName(MvcActionInfo actionInfo)
		{
			if (Context.FetchReturnType == "void")
			{
				return FetchFunctionName;
			}
			else
			{
				return "return " + FetchFunctionName;
			}
		}

		/// <summary>
		/// Builds the action signature
		/// </summary>
		/// <param name="action">The action to build the signature for</param>
		/// <returns>The string signature</returns>
		private string BuildActionSignature(MvcActionInfo action)
		{
			List<string> actionParams = new List<string>();

			// Build parameters
			foreach (MvcActionParameter oneParam in GetParameters(action))
			{
				string paramText = $"{oneParam.Name}: {oneParam.Type.FormatType(TypeFormatter)}";
				actionParams.Add(paramText);
			}

			// Add user defined params
			foreach (var addlParam in Context.AdditionalParameters)
			{
				string retType = addlParam.Type.Replace("$returnType$", action.ReturnType.FormatType(TypeFormatter));
				string paramText = $"{addlParam.Name}{ (addlParam.Optional ? "?" : "") }: { retType }";
				actionParams.Add(paramText);
			}

			return $"{action.Name}({string.Join(", ", actionParams)}): { ReplaceTokens(Context.FetchReturnType, action) }";
		}

		/// <summary>
		/// Builds the aja
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		private string BuildWebServiceParams(MvcActionInfo action)
		{
			if (Context.ModelBinding == ModelBindingType.SingleParam)
			{
				// If we are only using a single parameter model binding (i.e. asp.net core), then the object itself should be the body
				MvcActionParameter firstFromBody = action.Parameters.FirstOrDefault(p => Context.MvcParameterFilter.Evaluate(p));
				return firstFromBody?.Name ?? "{ }";
			}
			else
			{
				IEnumerable<string> multiParam = action.Parameters.Select(p => $"{p.Name}: {p.Name}");  // Transform to param1: param1
				return $"{{ {string.Join(", ", multiParam)} }}";
			}
		}

		/// <summary>
		/// Builds the additional parameters
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		private string BuildAddlParameters(MvcActionInfo action)
		{
			if (Context.AdditionalParameters.Count == 0)
			{
				return "";
			}
			return $", {string.Join(", ", Context.AdditionalParameters.Select(p => ReplaceTokens(p.Name, action))) }";
		}

		/// <summary>
		/// Gets the URL for an action
		/// </summary>
		/// <param name="action">The action</param>
		/// <returns>The URL</returns>
		private string GetUrl(MvcActionInfo action)
		{
			return $"{BaseActionUrl}{action.Name}";
		}

		/// <summary>
		/// Replaces any tokens
		/// </summary>
		/// <param name="typeStr"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		private string ReplaceTokens(string typeStr, MvcActionInfo action)
		{
			return typeStr.Replace("$returnType$", action.ReturnType.FormatType(TypeFormatter));
		}

		/// <summary>
		/// Gets the key value pairs of parameters and comments for this action
		/// </summary>
		/// <param name="action">The action</param>
		/// <returns></returns>
		private IEnumerable<KeyValuePair<string, string>> GetParameterComments(MvcActionInfo action)
		{
			// Get the params that should actually be written
			HashSet<string> allParams = new HashSet<string>(GetParameters(action).Select(p => p.Name));

			return action.ParameterComments.Where(kv => allParams.Contains(kv.Key));
		}

		/// <summary>
		/// Gets the parameters that should actually be written to the script
		/// </summary>
		/// <param name="action">The action</param>
		/// <returns>The enumerable list of parameters</returns>
		private IEnumerable<MvcActionParameter> GetParameters(MvcActionInfo action)
		{
			return action.Parameters.Where(p =>
				Context.ModelBinding == ModelBindingType.MultiParam		// Either we are in multiparam mode
				|| Context.MvcParameterFilter.Evaluate(p)				// Or the parameter filter allows it
				);
		}
	}
}
