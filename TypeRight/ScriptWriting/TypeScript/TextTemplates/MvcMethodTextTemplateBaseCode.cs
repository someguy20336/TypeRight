using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		protected ScriptWriteContext Context { get; private set; }

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
		public string AjaxFunctionName { get; private set; }

		/// <summary>
		/// The base URL for all actions
		/// </summary>
		public string BaseActionUrl { get; private set; }

		public TypeFormatter TypeFormatter { get; private set; }

		public void Initialize(MvcControllerInfo controllerInfo, ScriptWriteContext context, TypeFormatter formatter)
		{
			ControllerInfo = controllerInfo;
			TypeFormatter = formatter;
			Context = context;

			if (!string.IsNullOrEmpty(context.AjaxFunctionName))
			{
				HasOwnAjaxFunction = true;
				AjaxFunctionName = context.AjaxFunctionName;
			}
			else
			{
				AjaxFunctionName = "callService";
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
				BaseActionUrl = $"{areaDir.Name}/{ControllerName}/";
			}
			else
			{
				// ControllerName/Action
				BaseActionUrl = $"{ControllerName}/";
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
		/// Builds the action signature
		/// </summary>
		/// <param name="action">The action to build the signature for</param>
		/// <returns>The string signature</returns>
		private string BuildActionSignature(MvcActionInfo action)
		{
			List<string> actionParams = new List<string>();

			// Build parameters
			foreach (MvcActionParameter oneParam in action.Parameters)
			{
				string paramText = $"{oneParam.Name}: {oneParam.Type.FormatType(TypeFormatter)}";
				actionParams.Add(paramText);
			}

			// Add success and fail functions
			actionParams.Add($"{SuccessFuncName}?: (result: {action.ReturnType.FormatType(TypeFormatter)}) => void");
			actionParams.Add($"{FailFuncName}?: (result: any) => void");
			return $"{action.Name}({string.Join(", ", actionParams)}): void";
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

	}
}
