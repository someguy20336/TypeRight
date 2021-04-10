﻿using TypeRight.Configuration;
using TypeRight.ScriptWriting;
using TypeRight.ScriptWriting.TypeScript;
using TypeRight.TypeProcessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TypeRight.Tests.TestBuilders;
using System.Collections.Specialized;

namespace TypeRight.Tests.Testers
{
	public class TypeCollectionTester
	{
		private ExtractedTypeCollection _typeCollection;

		private IScriptTemplate _scriptWriter;

		private readonly TypeFormatter _typeFormatter;

		public TypeCollectionTester(ExtractedTypeCollection typeCollection)
		{
			_typeCollection = typeCollection;

			// TODO not hardcode?
			_scriptWriter = new ModuleTemplate();

			// TODO any way to define this?  Or maybe an option when getting type name
			_typeFormatter = new TypeScriptTypeFormatter(_typeCollection, new FakeTypePrefixer());
		}

		public ReferenceTypeTester TestReferenceTypeWithName(string name, int? typeArgCnt = null)
		{
			ExtractedReferenceType extrType = _typeCollection.GetReferenceTypes()
					.Where(type => type.NamedType.Name == name && (!typeArgCnt.HasValue || typeArgCnt.Value == type.NamedType.TypeArguments.Count))
					.FirstOrDefault() as ExtractedReferenceType;
			return new ReferenceTypeTester(extrType, _typeFormatter);
		}

		public EnumTester TestEnumWithName(string name)
		{
			return new EnumTester(_typeCollection.GetEnumTypes().Where(en => en.Name == name).FirstOrDefault());
		}

		public ControllerTester TestControllerWithName(string name, ControllerContext context = null)
		{
			context = context ?? GetDefaultControllerContext(name);
			return new ControllerTester(_typeFormatter, context);
		}

		public TypeCollectionTester TestScriptText()
		{
			TypeWriteContext context = new TypeWriteContext(_typeCollection, _typeCollection, TestWorkspaceBuilder.DefaultResultPath);
			string scriptText = _scriptWriter.CreateTypeTemplate().GetText(context);
			Assert.IsFalse(string.IsNullOrEmpty(scriptText));
			return this;
		}

		public TypeCollectionTester AssertScriptText(string expectedText)
		{
			TypeWriteContext context = new TypeWriteContext(_typeCollection, _typeCollection, TestWorkspaceBuilder.DefaultResultPath);
			string scriptText = _scriptWriter.CreateTypeTemplate().GetText(context).Trim();
			expectedText = expectedText.Trim();
			Assert.AreEqual(expectedText, scriptText);
			return this;
		}

		public ControllerContext GetDefaultControllerContext(string controllerName, List<ActionConfig> actionConfig = null, NameValueCollection queryParams = null)
		{
			FetchFunctionResolver resolver = new ActionConfigFetchFunctionResolver(new Uri(@"C:\FolderA\FolderB\Project.csproj"), actionConfig ?? GetDefaultActionConfig(), queryParams);
			return new ControllerContext(
				_typeCollection.GetMvcControllers().Where(c => c.Name == controllerName).FirstOrDefault(),
				@"C:\FolderA\FolderB\FolderX\FolderY\SomeController.ts",
				_typeCollection,
				new Uri(@"C:\FolderA\FolderB\FolderC\FolderD\ServerObjects.ts"),
				resolver,
				queryParams: queryParams
				);
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
					Method = RequestMethod.Default.Name,
					Parameters = new List<ActionParameter>()		// TODO: this is dupliated in ControllerWritingTestsBase... don't
					{
						new ActionParameter() {Name = "success", Type = "(result: $returnType$) => void", Optional = true},
						new ActionParameter() {Name = "fail", Type = "(result: any) => void", Optional = true }
					},
					ReturnType = "void"
				},
				new ActionConfig()
				{
					FetchFilePath = @".\FolderM\FolderN\AjaxFunc.ts",
					FetchFunctionName = "callDelete",
					Imports = new List<ImportDefinition>(),
					Method = "DELETE",
					ReturnType = "void"
				}
			};
		}


		public TypeCollectionTester AssertControllerScriptText(ControllerContext context, string expectedText)
		{
			string scriptText = _scriptWriter.CreateControllerTextTemplate().GetText(context).Trim();

			expectedText = expectedText.Trim();
			Assert.AreEqual(expectedText, scriptText);
			return this;
		}
	}
}
