using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.Configuration;
using TypeRight.ScriptWriting.TypeScript;
using TypeRight.TypeLocation;
using TypeRight.TypeProcessing;
using TypeRight.Workspaces.Parsing;
using TypeRightTests.TestBuilders;
using TypeRightTests.Testers;

namespace TypeRightTests.Tests
{
	[TestClass]
	public class ControllerModelTests
	{
		private static TypeCollectionTester _packageTester;

		/// <summary>
		/// Sets up a parse of this solution
		/// </summary>
		[ClassInitialize]
		public static void SetupParse(TestContext context)
		{

			TestWorkspaceBuilder wkspBuilder = new TestWorkspaceBuilder();

			wkspBuilder.DefaultProject
				.AddFakeTypeRight()
				.AddFakeMvc()

				.CreateClassBuilder("TestController")
					.WithControllerBaseClass()
					.AddMethod("FromQuerySingleParamAction", "string")
						.AddScriptActionAttribute()
						.AddParameter("id", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
						.AddLineOfCode("return null", 0)
						.Commit()

					.AddMethod("HasFromQueryAndBody", "string")
						.AddScriptActionAttribute()
						.AddParameter("id", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
						.AddParameter("body", "int", attribute: MvcConstants.FromBodyAttributeFullName_AspNetCore)
						.AddLineOfCode("return null", 0)
						.Commit()

					.AddMethod("HasFromQueryAndServices", "string")
						.AddScriptActionAttribute()
						.AddParameter("id", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
						.AddParameter("service", "int", attribute: MvcConstants.FromServicesAttributeFullName_AspNetCore)
						.AddLineOfCode("return null", 0)
						.Commit()
					.Commit();
			
			wkspBuilder.FilterSettings = new ParseFilterSettings();

			_packageTester = wkspBuilder.GetPackageTester();
		}

		[TestMethod]
		public void IdentifiesQueryParameter()
		{
			GetControllerTester()
				.TestActionModelWithName("FromQuerySingleParamAction")
				.ParameterSourceTypeIs("id", ActionParameterSourceType.Query);
		}

		[TestMethod]
		public void IdentifiesQueryAndBodyParams()
		{
			GetControllerTester()
				.TestActionModelWithName("HasFromQueryAndBody")
				.ParameterSourceTypeIs("id", ActionParameterSourceType.Query)
				.ParameterSourceTypeIs("body", ActionParameterSourceType.Body);
		}

		[TestMethod]
		public void FromServicesIsIgnored()
		{
			GetControllerTester()
				.TestActionModelWithName("HasFromQueryAndServices")
				.ParameterSourceTypeIs("id", ActionParameterSourceType.Query)
				.ParameterSourceTypeIs("service", ActionParameterSourceType.Ignored);
		}

		private ControllerTester GetControllerTester()
		{
			var context = _packageTester.GetDefaultControllerContext();
			context.ModelBinding = ModelBindingType.SingleParam;

			return _packageTester.TestControllerWithName("TestController", context);
		}
	}
}
