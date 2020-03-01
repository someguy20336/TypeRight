﻿using TypeRight.ScriptWriting.TypeScript;
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
	public class ComplexPropertyTypeTests
	{
		private static TypeCollectionTester _packageTester;

		private const string Class_Extracted = "ExtractedClassName";

		private const string Class_OtherExtracted = "OtherExtractedClassName";

		private const string Class_NotExtracted = "NotExtractedClassName";

		private const string Prop_NotExtracted = "Prop_Extracted_NotExtracted";

		private const string Prop_OtherExtracted = "OtherExractedClassProperty";

		private const string Prop_ExtractedRecursive = "ExractedRecursiveProperty";

		/// <summary>
		/// Sets up a parse of this solution
		/// </summary>
		[ClassInitialize]
		public static void SetupParse(TestContext context)
		{
			TestWorkspaceBuilder wkspBuilder = new TestWorkspaceBuilder();

			wkspBuilder.DefaultProject
				.AddFakeTypeRight()

				// create a class that isn't extracted
				.CreateClassBuilder(Class_NotExtracted)
					.AddProperty("WhoCares", "int")
					.Commit()

				// Create another class that is extracted
				.CreateClassBuilder(Class_OtherExtracted)
					.AddScriptObjectAttribute()
					.AddProperty("AlsoWhoCares", "string")
					.Commit()

				// And create an extracted class that get all
				.CreateClassBuilder(Class_Extracted)
					.AddScriptObjectAttribute()
					.AddProperty(Prop_NotExtracted, Class_NotExtracted)
					.AddProperty(Prop_OtherExtracted, Class_OtherExtracted)
					.AddProperty(Prop_ExtractedRecursive, Class_Extracted)
					.Commit();
			
			_packageTester = wkspBuilder.GetPackageTester();
		}

		[TestMethod]
		public void Complex_AnyForNotExtracted()
		{
			_packageTester.TestReferenceTypeWithName(Class_Extracted)
				.TestPropertyWithName(Prop_NotExtracted)
				.TypescriptNameIs(TypeScriptHelper.AnyTypeName);
		}

		[TestMethod]
		public void Complex_HasExtractedClassProperty()
		{
			_packageTester.TestReferenceTypeWithName(Class_Extracted)
				.TestPropertyWithName(Prop_OtherExtracted)
				.TypescriptNameIs($"{ReferenceTypeTester.TestNamespace}.{Class_OtherExtracted}");
		}

		[TestMethod]
		public void Complex_ScriptWrites()
		{
			_packageTester.TestScriptText();
		}
	}
}