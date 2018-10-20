using TypeRightTests.HelperClasses;
using TypeRightTests.TestBuilders;
using TypeRightTests.Testers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TypeRightTests.Tests
{
	[TestClass]
	public class InteritenceTests
	{
		private static PackageTester _packageTester;

		private const string SimpleBaseClassName = "SimpleBaseClassName";

		private const string SimpleDerivedClassName = "SimpleDerivedClassName";

		private const string BaseClassIntProperty = "BaseClassIntProperty";

		private const string DerivedClassStringProperty = "DerivedClassStringProperty";
		/// <summary>
		/// Sets up a parse of this solution
		/// </summary>
		[ClassInitialize]
		public static void SetupParse(TestContext context)
		{
			TestWorkspaceBuilder wkspBuilder = new TestWorkspaceBuilder();

			wkspBuilder.DefaultProject
				// Simple Base class, extracted
				.CreateClassBuilder(SimpleBaseClassName)
					.AddProperty(BaseClassIntProperty, "int")
					.Commit()

				// Simple derived class, extracted
				.CreateClassBuilder(SimpleDerivedClassName)
					.AddBaseClass(SimpleBaseClassName)
					.AddProperty(DerivedClassStringProperty, "string")
					.Commit()

				// Non extracted base class
				.CreateClassBuilder("NotExtracted")
					.AddProperty("IsStillExtractedProperty", "string")
					.Commit()

				// Extracted derived class of non extracted
				.CreateClassBuilder("ExtendsNotExtracted")
					.AddBaseClass("NotExtracted")
					.AddProperty("ExtendsNotExtProperty", "int")
					.Commit()

				// Not extracted, but base is extracted
				.CreateClassBuilder("NotExtractedWithBase")
					.AddBaseClass(SimpleBaseClassName)
					.AddProperty("NotExtractedWithBaseProp", "int")
					.Commit()

				// Extracted with non-extracted base
				.CreateClassBuilder("ExtractedWithNonExtractedBase")
					.AddBaseClass("NotExtractedWithBase")
					.AddProperty("MyExtractedOProp", "string")
					.Commit()

				.CreateInterfaceBuilder("IBaseInterface")
					.AddProperty("BaseProperty", "string")
					.Commit()

				.CreateInterfaceBuilder("IDerivedInterface")
					.AddBaseInterface("IBaseInterface")
					.AddProperty("DerivedProp", "int")
					.Commit()

				.CreateInterfaceBuilder("IComplexDerivedInterface")
					.AddBaseInterface("IBaseInterface")
					.AddBaseInterface("IDerivedInterface")
					.AddBaseInterface("INotExtractedInterface")
					.AddProperty("ComplexDerivedProp", "int")
					.Commit()

				.CreateInterfaceBuilder("INotExtractedInterface")
					.AddProperty("NotExtractedProp", "int")
					.Commit()

				.CreateInterfaceBuilder("IGenericDerivedInterface")
					.AddBaseInterface("IBaseInterface")
					.AddGenericParameter("T")
					.AddProperty("GenericProp", "T")
					.Commit()
					;

			wkspBuilder.ClassParseFilter = new ExcludeWithAnyName("NotExtracted", "NotExtractedWithBase", "INotExtractedInterface");

			_packageTester = wkspBuilder.GetPackageTester();
		}

		[TestMethod]
		public void Inheritence_Simple()
		{
			_packageTester.TestReferenceTypeWithName(SimpleDerivedClassName)
				.BaseClassNameIs(SimpleBaseClassName);
		}

		[TestMethod]
		public void Inheritence_DoesNotContainBaseClassForNonExtracted()
		{
			_packageTester.TestReferenceTypeWithName("ExtendsNotExtracted")
				.DoesNotHaveBaseClass();
		}

		[TestMethod]
		public void Inheritence_ContainsNonExtractedBaseProperties()
		{
			_packageTester.TestReferenceTypeWithName("ExtendsNotExtracted")
				.WillWritePropertyWithName("IsStillExtractedProperty");
		}
		

		[TestMethod]
		public void Inheritence_DoesNotContainBaseExtractedProperties()
		{
			_packageTester.TestReferenceTypeWithName(SimpleDerivedClassName)
				.WillNotWritePropertyWithName(BaseClassIntProperty);
		}


		[TestMethod]
		public void Inheritence_ExtractedWithMiddleNonExtracted()
		{
			_packageTester.TestReferenceTypeWithName("ExtractedWithNonExtractedBase")
				.BaseClassNameIs(SimpleBaseClassName)
				.TestPropertyWithName("NotExtractedWithBaseProp") 
				.Exists();
		}

		
		[TestMethod]
		public void Inheritence_SimpleDerivedInterface()
		{
			_packageTester.TestReferenceTypeWithName("IDerivedInterface")
				.HasInterfaceWithName("IBaseInterface");
		}

		[TestMethod]
		public void Inheritence_ComplexDerivedInterface()
		{
			_packageTester.TestReferenceTypeWithName("IComplexDerivedInterface")
				.HasInterfaceWithName("IBaseInterface")
				.HasInterfaceWithName("IDerivedInterface")
				.DoesNotHaveInterfaceWithName("INotExtractedInterface");
		}

		[TestMethod]
		public void Inheritence_InterfacePropertyOnNonExtractedInterfaceExists()
		{
			_packageTester.TestReferenceTypeWithName("IComplexDerivedInterface")
				.TestPropertyWithName("NotExtractedProp")
				.Exists(); 
		}

		[TestMethod]
		public void Inheritence_InterfacePropertyExtractedInterfaceDoesNotExist()
		{
			_packageTester.TestReferenceTypeWithName("IComplexDerivedInterface")
				.TestPropertyWithName("DerivedProp")
				.DoesNotExist();
		}


		[TestMethod]
		public void Inheritence_ScriptWrites()
		{
			_packageTester.TestScriptText();
		}
	}
}
