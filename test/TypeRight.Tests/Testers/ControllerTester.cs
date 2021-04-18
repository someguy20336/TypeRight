using TypeRight.ScriptWriting;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.ScriptWriting.TypeScript;

namespace TypeRight.Tests.Testers
{
	public class ControllerTester
	{
		private readonly TypeFormatter _typeFormatter;
		private readonly ControllerContext _context;

		public ControllerTester(TypeFormatter typeFormatter, ControllerContext context)
		{
			_typeFormatter = typeFormatter;
			_context = context;
		}

		public MvcActionTester TestActionWithName(string name)
		{
			return new MvcActionTester(_context, _context.Actions.Where(m => m.Name == name).First(), _typeFormatter);
		}

		public ControllerTester HasImportForFile(string relPath)
		{
			var imports = ImportManager.FromControllerContext(_context).GetImports();
			Assert.IsTrue(imports.Any(imp => imp.FromRelativePath == relPath));
			return this;
		}
	}
}
