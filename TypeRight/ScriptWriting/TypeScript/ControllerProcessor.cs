using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypeRight.Configuration;
using TypeRight.TypeFilters;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting.TypeScript
{
	public class ControllerProcessor
	{
		private TypeFormatter _typeFormatter;
		private ControllerContext _context;
		public Dictionary<string, ImportStatement> Imports { get; } = new Dictionary<string, ImportStatement>();
		private MvcControllerInfo _controllerInfo;

		public ControllerProcessor(MvcControllerInfo controllerInfo, ControllerContext context)
		{
			_context = context;
			_controllerInfo = controllerInfo;
			CompileImports();
		}

		public ControllerModel CreateModel(TypeFormatter formatter)
		{
			_typeFormatter = formatter;
			ControllerModel controllerModel = new ControllerModel();

			controllerModel.Name = _controllerInfo.Name;
			controllerModel.Actions = _controllerInfo.Actions.Select(ac => CreateActionModel(ac));
			controllerModel.Imports = Imports.Values;
			return controllerModel;
		}

		private ControllerActionModel CreateActionModel(MvcActionInfo actionInfo)
		{
			FetchFunctionDescriptor fetchDescriptor = _context.FetchFunctionResolver.Resolve(actionInfo);
			
			var fetchParameters = fetchDescriptor.AdditionalParameters.Select(p => new ActionParameterModel()
			{
				ActionParameterSourceType = ActionParameterSourceType.Fetch,
				Name = p.Name,
				Comments = "",
				ParameterType = ReplaceTokens(p.Type, actionInfo),
				IsOptional = p.Optional
			});
			var parameters = actionInfo.Parameters.Select(param => CreateActionParameterModel(param)).Union(fetchParameters);
			return new ControllerActionModel()
			{
				BaseUrl = _controllerInfo.GetBaseUrl() + actionInfo.Name,
				SummaryComments = actionInfo.SummaryComments,
				ReturnsComments = actionInfo.ReturnsComments,
				ParameterComments = actionInfo.ParameterComments,
				FetchFunctionName = fetchDescriptor.FunctionName,
				Parameters = parameters,
				Name = actionInfo.Name,
				ReturnType = ReplaceTokens(fetchDescriptor.ReturnType, actionInfo),
				RequestMethod = actionInfo.RequestMethod
			};
		}

		private ActionParameterModel CreateActionParameterModel(MvcActionParameter actionParameter)
		{
			ActionParameterSourceType sourceType = ActionParameterSourceType.Body;

			if (_context.ModelBinding == ModelBindingType.SingleParam)
			{
				var bodyFilter = new ParameterHasAttributeFilter(new IsOfTypeFilter(MvcConstants.FromBodyAttributeFullName_AspNetCore));
				var queryFilter = new ParameterHasAttributeFilter(new IsOfTypeFilter(MvcConstants.FromQueryAttributeFullName_AspNetCore));

				if (bodyFilter.Evaluate(actionParameter))
				{
					sourceType = ActionParameterSourceType.Body;
				}
				else if (queryFilter.Evaluate(actionParameter))
				{
					sourceType = ActionParameterSourceType.Query;
				}
				else
				{
					sourceType = ActionParameterSourceType.Ignored;
				}
			}
			

			return new ActionParameterModel()
			{
				ActionParameterSourceType = sourceType,
				Name = actionParameter.Name,
				ParameterType = actionParameter.Type.FormatType(_typeFormatter)
			};
		}

		private void CompileImports()
		{
			foreach (MvcActionInfo actionInfo in _controllerInfo.Actions)
			{
				CompileActionImport(actionInfo);
			}
		}

		private void CompileActionImport(MvcActionInfo actionInfo)
		{
			FetchFunctionDescriptor fetchDescriptor = _context.FetchFunctionResolver.Resolve(actionInfo);

			string funcKey = "fetch-" + fetchDescriptor.FetchModulePath;
			if (!Imports.ContainsKey(funcKey))
			{				
				Imports.Add(funcKey, new ImportStatement(_context.OutputPath, fetchDescriptor.FetchModulePath, false));
			}
			ImportStatement ajaxImport = Imports[funcKey];
			ajaxImport.AddItem(fetchDescriptor.FunctionName);

			AddActionImports(actionInfo, fetchDescriptor.AdditionalImports);
		}

		/// <summary>
		/// Replaces any tokens
		/// </summary>
		/// <param name="typeStr"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		private string ReplaceTokens(string typeStr, MvcActionInfo action)
		{
			return typeStr.Replace("$returnType$", action.ReturnType.FormatType(_typeFormatter));
		}

		private void AddActionImports(MvcActionInfo action, IEnumerable<ImportDefinition> additionalImports)
		{
			TryAddImport(action.ReturnType);
			foreach (var param in action.Parameters)
			{
				TryAddImport(param.Type);
			}


			// Additional imports
			foreach (ImportDefinition def in additionalImports)
			{
				string importPath = PathUtils.ResolveRelativePath(_context.OutputPath, def.Path);

				string key = "custom" + importPath;
				if (!Imports.ContainsKey(key))
				{
					Imports.Add(key, new ImportStatement(_context.OutputPath, importPath, def.UseAlias));
				}

				ImportStatement statement = Imports[key];
				if (def.Items != null)
				{
					foreach (var item in def.Items)
					{
						statement.AddItem(item);
					}
				}

			}
		}

		private void TryAddImport(TypeDescriptor type)
		{
			TypeScriptHelper.TryAddToImports(Imports, type, _context.OutputPath);
		}

	}
}
