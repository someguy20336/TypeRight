using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.Configuration;
using TypeRight.ScriptWriting.TypeScript;
using TypeRight.Tests.TestBuilders;
using TypeRight.Tests.Testers;

namespace TypeRight.Tests.Controllers
{
	[TestClass]
	public class ControllerModelTests : ControllerTestsBase
	{

		protected override bool IsAspNetCore => true;

		[TestInitialize]
		public override void TestInitialize()
		{
			base.TestInitialize();

			GivenActionConfig(new ActionConfig()
			{
				FetchFilePath = @".\FolderM\FolderN\AjaxFuncGet.ts",
				FetchFunctionName = "callGet",
				Imports = null,
				Method = "GET",
				Parameters = null,
				ReturnType = "void"
			});

			GivenActionConfig(new ActionConfig()
			{
				FetchFilePath = @".\FolderM\FolderN\AjaxFuncPost.ts",
				FetchFunctionName = "callPost",
				Imports = null,
				Method = "POST",
				Parameters = null,
				ReturnType = "void"
			});
		}


		[TestMethod]
		public void IdentifiesQueryParameter()
		{
			AddControllerAction("FromQuerySingleParamAction", "string")
					.AddParameter("id", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
					.AddLineOfCode("return null", 0)
					.Commit();

			AssertThatThisControllerActionModel("FromQuerySingleParamAction")
				.ParameterSourceTypeIs("id", ActionParameterSourceType.Query);
		}

		[TestMethod]
		public void IdentifiesQueryAndBodyParams()
		{
			AddControllerAction("HasFromQueryAndBody", "string")
				.AddParameter("id", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
				.AddParameter("body", "int", attribute: MvcConstants.FromBodyAttributeFullName_AspNetCore)
				.AddLineOfCode("return null", 0)
				.Commit();

			AssertThatThisControllerActionModel("HasFromQueryAndBody")
				.ParameterSourceTypeIs("id", ActionParameterSourceType.Query)
				.ParameterSourceTypeIs("body", ActionParameterSourceType.Body);
		}

		[TestMethod]
		public void FromServicesIsIgnored()
		{
			AddControllerAction("HasFromQueryAndServices", "string")
				.AddParameter("id", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
				.AddParameter("service", "int", attribute: MvcConstants.FromServicesAttributeFullName_AspNetCore)
				.AddLineOfCode("return null", 0)
				.Commit();

			AssertThatThisControllerActionModel("HasFromQueryAndServices")
				.ParameterSourceTypeIs("id", ActionParameterSourceType.Query)
				.ParameterSourceTypeIs("service", ActionParameterSourceType.Ignored);
		}

		[TestMethod]
		public void HttpGet_UsesDefaultActionConfig()
		{
			AddControllerAction("HasFromQueryAndBody", "string")
				.AddParameter("id", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
				.AddParameter("body", "int", attribute: MvcConstants.FromBodyAttributeFullName_AspNetCore)
				.AddLineOfCode("return null", 0)
				.Commit();

			AssertThatThisControllerActionModel("HasFromQueryAndBody")
				.FetchFunctionIs("TestAjax");
		}

		[TestMethod]
		public void HttpGet_UsesGetActionConfig()
		{
			AddControllerAction("GetMethod", "string")
				.AddAttribute(MvcConstants.HttpGetAttributeFullName_AspNetCore).Commit()
				.AddParameter("id", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
				.AddParameter("service", "int", attribute: MvcConstants.FromServicesAttributeFullName_AspNetCore)
				.AddLineOfCode("return null", 0)
				.Commit();

			AssertThatThisControllerActionModel("GetMethod")
				.FetchFunctionIs("callGet");
		}

		[TestMethod]
		public void HttpPost_UsesPostActionConfig()
		{
			AddControllerAction("PostMethod", "string")
				.AddAttribute(MvcConstants.HttpPostAttributeFullName_AspNetCore).Commit()
				.AddParameter("id", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
				.AddParameter("service", "int", attribute: MvcConstants.FromServicesAttributeFullName_AspNetCore)
				.AddLineOfCode("return null", 0)
				.Commit();

			AssertThatThisControllerActionModel("PostMethod")
				.FetchFunctionIs("callPost");
		}

		[TestMethod]
		public void ImportsFetchFile()
		{
			// Turns out actions are required
			AddControllerAction("PostMethod", "string")
				.AddAttribute(MvcConstants.HttpPostAttributeFullName_AspNetCore).Commit()
				.AddLineOfCode("return null", 0)
				.Commit();


			AssertThatThisController()
				.HasImportForFile(@"../../FolderM/FolderN/AjaxFuncPost");
		}

	}
}
