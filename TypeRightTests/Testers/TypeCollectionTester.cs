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
using TypeRightTests.HelperClasses;

namespace TypeRightTests.Testers
{
	class TypeCollectionTester
	{
		private ExtractedTypeCollection _typeCollection;

		private IScriptTemplate _scriptWriter;

		private readonly TypeFormatter _typeFormatter;

		public TypeCollectionTester(ExtractedTypeCollection typeCollection, TypeFilter dispNameFilter)
		{
			_typeCollection = typeCollection;

			// TODO not hardcode?
			_scriptWriter = new NamespaceTemplate();
			_scriptWriter = new ModuleTemplate();


			// TODO any way to define this?  Or maybe an option when getting type name
			_typeFormatter = new TypeScriptTypeFormatter(_typeCollection, new NamespacedTypePrefixResolver(EnumTester.TestNamespace, ReferenceTypeTester.TestNamespace));
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

		public ControllerTester TestControllerWithName(string name, ControllerContext context = null)
		{
			context = context ?? GetDefaultControllerContext();
			return new ControllerTester(_typeCollection.GetMvcControllers().Where(c => c.Name == name).FirstOrDefault(), _typeFormatter, context);
		}

		public TypeCollectionTester TestScriptText()
		{
			TypeWriteContext context = new TypeWriteContext()
			{
				IncludedTypes = _typeCollection,
				TypeCollection = _typeCollection,
				OutputPath = TestWorkspaceBuilder.DefaultResultPath,

				TypeNamespace = ReferenceTypeTester.TestNamespace,
				EnumNamespace = EnumTester.TestNamespace,
			};
			string scriptText = _scriptWriter.CreateTypeTemplate().GetText(context);
			Assert.IsFalse(string.IsNullOrEmpty(scriptText));
			return this;
		}

		public TypeCollectionTester AssertScriptText(string expectedText)
		{
			TypeWriteContext context = new TypeWriteContext()
			{
				IncludedTypes = _typeCollection,
				TypeCollection = _typeCollection,
				OutputPath = TestWorkspaceBuilder.DefaultResultPath,

				TypeNamespace = ReferenceTypeTester.TestNamespace,
				EnumNamespace = EnumTester.TestNamespace,
			};
			string scriptText = _scriptWriter.CreateTypeTemplate().GetText(context).Trim();
			expectedText = expectedText.Trim();
			Assert.AreEqual(expectedText, scriptText);
			return this;
		}

		public ControllerContext GetDefaultControllerContext(List<ActionConfig> actionConfig = null)
		{
			FetchFunctionResolver resolver = new FetchFunctionResolver(new Uri(@"C:\FolderA\FolderB\Project.csproj"), actionConfig ?? GetDefaultActionConfig());
			return new ControllerContext()
			{
				WebMethodNamespace = "MethodNamespace",
				TypeCollection = _typeCollection,
				ServerObjectsResultFilepath = new Uri(@"C:\FolderA\FolderB\FolderC\FolderD\ServerObjects.ts"),
				OutputPath = @"C:\FolderA\FolderB\FolderX\FolderY\SomeController.ts",

				FetchFunctionResolver = resolver,
				ModelBinding = ModelBindingType.MultiParam, // TODO stop this

				TypeNamespace = ReferenceTypeTester.TestNamespace,
				EnumNamespace = EnumTester.TestNamespace,
			};
		}

		public List<ActionConfig> GetDefaultActionConfig()
		{
			return new List<ActionConfig>()
			{
				new ActionConfig()
				{
					FetchFilePath = @".\FolderM\FolderN\AjaxFunc.ts",
					FetchFunctionName = "TestAjax",
					Imports = new List<ImportDefinition>(),
					Method = RequestMethod.Default,
					Parameters = new List<ActionParameter>()
					{
						new ActionParameter() {Name = "success", Type = "(result: $returnType$) => void", Optional = true},
						new ActionParameter() {Name = "fail", Type = "(result: any) => void", Optional = true }
					},
					ReturnType = "void"
				}
			};
		}

		public TypeCollectionTester AssertControllerScriptText(string controllerName, string expectedText)
		{

			ControllerContext context = GetDefaultControllerContext();

			return AssertControllerScriptText(controllerName, context, expectedText);
		}

		public TypeCollectionTester AssertControllerScriptText(string controllerName, ControllerContext context, string expectedText)
		{

			string scriptText = _scriptWriter.CreateControllerTextTemplate().GetText(
				_typeCollection.GetMvcControllers().Where(c => c.Name == controllerName).First(),
				context
				).Trim();

			expectedText = expectedText.Trim();
			Assert.AreEqual(expectedText, scriptText);
			return this;
		}
	}
}
