using System.Collections.Generic;
using System.Linq;
using TypeRight.TypeFilters;
using TypeRight.TypeProcessing;

namespace TypeRight.ScriptWriting.TypeScript
{
	public class ControllerProcessor
	{
		private TypeFormatter _typeFormatter;
		private readonly ControllerContext _context;
		protected MvcControllerInfo ControllerInfo => _context.Controller;

		public ControllerProcessor(ControllerContext context)
		{
			_context = context;
		}

		public ControllerModel CreateModel(TypeFormatter formatter)
		{
			_typeFormatter = formatter;
			ControllerModel controllerModel = new ControllerModel
			{
				Name = ControllerInfo.Name,
				Actions = ControllerInfo.Actions.Select(ac => CreateActionModel(ac))
			};
			return controllerModel;
		}

		private ControllerActionModel CreateActionModel(MvcActionInfo actionInfo)
		{
			FetchFunctionDescriptor fetchDescriptor = _context.FetchFunctionResolver.Resolve(actionInfo.RequestMethod.Name);
			string routeTemplate = MvcRouteGenerator.CreateGenerator(_context).GenerateRouteTemplate(actionInfo);

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
			if (ControllerInfo.IsAspNetCore)
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
				ParameterType = string.Join(" | ", actionParameter.Types.Select(t => t.FormatType(_typeFormatter))),
				IsOptional = actionParameter.IsOptional
			};
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

	}
}
