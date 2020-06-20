using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.Configuration;
using TypeRight.ScriptWriting;
using TypeRight.TypeProcessing;
using TypeRight.Workspaces.Parsing;
using TypeRight.Tests.TestBuilders;
using TypeRight.Tests.Testers;

namespace TypeRight.Tests.TestsWithParsing
{
	[TestClass]
	public class ScriptWriting
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

				// Test class to use as return/param
				.CreateClassBuilder("TestClass")
					.AddScriptObjectAttribute()
					.AddProperty("DontCare", "int")
					.Commit()

				// Test generic class to use as return/param
				.CreateClassBuilder("TestGenericClass")
					.AddScriptObjectAttribute()
					.AddGenericParameter("T")
					.AddProperty("GenericProp", "T")
					.Commit()
					
				// Display name attribute
				.CreateClassBuilder("SimpleController")
					.WithControllerBaseClass()
					.AddMethod("Json", "FakeJsonResultLikeClass")
						.AddParameter("data", "object")
						.AddLineOfCode("return null;", 0)
						.Commit()

					.AddMethod("StringResult", "string")
						.AddScriptActionAttribute()
						.AddLineOfCode("return \"Hi\";", 0)
						.Commit()
					.AddMethod("SimpleParameter_Json", MvcConstants.JsonResult_AspNetCore)
						.AddScriptActionAttribute()
						.AddParameter("testParam", "TestClass")
						.AddLineOfCode("return Json(testParam);", 0)
						.Commit()
					.AddMethod("GenericPropReturn_Json", MvcConstants.JsonResult_AspNetCore)
						.AddScriptActionAttribute()
						.AddLineOfCode("TestGenericClass<TestClass> gen = new TestGenericClass<TestClass>();", 0)
						.AddLineOfCode("return Json(gen.GenericProp);", 0)
						.Commit()
					.Commit()

				.CreateClassBuilder("TestParamAttributesController")
					.WithControllerBaseClass()
					.AddMethod("TestingParamFilter", MvcConstants.JsonResult_AspNetCore)
						.AddScriptActionAttribute()
						.AddParameter("fromBody", "string", attribute: MvcConstants.FromBodyAttributeFullName_AspNetCore)
						.AddParameter("fromServices", "TestClass", attribute: MvcConstants.FromServicesAttributeFullName_AspNetCore)
						.AddLineOfCode("return Json(0);", 0)
						.Commit()
					.AddMethod("IsNotFirstParameter", MvcConstants.JsonResult_AspNetCore)
						.AddScriptActionAttribute()
						.AddParameter("fromServices", "TestClass", attribute: MvcConstants.FromServicesAttributeFullName_AspNetCore)
						.AddParameter("fromBody", "string", attribute: MvcConstants.FromBodyAttributeFullName_AspNetCore)
						.AddLineOfCode("return Json(0);", 0)
						.Commit()
					.AddMethod("NoFromBodyParams", MvcConstants.JsonResult_AspNetCore)
						.AddScriptActionAttribute()
						.AddParameter("fromServices", "TestClass", attribute: MvcConstants.FromServicesAttributeFullName_AspNetCore)
						.AddParameter("fromServices2", "string", attribute: MvcConstants.FromServicesAttributeFullName_AspNetCore)
						.AddParameter("fromServices3", "string", attribute: MvcConstants.FromServicesAttributeFullName_AspNetCore)
						.AddLineOfCode("return Json(0);", 0)
						.Commit()
					.AddMethod("QueryParameterWithBody", MvcConstants.JsonResult_AspNetCore)
						.AddScriptActionAttribute()
						.AddParameter("fromQuery", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
						.AddParameter("fromBody", "TestClass", attribute: MvcConstants.FromBodyAttributeFullName_AspNetCore)
						.Commit()
					.Commit()

				.CreateClassBuilder("RoutedApiController")
					.WithControllerBaseClass()
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
					.Commit()
			;
			
			s_packageTester = wkspBuilder.GetPackageTester();
		}

