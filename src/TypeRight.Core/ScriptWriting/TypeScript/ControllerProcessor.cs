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
		public ImportManager Imports { get; private set; }
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
			controllerModel.Imports = Imports.GetImports();
			return controllerModel;
		}

		private ControllerActionModel CreateActionModel(MvcActionInfo actionInfo)
		{
			FetchFunctionDescriptor fetchDescriptor = _context.FetchFunctionResolver.Resolve(actionInfo);
			string routeTemplate = _controllerInfo.GetActionUrlTemplate(actionInfo);

			return new ControllerActionModel()
			{
				RouteTemplate = routeTemplate,
				SummaryComments = actionInfo.SummaryComments,
				ReturnsComments = actionInfo.ReturnsComments,
				ParameterComments = actionInfo.ParameterComments,
				FetchFunctionName = fetchDescriptor.FunctionName,
				Parameters = CompileParameters(actionInfo, fetchDescriptor, routeTemplate),
				Name = actionInfo.Name,
				ReturnType = ReplaceTokens(fetchDescriptor.ReturnType, actionInfo),
				RequestMethod = actionInfo.RequestMethod
			};
		}

		private IEnumerable<ActionParameterModel> CompileParameters(MvcActionInfo actionInfo, FetchFunctionDescriptor fetchDescriptor, string routeTemplate)
		{
			var fetchParameters = fetchDescriptor.AdditionalParameters.Select(p => new ActionParameterModel()
			{
				ActionParameterSourceType = ActionParameterSourceType.Fetch,
				Name = p.Name,
				Comments = "",
				ParameterType = ReplaceTokens(p.Type, actionInfo),
				IsOptional = p.Optional
			});


			var methodRequiredParameters = actionInfo.Parameters.Where(p => !p.IsOptional).Select(p => CreateActionParameterModel(actionInfo, p, routeTemplate));
			var userRequiredParameters = fetchParameters.Where(p => !p.IsOptional);
			var methodOptionalParameters = actionInfo.Parameters.Where(p => p.IsOptional).Select(p => CreateActionParameterModel(actionInfo, p, routeTemplate));
			var userOptionalParameters = fetchParameters.Where(p => p.IsOptional);

			return methodRequiredParameters.Union(userRequiredParameters).Union(methodOptionalParameters).Union(userOptionalParameters);
		}

		private ActionParameterModel CreateActionParameterModel(MvcActionInfo actionInfo, MvcActionParameter actionParameter, string routeTemplate)
		{
			ActionParameterSourceType sourceType = ActionParameterSourceType.Body;

			// Note - this isn't great, but is how it has always worked.
			// consider improving the asp.net stuff... or just cutting it out
			if (_controllerInfo.IsAspNetCore)
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
				else if (routeTemplate.Contains($"{{{actionParameter.Name}}}"))
				{
					sourceType = ActionParameterSourceType.Route;
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
				ParameterType = actionParameter.Type.FormatType(_typeFormatter),
				IsOptional = actionParameter.IsOptional
			};
		}

		private void CompileImports()
		{
			Imports = new ImportManager(_context.OutputPath);
			foreach (MvcActionInfo actionInfo in _controllerInfo.Actions)
			{
				CompileActionImport(actionInfo);
			}
		}

		private void CompileActionImport(MvcActionInfo actionInfo)
		{
			FetchFunctionDescriptor fetchDescriptor = _context.FetchFunctionResolver.Resolve(actionInfo);

			string funcKey = "fetch-" + fetchDescriptor.FetchModulePath;
			if (!Imports.ContainsImportPath(funcKey))
			{
				Imports.AddImport(funcKey, new ImportStatement(_context.OutputPath, fetchDescriptor.FetchModulePath, false));
			}
			ImportStatement ajaxImport = Imports.GetImportAtPath(funcKey);
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
			Imports.TryAddToImports(action.ReturnType);
			foreach (var param in action.Parameters)
			{
				Imports.TryAddToImports(param.Type);
			}


			// Additional imports
			foreach (ImportDefinition def in additionalImports)
			{
				string importPath = PathUtils.ResolveRelativePath(_context.OutputPath, def.Path);

				string key = "custom" + importPath;
				if (!Imports.ContainsImportPath(key))
				{
					Imports.AddImport(key, new ImportStatement(_context.OutputPath, importPath, def.UseAlias));
				}

				ImportStatement statement = Imports.GetImportAtPath(key);
				if (def.Items != null)
				{
					foreach (var item in def.Items)
					{
						statement.AddItem(item);
					}
				}

			}
		}

	}
}
