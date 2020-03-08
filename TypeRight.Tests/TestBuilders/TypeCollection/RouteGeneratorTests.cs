using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.TypeProcessing;

namespace TypeRight.Tests.TestBuilders.TypeCollection
{
	[TestClass]
	public class RouteGeneratorTests
	{
		[TestMethod]
		public void NetCore_RoutedByConvention()
		{
			var collection = TypeCollectionBuilder.Create()
				.AddAspNetCoreTypes()
				.AddNamedType("NewController")
					.WithBaseType(MvcConstants.ControllerBaseName, MvcConstants.AspNetCoreNamespace)
					.AddMethod("RandoMethod", typeof(int))
						.AddScriptActionAttribute()
						.Commit()
					.BuildAsController()
				.Build();

			var controller = collection.GetMvcControllers().First();
			var routeGen = MvcRouteGenerator.CreateGenerator(controller);
			Assert.AreEqual("/New/RandoMethod", routeGen.GenerateRouteTemplate(controller.Actions.First()));

		}

		[TestMethod]
		public void NetCore_RoutedByAttribute()
		{
			var collection = TypeCollectionBuilder.Create()
				.AddAspNetCoreTypes()
				.AddNamedType("NewController")
					.WithBaseType(MvcConstants.ControllerBaseName, MvcConstants.AspNetCoreNamespace)
					.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore, "api/Things/[action]")
					.AddMethod("RandoMethod", typeof(int))
						.AddScriptActionAttribute()
						.Commit()
					.BuildAsController()
				.Build();

			var controller = collection.GetMvcControllers().First();
			var routeGen = MvcRouteGenerator.CreateGenerator(controller);
			Assert.AreEqual("/api/Things/RandoMethod", routeGen.GenerateRouteTemplate(controller.Actions.First()));
		}

		[TestMethod]
		public void NetCore_Area_RoutedByConvention()
		{
			var collection = TypeCollectionBuilder.Create()
				.AddAspNetCoreTypes()
				.AddNamedType("ThingsController")
					.WithBaseType(MvcConstants.ControllerBaseName, MvcConstants.AspNetCoreNamespace)
					.AddAttribute(MvcConstants.AreaAttributeFullName_AspNetCore, "ThingsManagement")
					.AddMethod("RandoMethod", typeof(int))
						.AddScriptActionAttribute()
						.Commit()
					.BuildAsController()
				.Build();

			var controller = collection.GetMvcControllers().First();
			var routeGen = MvcRouteGenerator.CreateGenerator(controller);
			Assert.AreEqual("/ThingsManagement/Things/RandoMethod", routeGen.GenerateRouteTemplate(controller.Actions.First()));
		}

		[TestMethod]
		public void NetCore_Area_InCustomRouteWithTokens()
		{
			var collection = TypeCollectionBuilder.Create()
				.AddAspNetCoreTypes()
				.AddNamedType("ThingsController")
					.WithBaseType(MvcConstants.ControllerBaseName, MvcConstants.AspNetCoreNamespace)
					.AddAttribute(MvcConstants.AreaAttributeFullName_AspNetCore, "ThingsManagement")
					.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore, "api/[area]/[controller]/[action]")
					.AddMethod("RandoMethod", typeof(int))
						.AddScriptActionAttribute()
						.Commit()
					.BuildAsController()
				.Build();

