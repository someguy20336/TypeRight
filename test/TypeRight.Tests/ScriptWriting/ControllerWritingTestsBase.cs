using System.Collections.Generic;
using System.Linq;
using TypeRight.Configuration;
using TypeRight.Tests.TestBuilders;

namespace TypeRight.Tests.ScriptWriting
{
	public abstract class ControllerWritingTestsBase : ScriptWritingTestBase
	{
		private List<ImportDefinition> _importDefinitions;
		private List<ActionParameter> _actionParams;

		private string _scriptReturnType;

		protected abstract string ControllerName { get; }

		protected override void InitializeDefaultBuilder()
		{
			base.InitializeDefaultBuilder();

			_importDefinitions = new List<ImportDefinition>();
			_actionParams = new List<ActionParameter>()
			{
				new ActionParameter() {Name = "success", Type = "(result: $returnType$) => void", Optional = true},
				new ActionParameter() {Name = "fail", Type = "(result: any) => void", Optional = true }
			};
			_scriptReturnType = "";

			WorkspaceBuilder.DefaultProject
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
					.Commit();

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

		protected void AssertControllerGeneratedText(string expectedText)
		{

			var packageTester = WorkspaceBuilder.GetPackageTester();
			var actionConfig = packageTester.GetDefaultActionConfig();
			actionConfig[0].Imports.AddRange(_importDefinitions);
			actionConfig[0].Parameters = _actionParams;

			if (!string.IsNullOrEmpty(_scriptReturnType))
			{
				actionConfig[0].ReturnType = _scriptReturnType;
			}

			var context = packageTester.GetDefaultControllerContext(actionConfig);

			packageTester.AssertControllerScriptText(ControllerName, context, expectedText);
		}
	}
}