		[TestMethod]
		public void Controllers_LegacyOptions_Generated()
		{

			s_packageTester.AssertControllerScriptText("SimpleController",
			#region ScriptText	
				@"
// File Autogenerated by TypeRight.  DO NOT EDIT
import * as DefaultResult from ""../../DefaultResult"";
import { TestAjax } from ""../../FolderM/FolderN/AjaxFunc"";


/**
 * 
 */
export function GenericPropReturn_Json(success?: (result: DefaultResult.TestClass) => void, fail?: (result: any) => void): void {
	TestAjax(`/Simple/GenericPropReturn_Json`, {}, success, fail);
}

/**
 * 
 * @param testParam 
 */
export function SimpleParameter_Json(testParam: DefaultResult.TestClass, success?: (result: DefaultResult.TestClass) => void, fail?: (result: any) => void): void {
	TestAjax(`/Simple/SimpleParameter_Json`, { testParam: testParam }, success, fail);
}

/**
 * 
 */
export function StringResult(success?: (result: string) => void, fail?: (result: any) => void): void {
	TestAjax(`/Simple/StringResult`, {}, success, fail);
}



"
			#endregion
				);
		}

		[TestMethod]
		public void Controllers_AddlImportsIncluded()
		{
			var actionConfig = s_packageTester.GetDefaultActionConfig();
			actionConfig[0].Imports.Add(new ImportDefinition()
			{
				Items = new List<string>() { "Item1", "Item2" },
				Path = @"C:\FolderA\FolderB\ItemListImport.ts"
			});

			actionConfig[0].Imports.Add(new ImportDefinition()
			{
				Path = @"C:\FolderA\FolderB\AliasedImport.ts",
				UseAlias = true
			});
			ControllerContext context = s_packageTester.GetDefaultControllerContext(actionConfig);		

			s_packageTester.AssertControllerScriptText("SimpleController",
				context,
			#region ScriptText	
				@"
// File Autogenerated by TypeRight.  DO NOT EDIT
import * as AliasedImport from ""../../AliasedImport"";
import * as DefaultResult from ""../../DefaultResult"";
import { TestAjax } from ""../../FolderM/FolderN/AjaxFunc"";
import { Item1, Item2 } from ""../../ItemListImport"";


/**
 * 
 */
export function GenericPropReturn_Json(success?: (result: DefaultResult.TestClass) => void, fail?: (result: any) => void): void {
	TestAjax(`/Simple/GenericPropReturn_Json`, {}, success, fail);
}

/**
 * 
 * @param testParam 
 */
export function SimpleParameter_Json(testParam: DefaultResult.TestClass, success?: (result: DefaultResult.TestClass) => void, fail?: (result: any) => void): void {
	TestAjax(`/Simple/SimpleParameter_Json`, { testParam: testParam }, success, fail);
}

/**
 * 
 */
export function StringResult(success?: (result: string) => void, fail?: (result: any) => void): void {
	TestAjax(`/Simple/StringResult`, {}, success, fail);
}



"
			#endregion
				);
		}

		[TestMethod]
		public void Controllers_NonVoidReturnType()
		{
			var actionConfig = s_packageTester.GetDefaultActionConfig();
			actionConfig[0].Parameters = new List<ActionParameter>(); // remove addl params
			actionConfig[0].ReturnType = "Promise<$returnType$>";

			ControllerContext context = s_packageTester.GetDefaultControllerContext(actionConfig);
			
			s_packageTester.AssertControllerScriptText("SimpleController",
				context,
			#region ScriptText	
				@"
// File Autogenerated by TypeRight.  DO NOT EDIT
import * as DefaultResult from ""../../DefaultResult"";
import { TestAjax } from ""../../FolderM/FolderN/AjaxFunc"";


/**
 * 
 */
export function GenericPropReturn_Json(): Promise<DefaultResult.TestClass> {
	return TestAjax(`/Simple/GenericPropReturn_Json`, {});
}

/**
 * 
 * @param testParam 
 */
export function SimpleParameter_Json(testParam: DefaultResult.TestClass): Promise<DefaultResult.TestClass> {
	return TestAjax(`/Simple/SimpleParameter_Json`, { testParam: testParam });
}

/**
 * 
 */
export function StringResult(): Promise<string> {
	return TestAjax(`/Simple/StringResult`, {});
}



"
			#endregion
				);
		}


