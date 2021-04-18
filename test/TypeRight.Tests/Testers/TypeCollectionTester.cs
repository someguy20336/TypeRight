using TypeRight.Configuration;
using TypeRight.ScriptWriting;
using TypeRight.ScriptWriting.TypeScript;
using TypeRight.TypeProcessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TypeRight.Tests.TestBuilders;

namespace TypeRight.Tests.Testers
{
	public class TypeCollectionTester
	{
		public const string FetchFilePath = @".\FolderM\FolderN\AjaxFunc.ts";

		private ExtractedTypeCollection _typeCollection;

		private readonly TypeFormatter _typeFormatter;

		public TypeCollectionTester(ExtractedTypeCollection typeCollection)
		{
			_typeCollection = typeCollection;

			// TODO any way to define this?  Or maybe an option when getting type name
			_typeFormatter = new TypeScriptTypeFormatter(_typeCollection, new FakeTypePrefixer());
		}

		public ReferenceTypeTester TestReferenceTypeWithName(string name, int? typeArgCnt = null)
		{
			ExtractedReferenceType extrType = _typeCollection.GetReferenceTypes()
					.Where(type => type.NamedType.Name == name && (!typeArgCnt.HasValue || typeArgCnt.Value == type.NamedType.TypeArguments.Count))
					.FirstOrDefault();
			return new ReferenceTypeTester(extrType, _typeFormatter);
		}

		public EnumTester TestEnumWithName(string name)
		{
			return new EnumTester(_typeCollection.GetEnumTypes().Where(en => en.Name == name).FirstOrDefault());
		}

		public ControllerTester TestControllerWithName(ControllerContext context)
		{
			return new ControllerTester(_typeFormatter, context);
		}

		public TypeCollectionTester AssertScriptText(string expectedText)
		{
			TypeWriteContext context = new TypeWriteContext(_typeCollection, _typeCollection, TestWorkspaceBuilder.DefaultResultPath);
			string scriptText = ScriptTemplateFactory.CreateTypeTextTemplate().GetText(context).Trim();
			expectedText = expectedText.Trim();
			Assert.AreEqual(expectedText, scriptText);
			return this;
		}

		public ControllerContext GetDefaultControllerContext(string controllerName, ConfigOptions config)
		{
			FetchFunctionResolver resolver = FetchFunctionResolver.FromConfig(new Uri(@"C:\FolderA\FolderB\Project.csproj"), config);
			return new ControllerContext(
				_typeCollection.GetMvcControllers().Where(c => c.Name == controllerName).FirstOrDefault(),
				@"C:\FolderA\FolderB\FolderX\FolderY\SomeController.ts",
				_typeCollection,
				resolver
				);
		}

		public static List<ActionConfig> GetDefaultActionConfig()
		{
			return new List<ActionConfig>()
			{
				new ActionConfig()
				{
					FetchFilePath = FetchFilePath,
					FetchFunctionName = "TestAjax",
					Imports = new List<ImportDefinition>(),
					Method = RequestMethod.Default.Name,
					Parameters = new List<ActionParameter>()		// TODO: this is dupliated in ControllerWritingTestsBase... don't
					{
						new ActionParameter("success", "(result: $returnType$) => void", true),
						new ActionParameter("fail", "(result: any) => void", true)
					},
					ReturnType = "void"
				},
				new ActionConfig()
				{
					FetchFilePath = FetchFilePath,
					FetchFunctionName = "callDelete",
					Imports = new List<ImportDefinition>(),
					Method = "DELETE",
					ReturnType = "void"
				}
			};
		}


		public TypeCollectionTester AssertControllerScriptText(ControllerContext context, string expectedText)
		{
			string scriptText = ScriptTemplateFactory.CreateControllerTextTemplate().GetText(context).Trim();

			expectedText = expectedText.Trim();
			Assert.AreEqual(expectedText, scriptText);
			return this;
		}
	}
}
