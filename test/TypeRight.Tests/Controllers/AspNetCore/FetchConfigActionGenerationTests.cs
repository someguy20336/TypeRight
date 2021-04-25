using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TypeRight.Configuration;
using TypeRight.Tests.TestBuilders;
using TypeRight.Tests.Testers;

namespace TypeRight.Tests.Controllers.AspNetCore
{
	[TestClass]
	public class FetchConfigActionGenerationTests : ControllerTestsBase
	{
		protected override bool IsAspNetCore => true;

		[TestInitialize]
		public override void TestInitialize()
		{
			base.TestInitialize();

			ControllerBuilder
				.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore)
					.AddConstructorArg("\"api/RoutedApi\"")
					.Commit();

			GivenFetchConfig(new FetchConfig()
			{
				FilePath = TypeCollectionTester.FetchFilePath,
				Imports = new List<ImportDefinition>(),
				Name = "fetchWrapper",
				ReturnType = "void",
				Parameters = new List<ActionParameter>()
				{
					ActionParameter.RequestMethod,
					ActionParameter.Url,
					ActionParameter.Body
				}
			});
		}

		[TestMethod]
		public void StandardParams_BasicGetRequest_ScriptIsWritten()
		{
			ControllerBuilder
				.AddMethod("GetThing", "string")
					.AddScriptActionAttribute()
					.AddAttribute(MvcConstants.HttpGetAttributeFullName_AspNetCore)
						.AddConstructorArg("\"thing/{thingId}\"").Commit()
					.AddParameter("thingId", "string")
					.AddLineOfCode("return null", 0)
					.Commit()
					;

			AssertScriptTextForFunctionIs(
				@"
export function GetThing(thingId: string): void {
	fetchWrapper(""GET"", `/api/RoutedApi/thing/${thingId}`, null);
}");
		}

		[TestMethod]
		public void StandardParams_GetRequestWithQuery_ScriptIsWritten()
		{
			ControllerBuilder
				.AddMethod("GetThing", "string")
					.AddScriptActionAttribute()
					.AddAttribute(MvcConstants.HttpGetAttributeFullName_AspNetCore)
						.AddConstructorArg("\"thing/{thingId}\"").Commit()
					.AddParameter("thingId", "string")
					.AddParameter("queryP", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
					.AddLineOfCode("return null", 0)
					.Commit()
					;

			AssertScriptTextForFunctionIs(@"
export function GetThing(thingId: string, queryP: string): void {
	let urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, ""queryP"", queryP);
	fetchWrapper(""GET"", `/api/RoutedApi/thing/${thingId}${getQueryString(urlParams)}`, null);
}", ScriptExtensions.KeyValueQueryParamHelper);
		}

		[TestMethod]
		public void CustomParameter_GetRequest_ScriptIsWritten()
		{
			AddFetchConfigParameter(new ActionParameter("test", "string", false));

			ControllerBuilder
				.AddMethod("GetThing", "string")
					.AddScriptActionAttribute()
					.AddAttribute(MvcConstants.HttpGetAttributeFullName_AspNetCore)
						.AddConstructorArg("\"thing/{thingId}\"").Commit()
					.AddParameter("thingId", "string")
					.AddLineOfCode("return null", 0)
					.Commit()
					;

			AssertScriptTextForFunctionIs(@"
export function GetThing(thingId: string, test: string): void {
	fetchWrapper(""GET"", `/api/RoutedApi/thing/${thingId}`, null, test);
}");
		}

		[TestMethod]
		public void StandardParams_PostRequest_ScriptIsWritten()
		{
			ControllerBuilder
				.AddMethod("GetThing", "string")
					.AddScriptActionAttribute()
					.AddAttribute(MvcConstants.HttpPostAttributeFullName_AspNetCore)
						.AddConstructorArg("\"thing/{thingId}\"").Commit()
					.AddParameter("thingId", "string")
					.AddParameter("body", "string", attribute: MvcConstants.FromBodyAttributeFullName_AspNetCore)
					.AddLineOfCode("return null", 0)
					.Commit()
					;

			AssertScriptTextForFunctionIs(@"
export function GetThing(thingId: string, body: string): void {
	fetchWrapper(""POST"", `/api/RoutedApi/thing/${thingId}`, body);
}");
		}

		[TestMethod]
		public void StandardParams_ActionHasNoMethod_DefaultsGET()
		{
			ControllerBuilder
				.AddMethod("GetThing", "string")
					.AddScriptActionAttribute()
					.AddLineOfCode("return null", 0)
					.Commit()
					;

			AssertScriptTextForFunctionIs(@"
export function GetThing(): void {
	fetchWrapper(""GET"", `/api/RoutedApi`, null);
}");
		}
	}
}
