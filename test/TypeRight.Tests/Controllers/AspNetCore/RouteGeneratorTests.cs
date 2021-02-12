using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.Tests.TestBuilders.TypeCollection;
using TypeRight.TypeProcessing;

namespace TypeRight.Tests.Controllers.AspNetCore
{
	[TestClass]
	public class RouteGeneratorTests
	{
		private TypeCollectionBuilder _builder;

		private NamedTypeBuilder _controller;

		private MethodBuilder _action;

		[TestInitialize]
		public void Initialize()
		{
			_builder = TypeCollectionBuilder.Create()
				.AddAspNetCoreTypes();

			_controller = _builder.AddNamedType("ThingsController")
				.WithBaseType(MvcConstants.ControllerBaseName, MvcConstants.AspNetCoreNamespace);

			_action = _controller.AddMethod("RandoMethod", typeof(int))
				.AddScriptActionAttribute();
		}


		[TestMethod]
		public void RoutedByConvention()
		{
			AssertRouteEquals("/Things/RandoMethod");
		}

		[TestMethod]
		public void RoutedByAttribute()
		{
			_controller.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore, "api/Things/[action]");

			AssertRouteEquals("/api/Things/RandoMethod");
		}

		[TestMethod]
		public void Area_RoutedByConvention()
		{
			_controller.AddAttribute(MvcConstants.AreaAttributeFullName_AspNetCore, "ThingsManagement");

			AssertRouteEquals("/ThingsManagement/Things/RandoMethod");
		}

		[TestMethod]
		public void Area_InCustomRouteWithTokens()
		{
			_controller.AddAttribute(MvcConstants.AreaAttributeFullName_AspNetCore, "ThingsManagement")
				.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore, "api/[area]/[controller]/[action]"); ;

			AssertRouteEquals("/api/ThingsManagement/Things/RandoMethod");
		}

		[TestMethod]
		public void HttpGetRoute_IsAppended()
		{
			_controller.AddAttribute(MvcConstants.AreaAttributeFullName_AspNetCore, "ThingsManagement")
				.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore, "api/[area]/[controller]");

			_action.AddAttribute(MvcConstants.HttpGetAttributeFullName_AspNetCore, "stuff"); ;

			AssertRouteEquals("/api/ThingsManagement/Things/stuff");
		}

		[TestMethod]
		public void HttpPostRoute_WithIdPlaceholder_IsAppended()
		{
			_controller.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore, "api/[controller]");
			_action.AddAttribute(MvcConstants.HttpPostAttributeFullName_AspNetCore, "{id}/dostuff"); ;

			AssertRouteEquals("/api/Things/{id}/dostuff");
		}

		[TestMethod]
		public void HttpPatchRoute_WithIdPlaceholder_IsAppended()
		{
			_controller.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore, "api/[controller]");
			_action.AddAttribute(MvcConstants.ToAspNetCoreFullName(MvcConstants.HttpPatchAttributeName), "{id}/dostuff"); ;

			AssertRouteEquals("/api/Things/{id}/dostuff");
		}

		private void AssertRouteEquals(string expectedRoute)
		{
			_action.Commit();
			_controller.BuildAsController();
			var collection = _builder.Build();
			var controller = collection.GetMvcControllers().First();
			var routeGen = MvcRouteGenerator.CreateGenerator(controller);
			Assert.AreEqual(expectedRoute, routeGen.GenerateRouteTemplate(controller.Actions.First()));
		}

	}
}
