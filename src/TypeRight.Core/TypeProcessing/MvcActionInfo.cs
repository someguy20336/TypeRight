using TypeRight.CodeModel;
using System.Collections.Generic;
using System.Linq;
using TypeRight.TypeFilters;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// An object that contains information about an MVC action
	/// </summary>
	public class MvcActionInfo
	{

		private IRequestMethod _method;

		/// <summary>
		/// Gets the method behind this action info
		/// </summary>
		public IMethod Method { get; private set; }

		/// <summary>
		/// Gets the name of the action
		/// </summary>
		public string Name => Method.Name;

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
		internal MvcActionInfo(IMethod method, TypeFactory typeFactory)
		{
			Method = method;
			ReturnType = typeFactory.LookupType(method.ReturnType);
			ParameterComments = method.Parameters.ToDictionary(param => param.Name, param => param.Comments);
			Parameters = method.Parameters.Select(p => new MvcActionParameter(p, typeFactory)).ToList().AsReadOnly();
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

		internal MvcActionParameter(IMethodParameter methodParameter, TypeFactory typeFactory)
		{
			Name = methodParameter.Name;
			Types = CompileTypes(methodParameter, typeFactory);
			Attributes = methodParameter.Attributes;
			IsOptional = methodParameter.IsOptional;
		}

		private List<TypeDescriptor> CompileTypes(IMethodParameter methodParameter, TypeFactory typeFactory)
		{
			var attr = methodParameter.Attributes.FirstOrDefault(a => s_scriptParamTypes.Evaluate(a.AttributeType));
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
