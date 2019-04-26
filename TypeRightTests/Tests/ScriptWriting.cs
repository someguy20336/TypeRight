﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.Configuration;
using TypeRight.ScriptWriting;
using TypeRight.Workspaces.Parsing;
using TypeRightTests.HelperClasses;
using TypeRightTests.TestBuilders;
using TypeRightTests.Testers;

namespace TypeRightTests.Tests
{
	[TestClass]
	public class ScriptWriting
	{

		private static TypeCollectionTester _packageTester;

		/// <summary>
		/// Sets up a parse of this solution
		/// </summary>
		[ClassInitialize]
		public static void SetupParse(TestContext context)
		{

			TestWorkspaceBuilder wkspBuilder = new TestWorkspaceBuilder();

			wkspBuilder.DefaultProject
				// Dummy MvcActionAttribute
				.CreateClassBuilder("DummyAttribute")
					.AddBaseClass("Attribute")
					.Commit()

				// Test class to use as return/param
				.CreateClassBuilder("TestClass")
					.AddProperty("DontCare", "int")
					.Commit()

				// Test generic class to use as return/param
				.CreateClassBuilder("TestGenericClass")
					.AddGenericParameter("T")
					.AddProperty("GenericProp", "T")
					.Commit()

				// Fake JsonResult-like class
				.CreateClassBuilder("FakeJsonResultLikeClass")
					.AddProperty("DontCare", "int")
					.Commit()

				// Display name attribute
				.CreateClassBuilder("SimpleController")
					.AddMethod("FakeJson", "FakeJsonResultLikeClass")
						.AddParameter("data", "object")
						.AddLineOfCode("return null;", 0)
						.Commit()

					.AddMethod("StringResult", "string")
						.AddAttribute("DummyAttribute").Commit()
						.AddLineOfCode("return \"Hi\";", 0)
						.Commit()
					.AddMethod("SimpleParameter_Json", "FakeJsonResultLikeClass")
						.AddAttribute("DummyAttribute").Commit()
						.AddParameter("testParam", "TestClass")
						.AddLineOfCode("return FakeJson(testParam);", 0)
						.Commit()
					.AddMethod("GenericPropReturn_Json", "FakeJsonResultLikeClass")
						.AddAttribute("DummyAttribute").Commit()
						.AddLineOfCode("TestGenericClass<TestClass> gen = new TestGenericClass<TestClass>();", 0)
						.AddLineOfCode("return FakeJson(gen.GenericProp);", 0)
						.Commit()
					.Commit()

			;


			wkspBuilder.ClassParseFilter = new ExcludeWithAnyName("DummyAttribute", "SimpleController", "FakeJsonResultLikeClass");
			wkspBuilder.ControllerParseFilter = new AlwaysAcceptFilter();
			wkspBuilder.MvcActionFilter = new AlwaysAcceptFilter();

			MethodReturnTypeHandler handler = new ParseSyntaxForTypeMethodHandler(
				"Test.FakeJsonResultLikeClass",
				new InvocationReturnForwardFilter("FakeJson", 0)
				);
			ParseOptions parseOptions = new ParseOptions();
			parseOptions.MethodReturnTypeHandlers.Add(handler);
			wkspBuilder.ParseOptions = parseOptions;

			_packageTester = wkspBuilder.GetPackageTester();
		}

		[TestMethod]
		public void Controllers_LegacyOptions_Generated()
		{

			_packageTester.AssertScriptText("SimpleController",
			#region ScriptText	
				@"
// File Autogenerated by TypeRight.  DO NOT EDIT
import { TestAjax } from ""../../FolderM/FolderN/AjaxFunc"";
import * as DefaultResult from ""../../DefaultResult"";


/**
 * 
 */
export function GenericPropReturn_Json(success?: (result: DefaultResult.TestClass) => void, fail?: (result: any) => void): void {
	TestAjax(""/Simple/GenericPropReturn_Json"", {  }, success, fail);
}

/**
 * 
 * @param testParam 
 */
export function SimpleParameter_Json(testParam: DefaultResult.TestClass, success?: (result: DefaultResult.TestClass) => void, fail?: (result: any) => void): void {
	TestAjax(""/Simple/SimpleParameter_Json"", { testParam: testParam }, success, fail);
}

/**
 * 
 */
export function StringResult(success?: (result: string) => void, fail?: (result: any) => void): void {
	TestAjax(""/Simple/StringResult"", {  }, success, fail);
}



"
			#endregion
				);
		}

