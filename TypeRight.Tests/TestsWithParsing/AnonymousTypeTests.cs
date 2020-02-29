using TypeRight.ScriptWriting.TypeScript;
using TypeRight.Workspaces.Parsing;
using TypeRight.Tests.HelperClasses;
using TypeRight.Tests.TestBuilders;
using TypeRight.Tests.Testers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.TypeProcessing;
using TypeRight.TypeLocation;

namespace TypeRight.Tests.TestsWithParsing
{
	[TestClass]
	public class AnonymousTypeTests
	{

		private static TypeCollectionTester s_packageTester;

		/// <summary>
		/// Sets up a parse of this solution
		/// </summary>
		[ClassInitialize]
		public static void SetupParse(TestContext context)
		{

			TestWorkspaceBuilder wkspBuilder = new TestWorkspaceBuilder();

			wkspBuilder.DefaultProject
				.AddFakeMvc()
				.AddFakeTypeRight()

				// Test class to use as return/param
				.CreateClassBuilder("TestClass")
					.AddScriptObjectAttribute()
					.AddProperty("DontCare", "int")
					.Commit()

				// Not Extracted
				.CreateClassBuilder("NotExtracted")
					.AddProperty("DontCare", "int")
					.Commit()

				// Fake JsonResult-like class
				.CreateClassBuilder("FakeJsonResultLikeClass")
					.AddProperty("DontCare", "int")
					.Commit()

				// Display name attribute
				.CreateClassBuilder("SimpleController")
					.WithControllerBaseClass()

					// Fake Json Method - not extracted
					.AddMethod("Json", MvcConstants.JsonResult_AspNetCore)
						.AddParameter("data", "object")
						.AddLineOfCode("return null;", 0)
						.Commit()
					.AddMethod("SimpleAnonymousType", MvcConstants.JsonResult_AspNetCore)
						.AddScriptActionAttribute()
						.AddLineOfCode("return Json(new { stringProp = \"Hi\", intProp = 1 });", 0)
						.Commit()
					.AddMethod("AnonymousTypeWithExtracted", MvcConstants.JsonResult_AspNetCore)
						.AddScriptActionAttribute()
						.AddLineOfCode("TestClass test = new TestClass();", 0)
						.AddLineOfCode("return Json(new { testClassProp = test, intProp = 1 });", 0)
						.Commit()
					.AddMethod("AnonymousTypeWithNonExtracted", MvcConstants.JsonResult_AspNetCore)
						.AddScriptActionAttribute()
						.AddLineOfCode("NotExtracted test = new NotExtracted();", 0)
						.AddLineOfCode("return Json(new { testClassProp = test, intProp = 1 });", 0)
						.Commit()
					.Commit()

			;
			
			s_packageTester = wkspBuilder.GetPackageTester();
		}


		[TestMethod]
		public void AnonymousTypes_Simple()
		{
			Dictionary<string, string> expected = new Dictionary<string, string>
			{
				{ "stringProp", TypeScriptHelper.StringTypeName },
				{ "intProp", TypeScriptHelper.NumericTypeName }
			};
			s_packageTester.TestControllerWithName("SimpleController")
				.TestActionWithName("SimpleAnonymousType")
				.ReturnTypeTypescriptNameIs(TypeScriptHelper.BuildAnonymousType(expected));

		}

		[TestMethod]
		public void AnonymousTypes_WithExtractedObject()
		{
			Dictionary<string, string> expected = new Dictionary<string, string>
			{
				{ "testClassProp", $"{ReferenceTypeTester.TestNamespace}.TestClass" },
				{ "intProp", TypeScriptHelper.NumericTypeName }
			};
			s_packageTester.TestControllerWithName("SimpleController")
				.TestActionWithName("AnonymousTypeWithExtracted")
				.ReturnTypeTypescriptNameIs(TypeScriptHelper.BuildAnonymousType(expected));

		}

		[TestMethod]
		public void AnonymousTypes_WithNonExtractedObject()
		{
			Dictionary<string, string> expected = new Dictionary<string, string>
			{
				{ "testClassProp", "any" },
				{ "intProp", TypeScriptHelper.NumericTypeName }
			};
			s_packageTester.TestControllerWithName("SimpleController")
				.TestActionWithName("AnonymousTypeWithNonExtracted")
				.ReturnTypeTypescriptNameIs(TypeScriptHelper.BuildAnonymousType(expected));

		}
	}
}
