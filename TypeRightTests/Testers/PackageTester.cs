using TypeRight.Configuration;
using TypeRight.Packages;
using TypeRight.ScriptWriting;
using TypeRight.ScriptWriting.TypeScript;
using TypeRight.TypeFilters;
using TypeRight.TypeProcessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRightTests.Testers
{
	class PackageTester
	{
		private ExtractedTypeCollection _typeCollection;
        
		private IScriptTemplate _scriptWriter;

		private TypeFormatter _typeFormatter;

		public PackageTester(ScriptPackage package, TypeFilter dispNameFilter, TypeFilter mvcActionFilter)
		{
			_typeCollection = new ExtractedTypeCollection(package, new ProcessorSettings()
			{
				TypeNamespace = ReferenceTypeTester.TestNamespace,
				EnumNamespace = EnumTester.TestNamespace,
				DisplayNameFilter = dispNameFilter,
				MvcActionFilter = mvcActionFilter
			});

			// TODO not hardcode?
			_scriptWriter = new NamespaceTemplate();
			_scriptWriter = new ModuleTemplate();
			

			// TODO any way to define this?  Or maybe an option when getting type name
			_typeFormatter = new TypeScriptTypeFormatter(_typeCollection);
		}

		public ReferenceTypeTester TestReferenceTypeWithName(string name, int? typeArgCnt = null)
		{
			ExtractedReferenceType extrType = _typeCollection.GetReferenceTypes()
					.Where(type => type.NamedType.Name == name && (!typeArgCnt.HasValue || typeArgCnt.Value == type.NamedType.TypeArguments.Count))
					.FirstOrDefault() as ExtractedReferenceType;
			return new ReferenceTypeTester(extrType, _typeFormatter);
		}

		public EnumTester TestEnumsWithName(string name)
		{
			return new EnumTester(_typeCollection.GetEnums().Where(en => en.Name == name).FirstOrDefault());
		}

		public ControllerTester TestControllerWithName(string name)
		{
			return new ControllerTester(_typeCollection.GetMvcControllers().Where(c => c.Name == name).FirstOrDefault(), _typeFormatter);
		}

		public PackageTester TestScriptText()
		{
			string scriptText = _scriptWriter.CreateTypeTemplate().GetText(_typeCollection);
			Assert.IsFalse(string.IsNullOrEmpty(scriptText));
			return this;
		}

		public PackageTester TestScriptControllerText(string controllerName)
		{
			Uri fakeOutputPath = new Uri(@"C:\FolderA\FolderB\FolderX\FolderY\SomeController.ts");

            ControllerContext _context = new ControllerContext()
            {
                AjaxFunctionName = "TestAjax",
                WebMethodNamespace = "MethodNamespace",
                ExtractedTypes = _typeCollection,
                ServerObjectsResultFilepath = new Uri(@"C:\FolderA\FolderB\FolderC\FolderD\ServerObjects.ts"),
                AjaxFunctionModulePath = new Uri(@"C:\FolderA\FolderB\FolderM\FolderN\AjaxFunc.ts")
            };

            string scriptText = _scriptWriter.CreateControllerTextTemplate().GetText(
				_typeCollection.GetMvcControllers().Where(c => c.Name == controllerName).First(),
				_context,
				fakeOutputPath
				);
			Assert.IsFalse(string.IsNullOrEmpty(scriptText));
			return this;
		}
	}
}
