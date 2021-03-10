using System;
using System.Collections.Generic;
using System.Linq;
using TypeRight.Configuration;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting
{
	public class FetchFunctionResolver
	{
		private IEnumerable<ActionConfig> _actionConfigs;
		private readonly Uri _projUri;
			   
		public FetchFunctionResolver(Uri projUri, IEnumerable<ActionConfig> configOptions)
		{
			_actionConfigs = configOptions;
			_projUri = projUri;
		}

		public FetchFunctionDescriptor Resolve(MvcActionInfo actionInfo)
		{
			ActionConfig selected = null;
			selected = _actionConfigs.FirstOrDefault(ac => ac.Method.ToUpper() == actionInfo.RequestMethod.Name);
			selected = selected ?? _actionConfigs.First(ac => ac.Method.ToUpper() == RequestMethod.Default.Name);

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
					new ActionParameter() {Name = "success", Type = "(result: $returnType$) => void", Optional = true},
					new ActionParameter() {Name = "fail", Type = "(result: any) => void", Optional = true }
				};
			}

			string fetchModulePath = string.IsNullOrEmpty(actionConfig.FetchFilePath) ? null : new Uri(_projUri, actionConfig.FetchFilePath).LocalPath;

			return new FetchFunctionDescriptor()
			{
				AdditionalImports = actionConfig.Imports ?? new List<ImportDefinition>(),
				AdditionalParameters = addlParameters,
				FetchModulePath = fetchModulePath,
				FunctionName = actionConfig.FetchFunctionName,
				ReturnType = string.IsNullOrEmpty(actionConfig.ReturnType) ? "void" : actionConfig.ReturnType
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


	}
}
