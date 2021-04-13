using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.Tests.TestBuilders;

namespace TypeRight.Tests.Controllers.AspNet
{
	[TestClass]
	public class AspNetRouteGeneratorTests : RouteGeneratorTestBase
	{

		protected override string ControllerNamespace => MvcConstants.AspNetNamespace;

		protected override bool IsAspNetCore => false;

		[TestMethod]
		public void RoutedByConvention()
		{
			AssertRouteEquals("/Things/RandoMethod");
		}

		// TODO: not sure what to do here anymore
		//[TestMethod]
		//public void Area_FromFolder_RoutedByConvention()
		//{
		//	ControllerBuilder.WithFilePath(@"C:\FolderA\Areas\ThingsArea\Controllers\ThingsController.cs");
		//	AssertRouteEquals("/ThingsArea/Things/RandoMethod");
		//}

		[TestMethod]
		public void Area_FromRouteArea_RoutedByConvention()
		{
			ControllerBuilder.AddAttribute(MvcConstants.RouteAreaAttributeFullName_AspNet)
				.AddStringConstructorArg("ThingsArea").Commit();

			AssertRouteEquals("/ThingsArea/Things/RandoMethod");
		}

		[TestMethod]
		public void HttpGet_RoutedByAttribute()
		{
			ControllerBuilder.AddAttribute(MvcConstants.RouteAttributeFullName_AspNet)
				.AddStringConstructorArg("api/Things").Commit();

			Action.AddAttribute(MvcConstants.HttpGetAttributeFullName_AspNet)
				.AddStringConstructorArg("GetThings").Commit();

			AssertRouteEquals("/api/Things/GetThings");
		}

		[TestMethod]
		public void HttpPatch_RoutedByAttribute()
		{
			ControllerBuilder.AddAttribute(MvcConstants.RouteAttributeFullName_AspNet)
				.AddStringConstructorArg("api/Things").Commit();

			Action.AddAttribute(MvcConstants.ToAspNetFullName(MvcConstants.HttpPatchAttributeName))
				.AddStringConstructorArg("GetThings").Commit();

			AssertRouteEquals("/api/Things/GetThings");
		}

		[TestMethod]
		public void BaseUrl_IsPrepended()
		{
			GivenBaseUrl("api");
			ControllerBuilder.AddAttribute(MvcConstants.RouteAttributeFullName_AspNet)
				.AddStringConstructorArg("Things").Commit();

			Action.AddAttribute(MvcConstants.HttpGetAttributeFullName_AspNet)
				.AddStringConstructorArg("GetThings").Commit();

			AssertRouteEquals("/api/Things/GetThings");
		}

		[TestMethod]
		public void ApiVersion_IsResolved()
		{
			ControllerBuilder.AddAttribute(MvcConstants.RouteAttributeFullName_AspNet)
				.AddStringConstructorArg("api/v{v:apiVersion}/[controller]").Commit()
				.AddAttribute(MvcConstants.ApiVersionAttributeFullName_AspNet)
					.AddStringConstructorArg("1.0").Commit();


			AssertRouteEquals("/api/v1.0/Things");
		}

	}
}
