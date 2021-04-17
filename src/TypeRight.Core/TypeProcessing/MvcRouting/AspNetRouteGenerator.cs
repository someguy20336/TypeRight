using System.Collections.Generic;
using System.IO;
using System.Linq;
using TypeRight.CodeModel;
using TypeRight.TypeFilters;

namespace TypeRight.TypeProcessing.MvcRouting
{
	/// <summary>
	/// Route generator for ASP net controllers
	/// </summary>
	internal class AspNetRouteGenerator : MvcRouteGenerator
	{

		private static TypeFilter s_routeAreaTypeFilter = new IsOfTypeFilter(MvcConstants.RouteAreaAttributeFullName_AspNet);

		public AspNetRouteGenerator(MvcControllerInfo controller, string baseUrl) : base(controller, baseUrl)
		{
		}

		protected override string GetArea()
		{
			// Check for route area attribute
			var routeArea = Controller.NamedType.Attributes.FirstOrDefault(attr => s_routeAreaTypeFilter.Evaluate(attr.AttributeType));
			if (routeArea != null)
			{
				return routeArea.ConstructorArguments[0] as string;
			}

			// Fall back to folder structure
			FileInfo fileInfo = new FileInfo(Controller.FilePath);
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

			return foundAreas ? areaDir.Name : "";
		}

		protected override string GetBaseRouteTemplate()
		{
			IAttributeData attr = Controller.NamedType.Attributes.FirstOrDefault(a =>
				a.AttributeType.FullName == MvcConstants.RouteAttributeFullName_AspNet
			);

			return attr != null
				? attr.ConstructorArguments[0] as string
				: "";
		}

		protected override List<RouteParameterResolver> GetParameterResolvers()
		{
			var list = base.GetParameterResolvers();
			list.Add(ApiVersionRouteParameterResolver.AspNet);

			return list;
		}
	}
}
