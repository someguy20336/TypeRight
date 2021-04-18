using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.Tests.TestBuilders;

namespace TypeRight.Tests.Controllers
{
	public abstract class RouteGeneratorTestBase : ControllerTestsBase
	{

		protected TestMethodBuilder Action { get; private set; }

		protected abstract string ControllerNamespace { get; }

		protected override string ControllerName => "Things";

		[TestInitialize]
		public void Initialize()
		{
			base.TestInitialize();

			Action = AddControllerAction("RandoMethod", "int");

		}

		protected void AssertRouteEquals(string expectedRoute)
		{
			Action.Commit();

			AssertThatThisControllerAction("RandoMethod")
				.RouteTemplateIs(expectedRoute, ConfigOptions.BaseUrl);
		}
	}
}
