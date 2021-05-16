using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.Tests.TestBuilders;

namespace TypeRight.Tests.Controllers.AspNetCore
{
	[TestClass]
	public class AspNetCoreRouteGeneratorTests : RouteGeneratorTestBase
	{

		protected override string ControllerNamespace => MvcConstants.AspNetCoreNamespace;


		[TestMethod]
		public void RoutedByConvention()
		{
			AssertRouteEquals("/Things/RandoMethod");
		}

		[TestMethod]
		public void RoutedByAttribute()
		{
			ControllerBuilder.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore)
				.AddStringConstructorArg("api/Things/[action]").Commit();

			AssertRouteEquals("/api/Things/RandoMethod");
		}

		[TestMethod]
		public void Area_RoutedByConvention()
		{
			ControllerBuilder.AddAttribute(MvcConstants.AreaAttributeFullName_AspNetCore)
				.AddStringConstructorArg("ThingsManagement").Commit();

			AssertRouteEquals("/ThingsManagement/Things/RandoMethod");
		}

		[TestMethod]
		public void Area_InCustomRouteWithTokens()
		{
			ControllerBuilder.AddAttribute(MvcConstants.AreaAttributeFullName_AspNetCore)
					.AddStringConstructorArg("ThingsManagement").Commit()
				.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore)
					.AddStringConstructorArg("api/[area]/[controller]/[action]").Commit();

			AssertRouteEquals("/api/ThingsManagement/Things/RandoMethod");
		}

		[TestMethod]
		public void HttpGetRoute_IsAppended()
		{
			ControllerBuilder.AddAttribute(MvcConstants.AreaAttributeFullName_AspNetCore)
					.AddStringConstructorArg("ThingsManagement").Commit()
				.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore)
					.AddStringConstructorArg("api/[area]/[controller]").Commit();

			Action.AddAttribute(MvcConstants.HttpGetAttributeFullName_AspNetCore)
				.AddStringConstructorArg("stuff").Commit();

			AssertRouteEquals("/api/ThingsManagement/Things/stuff");
		}

		[TestMethod]
		public void HttpPostRoute_WithIdPlaceholder_IsAppended()
		{
			ControllerBuilder.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore)
				.AddStringConstructorArg("api/[controller]").Commit();

			Action.AddAttribute(MvcConstants.HttpPostAttributeFullName_AspNetCore)
				.AddStringConstructorArg("{id}/dostuff").Commit();

			AssertRouteEquals("/api/Things/{id}/dostuff");
		}

		[TestMethod]
		public void HttpPatchRoute_WithIdPlaceholder_IsAppended()
		{
			ControllerBuilder.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore)
				.AddStringConstructorArg("api/[controller]").Commit();

			Action.AddAttribute(MvcConstants.ToAspNetCoreFullName(MvcConstants.HttpPatchAttributeName))
				.AddStringConstructorArg("{id}/dostuff").Commit();

			AssertRouteEquals("/api/Things/{id}/dostuff");
		}

		[TestMethod]
		public void BaseUrl_IsPrepended_ScriptGenerated()
		{
			GivenBaseUrl("api");
			ControllerBuilder.AddAttribute(MvcConstants.AreaAttributeFullName_AspNetCore)
					.AddStringConstructorArg("ThingsManagement").Commit()
				.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore)
					.AddStringConstructorArg("[area]/[controller]").Commit();

			Action.Commit();

			AssertScriptTextForFunctionIs(@"
export function RandoMethod(): void {
	fetchWrapper(""GET"", `/api/ThingsManagement/Things`, null);
}");
		}

		[TestMethod]
		public void ApiVersion_IsResolved()
		{
			ControllerBuilder.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore)
					.AddStringConstructorArg("api/v{v:apiVersion}/[controller]").Commit()
				.AddAttribute(MvcConstants.ApiVersionAttributeFullName_AspNetCore)
					.AddStringConstructorArg("1.0").Commit();

			AssertRouteEquals("/api/v1.0/Things");
		}
	}
}
