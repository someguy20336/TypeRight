using System.Collections.Generic;
using System.Linq;
using TypeRight.CodeModel;
using TypeRight.ScriptWriting;

namespace TypeRight.TypeProcessing.MvcRouting
{
	/// <summary>
	/// Route generator for ASP net Core controllers
	/// </summary>
	class AspNetCoreRouteGenerator : MvcRouteGenerator
	{
		public AspNetCoreRouteGenerator(ControllerContext context) : base(context)
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
