using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TypeRight.Configuration;

namespace TypeRight.Tests.Controllers.AspNet
{
	[TestClass]
	public class ControllerWriteOptionsTests : ControllerTestsBase
	{

		protected override bool IsAspNetCore => false;

		/// <summary>
		/// Sets up a parse of this solution
		/// </summary>
		[TestInitialize]
		public override void TestInitialize()
		{
			base.TestInitialize();

			ControllerBuilder

				.AddMethod("StringResult", "string")
					.AddScriptActionAttribute()
					.AddLineOfCode("return \"Hi\";", 0)
					.Commit()
				.AddMethod("SimpleParameter_Json", MvcConstants.JsonResult_AspNet)
					.AddScriptActionAttribute()
					.AddParameter("testParam", "TestClass")
					.AddLineOfCode("return Json(testParam);", 0)
					.Commit()
				.AddMethod("GenericPropReturn_Json", MvcConstants.JsonResult_AspNet)
					.AddScriptActionAttribute()
					.AddLineOfCode("TestGenericClass<TestClass> gen = new TestGenericClass<TestClass>();", 0)
					.AddLineOfCode("return Json(gen.GenericProp);", 0)
					.Commit()
				;
		}

		[TestMethod]
		public void DefaultOptions_Generated()
		{
			AssertControllerGeneratedText(
			#region ScriptText	
				@$"
import * as DefaultResult from ""../../DefaultResult"";
import {{ TestAjax }} from ""../../FolderM/FolderN/AjaxFunc"";


/**
 * 
 */
export function GenericPropReturn_Json(): void {{
	TestAjax(`/{ControllerName}/GenericPropReturn_Json`, {{}});
}}

/**
 * 
 * @param testParam 
 */
export function SimpleParameter_Json(testParam: DefaultResult.TestClass): void {{
	TestAjax(`/{ControllerName}/SimpleParameter_Json`, testParam);
}}

/**
 * 
 */
export function StringResult(): void {{
	TestAjax(`/{ControllerName}/StringResult`, {{}});
}}



"
			#endregion
				);
		}


		[TestMethod]
		public void AddlImportsIncluded()
		{
			GivenImportDefinition(new ImportDefinition()
			{
				Items = new List<string>() { "Item1", "Item2" },
				Path = @"C:\FolderA\FolderB\ItemListImport.ts"
			});

			GivenImportDefinition(new ImportDefinition()
			{
				Path = @"C:\FolderA\FolderB\AliasedImport.ts",
				UseAlias = true
			});

			AssertControllerGeneratedText(
			#region ScriptText	
				@$"
import * as AliasedImport from ""../../AliasedImport"";
import * as DefaultResult from ""../../DefaultResult"";
import {{ TestAjax }} from ""../../FolderM/FolderN/AjaxFunc"";
import {{ Item1, Item2 }} from ""../../ItemListImport"";


/**
 * 
 */
export function GenericPropReturn_Json(): void {{
	TestAjax(`/{ControllerName}/GenericPropReturn_Json`, {{}});
}}

/**
 * 
 * @param testParam 
 */
export function SimpleParameter_Json(testParam: DefaultResult.TestClass): void {{
	TestAjax(`/{ControllerName}/SimpleParameter_Json`, testParam);
}}

/**
 * 
 */
export function StringResult(): void {{
	TestAjax(`/{ControllerName}/StringResult`, {{}});
}}



"
			#endregion
				);
		}

		[TestMethod]
		public void PromiseWithReturnType()
		{
			GivenActionParameters(new List<ActionParameter>()); // remove addl params
			GivenScriptReturnType("Promise<$returnType$>");

			AssertControllerGeneratedText(
			#region ScriptText	
				@$"
import * as DefaultResult from ""../../DefaultResult"";
import {{ TestAjax }} from ""../../FolderM/FolderN/AjaxFunc"";


/**
 * 
 */
export function GenericPropReturn_Json(): Promise<DefaultResult.TestClass> {{
	return TestAjax(`/{ControllerName}/GenericPropReturn_Json`, {{}});
}}

/**
 * 
 * @param testParam 
 */
export function SimpleParameter_Json(testParam: DefaultResult.TestClass): Promise<DefaultResult.TestClass> {{
	return TestAjax(`/{ControllerName}/SimpleParameter_Json`, testParam);
}}

/**
 * 
 */
export function StringResult(): Promise<string> {{
	return TestAjax(`/{ControllerName}/StringResult`, {{}});
}}



"
			#endregion
				);
		}

		[TestMethod]
		public void CustomAdditionalParams()
		{
			GivenActionParameters(new List<ActionParameter>()
			{
				new ActionParameter("randomString", "string", false),
				new ActionParameter("abortSignal", "AbortSignal", true)
			});
			
			GivenScriptReturnType("$returnType$");

			AssertControllerGeneratedText(
			#region ScriptText	
				@$"
import * as DefaultResult from ""../../DefaultResult"";
import {{ TestAjax }} from ""../../FolderM/FolderN/AjaxFunc"";


/**
 * 
 */
export function GenericPropReturn_Json(randomString: string, abortSignal?: AbortSignal): DefaultResult.TestClass {{
	return TestAjax(`/{ControllerName}/GenericPropReturn_Json`, {{}}, randomString, abortSignal);
}}

/**
 * 
 * @param testParam 
 */
export function SimpleParameter_Json(testParam: DefaultResult.TestClass, randomString: string, abortSignal?: AbortSignal): DefaultResult.TestClass {{
	return TestAjax(`/{ControllerName}/SimpleParameter_Json`, testParam, randomString, abortSignal);
}}

/**
 * 
 */
export function StringResult(randomString: string, abortSignal?: AbortSignal): string {{
	return TestAjax(`/{ControllerName}/StringResult`, {{}}, randomString, abortSignal);
}}



"
			#endregion
				);
		}
	}
}
