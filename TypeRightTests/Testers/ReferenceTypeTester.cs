using TypeRight.CodeModel;
using TypeRight.Packages;
using TypeRight.ScriptWriting;
using TypeRight.ScriptWriting.TypeScript;
using TypeRight.TypeProcessing;
using TypeRightTests.HelperClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace TypeRightTests.Testers
{
	class ReferenceTypeTester
	{
		private INamedType NamedType => _namedObjectType.NamedType;

		private ExtractedReferenceType _namedObjectType;

		private TypeFormatter _typeFormatter;

		public const string TestNamespace = "DefaultClass";
		
		public ReferenceTypeTester(ExtractedReferenceType scriptClass, TypeFormatter typeFormatter)
		{
			_namedObjectType = scriptClass;
			_typeFormatter = typeFormatter;
		}

		public ReferenceTypeTester CommentsAre(string comments)
		{
			Assert.AreEqual(comments, NamedType.Comments);
			return this;
		}

		public ReferenceTypeTester Exists()
		{
			Assert.IsNotNull(NamedType);
			return this;
		}

		public ReferenceTypeTester HasInterfaceWithName(string name)
		{
			Assert.IsNotNull(_namedObjectType.Interfaces.FirstOrDefault(i => i.Name == name));
			return this;
		}

		public ReferenceTypeTester DoesNotHaveInterfaceWithName(string name)
		{
			Assert.IsNull(_namedObjectType.Interfaces.FirstOrDefault(i => i.Name == name));
			return this;
		}

		public PropertyTester TestPropertyWithName(string name)
		{
			return new PropertyTester(GetPropertyWithName(name), _typeFormatter);
		}


		public ReferenceTypeTester HasGenericParameter(string genericParamName)
		{
			Assert.IsTrue(NamedType.TypeArguments.Any(test => test.Name == genericParamName));
			return this;
		}

		public ReferenceTypeTester ClassNameIs(string name)
		{
			Assert.AreEqual(name, _namedObjectType.GetClassDeclaration(_typeFormatter));
			return this;
		}

		public ReferenceTypeTester DoesNotHaveBaseClass()
		{
			Assert.IsNull(_namedObjectType.BaseType);
			return this;
		}

		public ReferenceTypeTester BaseClassNameIs(string expectedName)
		{
			Assert.AreEqual($"{TestNamespace}.{expectedName}", GetBaseClassName());
			return this;
		}

		private string GetBaseClassName()
		{
			return _namedObjectType.BaseType.FormatType(_typeFormatter);
		}

		public ReferenceTypeTester WillWritePropertyWithName(string propName)
		{
			Assert.IsTrue(ContainsPropertyWithName(propName));
			return this;
		}

		public ReferenceTypeTester WillNotWritePropertyWithName(string propName)
		{
			Assert.IsFalse(ContainsPropertyWithName(propName));
			return this;
		}

		private bool ContainsPropertyWithName(string propName)
		{
			return GetPropertyWithName(propName) != null;
		}

		private ExtractedProperty GetPropertyWithName(string name)
		{
			return _namedObjectType.Properties.FirstOrDefault(pr => pr.Name == name);
		}
	}
}
