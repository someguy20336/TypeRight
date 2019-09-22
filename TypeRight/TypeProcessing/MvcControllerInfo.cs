using TypeRight.CodeModel;
using TypeRight.TypeFilters;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// A single MVC controller containing HTTP Post actions
	/// </summary>
	public class MvcControllerInfo : ITypeWithFullName
	{
		public const string AspNetCoreMvcNamespace = "Microsoft.AspNetCore.Mvc";
		public const string AspNetMvcNamespace = "System.Web.Mvc";
		public const string RouteAttributeName = "RouteAttribute";


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

		public string ControllerName => Name.Substring(0, Name.Length - "Controller".Length);

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

		public string GetBaseUrl()
		{
			
			FileInfo fileInfo = new FileInfo(FilePath);
			DirectoryInfo controllerDir = fileInfo.Directory;

			// Check if this controller is in an "Area"
			bool foundAreas = false;
			DirectoryInfo dir = controllerDir;
			DirectoryInfo areaDir = null;  // if areas is found, this is the specific area directory (like "Admin", or "Shared", etc)
			while (dir != null)
			{
				if (dir.Name == "Areas")
				{
					foundAreas = true;
					break;
				}
				areaDir = dir;
				dir = dir.Parent;
			}

			IAttributeData attr = NamedType.Attributes.FirstOrDefault(a => 
				a.AttributeType.FullName == "Microsoft.AspNetCore.Mvc.RouteAttribute"
				|| a.AttributeType.FullName == "System.Web.Mvc.RouteAttribute"
			);
			if (attr == null)
			{		

				if (foundAreas)
				{
					// Area/ControllerName/Action
					return $"/{areaDir.Name}/{ControllerName}/";
				}
				else
				{
					// ControllerName/Action
					return $"/{ControllerName}/";
				}
			}
			else
			{
				string template = attr.ConstructorArguments[0] as string;
				template = template.Replace("[controller]", ControllerName)
					.Replace("[area]", foundAreas ? "" : areaDir.Name);

				if (!template.StartsWith("/"))
				{
					template = "/" + template;
				}
				if (!template.EndsWith("/"))
				{
					template += "/";
				}

				return template;
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