			var controller = collection.GetMvcControllers().First();
			var routeGen = MvcRouteGenerator.CreateGenerator(controller);
			Assert.AreEqual("/api/ThingsManagement/Things/RandoMethod", routeGen.GenerateRouteTemplate(controller.Actions.First()));
		}

		[TestMethod]
		public void NetCore_HttpGetRoute_IsAppended()
		{
			var collection = TypeCollectionBuilder.Create()
				.AddAspNetCoreTypes()
				.AddNamedType("ThingsController")
					.WithBaseType(MvcConstants.ControllerBaseName, MvcConstants.AspNetCoreNamespace)
					.AddAttribute(MvcConstants.AreaAttributeFullName_AspNetCore, "ThingsManagement")
					.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore, "api/[area]/[controller]")
					.AddMethod("RandoMethod", typeof(int))
						.AddScriptActionAttribute()
						.AddAttribute(MvcConstants.HttpGetAttributeFullName_AspNetCore, "stuff")
						.Commit()
					.BuildAsController()
				.Build();

			var controller = collection.GetMvcControllers().First();
			var routeGen = MvcRouteGenerator.CreateGenerator(controller);
			Assert.AreEqual("/api/ThingsManagement/Things/stuff", routeGen.GenerateRouteTemplate(controller.Actions.First()));
		}

		[TestMethod]
		public void NetCore_HttpPostRoute_WithIdPlaceholder_IsAppended()
		{
			var collection = TypeCollectionBuilder.Create()
				.AddAspNetCoreTypes()
				.AddNamedType("ThingsController")
					.WithBaseType(MvcConstants.ControllerBaseName, MvcConstants.AspNetCoreNamespace)
					.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore, "api/[controller]")
					.AddMethod("RandoMethod", typeof(int))
						.AddScriptActionAttribute()
						.AddAttribute(MvcConstants.HttpPostAttributeFullName_AspNetCore, "{id}/dostuff")
						.Commit()
					.BuildAsController()
				.Build();

			var controller = collection.GetMvcControllers().First();
			var routeGen = MvcRouteGenerator.CreateGenerator(controller);
			Assert.AreEqual("/api/Things/{id}/dostuff", routeGen.GenerateRouteTemplate(controller.Actions.First()));
		}

		[TestMethod]
		public void DotNet_RoutedByConvention()
		{
			var collection = TypeCollectionBuilder.Create()
				.AddAspNetTypes()
				.AddNamedType("ThingsController")
					.WithBaseType(MvcConstants.ControllerBaseName, MvcConstants.AspNetNamespace)
					.AddMethod("RandoMethod", typeof(int))
						.AddScriptActionAttribute()
						.Commit()
					.BuildAsController()
				.Build();

			var controller = collection.GetMvcControllers().First();
			var routeGen = MvcRouteGenerator.CreateGenerator(controller);
			Assert.AreEqual("/Things/RandoMethod", routeGen.GenerateRouteTemplate(controller.Actions.First()));
		}

		[TestMethod]
		public void DotNet_Area_FromFolder_RoutedByConvention()
		{
			var collection = TypeCollectionBuilder.Create()
				.AddAspNetTypes()
				.AddNamedType("ThingsController")
					.WithFilePath(@"C:\FolderA\Areas\ThingsArea\Controllers\ThingsController.cs")
					.WithBaseType(MvcConstants.ControllerBaseName, MvcConstants.AspNetNamespace)
					.AddMethod("RandoMethod", typeof(int))
						.AddScriptActionAttribute()
						.Commit()
					.BuildAsController()
				.Build();

			var controller = collection.GetMvcControllers().First();
			var routeGen = MvcRouteGenerator.CreateGenerator(controller);

			Assert.AreEqual("/ThingsArea/Things/RandoMethod", routeGen.GenerateRouteTemplate(controller.Actions.First()));
		}

		[TestMethod]
		public void DotNet_Area_FromRouteArea_RoutedByConvention()
		{
			var collection = TypeCollectionBuilder.Create()
				.AddAspNetTypes()
				.AddNamedType("ThingsController")
					.WithBaseType(MvcConstants.ControllerBaseName, MvcConstants.AspNetNamespace)
					.AddAttribute(MvcConstants.RouteAreaAttributeFullName_AspNet, "ThingsArea")
					.AddMethod("RandoMethod", typeof(int))
						.AddScriptActionAttribute()
						.Commit()
					.BuildAsController()
				.Build();

			var controller = collection.GetMvcControllers().First();
			var routeGen = MvcRouteGenerator.CreateGenerator(controller);
			Assert.AreEqual("/ThingsArea/Things/RandoMethod", routeGen.GenerateRouteTemplate(controller.Actions.First()));
		}

		[TestMethod]
		public void DotNet_HttpGet_RoutedByAttribute()
		{
			var collection = TypeCollectionBuilder.Create()
				.AddAspNetTypes()
				.AddNamedType("ThingsController")
					.WithBaseType(MvcConstants.ControllerBaseName, MvcConstants.AspNetNamespace)
					.AddAttribute(MvcConstants.RouteAttributeFullName_AspNet, "api/Things")
					.AddMethod("RandoMethod", typeof(int))
						.AddScriptActionAttribute()
						.AddAttribute(MvcConstants.HttpGetAttributeFullName_AspNet, "GetThings")
						.Commit()
					.BuildAsController()
				.Build();

			var controller = collection.GetMvcControllers().First();
			var routeGen = MvcRouteGenerator.CreateGenerator(controller);
			Assert.AreEqual("/api/Things/GetThings", routeGen.GenerateRouteTemplate(controller.Actions.First()));
		}
	}
}
