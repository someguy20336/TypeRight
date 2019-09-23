using System;
using System.Collections.Generic;
using System.Text;

namespace TypeRight.TypeProcessing
{
	public static class MvcConstants
	{
		public const string AspNetNamespace = "System.Web.Mvc";
		public const string AspNetCoreNamespace = "Microsoft.AspNetCore.Mvc";

		public const string RouteAttributeName = "RouteAttribute";
		public static string RouteAttributeFullName_AspNet => ToAspNetFullName(RouteAttributeName);
		public static string RouteAttributeFullName_AspNetCore => ToAspNetCoreFullName(RouteAttributeName);

		public const string ControllerBaseName = "ControllerBase";
		public static string ControllerBaseFullName_AspNet => ToAspNetFullName(ControllerBaseName);
		public static string ControllerBaseFullName_AspNetCore => ToAspNetCoreFullName(ControllerBaseName);

		public const string FromBodyAttributeName = "FromBodyAttribute";
		public static string FromBodyAttributeFullName_AspNetCore => ToAspNetCoreFullName(FromBodyAttributeName);

		public const string FromServicesAttributeName = "FromServicesAttribute";
		public static string FromServicesAttributeFullName_AspNetCore => ToAspNetCoreFullName(FromServicesAttributeName);

		public const string FromQueryAttributeName = "FromQueryAttribute";
		public static string FromQueryAttributeFullName_AspNetCore => ToAspNetCoreFullName(FromQueryAttributeName);

		public static string ToAspNetCoreFullName(string className) => $"{AspNetCoreNamespace}.{className}";
		public static string ToAspNetFullName(string className) => $"{AspNetNamespace}.{className}";
	}
}
