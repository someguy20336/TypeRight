using TypeRight.CodeModel;
using System.Collections.Generic;
using System.Linq;
using TypeRight.TypeFilters;
using TypeRight.Configuration;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// An object that contains information about an MVC action
	/// </summary>
	public class MvcActionInfo
	{
		private static ActionFilter s_postFilter = new ActionHasAttributeFilter(
			new IsOfAnyTypeFilter(MvcConstants.HttpPostAttributeFullName_AspNet, MvcConstants.HttpPostAttributeFullName_AspNetCore)
			);

		private static ActionFilter s_getFilter = new ActionHasAttributeFilter(
			new IsOfAnyTypeFilter(MvcConstants.HttpGetAttributeFullName_AspNet, MvcConstants.HttpGetAttributeFullName_AspNetCore)
			);

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

		public RequestMethod RequestMethod {
			get
			{
				if (s_getFilter.Evaluate(this))
				{
					return RequestMethod.Get;
				}
				else if (s_postFilter.Evaluate(this))
				{
					return RequestMethod.Post;
				}
				else 
				{
					return RequestMethod.Default;
				}
			}
			
		}


		/// <summary>
		/// Creates a new action info from the given method
		/// </summary>
		/// <param name="method">the method</param>
		/// <param name="typeTable">The type table</param>
		internal MvcActionInfo(IMethod method, TypeTable typeTable)
		{
			Method = method;
			ReturnType = typeTable.LookupType(method.ReturnType);
			ParameterComments = method.Parameters.ToDictionary(param => param.Name, param => param.Comments);
			Parameters = method.Parameters.Select(p => new MvcActionParameter(p, typeTable)).ToList().AsReadOnly();
		}

		/// <summary>
		/// Fancy string
		/// </summary>
		/// <returns>Fancy</returns>
		public override string ToString()
		{
			return $"{ReturnType.ToString()} {Name}({string.Join(",", Parameters.Select(p => p.ToString()))})";
		}
	}

	/// <summary>
	/// An MVC action parameter
	/// </summary>
	public class MvcActionParameter
	{
		/// <summary>
		/// Gets the name of the parameter
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Gets the type descriptor for the parameter
		/// </summary>
		public TypeDescriptor Type { get; }

		/// <summary>
		/// Gets attributes for this action parameter
		/// </summary>
		public IEnumerable<IAttributeData> Attributes { get; }

		internal MvcActionParameter(IMethodParameter methodParameter, TypeTable table)
		{
			Name = methodParameter.Name;
			Type = table.LookupType(methodParameter.ParameterType);
			Attributes = methodParameter.Attributes;
		}
	}
}
