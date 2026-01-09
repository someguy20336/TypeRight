namespace TypeRight
{
	public static class MvcConstants
	{
		public const string AspNetCoreNamespace = "Microsoft.AspNetCore.Mvc";
		public const string ApiVersioningNamespace = "Asp.Versioning";

		public const string ApiVersionAttributeName = "ApiVersionAttribute";
		public static string ApiVersionAttributeFullName_AspNetCore = ToAspNetCoreFullName(ApiVersionAttributeName);
		public static string ApiVersionAttributeFullName_ApiVersioning = $"{ApiVersioningNamespace}.{ApiVersionAttributeName}";

		public const string RouteAttributeName = "RouteAttribute";
		public static string RouteAttributeFullName_AspNetCore => ToAspNetCoreFullName(RouteAttributeName);

		public const string AreaAttribute = "AreaAttribute";
		public static string AreaAttributeFullName_AspNetCore => ToAspNetCoreFullName(AreaAttribute);

		public const string RouteAreaAttribute = "RouteAreaAttribute";

		public const string ControllerBaseName = "ControllerBase";
		public static string ControllerBaseFullName_AspNetCore => ToAspNetCoreFullName(ControllerBaseName);

		public const string FromBodyAttributeName = "FromBodyAttribute";
		public static string FromBodyAttributeFullName_AspNetCore => ToAspNetCoreFullName(FromBodyAttributeName);

		public const string FromRouteAttributeName = "FromRouteAttribute";
		public static string FromRouteAttributeFullName_AspNetCore => ToAspNetCoreFullName(FromRouteAttributeName);

		public const string FromServicesAttributeName = "FromServicesAttribute";
		public static string FromServicesAttributeFullName_AspNetCore => ToAspNetCoreFullName(FromServicesAttributeName);

		public const string FromQueryAttributeName = "FromQueryAttribute";
		public static string FromQueryAttributeFullName_AspNetCore => ToAspNetCoreFullName(FromQueryAttributeName);

		public const string HttpPostAttributeName = "HttpPostAttribute";
		public static string HttpPostAttributeFullName_AspNetCore => ToAspNetCoreFullName(HttpPostAttributeName);

		public const string HttpGetAttributeName = "HttpGetAttribute";
		public static string HttpGetAttributeFullName_AspNetCore => ToAspNetCoreFullName(HttpGetAttributeName);

		public const string HttpPutAttributeName = "HttpPutAttribute";
		public static string HttpPutAttributeFullName_AspNetCore => ToAspNetCoreFullName(HttpPutAttributeName);


		public const string HttpDeleteAttributeName = "HttpDeleteAttribute";
		public const string HttpPatchAttributeName = "HttpPatchAttribute";

		public const string JsonResultName = "JsonResult";
		public static string JsonResult_AspNetCore => ToAspNetCoreFullName(JsonResultName);

		public const string ActionResultName = "ActionResult";
		public static string ActionResult_AspNetCore => ToAspNetCoreFullName(ActionResultName);

		public static string ToAspNetCoreFullName(string className) => $"{AspNetCoreNamespace}.{className}";
	}
}
