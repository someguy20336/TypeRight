using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TypeRight.ScriptWriting;
using TypeRight.Tests.TestBuilders.TypeCollection;
using TypeRight.TypeProcessing;

namespace TypeRight.Tests.Controllers
{
	public abstract class RouteGeneratorTestBase
	{
		private TypeCollectionBuilder _builder;
		private string _basePath = "";

		protected NamedTypeBuilder Controller { get; private set; }

		protected MethodBuilder Action { get; private set; }

		protected abstract string ControllerNamespace { get; }

		protected abstract void AddMvcTypes(TypeCollectionBuilder builder);

		[TestInitialize]
		public void Initialize()
		{
			_builder = TypeCollectionBuilder.Create();
			AddMvcTypes(_builder);

			Controller = _builder.AddNamedType("ThingsController")
				.WithBaseType(MvcConstants.ControllerBaseName, ControllerNamespace);

			Action = Controller.AddMethod("RandoMethod", typeof(int))
				.AddScriptActionAttribute();
		}

		protected void GivenBaseUrl(string path) => _basePath = path;

		protected void AssertRouteEquals(string expectedRoute)
		{
			Action.Commit();
			Controller.BuildAsController();
			var collection = _builder.Build();
			var controller = collection.GetMvcControllers().First();
			var routeGen = MvcRouteGenerator.CreateGenerator(controller, _basePath);
			Assert.AreEqual(expectedRoute, routeGen.GenerateRouteTemplate(controller.Actions.First()));
		}
	}
}
