using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.Tests.TestBuilders.TypeCollection;
using TypeRight.TypeProcessing;

namespace TypeRight.Tests.Controllers.AspNet
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
				.AddAspNetTypes();

			_controller = _builder.AddNamedType("ThingsController")
				.WithBaseType(MvcConstants.ControllerBaseName, MvcConstants.AspNetNamespace);

			_action = _controller.AddMethod("RandoMethod", typeof(int))
				.AddScriptActionAttribute();
		}

		[TestMethod]
		public void RoutedByConvention()
		{
			AssertRouteEquals("/Things/RandoMethod");
		}

		[TestMethod]
		public void Area_FromFolder_RoutedByConvention()
		{
			_controller.WithFilePath(@"C:\FolderA\Areas\ThingsArea\Controllers\ThingsController.cs");
			AssertRouteEquals("/ThingsArea/Things/RandoMethod");
		}

		[TestMethod]
		public void DotNet_Area_FromRouteArea_RoutedByConvention()
		{
			_controller.AddAttribute(MvcConstants.RouteAreaAttributeFullName_AspNet, "ThingsArea");

			AssertRouteEquals("/ThingsArea/Things/RandoMethod");
		}

		[TestMethod]
		public void HttpGet_RoutedByAttribute()
		{
			_controller.AddAttribute(MvcConstants.RouteAttributeFullName_AspNet, "api/Things");
			_action.AddAttribute(MvcConstants.HttpGetAttributeFullName_AspNet, "GetThings");

			AssertRouteEquals("/api/Things/GetThings");
		}

		[TestMethod]
		public void HttpPatch_RoutedByAttribute()
		{
			_controller.AddAttribute(MvcConstants.RouteAttributeFullName_AspNet, "api/Things");
			_action.AddAttribute(MvcConstants.ToAspNetFullName(MvcConstants.HttpPatchAttributeName), "GetThings");

			AssertRouteEquals("/api/Things/GetThings");
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
