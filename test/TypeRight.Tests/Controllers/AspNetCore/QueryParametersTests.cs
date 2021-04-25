using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.Tests.TestBuilders;

namespace TypeRight.Tests.Controllers.AspNetCore
{
	[TestClass]
	public class QueryParametersTests : ControllerTestsBase
	{

		protected override bool IsAspNetCore => true;

		[TestMethod]
		public void SingleSimpleFromQueryParam_KeyValueFuncAdded_Generated()
		{
			AddControllerAction("TestAction", MvcConstants.JsonResult_AspNetCore)
					.AddParameter("fromQuery", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
					.Commit()
			;

			AssertScriptTextForFunctionIs(@$"
export function TestAction(fromQuery: string): void {{
	let urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, ""fromQuery"", fromQuery);
	let queryString = """";
	if (urlParams.getAll().length > 0) {{
		queryString = ""?"" + urlParams.toString();
	}}
	TestAjax(`/{ControllerName}/TestAction${{queryString}}`, null);
}}", ScriptExtensions.KeyValueQueryParamHelper);
		}

		[TestMethod]
		public void SingleObjectFromQueryParam_BothQueryHelperFuncsAdded_Generated()
		{
			AddControllerAction("TestAction", MvcConstants.JsonResult_AspNetCore)
					.AddParameter("fromQuery", "TestClass", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
					.Commit()
			;

			AssertScriptTextForFunctionIs(@$"
export function TestAction(fromQuery: DefaultResult.TestClass): void {{
	let urlParams = new URLSearchParams();
	tryAppendObjectValuesToUrl(urlParams, fromQuery);
	let queryString = """";
	if (urlParams.getAll().length > 0) {{
		queryString = ""?"" + urlParams.toString();
	}}
	TestAjax(`/{ControllerName}/TestAction${{queryString}}`, null);
}}", ScriptExtensions.KeyValueQueryParamHelper | ScriptExtensions.ObjectQueryParamHelper);
		}

		[TestMethod]
		public void ComplexAndSimpleParameters_BothQueryHelperFuncsAdded_ScriptGenerated()
		{
			AddControllerAction("TestAction", MvcConstants.JsonResult_AspNetCore)
					.AddParameter("simple", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
					.AddParameter("complex", "TestClass", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
					.Commit()
			;

			AssertScriptTextForFunctionIs(@$"
export function TestAction(simple: string, complex: DefaultResult.TestClass): void {{
	let urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, ""simple"", simple);
	tryAppendObjectValuesToUrl(urlParams, complex);
	let queryString = """";
	if (urlParams.getAll().length > 0) {{
		queryString = ""?"" + urlParams.toString();
	}}
	TestAjax(`/{ControllerName}/TestAction${{queryString}}`, null);
}}", ScriptExtensions.KeyValueQueryParamHelper | ScriptExtensions.ObjectQueryParamHelper);
		}


		[TestMethod]
		public void ConfiguredQueryParams_Multi_AddedToUrl_ScriptWritten()
		{
			GivenQueryParameter("key1", "val1");
			GivenQueryParameter("key2", "val2");
			ControllerBuilder
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
					;

			AssertScriptTextForFunctionIs(@"
export function GetThing(thingId: string): void {
	let urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, ""key1"", ""val1"");
	tryAppendKeyValueToUrl(urlParams, ""key2"", ""val2"");
	let queryString = """";
	if (urlParams.getAll().length > 0) {
		queryString = ""?"" + urlParams.toString();
	}
	TestAjax(`/api/RoutedApi/thing/${thingId}${queryString}`, null);
}", ScriptExtensions.KeyValueQueryParamHelper);
		}

		[TestMethod]
		public void ConfiguredQueryParams_WithFromQuery_AddedToUrl_ScriptWritten()
		{
			GivenQueryParameter("key1", "val1");
			ControllerBuilder
				.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore)
					.AddConstructorArg("\"api/RoutedApi\"")
					.Commit()
				.AddMethod("GetThing", "string")
					.AddScriptActionAttribute()
					.AddAttribute(MvcConstants.HttpGetAttributeFullName_AspNetCore)
						.AddConstructorArg("\"thing\"").Commit()
					.AddParameter("thingId", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
					.AddLineOfCode("return null", 0)
					.Commit()
					;

			AssertScriptTextForFunctionIs(@"
export function GetThing(thingId: string): void {
	let urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, ""thingId"", thingId);
	tryAppendKeyValueToUrl(urlParams, ""key1"", ""val1"");
	let queryString = """";
	if (urlParams.getAll().length > 0) {
		queryString = ""?"" + urlParams.toString();
	}
	TestAjax(`/api/RoutedApi/thing${queryString}`, null);
}", ScriptExtensions.KeyValueQueryParamHelper);
		}

		[TestMethod]
		public void ConfiguredQueryParams_Single_AddedToUrl_ScriptWritten()
		{
			GivenQueryParameter("key1", "val1");
			ControllerBuilder
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
					;

			AssertScriptTextForFunctionIs(@"
export function GetThing(thingId: string): void {
	let urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, ""key1"", ""val1"");
	let queryString = """";
	if (urlParams.getAll().length > 0) {
		queryString = ""?"" + urlParams.toString();
	}
	TestAjax(`/api/RoutedApi/thing/${thingId}${queryString}`, null);
}", ScriptExtensions.KeyValueQueryParamHelper);
		}

	}
}
