using System;
using TypeRight.ScriptWriting.TypeScript;
using TypeRight.Tests.HelperClasses;
using TypeRight.Tests.TestBuilders;
using TypeRight.Tests.Testers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TypeRight.Tests.Types
{
	[TestClass]
	public class GenericsTests : TypesTestBase
	{
		[TestMethod]
		public void GenericTypes_ClassName()
		{
			AddDefaultExtractedClass()
				.AddGenericParameter("T")
				.AddProperty("GenericProp", "T")
				.Commit();

			AssertThatTheDefaultReferenceType()
				.ClassNameIs($"{TestRefTypeName}<T>");
		}

		[TestMethod]
		public void GenericTypes_HasGenericTypeParmeter()
		{
			AddDefaultExtractedClass()
				.AddGenericParameter("T")
				.AddProperty("GenericProp", "T")
				.Commit();

			AssertThatTheDefaultReferenceType()
				.HasGenericParameter("T");
		}

		[TestMethod]
		public void GenericTypes_PropertyIsGenericType()
		{
			AddDefaultExtractedClass()
				.AddGenericParameter("T")
				.AddProperty("Prop", "T")
				.Commit();

			AssertThatTheDefaultReferenceType()
				.TestPropertyWithName("Prop")
				.IsGenericType()
				.TypescriptNameIs("T");
		}

		[TestMethod]
		public void GenericTypes_ExtendedGenericClass()
		{
			AddExtractedClass("BaseClass")
				.AddGenericParameter("T")
				.AddProperty("BaseProp", "T")
				.Commit();

			// Generic that inherits from the simple generic
			AddDefaultExtractedClass()
				.AddGenericParameter("T")
				.AddBaseClass("BaseClass<T>")
				.Commit();

			AssertThatTheDefaultReferenceType()
				.BaseClassNameIs($"BaseClass<T>");
		}

		[TestMethod]
		public void GenericTypes_BaseClassPropertyIsGenericType()
		{
			AddClass("NonExtractedGeneric")
				.AddGenericParameter("TType")
				.AddProperty("IsStillExtracted", "TType")
				.Commit();

			AddDefaultExtractedClass()
				.AddBaseClass("NonExtractedGeneric<T>")
				.AddGenericParameter("T")
				.Commit();

			AssertThatTheDefaultReferenceType()
				.DoesNotHaveBaseClass()
				.TestPropertyWithName("IsStillExtracted")
				.TypescriptNameIs("T");
		}

		[TestMethod]
		public void GenericTypes_PropertyWithGenericType()
		{
			AddExtractedClass("ExtractedGeneric")
				.AddGenericParameter("TType")
				.Commit();

			AddDefaultExtractedClass()
				.AddProperty("ExtractedGenericProp", $"ExtractedGeneric<int>")
				.Commit();
			
			AssertThatTheDefaultReferenceType()
				.TestPropertyWithName("ExtractedGenericProp")
				.TypescriptNameIs($"{FakeTypePrefixer.Prefix}.ExtractedGeneric<number>");
		}

		[TestMethod]
		public void GenericTypes_ExtendGenericWithResolved()
		{
			AddExtractedClass("BaseClass")
				.AddGenericParameter("TType")
				.Commit();

			AddDefaultExtractedClass()
				.AddBaseClass("BaseClass<bool>")
				.Commit();

			AssertThatTheDefaultReferenceType()
				.BaseClassNameIs($"BaseClass<boolean>");
		}

		[TestMethod]
		public void GenericTypes_ExtendsResolvedNonExtracted()
		{
			AddClass("NonExtractedGeneric")
				.AddGenericParameter("TType")
				.AddProperty("IsStillExtracted", "TType")
				.Commit();

			AddDefaultExtractedClass()
				.AddBaseClass("NonExtractedGeneric<string>")
				.Commit();

			AssertThatTheDefaultReferenceType()
				.TestPropertyWithName("IsStillExtracted")
				.TypescriptNameIs(TypeScriptHelper.StringTypeName);
		}
	}
}
