using TypeRight.Tests.HelperClasses;
using TypeRight.Tests.TestBuilders;
using TypeRight.Tests.Testers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TypeRight.Tests.Types
{
	[TestClass]
	public class InteritenceTests : TypesTestBase
	{
		private const string SimpleBaseClassName = "SimpleBaseClassName";
		private const string BaseClassIntProperty = "BaseClassIntProperty";


		[TestInitialize]
		public override void TestInitialize()
		{
			base.TestInitialize();

			// Simple Base class, extracted
			AddExtractedClass(SimpleBaseClassName)
				.AddProperty(BaseClassIntProperty, "int")
				.Commit();

			// Non extracted base class
			AddClass("NotExtracted")
				.AddProperty("IsStillExtractedProperty", "string")
				.Commit();

			AddExtractedInterface("IBaseInterface")
				.AddProperty("BaseProperty", "string")
				.Commit();

			AddInterface("INotExtractedInterface")
				.AddProperty("NotExtractedProp", "int")
				.Commit();
		}

		[TestMethod]
		public void Inheritence_Simple()
		{
			AddDefaultExtractedClass()
				.AddBaseClass(SimpleBaseClassName)
				.Commit();

			AssertThatTheDefaultReferenceType()
				.BaseClassNameIs(SimpleBaseClassName);
		}

		[TestMethod]
		public void Inheritence_DoesNotContainBaseClassForNonExtracted()
		{
			AddDefaultExtractedClass()
				.AddBaseClass("NotExtracted")
				.Commit();

			AssertThatTheDefaultReferenceType()
				.DoesNotHaveBaseClass();
		}

		[TestMethod]
		public void Inheritence_ContainsNonExtractedBaseProperties()
		{
			AddDefaultExtractedClass()
				.AddBaseClass("NotExtracted")
				.Commit();

			AssertThatTheDefaultReferenceType()
				.WillWritePropertyWithName("IsStillExtractedProperty");
		}
		

		[TestMethod]
		public void Inheritence_DoesNotContainBaseExtractedProperties()
		{
			AddDefaultExtractedClass()
				.AddBaseClass(SimpleBaseClassName)
				.Commit();

			AssertThatTheDefaultReferenceType()
				.WillNotWritePropertyWithName(BaseClassIntProperty);
		}


		[TestMethod]
		public void Inheritence_ExtractedWithMiddleNonExtracted()
		{
			AddClass("NotExtractedWithBase")
				.AddBaseClass(SimpleBaseClassName)
				.AddProperty("NotExtractedWithBaseProp", "int")
				.Commit();

			// Extracted with non-extracted base
			AddDefaultExtractedClass()
				.AddScriptObjectAttribute()
				.AddBaseClass("NotExtractedWithBase")
				.AddProperty("MyExtractedOProp", "string")
				.Commit();
			
			AssertThatTheDefaultReferenceType()
				.BaseClassNameIs(SimpleBaseClassName)
				.TestPropertyWithName("NotExtractedWithBaseProp") 
				.Exists();
		}

		
		[TestMethod]
		public void Inheritence_SimpleDerivedInterface()
		{
			AddDefaultExtractedInterface()
				.AddBaseInterface("IBaseInterface")
				.Commit();

			AssertThatTheDefaultReferenceType()
				.HasInterfaceWithName("IBaseInterface");
		}

		[TestMethod]
		public void Inheritence_ComplexDerivedInterface()
		{
			AddExtractedInterface("IDerivedInterface")
				.AddBaseInterface("IBaseInterface")
				.AddProperty("DerivedProp", "int")
				.Commit();

			AddDefaultExtractedInterface()
				.AddBaseInterface("IBaseInterface")
				.AddBaseInterface("IDerivedInterface")
				.AddBaseInterface("INotExtractedInterface")
				.AddProperty("ComplexDerivedProp", "int")
				.Commit();

			AssertThatTheDefaultReferenceType()
				.HasInterfaceWithName("IBaseInterface")
				.HasInterfaceWithName("IDerivedInterface")
				.DoesNotHaveInterfaceWithName("INotExtractedInterface");
		}

		[TestMethod]
		public void Inheritence_InterfacePropertyOnNonExtractedInterfaceExists()
		{
			AddExtractedInterface("IDerivedInterface")
				.AddBaseInterface("IBaseInterface")
				.AddProperty("DerivedProp", "int")
				.Commit();

			AddDefaultExtractedInterface()
				.AddBaseInterface("IBaseInterface")
				.AddBaseInterface("IDerivedInterface")
				.AddBaseInterface("INotExtractedInterface")
				.AddProperty("ComplexDerivedProp", "int")
				.Commit();

			AssertThatTheDefaultReferenceType()
				.TestPropertyWithName("NotExtractedProp")
				.Exists(); 
		}

		[TestMethod]
		public void Inheritence_InterfacePropertyExtractedInterfaceDoesNotExist()
		{
			AddExtractedInterface("IDerivedInterface")
				.AddBaseInterface("IBaseInterface")
				.AddProperty("DerivedProp", "int")
				.Commit();

			AddDefaultExtractedInterface()
				.AddBaseInterface("IBaseInterface")
				.AddBaseInterface("IDerivedInterface")
				.AddBaseInterface("INotExtractedInterface")
				.AddProperty("ComplexDerivedProp", "int")
				.Commit();

			AssertThatTheDefaultReferenceType()
				.TestPropertyWithName("DerivedProp")
				.DoesNotExist();
		}

		// TODO: should actually test the script writing here...
		//[TestMethod]
		//public void Inheritence_ScriptWrites()  
		//{
		//	_packageTester.TestScriptText();
		//}
	}
}
