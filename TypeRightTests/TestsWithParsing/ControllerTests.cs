using TypeRight.ScriptWriting.TypeScript;
using TypeRight.Workspaces.Parsing;
using TypeRightTests.TestBuilders;
using TypeRightTests.Testers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.TypeProcessing;
using TypeRight.TypeLocation;

namespace TypeRightTests.TestsWithParsing
{
	[TestClass]
	public class ControllerTests
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
				.AddFakeMvc()
				
				// Test class to use as return/param
				.CreateClassBuilder("TestClass")
					.AddScriptObjectAttribute()
					.AddProperty("DontCare", "int")
					.Commit()

				// Test generic class to use as return/param
				.CreateClassBuilder("TestGenericClass")
					.AddScriptObjectAttribute()
					.AddGenericParameter("T")
					.AddProperty("GenericProp", "T")
					.Commit()

				// Not Extracted
				.CreateClassBuilder("NotExtracted")
					.AddProperty("DontCare", "int")
					.Commit()

				// Display name attribute
				.CreateClassBuilder("SimpleController")
					.WithControllerBaseClass()

					// Fake Json Method - not extracted
					.AddMethod("Json", "FakeJsonResultLikeClass")
						.AddParameter("data", "object")
						.AddLineOfCode("return null;", 0)
						.Commit()
					.AddMethod("ARandomMethod", "TestClass")  // Random Method
						.AddLineOfCode("return null;", 0)
						.Commit()


					.AddMethod("StringResult", "string")
						.AddScriptActionAttribute()
						.AddLineOfCode("return \"Hi\";", 0)
						.Commit()
					.AddMethod("NewObjectResult", "TestClass")
						.AddScriptActionAttribute()
						.AddLineOfCode("return new TestClass();", 0)
						.Commit()
					.AddMethod("NewObjectResult_Json", MvcConstants.JsonResult_AspNet)
						.AddScriptActionAttribute()
						.AddLineOfCode("return Json(new TestClass());", 0)
						.Commit()
					.AddMethod("VariableResult", "bool")
						.AddScriptActionAttribute()
						.AddLineOfCode("bool testVar = true;", 0)
						.AddLineOfCode("return testVar;", 0)
						.Commit()
					.AddMethod("VariableResult_Json", MvcConstants.JsonResult_AspNet)
						.AddScriptActionAttribute()
						.AddLineOfCode("bool testVar = true;", 0)
						.AddLineOfCode("return Json(testVar);", 0)
						.Commit()
					.AddMethod("StringConcatResult", "string")
						.AddScriptActionAttribute()
						.AddLineOfCode("return \"Hello\" + \"World\";", 0)
						.Commit()
					.AddMethod("MethodResult", "TestClass")
						.AddScriptActionAttribute()
						.AddLineOfCode("return ARandomMethod();", 0)
						.Commit()
					.AddMethod("SimpleParameter_Json", MvcConstants.JsonResult_AspNet)
						.AddScriptActionAttribute()
						.AddParameter("testParam", "TestClass")
						.AddLineOfCode("return Json(testParam);", 0)
						.Commit()
					.AddMethod("NotExtracted_Json", MvcConstants.JsonResult_AspNet)
						.AddScriptActionAttribute()
						.AddLineOfCode("return Json(new NotExtracted());", 0)
						.Commit()
					.AddMethod("ComplexMethod", MvcConstants.JsonResult_AspNet)
						.AddScriptActionAttribute()
						.AddParameter("dateStringDict", "Dictionary<DateTime, string>")
						.AddParameter("intStringDict", "Dictionary<int, string>")
						.AddParameter("stringStringDict", "Dictionary<string, string>")
						.AddLineOfCode("TestGenericClass<TestClass> gen = new TestGenericClass<TestClass>();", 0)
						.AddLineOfCode("return Json(gen);", 0)
						.Commit()
					.AddMethod("GenericPropReturn_Json", MvcConstants.JsonResult_AspNet)
						.AddScriptActionAttribute()
						.AddLineOfCode("TestGenericClass<TestClass> gen = new TestGenericClass<TestClass>();", 0)
						.AddLineOfCode("return Json(gen.GenericProp);", 0)
						.Commit()
					.Commit()
				
