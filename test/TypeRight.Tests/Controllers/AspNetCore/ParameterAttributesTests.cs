﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TypeRight.Configuration;
using TypeRight.Tests.TestBuilders;

namespace TypeRight.Tests.Controllers.AspNetCore
{
	[TestClass]
	public class ParameterAttributesTests : ControllerTestsBase
	{

		protected override bool IsAspNetCore => true;

		/// <summary>
		/// Sets up a parse of this solution
		/// </summary>
		[TestInitialize]
		public void SetupParse()
		{
			TestInitialize();

			GivenActionParameters(new List<ActionParameter>());  // Cleared for simplicity... should this just be the default?

		}


		[TestMethod]
		public void FromBody_WithFromServices_FirstParameter_ScriptIsCorrect()
		{
			ControllerBuilder
				.AddMethod("TestingParamFilter", MvcConstants.JsonResult_AspNetCore)
					.AddScriptActionAttribute()
					.AddParameter("fromBody", "string", attribute: MvcConstants.FromBodyAttributeFullName_AspNetCore)
					.AddParameter("fromServices", "TestClass", attribute: MvcConstants.FromServicesAttributeFullName_AspNetCore)
					.AddLineOfCode("return Json(0);", 0)
					.Commit()
				;

			AssertControllerGeneratedText(
			#region ScriptText	
				@$"
import * as DefaultResult from ""../../DefaultResult"";
import {{ TestAjax }} from ""../../FolderM/FolderN/AjaxFunc"";


/**
 * 
 * @param fromBody 
 */
export function TestingParamFilter(fromBody: string): void {{
	TestAjax(`/{ControllerName}/TestingParamFilter`, fromBody);
}}


"
			#endregion
				);
		}

		[TestMethod]
		public void FromBody_WithFromServices_FromBodySecondParam_IsCorrect()
		{
			ControllerBuilder
				.AddMethod("IsNotFirstParameter", MvcConstants.JsonResult_AspNetCore)
					.AddScriptActionAttribute()
					.AddParameter("fromServices", "TestClass", attribute: MvcConstants.FromServicesAttributeFullName_AspNetCore)
					.AddParameter("fromBody", "string", attribute: MvcConstants.FromBodyAttributeFullName_AspNetCore)
					.AddLineOfCode("return Json(0);", 0)
					.Commit()
				;

			AssertControllerGeneratedText(
			#region ScriptText	
				@$"
import * as DefaultResult from ""../../DefaultResult"";
import {{ TestAjax }} from ""../../FolderM/FolderN/AjaxFunc"";


/**
 * 
 * @param fromBody 
 */
export function IsNotFirstParameter(fromBody: string): void {{
	TestAjax(`/{ControllerName}/IsNotFirstParameter`, fromBody);
}}


"
			#endregion
				);
		}


		[TestMethod]
		public void FromServices_AllParameters_GeneratedWithNoParameters()
		{
			ControllerBuilder
				.AddMethod("NoFromBodyParams", MvcConstants.JsonResult_AspNetCore)
					.AddScriptActionAttribute()
					.AddParameter("fromServices", "TestClass", attribute: MvcConstants.FromServicesAttributeFullName_AspNetCore)
					.AddParameter("fromServices2", "string", attribute: MvcConstants.FromServicesAttributeFullName_AspNetCore)
					.AddParameter("fromServices3", "string", attribute: MvcConstants.FromServicesAttributeFullName_AspNetCore)
					.AddLineOfCode("return Json(0);", 0)
					.Commit()
				;

			AssertControllerGeneratedText(
			#region ScriptText	
				@$"
import * as DefaultResult from ""../../DefaultResult"";
import {{ TestAjax }} from ""../../FolderM/FolderN/AjaxFunc"";


/**
 * 
 */
export function NoFromBodyParams(): void {{
	TestAjax(`/{ControllerName}/NoFromBodyParams`, null);
}}



"
			#endregion
				);
		}


