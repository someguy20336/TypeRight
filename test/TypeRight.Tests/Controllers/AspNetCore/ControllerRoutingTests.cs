using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.ScriptWriting.TypeScript.PartialTextTemplates;
using TypeRight.Tests.TestBuilders;

namespace TypeRight.Tests.Controllers.AspNetCore
{
	[TestClass]
	public class ControllerRoutingTests : ControllerTestsBase
	{
		/// <summary>
		/// Sets up a parse of this solution
		/// </summary>
		[TestInitialize]
		public override void TestInitialize()
		{
			base.TestInitialize();
		}

		[TestMethod]
		public void RouteAttribute_UsedInBaseURL()
		{
			ControllerBuilder
				.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore)
					.AddConstructorArg("\"api/[controller]\"")
					.Commit();

			AddControllerAction("GetStringList", "List<string>")
				.AddAttribute(MvcConstants.HttpGetAttributeFullName_AspNetCore).AddConstructorArg("\"getList\"").Commit()
				.AddLineOfCode("return new List<string>();", 0)
				.Commit();

			// Asp net core
			AssertThatThisControllerAction("GetStringList")
				.RouteTemplateIs($"/api/{ControllerName}/getList", "");

		}

		[TestMethod]
		public void RoutedApiController_ScriptWritten()
		{
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
				.AddMethod("PutThingWithQuery", "string")
					.AddScriptActionAttribute()
					.AddAttribute(MvcConstants.HttpPutAttributeFullName_AspNetCore)
						.AddConstructorArg("\"thing/{thingId}/put\"").Commit()
					.AddParameter("thingId", "string")
					.AddParameter("query", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
					.AddParameter("body", "bool", attribute: MvcConstants.FromBodyAttributeFullName_AspNetCore)
					.AddLineOfCode("return null", 0)
					.Commit()
				.AddMethod("DeleteThing", "string")
					.AddScriptActionAttribute()
					.AddAttribute(MvcConstants.ToAspNetCoreFullName(MvcConstants.HttpDeleteAttributeName))
						.AddConstructorArg("\"thing/{thingId}\"").Commit()
					.AddParameter("thingId", "string")
					.AddLineOfCode("return null", 0)
					.Commit()
					;
			string queryHelpers = new QueryParameterHelperFunctions(false).TransformText();

			AssertControllerGeneratedText(
			#region ScriptText	
				@"
import { fetchWrapper } from ""../../FolderM/FolderN/FetchFile"";

" + queryHelpers + @"
/**
 * 
 * @param thingId 
 */
export function DeleteThing(thingId: string): void {
	fetchWrapper(""DELETE"", `/api/RoutedApi/thing/${thingId}`, null);
}

/**
 * 
 * @param thingId 
 */
export function GetThing(thingId: string): void {
	fetchWrapper(""GET"", `/api/RoutedApi/thing/${thingId}`, null);
}

/**
 * 
 * @param thingId 
 * @param query 
 * @param body 
 */
export function PutThingWithQuery(thingId: string, query: string, body: boolean): void {
	const urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, ""query"", query);
	fetchWrapper(""PUT"", `/api/RoutedApi/thing/${thingId}/put${getQueryString(urlParams)}`, body);
}

"
			#endregion
				);
		}