		[TestMethod]
		public void Controllers_AddlImportsIncluded()
		{
			ControllerContext context = _packageTester.GetDefaultControllerContext();
			context.AdditionalImports.Add(new ImportDefinition()
			{
				Items = new List<string>() { "Item1", "Item2" },
				Path = @"C:\FolderA\FolderB\ItemListImport.ts"
			});

			context.AdditionalImports.Add(new ImportDefinition()
			{
				Path = @"C:\FolderA\FolderB\AliasedImport.ts",
				UseAlias = true
			});

			_packageTester.AssertScriptText("SimpleController",
				context,
			#region ScriptText	
				@"
// File Autogenerated by TypeRight.  DO NOT EDIT
import { TestAjax } from ""../../FolderM/FolderN/AjaxFunc"";
import * as DefaultResult from ""../../DefaultResult"";
import { Item1, Item2 } from ""../../ItemListImport"";
import * as AliasedImport from ""../../AliasedImport"";


/**
 * 
 */
export function GenericPropReturn_Json(success?: (result: DefaultResult.TestClass) => void, fail?: (result: any) => void): void {
	TestAjax(""/Simple/GenericPropReturn_Json"", {  }, success, fail);
}

/**
 * 
 * @param testParam 
 */
export function SimpleParameter_Json(testParam: DefaultResult.TestClass, success?: (result: DefaultResult.TestClass) => void, fail?: (result: any) => void): void {
	TestAjax(""/Simple/SimpleParameter_Json"", { testParam: testParam }, success, fail);
}

/**
 * 
 */
export function StringResult(success?: (result: string) => void, fail?: (result: any) => void): void {
	TestAjax(""/Simple/StringResult"", {  }, success, fail);
}



"
			#endregion
				);
		}

		[TestMethod]
		public void Controllers_NonVoidReturnType()
		{
			ControllerContext context = _packageTester.GetDefaultControllerContext();
			context.AdditionalParameters = new List<ActionParameter>();	// remove addl params
			context.FetchReturnType = "Promise<$returnType$>";
			_packageTester.AssertScriptText("SimpleController",
				context,
			#region ScriptText	
				@"
// File Autogenerated by TypeRight.  DO NOT EDIT
import { TestAjax } from ""../../FolderM/FolderN/AjaxFunc"";
import * as DefaultResult from ""../../DefaultResult"";


/**
 * 
 */
export function GenericPropReturn_Json(): Promise<DefaultResult.TestClass> {
	return TestAjax(""/Simple/GenericPropReturn_Json"", {  });
}

/**
 * 
 * @param testParam 
 */
export function SimpleParameter_Json(testParam: DefaultResult.TestClass): Promise<DefaultResult.TestClass> {
	return TestAjax(""/Simple/SimpleParameter_Json"", { testParam: testParam });
}

/**
 * 
 */
export function StringResult(): Promise<string> {
	return TestAjax(""/Simple/StringResult"", {  });
}



"
			#endregion
				);
		}


		[TestMethod]
		public void Controllers_AdditionalParams()
		{
			ControllerContext context = _packageTester.GetDefaultControllerContext();
			context.AdditionalParameters = new List<ActionParameter>()
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

			context.FetchReturnType = "$returnType$";
			_packageTester.AssertScriptText("SimpleController",
				context,
			#region ScriptText	
				@"
// File Autogenerated by TypeRight.  DO NOT EDIT
import { TestAjax } from ""../../FolderM/FolderN/AjaxFunc"";
import * as DefaultResult from ""../../DefaultResult"";


/**
 * 
 */
export function GenericPropReturn_Json(randomString: string, abortSignal?: AbortSignal): DefaultResult.TestClass {
	return TestAjax(""/Simple/GenericPropReturn_Json"", {  }, randomString, abortSignal);
}

/**
 * 
 * @param testParam 
 */
export function SimpleParameter_Json(testParam: DefaultResult.TestClass, randomString: string, abortSignal?: AbortSignal): DefaultResult.TestClass {
	return TestAjax(""/Simple/SimpleParameter_Json"", { testParam: testParam }, randomString, abortSignal);
}

/**
 * 
 */
export function StringResult(randomString: string, abortSignal?: AbortSignal): string {
	return TestAjax(""/Simple/StringResult"", {  }, randomString, abortSignal);
}



"
			#endregion
				);
		}
	}
}
