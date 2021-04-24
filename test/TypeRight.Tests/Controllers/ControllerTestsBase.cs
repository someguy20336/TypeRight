﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using TypeRight.Configuration;
using TypeRight.ScriptWriting;
using TypeRight.Tests.TestBuilders;
using TypeRight.Tests.Testers;

namespace TypeRight.Tests.Controllers
{
	public abstract class ControllerTestsBase : TypeRightTestBase
	{
		protected ConfigOptions ConfigOptions { get; private set; }

		protected TestClassBuilder ControllerBuilder { get; private set; }

		protected string ControllerFullName => $"{ControllerName}Controller";
		protected virtual string ControllerName => "Test";

		protected abstract bool IsAspNetCore { get; }

		[TestInitialize]
		public override void TestInitialize()
		{
			base.TestInitialize();

			ConfigOptions = new ConfigOptions()
			{
				ActionConfigurations = TypeCollectionTester.GetDefaultActionConfig(),
				QueryParams = new NameValueCollection()
			};

			GivenActionParameters(Array.Empty<ActionParameter>());

			WorkspaceBuilder.DefaultProject
				.AddFakeMvc(IsAspNetCore);

			// Test class to use as return/param
			AddClass("TestClass")
				.AddScriptObjectAttribute()
				.AddProperty("DontCare", "int")
				.Commit();

			// Test generic class to use as return/param
			AddClass("TestGenericClass")
				.AddScriptObjectAttribute()
				.AddGenericParameter("T")
				.AddProperty("GenericProp", "T")
				.Commit();

			ControllerBuilder = AddClass(ControllerFullName)
				.WithControllerBaseClass(IsAspNetCore)

				// Fake Json method
				.AddMethod("Json", IsAspNetCore ? MvcConstants.JsonResult_AspNetCore : MvcConstants.JsonResult_AspNet)
					.AddParameter("data", "object")
					.AddLineOfCode("return null;", 0)
					.Commit();
		}

		protected TestMethodBuilder AddControllerAction(string name, string returnType)
		{
			return ControllerBuilder.AddMethod(name, returnType).AddScriptActionAttribute();
		}

		protected void GivenImportDefinition(ImportDefinition definition)
		{
			ConfigOptions.ActionConfigurations[0].Imports.Add(definition);
		}

		protected void GivenActionParameters(IEnumerable<ActionParameter> actionParameters)
		{
			ConfigOptions.ActionConfigurations[0].Parameters = actionParameters.ToList();
		}

		protected void GivenActionConfig(ActionConfig config)
		{
			ConfigOptions.ActionConfigurations.Add(config);
		}

		protected void GivenQueryParameter(string key, string value)
		{
			ConfigOptions.QueryParams.Add(key, value);
		}

		protected void GivenBaseUrl(string url) => ConfigOptions.BaseUrl = url;

		protected void GivenFetchConfig(FetchConfig fetchConfig)
		{
			ConfigOptions.ActionConfigurations = null;
			ConfigOptions.FetchConfig = fetchConfig;
		}

		protected void AddFetchConfigParameter(ActionParameter actionParam)
		{
			ConfigOptions.FetchConfig.Parameters.Add(actionParam);
		}

		protected void GivenScriptReturnType(string returnType) => ConfigOptions.ActionConfigurations[0].ReturnType = returnType;

		protected MvcActionTester AssertThatThisControllerAction(string actionName)
		{
			return AssertThatThisController().TestActionWithName(actionName);
		}

		protected ControllerTester AssertThatThisController()
		{
			var packageTester = CreateTester();
			return packageTester.TestControllerWithName(CreateContext(packageTester));
		}

		protected void AssertControllerGeneratedText(string expectedText)
		{
			string actualText = GetScriptText().Trim();
			expectedText = "// File Autogenerated by TypeRight.  DO NOT EDIT\r\n" + expectedText.Trim();
			expectedText = expectedText.Trim();

			Assert.AreEqual(expectedText, actualText);
		}

		/// <summary>
		/// asserts the script text without all the stuff i don't care about (comments, imports, etc)
		/// </summary>
		/// <param name="expectedText"></param>
		protected void AssertScriptTextForFunctionIs(string expectedText)
		{
			string scriptText = GetScriptText();

			var scriptLines = scriptText.Split(Environment.NewLine);
			scriptLines = scriptLines.Skip(1)
				.Where(ln => !ln.StartsWith("import"))
				.Where(ln => !ln.Trim().StartsWith("/*"))
				.Where(ln => !ln.Trim().StartsWith("*"))
				.Where(ln => !ln.Trim().StartsWith("*/"))
				.ToArray();

			scriptText = string.Join(Environment.NewLine, scriptLines).Trim();
			expectedText = expectedText.Trim();
			Assert.AreEqual(expectedText, scriptText);
		}

		protected string GetScriptText()
		{
			ControllerBuilder.Commit();

			var packageTester = WorkspaceBuilder.GetPackageTester();
			var context = CreateContext(packageTester);

			string scriptText = ScriptTemplateFactory.CreateControllerTextTemplate().GetText(context);
			return scriptText;
		}

		private ControllerContext CreateContext(TypeCollectionTester tester)
		{
			return tester.GetDefaultControllerContext(ControllerFullName, ConfigOptions);
		}

		private TypeCollectionTester CreateTester()
		{
			ControllerBuilder.Commit();
			return WorkspaceBuilder.GetPackageTester();

		}
	}
}
