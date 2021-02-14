using TypeRight.ScriptWriting;
using TypeRight.TypeProcessing;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.ScriptWriting.TypeScript;

namespace TypeRight.Tests.Testers
{
	public class ControllerTester
	{
		private MvcControllerInfo _controllerInfo;
		private ControllerModel _controllerModel;

		private readonly TypeFormatter _typeFormatter;

		public ControllerTester(MvcControllerInfo controller, TypeFormatter typeFormatter, ControllerContext context)
		{
			_controllerInfo = controller;
			_controllerModel = new ControllerProcessor(controller, context).CreateModel(typeFormatter);
			_typeFormatter = typeFormatter;
		}

		public MvcActionTester TestActionWithName(string name)
		{
			return new MvcActionTester(_controllerInfo, _controllerInfo.Actions.Where(m => m.Name == name).First(), _typeFormatter);
		}

		public MvcActionModelTester TestActionModelWithName(string name)
		{
			return new MvcActionModelTester(_controllerModel.Actions.Where(m => m.Name == name).First());
		}

		public ControllerTester HasImportForFile(string relPath)
		{
			Assert.IsTrue(_controllerModel.Imports.Any(imp => imp.FromRelativePath == relPath));
			return this;
		}
	}
}
