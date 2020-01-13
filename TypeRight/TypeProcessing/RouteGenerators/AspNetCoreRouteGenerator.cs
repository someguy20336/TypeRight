using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypeRight.CodeModel;

namespace TypeRight.TypeProcessing.RouteGenerators
{
	/// <summary>
	/// Route generator for ASP net Core controllers
	/// </summary>
	class AspNetCoreRouteGenerator : MvcRouteGenerator
	{
		public AspNetCoreRouteGenerator(MvcControllerInfo controllerInfo) : base(controllerInfo)
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
	}
}
