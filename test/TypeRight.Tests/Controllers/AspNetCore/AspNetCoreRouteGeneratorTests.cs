using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.Tests.TestBuilders.TypeCollection;

namespace TypeRight.Tests.Controllers.AspNetCore
{
	[TestClass]
	public class AspNetCoreRouteGeneratorTests : RouteGeneratorTestBase
	{

		protected override string ControllerNamespace => MvcConstants.AspNetCoreNamespace;

		protected override void AddMvcTypes(TypeCollectionBuilder builder)
		{
			builder.AddAspNetCoreTypes();
		}

		[TestMethod]
		public void RoutedByConvention()
		{
			AssertRouteEquals("/Things/RandoMethod");
		}

		[TestMethod]
		public void RoutedByAttribute()
		{
			Controller.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore, "api/Things/[action]");

			AssertRouteEquals("/api/Things/RandoMethod");
		}

		[TestMethod]
		public void Area_RoutedByConvention()
		{
			Controller.AddAttribute(MvcConstants.AreaAttributeFullName_AspNetCore, "ThingsManagement");

			AssertRouteEquals("/ThingsManagement/Things/RandoMethod");
		}

		[TestMethod]
		public void Area_InCustomRouteWithTokens()
		{
			Controller.AddAttribute(MvcConstants.AreaAttributeFullName_AspNetCore, "ThingsManagement")
				.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore, "api/[area]/[controller]/[action]");

			AssertRouteEquals("/api/ThingsManagement/Things/RandoMethod");
		}

		[TestMethod]
		public void HttpGetRoute_IsAppended()
		{
			Controller.AddAttribute(MvcConstants.AreaAttributeFullName_AspNetCore, "ThingsManagement")
				.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore, "api/[area]/[controller]");

			Action.AddAttribute(MvcConstants.HttpGetAttributeFullName_AspNetCore, "stuff");

			AssertRouteEquals("/api/ThingsManagement/Things/stuff");
		}

		[TestMethod]
		public void HttpPostRoute_WithIdPlaceholder_IsAppended()
		{
			Controller.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore, "api/[controller]");
			Action.AddAttribute(MvcConstants.HttpPostAttributeFullName_AspNetCore, "{id}/dostuff");

			AssertRouteEquals("/api/Things/{id}/dostuff");
		}

		[TestMethod]
		public void HttpPatchRoute_WithIdPlaceholder_IsAppended()
		{
			Controller.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore, "api/[controller]");
			Action.AddAttribute(MvcConstants.ToAspNetCoreFullName(MvcConstants.HttpPatchAttributeName), "{id}/dostuff");

			AssertRouteEquals("/api/Things/{id}/dostuff");
		}

		[TestMethod]
		public void BaseUrl_IsPrepended()
		{
			GivenBaseUrl("api");
			Controller.AddAttribute(MvcConstants.AreaAttributeFullName_AspNetCore, "ThingsManagement")
				.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore, "[area]/[controller]");


			AssertRouteEquals("/api/ThingsManagement/Things");
		}
	}
}