		[TestMethod]
		public void Controllers_AdditionalParams()
		{
			var actionConfig = s_packageTester.GetDefaultActionConfig();
			actionConfig[0].Parameters = new List<ActionParameter>()
			{

				new ActionParameter()
				{
					Name = "randomString",
					Type = "string"
				},
				new ActionParameter()
				{
					Name = "abortSignal",
					Optional = true,
					Type = "AbortSignal"
				}
			};
			actionConfig[0].ReturnType = "$returnType$";
			ControllerContext context = s_packageTester.GetDefaultControllerContext(actionConfig);			

			s_packageTester.AssertControllerScriptText("SimpleController",
				context,
			#region ScriptText	
				@"
// File Autogenerated by TypeRight.  DO NOT EDIT
import * as DefaultResult from ""../../DefaultResult"";
import { TestAjax } from ""../../FolderM/FolderN/AjaxFunc"";


/**
 * 
 */
export function GenericPropReturn_Json(randomString: string, abortSignal?: AbortSignal): DefaultResult.TestClass {
	return TestAjax(`/Simple/GenericPropReturn_Json`, {}, randomString, abortSignal);
}

/**
 * 
 * @param testParam 
 */
export function SimpleParameter_Json(testParam: DefaultResult.TestClass, randomString: string, abortSignal?: AbortSignal): DefaultResult.TestClass {
	return TestAjax(`/Simple/SimpleParameter_Json`, { testParam: testParam }, randomString, abortSignal);
}

/**
 * 
 */
export function StringResult(randomString: string, abortSignal?: AbortSignal): string {
	return TestAjax(`/Simple/StringResult`, {}, randomString, abortSignal);
}



"
			#endregion
				);
		}

		[TestMethod]
		public void Controllers_ExcludingNonFromBodyParameters()
		{
			var actionConfig = s_packageTester.GetDefaultActionConfig();
			actionConfig[0].Parameters = new List<ActionParameter>();   // clear out for simplicity
			ControllerContext context = s_packageTester.GetDefaultControllerContext(actionConfig);
			context.ModelBinding = ModelBindingType.SingleParam;
			s_packageTester.AssertControllerScriptText("TestParamAttributesController",
				context,
			#region ScriptText	
				@"
// File Autogenerated by TypeRight.  DO NOT EDIT
import * as DefaultResult from ""../../DefaultResult"";
import { TestAjax } from ""../../FolderM/FolderN/AjaxFunc"";


/**
 * 
 * @param fromBody 
 */
export function IsNotFirstParameter(fromBody: string): void {
	TestAjax(`/TestParamAttributes/IsNotFirstParameter`, fromBody);
}

/**
 * 
 */
export function NoFromBodyParams(): void {
	TestAjax(`/TestParamAttributes/NoFromBodyParams`, {});
}

/**
 * 
 * @param fromQuery 
 * @param fromBody 
 */
export function QueryParameterWithBody(fromQuery: string, fromBody: DefaultResult.TestClass): void {
	TestAjax(`/TestParamAttributes/QueryParameterWithBody?fromQuery=${fromQuery}`, fromBody);
}

/**
 * 
 * @param fromBody 
 */
export function TestingParamFilter(fromBody: string): void {
	TestAjax(`/TestParamAttributes/TestingParamFilter`, fromBody);
}


"
			#endregion
				);
		}

		[TestMethod]
		public void Controllers_RoutedApiController()
		{
			var actionConfig = s_packageTester.GetDefaultActionConfig();
			actionConfig[0].Parameters = new List<ActionParameter>();   // clear out for simplicity
			ControllerContext context = s_packageTester.GetDefaultControllerContext(actionConfig);
			context.ModelBinding = ModelBindingType.SingleParam;
			s_packageTester.AssertControllerScriptText("RoutedApiController",
				context,
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
	TestAjax(`/api/RoutedApi/thing/${thingId}/put?query=${query}`, body);
}

"
			#endregion
				);
		}

		[TestMethod]
		public void Scripts_ExtendsBaseType_WithSameName_DiffTypeParams()
		{
			TestWorkspaceBuilder builder = new TestWorkspaceBuilder();

			builder.DefaultProject
				.AddFakeTypeRight()
				.CreateClassBuilder("MyType")
					.AddScriptObjectAttribute()
					.AddProperty("BaseProperty", "string")
					.Commit()
				.CreateClassBuilder("MyType")
					.AddScriptObjectAttribute()
					.AddGenericParameter("T")
					.AddBaseClass("MyType")
					.AddProperty("GenericProp", "T")
					.Commit()
				;

			TypeCollectionTester tcTester = builder.GetPackageTester();
			tcTester.AssertScriptText(@"
// File Autogenerated by TypeRight.  DO NOT EDIT

// ===============================
// Classes
// ===============================
/**  */
export interface MyType {
	/**  */
	BaseProperty: string;
}

/**  */
export interface MyType_1<T> extends MyType {
	/**  */
	GenericProp: T;
}

  

// ===============================
// Enums
// ===============================

");

		}
		
	}
}
