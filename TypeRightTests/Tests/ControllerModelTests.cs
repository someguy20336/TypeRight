using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.Configuration;
using TypeRight.ScriptWriting.TypeScript;
using TypeRight.TypeLocation;
using TypeRight.TypeProcessing;
using TypeRightTests.TestBuilders;
using TypeRightTests.Testers;

namespace TypeRightTests.Tests
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
					.Commit();
			

			s_packageTester = wkspBuilder.GetPackageTester();
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

		[TestMethod]
		public void HttpGet_UsesDefaultActionConfig()
		{
			GetControllerTester()
				.TestActionModelWithName("HasFromQueryAndBody")
				.FetchFunctionIs("TestAjax");
		}

		[TestMethod]
		public void HttpGet_UsesGetActionConfig()
		{
			GetControllerTester()
				.TestActionModelWithName("GetMethod")
				.FetchFunctionIs("callGet");
		}

		[TestMethod]
		public void HttpGet_UsesPostActionConfig()
		{
			GetControllerTester()
				.TestActionModelWithName("PostMethod")
				.FetchFunctionIs("callPost");
		}

		[TestMethod]
		public void ImportsAllFetchFiles()
		{
			GetControllerTester()
				.HasImportForFile(@"../../FolderM/FolderN/AjaxFunc")
				.HasImportForFile(@"../../FolderM/FolderN/AjaxFuncPost")
				.HasImportForFile(@"../../FolderM/FolderN/AjaxFuncGet");
		}

		private ControllerTester GetControllerTester()
		{
			var actionConfig = s_packageTester.GetDefaultActionConfig();
			actionConfig.Add(new ActionConfig()
			{
				FetchFilePath = @".\FolderM\FolderN\AjaxFuncGet.ts",
				FetchFunctionName = "callGet",
				Imports = null,
				Method = RequestMethod.Get,
				Parameters = null,
				ReturnType = "void"
			});

			actionConfig.Add(new ActionConfig()
			{
				FetchFilePath = @".\FolderM\FolderN\AjaxFuncPost.ts",
				FetchFunctionName = "callPost",
				Imports = null,
				Method = RequestMethod.Post,
				Parameters = null,
				ReturnType = "void"
			});
			var context = s_packageTester.GetDefaultControllerContext(actionConfig);
			context.ModelBinding = ModelBindingType.SingleParam;

			return s_packageTester.TestControllerWithName("TestController", context);
		}
	}
}