		[TestMethod]
		public void FromBody_WithFromQuery_ScriptIsCorrect()
		{
			ControllerBuilder
				.AddMethod("QueryParameterWithBody", MvcConstants.JsonResult_AspNetCore)
					.AddScriptActionAttribute()
					.AddParameter("fromQuery", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
					.AddParameter("fromBody", "TestClass", attribute: MvcConstants.FromBodyAttributeFullName_AspNetCore)
					.Commit()
				;

			AssertControllerGeneratedText(
			#region ScriptText	
				@$"
import * as DefaultResult from ""../../DefaultResult"";
import {{ TestAjax }} from ""../../FolderM/FolderN/AjaxFunc"";


/**
 * 
 * @param fromQuery 
 * @param fromBody 
 */
export function QueryParameterWithBody(fromQuery: string, fromBody: DefaultResult.TestClass): void {{
	TestAjax(`/{ControllerName}/QueryParameterWithBody?fromQuery=${{ fromQuery ?? """" }}`, fromBody);
}}


"
			#endregion
				);
		}

		[TestMethod]
		public void ParamTypes_SingleOverrideTypeSpecified_GeneratesWithType()
		{
			var attrs = new List<AttributeInfo>()
			{
				new AttributeInfo()
				{
					AttributeTypeName = KnownTypes.ScriptParamTypesAttributeName,
					CtorArguments = new List<string>()
					{
						"typeof(int)"
					}
				},
				new AttributeInfo()
				{
					AttributeTypeName = MvcConstants.FromQueryAttributeFullName_AspNetCore,
				},
			};

			ControllerBuilder
				.AddMethod("Action", MvcConstants.JsonResult_AspNetCore)
					.AddScriptActionAttribute()
					.AddParameter("p", "string", "", attrs)
					.Commit()
			;

			AssertControllerGeneratedText(
			#region ScriptText	
				@$"
import {{ TestAjax }} from ""../../FolderM/FolderN/AjaxFunc"";


/**
 * 
 * @param p 
 */
export function Action(p: number): void {{
	TestAjax(`/{ControllerName}/Action?p=${{ p ?? """" }}`, null);
}}


"
			#endregion
				);
		}

		[TestMethod]
		public void ParamTypes_MultipleTypesSpecified_GeneratesWithTypes()
		{
			var attrs = new List<AttributeInfo>()
			{
				new AttributeInfo()
				{
					AttributeTypeName = KnownTypes.ScriptParamTypesAttributeName,
					CtorArguments = new List<string>()
					{
						"typeof(string)",
						"typeof(int)"
					}
				},
				new AttributeInfo()
				{
					AttributeTypeName = MvcConstants.FromQueryAttributeFullName_AspNetCore,
				},
			};

			ControllerBuilder
				.AddMethod("MultipleTypes", MvcConstants.JsonResult_AspNetCore)
					.AddScriptActionAttribute()
					.AddParameter("multTypes", "string", "", attrs)
					.Commit()
			;

			AssertControllerGeneratedText(
			#region ScriptText	
				@$"
import {{ TestAjax }} from ""../../FolderM/FolderN/AjaxFunc"";


/**
 * 
 * @param multTypes 
 */
export function MultipleTypes(multTypes: string | number): void {{
	TestAjax(`/{ControllerName}/MultipleTypes?multTypes=${{ multTypes ?? """" }}`, null);
}}


"
			#endregion
				);
		}

		[TestMethod]
		public void ParamTypes_FromRoute_GeneratesWithTypes()
		{
			var attrs = new List<AttributeInfo>()
			{
				new AttributeInfo()
				{
					AttributeTypeName = KnownTypes.ScriptParamTypesAttributeName,
					CtorArguments = new List<string>()
					{
						"typeof(string)",
						"typeof(int)"
					}
				}
			};

			ControllerBuilder
				.AddMethod("Action", MvcConstants.JsonResult_AspNetCore)
					.AddScriptActionAttribute()
					.AddAttribute(MvcConstants.HttpGetAttributeFullName_AspNetCore)
						.AddConstructorArg("\"{id}\"")
						.Commit()
					.AddParameter("id", "string", "", attrs)
					.Commit()
			;

			AssertControllerGeneratedText(
			#region ScriptText	
				@$"
import {{ TestAjax }} from ""../../FolderM/FolderN/AjaxFunc"";


/**
 * 
 * @param id 
 */
export function Action(id: string | number): void {{
	TestAjax(`/{ControllerName}/Action/${{id}}`, null);
}}


"
			#endregion
				);
		}
	}
}
