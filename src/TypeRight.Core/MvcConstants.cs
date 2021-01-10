using System;
using System.Collections.Generic;
using System.Text;

namespace TypeRight
{
	public static class MvcConstants
	{
		public const string AspNetNamespace = "System.Web.Mvc";
		public const string AspNetCoreNamespace = "Microsoft.AspNetCore.Mvc";

		public const string RouteAttributeName = "RouteAttribute";
		public static string RouteAttributeFullName_AspNet => ToAspNetFullName(RouteAttributeName);
		public static string RouteAttributeFullName_AspNetCore => ToAspNetCoreFullName(RouteAttributeName);

		public const string AreaAttribute = "AreaAttribute";
		public static string AreaAttributeFullName_AspNetCore => ToAspNetCoreFullName(AreaAttribute);

		public const string RouteAreaAttribute = "RouteAreaAttribute";
		public static string RouteAreaAttributeFullName_AspNet => ToAspNetFullName(RouteAreaAttribute);

		public const string ControllerBaseName = "ControllerBase";
		public static string ControllerBaseFullName_AspNet => ToAspNetFullName(ControllerBaseName);
		public static string ControllerBaseFullName_AspNetCore => ToAspNetCoreFullName(ControllerBaseName);

		public const string FromBodyAttributeName = "FromBodyAttribute";
		public static string FromBodyAttributeFullName_AspNetCore => ToAspNetCoreFullName(FromBodyAttributeName);

		public const string FromServicesAttributeName = "FromServicesAttribute";
		public static string FromServicesAttributeFullName_AspNetCore => ToAspNetCoreFullName(FromServicesAttributeName);

		public const string FromQueryAttributeName = "FromQueryAttribute";
		public static string FromQueryAttributeFullName_AspNetCore => ToAspNetCoreFullName(FromQueryAttributeName);

		public const string HttpPostAttributeName = "HttpPostAttribute";
		public static string HttpPostAttributeFullName_AspNetCore => ToAspNetCoreFullName(HttpPostAttributeName);
		public static string HttpPostAttributeFullName_AspNet => ToAspNetFullName(HttpPostAttributeName);

		public const string HttpGetAttributeName = "HttpGetAttribute";
		public static string HttpGetAttributeFullName_AspNetCore => ToAspNetCoreFullName(HttpGetAttributeName);
		public static string HttpGetAttributeFullName_AspNet => ToAspNetFullName(HttpGetAttributeName);

		public const string HttpPutAttributeName = "HttpPutAttribute";
		public static string HttpPutAttributeFullName_AspNetCore => ToAspNetCoreFullName(HttpPutAttributeName);
		public static string HttpPutAttributeFullName_AspNet => ToAspNetFullName(HttpPutAttributeName);


		public const string HttpDeleteAttributeName = "HttpDeleteAttribute";

		public const string JsonResultName = "JsonResult";
		public static string JsonResult_AspNetCore => ToAspNetCoreFullName(JsonResultName);
		public static string JsonResult_AspNet => ToAspNetFullName(JsonResultName);

		public const string ActionResultName = "ActionResult";
		public static string ActionResult_AspNetCore => ToAspNetCoreFullName(ActionResultName);

		public static string ToAspNetCoreFullName(string className) => $"{AspNetCoreNamespace}.{className}";
		public static string ToAspNetFullName(string className) => $"{AspNetNamespace}.{className}";
	}
}
