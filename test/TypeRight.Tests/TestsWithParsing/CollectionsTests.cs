using TypeRight.ScriptWriting.TypeScript;
using TypeRight.Tests.HelperClasses;
using TypeRight.Tests.TestBuilders;
using TypeRight.Tests.Testers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRight.Tests.TestsWithParsing
{
	[TestClass]
	public class CollectionsTests
	{
		private static TypeCollectionTester _packageTester;

		private const string ClassName = "CollectionsClass";

		private const string StringListProperty = "StringListProperty";

		private const string StringIntDictionaryProperty = "StringIntDictionaryProperty";

		/// <summary>
		/// Sets up a parse of this solution
		/// </summary>
		[ClassInitialize]
		public static void SetupParse(TestContext context)
		{
			TestWorkspaceBuilder wkspBuilder = new TestWorkspaceBuilder();

			wkspBuilder.DefaultProject
				.AddFakeTypeRight()
				.CreateClassBuilder(ClassName)
				.AddScriptObjectAttribute()
				.AddProperty(StringListProperty, "List<string>")
				.AddProperty(StringIntDictionaryProperty, "Dictionary<string, int>")
                .AddProperty("EnumerableString", "IEnumerable<string>")
				.AddProperty("EnumerableObject", "System.Collections.IEnumerable")
				.Commit();
			
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

        [TestMethod]
        public void Collections_IEnumerable_1()
        {
            _packageTester.TestReferenceTypeWithName(ClassName)
                .TestPropertyWithName("EnumerableString")
                .TypescriptNameIs(TypeScriptHelper.StringTypeName + "[]");
        }

		[TestMethod]
		public void Collections_IEnumerable()
		{
			_packageTester.TestReferenceTypeWithName(ClassName)
				.TestPropertyWithName("EnumerableObject")
				.TypescriptNameIs(TypeScriptHelper.AnyTypeName + "[]");
		}
	}
}
