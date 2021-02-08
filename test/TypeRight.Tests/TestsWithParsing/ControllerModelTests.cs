using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.Configuration;
using TypeRight.ScriptWriting.TypeScript;
using TypeRight.TypeLocation;
using TypeRight.TypeProcessing;
using TypeRight.Tests.TestBuilders;
using TypeRight.Tests.Testers;

namespace TypeRight.Tests.TestsWithParsing
{
	[TestClass]
	public class ControllerModelTests
	{
		private static TypeCollectionTester s_packageTester;

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

					.AddMethod("GetMethod", "string")
						.AddScriptActionAttribute()
						.AddAttribute(MvcConstants.HttpGetAttributeFullName_AspNetCore).Commit()
						.AddParameter("id", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
						.AddParameter("service", "int", attribute: MvcConstants.FromServicesAttributeFullName_AspNetCore)
						.AddLineOfCode("return null", 0)
						.Commit()

					.AddMethod("PostMethod", "string")
						.AddScriptActionAttribute()
						.AddAttribute(MvcConstants.HttpPostAttributeFullName_AspNetCore).Commit()
						.AddParameter("id", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
						.AddParameter("service", "int", attribute: MvcConstants.FromServicesAttributeFullName_AspNetCore)
						.AddLineOfCode("return null", 0)
						.Commit()
					.Commit()
					
				.CreateClassBuilder("RoutedApiController")
					.WithControllerBaseClass()
					.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore)
						.AddConstructorArg("\"api/RoutedApi\"")
						.Commit()
					.AddMethod("GetThing", "string")
						.AddScriptActionAttribute()
						.AddAttribute(MvcConstants.HttpGetAttributeFullName_AspNetCore)
							.AddConstructorArg("\"thing/{thingId}\"").Commit()
						.AddParameter("thingId", "string")
						.AddLineOfCode("return null", 0)
						.Commit()
					.Commit()
					;
			

			s_packageTester = wkspBuilder.GetPackageTester();
		}

		[TestMethod]
		public void IdentifiesQueryParameter()
		{
			TestNonRoutedController()
				.TestActionModelWithName("FromQuerySingleParamAction")
				.ParameterSourceTypeIs("id", ActionParameterSourceType.Query);
		}

		[TestMethod]
		public void IdentifiesQueryAndBodyParams()
		{
			TestNonRoutedController()
				.TestActionModelWithName("HasFromQueryAndBody")
				.ParameterSourceTypeIs("id", ActionParameterSourceType.Query)
				.ParameterSourceTypeIs("body", ActionParameterSourceType.Body);
		}

		[TestMethod]
		public void FromServicesIsIgnored()
		{
			TestNonRoutedController()
				.TestActionModelWithName("HasFromQueryAndServices")
				.ParameterSourceTypeIs("id", ActionParameterSourceType.Query)
				.ParameterSourceTypeIs("service", ActionParameterSourceType.Ignored);
		}

		[TestMethod]
		public void HttpGet_UsesDefaultActionConfig()
		{
			TestNonRoutedController()
				.TestActionModelWithName("HasFromQueryAndBody")
				.FetchFunctionIs("TestAjax");
		}

		[TestMethod]
		public void HttpGet_UsesGetActionConfig()
		{
			TestNonRoutedController()
				.TestActionModelWithName("GetMethod")
				.FetchFunctionIs("callGet");
		}

		[TestMethod]
		public void HttpGet_UsesPostActionConfig()
		{
			TestNonRoutedController()
				.TestActionModelWithName("PostMethod")
				.FetchFunctionIs("callPost");
		}

		[TestMethod]
		public void ImportsAllFetchFiles()
		{
			TestNonRoutedController()
				.HasImportForFile(@"../../FolderM/FolderN/AjaxFunc")
				.HasImportForFile(@"../../FolderM/FolderN/AjaxFuncPost")
				.HasImportForFile(@"../../FolderM/FolderN/AjaxFuncGet");
		}

		[TestMethod]
		public void HttpGet_RouteParameterInTemplate()
		{
			TestRoutedController()
				.TestActionModelWithName("GetThing")
				.RouteTemplateIs("/api/RoutedApi/thing/{thingId}");
		}

		private ControllerTester TestNonRoutedController() => CreateControllerTester("TestController");
		private ControllerTester TestRoutedController() => CreateControllerTester("RoutedApiController");

		private ControllerTester CreateControllerTester(string name)
		{
			var actionConfig = s_packageTester.GetDefaultActionConfig();
			actionConfig.Add(new ActionConfig()
			{
				FetchFilePath = @".\FolderM\FolderN\AjaxFuncGet.ts",
				FetchFunctionName = "callGet",
				Imports = null,
				Method = "GET",
				Parameters = null,
				ReturnType = "void"
			});

			actionConfig.Add(new ActionConfig()
			{
				FetchFilePath = @".\FolderM\FolderN\AjaxFuncPost.ts",
				FetchFunctionName = "callPost",
				Imports = null,
				Method = "POST",
				Parameters = null,
				ReturnType = "void"
			});
			var context = s_packageTester.GetDefaultControllerContext(actionConfig);

			return s_packageTester.TestControllerWithName(name, context);
		}
	}
}
