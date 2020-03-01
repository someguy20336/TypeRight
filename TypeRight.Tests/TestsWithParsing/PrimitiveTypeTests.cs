﻿using TypeRight.ScriptWriting.TypeScript;
using TypeRight.Tests.HelperClasses;
using TypeRight.Tests.TestBuilders;
using TypeRight.Tests.Testers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TypeRight.Tests.TestsWithParsing
{
	[TestClass]
	public class PrimitiveTypeTests
	{

		private static TypeCollectionTester _packageTester;

		private const string ClassName = "SimplePrimitiveClass";

		private const string IntegerPropName = "IntegerProp";

		private const string StringPropName = "StringProp";

		private const string NullableIntPropName = "NullableIntProp";

		private const string IntegerArrayPropName = "IntegerArrayProp";

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
					.AddProperty(IntegerPropName, "int")
					.AddProperty(StringPropName, "string")
					.AddProperty(NullableIntPropName, "int?")
					.AddProperty(IntegerArrayPropName, "int[]")
					.Commit();


			_packageTester = wkspBuilder.GetPackageTester();
		}
		
		[TestMethod]
		public void PrimitiveTypes_TestInt()
		{

			_packageTester
				.TestReferenceTypeWithName(ClassName)
				.Exists()
				.TestPropertyWithName(IntegerPropName)
				.Exists()
				.TypeMetadataIs(typeof(int));

		}

		/// <summary>
		/// Tests int typescript name
		/// </summary>
		[TestMethod]
		public void PrimitiveTypes_IntTypescriptName()
		{
			_packageTester
				.TestReferenceTypeWithName(ClassName)
				.Exists()
				.TestPropertyWithName(IntegerPropName)
				.Exists()
				.TypescriptNameIs(TypeScriptHelper.NumericTypeName);
		}



		/// <summary>
		/// Test string typescript name
		/// </summary>
		[TestMethod]
		public void PrimitiveTypes_StringTypescriptName()
		{
			_packageTester
				.TestReferenceTypeWithName(ClassName)
				.Exists()
				.TestPropertyWithName(StringPropName)
				.Exists()
				.TypescriptNameIs(TypeScriptHelper.StringTypeName);
		}


		/// <summary>
		/// Tests a nullable typescript name (i.e. int? == number)
		/// </summary>
		[TestMethod]
		public void PrimitiveTypes_NullableIntTypescriptName()
		{
			_packageTester
				.TestReferenceTypeWithName(ClassName)
				.Exists()
				.TestPropertyWithName(NullableIntPropName)
				.Exists()
				.TypescriptNameIs(TypeScriptHelper.NumericTypeName);
		}

		/// <summary>
		/// Tests a nullable typescript name (i.e. int? == number)
		/// </summary>
		[TestMethod]
		public void PrimitiveTypes_IntArray()
		{
			_packageTester
				.TestReferenceTypeWithName(ClassName)
				.Exists()
				.TestPropertyWithName(IntegerArrayPropName)
				.Exists()
				.TypescriptNameIs(TypeScriptHelper.NumericTypeName + "[]");
		}
	}
}