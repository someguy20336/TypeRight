using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TypeRight.Configuration;
using TypeRight.Tests.TestBuilders;

namespace TypeRight.Tests.Controllers.AspNetCore
{
	[TestClass]
	public class OldActionConfigTests : ControllerTestsBase
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
		public void HttpGet_UsesDefaultActionConfig()
		{
			AddControllerAction("HasFromQueryAndBody", "string")
				.AddParameter("id", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
				.AddParameter("body", "int", attribute: MvcConstants.FromBodyAttributeFullName_AspNetCore)
				.AddLineOfCode("return null", 0)
				.Commit();

			AssertThatThisControllerAction("HasFromQueryAndBody")
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

			AssertThatThisControllerAction("GetMethod")
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

			AssertThatThisControllerAction("PostMethod")
				.FetchFunctionIs("callPost");
		}


		// TODO: should this go somewhere else?
		[TestMethod]
		public void MethodParameters_RequiredAreFirst()
		{
			var attrs = new List<AttributeInfo>() { new AttributeInfo() { AttributeTypeName = MvcConstants.FromQueryAttributeFullName_AspNetCore } };
			AddControllerAction("CustomMethod", "int")
				.AddParameter("param1", "string", "", attrs) 
				.AddParameter("param2", "string", "", attrs)
				.AddParameter("param3", "string", "", attrs, true)
				.Commit();

			// Test that the order of parameters is maintained.  Kind of a "control" test here
			AssertScriptTextForFunctionIs(@"
export function CustomMethod(param1: string, param2: string, param3?: string): void {
	let urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, ""param1"", param1);
	tryAppendKeyValueToUrl(urlParams, ""param2"", param2);
	tryAppendKeyValueToUrl(urlParams, ""param3"", param3);
	let queryString = """";
	if (urlParams.getAll().length > 0) {
		queryString = ""?"" + urlParams.toString();
	}
	TestAjax(`/Test/CustomMethod${queryString}`, null);
}", ScriptExtensions.KeyValueQueryParamHelper);
			
		}

		[TestMethod]
		public void MethodParameters_OptionalAndRequiredUserParameters_RequiredAreFirst()
		{
			// Setup action parameters
			List<ActionParameter> actionParameters = new List<ActionParameter>()
			{
				new ActionParameter("userParam1", "string", false),
				new ActionParameter("userParam2", "string", true)
			};
			GivenActionParameters(actionParameters);

			var attrs = new List<AttributeInfo>() { new AttributeInfo() { AttributeTypeName = MvcConstants.FromQueryAttributeFullName_AspNetCore } };
			AddControllerAction("CustomMethod", "int")
				.AddParameter("param1", "string", "", attrs)
				.AddParameter("param2", "string", "", attrs)
				.AddParameter("param3", "string", "", attrs, true)
				.Commit();

			// Test that the order of parameters is maintained.  Kind of a "control" test here
			AssertScriptTextForFunctionIs(@"
export function CustomMethod(param1: string, param2: string, userParam1: string, param3?: string, userParam2?: string): void {
	let urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, ""param1"", param1);
	tryAppendKeyValueToUrl(urlParams, ""param2"", param2);
	tryAppendKeyValueToUrl(urlParams, ""param3"", param3);
	let queryString = """";
	if (urlParams.getAll().length > 0) {
		queryString = ""?"" + urlParams.toString();
	}
	TestAjax(`/Test/CustomMethod${queryString}`, null, userParam1, userParam2);
}", ScriptExtensions.KeyValueQueryParamHelper);
		}
	}
}
