using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.Tests.TestBuilders;

namespace TypeRight.Tests.Controllers.AspNetCore
{
	[TestClass]
	public class ControllerRoutingTests : ControllerTestsBase
	{
		protected override bool IsAspNetCore => true;

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

			AssertControllerGeneratedText(
			#region ScriptText	
				@"
import { TestAjax, callDelete } from ""../../FolderM/FolderN/AjaxFunc"";


/**
 * 
 * @param thingId 
 */
export function DeleteThing(thingId: string): void {
	callDelete(`/api/RoutedApi/thing/${thingId}`, null);
}

/**
 * 
 * @param thingId 
 */
export function GetThing(thingId: string): void {
	TestAjax(`/api/RoutedApi/thing/${thingId}`, null);
}

/**
 * 
 * @param thingId 
 * @param query 
 * @param body 
 */
export function PutThingWithQuery(thingId: string, query: string, body: boolean): void {
	let urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, ""query"", query);
	let queryString = """";
	if (urlParams.getAll().length > 0) {
		queryString = ""?"" + urlParams.toString();
	}
	TestAjax(`/api/RoutedApi/thing/${thingId}/put${queryString}`, body);
}

function tryAppendKeyValueToUrl(urlParams: URLSearchParams, key: string, value: any): void {
    if (value !== null && typeof value !== ""undefined"") {
        if (Array.isArray(val)) {
            for (let aryVal of val) {
                urlParams.append(key, aryVal.toString());
            }
        } else {
            urlParams.append(key, val);
        }
    }
}
"
			#endregion
				);
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
	TestAjax(`/api/RoutedApi/thing/${thingId}?key1=val1`, null);
}");
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
	TestAjax(`/api/RoutedApi/thing/${thingId}?key1=val1&key2=val2`, null);
}");
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
	TestAjax(`/api/RoutedApi/thing?key1=val1&thingId=${ thingId ?? """" }`, null);
}");
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
	let urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, ""thingId"", thingId);
	let queryString = """";
	if (urlParams.getAll().length > 0) {
		queryString = ""?"" + urlParams.toString();
	}
	TestAjax(`/api/RoutedApi/thing${queryString}`, null);
}", ScriptExtensions.KeyValueQueryParamHelper);
		}
	}
}
