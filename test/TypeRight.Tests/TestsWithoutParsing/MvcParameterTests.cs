using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TypeRight.Configuration;
using TypeRight.Tests.TestBuilders.TypeCollection;

namespace TypeRight.Tests.TestsWithoutParsing
{
	[TestClass]
	public class MvcParameterTests
	{
		[TestMethod]
		public void MethodParameters_RequiredAreFirst()
		{
			var tester = TypeCollectionBuilder.Create()
				.AddAspNetCoreTypes()
				.AddNamedType("TestController")
					.WithBaseType(MvcConstants.ControllerBaseName, MvcConstants.AspNetCoreNamespace)
					.AddMethod("CustomMethod", typeof(int))
						.AddScriptActionAttribute()
						.AddParameter("param1", typeof(string), false)
						.AddParameter("param2", typeof(string), false)
						.AddParameter("param3", typeof(string), true)
						.Commit()
					.BuildAsController()
				.BuildAndTest();

			// Test that the order of parameters is maintained.  Kind of a "control" test here
			tester.TestControllerWithName("TestController")
				.TestActionModelWithName("CustomMethod")
					.ParameterAtIndexIs(0, "param1", false)
					.ParameterAtIndexIs(1, "param2", false)
					.ParameterAtIndexIs(2, "param3", true);			
		}

		[TestMethod]
		public void MethodParameters_OptionalAndRequiredUserParameters_RequiredAreFirst()
		{
			var tester = TypeCollectionBuilder.Create()
				.AddAspNetCoreTypes()
				.AddNamedType("TestController")
					.WithBaseType(MvcConstants.ControllerBaseName, MvcConstants.AspNetCoreNamespace)
					.AddMethod("CustomMethod", typeof(int))
						.AddScriptActionAttribute()
						.AddParameter("param1", typeof(string), false)
						.AddParameter("param2", typeof(string), false)
						.AddParameter("param3", typeof(string), true)
						.Commit()
					.BuildAsController()
				.BuildAndTest();

			// Setup action parameters
			List<ActionParameter> actionParameters = new List<ActionParameter>()
			{
				new ActionParameter()
				{
					Name = "userParam1",
					Optional = false,
					Type = "string"
				},
				new ActionParameter()
				{
					Name = "userParam2",
					Optional = true,
					Type = "string"
				}
			};
			var actionConfig = tester.GetDefaultActionConfig();
			actionConfig.First().Parameters = actionParameters;
			var context = tester.GetDefaultControllerContext(actionConfig);

			// Required user param comes before optional ones
			tester.TestControllerWithName("TestController", context)
				.TestActionModelWithName("CustomMethod")
					.ParameterAtIndexIs(2, "userParam1", false)
					.ParameterAtIndexIs(3, "param3", true)
					.ParameterAtIndexIs(4, "userParam2", true)
					;
		}
	}
}
