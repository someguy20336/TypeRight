using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.Tests.TestBuilders;

namespace TypeRight.Tests.Controllers.AspNet
{
	[TestClass]
	public class RoutingTests : ControllerTestsBase
	{

		protected override bool IsAspNetCore => false;


		[TestMethod]
		public void RouteAttribute_UsedInBaseURL()
		{
			ControllerBuilder
				.AddAttribute(MvcConstants.RouteAttributeFullName_AspNet)
					.AddConstructorArg("\"api/asp/[controller]/[action]\"")
					.Commit();

			AddControllerAction("GetStringList", "List<string>")
				.AddLineOfCode("return new List<string>();", 0)
				.Commit();

			// asp.net
			AssertThatThisControllerAction("GetStringList")
				.RouteTemplateIs($"/api/asp/{ControllerName}/GetStringList", "");
		}
	}
}
