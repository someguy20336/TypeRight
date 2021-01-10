using TypeRight.CodeModel;
using TypeRight.ScriptWriting;
using TypeRight.ScriptWriting.TypeScript;
using TypeRight.TypeProcessing;
using TypeRight.Tests.HelperClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TypeRight.Tests.Testers
{
	class PropertyTester
	{
		private ExtractedProperty _property;

		private readonly TypeFormatter _typeFormatter;
		
		public PropertyTester(ExtractedProperty property, TypeFormatter typeFormatter)
		{
			_property = property;
			_typeFormatter = typeFormatter;
		}

		public PropertyTester CommentsAre(string comments)
		{
			Assert.AreEqual(comments, _property.Comments);
			return this;
		}
		public PropertyTester Exists()
		{
			Assert.IsNotNull(_property);
			return this;
		}

		public PropertyTester DoesNotExist()
		{
			Assert.IsNull(_property);
			return this;
		}

		public PropertyTester TypeMetadataIs(Type type)
		{
			if (_property.Type.Type is INamedType namedType)
			{
				Assert.AreEqual(type.FullName, namedType.FullName);
			}
			else
			{
				Assert.Fail();
			}
			
			return this;
		}

		public PropertyTester IsGenericType()
		{
			Assert.IsNotNull(_property.Type.Type is ITypeParameter);
			return this;
		}

		public PropertyTester TypescriptNameIs(string typescriptName)
		{
			Assert.AreEqual(typescriptName, _property.Type.FormatType(_typeFormatter));
			return this;
		}
	}
}
