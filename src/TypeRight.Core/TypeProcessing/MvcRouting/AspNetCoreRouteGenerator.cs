using System.Collections.Generic;
using System.Linq;
using TypeRight.CodeModel;

namespace TypeRight.TypeProcessing.MvcRouting
{
	/// <summary>
	/// Route generator for ASP net Core controllers
	/// </summary>
	internal class AspNetCoreRouteGenerator : MvcRouteGenerator
	{

		public AspNetCoreRouteGenerator(MvcController controller, string baseUrl) : base(controller, baseUrl)
		{
		}

		protected override string GetArea()
		{
			IAttributeData areaAttr = Controller.NamedType.Attributes.FirstOrDefault(a =>
				   a.AttributeType.FullName == MvcConstants.AreaAttributeFullName_AspNetCore
			   );

			return areaAttr != null ? areaAttr.ConstructorArguments.FirstOrDefault() as string : "";
		}

		protected override string GetBaseRouteTemplate()
		{
			IAttributeData routeAttr = Controller.NamedType.Attributes.FirstOrDefault(a =>
				a.AttributeType.FullName == MvcConstants.RouteAttributeFullName_AspNetCore
			);

			return routeAttr != null ? routeAttr.ConstructorArguments.FirstOrDefault() as string : "";
		}

		protected override List<RouteParameterResolver> GetParameterResolvers()
		{
			var list = base.GetParameterResolvers();
			list.Add(ApiVersionRouteParameterResolver.AspNetCore);

			return list;
		}
	}
}
