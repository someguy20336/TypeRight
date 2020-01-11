using System;
using TypeRight.ScriptWriting.TypeScript;
using TypeRightTests.HelperClasses;
using TypeRightTests.TestBuilders;
using TypeRightTests.Testers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TypeRightTests.TestsWithParsing
{
	[TestClass]
	public class GenericsTests
	{
		private static TypeCollectionTester _packageTester;

		private const string Class_SimpleGeneric = "GenericsClass";

		private const string Class_ExtendedGeneric = "ExtendedGeneric";

		private const string GenericParameterName = "TType";

		private const string GenericPropertyName = "GenericProperty";

		private const string Property_ExtendedGenericProp = "ExtendedGenericProp";

		/// <summary>
		/// Sets up a parse of this solution
		/// </summary>
		[ClassInitialize]
		public static void SetupParse(TestContext context)
		{
			TestWorkspaceBuilder wkspBuilder = new TestWorkspaceBuilder();

			wkspBuilder.DefaultProject
				.AddFakeTypeRight()

				// Simple Generic Class with Type Param
				.CreateClassBuilder(Class_SimpleGeneric)
					.AddScriptObjectAttribute()
					.AddGenericParameter(GenericParameterName)
					.AddProperty(GenericPropertyName, GenericParameterName)
					.Commit()
				
				// Generic that inherits from the simple generic
				.CreateClassBuilder("ExtendedGeneric")
					.AddScriptObjectAttribute()
					.AddGenericParameter("T")
					.AddBaseClass(Class_SimpleGeneric + "<T>")
					.AddProperty(Property_ExtendedGenericProp, "T")
					.Commit()

				// NonExtracted Base Generic
				.CreateClassBuilder("NonExtractedGeneric")
					.AddGenericParameter("TType")
					.AddProperty("IsStillExtracted", "TType")
					.Commit()

				// Extract derived from non extracted base
				.CreateClassBuilder("ExtendedGenericWithBaseNonExtracted")
					.AddScriptObjectAttribute()
					.AddBaseClass("NonExtractedGeneric<T>")
					.AddGenericParameter("T")
					.AddProperty("ExtractedYay", "T")
					.AddProperty("ExtractedGeneric", $"{Class_SimpleGeneric}<int>")
					.Commit()

				// Derived class with resolved TypeParam
				.CreateClassBuilder("ExtendsGenericWithResolved")
					.AddScriptObjectAttribute()
					.AddBaseClass(Class_SimpleGeneric + "<bool>")
					.AddProperty("DontCareProperty", "int")
					.Commit()

				// Derived class with resolved TypeParam
				.CreateClassBuilder("ExtendsNonExtractedGenericWithResolved")
					.AddScriptObjectAttribute()
					.AddBaseClass("NonExtractedGeneric<string>")
					.AddProperty("AlsoDontCareProperty", "int")
					.Commit()
				;


			_packageTester = wkspBuilder.GetPackageTester();
		}

		[TestMethod]
		public void GenericTypes_ClassName()
		{
			_packageTester.TestReferenceTypeWithName(Class_SimpleGeneric)
				.ClassNameIs($"{Class_SimpleGeneric}<{GenericParameterName}>");
		}

		[TestMethod]
		public void GenericTypes_HasGenericTypeParmeter()
		{
			_packageTester.TestReferenceTypeWithName(Class_SimpleGeneric)
				.HasGenericParameter(GenericParameterName);
		}

		[TestMethod]
		public void GenericTypes_PropertyIsGenericType()
		{
			_packageTester.TestReferenceTypeWithName(Class_SimpleGeneric)
				.TestPropertyWithName(GenericPropertyName)
				.IsGenericType()
				.TypescriptNameIs(GenericParameterName);
		}

		[TestMethod]
		public void GenericTypes_ExtendedGenericClass()
		{
			_packageTester.TestReferenceTypeWithName(Class_ExtendedGeneric)
				.BaseClassNameIs($"{Class_SimpleGeneric}<T>");
		}

		[TestMethod]
		public void GenericTypes_BaseClassPropertyIsGenericType()
		{
			_packageTester.TestReferenceTypeWithName("ExtendedGenericWithBaseNonExtracted")
				.DoesNotHaveBaseClass()
				.TestPropertyWithName("IsStillExtracted")
				.TypescriptNameIs("T");
		}

		[TestMethod]
		public void GenericTypes_PropertyWithGenericType()
		{
			_packageTester.TestReferenceTypeWithName("ExtendedGenericWithBaseNonExtracted")
				.TestPropertyWithName("ExtractedGeneric")
				.TypescriptNameIs($"{ReferenceTypeTester.TestNamespace}.{Class_SimpleGeneric}<number>");
		}

		[TestMethod]
		public void GenericTypes_ExtendGenericWithResolved()
		{
			_packageTester.TestReferenceTypeWithName("ExtendsGenericWithResolved")
				.BaseClassNameIs($"{Class_SimpleGeneric}<boolean>");
		}

		[TestMethod]
		public void GenericTypes_ExtendsResolvedNonExtracted()
		{
			_packageTester.TestReferenceTypeWithName("ExtendsNonExtractedGenericWithResolved")
				.TestPropertyWithName("IsStillExtracted")
				.TypescriptNameIs(TypeScriptHelper.StringTypeName);
		}
	}
}
