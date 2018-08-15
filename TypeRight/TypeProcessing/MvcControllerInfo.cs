using TypeRight.CodeModel;
using TypeRight.TypeFilters;
using System.Collections.Generic;
using System.Linq;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// A single MVC controller containing HTTP Post actions
	/// </summary>
	public class MvcControllerInfo : ITypeWithFullName
	{

		private List<MvcActionInfo> _actions = new List<MvcActionInfo>();

		/// <summary>
		/// Gets the named type for this action
		/// </summary>
		public INamedType NamedType { get; private set; }

		/// <summary>
		/// Gets the name of the Type
		/// </summary>
		public string Name => NamedType.Name;

		/// <summary>
		/// Gets the full name of the Type, as known in code
		/// </summary>
		public string FullName => NamedType.FullName;

		/// <summary>
		/// Gets the filepath to the controller
		/// </summary>
		public string FilePath => NamedType.FilePath;

		/// <summary>
		/// Gets a list of the actions
		/// </summary>
		public IReadOnlyList<MvcActionInfo> Actions => _actions;

		/// <summary>
		/// Creates a new the MVC controllers info
		/// </summary>
		/// <param name="namedType">The named type for the controller</param>
		/// <param name="actionFilter">The parse filter to use for the MVC action attribute</param>
		/// <param name="typeTable">The type table</param>
		internal MvcControllerInfo(INamedType namedType, TypeFilter actionFilter, TypeTable typeTable)
		{
			NamedType = namedType;
			
			foreach (IMethod method in namedType.Methods)
			{
				if (method.Attributes.Any(attrData => actionFilter.Evaluate(attrData.AttributeType)))
				{
					MvcActionInfo action = new MvcActionInfo(method, typeTable);
					_actions.Add(action);
				}
			}
		}

		/// <summary>
		/// Writes to a string
		/// </summary>
		/// <returns>nice string</returns>
		public override string ToString()
		{
			return Name;
		}
	}
}
