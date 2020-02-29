using System;
using TypeRight.Tests.HelperClasses;
using TypeRight.Tests.TestBuilders;
using TypeRight.Tests.Testers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TypeRight.Tests.TestsWithParsing
{
	[TestClass]
	public class DocumentationTests
	{

		private static TypeCollectionTester _packageTester;

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
				.CreateClassBuilder("SimpleClass")
					.AddScriptObjectAttribute()
					.SetDocumentationComments("These are comments")
					.AddProperty("SimpleProp", "int", "Property Comments Here")
					.Commit()				
				;
			
			_packageTester = wkspBuilder.GetPackageTester();
		}

		[TestMethod]
		public void DocumentationTests_ClassHasComments()
		{
			_packageTester.TestReferenceTypeWithName("SimpleClass")
				.CommentsAre("These are comments");
		}

		[TestMethod]
		public void DocumentationTests_PropertyHasComments()
		{
			_packageTester.TestReferenceTypeWithName("SimpleClass")
				.TestPropertyWithName("SimpleProp")
				.CommentsAre("Property Comments Here");
		}
	}
}
