﻿using TypeRight.CodeModel;
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
		private readonly TypeFactory _typeFactory;
		private List<MvcActionParameter> _compiledParameters;

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
		public IReadOnlyList<MvcActionParameter> ActionParameters { get; }

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
			_typeFactory = typeFactory;
			ScriptName = GetScriptName(method, typeFactory.Settings.NamingStrategy);
			ReturnType = typeFactory.LookupType(method.ReturnType);
			ParameterComments = method.Parameters.ToDictionary(param => param.Name, param => param.Comments);
			ActionParameters = method.Parameters.Select(p => new MvcActionParameter(this, p, typeFactory)).ToList().AsReadOnly();
		}

		public string GetRouteTemplate(string baseUrl = "")
		{
			return MvcRouteGenerator.CreateGenerator(Controller, baseUrl).GenerateRouteTemplate(this);
		}

		public IEnumerable<MvcActionParameter> GetCompiledParameters()
		{
			if (_compiledParameters != null)
			{
				return _compiledParameters;
			}
			string routeTemplate = GetRouteTemplate("");

			var controllerPropParams = Controller.GetPropertyBoundParams();

			List<MvcActionParameter> parameters = new List<MvcActionParameter>();
			foreach (var routeParamName in controllerPropParams.Keys)
			{
				if (routeTemplate.Contains($"{{{routeParamName}}}"))
				{
					IProperty property = controllerPropParams[routeParamName];
					parameters.Add(new MvcActionParameter(this, routeParamName, property.PropertyType, property.Attributes, _typeFactory));
				}
			}

			_compiledParameters = parameters.Concat(ActionParameters).ToList();
			return _compiledParameters;
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
			return $"{ReturnType} {Name}({string.Join(",", ActionParameters.Select(p => p.ToString()))})";
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
			: this(action, methodParameter.Name, methodParameter.ParameterType, methodParameter.Attributes, typeFactory)
		{
			IsOptional = methodParameter.IsOptional;
		}

		internal MvcActionParameter(MvcAction action, 
			string paramName, 
			IType memberType, 
			IEnumerable<IAttributeData> memberAttributes, 
			TypeFactory typeFactory)
		{
			Action = action;
			Name = paramName;
			Types = CompileTypes(memberType, memberAttributes, typeFactory);
			Attributes = memberAttributes;
			IsOptional = false;
		}

		private ActionParameterSourceType ComputeSource()
		{
			ActionParameterSourceType sourceType;
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


			return sourceType;
		}

		private List<TypeDescriptor> CompileTypes(IType memberType, IEnumerable<IAttributeData> memberAttributes, TypeFactory typeFactory)
		{
			var attr = memberAttributes.FirstOrDefault(a => s_scriptParamTypes.Matches(a.AttributeType));
			if (attr == null)
			{
				return new List<TypeDescriptor>()
				{
					typeFactory.LookupType(memberType)
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
