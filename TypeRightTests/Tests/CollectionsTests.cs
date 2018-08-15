using TypeRight.ScriptWriting.TypeScript;
using TypeRightTests.HelperClasses;
using TypeRightTests.TestBuilders;
using TypeRightTests.Testers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRightTests.Tests
{
	[TestClass]
	public class CollectionsTests
	{
		private PackageTester _packageTester;

		private const string ClassName = "CollectionsClass";

		private const string StringListProperty = "StringListProperty";

		private const string StringIntDictionaryProperty = "StringIntDictionaryProperty";

		/// <summary>
		/// Sets up a parse of this solution
		/// </summary>
		[TestInitialize]
		public void SetupParse()
		{
			TestWorkspaceBuilder wkspBuilder = new TestWorkspaceBuilder();

			wkspBuilder.DefaultProject
				.CreateClassBuilder(ClassName)
				.AddProperty(StringListProperty, "List<string>")
				.AddProperty(StringIntDictionaryProperty, "Dictionary<string, int>")
				.Commit();

			wkspBuilder.ClassParseFilter = new AlwaysAcceptFilter();

			_packageTester = wkspBuilder.GetPackageTester();
		}

		[TestMethod]
		public void Collections_StringList()
		{
			_packageTester
				.TestReferenceTypeWithName(ClassName)
				.Exists()
				.TestPropertyWithName(StringListProperty)
				.Exists()
				.TypescriptNameIs(TypeScriptHelper.StringTypeName + "[]");
		}

		[TestMethod]
		public void Collections_StringIntDictionary()
		{
			string expectedName = $"{{ [key: {TypeScriptHelper.StringTypeName}]: {TypeScriptHelper.NumericTypeName} }}";
			_packageTester
				.TestReferenceTypeWithName(ClassName)
				.Exists()
				.TestPropertyWithName(StringIntDictionaryProperty)
				.Exists()
				.TypescriptNameIs(expectedName);
		}
	}
}
