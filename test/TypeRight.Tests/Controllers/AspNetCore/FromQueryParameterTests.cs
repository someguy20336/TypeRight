using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TypeRight.Tests.Controllers.AspNetCore
{
	[TestClass]
	public class FromQueryParameterTests : ControllerTestsBase
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
	}
}
