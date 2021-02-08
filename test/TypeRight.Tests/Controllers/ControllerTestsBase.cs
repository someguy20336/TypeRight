using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TypeRight.Configuration;
using TypeRight.Tests.TestBuilders;
using TypeRight.Tests.Testers;

namespace TypeRight.Tests.Controllers
{
	public abstract class ControllerTestsBase : TypeRightTestBase
	{
		private List<ImportDefinition> _importDefinitions;
		private List<ActionParameter> _actionParams;

		private string _scriptReturnType;

		protected TestClassBuilder ControllerBuilder { get; private set; }

		protected string ControllerFullName => "TestController";
		protected string ControllerName => "Test";

		protected abstract bool IsAspNetCore { get; }

		[TestInitialize]
		public override void TestInitialize()
		{
			base.TestInitialize();

			_importDefinitions = new List<ImportDefinition>();
			_actionParams = new List<ActionParameter>();
			_scriptReturnType = "";

			WorkspaceBuilder.DefaultProject
				.AddFakeMvc();

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
			_importDefinitions.Add(definition);
		}

		protected void GivenActionParameters(IEnumerable<ActionParameter> actionParameters)
		{
			_actionParams = actionParameters.ToList();
		}

		protected void GivenScriptReturnType(string returnType) => _scriptReturnType = returnType;

		protected MvcActionTester AssertThatThisControllerAction(string actionName)
		{
			var packageTester = CreateTester();

			return packageTester.TestControllerWithName(ControllerFullName).TestActionWithName(actionName);		// TODO controller context...
		}

		protected void AssertControllerGeneratedText(string expectedText)
		{
			ControllerBuilder.Commit();

			var packageTester = WorkspaceBuilder.GetPackageTester();
			var actionConfig = packageTester.GetDefaultActionConfig();
			actionConfig[0].Imports.AddRange(_importDefinitions);
			actionConfig[0].Parameters = _actionParams;

			if (!string.IsNullOrEmpty(_scriptReturnType))
			{
				actionConfig[0].ReturnType = _scriptReturnType;
			}

			var context = packageTester.GetDefaultControllerContext(actionConfig);

			packageTester.AssertControllerScriptText(ControllerFullName, context, expectedText);
		}

		private TypeCollectionTester CreateTester()
		{
			ControllerBuilder.Commit();
			return WorkspaceBuilder.GetPackageTester();

		}
	}
}
