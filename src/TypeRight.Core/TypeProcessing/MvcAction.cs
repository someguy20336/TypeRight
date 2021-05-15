using TypeRight.CodeModel;
using System.Collections.Generic;
using System.Linq;
using TypeRight.TypeFilters;
using TypeRight.Attributes;
using TypeRight.ScriptWriting;

namespace TypeRight.TypeProcessing
{

	public enum ActionParameterSourceType
	{
		Query,
		Body,
		Fetch,
		Route,
		Ignored
	}

	/// <summary>
	/// An object that contains information about an MVC action
	/// </summary>
	public class MvcAction
	{

		private IRequestMethod _method;
		internal MvcController Controller { get; }

		/// <summary>
		/// Gets the method behind this action info
		/// </summary>
		public IMethod Method { get; private set; }

		/// <summary>
		/// Gets the name of the action
		/// </summary>
		public string Name => Method.Name;

		/// <summary>
		/// Gets the name to use for the script
		/// </summary>
		public string ScriptName { get; }

		/// <summary>
		/// Gets the action summary comments
		/// </summary>
		public string SummaryComments => Method.SummaryComments;

		/// <summary>
		/// Gets the parameter comments in an index of parameter name description
		/// </summary>
		public IReadOnlyDictionary<string, string> ParameterComments { get; private set; }

		/// <summary>
		/// Gets the "returns" comments
		/// </summary>
		public string ReturnsComments => Method.ReturnsComments;

		/// <summary>
		/// Gets the return type of the action
		/// </summary>
		public TypeDescriptor ReturnType { get; }

		/// <summary>
		/// Gets the list of MVC action parameters
		/// </summary>
		public IReadOnlyList<MvcActionParameter> Parameters { get; }

		/// <summary>
		/// Gets attributes for this action parameter
		/// </summary>
		public IEnumerable<IAttributeData> Attributes => Method.Attributes;

		public IRequestMethod RequestMethod
		{
			get
			{
				if (_method == null)
				{
					foreach (IRequestMethod testMethod in TypeProcessing.RequestMethod.RequestMethods)
					{
						if (testMethod.ActionFilter.Evaluate(this))
						{
							_method = testMethod;
							break;
						}
					}
				}

				return _method;
			}
		}



		/// <summary>
		/// Creates a new action info from the given method
		/// </summary>
		/// <param name="method">the method</param>
		/// <param name="typeFactory">The type table</param>
		internal MvcAction(MvcController controller, IMethod method, TypeFactory typeFactory)
		{
			Controller = controller;
			Method = method;
			ScriptName = GetScriptName(method, typeFactory.Settings.NamingStrategy);
			ReturnType = typeFactory.LookupType(method.ReturnType);
			ParameterComments = method.Parameters.ToDictionary(param => param.Name, param => param.Comments);
			Parameters = method.Parameters.Select(p => new MvcActionParameter(this, p, typeFactory)).ToList().AsReadOnly();
		}

		public string GetRouteTemplate(string baseUrl = "")
		{
			return MvcRouteGenerator.CreateGenerator(Controller, baseUrl).GenerateRouteTemplate(this);
		}

		private string GetScriptName(IMethod method, NamingStrategy namingStrategy)
		{
			IAttributeData actionAttr = method.Attributes.FirstOrDefault(attr => CommonFilters.ScriptActionAttributeTypeFilter.Matches(attr.AttributeType));

			string key = nameof(ScriptActionAttribute.Name);
			if (actionAttr != null && actionAttr.NamedArguments.ContainsKey(key))
			{
				return actionAttr.NamedArguments[key] as string;
			}

			return namingStrategy.GetName(method.Name);
		}

		/// <summary>
		/// Fancy string
		/// </summary>
		/// <returns>Fancy</returns>
		public override string ToString()
		{
			return $"{ReturnType} {Name}({string.Join(",", Parameters.Select(p => p.ToString()))})";
		}
	}

	/// <summary>
	/// An MVC action parameter
	/// </summary>
	public class MvcActionParameter
	{
		private static TypeFilter s_scriptParamTypes
			= new IsOfTypeFilter(KnownTypes.ScriptParamTypesAttributeName);

		private ActionParameterSourceType? _bindingType;

		internal MvcAction Action { get; }

		/// <summary>
		/// Gets the name of the parameter
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Gets the type descriptor for the parameter
		/// </summary>
		public IReadOnlyList<TypeDescriptor> Types { get; }

		/// <summary>
		/// Gets attributes for this action parameter
		/// </summary>
		public IEnumerable<IAttributeData> Attributes { get; }

		/// <summary>
		/// Gets whether the parameter is optional
		/// </summary>
		public bool IsOptional { get; }

		public ActionParameterSourceType BindingType
		{
			get
			{
				if (!_bindingType.HasValue)
				{
					_bindingType = ComputeSource();
				}
				return _bindingType.Value;
			}
		}

		internal MvcActionParameter(MvcAction action, IMethodParameter methodParameter, TypeFactory typeFactory)
		{
			Action = action;
			Name = methodParameter.Name;
			Types = CompileTypes(methodParameter, typeFactory);
			Attributes = methodParameter.Attributes;
			IsOptional = methodParameter.IsOptional;
		}

		private ActionParameterSourceType ComputeSource()
		{
			ActionParameterSourceType sourceType = ActionParameterSourceType.Body;

			// Note - this isn't great, but is how it has always worked.
			// consider improving the asp.net stuff... or just cutting it out
			if (Action.Controller.IsAspNetCore)
			{
				var bodyFilter = new ParameterHasAttributeFilter(new IsOfTypeFilter(MvcConstants.FromBodyAttributeFullName_AspNetCore));
				var queryFilter = new ParameterHasAttributeFilter(new IsOfTypeFilter(MvcConstants.FromQueryAttributeFullName_AspNetCore));

				string routeTemplate = Action.GetRouteTemplate();
				if (bodyFilter.Evaluate(this))
				{
					sourceType = ActionParameterSourceType.Body;
				}
				else if (queryFilter.Evaluate(this))
				{
					sourceType = ActionParameterSourceType.Query;
				}
				else if (routeTemplate.Contains($"{{{this.Name}}}"))
				{
					sourceType = ActionParameterSourceType.Route;
				}
				else
				{
					sourceType = ActionParameterSourceType.Ignored;
				}
			}

			return sourceType;
		}

		private List<TypeDescriptor> CompileTypes(IMethodParameter methodParameter, TypeFactory typeFactory)
		{
			var attr = methodParameter.Attributes.FirstOrDefault(a => s_scriptParamTypes.Matches(a.AttributeType));
			if (attr == null)
			{
				return new List<TypeDescriptor>()
				{
					typeFactory.LookupType(methodParameter.ParameterType)
				};
			}

			object[] typeArgs = attr.ConstructorArguments[0] as object[];

			return typeArgs
				.OfType<INamedType>()
				.Select(arg => typeFactory.LookupType(arg))
				.ToList();
		}
	}
}
