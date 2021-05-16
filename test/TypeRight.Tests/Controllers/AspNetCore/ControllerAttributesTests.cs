using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TypeRight.Configuration;
using TypeRight.Tests.TestBuilders;

namespace TypeRight.Tests.Controllers.AspNetCore
{
	[TestClass]
	public class ControllerAttributesTests : ControllerTestsBase
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
		public void FromQuery_SingleSimple_KeyValueFuncAdded_Generated()
		{
			AddControllerAction("TestAction", MvcConstants.JsonResult_AspNetCore)
					.AddParameter("fromQuery", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
					.Commit()
			;

			AssertScriptTextForFunctionIs(@$"
export function TestAction(fromQuery: string): void {{
	let urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, ""fromQuery"", fromQuery);
	fetchWrapper(""GET"", `/{ControllerName}/TestAction${{getQueryString(urlParams)}}`, null);
}}", ScriptExtensions.KeyValueQueryParamHelper);
		}

		[TestMethod]
		public void FromQuery_SingleObject_BothQueryHelperFuncsAdded_Generated()
		{
			AddControllerAction("TestAction", MvcConstants.JsonResult_AspNetCore)
					.AddParameter("fromQuery", "TestClass", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
					.Commit()
			;

			AssertScriptTextForFunctionIs(@$"
export function TestAction(fromQuery: Partial<DefaultResult.TestClass>): void {{
	let urlParams = new URLSearchParams();
	tryAppendObjectValuesToUrl(urlParams, fromQuery);
	fetchWrapper(""GET"", `/{ControllerName}/TestAction${{getQueryString(urlParams)}}`, null);
}}", ScriptExtensions.KeyValueQueryParamHelper | ScriptExtensions.ObjectQueryParamHelper);
		}

		[TestMethod]
		public void FromQuery_ComplexAndSimpleParameters_BothQueryHelperFuncsAdded_ScriptGenerated()
		{
			AddControllerAction("TestAction", MvcConstants.JsonResult_AspNetCore)
					.AddParameter("simple", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
					.AddParameter("complex", "TestClass", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
					.Commit()
			;

			AssertScriptTextForFunctionIs(@$"
export function TestAction(simple: string, complex: Partial<DefaultResult.TestClass>): void {{
	let urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, ""simple"", simple);
	tryAppendObjectValuesToUrl(urlParams, complex);
	fetchWrapper(""GET"", `/{ControllerName}/TestAction${{getQueryString(urlParams)}}`, null);
}}", ScriptExtensions.KeyValueQueryParamHelper | ScriptExtensions.ObjectQueryParamHelper);
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

			AssertScriptTextForFunctionIs(@$"
export function TestingParamFilter(fromBody: string): void {{
	fetchWrapper(""GET"", `/{ControllerName}/TestingParamFilter`, fromBody);
}}");
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

			AssertScriptTextForFunctionIs(@$"
export function IsNotFirstParameter(fromBody: string): void {{
	fetchWrapper(""GET"", `/{ControllerName}/IsNotFirstParameter`, fromBody);
}}");
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

			AssertScriptTextForFunctionIs(@$"
export function NoFromBodyParams(): void {{
	fetchWrapper(""GET"", `/{ControllerName}/NoFromBodyParams`, null);
}}");
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

			AssertScriptTextForFunctionIs(@$"
export function QueryParameterWithBody(fromQuery: string, fromBody: DefaultResult.TestClass): void {{
	let urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, ""fromQuery"", fromQuery);
	fetchWrapper(""GET"", `/{ControllerName}/QueryParameterWithBody${{getQueryString(urlParams)}}`, fromBody);
}}", ScriptExtensions.KeyValueQueryParamHelper);
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

			AssertScriptTextForFunctionIs(@$"
export function Action(p: number): void {{
	let urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, ""p"", p);
	fetchWrapper(""GET"", `/{ControllerName}/Action${{getQueryString(urlParams)}}`, null);
}}", ScriptExtensions.KeyValueQueryParamHelper);
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

			AssertScriptTextForFunctionIs(@$"
export function MultipleTypes(multTypes: string | number): void {{
	let urlParams = new URLSearchParams();
	tryAppendKeyValueToUrl(urlParams, ""multTypes"", multTypes);
	fetchWrapper(""GET"", `/{ControllerName}/MultipleTypes${{getQueryString(urlParams)}}`, null);
}}", ScriptExtensions.KeyValueQueryParamHelper);
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

			AssertScriptTextForFunctionIs(@$"
export function Action(id: string | number): void {{
	fetchWrapper(""GET"", `/{ControllerName}/Action/${{id}}`, null);
}}");
		}

		[TestMethod]
		public void ScriptActionName_ActionScriptNameIsOverridden()
		{
			ControllerBuilder
				.AddMethod("Action", MvcConstants.JsonResult_AspNetCore)
					.AddScriptActionAttribute("DifferentName")
					.AddAttribute(MvcConstants.HttpGetAttributeFullName_AspNetCore)
						.AddConstructorArg("\"{id}\"")
						.Commit()
					.AddParameter("id", "string")
					.Commit()
			;

			AssertScriptTextForFunctionIs(@$"
export function DifferentName(id: string): void {{
	fetchWrapper(""GET"", `/{ControllerName}/Action/${{id}}`, null);
}}");
		}

		[TestMethod]
		public void ScriptActionName_WithConfigAsCamel_ActionScriptNameIsStillOverridden()
		{
			ConfigOptions.NameCasingConverter = ScriptWriting.NamingStrategyType.Camel;
			ScriptActionName_ActionScriptNameIsOverridden();
		}

	}
}
