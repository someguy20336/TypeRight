using TypeRight.CodeModel;
using TypeRight.TypeFilters;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using TypeRight.Attributes;

namespace TypeRight.TypeProcessing
{
	/// <summary>
	/// A single MVC controller containing HTTP Post actions
	/// </summary>
	public class MvcController : ITypeWithFullName
	{

		private string _lazyResultPath = null;
		private List<MvcAction> _actions = new List<MvcAction>();
		private readonly TypeFactory _typeFactory;
		private Dictionary<string, IProperty> _routeParameters;

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

		public string ResultPath => GetControllerResultPath();

		/// <summary>
		/// Gets a list of the actions
		/// </summary>
		public IReadOnlyList<MvcAction> Actions => _actions;

		public string ControllerName => Name.Substring(0, Name.Length - "Controller".Length);

		public bool IsAspNetCore { get; }

		/// <summary>
		/// Creates a new the MVC controllers info
		/// </summary>
		/// <param name="namedType">The named type for the controller</param>
		/// <param name="actionFilter">The parse filter to use for the MVC action attribute</param>
		/// <param name="typeFactory">The type table</param>
		internal MvcController(INamedType namedType, TypeFilter actionFilter, TypeFactory typeFactory)
		{
			NamedType = namedType;
			_typeFactory = typeFactory;

			foreach (IMethod method in namedType.Methods)
			{
				if (method.Attributes.Any(attrData => actionFilter.Matches(attrData.AttributeType)))
				{
					MvcAction action = new MvcAction(this, method, typeFactory);
					_actions.Add(action);
				}
			}

			var baseType = NamedType.BaseType;
			while (baseType != null)
			{
				if (baseType.FullName == MvcConstants.ControllerBaseFullName_AspNetCore)
				{
					IsAspNetCore = true;
					break;
				}
				baseType = baseType.BaseType;
			}
		}

		private IEnumerable<MvcActionParameter> ResolveControllerRouteParams(MvcAction forAction)
		{

			//List<MvcActionParameter> controllerParams = new List<MvcActionParameter>();
			//foreach (var property in NamedType.Properties)
			//{

			//	var fromRoute = property.Attributes.FirstOrDefault(attr => attr.AttributeType.FullName == MvcConstants.FromRouteAttributeFullName_AspNetCore);
			//	if (fromRoute != null)
			//	{
			//		string routeParamName = fromRoute.NamedArguments["Name"].ToString();
			//		MvcActionParameter actionParameter = new MvcActionParameter(forAction, routeParamName, property.PropertyType, property.Attributes, _typeFactory);
			//	}
			//}

			return null;

		}

		public IReadOnlyDictionary<string, IProperty> GetPropertyRouteParams()
		{
			if (_routeParameters != null)
			{
				return _routeParameters;
			}

			_routeParameters = new Dictionary<string, IProperty>();
			foreach (var property in NamedType.Properties)
			{

				var fromRoute = property.Attributes.FirstOrDefault(attr => attr.AttributeType.FullName == MvcConstants.FromRouteAttributeFullName_AspNetCore);
				if (fromRoute != null)
				{
					string routeParamName = fromRoute.NamedArguments["Name"].ToString();
					_routeParameters.Add(routeParamName, property);
				}
			}

			return _routeParameters;
		}

		/// <summary>
		/// Gets the result path for a controller
		/// </summary>
		/// <returns>The result path</returns>
		private string GetControllerResultPath()
		{
			if (_lazyResultPath != null)
			{
				return _lazyResultPath;
			}

			FileInfo fileInfo = new FileInfo(FilePath);
			DirectoryInfo controllerDir = fileInfo.Directory;

			// Get relative output path
			string controllerName = Name.Substring(0, Name.Length - "Controller".Length);
			string relativeOutputPath = Path.Combine($"..\\Scripts\\{controllerName}", controllerName + "Actions.ts");

			var outputAttr = NamedType.Attributes.FirstOrDefault(attr => attr.AttributeType.FullName == typeof(ScriptOutputAttribute).FullName);
			if (outputAttr != null)
			{
				relativeOutputPath = outputAttr.ConstructorArguments[0] as string;
			}

			// Calculate the result
			string resultPath = Path.Combine(controllerDir.FullName, relativeOutputPath);
			_lazyResultPath = Path.GetFullPath(resultPath);

			return _lazyResultPath;
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