		[TestMethod]
		public void NonRoutedController_HttpVerbIsRooted_UsesHttpVerbPath()
		{
			ControllerBuilder
				.AddMethod("GetThing", "string")
					.AddScriptActionAttribute()
					.AddAttribute(MvcConstants.HttpGetAttributeFullName_AspNetCore)
						.AddStringConstructorArg("/different/path/thing").Commit()
					.AddParameter("thingId", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
					.AddLineOfCode("return null", 0)
					.Commit()
					;

			AssertScriptTextForFunctionIs(@"
export function GetThing(thingId: string): void {
	const urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, ""thingId"", thingId);
	fetchWrapper(""GET"", `/different/path/thing${getQueryString(urlParams)}`, null);
}", ScriptExtensions.KeyValueQueryParamHelper);
		}

		[TestMethod]
		public void RoutedController_HttpVerbIsRooted_UsesHttpVerbPath()
		{
			ControllerBuilder
				.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore)
					.AddStringConstructorArg("RoutedApi")
					.Commit()
				.AddMethod("GetThing", "string")
					.AddScriptActionAttribute()
					.AddAttribute(MvcConstants.HttpGetAttributeFullName_AspNetCore)
						.AddStringConstructorArg("/different/path/thing").Commit()
					.AddParameter("thingId", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
					.AddLineOfCode("return null", 0)
					.Commit()
					;

			AssertScriptTextForFunctionIs(@"
export function GetThing(thingId: string): void {
	const urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, ""thingId"", thingId);
	fetchWrapper(""GET"", `/different/path/thing${getQueryString(urlParams)}`, null);
}", ScriptExtensions.KeyValueQueryParamHelper);
		}

		[TestMethod]
		public void NoRoute_NoHttpVerb_UsesConventionalRouting()
		{
			ControllerBuilder
				.AddMethod("GetThing", "string")
					.AddScriptActionAttribute()
					.AddParameter("thingId", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
					.AddLineOfCode("return null", 0)
					.Commit()
					;

			AssertScriptTextForFunctionIs(@$"
export function GetThing(thingId: string): void {{
	const urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, ""thingId"", thingId);
	fetchWrapper(""GET"", `/{ControllerName}/GetThing${{getQueryString(urlParams)}}`, null);
}}", ScriptExtensions.KeyValueQueryParamHelper);
		}


		[TestMethod]
		public void BaseUrl_AppendedToBeginning()
		{
			GivenBaseUrl("api");
			ControllerBuilder
				.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore)
					.AddConstructorArg("\"RoutedApi\"")
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
	const urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, ""thingId"", thingId);
	fetchWrapper(""GET"", `/api/RoutedApi/thing${getQueryString(urlParams)}`, null);
}", ScriptExtensions.KeyValueQueryParamHelper);
		}

		[TestMethod]
		public void BaseUrl_NoRoute_NoHttpVerb_UsesConventionalRouting()
		{
			GivenBaseUrl("api");
			ControllerBuilder
				.AddMethod("GetThing", "string")
					.AddScriptActionAttribute()
					.AddParameter("thingId", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
					.AddLineOfCode("return null", 0)
					.Commit()
					;

			AssertScriptTextForFunctionIs(@$"
export function GetThing(thingId: string): void {{
	const urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, ""thingId"", thingId);
	fetchWrapper(""GET"", `/api/{ControllerName}/GetThing${{getQueryString(urlParams)}}`, null);
}}", ScriptExtensions.KeyValueQueryParamHelper);
		}

		[TestMethod]
		public void BaseUrl_NoRoute_HttpVerbIsRooted_UsesHttpVerbPath()
		{
			GivenBaseUrl("api");
			ControllerBuilder
				.AddMethod("GetThing", "string")
					.AddScriptActionAttribute()
					.AddAttribute(MvcConstants.HttpGetAttributeFullName_AspNetCore)
						.AddStringConstructorArg("/different/path/thing").Commit()
					.AddParameter("thingId", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
					.AddLineOfCode("return null", 0)
					.Commit()
					;

			AssertScriptTextForFunctionIs(@"
export function GetThing(thingId: string): void {
	const urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, ""thingId"", thingId);
	fetchWrapper(""GET"", `/api/different/path/thing${getQueryString(urlParams)}`, null);
}", ScriptExtensions.KeyValueQueryParamHelper);
		}

		[TestMethod]
		public void BaseUrl_WithRoute_HttpVerbIsRooted_UsesHttpVerbPath()
		{
			GivenBaseUrl("api");
			ControllerBuilder
				.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore)
					.AddStringConstructorArg("RoutedApi")
					.Commit()
				.AddMethod("GetThing", "string")
					.AddScriptActionAttribute()
					.AddAttribute(MvcConstants.HttpGetAttributeFullName_AspNetCore)
						.AddStringConstructorArg("/different/path/thing").Commit()
					.AddParameter("thingId", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
					.AddLineOfCode("return null", 0)
					.Commit()
					;

			AssertScriptTextForFunctionIs(@"
export function GetThing(thingId: string): void {
	const urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, ""thingId"", thingId);
	fetchWrapper(""GET"", `/api/different/path/thing${getQueryString(urlParams)}`, null);
}", ScriptExtensions.KeyValueQueryParamHelper);
		}
	}
}
