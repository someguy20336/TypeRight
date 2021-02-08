﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
				.UrlTemplateIs($"/api/{ControllerName}/getList");

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
// File Autogenerated by TypeRight.  DO NOT EDIT
import { TestAjax, callDelete } from ""../../FolderM/FolderN/AjaxFunc"";


/**
 * 
 * @param thingId 
 */
export function DeleteThing(thingId: string): void {
	callDelete(`/api/RoutedApi/thing/${thingId}`);
}

/**
 * 
 * @param thingId 
 */
export function GetThing(thingId: string): void {
	TestAjax(`/api/RoutedApi/thing/${thingId}`);
}

/**
 * 
 * @param thingId 
 * @param query 
 * @param body 
 */
export function PutThingWithQuery(thingId: string, query: string, body: boolean): void {
	TestAjax(`/api/RoutedApi/thing/${thingId}/put?query=${ query ?? """" }`, body);
}

"
			#endregion
				);
		}
	}
}
