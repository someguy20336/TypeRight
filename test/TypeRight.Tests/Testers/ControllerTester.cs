using TypeRight.ScriptWriting;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.ScriptWriting.TypeScript;

namespace TypeRight.Tests.Testers
{
	public class ControllerTester
	{
		private readonly ControllerModel _controllerModel;

		private readonly TypeFormatter _typeFormatter;
		private readonly ControllerContext _context;

		public ControllerTester(TypeFormatter typeFormatter, ControllerContext context)
		{
			_controllerModel = new ControllerProcessor(context).CreateModel(typeFormatter);
			_typeFormatter = typeFormatter;
			_context = context;
		}

		public MvcActionTester TestActionWithName(string name)
		{
			return new MvcActionTester(_context, _context.Controller.Actions.Where(m => m.Name == name).First(), _typeFormatter);
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
