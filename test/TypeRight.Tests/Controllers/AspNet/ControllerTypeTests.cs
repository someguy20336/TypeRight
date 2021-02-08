using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.ScriptWriting.TypeScript;
using TypeRight.Tests.Testers;

namespace TypeRight.Tests.Controllers.AspNet
{
	[TestClass]
	public class ControllerTypeTests : ControllerTestsBase
	{

		protected override bool IsAspNetCore => false;

		[TestInitialize]
		public override void TestInitialize()
		{
			base.TestInitialize();
		}


		[TestMethod]
		public void Controllers_StringResult()
		{
			AddControllerAction("StringResult", "string")
				.AddLineOfCode("return \"Hi\";", 0)
				.Commit();

			AssertThatThisControllerAction("StringResult")
				.ReturnTypeTypescriptNameIs("string");

		}


		[TestMethod]
		public void Controllers_NewObjectResult()
		{
			AddControllerAction("NewObjectResult", "TestClass")
				.AddLineOfCode("return new TestClass();", 0)
				.Commit();

			AssertThatThisControllerAction("NewObjectResult")
				.ReturnTypeTypescriptNameIs($"{FakeTypePrefixer.Prefix}.TestClass");
		}

		[TestMethod]
		public void Controllers_NewObjectResult_Json()
		{
			AddControllerAction("NewObjectResult_Json", MvcConstants.JsonResult_AspNet)
				.AddLineOfCode("return Json(new TestClass());", 0)
				.Commit();

			AssertThatThisControllerAction("NewObjectResult_Json")
				.ReturnTypeTypescriptNameIs($"{FakeTypePrefixer.Prefix}.TestClass");

		}

		[TestMethod]
		public void Controllers_VariableResult()
		{
			AddControllerAction("VariableResult", "bool")
				.AddLineOfCode("bool testVar = true;", 0)
				.AddLineOfCode("return testVar;", 0)
				.Commit();

			AssertThatThisControllerAction("VariableResult")
				.ReturnTypeTypescriptNameIs(TypeScriptHelper.BooleanTypeName);
		}

		[TestMethod]
		public void Controllers_VariableResult_Json()
		{
			AddControllerAction("VariableResult_Json", MvcConstants.JsonResult_AspNet)
				.AddLineOfCode("bool testVar = true;", 0)
				.AddLineOfCode("return Json(testVar);", 0)
				.Commit();

			//VariableResult
			AssertThatThisControllerAction("VariableResult_Json")
				.ReturnTypeTypescriptNameIs(TypeScriptHelper.BooleanTypeName);
		}

		[TestMethod]
		public void Controllers_StringConcatResult()
		{
			AddControllerAction("StringConcatResult", "string")
				.AddLineOfCode("return \"Hello\" + \"World\";", 0)
				.Commit();

			AssertThatThisControllerAction("StringConcatResult")
				.ReturnTypeTypescriptNameIs(TypeScriptHelper.StringTypeName);
		}

		[TestMethod]
		public void Controllers_MethodResult()
		{
			ControllerBuilder
				.AddMethod("ARandomMethod", "TestClass")  
				.AddLineOfCode("return null;", 0)
				.Commit();

			AddControllerAction("MethodResult", "TestClass")
				.AddLineOfCode("return ARandomMethod();", 0)
				.Commit();

			AssertThatThisControllerAction("MethodResult")
				.ReturnTypeTypescriptNameIs($"{FakeTypePrefixer.Prefix}.TestClass");
		}

		[TestMethod]
		public void Controllers_SimpleParameter_ParameterType_Json()
		{
			AddControllerAction("SimpleParameter_Json", MvcConstants.JsonResult_AspNet)
				.AddParameter("testParam", "TestClass")
				.AddLineOfCode("return Json(testParam);", 0)
				.Commit();

			AssertThatThisControllerAction("SimpleParameter_Json")
				.ParameterTypeIs("testParam", $"{FakeTypePrefixer.Prefix}.TestClass");

		}

		[TestMethod]
		public void Controllers_SimpleParameter_ReturnType_Json()
		{
			AddControllerAction("SimpleParameter_Json", MvcConstants.JsonResult_AspNet)
				.AddParameter("testParam", "TestClass")
				.AddLineOfCode("return Json(testParam);", 0)
				.Commit();

			AssertThatThisControllerAction("SimpleParameter_Json")
				.ReturnTypeTypescriptNameIs($"{FakeTypePrefixer.Prefix}.TestClass");
		}

		[TestMethod]
		public void Controllers_NotExtracted_Json()
		{
			AddControllerAction("NotExtracted_Json", MvcConstants.JsonResult_AspNet)
				.AddLineOfCode("return Json(new NotExtracted());", 0)
				.Commit();

			AssertThatThisControllerAction("NotExtracted_Json")
				.ReturnTypeTypescriptNameIs(TypeScriptHelper.AnyTypeName);
		}

		[TestMethod]
		public void Controllers_DictionaryParams()
		{
			AddControllerAction("ComplexMethod", MvcConstants.JsonResult_AspNet)
				.AddParameter("dateStringDict", "Dictionary<DateTime, string>")
				.AddParameter("intStringDict", "Dictionary<int, string>")
				.AddParameter("stringStringDict", "Dictionary<string, string>")
				.AddLineOfCode("TestGenericClass<TestClass> gen = new TestGenericClass<TestClass>();", 0)
				.AddLineOfCode("return Json(gen);", 0)
				.Commit();

			//DictionaryParams
			AssertThatThisControllerAction("ComplexMethod")
				.ParameterTypeIs("dateStringDict", TypeScriptHelper.FormatDictionaryType(TypeScriptHelper.StringTypeName, TypeScriptHelper.StringTypeName))
				.ParameterTypeIs("intStringDict", TypeScriptHelper.FormatDictionaryType(TypeScriptHelper.NumericTypeName, TypeScriptHelper.StringTypeName))
				.ParameterTypeIs("stringStringDict", TypeScriptHelper.FormatDictionaryType(TypeScriptHelper.StringTypeName, TypeScriptHelper.StringTypeName))
				;
		}

		[TestMethod]
		public void Controllers_GenericReturnType()
		{
			AddControllerAction("ComplexMethod", MvcConstants.JsonResult_AspNet)
				.AddParameter("dateStringDict", "Dictionary<DateTime, string>")
				.AddParameter("intStringDict", "Dictionary<int, string>")
				.AddParameter("stringStringDict", "Dictionary<string, string>")
				.AddLineOfCode("TestGenericClass<TestClass> gen = new TestGenericClass<TestClass>();", 0)
				.AddLineOfCode("return Json(gen);", 0)
				.Commit();


			AssertThatThisControllerAction("ComplexMethod")
				.ReturnTypeTypescriptNameIs($"{FakeTypePrefixer.Prefix}.TestGenericClass<{FakeTypePrefixer.Prefix}.TestClass>");
		}

		[TestMethod]
		public void Controllers_GenericPropReturnType()
		{
			AddControllerAction("GenericPropReturn_Json", MvcConstants.JsonResult_AspNet)
				.AddLineOfCode("TestGenericClass<TestClass> gen = new TestGenericClass<TestClass>();", 0)
				.AddLineOfCode("return Json(gen.GenericProp);", 0)
				.Commit();

			AssertThatThisControllerAction("GenericPropReturn_Json")
				.ReturnTypeTypescriptNameIs($"{FakeTypePrefixer.Prefix}.TestClass");
		}
	}
}
