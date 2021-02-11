using System;
using TypeRight.Tests.HelperClasses;
using TypeRight.Tests.TestBuilders;
using TypeRight.Tests.Testers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TypeRight.Tests.Types
{
	[TestClass]
	public class DocumentationTests : TypesTestBase
	{
		[TestMethod]
		public void DocumentationTests_ClassHasComments()
		{
			AddDefaultExtractedClass()
				.SetDocumentationComments("These are comments")
				.Commit();

			AssertThatTheDefaultReferenceType()
				.CommentsAre("These are comments");
		}

		[TestMethod]
		public void DocumentationTests_PropertyHasComments()
		{
			AddDefaultExtractedClass()
				.AddProperty("SimpleProp", "int", "Property Comments Here")
				.Commit();

			AssertThatTheDefaultReferenceType()
				.TestPropertyWithName("SimpleProp")
				.CommentsAre("Property Comments Here");
		}
	}
}
