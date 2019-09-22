using System;
using System.Collections.Generic;
using System.Text;
using TypeRight.Configuration;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting
{
	public class FetchFunctionResolver
	{
		private ActionConfig _options;
		private Uri _projUri;

		public FetchFunctionResolver(Uri projUri, ActionConfig configOptions)
		{
			_options = configOptions;
			_projUri = projUri;
		}

		public FetchFunctionDescriptor Resolve()
		{
			return ActionConfigToDescriptor(_options);
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
				AdditionalImports = actionConfig.Imports,
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
