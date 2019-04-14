using TypeRight.Configuration;
using TypeRight.TypeLocation;
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
using TypeRightTests.TestBuilders;

namespace TypeRightTests.Testers
{
	class TypeCollectionTester
	{
		private ExtractedTypeCollection _typeCollection;
        
		private IScriptTemplate _scriptWriter;

		private readonly TypeFormatter _typeFormatter;

		public TypeCollectionTester(ExtractedTypeCollection typeCollection, TypeFilter dispNameFilter, TypeFilter mvcActionFilter)
		{
			_typeCollection = typeCollection;

			// TODO not hardcode?
			_scriptWriter = new NamespaceTemplate();
			_scriptWriter = new ModuleTemplate();
			

			// TODO any way to define this?  Or maybe an option when getting type name
			_typeFormatter = new TypeScriptTypeFormatter(_typeCollection, new NamespacedTypePrefixResolver());
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
			return new EnumTester(_typeCollection.GetEnumTypes().Where(en => en.Name == name).FirstOrDefault());
		}

		public ControllerTester TestControllerWithName(string name)
		{
			return new ControllerTester(_typeCollection.GetMvcControllers().Where(c => c.Name == name).FirstOrDefault(), _typeFormatter);
		}

		public TypeCollectionTester TestScriptText()
		{
			ScriptWriteContext context = new ScriptWriteContext()
			{
				IncludedTypes = _typeCollection,
				TypeCollection = _typeCollection,
				OutputPath = TestWorkspaceBuilder.DefaultResultPath
			};
			string scriptText = _scriptWriter.CreateTypeTemplate().GetText(context);
			Assert.IsFalse(string.IsNullOrEmpty(scriptText));
			return this;
		}

		public TypeCollectionTester TestScriptControllerText(string controllerName)
		{

            ControllerContext _context = new ControllerContext()
            {
                AjaxFunctionName = "TestAjax",
                WebMethodNamespace = "MethodNamespace",
                ExtractedTypes = _typeCollection,
                ServerObjectsResultFilepath = new Uri(@"C:\FolderA\FolderB\FolderC\FolderD\ServerObjects.ts"),
                AjaxFunctionModulePath = @"C:\FolderA\FolderB\FolderM\FolderN\AjaxFunc.ts",
				OutputPath = @"C:\FolderA\FolderB\FolderX\FolderY\SomeController.ts"
			};

            string scriptText = _scriptWriter.CreateControllerTextTemplate().GetText(
				_typeCollection.GetMvcControllers().Where(c => c.Name == controllerName).First(),
				_context
				);
			Assert.IsFalse(string.IsNullOrEmpty(scriptText));
			return this;
		}
	}
}
