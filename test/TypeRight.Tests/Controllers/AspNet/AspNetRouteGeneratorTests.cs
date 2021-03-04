using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.Tests.TestBuilders.TypeCollection;

namespace TypeRight.Tests.Controllers.AspNet
{
	[TestClass]
	public class AspNetRouteGeneratorTests : RouteGeneratorTestBase
	{

		protected override string ControllerNamespace => MvcConstants.AspNetNamespace;

		protected override void AddMvcTypes(TypeCollectionBuilder builder)
		{
			builder.AddAspNetTypes();
		}

		[TestMethod]
		public void RoutedByConvention()
		{
			AssertRouteEquals("/Things/RandoMethod");
		}

		[TestMethod]
		public void Area_FromFolder_RoutedByConvention()
		{
			Controller.WithFilePath(@"C:\FolderA\Areas\ThingsArea\Controllers\ThingsController.cs");
			AssertRouteEquals("/ThingsArea/Things/RandoMethod");
		}

		[TestMethod]
		public void DotNet_Area_FromRouteArea_RoutedByConvention()
		{
			Controller.AddAttribute(MvcConstants.RouteAreaAttributeFullName_AspNet, "ThingsArea");

			AssertRouteEquals("/ThingsArea/Things/RandoMethod");
		}

		[TestMethod]
		public void HttpGet_RoutedByAttribute()
		{
			Controller.AddAttribute(MvcConstants.RouteAttributeFullName_AspNet, "api/Things");
			Action.AddAttribute(MvcConstants.HttpGetAttributeFullName_AspNet, "GetThings");

			AssertRouteEquals("/api/Things/GetThings");
		}

		[TestMethod]
		public void HttpPatch_RoutedByAttribute()
		{
			Controller.AddAttribute(MvcConstants.RouteAttributeFullName_AspNet, "api/Things");
			Action.AddAttribute(MvcConstants.ToAspNetFullName(MvcConstants.HttpPatchAttributeName), "GetThings");

			AssertRouteEquals("/api/Things/GetThings");
		}

	}
}