				.CreateClassBuilder("WebApiController")
					.WithControllerBaseClass()
					.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore)
						.AddConstructorArg("\"api/[controller]\"")
						.Commit()
					.AddMethod("GetStringList", "List<string>")
						.AddScriptActionAttribute()
						.AddLineOfCode("return new List<string>();", 0)
						.Commit()
					.AddMethod("GetStringActionResult", MvcConstants.ActionResult_AspNetCore + "<string>")
						.AddScriptActionAttribute()
						.AddLineOfCode("return null;", 0)
						.Commit()
					.Commit()

				.CreateClassBuilder("AspNetWebApiController")
					.WithControllerBaseClass()
					.AddAttribute(MvcConstants.RouteAttributeFullName_AspNet)
						.AddConstructorArg("\"api/asp/[controller]\"")
						.Commit()
					.AddMethod("WhoCares", MvcConstants.JsonResult_AspNet)
						.AddScriptActionAttribute()
						.AddLineOfCode("return Json(true);", 0)
						.Commit()
					.Commit()
			;
			
			_packageTester = wkspBuilder.GetPackageTester();
		}
		
		[TestMethod]
		public void Controllers_StringResult()
		{
			_packageTester.TestControllerWithName("SimpleController")
				.TestActionWithName("StringResult")
				.ReturnTypeTypescriptNameIs("string");
			
		}

		[TestMethod]
		public void Controllers_NewObjectResult()
		{
			_packageTester.TestControllerWithName("SimpleController")
				.TestActionWithName("NewObjectResult")
				.ReturnTypeTypescriptNameIs($"{ReferenceTypeTester.TestNamespace}.TestClass");
		}

		[TestMethod]
		public void Controllers_NewObjectResult_Json()
		{
			_packageTester.TestControllerWithName("SimpleController")
				.TestActionWithName("NewObjectResult_Json")
				.ReturnTypeTypescriptNameIs($"{ReferenceTypeTester.TestNamespace}.TestClass");
		}

		[TestMethod]
		public void Controllers_VariableResult()
		{
			//VariableResult
			_packageTester.TestControllerWithName("SimpleController")
				.TestActionWithName("VariableResult")
				.ReturnTypeTypescriptNameIs(TypeScriptHelper.BooleanTypeName);
		}

		[TestMethod]
		public void Controllers_VariableResult_Json()
		{
			//VariableResult
			_packageTester.TestControllerWithName("SimpleController")
				.TestActionWithName("VariableResult_Json")
				.ReturnTypeTypescriptNameIs(TypeScriptHelper.BooleanTypeName);
		}

		[TestMethod]
		public void Controllers_StringConcatResult()
		{
			_packageTester.TestControllerWithName("SimpleController")
				.TestActionWithName("StringConcatResult")
				.ReturnTypeTypescriptNameIs(TypeScriptHelper.StringTypeName);
		}

		[TestMethod]
		public void Controllers_MethodResult()
		{
			_packageTester.TestControllerWithName("SimpleController")
				.TestActionWithName("MethodResult")
				.ReturnTypeTypescriptNameIs($"{ReferenceTypeTester.TestNamespace}.TestClass");
		}

		[TestMethod]
		public void Controllers_SimpleParameter_Json()
		{
			_packageTester.TestControllerWithName("SimpleController")
				.TestActionWithName("SimpleParameter_Json")
				.ParameterTypeIs("testParam", $"{ReferenceTypeTester.TestNamespace}.TestClass");

			_packageTester.TestControllerWithName("SimpleController")
				.TestActionWithName("SimpleParameter_Json")
				.ReturnTypeTypescriptNameIs($"{ReferenceTypeTester.TestNamespace}.TestClass");
		}

		[TestMethod]
		public void Controllers_NotExracted_Json()
		{
			_packageTester.TestControllerWithName("SimpleController")
				.TestActionWithName("NotExtracted_Json")
				.ReturnTypeTypescriptNameIs(TypeScriptHelper.AnyTypeName);
		}

		[TestMethod]
		public void Controllers_DictionaryParams()
		{
			//DictionaryParams
			_packageTester.TestControllerWithName("SimpleController")
				.TestActionWithName("ComplexMethod")
				.ParameterTypeIs("dateStringDict", TypeScriptHelper.FormatDictionaryType(TypeScriptHelper.StringTypeName, TypeScriptHelper.StringTypeName))
				.ParameterTypeIs("intStringDict", TypeScriptHelper.FormatDictionaryType(TypeScriptHelper.NumericTypeName, TypeScriptHelper.StringTypeName))
				.ParameterTypeIs("stringStringDict", TypeScriptHelper.FormatDictionaryType(TypeScriptHelper.StringTypeName, TypeScriptHelper.StringTypeName))
				;
		}

		[TestMethod]
		public void Controllers_GenericReturnType()
		{
			//DictionaryParams
			_packageTester.TestControllerWithName("SimpleController")
				.TestActionWithName("ComplexMethod")
				.ReturnTypeTypescriptNameIs($"{ReferenceTypeTester.TestNamespace}.TestGenericClass<{ReferenceTypeTester.TestNamespace}.TestClass>");
		}

		[TestMethod]
		public void Controllers_GenericPropReturnType()
		{
			//DictionaryParams
			_packageTester.TestControllerWithName("SimpleController")
				.TestActionWithName("GenericPropReturn_Json")
				.ReturnTypeTypescriptNameIs($"{ReferenceTypeTester.TestNamespace}.TestClass");
		}

		[TestMethod]
		public void RouteAttribute_UsedInBaseURL()
		{
			// Asp net core
			_packageTester.TestControllerWithName("WebApiController")
				.BaseUrlIs("/api/WebApi/");

			// asp.net
			_packageTester.TestControllerWithName("AspNetWebApiController")
				.BaseUrlIs("/api/asp/AspNetWebApi/");
		}

		[TestMethod]
		public void WebApi_ReturnObjectIsFound()
		{
			_packageTester.TestControllerWithName("WebApiController")
				.TestActionWithName("GetStringList")
				.ReturnTypeTypescriptNameIs("string[]");
		}

		[TestMethod]
		public void WebApi_ActionResultUsesTypeParam()
		{
			_packageTester.TestControllerWithName("WebApiController")
				.TestActionWithName("GetStringActionResult")
				.ReturnTypeTypescriptNameIs("string");
		}
	}
}
