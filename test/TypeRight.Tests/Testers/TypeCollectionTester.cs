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
		public const string FetchFilePath = @".\FolderM\FolderN\FetchFile.ts";

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

			var templateFactory = new ScriptTemplateFactory(new ConfigOptions());
			string scriptText = templateFactory.CreateTypeTextTemplate().GetText(context).Trim();
			expectedText = expectedText.Trim();
			Assert.AreEqual(expectedText, scriptText);
			return this;
		}

		public ControllerContext GetDefaultControllerContext(string controllerName, ConfigOptions config)
		{
			FetchFunctionResolver resolver = FetchFunctionResolver.FromConfig(new Uri(@"C:\FolderA\FolderB\Project.csproj"), config);
			return new ControllerContext(
				_typeCollection.GetMvcControllers().Where(c => c.Name == controllerName).ToList(),
				@"C:\FolderA\FolderB\FolderX\FolderY\SomeController.ts",
				_typeCollection,
				resolver
				);
		}

		public static FetchConfig GetDefaultFetchConfig()
		{

			return new FetchConfig()
			{
				FilePath = FetchFilePath,
				Name = "fetchWrapper",
				Imports = new List<ImportDefinition>(),
				Parameters = new List<ActionParameter>()
				{
					ActionParameter.RequestMethod,
					ActionParameter.Url,
					ActionParameter.Body
				},
				//ReturnType = "$returnType$"		// Would be nice
				ReturnType = "void"	
			};
		}


	}
}
