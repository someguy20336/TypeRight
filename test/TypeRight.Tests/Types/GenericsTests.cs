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

			AssertThatTheDefaultClass()
				.ClassNameIs($"{TestClassName}<T>");
		}

		[TestMethod]
		public void GenericTypes_HasGenericTypeParmeter()
		{
			AddDefaultExtractedClass()
				.AddGenericParameter("T")
				.AddProperty("GenericProp", "T")
				.Commit();

			AssertThatTheDefaultClass()
				.HasGenericParameter("T");
		}

		[TestMethod]
		public void GenericTypes_PropertyIsGenericType()
		{
			AddDefaultExtractedClass()
				.AddGenericParameter("T")
				.AddProperty("Prop", "T")
				.Commit();

			AssertThatTheDefaultClass()
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

			AssertThatTheDefaultClass()
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

			AssertThatTheDefaultClass()
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
			
			AssertThatTheDefaultClass()
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

			AssertThatTheDefaultClass()
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

			AssertThatTheDefaultClass()
				.TestPropertyWithName("IsStillExtracted")
				.TypescriptNameIs(TypeScriptHelper.StringTypeName);
		}
	}
}
