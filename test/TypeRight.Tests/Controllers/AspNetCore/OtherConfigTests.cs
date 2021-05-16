using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.ScriptWriting;
using TypeRight.Tests.TestBuilders;

namespace TypeRight.Tests.Controllers.AspNetCore
{
	[TestClass]
	public class OtherConfigTests : ControllerTestsBase
	{

		[TestMethod]
		public void QueryParams_Configured_Multi_AddedToUrl_ScriptWritten()
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
	fetchWrapper(""GET"", `/api/RoutedApi/thing/${thingId}${getQueryString(urlParams)}`, null);
}", ScriptExtensions.KeyValueQueryParamHelper);
		}

		[TestMethod]
		public void QueryParams_Configured_WithFromQuery_AddedToUrl_ScriptWritten()
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
	fetchWrapper(""GET"", `/api/RoutedApi/thing${getQueryString(urlParams)}`, null);
}", ScriptExtensions.KeyValueQueryParamHelper);
		}

		[TestMethod]
		public void QueryParams_Configured_Single_AddedToUrl_ScriptWritten()
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
	fetchWrapper(""GET"", `/api/RoutedApi/thing/${thingId}${getQueryString(urlParams)}`, null);
}", ScriptExtensions.KeyValueQueryParamHelper);
		}

		[TestMethod]
		public void NameCasing_Camel_ScriptIsCorrect()
		{
			AddControllerAction("CamelCasedTestAction", MvcConstants.JsonResult_AspNetCore)
				.Commit()
				;

			ConfigOptions.NameCasingConverter = NamingStrategyType.Camel;

			AssertScriptTextForFunctionIs(@$"
export function camelCasedTestAction(): void {{
	fetchWrapper(""GET"", `/{ControllerName}/CamelCasedTestAction`, null);
}}");
		}

	}
}
